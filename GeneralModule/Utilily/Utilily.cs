using System;
using  System.Collections;
using  System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using GeneralModule.Utilily.Extension;


namespace GeneralModule.Utilily
{
        public static class Utilily
        {

          
            public static void OverriedAttribute(object obj1, object obj2)
            {
                obj1.eOverrideAttr(obj2);
            }
        
            public static void OverriedAttribute(object target, object attach, bool validPropertyType)
            {
                target.eOverrideAttr(attach, validPropertyType);
            }
            public static bool SetSpecialAttVal(ref object objEntity, string strAttName, object objVal)
            {
                bool bolSeted = false;
                PropertyInfo[] properties = objEntity.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    string strCurrentAttName = property.Name;
                    if (strCurrentAttName == strAttName)
                    {
                        try
                        {
                            property.SetValue(objEntity, objVal, null);
                            bolSeted = true;
                        }
                        catch { }
                    }
                }
                return bolSeted;
            }



            //public static string Serialize<T>(T obj)
            //{
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            //    MemoryStream ms = new MemoryStream();
            //    serializer.WriteObject(ms, obj);
            //    string retVal = Encoding.UTF8.GetString(ms.ToArray());
            //    return retVal;
            //}

            public static T Deserialize<T>(string json)
            {
                T obj = Activator.CreateInstance<T>();
                try
                {
                    MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    obj = (T)serializer.ReadObject(ms);
                    ms.Close();
                }
                catch { }
                return obj;
            }

            public static string ContractSerialize(Object obj)
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                var ms = new MemoryStream();
                serializer.WriteObject(ms, obj);
                string retVal = Encoding.UTF8.GetString(ms.ToArray());
                return retVal;
            }

            public static string JsSerialize(object obj)
            {
                if (obj == null) return "";
                var js = new JavaScriptSerializer();
                return js.Serialize(obj);
            }
            public static string Serialize(object obj)
            {
                if (obj == null) return "";
                var js = new JavaScriptSerializer();
                return js.Serialize(obj);
            }

            public static string TableSerialize(DataTable table)
            {
                if (table.eIsNull()) return null;

                var list = (
                            from DataRow row in table.Rows 
                            select table.Columns.Cast<DataColumn>().ToDictionary(column => column.ColumnName, column => row[column])
                            )
                            .ToList();
                return JsSerialize(list);
            }

            //public static string Serializer(object obj)
            //{
            //    if (obj == null) return "";
            //    var js = new JavaScriptSerializer();
            //    return js.Serialize(obj);
            //}

            /// <summary>
            /// Get Application Value by Name
            /// </summary>
            /// <typeparam name="T">Stored Type in Application</typeparam>
            /// <param name="name">Stored Name in Application</param>
            /// <returns>Entity of T</returns>
            public static T GetAppValueByName<T>(string name)
            {
                try
                {
                    object value = WebTool.app[name];
                    T obj = (T)value;
                    return obj;
                }
                catch
                {
                    T obj = Activator.CreateInstance<T>();
                    return obj;
                }
            }

            public static string MD5Decrypt(string pToDecrypt, string sKey)
            {
                try
                {
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    byte[] inputByteArray = new byte[pToDecrypt.Length / 2 - 1];
                    for (int x = 0; x <= pToDecrypt.Length / 2 - 1; x++)
                    {
                        int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                        inputByteArray[x] = (byte)i;
                    }
                    des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                    des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    //Dim ret As New StringBuilder()
                    return System.Text.Encoding.Default.GetString(ms.ToArray());
                }
                catch
                {
                    return pToDecrypt;
                }
            }

            public static string MD5Encrypt(string pToEncrypt, string sKey)
            {
                try
                {
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                    des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                    des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    StringBuilder ret = new StringBuilder();
                    foreach (byte b in ms.ToArray())
                    {
                        ret.AppendFormat("{0:X2}", b);
                    }
                    cs.Close();
                    ms.Close();
                    return ret.ToString();
                }
                catch
                {
                    return pToEncrypt;
                }
            }
        }
    }