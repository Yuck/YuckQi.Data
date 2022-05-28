using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Attributes;

namespace YuckQi.Data.DocumentDb.MongoDb.Extensions;

public static class DocumentModelExtensions
{
    #region Constants

    private const String DefaultObjectIdPropertyName = "_id";

    #endregion


    #region Private Members

    private static readonly ConcurrentDictionary<Type, String> CollectionNameByType = new();
    private static readonly ConcurrentDictionary<Type, String> DatabaseNameByType = new();
    private static readonly ConcurrentDictionary<Type, PropertyInfo> IdentifierByType = new();

    #endregion


    #region Extension Methods

    public static String GetCollectionName(this Type type) => type != null ? CollectionNameByType.GetOrAdd(type, identifier => GetCollectionAttribute(identifier)?.Name ?? identifier.Name) : null;

    public static String GetDatabaseName(this Type type) => type != null ? DatabaseNameByType.GetOrAdd(type, identifier => GetDatabaseAttribute(identifier)?.Name) : null;

    public static TIdentifier? GetIdentifier<TDocument, TIdentifier>(this TDocument document) where TIdentifier : struct => document != null ? GetIdentifierPropertyInfo(typeof(TDocument))?.GetValue(document) as TIdentifier? : null;

    public static StringFieldDefinition<TDocument, TIdentifier?> GetIdentifierFieldDefinition<TDocument, TIdentifier>(this Type type) where TIdentifier : struct
    {
        if (type == null)
            return null;
        // This isn't great since it should be enforced at compile time
        if (type != typeof(TDocument))
            throw new ArgumentException($"Type of '{type.FullName}' must match '{typeof(TDocument)}'.");

        var propertyInfo = GetIdentifierPropertyInfo(typeof(TDocument));
        var field = new StringFieldDefinition<TDocument, TIdentifier?>(propertyInfo?.Name);

        return field;
    }

    #endregion


    #region Supporting Methods

    private static CollectionAttribute GetCollectionAttribute(MemberInfo type) => type.GetCustomAttribute(typeof(CollectionAttribute)) as CollectionAttribute;

    private static DatabaseAttribute GetDatabaseAttribute(MemberInfo type) => type.GetCustomAttribute(typeof(DatabaseAttribute)) as DatabaseAttribute;

    private static PropertyInfo GetIdentifierPropertyInfo(Type type) => type != null ? IdentifierByType.GetOrAdd(type, IdentifierPropertyInfoValueFactory) : null;

    private static PropertyInfo IdentifierPropertyInfoValueFactory(Type type) => type.GetProperties()
                                                                                     .Select(t => t.GetCustomAttribute<BsonIdAttribute>() != null ? t : null)
                                                                                     .SingleOrDefault(t => t != null) ?? type.GetProperty(DefaultObjectIdPropertyName);

    #endregion
}
