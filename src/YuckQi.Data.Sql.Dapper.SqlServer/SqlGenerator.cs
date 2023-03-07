using System.Collections;
using System.Reflection;
using Dapper;
using YuckQi.Data.Filtering;
using YuckQi.Data.Sorting;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.SqlServer;

public class SqlGenerator<TRecord> : ISqlGenerator<TRecord>
{
    #region Private Members

    private const String DefaultSchemaName = "dbo";
    private static readonly String DefaultTableName = typeof(TRecord).Name;
    private static readonly TableAttribute? TableAttribute = typeof(TRecord).GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;

    #endregion


    #region Properties

    private static String SchemaName => TableAttribute?.Schema ?? DefaultSchemaName;
    private static String TableName => TableAttribute?.Name ?? DefaultTableName;

    #endregion


    #region Public Methods

    public String GenerateCountQuery(IReadOnlyCollection<FilterCriteria> parameters)
    {
        var select = "select count(*)";
        var from = BuildFromSql();
        var where = BuildWhereSql(parameters);
        var sql = $"{CombineSql(select, from, where)};";

        return sql;
    }

    public String GenerateGetQuery(IReadOnlyCollection<FilterCriteria>? parameters)
    {
        var columns = BuildColumnsSql();

        var select = $"select {columns}";
        var from = BuildFromSql();
        var where = BuildWhereSql(parameters);
        var sql = $"{CombineSql(select, from, where)};";

        return sql;
    }

    public String GenerateSearchQuery(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort)
    {
        var columns = BuildColumnsSql();
        var sorting = String.Join(", ", sort.Select(t => $"[{t.Expression}]{(t.Order == SortOrder.Descending ? " desc" : String.Empty)}"));

        var select = $"select {columns}";
        var from = BuildFromSql();
        var where = BuildWhereSql(parameters);
        var order = ! String.IsNullOrWhiteSpace(sorting) ? $"order by {sorting}" : String.Empty;
        var limit = $"offset {(page.PageNumber - 1) * page.PageSize} rows fetch first {page.PageSize} rows only";
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

    private static String BuildComparison(Object? value, FilterOperation operation)
    {
        return operation switch
        {
            FilterOperation.Equal => value != null ? "=" : "is",
            FilterOperation.GreaterThan => ">",
            FilterOperation.GreaterThanOrEqual => ">=",
            FilterOperation.In => "in",
            FilterOperation.LessThan => "<",
            FilterOperation.LessThanOrEqual => "<=",
            FilterOperation.NotEqual => value != null ? "!=" : "is not",
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }

    private static String BuildFromSql() => $"from [{SchemaName}].[{TableName}]";

    private static String BuildWhereSql(IEnumerable<FilterCriteria>? parameters)
    {
        var filter = String.Join(" and ", parameters?.Select(t =>
        {
            if (t is { Operation: FilterOperation.In, Value: not IEnumerable })
                throw new ArgumentException($"{nameof(t.Value)} must be convertible to {nameof(IEnumerable)}.");

            var column = $"[{t.FieldName}]";
            var value = t.Value;
            var comparison = BuildComparison(value, t.Operation);
            var set = t.Value is IEnumerable enumerable
                          ? enumerable.Cast<Object>().ToArray().Select((_, i) => $"@{t.FieldName}{i}").ToList()
                          : null;
            var parameter = t.Operation == FilterOperation.In
                                ? set != null && set.Any()
                                      ? $"({String.Join(",", set)})"
                                      : "(null)"
                                : value != null
                                    ? $"@{t.FieldName}"
                                    : "null";

            return $"({column} {comparison} {parameter})";
        }) ?? Array.Empty<String>());
        var where = $"{(String.IsNullOrWhiteSpace(filter) ? String.Empty : $"where {filter}")}";

        return where;
    }

    private static String CombineSql(params String[] fragments)
    {
        return String.Join(Environment.NewLine, fragments.Where(t => ! String.IsNullOrWhiteSpace(t)));
    }

    #endregion
}
