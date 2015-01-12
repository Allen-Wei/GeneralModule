using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

/// <summary>
/// Summary description for ModuleInfo
/// </summary>
namespace GeneralModule
{
    public class ModuleInfo
    {
        /// <summary>
        /// Module name
        /// </summary>
        public string moduleName { get; set; }
        /// <summary>
        /// Module data stored type: XML or SQLServer
        /// </summary>
        public ModuleInfo.DataType dataType { get; set; }
        /// <summary>
        /// Module data stored postion: XML file path or table name
        /// </summary>
        public string dataPosition { get; set; }
        /// <summary>
        /// Module is debug mode
        /// </summary>
        public bool isDebug { get; set; }
        /// <summary>
        /// Module version
        /// </summary>
        public string version { get; set; }


        /// <summary>
        /// Get ModuleInfo xml file path (priority get file path from WebConfig[configuration>appSettings>Module-XML-Path])
        /// </summary>
        /// <returns>file full path</returns>
        private static string GetModuleInfoXMLPath()
        {
            string strXmlPath = Utilily.Utilily.GetAppValueByName<string>("Module-XML-Path");
            if (String.IsNullOrWhiteSpace(strXmlPath))
            {
                strXmlPath = Utilily.WebTool.svr.MapPath("~/XMLData/Module.xml");
            }
            if (!File.Exists(strXmlPath))
            {
                throw new FileNotFoundException("ModuleInfo configure file not found.");
            }
            return strXmlPath;
        }
        public static ModuleInfo GetModuleInfo(string strModuleName)
        {
            XElement eles = null;
            //get moduleinfo from cache
            object moduleInfoXML = Utilily.WebTool.GetCache("Alan-ModuleInfo-XML");
            if (moduleInfoXML != null)
                eles = XElement.Parse(moduleInfoXML.ToString());
            else
            {
                string strXMLFullPath = ModuleInfo.GetModuleInfoXMLPath();
                if (!File.Exists(strXMLFullPath)) throw new FileNotFoundException("ModuleInfo configure file not found.");

                using (FileStream stream = new FileStream(strXMLFullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    eles = XElement.Load(stream);
                    Utilily.WebTool.SetCache("Alan-ModuleInfo-XML", eles.ToString());
                }
            }

            var query = from ele in eles.Descendants("ModuleInfo")
                        where ele.Descendants("moduleName").First().Value == strModuleName
                        select ele;
            if (query.Count() <= 0)
            {
                return null;
            }
            var first = query.FirstOrDefault();

            return ModuleInfo.ToModuleInfo(first);
        }
        public static string GetModuleDataPosition(string strModuleName)
        {
            ModuleInfo info = ModuleInfo.GetModuleInfo(strModuleName);
            if (info != null)
            {
                return info.dataPosition;
            }
            else
            {
                return null;
            }
        }

        public bool InsertModuleConfig()
        {
            string xmlPath = ModuleInfo.GetModuleInfoXMLPath();
            XElement eles = XElement.Load(xmlPath);
            XElement ele = this.ToXElement();// Utilily.XMLUtilily.EntityToEle(this);
            eles.Add(ele);
            eles.Save(xmlPath);
            return true;
        }
        public static ModuleInfo ToModuleInfo(XElement ele)
        {
            ModuleInfo module = new ModuleInfo();
            module.moduleName = ele.Element("moduleName").Value;
            DataType dType = DataType.XML;
            Enum.TryParse<DataType>(ele.Element("dataType").Value, out dType);
            module.dataType = dType;
            module.dataPosition = ele.Element("dataPosition").Value;
            module.isDebug = Convert.ToBoolean(ele.Element("isDebug").Value);
            module.version = ele.Element("version").Value;
            return module;
        }
        public XElement ToXElement()
        {
            XElement ele = new XElement("ModuleInfo",
                new XElement("moduleName", this.moduleName),
                new XElement("dataType", this.dataType.ToString()),
                new XElement("dataPosition", this.dataPosition),
                new XElement("isDebug", this.isDebug.ToString()),
                new XElement("version", this.version));
            return ele;
        }
        public enum DataType
        {
            XML,
            SQLServer
        }

    }
}