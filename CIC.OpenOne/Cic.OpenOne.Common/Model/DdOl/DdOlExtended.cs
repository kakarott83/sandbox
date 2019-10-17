using System;
using System.Data;
using Cic.OpenOne.Common.Util.Config;
using System.Data.Common;
using System.Data.EntityClient;
using CIC.Database.OL.EF4.Model;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using System.Collections.Generic;
using Cic.OpenOne.Common.Util.Extension;
using System.Data.Metadata.Edm;

namespace Cic.OpenOne.Common.Model.DdOl
{
    /// <summary>
    /// Extension of generated EDMX to use the configured database connection
    /// </summary>
    public class DdOlExtended : OLContext, IAlteredSession
    {
       
        /// <summary>
        /// DdOlExtended-Konstruktor
        /// </summary>
        public DdOlExtended()
            : base(ConnectionStringBuilder.GetConnectionString(typeof(OLContext)))
        {
            ((System.Data.Objects.ObjectContext)this).RegisterObjectContext(this);
        }
        public void EntityConnection_StateChange(object sender, StateChangeEventArgs e)
        {
            this.PerformStateChange(e);
        }
        /// <summary>
        /// For TEntity return the max field length or -1 when no value defined
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public int Length<TEntity>(string field)
        {
            var metaWorkspace = new System.Data.Metadata.Edm.MetadataWorkspace(new string[] { "res://*/" }, new Assembly[] { GetType().BaseType.Assembly });
            object val = metaWorkspace.GetEntityContainer(DefaultContainerName + "StoreContainer", DataSpace.SSpace).BaseEntitySets[typeof(TEntity).Name].ElementType.Members[field].TypeUsage.Facets["MaxLength"].Value;
            if (val != null)
                return (int)val;
            return -1;
        }

        public List<KeyValuePair<String,int>> getLengths<TEntity>()
        {
            List<KeyValuePair<String, int>> rval = new List<KeyValuePair<string, int>>();
            var metaWorkspace = new System.Data.Metadata.Edm.MetadataWorkspace(new string[] { "res://*/" }, new Assembly[] { GetType().BaseType.Assembly });
            var members = metaWorkspace.GetEntityContainer(DefaultContainerName + "StoreContainer", DataSpace.SSpace).BaseEntitySets[typeof(TEntity).Name].ElementType.Members;
            foreach(EdmMember member in members)
            {
                if("VARCHAR2".Equals(member.TypeUsage.EdmType.Name))
                {
                    if (member.TypeUsage.Facets["MaxLength"].Value != null)
                    {
                        rval.Add(new KeyValuePair<string, int>(member.Name, (int)member.TypeUsage.Facets["MaxLength"].Value));
                    }
                }
            }
            return rval;
        }
        /// <summary>
        /// Executes a named stored procedure directly at the database with the given parameters
        /// the conceptual model is not used. The Procedure doesnt have to be mapped to the Entity Framework.
        /// Returned Values are stored in the param.value when param is defined as out-param.
        /// </summary>
        /// <param name="procedure">Name of the Procedure</param>
        /// <param name="param">Parameters needed by the Procedure.</param>
        public void ExecuteProcedure(string procedure, params DbParameter[] param)
        {
            ((System.Data.Objects.ObjectContext)this).ExecuteProcedure(procedure, param);
        }

        /// <summary>
        /// Executes a stored Function and returns the result value
        /// The Parameters have to contain a ReturnValue Direction Parameter. 
        /// All Parameters (except the return value Parameter) must be named as defined in the Database.
        /// </summary>
        /// <param name="function">Name of the Function including Package-Name, eg. CIC_UTILS.TEST</param>
        /// <param name="param">All Parameters including Return Value Parameter, including Type and Name</param>
        /// <returns></returns>
        public object ExecuteFunction(string function, params DbParameter[] param)
        {
            return ((System.Data.Objects.ObjectContext)this).ExecuteFunction(function, param);            
        }

     


    }
}