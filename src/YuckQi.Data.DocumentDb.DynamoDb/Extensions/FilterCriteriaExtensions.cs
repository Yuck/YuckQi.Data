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
            var comparison = parameter.Operation.ToQueryOperator();

            switch (parameter.Value)
            {
                case Boolean value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case Char value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case Byte value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case Decimal value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case Double value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case Int16 value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case Int32 value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case Int64 value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case SByte value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case Single value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case String value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case UInt16 value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case UInt32 value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
                case UInt64 value:
                    filter.AddCondition(attribute, comparison, value);
                    break;
            }
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
