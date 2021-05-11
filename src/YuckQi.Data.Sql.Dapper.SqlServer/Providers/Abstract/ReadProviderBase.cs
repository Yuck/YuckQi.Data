using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;
using YuckQi.Data.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.SqlServer.Providers.Abstract
{
    // 3. figure out which pieces can be shared by SqlServer and MySql and create project YuckQi.Data.Sql.Dapper
    public abstract class ReadProviderBase<TRecord> : DataProviderBase
    {
        #region Private Members

        private const String DefaultSchemaName = "dbo";
        private static readonly String DefaultTableName = typeof(TRecord).Name;
        private static readonly TableAttribute TableAttribute = (TableAttribute) typeof(TRecord).GetCustomAttribute(typeof(TableAttribute));

        #endregion


        #region Properties

        private static String SchemaName => TableAttribute?.Schema ?? DefaultSchemaName;
        private static String TableName => TableAttribute?.Name ?? DefaultTableName;

        #endregion


        #region Constructors

        protected ReadProviderBase(IUnitOfWork context) : base(context) { }

        #endregion


        #region Protected Methods

        protected String BuildSqlForCount(IReadOnlyCollection<IDataParameter> parameters)
        {
            var select = "select count(*)";
            var from = BuildFromSql();
            var where = BuildWhereSql(parameters);
            var sql = $"{CombineSql(select, from, where)};";

            return sql;
        }

        protected String BuildSqlForGet(IReadOnlyCollection<IDataParameter> parameters)
        {
            var columns = BuildColumnsSql();

            var select = $"select {columns}";
            var from = BuildFromSql();
            var where = BuildWhereSql(parameters);
            var sql = $"{CombineSql(select, from, where)};";

            return sql;
        }

        protected String BuildSqlForSearch(IReadOnlyCollection<IDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort)
        {
            var columns = BuildColumnsSql();
            var sorting = String.Join(", ", sort.Select(t => $"[{t.Expression}]{(t.Order == SortOrder.Descending ? " desc" : String.Empty)}"));

            var select = $"select {columns}";
            var from = BuildFromSql();
            var where = BuildWhereSql(parameters);
            var order = ! String.IsNullOrWhiteSpace(sorting) ? $"order by {sorting}" : String.Empty;
            var limit = page != null ? $"offset {(page.PageNumber - 1) * page.PageSize} rows fetch first {page.PageSize} rows only" : String.Empty;
            var sql = $"{CombineSql(select, from, where, order, limit)};";

            return sql;
        }

        #endregion


        #region Supporting Methods

        private static String BuildColumnsSql()
        {
            var properties = typeof(TRecord).GetProperties().Where(t => t.CustomAttributes.All(u => u.AttributeType != typeof(IgnoreSelectAttribute)));
            var columns = String.Join(", ", properties.Select(t =>
            {
                var attribute = t.CustomAttributes.SingleOrDefault(u => u.AttributeType == typeof(ColumnAttribute));
                var custom = attribute?.ConstructorArguments.FirstOrDefault().Value as String;
                var column = String.IsNullOrWhiteSpace(custom)
                                 ? $"[{t.Name}]"
                                 : $"[{custom}] [{t.Name}]";

                return column;
            }));

            return columns;
        }

        private static String BuildFromSql() => $"from [{SchemaName}].[{TableName}]";

        private static String BuildWhereSql(IEnumerable<IDataParameter> parameters)
        {
            var filter = String.Join(" and ", parameters.Select(t =>
            {
                var column = $"[{t.ParameterName}]";
                var value = t.Value;
                var comparison = value != null ? "=" : "is";
                var parameter = value != null ? $"@{t.ParameterName}" : "null";

                return $"({column} {comparison} {parameter})";
            }));
            var where = $"{(String.IsNullOrWhiteSpace(filter) ? "" : $"where {filter}")}";

            return where;
        }

        private static String CombineSql(params String[] fragments)
        {
            return String.Join(Environment.NewLine, fragments.Where(t => ! String.IsNullOrWhiteSpace(t)));
        }

        #endregion
    }
}