using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common
{
    public static class DataReaderExtensions
    {
        public static T ParseReader<T>(IDataReader rdr) where T : class, IReadable
        {
            T newObj = Activator.CreateInstance(typeof(T)) as T;
            var result = (T)newObj.ParseFromReader(rdr);
            return result;
        }

        public static T ReadValue<T>(this IDataReader reader, string column)
        {
            if (reader != null && reader[column] != null && reader[column] != DBNull.Value)
            {
                try
                {
                    return (T)(object)reader[column];
                }
                catch { }
            }

            return default(T);
        }
    }
}
