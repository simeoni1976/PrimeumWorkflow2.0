using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Workflow.BusinessCore.ServiceLayer.Helpers
{
    /// <summary>
    /// DictionaryConverter class.
    /// </summary>
    public static class DictionaryConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ObjectToDictionary<TKey, TValue>(object obj)
        {
            var stringDictionary = obj as Dictionary<TKey, TValue>;

            if (stringDictionary != null)
            {
                return stringDictionary;
            }
            var baseDictionary = obj as IDictionary;

            if (baseDictionary != null)
            {
                var dictionary = new Dictionary<TKey, TValue>();
                foreach (DictionaryEntry keyValue in baseDictionary)
                {
                    if (!(keyValue.Value is TValue))
                    {
                        // value is not TKey. perhaps throw an exception
                        return null;
                    }
                    if (!(keyValue.Key is TKey))
                    {
                        // value is not TValue. perhaps throw an exception
                        return null;
                    }

                    dictionary.Add((TKey)keyValue.Key, (TValue)keyValue.Value);
                }
                return dictionary;
            }
            // object is not a dictionary. perhaps throw an exception
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static T DictionaryToObject<T>(Dictionary<string, string> dict) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase))) // InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, string> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase));///InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property is...
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;
                
                // Fix nullables...
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // ...and change the type
                object newA = Convert.ChangeType(item.Value, newT);
                t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
            }
            return t;
        }

    }
}
