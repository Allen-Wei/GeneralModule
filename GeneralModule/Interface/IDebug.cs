using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IDebug
/// </summary>
namespace GeneralModule.Interface
{

        public interface IDebug
        {
            string GetAppName();
            string DebugPath();
        }
}