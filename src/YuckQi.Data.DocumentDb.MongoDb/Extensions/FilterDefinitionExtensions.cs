using System;
using System.Collections.Generic;
using MongoDB.Driver;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.DocumentDb.MongoDb.Extensions;

public static class FilterDefinitionExtensions
{
    public static FilterDefinition<TDocument> ToFilterDefinition<TDocument>(this IEnumerable<FilterCriteria> parameters)
    {
        if (parameters == null)
            return null;

        var builder = Builders<TDocument>.Filter;
        var result = new List<FilterDefinition<TDocument>>();

        foreach (var parameter in parameters)
        {
            var field = new StringFieldDefinition<TDocument, Object>(parameter.FieldName);
            switch (parameter.Operation)
            {
                case FilterOperation.Equal:
                    result.Add(builder.Eq(field, parameter.Value));
                    break;
                case FilterOperation.GreaterThan:
                    result.Add(builder.Gt(field, parameter.Value));
                    break;
                case FilterOperation.GreaterThanOrEqual:
                    result.Add(builder.Gte(field, parameter.Value));
                    break;
                case FilterOperation.LessThan:
                    result.Add(builder.Lt(field, parameter.Value));
                    break;
                case FilterOperation.LessThanOrEqual:
                    result.Add(builder.Lte(field, parameter.Value));
                    break;
                case FilterOperation.NotEqual:
                    result.Add(builder.Not(builder.Eq(field, parameter.Value)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return builder.And(result);
    }
}
