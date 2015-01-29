using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GeneralModule.Utilily.Extension
{
    public static partial class Extension
    {

        #region Don't recommand use.
        /// <summary>
        /// if string is empty or null
        /// </summary>
        /// <param name="t">string</param>
        /// <param name="trueValue">return value when string is empoty or null</param>
        /// <returns>string</returns>
        public static string eIsEmptyOrNull(this string t, string trueValue = "")
        {
            if (t.eIsNullOrEmpty())
            {
                return trueValue;
            }
            return t;
        }

        /// <summary>
        /// String is empty/null or not
        /// </summary>
        /// <param name="t">string</param>
        /// <returns>is empty/null</returns>
        public static bool eIsNullOrEmpty(this string t)
        {
            if (String.IsNullOrWhiteSpace(t))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion



        public static string eIfEmpty(this string t, string whenTrue = "")
        {
            return t.eIsEmpty() ? whenTrue : t;
        }

        public static bool eIsEmpty(this string t)
        {
            return String.IsNullOrEmpty(t);
        }

        public static bool eIsSpace(this string t)
        {
            return String.IsNullOrWhiteSpace(t);
        }

        public static string eIfSpace(this string t, string whenTrue = "")
        {
            return t.eIsSpace() ? whenTrue : t;
        }


        public static string eToJson(this object t)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(t);
        }

        public static string eToJson(this DataTable table)
        {
            if (table.eIsNull()) return null;

            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    dict.Add(column.ColumnName, row[column]);
                }
                list.Add(dict);
            }
            return list.eToJson();
        }

        public static bool eIsNull(this object t)
        {
            return t == null || t == DBNull.Value;
        }

        public static bool eIsNotNull(this object t)
        {
            return !t.eIsNull();
        }


        public static string eIfNull(this object t, string whenTrue = "")
        {
            return t.eIsNull() ? whenTrue : t.ToString();
        }

        public static string eIfNull(this object t, string propertyName, string whenTrue)
        {
            if (t.eIsNull()) return whenTrue;
            var property = t.GetType().GetProperty(propertyName);
            if (!property.CanRead) return whenTrue;
            var value = property.GetValue(t, null);
            return value.eIfNull(whenTrue);
        }

        public static string eIfNull(this XAttribute attribute, string whenTrue)
        {
            return attribute.eIsNull() ? whenTrue : attribute.Value;
        }
        public static int eIfNull(this XAttribute attribute, int whenTrue)
        {
            string value = attribute.eIsNull() ? whenTrue.ToString() : attribute.Value;
            return value.eToInt(whenTrue);
        }

        public static bool eLastIs(this string t, string lastVal)
        {
            if (t.eIsEmpty() || lastVal.eIsEmpty()) return false;
            int index = t.LastIndexOf(lastVal);
            if (index == -1) return false;
            return t.Length == (lastVal.Length + index);

        }
        public static string eIfLastIs(this string t, string lastVal)
        {
            bool isLast = t.eLastIs(lastVal);
            if (isLast) return t;
            else return t + lastVal;
        }






        public static T eToEntity<T>(this string t)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(t);
        }
        public static string eStreamToString(this HttpRequestBase req)
        {
            StreamReader reader = new StreamReader(req.InputStream);
            string json = reader.ReadToEnd();
            reader.Close();
            return json;
        }
        public static string eStreamToString(this HttpRequest req)
        {
            StreamReader reader = new StreamReader(req.InputStream);
            string json = reader.ReadToEnd();
            reader.Close();
            return json;
        }
        public static T eToEntity<T>(this HttpRequestBase req)
        {
            string json = req.eStreamToString();
            return json.eToEntity<T>();
        }
        public static T eToEntity<T>(this HttpRequest req)
        {
            string json = req.eStreamToString();
            return json.eToEntity<T>();
        }



        public static Object eOverrideAttr(this object thisObj, object tempObj)
        {
            PropertyInfo[] properties = thisObj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string thisAttrName = property.Name;
                PropertyInfo[] tempProperties = tempObj.GetType().GetProperties();
                foreach (PropertyInfo tempProperty in tempProperties)
                {
                    string tempAttrName = tempProperty.Name;
                    if (thisAttrName.ToLower() == tempAttrName.ToLower() && property.PropertyType.Name == tempProperty.PropertyType.Name)
                    {
                        object tempVal = tempProperty.GetValue(tempObj, null);
                        try
                        {
                            if (property.CanWrite) property.SetValue(thisObj, tempVal, null);
                        }
                        catch { }
                        break;
                    }
                }
            }
            return thisObj;
        }

        public static Object eOverrideAttr(this object target, object attach, bool validPropertyType)
        {
            PropertyInfo[] targetProperties = target.GetType().GetProperties();
            foreach (PropertyInfo targetProperty in targetProperties)
            {
                string targetAttrName = targetProperty.Name;
                PropertyInfo[] attachProperties = attach.GetType().GetProperties();
                foreach (PropertyInfo attachProperty in attachProperties)
                {
                    string attachAttrName = attachProperty.Name;
                    if (validPropertyType)
                    {
                        if (targetAttrName.ToLower() == attachAttrName.ToLower() && targetProperty.PropertyType.Name == attachProperty.PropertyType.Name)
                        {
                            object attachVal = attachProperty.GetValue(attach, null);
                            try
                            {
                                if (targetProperty.CanWrite)
                                {
                                    targetProperty.SetValue(target, attachVal, null);
                                }
                            }
                            catch { }
                            break;
                        }
                    }
                    else
                    {

                        if (targetAttrName.ToLower() == attachAttrName.ToLower())
                        {
                            object attachVal = attachProperty.GetValue(attach, null);
                            try
                            {
                                if (targetProperty.CanWrite)
                                {
                                    targetProperty.SetValue(target, attachVal, null);
                                }
                            }
                            catch { }
                            break;
                        }

                    }
                }
            }
            return target;
        }

        public static string eReplaceWithObject(this string content, object entity, string prefix = "")
        {
            if (String.IsNullOrWhiteSpace(prefix))
            {
                return content.eReplaceWithObject(entity.GetType(), entity, prefix);
            }
            else
            {
                return content.eReplaceWithObject(entity.GetType(), entity, prefix);
            }
        }

        public static string eReplaceWithObject(this string content, Type t, object entity, string prefix = "")
        {
            try
            {
                PropertyInfo[] properties = t.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    string proName = property.Name;
                    object value = property.GetValue(entity, null);
                    if (value == null) continue;
                    string placeHold = String.Format("${{{0}.{1}}}", prefix, proName);
                    if (String.IsNullOrWhiteSpace(prefix)) placeHold = "${" + proName + "}";
                    content = Regex.Replace(content, "\\" + placeHold, value.ToString(), RegexOptions.IgnoreCase);
                }
                return content;
            }
            catch
            {
                return "";
            }
        }

        public static EntitySet<T> eToEntitySet<T>(this IEnumerable<T> source) where T : class
        {
            var set = new EntitySet<T>();
            set.AddRange(source);
            return set;
        }


        #region Date type convert


        public static T eParse<T>(this object obj)
        {
            T value;
            try
            {
                value = (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                value = default(T);
            }
            return value;
        }

        public static T eStringParse<T>(this string value)
        {
            var result = default(T);
            try
            {
                if (value.eIsSpace()) return result;
                var tc = TypeDescriptor.GetConverter(typeof (T));
                result = (T) tc.ConvertFrom(value);
            }
            catch { }
            return result;
        }


        public static DateTime eToDate(this object t, DateTime whenFail = new DateTime())
        {
            if (t.eIsNull()) return whenFail;
            DateTime.TryParse(t.ToString(), out whenFail);
            return whenFail;
        }

        public static int eToInt(this object current, int whenFail = 0)
        {
            if (current.eIsNull()) return whenFail;

            var str = current.ToString();
            if (str.eIsEmpty()) return whenFail;

            int intVal;
            return int.TryParse(str, out intVal) ? intVal : whenFail;
        }

        public static double eToDouble(this object current, double whenFail = 0d)
        {
            if (current.eIsNull()) return whenFail;

            var str = current.ToString();
            if (str.eIsEmpty()) return whenFail;

            double dbVal;
            return double.TryParse(str, out dbVal) ? dbVal : whenFail;
        }

        public static float eToFloat(this object current, float whenFail = 0)
        {
            if (current.eIsNull()) return whenFail;

            var str = current.ToString();
            if (str.eIsEmpty()) return whenFail;

            float floatVal;
            return float.TryParse(str, out floatVal) ? floatVal : whenFail;
        }

        #endregion


        #region Reflection
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static List<T> eToInstances<T>(this DataTable table)
        {
            var results = table.Rows.OfType<DataRow>().Select(row => row.eToInstance<T>()).ToList();
            return results;
        }

        public static T eToInstance<T>(this DataRow row)
        {
            var instance = Activator.CreateInstance<T>();
            instance.GetType()
                .GetProperties()
                .Where(p => p.CanWrite)
                .ForEach(property =>
                {
                    try
                    {
                        var columnValue = row[property.Name];
                        if (columnValue != null)
                        {
                            property.SetValue(instance, columnValue, null);
                        }
                    }
                    catch { }
                }
                );
            return instance;
        }


        public static void eSetValuesExclude(this object entity, object target, params string[] excludes)
        {
            entity.GetType().GetProperties().Where(prop => !excludes.Contains(prop.Name)).All(prop =>
            {
                prop.SetValue(entity, target.eGetValue(prop.Name), null);
                return true;
            });
        }

        public static void eSetValuesInclude(this object entity, object target, params string[] includes)
        {
            entity.GetType().GetProperties().Where(prop => includes.Contains(prop.Name)).All(prop =>
            {
                prop.SetValue(entity, target.eGetValue(prop.Name), null);
                return true;
            });
        }
        public static object eGetValue(this object entity, string propertyName)
        {
            var property = entity.GetType().GetProperties().FirstOrDefault(prop => prop.Name == propertyName);
            if (property == null) return null;
            return property.GetValue(entity, null);
        }
        #endregion

    }
}