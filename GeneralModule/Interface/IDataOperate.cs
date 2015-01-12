using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralModule.Interface
{

        public interface IDataOperate
        {
            bool Insert();
            bool Delete();
            bool Update();
            bool IsExist();
        }
}
