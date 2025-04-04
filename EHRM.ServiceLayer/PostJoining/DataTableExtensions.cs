using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.PostJoining
{
    public static class DataTableExtensions
    {
        public static List<T> ConvertToList<T>(this DataTable dt) where T : new()
        {
            List<T> dataList = new List<T>();
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (DataRow row in dt.Rows)
            {
                T obj = new T();

                foreach (PropertyInfo prop in properties)
                {
                    if (dt.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                    {
                        prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                    }
                }
                dataList.Add(obj);
            }
            return dataList;
        }


        public static List<T> ConvertToListWIthJson<T>(this DataTable dt) where T : new()
        {
            List<T> dataList = new List<T>();
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (DataRow row in dt.Rows)
            {
                T obj = new T();

                foreach (PropertyInfo prop in properties)
                {
                    if (dt.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                    {
                        // Check if property is a class (to handle JSON data)
                        if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                        {
                            try
                            {
                                var json = row[prop.Name].ToString();
                                var deserializedObj = JsonSerializer.Deserialize(json, prop.PropertyType);
                                prop.SetValue(obj, deserializedObj);
                            }
                            catch (Exception)
                            {
                                prop.SetValue(obj, null); // Handle invalid JSON gracefully
                            }
                        }
                        else
                        {
                            prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                        }
                    }
                }
                dataList.Add(obj);
            }
            return dataList;
        }

        //public static T ConvertToSingle<T>(this DataTable dt) where T : new()
        //{
        //    if (dt.Rows.Count == 0)
        //        return default; // Return default value (null for reference types)

        //    PropertyInfo[] properties = typeof(T).GetProperties();
        //    T obj = new T();

        //    DataRow row = dt.Rows[0]; // Get the first row

        //    foreach (PropertyInfo prop in properties)
        //    {
        //        if (dt.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
        //        {
        //            prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
        //        }
        //    }
        //    return obj;
        //}

        public static T ConvertToSingle<T>(this DataTable dt) where T : new()
        {
            if (dt.Rows.Count == 0)
                return default; // Return default value (null for reference types)

            PropertyInfo[] properties = typeof(T).GetProperties();
            T obj = new T();

            DataRow row = dt.Rows[0]; // Get the first row

            foreach (PropertyInfo prop in properties)
            {
                if (dt.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                {
                    Type propType = prop.PropertyType;

                    // Check if the property is a nullable type
                    Type underlyingType = Nullable.GetUnderlyingType(propType) ?? propType;


                    // Convert the value and set it
                    object value = Convert.ChangeType(row[prop.Name], underlyingType);
                    prop.SetValue(obj, value);
                }
            }
            return obj;
        }




    }
}
