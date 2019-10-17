using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using System.Collections.Generic;
using System.Linq;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Data Access Object for kinds of insurances
    /// </summary>
    [System.CLSCompliant(true)]
    public class VSARTDao
    {
        #region Private variables
        private DdOlExtended _context;

        #endregion

        #region Constructors
        public VSARTDao(DdOlExtended context)
        {
            _context = context;
        }
        #endregion


        /// <summary>
        /// delivers all available kinds of insurances for the given parameters
        /// </summary>
        /// <param name="sysPrkGroup">Kundengruppe</param>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="sysKdTyp"></param>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        public List<VSARTDto> DeliverAvailableVsArt(long sysPrkGroup, long sysObTyp, long sysObArt, long sysKdTyp, long sysPrProduct)
        {
            List<VSARTDto> VsArtDtoList = null;

            try
            {


                //Parameters for query
                System.Data.Common.DbParameter[] Parameters = 
                        { 
                            
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrkGroup", Value = sysPrkGroup}, 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObTyp", Value = sysObTyp},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObArt", Value = sysObArt}
                            ,new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysKdTyp", Value = sysKdTyp}
                            ,new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrProduct", Value = sysPrProduct}
                        };

                string Query = "SELECT distinct p.*,NVL(ob.needed,0)+NVL(per.needed,0) NEEDED,NVL(ob.disabledflag,0) + NVL(per.disabledflag,0) DISABLED  FROM vsart p  left outer join CIC.PRVSOB_V ob on ob.SYSVSART=p.sysvsart and ob.SYSPRPRODUCT=:sysPrProduct left outer join CIC.PRVSP_V per on per.SYSVSART=p.sysvsart and per.SYSPRPRODUCT=:sysPrProduct, TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailVsArt(:sysPrkGroup, :sysObTyp, :sysObArt, :sysKdTyp)) t where p.sysvsart = t.sysid";
                //"SELECT p.* FROM vsart p,TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailVsArtNew(:sysPrkGroup, :sysObTyp, :sysObArt, :sysKdTyp)) t where p.sysvsart = t.sysid";
                VsArtDtoList = _context.ExecuteStoreQuery<VSARTDto>(Query, Parameters).ToList();

            }
            catch
            {
                throw;
            }


            return VsArtDtoList;
        }


        /// <summary>
        /// Delivers a list of all configured availabilities
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        public List<PRVSDto> DeliverAvailabilities(long sysPrProduct)
        {
            List<PRVSDto> rList = new List<PRVSDto>();

            try
            {


                //Parameters for query
                System.Data.Common.DbParameter[] Parameters = 
                        { 
                           new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrProduct", Value = sysPrProduct}
                        };

                string Query = "SELECT *  FROM CIC.PRVSOB_V where SYSPRPRODUCT=:sysPrProduct";

                rList.AddRange(_context.ExecuteStoreQuery<PRVSDto>(Query, Parameters).ToList());


                System.Data.Common.DbParameter[] Parameters2 = 
                        { 
                           new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrProduct", Value = sysPrProduct}
                        };

                Query = "SELECT *  FROM CIC.PRVSP_V where SYSPRPRODUCT=:sysPrProduct";

                rList.AddRange(_context.ExecuteStoreQuery<PRVSDto>(Query, Parameters2).ToList());

            }
            catch
            {
                throw;
            }


            return rList;
        }

    }


}