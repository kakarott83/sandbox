using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// DAO for Permission Handling 
    /// </summary>
    public class PermissionDao : IPermissionDao
    {
        private const string QUERYMODULES = "select distinct codermo from rfu";
        private const string QUERYFUNCTIONS = "select distinct coderfu from rfu where codermo=:module";
        private const string QUERYPERMISSION = "select rfu.rshow, rfu.rchange,rfu.rinsert,rfu.rdelete,rfu.rexecute,rfu.ronstart,rfu.rres7,rfu.rres8 from rgm, rgr,rrorgrnm,rrorfunm,rfu where rgm.sysrgr = rgr.sysrgr AND rgr.name = rrorgrnm.codergr AND rrorgrnm.coderro = rrorfunm.coderro AND rrorfunm.codermo = rfu.codermo AND rrorfunm.coderfu = rfu.coderfu and rfu.codermo=:module and rfu.coderfu=:function and rgm.syswfuser=:syswfuser";



        /// <summary>
        /// Returns a list of Modules with permissions
        /// </summary>
        /// <returns></returns>
        public List<String> getModules()
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {

                return ctx.ExecuteStoreQuery<String>(QUERYMODULES, null).ToList();
            }
        }


        /// <summary>
        /// Returns a list of Functions for a module
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public List<String> getFunctions(String module)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "module", Value = module });


                return ctx.ExecuteStoreQuery<String>(QUERYFUNCTIONS, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// Returns the permission flags for a module, function and user
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public PermissionDto getPermission(String module, String function, long syswfuser)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "module", Value = module });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "function", Value = function });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });


                return ctx.ExecuteStoreQuery<PermissionDto>(QUERYPERMISSION, parameters.ToArray()).FirstOrDefault();
            }
        }
    }
}
