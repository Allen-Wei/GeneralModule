using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;

//sqlmetal /server:. /database:test /code:SignLog.cs /pluralize /views /functions /sprocs /namespace:GeneralModule.Module  /context:GMContext
namespace GeneralModule.Module
{
    public partial class GMContext : DataContext
    {

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
        partial void OnCreated();

        #region Constructor

        public GMContext(string connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public GMContext(System.Data.IDbConnection connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public GMContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public GMContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }
        #endregion
    }

}
