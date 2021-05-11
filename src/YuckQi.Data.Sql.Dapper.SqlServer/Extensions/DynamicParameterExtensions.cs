using System.Collections.Generic;
using System.Data;
using Dapper;

namespace YuckQi.Data.Sql.Dapper.SqlServer.Extensions
{
    public static class DynamicParameterExtensions
    {
        public static DynamicParameters ToDynamicParameters(this IEnumerable<IDataParameter> parameters)
        {
            if (parameters == null)
                return null;

            var result = new DynamicParameters();
            foreach (var parameter in parameters)
                result.Add(parameter.ParameterName, parameter.Value, parameter.DbType, parameter.Direction);

            return result;
        }
    }
}