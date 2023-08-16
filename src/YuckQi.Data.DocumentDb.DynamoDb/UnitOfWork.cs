using Amazon.DynamoDBv2.DataModel;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.DocumentDb.DynamoDb;

public class UnitOfWork : IUnitOfWork<IDynamoDBContext>
{
    public IDynamoDBContext? Scope { get; private set; }

    public UnitOfWork(IDynamoDBContext context)
    {
        Scope = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Dispose()
    {
        if (Scope == null)
            return;

        Scope?.Dispose();

        Scope = null;
    }

    public void SaveChanges()
    {
        throw new NotImplementedException();
    }
}
