using System.Text;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using ConsoleApp1;
using Mapster;
using YuckQi.Data.DocumentDb.DynamoDb.Handlers;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Options;
using YuckQi.Data.Sorting;
using YuckQi.Domain.ValueObjects;
using YuckQi.Extensions.Mapping.Mapster;

Console.WriteLine("Hello, World!");

var creationOptions = new CreationOptions<SampleEntityKey>(() => new SampleEntityKey { Id = Guid.NewGuid() }, PropertyHandling.Auto, PropertyHandling.Auto);
var configuration = TypeAdapterConfig.GlobalSettings
                                     .ForType<SampleEntity, SampleEntityRecord>().AfterMapping((source, target) =>
                                     {
                                         target.PartitionKey = $"{source.Identifier.Id:D}".ToUpperInvariant();
                                         target.SortKey = "DEFAULT";
                                     })
                                     .Config
                                     .ForType<SampleEntityRecord, SampleEntity>().AfterMapping((source, target) =>
                                     {
                                         target.Identifier = new SampleEntityKey
                                         {
                                             Id = Guid.Parse(source.PartitionKey)
                                         };
                                     })
                                     .Config;
var mapper = new DefaultMapper(configuration);
var creator = new CreationHandler<SampleEntity, SampleEntityKey, DynamoDBContext, SampleEntityRecord>(creationOptions, mapper);
var retriever = new RetrievalHandler<SampleEntity, SampleEntityKey, DynamoDBContext, SampleEntityRecord>(key => $"{key.Id:D}".ToUpperInvariant(), _ => "DEFAULT", mapper);
var revisionOptions = new RevisionOptions(PropertyHandling.Auto);
var reviser = new RevisionHandler<SampleEntity, SampleEntityKey, DynamoDBContext, SampleEntityRecord>(revisionOptions, mapper);
var activator = new ActivationHandler<SampleEntity, SampleEntityKey, DynamoDBContext>(reviser);
var logicalDeleter = new LogicalDeletionHandler<SampleEntity, SampleEntityKey, DynamoDBContext>(reviser);
var physicalDeleter = new PhysicalDeletionHandler<SampleEntity, SampleEntityKey, DynamoDBContext, SampleEntityRecord>(mapper);
var searcher = new SearchHandler<SampleEntity, SampleEntityKey, DynamoDBContext, SampleEntityRecord>(mapper);

var name = GetRandomString(25);
var entity = new SampleEntity { Name = name };
var credentials = new BasicAWSCredentials("AKIAZGCA7SNQIVCUWMER", "z/qV5HplDNq84BDySMrgDwJbxDlL9tgSdu5rrLQ7");
var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
var scope = new DynamoDBContext(client);

var created = await creator.Create(entity, scope, default);
var parameters = new List<FilterCriteria> { new(nameof(SampleEntityRecord.Name), FilterOperation.GreaterThan, created.Name.First()) };
var retrieved = await retriever.GetList(parameters, scope, default);
var revised = await reviser.Revise(retrieved.First(), scope, default);
var deactivated = await activator.Deactivate(revised, scope, default);
var activated = await activator.Activate(revised, scope, default);
var restored = await logicalDeleter.Restore(activated, scope, default);
var deleted = await logicalDeleter.Delete(deactivated, scope, default);
var removed = await physicalDeleter.Delete(restored, scope, default);
var page = new Page(2, 3);
var sort = new List<SortCriteria> { new("Name", SortOrder.Descending) }.OrderBy(t => t);
var results = await searcher.Search(parameters, page, sort, scope, default);

var x = DateTime.UtcNow;

static String GetRandomString(Int32 length)
{
    var builder = new StringBuilder(length);
    for (var i = 0; i < length; i++)
        builder.Append((Char) new Random().Next('A', 'Z'));

    return builder.ToString();
}
