using System;
using Amazon.DynamoDBv2;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.DocumentDb.DynamoDb.Extensions;

public static class FilterOperationExtensions
{
    public static ComparisonOperator ToComparisonOperator(this FilterOperation operation)
    {
        return operation switch
        {
            FilterOperation.Equal => ComparisonOperator.EQ,
            FilterOperation.GreaterThan => ComparisonOperator.GT,
            FilterOperation.GreaterThanOrEqual => ComparisonOperator.GE,
            FilterOperation.In => ComparisonOperator.IN,
            FilterOperation.LessThan => ComparisonOperator.LT,
            FilterOperation.LessThanOrEqual => ComparisonOperator.LE,
            FilterOperation.NotEqual => ComparisonOperator.NE,
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }
}
