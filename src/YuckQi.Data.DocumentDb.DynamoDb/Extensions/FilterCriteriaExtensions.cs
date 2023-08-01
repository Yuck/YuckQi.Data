using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.DocumentDb.DynamoDb.Extensions;

public static class FilterCriteriaExtensions
{
    public static QueryFilter ToQueryFilter(this IReadOnlyCollection<FilterCriteria> parameters)
    {
        var filter = new QueryFilter();

        foreach (var parameter in parameters)
        {
            var attribute = parameter.FieldName;
            var comparison = parameter.Operation.ToComparisonOperator();
            var value = new AttributeValue(parameter.Value?.ToString());
            var condition = new Condition
            {
                AttributeValueList = new List<AttributeValue> { value },
                ComparisonOperator = comparison
            };

            filter.AddCondition(attribute, condition);
        }

        return filter;
    }

    public static ScanFilter ToScanFilter(this IReadOnlyCollection<FilterCriteria> parameters)
    {
        var filter = new ScanFilter();

        foreach (var parameter in parameters)
        {
            var attribute = parameter.FieldName;
            var comparison = parameter.Operation.ToComparisonOperator();
            var value = new AttributeValue(parameter.Value?.ToString());
            var condition = new Condition
            {
                AttributeValueList = new List<AttributeValue> { value },
                ComparisonOperator = comparison
            };

            filter.AddCondition(attribute, condition);
        }

        return filter;
    }
}
