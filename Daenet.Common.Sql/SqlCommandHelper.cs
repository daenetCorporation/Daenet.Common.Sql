using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Daenet.Common.Sql
{
    public static class SqlCommandHelper
    {
        #region Public Fields

        public static int m_UniqueParamID = 0;

        #endregion

        #region Public Methods

        public static SqlParameter AddUniqueSqlParameter(this SqlCommand command, string parameter, object value)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameter + (m_UniqueParamID++).ToString();
            sqlParameter.Value = value;
            if (value == null)
            {
                sqlParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(sqlParameter);
            return sqlParameter;
        }

        public static SqlParameter AddSqlParameter(this SqlCommand command, string parameter, object value)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameter;
            sqlParameter.Value = value;
            if (value == null)
            {
                sqlParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(sqlParameter);
            return sqlParameter;
        }

        public static SqlParameter AddSqlParameter(this SqlCommand command, string parameter, DbType dbType, object value)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameter;
            sqlParameter.Value = value;
            sqlParameter.DbType = dbType;
            if (value == null)
            {
                sqlParameter.Value = DBNull.Value;
            }
            command.Parameters.Add(sqlParameter);
            return sqlParameter;
        }

        /// <summary>
        /// Builds "COLUMN IN (Param1, Param2, ParamN)" clause and adds Paramaters to Sql parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="column"></param>
        /// <param name="paramPrefix"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string BuildInClause<T>(this SqlCommand cmd, string column, string paramPrefix, IEnumerable<T> parameters)
        {
            var paramNames = new List<string>();

            foreach (var item in parameters)
            {
                paramNames.Add(cmd.AddUniqueSqlParameter(paramPrefix, item).ParameterName);
            }

            string inClause = $"{column} IN ({string.Join(",", paramNames)}) ";

            return inClause;
        }
        #endregion
    }
}
