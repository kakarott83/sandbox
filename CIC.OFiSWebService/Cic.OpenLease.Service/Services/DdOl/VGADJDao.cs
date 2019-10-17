using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    [System.CLSCompliant(true)]
    public class VGADJDao
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region Constants
        private const string CnstGETVGADJ = "CIC_COMMON_UTILS.GETVGADJ";
        private const string CnstGETAIDAVGADJ = "CIC_COMMON_UTILS.GETAIDAVGADJ";
        #endregion

        /// <summary>
        /// Delivers the RV correction value for the given SYSOBTYP and the current system date
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysObtyp"></param>
        /// <returns></returns>
        public static ADJDto deliverVGAdjValue(DdOlExtended context, long sysObtyp)
        {
            return deliverVGAdjValue(context, sysObtyp, DateTime.Now);
        }

        /// <summary>
        /// Delivers the RV correction value for the given SYSOBTYP and the SYSVG of the attached ValueGroup Table
        /// </summary>
        /// <param name="context">database context</param>
        /// <param name="sysObtyp">PK of the OBTYP Table</param>
        /// <param name="perDate">validation date of the correction table</param>
        /// <returns></returns>
        public static ADJDto deliverVGAdjValue(DdOlExtended context, long sysObtyp, System.DateTime perDate)
        {
            return MyDeliverAdjValue(context, sysObtyp, perDate, CnstGETVGADJ);
        }

        /// <summary>
        /// Delivers the AIDA RV correction value for the given SYSOBTYP and the current system date
        /// the brand will be used from the parent-obtyp with importtable='ETGMAKE'
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysObtyp"></param>
        /// <returns></returns>
        public static ADJDto deliverAIDAVGAdjValue(DdOlExtended context, long sysObtyp)
        {
            return deliverAIDAVGAdjValue(context, sysObtyp, DateTime.Now);
        }

        /// <summary>
        /// Delivers the AIDA RV correction value for the given SYSOBTYP and the SYSVG of the attached ValueGroup Table
        /// the brand will be used from the parent-obtyp with importtable='ETGMAKE'
        /// </summary>
        /// <param name="context">database context</param>
        /// <param name="sysObtyp">PK of the OBTYP Table</param>
        /// <param name="perDate">validation date of the correction table</param>
        /// <returns></returns>
        public static ADJDto deliverAIDAVGAdjValue(DdOlExtended context, long sysObtyp, System.DateTime perDate)
        {
            return MyDeliverAIDAAdjValue(context, sysObtyp, perDate);
        }

        /// <summary>
        /// Delivers the AIDA WR correction value for the given SYSOBTYP and the SYSVG of the attached ValueGroup Table
        /// the brand will be used from the parent-obtyp with importtable='ETGMAKE'
        /// </summary>
        /// <param name="context">database context</param>
        /// <param name="sysObtyp">PK of the OBTYP Table</param>
        /// <param name="perDate">validation date of the correction table</param>
        /// <returns></returns>
        public static ADJDto deliverVGWRAdjValue(DdOlExtended context, long sysObtyp, System.DateTime perDate)
        {
            return MyDeliverVGWRAdjValue(context, sysObtyp, perDate);
        }

        /// <summary>
        /// Delivers the specified correction value for the given SYSOBTYP and the SYSVG of the attached ValueGroup Table
        /// </summary>
        /// <param name="context">database context</param>
        /// <param name="sysObtyp">PK of the OBTYP Table</param>
        /// <param name="perDate">validation date of the correction table</param>
        /// <param name="procedure">Stored Procedure Name</param>
        /// <returns></returns>
        private static ADJDto MyDeliverAdjValue(DdOlExtended context, long sysObtyp, System.DateTime perDate, string procedure)
        {
            //Execute Query
            ADJDto rval = new ADJDto();
            
            try
            {
                string perDateStr = perDate.ToString("yyyy-MM-dd");
            
                Devart.Data.Oracle.OracleParameter p1 = new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysObtyp", Value = sysObtyp, DbType = System.Data.DbType.Decimal, Direction = System.Data.ParameterDirection.Input };
                Devart.Data.Oracle.OracleParameter p2 = new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate, DbType = System.Data.DbType.DateTime, Direction = System.Data.ParameterDirection.Input };
                Devart.Data.Oracle.OracleParameter pValue = new Devart.Data.Oracle.OracleParameter { ParameterName = "pValue", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };
                Devart.Data.Oracle.OracleParameter pSysVg = new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysVg", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };
                

                context.ExecuteProcedure(procedure, p1, p2, pValue, pSysVg);
                rval.adjvalue = ((System.Decimal)pValue.Value);
                rval.sysvg = Decimal.ToInt64(((System.Decimal)pSysVg.Value));
            }
            catch (Exception ex)
            {
                _Log.Error("VGADJDao.getVGAdjValue failed: " + ex.Message);
                throw new InvalidOperationException("getVGValue failed",ex);
            }
            return rval;
        }

        private class VGInfo
        {
            public decimal value {get;set;}
            public long sysvg {get;set;}
        }

        private static ADJDto MyDeliverAIDAAdjValue(DdOlExtended context, long sysObtyp, System.DateTime perDate)
        {
            //Execute Query
            ADJDto rval = new ADJDto();

            try
            {
                string perDateStr = perDate.ToString("yyyy-MM-dd");

                long sysbrand = context.ExecuteStoreQuery<long>("SELECT sysbrand  FROM (select sysbrand,importtable from obtyp   connect by PRIOR sysobtypp = sysobtyp start with sysobtyp=" + sysObtyp + " order by level desc ) WHERE importtable='ETGMAKE'", null).FirstOrDefault();

                object[] parameters = new object[]{
                    new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysObtyp", Value = sysObtyp, DbType = System.Data.DbType.Decimal, Direction = System.Data.ParameterDirection.Input }
                    ,new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate, DbType = System.Data.DbType.DateTime, Direction = System.Data.ParameterDirection.Input }
                    ,new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysBrand", Value = sysbrand, DbType = System.Data.DbType.Decimal, Direction = System.Data.ParameterDirection.Input }
                };


                rval.adjvalue = 0;
                VGInfo info = context.ExecuteStoreQuery<VGInfo>("select vgadj.value value,vgavg.sysvg sysvg from vgtype,vg,vgadjvalid,vgadjtrg,vgavg,vgadj,obtyp " 
                    + "  where  vgtype.sysvgtype=vg.sysvgtype and   vg.sysvg=vgavg.sysvg and vgtype.name='RW' and upper(vgadjvalid.name) not like '%CRV%' and obtyp.sysobtyp=:pSysObtyp " 
                    + " and vgavg.sysvg=obtyp.sysvgrw and  vgadjtrg.sysbrand = :pSysBrand and   vgadjvalid.sysvgadjvalid=vgadjtrg.sysvgadjvalid and vgavg.sysvgadjvalid=vgadjvalid.sysvgadjvalid " 
                    + " and vgadj.sysvgadjtrg=vgadjtrg.sysvgadjtrg and vgadj.sysvgavg=vgavg.sysvgavg and " 
                    // + " (vgadjvalid.validfrom is null or vgadjvalid.validfrom<=:perDate) and  (vgadjvalid.validuntil is null or vgadjvalid.validuntil>=:perDate)"
                    + SQL.CheckDate (" :perDate ", "vgadjvalid") 
                    , parameters).FirstOrDefault();
                if (info == null)
                {
                    info = context.ExecuteStoreQuery<VGInfo>("select 0 value,sysvgrw sysvg from obtyp where obtyp.sysobtyp=" + sysObtyp, null).FirstOrDefault();
                }
                
                rval.adjvalue = info.value;
                rval.sysvg = info.sysvg;
            }
            catch (Exception ex)
            {
                _Log.Error("VGADJDao.getVGAdjValue failed: " + ex.Message);
                throw new InvalidOperationException("getVGValue failed", ex);
            }
            return rval;
        }
        private static ADJDto MyDeliverVGWRAdjValue(DdOlExtended context, long sysObtyp, System.DateTime perDate)
        {
            //Execute Query
            ADJDto rval = new ADJDto();

            try
            {
                string perDateStr = perDate.ToString("yyyy-MM-dd");

                long sysbrand = context.ExecuteStoreQuery<long>("SELECT sysbrand  FROM (select sysbrand,importtable from obtyp   connect by PRIOR sysobtypp = sysobtyp start with sysobtyp=" + sysObtyp + " order by level desc ) WHERE importtable='ETGMAKE'", null).FirstOrDefault();

                object[] parameters = new object[]{
                    new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysObtyp", Value = sysObtyp, DbType = System.Data.DbType.Decimal, Direction = System.Data.ParameterDirection.Input }
                    ,new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate, DbType = System.Data.DbType.DateTime, Direction = System.Data.ParameterDirection.Input }
                    ,new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysBrand", Value = sysbrand, DbType = System.Data.DbType.Decimal, Direction = System.Data.ParameterDirection.Input }
                };


                rval.adjvalue = 0;
                VGInfo info = context.ExecuteStoreQuery<VGInfo>("select vgadj.value value,vgavg.sysvg sysvg from vgtype,vg,vgadjvalid,vgadjtrg,vgavg,vgadj,obtyp " 
                    + "  where  vgtype.sysvgtype=vg.sysvgtype and   vg.sysvg=vgavg.sysvg and vgtype.name='Wartung' and upper(vgadjvalid.name) not like '%CRV%' and obtyp.sysobtyp=:pSysObtyp " 
                    + " and vgavg.sysvg=obtyp.sysvgwr and  vgadjtrg.sysbrand = :pSysBrand and   vgadjvalid.sysvgadjvalid=vgadjtrg.sysvgadjvalid and vgavg.sysvgadjvalid=vgadjvalid.sysvgadjvalid " 
                    + " and vgadj.sysvgadjtrg=vgadjtrg.sysvgadjtrg and vgadj.sysvgavg=vgavg.sysvgavg and " 
                    // + " (vgadjvalid.validfrom is null or vgadjvalid.validfrom<=:perDate) and  (vgadjvalid.validuntil is null or vgadjvalid.validuntil>=:perDate)"
                    + SQL.CheckDate (" :perDate ", "vgadjvalid") 
                    , parameters).FirstOrDefault();
                if (info == null)
                {
                    info = context.ExecuteStoreQuery<VGInfo>("select 0 value,sysvgwr sysvg from obtyp where obtyp.sysobtyp=" + sysObtyp, null).FirstOrDefault();
                }

                rval.adjvalue = info.value;
                rval.sysvg = info.sysvg;
            }
            catch (Exception ex)
            {
                _Log.Error("VGADJDao.deliverVGWRAdjValue failed: " + ex.Message);
                throw new InvalidOperationException("deliverVGWRAdjValue failed", ex);
            }
            return rval;
        }


    }


}


