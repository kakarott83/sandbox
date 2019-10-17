using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using System.Collections.Generic;
using System.Linq;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Data Access Object for Insurance Companies
    /// </summary>
    [System.CLSCompliant(true)]
    public class VSPERSONDao
    {
        #region Private variables
        private DdOlExtended _context;

        #endregion

        #region Constructors
        public VSPERSONDao(DdOlExtended context)
        {
            _context = context;
        }
        #endregion


        public PERSONDto getVSPERSON(long sysperson)
        {

            PERSONDto tmp = _context.ExecuteStoreQuery<PERSONDto>("select vs.* from vs where vs.sysperson=" + sysperson, null).First();
            return tmp;
        }

        /// <summary>
        /// Delivers all available insurance companies for the given parameters
        /// </summary>
        /// <param name="sysPrkGroup"></param>
        /// <param name="sysVsArt">kind of insurance</param>
        /// <param name="sysObTyp"></param>
        /// <param name="sysObArt"></param>
        /// <param name="sysKdTyp"></param>
        /// <returns></returns>
        public List<PERSONDto> DeliverAvailableVs(long sysPrkGroup, long sysVsArt, long sysObTyp, long sysObArt, long sysKdTyp)
        {
            List<PERSONDto> PersonDtoList = null;

            try
            {
                

                    //Parameters for query
                    System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrkGroup", Value = sysPrkGroup}, 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysVsArt", Value = sysVsArt},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObTyp", Value = sysObTyp},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObArt", Value = sysObArt}
                            ,new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysKdTyp", Value = sysKdTyp}
                        };

                    //string Query = "SELECT distinct p.*,NVL(ob.needed,0)+NVL(per.needed,0) NEEDED,NVL(ob.disabledflag,0) + NVL(per.disabledflag,0) DISABLED  FROM CIC.PERSON p  left outer join CIC.PRVSOB_V ob on ob.SYSPERSON=p.sysperson  left outer join CIC.PRVSP_V per on per.SYSPERSON=p.sysperson, TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailVs(:sysPrkGroup, :sysVsArt, :sysObTyp, :sysObArt, :sysKdTyp)) t where p.sysperson = t.sysid";
                    //string Query = "SELECT * FROM CIC.PERSON p,TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailVsNew(:sysPrkGroup, :sysVsArt, :sysObTyp, :sysObArt, :sysKdTyp)) t where p.sysperson = t.sysid";
                    string Query = "SELECT * FROM TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailVs(:sysPrkGroup, :sysVsArt, :sysObTyp, :sysObArt, :sysKdTyp))";
                    string Query2 = "select PERSON.SYSPERSON, PERSON.NAME, PERSON.MATCHCODE, PERSON.CODE from CIC.PERSON where SYSPERSON=:t";
                    //_context.PERSON.MergeOption = MergeOption.NoTracking;
                    PersonDtoList = new List<PERSONDto>();
                    List<long> pids = _context.ExecuteStoreQuery<long>(Query, Parameters).ToList();
                    foreach (long pid in pids)
                    {
                       /* PERSONDto person = _context.PERSON.Where<PERSONDto>(p => p.SYSPERSON == pid).FirstOrDefault<PERSONDto>();
                        PersonDtoList.Add(person);*/
                        System.Data.Common.DbParameter[] Parameters2 = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "t", Value = pid}
                        };
                       // double s2 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        PersonDtoList.Add(_context.ExecuteStoreQuery<PERSONDto>(Query2, Parameters2).FirstOrDefault());
                       // double time2 = DateTime.Now.TimeOfDay.TotalMilliseconds - s2;
                    }
                    //PersonDtoList = _context.ExecuteStoreQuery<PERSONDto>(Query, Parameters).ToList();
                
            }
            catch
            {
                throw;
            }


            return PersonDtoList;
        }

    }
}