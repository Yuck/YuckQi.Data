using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Attributes;

namespace YuckQi.Data.DocumentDb.MongoDb.Extensions
{
    public static class DocumentModelExtensions
    {
        #region Constants

        private const String DefaultObjectIdPropertyName = "_id";

        #endregion


        #region Private Members

        private static readonly ConcurrentDictionary<Type, String> CollectionNameByType = new ConcurrentDictionary<Type, String>();
        private static readonly ConcurrentDictionary<Type, String> DatabaseNameByType = new ConcurrentDictionary<Type, String>();
        private static readonly ConcurrentDictionary<Type, PropertyInfo> KeyByType = new ConcurrentDictionary<Type, PropertyInfo>();

        #endregion


        #region Extension Methods

        public static String GetCollectionName(this Type type) => type != null ? CollectionNameByType.GetOrAdd(type, key => GetCollectionAttribute(key)?.Name ?? key.Name) : null;

        public static String GetDatabaseName(this Type type) => type != null ? DatabaseNameByType.GetOrAdd(type, key => GetDatabaseAttribute(key)?.Name) : null;

        public static TKey? GetKey<TDocument, TKey>(this TDocument document) where TKey : struct => document != null ? GetKeyPropertyInfo(typeof(TDocument))?.GetValue(document) as TKey? : null;

        public static StringFieldDefinition<TDocument, TKey?> GetKeyFieldDefinition<TDocument, TKey>(this Type type) where TKey : struct
        {
            if (type == null)
                return null;
            // This isn't great since it should be enforced at compile time
            if (type != typeof(TDocument))
                throw new ArgumentException($"Type of '{type.FullName}' must match '{typeof(TDocument)}'.");

            var propertyInfo = GetKeyPropertyInfo(typeof(TDocument));
            var field = new StringFieldDefinition<TDocument, TKey?>(propertyInfo?.Name);

            return field;
        }

        #endregion


        #region Supporting Methods

        private static CollectionAttribute GetCollectionAttribute(MemberInfo type) => type.GetCustomAttribute(typeof(CollectionAttribute)) as CollectionAttribute;

        private static DatabaseAttribute GetDatabaseAttribute(MemberInfo type) => type.GetCustomAttribute(typeof(DatabaseAttribute)) as DatabaseAttribute;

        private static PropertyInfo GetKeyPropertyInfo(Type type) => type != null ? KeyByType.GetOrAdd(type, KeyPropertyInfoValueFactory) : null;

        private static PropertyInfo KeyPropertyInfoValueFactory(Type type) => type.GetProperties()
                                                                                  .Select(t => t.GetCustomAttribute<BsonIdAttribute>() != null ? t : null)
                                                                                  .SingleOrDefault(t => t != null) ?? type.GetProperty(DefaultObjectIdPropertyName);

        #endregion
    }
}
