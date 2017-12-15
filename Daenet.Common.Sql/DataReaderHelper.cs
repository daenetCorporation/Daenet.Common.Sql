using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Reflection;

namespace Daenet.Common.Sql
{
    public static class DataReaderHelper
    {
        #region Public Methods
        /// <summary>
        /// Gets a value of an data reader.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="reader"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static TResult GetValue<TResult>(this IDataReader reader, string column)
        {
            if (reader.IsDBNull(column) == true)
            {
                return default(TResult);
            }
            else
            {
                int pos = reader.GetOrdinal(column);

                if (typeof(TResult).GetTypeInfo().IsGenericType &&
                    typeof(TResult).GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var types = typeof(TResult).GetTypeInfo().IsGenericTypeDefinition ? typeof(TResult).GetTypeInfo().GenericTypeParameters: typeof(TResult).GetTypeInfo().GenericTypeArguments;

                    Type innerType = types.FirstOrDefault();
                    object obj = null;

                    if (innerType.GetTypeInfo().IsEnum)
                    {
                        obj = Enum.Parse(innerType, reader.GetValue(pos).ToString());
                    }
                    else
                    {
                        obj = Convert.ChangeType(reader.GetValue(pos), innerType, CultureInfo.InvariantCulture);
                    }
                    NullableConverter e = new NullableConverter(typeof(TResult));
                    return (TResult)e.ConvertFrom(obj);
                }
                else if (typeof(TResult).GetTypeInfo().IsEnum)
                {
                    return (TResult)Enum.Parse(typeof(TResult), reader.GetValue(pos).ToString());
                }
                //else
                return (TResult)Convert.ChangeType(reader.GetValue(pos), typeof(TResult), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets a value of an data reader.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="reader"></param>
        /// <param name="column"></param>
        /// <returns></columnNo>
        public static TResult GetValue<TResult>(this IDataReader reader, int columnNo)
        {
            if (reader.IsDBNull(columnNo))
                return default(TResult);
            else
            {
                if (typeof(TResult).GetTypeInfo().IsGenericType &&
                    typeof(TResult).GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    throw new NotSupportedException("The method GetValue<TResult>() of IDataReader/DataReaderHelper doesn't support nullable types.");
                    ////Type innerType = typeof(TResult).GetGenericArguments()[0];
                    //Type innerType = (typeof(TResult).GetTypeInfo().IsGenericTypeDefinition ? typeof(TResult).GetTypeInfo().GenericTypeParameters : typeof(TResult).GetTypeInfo().GenericTypeArguments)[0];
                    //object obj = Convert.ChangeType(reader.GetValue(columnNo), innerType, CultureInfo.InvariantCulture);
                    //NullableConverter e = new NullableConverter(typeof(TResult));
                    //return (TResult)e.ConvertFrom(obj);
                }
                //else
                return (TResult)Convert.ChangeType(reader.GetValue(columnNo), typeof(TResult), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets a value of an data reader.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="reader"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static DateTime? GetUtcDateTime(this IDataReader reader, string column)
        {
            int pos = reader.GetOrdinal(column);

            if (reader.IsDBNull(pos))
                return null;
            else
            {
                return new DateTime(((DateTime)reader.GetDateTime(pos)).Ticks, DateTimeKind.Utc);
            }
        }



        public static bool IsDBNull(this IDataReader reader, string column)
        {
            if (reader.HasColumn(column))
            {
                int pos = reader.GetOrdinal(column);
                return reader.IsDBNull(pos);
            }
            else
            {
                return true;
            }
        }

        public static bool HasColumn(this IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        #endregion

    }
}
