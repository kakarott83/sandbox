using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Search;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Suche Data Access Obejct
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class SearchDao<R>
    {
        // private string QUERY = "select * from {0} where {1} order by {2} group by {3}";
        /// <summary>
        /// Constructor
        /// </summary>
        public SearchDao()
        {
           
        }

        /// <summary>
        /// Search Function
        /// </summary>
        /// <param name="query">Query</param>
        /// <param name="param">Parameters</param>
        /// <returns></returns>
        public List<R> search(string query, object[] param) 
        {

            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (!query.Contains(":p")) return ctx.ExecuteStoreQuery<R>(query, null).ToList();
                return ctx.ExecuteStoreQuery<R>(query, param).ToList();
            }
            
        }

       

    }
}