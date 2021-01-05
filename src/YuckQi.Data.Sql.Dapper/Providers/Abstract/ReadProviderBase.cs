using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;
using YuckQi.Data.Abstract;
using YuckQi.Data.Sorting.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers.Abstract
{
    public abstract class ReadProviderBase<TRecord> : DataProviderBase
    {
        #region Private Members

        private const string DefaultSchemaName = "dbo";
        private static readonly string DefaultTableName = typeof(TRecord).Name;
        private static readonly TableAttribute TableAttribute = (TableAttribute) typeof(TRecord).GetCustomAttribute(typeof(TableAttribute));

        #endregion


        #region Properties

        private static string SchemaName => TableAttribute?.Schema ?? DefaultSchemaName;
        private static string TableName => TableAttribute?.Name ?? DefaultTableName;

        #endregion


        #region Constructors

        protected ReadProviderBase(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion


        #region Protected Methods

        protected string BuildParameterizedSql(IReadOnlyCollection<IDataParameter> parameters, IPage page = null, IOrderedEnumerable<ISortExpression> sort = null)
        {
            var filter = string.Join(" and ", parameters.Select(t =>
            {
                var column = $"[{t.ParameterName}]";
                var value = t.Value;
                var comparison = value != null ? "=" : "is";
                var parameter = value != null ? $"@{t.ParameterName}" : "null";

                return $"({column} {comparison} {parameter})";
            }));
            var properties = typeof(TRecord).GetProperties().Where(t => t.CustomAttributes.All(u => u.AttributeType != typeof(IgnoreSelectAttribute)));
            var columns = string.Join(", ", properties.Select(t =>
            {
                var attribute = t.CustomAttributes.SingleOrDefault(u => u.AttributeType == typeof(ColumnAttribute));
                var custom = attribute?.ConstructorArguments.FirstOrDefault().Value as string;
                var column = string.IsNullOrWhiteSpace(custom)
                                 ? $"[{t.Name}]"
                                 : $"[{custom}] [{t.Name}]";

                return column;
            }));
            var top = page == null ? "top 2 " : string.Empty;
            var sorting = sort != null ? string.Join(", ", sort.Select(t => t.GetSortExpression())) : string.Empty;

            var select = $"select {top}{columns}";
            var from = $"from [{SchemaName}].[{TableName}]";
            var where = $"{(string.IsNullOrWhiteSpace(filter) ? "" : $"where {filter}")};";
            var order = !string.IsNullOrWhiteSpace(sorting) ? $"order by {sorting}" : string.Empty;
            var limit = page != null ? $"offset {(page.PageNumber - 1) * page.PageSize} rows fetch first {page.PageSize} only" : string.Empty;
            var sql = string.Join(" ", new[] { select, from, where, order, limit }.Where(t => !string.IsNullOrWhiteSpace(t)));

            return sql;
        }

        #endregion
    }
}