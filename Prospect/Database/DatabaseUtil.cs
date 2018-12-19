using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public class DatabaseUtil
    {
        public static object GetDotNetTypeValue(Type type, object value)
        {
            if (type.Equals(typeof(int?)))
            {
                return ConvertToDotNetTypeValue<int?>(value);
            }
            else if (type.Equals(typeof(string)))
            {
                return ConvertToDotNetTypeValue<string>(value);
            }
            else if (type.Equals(typeof(DateTime?)))
            {
                return ConvertToDotNetTypeValue<DateTime?>(value);
            }
            else if (type.Equals(typeof(bool?)))
            {
                return ConvertToDotNetTypeValue<bool?>(value);
            }
            else if (type.Equals(typeof(Decimal?)))
            {
                return ConvertToDotNetTypeValue<Decimal?>(value);
            }
            return value;
        }

        public static T ConvertToDotNetTypeValue<T>(object value)
        {
            T obj = default(T);
            if (value != DBNull.Value)
            {
                obj = (T)value;
            }
            return obj;
        }

        public static string GetSqlTypeValue(Type type, object value)
        {
            if (type.Equals(typeof(int?)))
            {
                return value.ToString();
            }
            else if (type.Equals(typeof(string)))
            {
                return "'" + value.ToString().Replace("'", "''") + "'";
            }
            else if (type.Equals(typeof(DateTime?)))
            {
                return "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            else if (type.Equals(typeof(bool?)))
            {
                return ((bool?)value) == true ? "1" : "0";
            }
            return value.ToString();
        }
    }
}
