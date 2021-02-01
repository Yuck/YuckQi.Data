using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;
using YuckQi.Data.Abstract;
using YuckQi.Data.Sorting;
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

        protected string BuildSqlForCount(IReadOnlyCollection<IDataParameter> parameters)
        {
            var select = "select count(*)";
            var from = BuildFromSql();
            var where = BuildWhereSql(parameters);
            var sql = $"{CombineSql(select, from, where)};";

            return sql;
        }

        protected string BuildSqlForGet(IReadOnlyCollection<IDataParameter> parameters)
        {
            var columns = BuildColumnsSql();

            var select = $"select {columns}";
            var from = BuildFromSql();
            var where = BuildWhereSql(parameters);
            var sql = $"{CombineSql(select, from, where)};";

            return sql;
        }

        protected string BuildSqlForSearch(IReadOnlyCollection<IDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort)
        {
            var columns = BuildColumnsSql();
            var sorting = string.Join(", ", sort.Select(t => $"[{t.Expression}] {(t.Order == SortOrder.Descending ? "desc" : "asc")}"));

            var select = $"select {columns}";
            var from = BuildFromSql();
            var where = BuildWhereSql(parameters);
            var order = ! string.IsNullOrWhiteSpace(sorting) ? $"order by {sorting}" : string.Empty;
            var limit = page != null ? $"offset {(page.PageNumber - 1) * page.PageSize} rows fetch first {page.PageSize} only" : string.Empty;
            var sql = $"{CombineSql(select, from, where, order, limit)};";

            return sql;
        }

        #endregion


        #region Supporting Methods

        private static string BuildColumnsSql()
        {
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

            return columns;
        }

        private static string BuildFromSql()
        {
            return $"from [{SchemaName}].[{TableName}]";
        }

        private static string BuildWhereSql(IEnumerable<IDataParameter> parameters)
        {
            var filter = string.Join(" and ", parameters.Select(t =>
            {
                var column = $"[{t.ParameterName}]";
                var value = t.Value;
                var comparison = value != null ? "=" : "is";
                var parameter = value != null ? $"@{t.ParameterName}" : "null";

                return $"({column} {comparison} {parameter})";
            }));
            var where = $"{(string.IsNullOrWhiteSpace(filter) ? "" : $"where {filter}")}";

            return where;
        }

        private static string CombineSql(params string[] fragments)
        {
            return string.Join(" ", fragments.Where(t => ! string.IsNullOrWhiteSpace(t)));
        }

        #endregion
    }
}