using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Interest Rate Data Access Object
    /// </summary>
    public class ZinsDao : IZinsDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string QUERYINTSTRCT = "select INTSTRCT.SYSINTSTRCT, SYSPRPRODUCT,INTSDATE.SYSINTSDATE,METHOD,INTSDATE.VALIDFROM from intstrct,intsdate,prproduct where prproduct.sourcebasis=1 and prproduct.sysintstrct=intstrct.sysintstrct   and intstrct.sysintstrct = intsdate.sysintstrct";
        private const string QUERYINTSTRCTBYID = "select INTSTRCT.SYSINTSTRCT, INTSDATE.SYSINTSDATE,METHOD,INTSDATE.VALIDFROM from intstrct,intsdate where  intstrct.sysintstrct = intsdate.sysintstrct and intstrct.sysintstrct =:psysintstrct";
        private const string QUERYRATE = "select * from intsrate";
        private const string QUERYMATU = "select * from intsmatu";
        private const string QUERYBAND = "select * from intsband";
        private const string QUERYIBOR = "select ibor.name, ibor.syswaehrung, ibordat.*, prproduct.sysprproduct from ibor, ibordat,prproduct where prproduct.sourcebasis=0 and prproduct.sysibor = ibor.sysibor and ibordat.sysibor = ibor.sysibor";
        private const string QUERYPRCLPRINTSET = "select * from PRCLPRINTSET";
        private const string QUERYPRINTSET = "select * from PRINTSET where activeflag=1";
        private const string QUERYPRINTSTEP = "select * from PRINTSTEP";
		private const string QUERYRAPVALUES = "select * from prrapval where sysprrap = :sysprrap and sysprkgroup is null order by score  asc";
        private const string QUERYPRRAPVALUES = "select prrap.* from prrap,prproduct where prproduct.sysprrap=prrap.sysprrap and prproduct.sysprproduct=:sysprproduct";

        /// <summary>
        /// Standard Constructor
        /// Database access Object for Zins
        /// </summary>
        public ZinsDao()
        {

        }

        /// <summary>
        /// Get Interest Rate
        /// </summary>
        /// <returns>Interest Rate List</returns>
        public virtual List<IborDto> getIbor()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<IborDto>(QUERYIBOR, null).ToList();
            }
        }

        /// <summary>
        /// Get Interest Rate
        /// </summary>
        /// <returns>Interest Rate List</returns>
        public virtual List<IntsDto> getIntsrate()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<IntsDto>(QUERYRATE, null).ToList();
            }
        }

        /// <summary>
        /// Get Interest Rates Matured
        /// </summary>
        /// <returns>Interest Rate List</returns>
        public virtual List<IntsDto> getIntsmatu()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<IntsDto>(QUERYMATU, null).ToList();
            }
        }

        /// <summary>
        /// Get Interest Rates Band
        /// </summary>
        /// <returns>Interest Rate List</returns>
        public virtual List<IntsDto> getIntsband()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<IntsDto>(QUERYBAND, null).ToList();
            }
        }

        /// <summary>
        /// Get Interest Rate Strict
        /// </summary>
        /// <returns>Interest Rate List</returns>
        public virtual List<IntstrctDto> getIntstrct()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<IntstrctDto>(QUERYINTSTRCT, null).ToList();
            }
        }

        /// <summary>
        /// Get Interest Rate Strict by SYSINTSTRCT
        /// </summary>
        /// <returns>Interest Rate List</returns>
        public virtual List<IntstrctDto> getIntstrctById(long sysintstrct)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { 
                      new Devart.Data.Oracle.OracleParameter { ParameterName = "psysintstrct", Value = sysintstrct } 
                };
                return ctx.ExecuteStoreQuery<IntstrctDto>(QUERYINTSTRCTBYID, pars).ToList();
            }
        }

        /// <summary>
        /// Get Product Links
        /// </summary>
        /// <returns>Product List</returns>
        public virtual List<PRCLPRINTSETDto> getProductLinks()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<PRCLPRINTSETDto>(QUERYPRCLPRINTSET, null).ToList();
            }
        }

        /// <summary>
        /// Get Interest Rate Groups
        /// </summary>
        /// <returns>Interest Rate Groups List</returns>
        public virtual List<PRINTSETDto> getIntGroups()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<PRINTSETDto>(QUERYPRINTSET, null).ToList();
            }
        }

        /// <summary>
        /// Get Ínterest Rate Steps
        /// </summary>
        /// <returns>Interest Rate Condition Link List</returns>
        public virtual List<InterestConditionLink> getIntSteps()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<InterestConditionLink>(QUERYPRINTSTEP, null).ToList();
            }
        }

        /// <summary>
        /// returns the prrap of the product
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        public virtual PRRAP getPrRap(long sysprproduct)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { 
                      new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprproduct", Value = sysprproduct } 
                };
                return ctx.ExecuteStoreQuery<PRRAP>(QUERYPRRAPVALUES, pars).FirstOrDefault();
            }
        }

        /// <summary>
        /// returns the rapvalues for the rap id
        /// </summary>
        /// <param name="sysprrap"></param>
        /// <returns></returns>
        public virtual List<PRRAPVAL> getRapValues(long sysprrap)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { 
                      new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprrap", Value = sysprrap } 
                };
                return ctx.ExecuteStoreQuery<PRRAPVAL>(QUERYRAPVALUES, pars).ToList();
               
            }
        }


        /// <summary>
        /// returns the rapvalues for the rap id
        /// </summary>
        /// <param name="sysprrap"></param>
        /// <param name="sysprkgroup"></param>
        /// <returns></returns>
        public List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> getRapValues(long sysprrap, long sysprkgroup)
        {

            using (PrismaExtended ctx = new PrismaExtended())
            {

                List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> rval = ctx.ExecuteStoreQuery<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL>("Select * from prrapval where sysprrap = " + sysprrap + " and sysprkgroup = " + sysprkgroup + " order by score  asc ", null).ToList();
                return rval;
            }
        }

        /// <summary>
        /// getRapValByScore
        /// </summary>
        /// <param name="values"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public Cic.OpenOne.Common.Model.Prisma.PRRAPVAL getRapValByScore(List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> values, String score)
        {

            Cic.OpenOne.Common.Model.Prisma.PRRAPVAL rval = new Cic.OpenOne.Common.Model.Prisma.PRRAPVAL();
            Cic.OpenOne.Common.Model.Prisma.PRRAPVAL cval = null;
            rval.VALUE = 0;
            if (values != null)
            {

                if (isNumeric(score))
                {
                    double dScore = Convert.ToDouble(score);
                    double minBand = -1;
                    foreach (Cic.OpenOne.Common.Model.Prisma.PRRAPVAL item in values)
                    {
                        cval = item;
                        double maxBand = Convert.ToDouble(item.SCORE);

                        if (dScore > minBand && dScore <= maxBand)
                        {
                            break;
                        }
                        minBand = maxBand;
                    }
                }
                else
                {
                    String minBand = "-1";
                    foreach (Cic.OpenOne.Common.Model.Prisma.PRRAPVAL item in values)
                    {
                        cval = item;
                        String maxBand = item.SCORE;

                        if (score.CompareTo(minBand) < 0 && score.CompareTo(maxBand) >= 0)
                        {
                            break;
                        }
                        minBand = maxBand;
                    }
                }
                if (cval == null || !cval.VALUE.HasValue)
                    return rval;
                rval.VALUE = cval.VALUE;
                rval.SCORE = cval.SCORE;
                rval.FAKTOR1 = cval.FAKTOR1;
                rval.FAKTOR2 = cval.FAKTOR2;
            }
            return rval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="theValue"></param>
        /// <returns></returns>
        private static bool isNumeric(string theValue)
        {
            try
            {
                Convert.ToDouble(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
