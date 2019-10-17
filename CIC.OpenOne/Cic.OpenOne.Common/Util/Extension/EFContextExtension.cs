using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using Cic.OpenOne.Common.Util.Linq;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.Model;

namespace Cic.OpenOne.Common.Util.Extension
{
    /// <summary>
    /// Idea of this extension
    /// * provide methods for the ObjectContexts of the project (like DdOlExtended) for usage inside to have an entry-point to Methods like
    /// ctx.ExecuteFunction(). For this these kind of methods must be implemented in the Context-Derived Class and at that point delegated to this extension Method
    /// That avoids having a lot of Extension-Includes spread over the project
    /// </summary>
    public static class EFContextExtension
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static List<String> sessionCommands = new List<String>();

        /// <summary>
        /// must be called to register a set of session commands on the Object Context to be registered every time the session opens
        /// </summary>
        /// <param name="context"></param>
        public static void RegisterObjectContext(this System.Data.Objects.ObjectContext context, IAlteredSession target)
        {
            //attention, register the event-handler on the target! there the call will be delegated to this extension Method: PerformStateChange
            context.Connection.StateChange += target.EntityConnection_StateChange;
        }

        /// <summary>
        /// Registers a sessioncommand to be executed on registered Object Contexts
        /// </summary>
        /// <param name="str"></param>
        public static void AddSessionCommand(String str)
        {
            sessionCommands.Add(str);
        }

        /// <summary>
        /// Performs the registered Session Commands
        /// </summary>
        /// <param name="context"></param>
        /// <param name="e"></param>
        public static void PerformStateChange(this System.Data.Objects.ObjectContext context, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Open)
            {
                if (sessionCommands.Count > 0)
                {
                    foreach (String cmd in sessionCommands)
                    {
                        //_log.Debug("Exec Session Command " + cmd);
                        context.ExecuteStoreCommand(cmd);
                    }
                }
            }
        }

        /// <summary>
        /// Executes a named stored procedure directly at the database with the given parameters
        /// the conceptual model is not used. The Procedure doesnt have to be mapped to the Entity Framework.
        /// Returned Values are stored in the param.value when param is defined as out-param.
        /// </summary>
        /// <param name="procedure">Name of the Procedure</param>
        /// <param name="param">Parameters needed by the Procedure.</param>
        public static void ExecuteProcedure(this  System.Data.Objects.ObjectContext context, string procedure, params DbParameter[] param)
        {
            //Execute Query
            DbConnection con = (context.Connection as EntityConnection).StoreConnection;
            ExecuteProcedure(con, procedure, param);
        }
        public static void ExecuteProcedure(DbConnection con, string procedure, params DbParameter[] param)
        { 
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                DbCommand cmd = con.CreateCommand();

                // query values with a stored procedure with two out parameters
                cmd.CommandText = procedure;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if(param!=null)
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

        }

        /// <summary>
        /// Executes a stored Function and returns the result value
        /// The Parameters have to contain a ReturnValue Direction Parameter. 
        /// All Parameters (except the return value Parameter) must be named as defined in the Database.
        /// </summary>
        /// <param name="function">Name of the Function including Package-Name, eg. CIC_UTILS.TEST</param>
        /// <param name="param">All Parameters including Return Value Parameter, including Type and Name</param>
        /// <returns></returns>
        public static object ExecuteFunction(this System.Data.Objects.ObjectContext context, string function, params DbParameter[] param)
        {

            //Execute Query
            DbConnection con = (context.Connection as EntityConnection).StoreConnection;
            return ExecuteFunction(con, function, param);
        }
        public static object ExecuteFunction(DbConnection con, string function, params DbParameter[] param)
        {
            object rval = null;
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

            return rval;
        }

        /// <summary>
        /// getKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long getKey(this System.Data.Objects.ObjectContext context,System.Data.EntityKey key)
        {
            if (key == null) return 0;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return 0;
            return (long)member[0].Value;
        }

        /// <summary>
        /// getKeyName
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String getKeyName(this System.Data.Objects.ObjectContext context,System.Data.EntityKey key)
        {
            if (key == null) return null;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return null;
            return member[0].Key;
        }

        /// <summary>
        /// getEntityKey
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.Data.EntityKey getEntityKey(this System.Data.Objects.ObjectContext context,System.Type type, long value)
        {
            System.Data.Metadata.Edm.EntityType etype = MyGetEntityType(context, type);
            return new System.Data.EntityKey(context.DefaultContainerName + "." + etype.Name, etype.KeyMembers[0].Name, value);
        }
       

        private static System.Data.Metadata.Edm.EntityType MyGetEntityType(this System.Data.Objects.ObjectContext context, System.Type type)
        {
            return context.MetadataWorkspace.GetType(type.Name, type.Namespace, System.Data.Metadata.Edm.DataSpace.CSpace) as System.Data.Metadata.Edm.EntityType;
        }
    }
}
