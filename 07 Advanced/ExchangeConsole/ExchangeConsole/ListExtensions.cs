using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;
using ReiserIntranet.TelephoneBook;

namespace ReiserIntranet
{
    public static class ListExtensions
    {
        public static string RemoveBlanks(this string obj)
        {
            return Regex.Replace(obj, @"\s+", ""); 
        }

        public static List<T> ToDistinctList<T>(this List<T> contacts)
        {
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            foreach (T obj in contacts)
            {
                string key = ((object)obj).ToJson();
                if (!dictionary.ContainsKey(key))
                    dictionary.Add(key, obj);
            }
            return dictionary.Select(f => f.Value).ToList<T>();
        }

        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                foreach (DataRow dataRow in table.AsEnumerable())
                {
                    T instance = Activator.CreateInstance<T>();
                    foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo property = instance.GetType().GetProperty(propertyInfo.Name);
                            property.SetValue(instance, Convert.ChangeType(dataRow[propertyInfo.Name], property.PropertyType), null);
                        }
                        catch
                        {
                        }
                    }
                    list.Add(instance);
                }
                return list;
            }
            catch
            {
                return (List<T>)null;
            }
        }
    }


}