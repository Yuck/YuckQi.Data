using System.Collections.Concurrent;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Attributes;

namespace YuckQi.Data.DocumentDb.MongoDb.Extensions;

public static class DocumentModelExtensions
{
    private const String DefaultObjectIdPropertyName = "_id";

    private static readonly ConcurrentDictionary<Type, String> CollectionNameByType = new ();
    private static readonly ConcurrentDictionary<Type, String> DatabaseNameByType = new ();
    private static readonly ConcurrentDictionary<Type, PropertyInfo> IdentifierByType = new ();

    public static String? GetCollectionName(this Type? type) => type != null ? CollectionNameByType.GetOrAdd(type, identifier => GetCollectionAttribute(identifier)?.Name ?? identifier.Name) : null;

    public static String? GetDatabaseName(this Type? type) => type != null ? DatabaseNameByType.GetOrAdd(type, identifier => GetDatabaseAttribute(identifier).Name) : null;

    public static TIdentifier? GetIdentifier<TDocument, TIdentifier>(this TDocument document)
    {
        if (document == null)
            return default;

        var property = GetIdentifierPropertyInfo(typeof(TDocument)) ?? throw new InvalidOperationException($"{typeof(TDocument).Name} identifier property could not be determined.");
        var value = property.GetValue(document);
        if (value is TIdentifier identifier)
            return identifier;

        throw new InvalidOperationException($"{typeof(TDocument).Name} identifier type {property.PropertyType.Name} does not match declared type {typeof(TIdentifier).Name}.");
    }

    public static StringFieldDefinition<TDocument, TIdentifier?>? GetIdentifierFieldDefinition<TDocument, TIdentifier>(this Type? type)
    {
        if (type == null)
            return null;

        // This isn't great since it should be enforced at compile time
        if (type != typeof(TDocument))
            throw new ArgumentException($"Type of '{type.FullName}' must match '{typeof(TDocument).Name}'.");

        var propertyInfo = GetIdentifierPropertyInfo(typeof(TDocument));
        var field = new StringFieldDefinition<TDocument, TIdentifier?>(propertyInfo?.Name);

        return field;
    }

    private static CollectionAttribute? GetCollectionAttribute(MemberInfo type) => type.GetCustomAttribute(typeof(CollectionAttribute)) as CollectionAttribute;

    private static DatabaseAttribute GetDatabaseAttribute(MemberInfo type)
    {
        if (type.GetCustomAttribute(typeof(DatabaseAttribute)) is DatabaseAttribute attribute)
            return attribute;

        throw new NullReferenceException();
    }

    private static PropertyInfo? GetIdentifierPropertyInfo(Type? type) => type != null ? IdentifierByType.GetOrAdd(type, IdentifierPropertyInfoValueFactory) : null;

    private static PropertyInfo IdentifierPropertyInfoValueFactory(Type type)
    {
        var property = type.GetProperties()
                           .Select(t => t.GetCustomAttribute<BsonIdAttribute>() != null ? t : null)
                           .SingleOrDefault(t => t != null) ?? type.GetProperty(DefaultObjectIdPropertyName);
        if (property != null)
            return property;

        throw new NullReferenceException();
    }
}
