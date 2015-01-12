using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.Xml.Linq;
using GeneralModule.Interface;
using GeneralModule.Utilily.Extension;

//TODO Parameter add automatic add placeholder
namespace GeneralModule.Module
{

    /// <summary>
    /// URL Route
    /// default xml path is ~\XMLData\CustomRoute.xml
    /// </summary>
    public class URLRoute : IModule
    {
        public static void RouteAll(RouteCollection globalRoute)
        {
            //ignores
            var ignores = from ele in IgnoreXEle.Elements("ignore")
                          select ele;
            foreach (var ignore in ignores)
            {
                globalRoute.Ignore(ignore.Attribute("value").Value);
            }


            var routes = RouteEntity.LoadAll().OrderByDescending(current => current.priority);
            foreach (var route in routes)
            {
                var paras = route.paras;
                if (paras != null && paras.Count >= 1)
                {
                    RouteValueDictionary defValDict = new RouteValueDictionary();
                    RouteValueDictionary regexValDict = new RouteValueDictionary();
                    foreach (var para in paras)
                    {
                        if (!String.IsNullOrWhiteSpace(para.defval))
                        {
                            defValDict.Add(para.name, para.defval);
                        }
                        if (!String.IsNullOrWhiteSpace(para.regval))
                        {
                            regexValDict.Add(para.name, para.regval);
                        }
                    }
                    globalRoute.MapPageRoute(route.name, route.url, route.page, false,
                        defValDict, regexValDict);
                    continue;
                }
                try
                {
                    globalRoute.MapPageRoute(route.name, route.url, route.page, false);
                }
                catch (Exception ex)
                {
                    //Note has same route name
                }
            }
        }

        public static string XMLPath
        {
            get
            {
                string strModuleName = (new URLRoute()).GetModuleName();
                string strDataPosition = ModuleInfo.GetModuleDataPosition(strModuleName);
                return Utilily.WebTool.svr.MapPath(strDataPosition);
            }
        }
        public static XElement XMLXEle
        {
            get
            {
                return XElement.Load(XMLPath);
            }
        }

        public static XElement RouteXEle
        {
            get
            {
                return XMLXEle.Element("routes");
            }
        }
        public static XElement IgnoreXEle
        {
            get
            {
                return XMLXEle.Element("ignores");
            }
        }


        public class RouteEntity
        {

            public string name { get; set; }
            public string url { get; set; }
            public string page { get; set; }
            public int priority { get; set; }
            public RouteEntity() { }

            public RouteEntity(string name, string url, string page, int priority)
            {
                this.name = name;
                this.url = url;
                this.page = page;
                this.priority = priority;
            }

            public List<RoutePara> paras { get; set; }

            /// <summary>
            /// Load all routes
            /// </summary>
            /// <returns>Routes list</returns>
            public static List<RouteEntity> LoadAll()
            {
                XElement eles = URLRoute.RouteXEle;
                List<RouteEntity> routes = (from ele in eles.Elements("route")
                                            select new RouteEntity
                                            {
                                                name = ele.Attribute("name").Value,
                                                url = ele.Attribute("url").Value,
                                                page = ele.Attribute("page").Value,
                                                priority = ele.Attribute("priority").eIsNull() ? 0 : ele.Attribute("priority").Value.eToInt(0),
                                                paras = (from para in ele.Elements("para")
                                                         select new RoutePara
                                                         {
                                                             name = para.Attribute("name").Value,
                                                             defval = para.Attribute("defval").Value,
                                                             regval = para.Attribute("regval").Value
                                                         }).ToList()
                                            }).ToList();



                return routes;
            }

            //TODO valid parameter match url
            public bool IsValid()
            {
                bool propertyValid =
                    !this.name.eIsEmpty() &&
                    Regex.IsMatch(this.page, "^~/");
                //url can't start with '/'
                if (!this.url.eIsEmpty())
                {
                    if (this.url[0] == '/') return false;
                }


                if (propertyValid)
                {
                    //valid url and parameter match
                }
                return propertyValid;
            }

            /// <summary>
            /// Route is exist 
            /// </summary>
            /// <param name="attribute">Route attribute name</param>
            /// <param name="value">Route attribute value</param>
            /// <returns>Is existed</returns>
            public static bool IsExist(string attribute, string value)
            {
                if (String.IsNullOrEmpty(value)) return true;

                return RouteXEle
                        .Elements("route")
                        .Any(current => current.Attribute(attribute).eIfNull(value).ToLower() == value.ToLower());
            }
            /// <summary>
            /// Route is exist
            /// </summary>
            /// <param name="routeName">Route name value</param>
            /// <returns>Is existed</returns>
            public static bool IsExist(string routeName)
            {
                return IsExist("name", routeName);
            }

            public static RouteEntity Get(string routeName)
            {
                XElement ele = RouteXEle.Elements("route")
                    .SingleOrDefault(current => current.Attribute("name").eIfNull("").ToLower() == routeName.ToLower());

                return Ele2Entity(ele);
            }

            public static List<RouteEntity> Query(string attributeName, string attributeValue)
            {
                List<RouteEntity> query = RouteXEle.Elements("route")
                                            .Where(current => current.Attribute(attributeName).eIfNull("").ToLower() == attributeValue.ToLower())
                                            .Select(current => Ele2Entity(current))
                                            .ToList<RouteEntity>();
                return query;
            }

            public static List<RouteEntity> QueryStartsWith(string attributeName, string starts)
            {
                List<RouteEntity> query = RouteXEle.Elements("route")
                                           .Where(current => current.Attribute(attributeName).eIfNull("").ToLower().StartsWith(starts))
                                           .Select(current => Ele2Entity(current))
                                           .ToList<RouteEntity>();
                return query;
            }

            public static List<RouteEntity> QueryLike(string attributeName, string containers)
            {
                List<RouteEntity> query = RouteXEle.Elements("route")
                                          .Where(current => current.Attribute(attributeName).eIfNull("").ToLower().Contains(containers))
                                          .Select(current => Ele2Entity(current))
                                          .ToList<RouteEntity>();
                return query;
            }

            /// <summary>
            /// Insert a route
            /// </summary>
            /// <returns></returns>
            public bool Insert()
            {
                if (!this.IsValid()) return false;

                if (IsExist(this.name)) { return false; }

                XElement ele = Entity2Ele(this);
                XElement eles = XElement.Load(XMLPath);
                eles.Element("routes").Add(ele);
                eles.Save(URLRoute.XMLPath);

                return true;
            }
          

            public bool AppendPara(RoutePara para)
            {
                if (this.url.eIsEmpty()) return false;

                this.url += "/{" + para.name + "}";
                if (this.paras.eIsNull()) this.paras = new List<RoutePara>();
                this.paras.Add(para);
                return true;
            }


            public bool Delete()
            {
                if (!RouteEntity.IsExist(this.name)) return false;

                XElement xele = XElement.Load(URLRoute.XMLPath);
                var query = from ele in xele.Element("routes").Elements("route")
                            where ele.Attribute("name").Value == this.name || ele.Attribute("url").Value == this.url
                            select ele;
                query.Remove();
                xele.Save(URLRoute.XMLPath);
                return true;
            }

            public static void DeleteAll(List<RouteEntity> routes)
            {
                XElement xele = XElement.Load(URLRoute.XMLPath);
                List<string> names = routes.Select(r => r.name).ToList();
                xele.Element("routes")
                    .Elements("route")
                    .Where(current => names.Contains(current.Attribute("name").eIfNull(""))).Remove();
                xele.Save(URLRoute.XMLPath);
            }

            public bool ParaAdd(URLRoute.RoutePara para)
            {
                if (String.IsNullOrWhiteSpace(this.name) || String.IsNullOrWhiteSpace(this.url)) return false; 

                XElement xele = XElement.Load(URLRoute.XMLPath);
                var query = (from ele in xele.Element("routes").Elements("route")
                             where ele.Attribute("name").Value == this.name
                             select ele).FirstOrDefault();
                if (query == null)
                {
                    return false;
                }

                query.Add(new XElement("para",
                    new XAttribute("name", para.name),
                    new XAttribute("defval", para.defval),
                    new XAttribute("regval", para.regval)));
                xele.Save(URLRoute.XMLPath);
                return true;
            }
            public bool ParaDelete(string paraName)
            {
                if (String.IsNullOrWhiteSpace(this.name) || String.IsNullOrWhiteSpace(this.url))
                {
                    return false;
                }

                XElement xele = XElement.Load(URLRoute.XMLPath);
                var queryRoute = (from ele in xele.Element("routes").Elements("route")
                                  where ele.Attribute("name").Value == this.name && ele.Attribute("url").Value == this.url
                                  select ele).FirstOrDefault();
                if (queryRoute == null)
                {
                    return false;
                }
                var queryPara = (from ele in queryRoute.Elements("para")
                                 where ele.Attribute("name").Value == paraName
                                 select ele).FirstOrDefault();
                if (queryPara == null)
                {
                    return false;
                }
                queryPara.Remove();
                xele.Save(URLRoute.XMLPath);
                return true;
            }

            public static bool IgnoreExtInsert(string extname)
            {
                if (String.IsNullOrWhiteSpace(extname)) { return false; }
                string value = "{resource}." + extname + "/{*pathInfo}";
                return IgnoreInsert(value);
            }
            public static bool IgnoreInsert(string value)
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    return false;
                }
                XElement xele = XElement.Load(URLRoute.XMLPath);
                xele.Element("ignores").Add(new XElement("ignore", new XAttribute("value", value)));
                xele.Save(URLRoute.XMLPath);
                return true;
            }
            public static bool IgnoreDelete(string value)
            {
                if (String.IsNullOrWhiteSpace(value)) { return false; }
                XElement xele = XElement.Load(URLRoute.XMLPath);
                var query = from ele in xele.Element("ignores").Elements("ignores")
                            where ele.Attribute("value").Value == value
                            select ele;
                query.Remove();
                xele.Save(URLRoute.XMLPath);
                return true;
            }

            public static XElement Entity2Ele(URLRoute.RouteEntity entity)
            {
                XElement ele = new XElement("route",
                    new XAttribute("name", entity.name),
                    new XAttribute("url", entity.url),
                    new XAttribute("page", entity.page),
                    new XAttribute("priority", entity.priority));
                if (entity.paras != null && entity.paras.Count >= 1)
                {
                    foreach (RoutePara para in entity.paras)
                    {
                        ele.Add(new XElement("para",
                            new XAttribute("name", para.name ?? ""),
                            new XAttribute("defval", para.defval ?? ""),
                            new XAttribute("regval", para.regval ?? "")));
                    }
                }

                return ele;
            }
            public static RouteEntity Ele2Entity(XElement ele)
            {
                if (ele.eIsNull()) return null;


                RouteEntity entity = new RouteEntity()
                {
                    name = ele.Attribute("name").eIfNull(""),
                    url = ele.Attribute("url").eIfNull(""),
                    page = ele.Attribute("page").eIfNull(""),
                    priority = ele.Attribute("priority").eIfNull(0)
                };
                var paraQuery = from paraEle in ele.Elements("para")
                                select paraEle;
                entity.paras = new List<RoutePara>();
                foreach (var para in paraQuery)
                {
                    entity.paras.Add(new RoutePara()
                    {
                        name = para.Attribute("name").Value,
                        defval = para.Attribute("defval").Value,
                        regval = para.Attribute("regval").Value
                    });
                }
                return entity;
            }
        }
        public class RoutePara
        {
            public string name { get; set; }
            public string defval { get; set; }
            public string regval { get; set; }
            public RoutePara() { }

            public RoutePara(string name, string defVal, string regVal)
            {
                this.name = name;
                this.defval = defVal;
                this.regval = regVal;
            }
        }

        public string GetModuleName()
        {
            return "URLRoute";
        }

        public static bool Validate(bool repaire = true)
        {

            string fileFullPath = XMLPath;
            if (!File.Exists(fileFullPath))
            {
                if (!repaire)
                {
                    return false;
                }
                else
                {
                    ReWriteXML();
                    return true;
                }
            }
            else
            {
                //validate structure
                XElement xele = XElement.Load(fileFullPath);
                if (xele.Elements("ignores").Count() <= 0 || xele.Elements("routes").Count() <= 0)
                {
                    if (repaire)
                    {
                        ReWriteXML();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }

            }

        }
        public static void ReWriteXML()
        {

            string fileFullPath = XMLPath;

            XDocument xdoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("URLRoute",
                    new XElement("ignores",
                        new XElement("ignore", new XAttribute("value", "{resource}.axd/{*pathInfo}")),
                        new XElement("ignore", new XAttribute("value", "{resource}.aspx/{*pathInfo}"))),
                    new XElement("routes")));
            xdoc.Save(fileFullPath);
        }
    }
}
