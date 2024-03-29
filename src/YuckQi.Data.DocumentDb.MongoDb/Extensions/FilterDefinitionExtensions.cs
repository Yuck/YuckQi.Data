﻿using System.Collections;
using MongoDB.Driver;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.DocumentDb.MongoDb.Extensions;

public static class FilterDefinitionExtensions
{
    public static FilterDefinition<TDocument>? ToFilterDefinition<TDocument>(this IEnumerable<FilterCriteria>? parameters)
    {
        if (parameters == null)
            return Builders<TDocument>.Filter.Empty;

        var builder = Builders<TDocument>.Filter;
        var result = new List<FilterDefinition<TDocument>>();

        foreach (var parameter in parameters)
        {
            if (parameter is { Operation: FilterOperation.In, Value: not IEnumerable })
                throw new ArgumentException($"{nameof(parameter.Value)} must be convertible to {nameof(IEnumerable)}.");

            var field = new StringFieldDefinition<TDocument, Object?>(parameter.FieldName);
            var set = (parameter.Value as IEnumerable)?.Cast<Object>();

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
                case FilterOperation.In:
                    result.Add(builder.In(field, set));
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
