using System;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Attributes;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers.Abstract
{
    public abstract class MongoProviderBase<TKey, TRecord> where TKey : struct
    {
        #region Constants

        private const String DefaultObjectIdPropertyName = "_id";

        #endregion


        #region Private Members

        private static readonly CollectionAttribute CollectionAttribute = (CollectionAttribute) typeof(TRecord).GetCustomAttribute(typeof(CollectionAttribute));
        private static readonly String DefaultCollectionName = typeof(TRecord).Name;
        private static readonly DatabaseAttribute DatabaseAttribute = (DatabaseAttribute) typeof(TRecord).GetCustomAttribute(typeof(DatabaseAttribute));
        private static readonly PropertyInfo KeyPropertyInfo = GetKeyPropertyInfo(typeof(TRecord));

        #endregion


        #region Properties

        protected static StringFieldDefinition<TRecord, TKey?> KeyFieldDefinition { get; } = GetKeyFieldDefinition();

        protected static String CollectionName => CollectionAttribute?.Name ?? DefaultCollectionName;
        protected static String DatabaseName => DatabaseAttribute.Name;

        #endregion


        #region Protected Methods

        protected TKey? GetKey(TRecord record) => KeyPropertyInfo?.GetValue(record) as TKey?;

        #endregion


        #region Supporting Methods

        private static StringFieldDefinition<TRecord, TKey?> GetKeyFieldDefinition() => new StringFieldDefinition<TRecord, TKey?>(KeyPropertyInfo?.Name);

        private static PropertyInfo GetKeyPropertyInfo(Type type) => type.GetProperty(DefaultObjectIdPropertyName) ?? type.GetProperties()
                                                                                                                          .Select(t => t.GetCustomAttribute<BsonIdAttribute>() != null ? t : null)
                                                                                                                          .SingleOrDefault(t => t != null);

        #endregion
    }
}