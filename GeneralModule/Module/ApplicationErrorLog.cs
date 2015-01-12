using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

using GeneralModule;
using GeneralModule.Interface;
using GeneralModule.Utilily;

namespace GeneralModule.Module
{
    public class ApplicationErrorLog : IHttpModule, IModule
    {
        public string GetModuleName()
        {
            return "ApplicationErrorLog";
        }

        void ErrorHandle(object sender, EventArgs e)
        {
            try
            {
                string strLogPath = this.GetModuleName() + ".log";
                string strRedirectURL = String.Empty;
                try {
                    string strDataPosition = ModuleInfo.GetModuleDataPosition(this.GetModuleName());
                    if (!String.IsNullOrWhiteSpace(strDataPosition))
                    {
                        string strFilePath = WebTool.svr.MapPath(strDataPosition);
                        if (File.Exists(strFilePath))
                        {
                            XElement eles = XElement.Load(strFilePath);
                            XElement eleRedirectURL = eles.Descendants("redirectURL").FirstOrDefault();
                            if (eleRedirectURL != null)
                                strRedirectURL = eleRedirectURL.Value;
                            XElement eleLogName= eles.Descendants("logName").FirstOrDefault();
                            if (eleLogName != null)
                                strLogPath = eleLogName.Value;

                        }
                    }
                }catch { }
                string strError = String.Format("Request URL:{0}, URL Reference:{1}, User Agent:{2}, Request IP:{3}", WebTool.req.RawUrl, WebTool.req.UrlReferrer, WebTool.req.UserAgent, WebTool.req.UserHostAddress);
                DebugUtilily.FileDebug(strError, strLogPath);
                //if (!String.IsNullOrWhiteSpace(strRedirectURL)) {
                //    WebTool.rep.Clear();
                //    WebTool.rep.Redirect(strRedirectURL, true);
                //}
            }
            catch { }
        }
        public void Dispose()
        {
        }


        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(ErrorHandle);
        }
    }
}