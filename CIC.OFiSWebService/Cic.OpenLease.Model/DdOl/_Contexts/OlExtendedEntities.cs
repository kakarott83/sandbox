using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.EntityClient;
using System.Data.Common;
using System.Data;

namespace Cic.OpenLease.Model.DdOl
{
    
    public partial class OlExtendedEntities : OlEntities
    {
        /// <summary>
        /// Executes a named stored procedure directly at the database with the given parameters
        /// the conceptual model is not used. The Procedure doesnt have to be mapped to the Entity Framework.
        /// Returned Values are stored in the param.value when param is defined as out-param.
        /// </summary>
        /// <param name="procedure">Name of the Procedure</param>
        /// <param name="param">Parameters needed by the Procedure.</param>
        public void ExecuteProcedure(string procedure, params DbParameter[] param)
        {
            //Execute Query
            DbConnection con = (this.Connection as EntityConnection).StoreConnection;
            try
            {
                
                con.Open();
                DbCommand cmd = con.CreateCommand();

                // query values with a stored procedure with two out parameters
                cmd.CommandText = procedure;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (DbParameter p in param)
                    cmd.Parameters.Add(p);

                //Execute Stored Procedure
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                //Log l = new Cic.Basic.Logging.Log4Net.Log(Cic.OpenLease.Service.UserConfigurationHelper.DeliverLoggerNameValue(), Cic.OpenLease.Service.UserConfigurationHelper.DeliverFileInfo());
                //l.Error("calling stored Procedure " + procedure + " failed: " + ex.Message);
                throw new InvalidOperationException("calling stored Procedure " + procedure + " failed: " + ex);
            }
            finally
            {
                con.Close();
            }
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
            object rval = null;
            //Execute Query
            DbConnection con = (this.Connection as EntityConnection).StoreConnection;
            try
            {

                con.Open();
                DbCommand cmd = con.CreateCommand();

                // query values with a stored procedure with two out parameters
                cmd.CommandText = function;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                DbParameter rParam = null;
                foreach (DbParameter p in param)
                {
                    cmd.Parameters.Add(p);
                    if (p.Direction == System.Data.ParameterDirection.ReturnValue)
                        rParam = p;
                }
                //Execute Stored Function
                //rval = cmd.ExecuteScalar();
                if (rParam == null)
                    throw new Exception("no Parameter with ParameterDirection.ReturnValue was specified!");
                cmd.ExecuteNonQuery();
                rval = rParam.Value;

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                //Log l = new Cic.Basic.Logging.Log4Net.Log(Cic.OpenLease.Service.UserConfigurationHelper.DeliverLoggerNameValue(), Cic.OpenLease.Service.UserConfigurationHelper.DeliverFileInfo());
                //l.Error("calling stored Procedure " + procedure + " failed: " + ex.Message);
                throw new InvalidOperationException("calling stored Function " + function + " failed: " + ex);
            }
            finally
            {
                con.Close();
            }
            return rval;
        }

        public System.Data.EntityKey getEntityKey(System.Type type, long value)
        {
            System.Data.Metadata.Edm.EntityType etype = MyGetEntityType(this, type);

            return new System.Data.EntityKey(this.DefaultContainerName + "." + etype.Name, etype.KeyMembers[0].Name, value);

        }

        private static System.Data.Metadata.Edm.EntityType MyGetEntityType(System.Data.Objects.ObjectContext context, System.Type type)
        {
            return context.MetadataWorkspace.GetType(type.Name, type.Namespace, System.Data.Metadata.Edm.DataSpace.CSpace) as System.Data.Metadata.Edm.EntityType;
        }

        public static long getKey(System.Data.EntityKey key)
        {
            if (key == null) return 0;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return 0;
            return (long)member[0].Value;
        }

        public static String getKeyName(System.Data.EntityKey key)
        {
            if (key == null) return null;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return null;
            return member[0].Key;
        }

    }
}