using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeneralModule.Utilily;
using GeneralModule.Utilily.Extension;

namespace GeneralModule.Test
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = File.ReadAllText(Server.MapPath("~/Module/URLRoute.cs"));
            var prefix = "alan";
            string fileName;
            DebugUtilily.SetDebugPath = Server.MapPath("~/Test");
            DebugUtilily.FileSplitDebug(context, prefix, out fileName, 1024 * 25, ".log");
            Response.Write(fileName);
        }


    }
}