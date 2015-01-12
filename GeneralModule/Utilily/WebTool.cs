using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace GeneralModule.Utilily
{
        public static class WebTool
        {
            public static HttpRequest req { get { return HttpContext.Current.Request; } }
            public static HttpResponse rep { get { return HttpContext.Current.Response; } }
            public static HttpServerUtility svr { get { return HttpContext.Current.Server; } }
            public static HttpSessionState session { get { return HttpContext.Current.Session; } }
            public static HttpApplicationState app { get { return HttpContext.Current.Application; } }
            public static TraceContext trace { get { return HttpContext.Current.Trace; } }
            public static System.Web.Caching.Cache cache { get { return HttpContext.Current.Cache; } }

            public static bool IsPC(string userAgent) {
                return !IsMobile(userAgent);
            }
            public static bool IsMobile(string userAgent)
            {
                try
                {
                    string[] mobileAgents = new string[] { "iphone", "ppc", "windows ce", "blackberry", "opera mini", "mobile", "palm", "portable", "opera mobi", "android" };
                    userAgent = userAgent.ToLower();
                    return mobileAgents.Any((agent) => { return userAgent.Contains(agent); });
                }
                catch { return false; }
            }

            public static string GetConnectionString(string connectionName)
            {
                if (String.IsNullOrWhiteSpace(connectionName)) return String.Empty;
                string connectionString= WebConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                return connectionString;
            }
            public static string GetAppConfigString(string appKeyName) {
                if (String.IsNullOrWhiteSpace(appKeyName)) return String.Empty;
                string appString = WebConfigurationManager.AppSettings[appKeyName];
                return appString;
            }

            public static bool SetSession(string strSessionName, object value) {
                try { session[strSessionName] = value; return true; }
                catch { return false; }
            }
            public static object GetSession(string strSessionName)
            {
                if (WebTool.session == null)
                {
                    return null;
                }
                return WebTool.session[strSessionName];
            }
            public static T GetSession<T>(string strSessionName, Type t)
            {
                T obj = Activator.CreateInstance<T>();
                try
                {
                    obj = (T)session[strSessionName];
                    return obj;
                }
                catch {
                    obj = default(T);
                    return obj;
                }
            }

            public static bool SetCache(string strCacheName, object value)
            {
                WebTool.cache.Insert(strCacheName, value);
                return true;
            }
            public static bool SetCache(string strCacheName, object value, TimeSpan slidingExpiration)
            {
                WebTool.cache.Insert(strCacheName, value, null, DateTime.MaxValue, slidingExpiration);
                return true;
            }
            public static bool SetCache(string strCacheName, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
            {
                WebTool.cache.Insert(strCacheName, value, null, absoluteExpiration, slidingExpiration);
                return true;
            }
            public static object GetCache(string strCachenName)
            {
                return WebTool.cache.Get(strCachenName);
            }
            public static T GetCache<T>(string strCacheName, Type t)
            {
                T obj = Activator.CreateInstance<T>();
                try
                {
                    var cacheValue = WebTool.cache.Get(strCacheName);
                    obj = (T)cacheValue;
                    return obj;
                }
                catch
                {
                    obj = default(T);
                    return obj;
                }
            }
        }
    }