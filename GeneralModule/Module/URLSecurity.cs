using System;
using System.IO;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.SessionState;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using GeneralModule;
using GeneralModule.Interface;
using GeneralModule.Utilily;

namespace GeneralModule.Module
{
        public class URLSecurity : IHttpModule, IModule, IDataOperate
        {
            #region Property
            private string _urlMatchRule;
            public string urlMatchRule
            {
                get { return WebTool.svr.HtmlDecode(_urlMatchRule); }
                set { this._urlMatchRule = WebTool.svr.HtmlEncode(value); }
            }
            public bool sessionValid { get; set; }
            public string sessionName { get; set; }
            public string redirectURL { get; set; }
            #endregion


            public void Init(HttpApplication application)
            {
                //application.PostAcquireRequestState += new EventHandler(Application_PostAcquireRequestState);
                //application.PostMapRequestHandler += new EventHandler(Application_PostMapRequestHandler);
                application.PreRequestHandlerExecute += Application_PreRequestHandlerExecute;
            }

            void Application_PreRequestHandlerExecute(object source, EventArgs e) {

                if (WebTool.session != null)
                {

                    string reqURL = WebTool.req.RawUrl;
                    List<URLSecurity> urls = URLSecurity.LoadAll();
                    if (urls == null)
                    {
                        return;
                    }
                    foreach (URLSecurity url in urls)
                    {
                        Regex urlReg = new Regex(url.urlMatchRule, RegexOptions.IgnoreCase);
                        if (urlReg.IsMatch(reqURL))
                        {
                            try
                            {
                                //detect whether need valid session
                                if (url.sessionValid)
                                {
                                    object sessionValue = WebTool.session[url.sessionName];
                                    if (sessionValue == null)
                                    {
                                        WebTool.rep.Clear();
                                        WebTool.rep.Redirect(url.redirectURL, true);
                                        return;
                                    }
                                    else
                                    {
                                        string allowPass = sessionValue.ToString().ToLower();
                                        if (allowPass != "true")
                                        {
                                            WebTool.rep.Clear();
                                            WebTool.rep.Redirect(url.redirectURL, true);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    WebTool.rep.Clear();
                                    WebTool.rep.Redirect(url.redirectURL, true);
                                    return;
                                }
                            }
                            catch
                            {
                                return;
                            }
                        }
                    }
                }
            }

            void Application_PostMapRequestHandler(object source, EventArgs e)
            {
                HttpApplication app = (HttpApplication)source;

                if (app.Context.Handler is IReadOnlySessionState || app.Context.Handler is IRequiresSessionState)
                {
                    // no need to replace the current handler
                    return;
                }

                // swap the current handler
                app.Context.Handler = new MyHttpHandler(app.Context.Handler);
            }

            void Application_PostAcquireRequestState(object source, EventArgs e)
            {
                HttpApplication app = (HttpApplication)source;

                MyHttpHandler resourceHttpHandler = HttpContext.Current.Handler as MyHttpHandler;
                if (resourceHttpHandler != null)
                {
                    // set the original handler back
                    HttpContext.Current.Handler = resourceHttpHandler.OriginalHandler;
                }

                string reqURL = WebTool.req.RawUrl;
                List<URLSecurity> urls = URLSecurity.LoadAll();
                if (urls == null)
                {
                    return;
                }
                foreach (URLSecurity url in urls)
                {
                    Regex urlReg = new Regex(url.urlMatchRule, RegexOptions.IgnoreCase);
                    if (urlReg.IsMatch(reqURL))
                    {
                        try
                        {
                            //detect whether need valid session
                            if (url.sessionValid)
                            {
                                object sessionValue = WebTool.session[url.sessionName];
                                if (sessionValue == null)
                                {
                                    WebTool.rep.Clear();
                                    WebTool.rep.Redirect(url.redirectURL, true);
                                    return;
                                }
                                else
                                {
                                    string allowPass = sessionValue.ToString().ToLower();
                                    if (allowPass != "true")
                                    {
                                        WebTool.rep.Clear();
                                        WebTool.rep.Redirect(url.redirectURL, true);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                WebTool.rep.Clear();
                                WebTool.rep.Redirect(url.redirectURL, true);
                                return;
                            }
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
            }

            public void Dispose()
            {
            }

            // a temp handler used to force the SessionStateModule to load session state
            private class MyHttpHandler : IHttpHandler, IRequiresSessionState
            {
                internal readonly IHttpHandler OriginalHandler;

                public MyHttpHandler(IHttpHandler originalHandler)
                {
                    OriginalHandler = originalHandler;
                }

                public void ProcessRequest(HttpContext context)
                {
                    // do not worry, ProcessRequest() will not be called, but let's be safe
                    //throw new InvalidOperationException("MyHttpHandler cannot process requests.");
                }

                public bool IsReusable
                {
                    // IsReusable must be set to false since class has a member!
                    get { return false; }
                }
            }

            public string GetModuleName()
            {
                return "URLSecurity";
            }


            public bool Insert()
            {
                try
                {
                    string xmlPath = ModuleInfo.GetModuleDataPosition(this.GetModuleName());
                    string xmlFullPath = WebTool.svr.MapPath(xmlPath);
                    XElement eles = XElement.Load(xmlFullPath);
                    XElement insertEle = XMLUtilily.EntityToEle(this);
                    eles.Add(insertEle);
                    eles.Save(xmlFullPath);

                    return true;
                }
                catch { return false; }
            }

            public bool Delete()
            {
                return false;
            }

            public bool Update()
            {
                return false;
            }

            public bool IsExist()
            {
                return false;
            }
            public static List<URLSecurity> LoadAll()
            {
                List<URLSecurity> urls = new List<URLSecurity>();
                string strCacheKey = String.Format("Module-{0}-Cache", (new URLSecurity()).GetModuleName());

                if (WebTool.cache == null)
                {
                    return null;
                }
                //load from cache
                object cacheResult = WebTool.cache[strCacheKey]; //WebTool.session[strCacheKey];
                
                if (cacheResult != null)
                {
                    urls = (List<URLSecurity>)cacheResult;
                }
                else
                {
                    string strModuleName = (new URLSecurity()).GetModuleName();
                    string strXMLPath = ModuleInfo.GetModuleDataPosition(strModuleName);
                    if (String.IsNullOrWhiteSpace(strXMLPath))
                    {
                        return null;
                    }
                    string strXMLFullPath = Utilily.WebTool.svr.MapPath(strXMLPath);
                    if (!File.Exists(strXMLFullPath))
                    {
                        throw new FileNotFoundException("URLSecurity configure file not found");
                    }
                    FileStream stream = File.Open(strXMLFullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    XElement eles = XElement.Load(stream);
                    var query = from ele in eles.Descendants("URLSecurity")
                                select ele;
                    foreach (var i in query)
                    {
                        urls.Add(XMLUtilily.EleToEntity<URLSecurity>(i.ToString()));
                    }
                    stream.Close();
                    //WebTool.session[strCacheKey] = urls;
                    WebTool.cache.Insert(strCacheKey, urls, null, DateTime.MaxValue, TimeSpan.FromDays(1));
                }

                return urls;
            }

            public override string ToString()
            {
                return Utilily.Utilily.Serialize(this);
            }
        }
    }