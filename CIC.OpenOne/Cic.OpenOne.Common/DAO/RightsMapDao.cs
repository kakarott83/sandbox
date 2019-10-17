using System;
using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// RightsMa DAO
    /// </summary>
    public class RightsMapDao : IRightsMapDao
    {
        const String RIGHTSMAP_QUERY = " SELECT " +
                                        " rfu.codermo || '_' || rfu.coderfu AS rightsMapId, " +
                                        " rfu.codermo, rfu.coderfu, " +
                                        " to_char(rfu.rshow) || to_char(rfu.rchange) || to_char(rfu.rdelete) || to_char(rfu.rinsert) || to_char(rfu.rexecute) || " +
                                        " to_char(rrorfunm.rshow) || to_char(rrorfunm.rchange) || to_char(rrorfunm.rdelete) || " + 
                                        " to_char(rrorfunm.rinsert) || to_char(rrorfunm.rexecute) AS rechte, " +
                                        " rfu.rshow AS rfuS, rfu.rchange AS rfuC, rfu.rdelete AS rfuD, rfu.rinsert AS rfuI, rfu.rexecute AS rfuX, " +
                                        " rrorfunm.rshow AS rroS, rrorfunm.rchange AS rroC, rrorfunm.rdelete AS rroD, rrorfunm.rinsert AS rroI, rrorfunm.rexecute AS rroX  " +
                                        " FROM rgm, rgr, rrorgrnm, rrorfunm, rfu " +
                                        " WHERE  " +
                                        " rgm.sysrgr = rgr.sysrgr AND  " +
                                        " rgr.name = rrorgrnm.codergr AND  " +
                                        " rrorgrnm.coderro = rrorfunm.coderro AND  " +
                                        " rrorfunm.codermo = rfu.codermo AND  " +
                                        " rrorfunm.coderfu = rfu.coderfu AND  " +
                                        " rgm.syswfuser = :sysWFUser  ";

        /// <summary>
        /// RightsMap für einen WFUser holen
        /// </summary>
        /// <param name="sysWFUser"></param>
        /// <returns></returns>
        virtual public List<RightsMap> getRightsForWFUser(long sysWFUser)
        {
            List<RightsMap> rightsMapList = new List<RightsMap>();
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysWFUser", Value = sysWFUser });

                rightsMapList = context.ExecuteStoreQuery<RightsMap>(RIGHTSMAP_QUERY, parameters.ToArray()).ToList();
            }
            return rightsMapList;
        }
    }
}