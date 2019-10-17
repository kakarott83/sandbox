using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    public enum VGAxisType
    {
        XAXIS,
        YAXIS
    }
    /// <summary>
    /// Data Access Object for Value Groups
    /// </summary>
    [System.CLSCompliant(true)]
    public class VGDao
    {
        #region constants
        public const int CnstINTERPOLATION_OFF = 0;
        public const int CnstINTERPOLATION_LINEAR = 1;
        public const int CnstINTERPOLATION_MINIMUM = 2;
        public const int CnstINTERPOLATION_MAXIMUM = 3;
        #endregion

        #region Private variables
        private DdOlExtended _context;
        private decimal _xmax;
        private decimal _ymax;
        private decimal _xmin;
        private decimal _ymin;
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructors
        public VGDao(DdOlExtended context)
        {
            _context = context;
        }
        #endregion

        #region Properties

        public decimal xmax
        {
            get { return _xmax; }
            set { _xmax = value; }
        }

        public decimal xmin
        {
            get { return _xmin; }
            set { _xmin = value; }
        }

        public decimal ymax
        {
            get { return _ymax; }
            set { _ymax = value; }
        }

        public decimal ymin
        {
            get { return _ymin; }
            set { _ymin = value; }
        }
        #endregion

        /// <summary>
        /// New Signature for cicvalue without format parameters
        /// Description see other getVGValue-method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="perDate"></param>
        /// <param name="sysVg"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <param name="interpolationMode"></param>
        /// <returns></returns>
        public static decimal deliverVGValue(DdOlExtended context, long sysVg, System.DateTime perDate, string xval, string yval, int interpolationMode)
        {
            return deliverVGValue(context, sysVg, perDate, xval, yval, interpolationMode, "", "");
        }
        private static CacheDictionary<String, decimal> vgCache = CacheFactory<String, decimal>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        /// <summary>
        /// Returns the interpolated value of a value group
        /// </summary>
        /// <param name="context">DB Access Context</param>
        /// <param name="perDate">Validity Date for the value group entries</param>
        /// <param name="sysVg">PK of the value group</param>
        /// <param name="xval">x-axis value to interpolate</param>
        /// <param name="yval">y-axis value to interpolate</param>
        /// <param name="interpolationMode">mode of interpolation: (0=off, 1=linear, 2=use minimum, 3=use maximum)</param>
        /// <param name="formatX">currently unused</param>
        /// <param name="formatY">currently unused</param>
        /// <returns></returns>
        private static decimal deliverVGValue(DdOlExtended context, long sysVg, System.DateTime perDate, string xval, string yval, int interpolationMode, string formatX, string formatY)
        {
            String key = sysVg + "_" + perDate.Year + "-" + perDate.Month + "-" + perDate.Day + "_" + xval + "_" + yval + "_" + interpolationMode + "_";

            if (!vgCache.ContainsKey(key))
            {

                String dayStr = "to_date('" + perDate.Year + "-" + perDate.Month + "-" + perDate.Day + "', 'yyyy-mm-dd')";


                String query = "select cic.cic_common_utils.cicvalue(" + dayStr + "," + sysVg + ",'" + xval + "','" + yval + "'," + interpolationMode + ",'" + formatX + "','" + formatY + "') v from dual";


                decimal rval;
                try
                {
                    rval = context.ExecuteStoreQuery<decimal>(query, null).FirstOrDefault();
                }
                catch (Exception ex)
                {

                    string msg = "VGDao.getVGValue failed for sysVG=" + sysVg + ", x=" + xval + ", y=" + yval;
                    _Log.Error(msg + ex.Message);
                    throw new InvalidOperationException(msg, ex);
                }
                vgCache[key]= rval;
            }
            return vgCache[key];
        }

        public decimal deliverVGValue(long sysVg, System.DateTime perDate, string xval, string yval, int interpolationMode)
        {
            return VGDao.deliverVGValue(_context, sysVg, perDate, xval, yval, interpolationMode);
        }

      
        /// <summary>
        /// Delivers the scale values for the given axis
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
       /* public decimal[] deliverVGScaleValues(long sysVg, System.DateTime perDate, VGAxisType type)
        {
            DbParameter[] Parameters = 
                { 
                        new Devart.Data.Oracle.OracleParameter{ ParameterName = "perDate", Value = perDate}, 
                        new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysvg", Value =sysVg}
                        };

            try
            {
                switch(type)
                {
                    case(VGAxisType.YAXIS):
                        return _context.ExecuteStoreQuery<decimal>("select distinct scalevalue from vgvalid, vgline where vgvalid.sysvg=:pSysvg and vgline.sysvgvalid=vgvalid.sysvgvalid and " 
                            // + " (validfrom<=:perDate  or validfrom is null) and (validuntil>=:perDate or validuntil is null)"
                            + SQL.CheckDate (" :perDate ", "vgvalid") 
                            , Parameters).OrderBy(p => p).ToArray<decimal>();
                        
                    case (VGAxisType.XAXIS):
                        return _context.ExecuteStoreQuery<decimal>("select distinct scalevalue from vgvalid, vgclmn where vgvalid.sysvg=:pSysvg and vgclmn.sysvgvalid=vgvalid.sysvgvalid and " 
                            // + " (validfrom<=:perDate  or validfrom is null) and (validuntil>=:perDate or validuntil is null)"
                            + SQL.CheckDate (" :perDate ", "vgvalid") 
                            , Parameters).OrderBy(p => p).ToArray<decimal>();
                }
                return null;
            }
            catch (Exception ex)
            {
                _Log.Error("VGDao.deliverVGScaleValues failed - no data found " + ex.Message);
                throw new InvalidOperationException("deliverVGScaleValues failed - no data found", ex);
            }
        }*/

        private static CacheDictionary<String, BoundResult> bdCache = CacheFactory<String, BoundResult>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        public void deliverVGBoundaries(long sysVg, System.DateTime perDate)
        {
            String key = sysVg + "_" + perDate.Year + "-" + perDate.Month + "-" + perDate.Day;
            if (!bdCache.ContainsKey(key))
            {
                String dayStr = "to_date('" + perDate.Year + "-" + perDate.Month + "-" + perDate.Day + "', 'yyyy-mm-dd')";
                BoundResult br = _context.ExecuteStoreQuery<BoundResult>("select MAX(TO_NUM(x1)) xmax, MAX(TO_NUM(y1)) ymax,MIN(TO_NUM(x1)) xmin, MIN(TO_NUM(y1))  ymin from CIC.VGVALUES_V where sysvg=" + sysVg + " and "
                    // + " (validfrom<= " + dayStr + "  or validfrom is null) and (validuntil>= " + dayStr + " or validuntil is null)"
                    + SQL.CheckDate(dayStr, "CIC.VGVALUES_V")
                    , null).FirstOrDefault();
                if (br != null)
                {
                    bdCache[key] = br;
                    _xmax = br.xmax;
                    _ymax = br.ymax;
                    _xmin = br.xmin;
                    _ymin = br.ymin;
                    return;
                }

                Devart.Data.Oracle.OracleParameter xmax = new Devart.Data.Oracle.OracleParameter { ParameterName = "xmax", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };
                Devart.Data.Oracle.OracleParameter ymax = new Devart.Data.Oracle.OracleParameter { ParameterName = "ymax", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };
                Devart.Data.Oracle.OracleParameter xmin = new Devart.Data.Oracle.OracleParameter { ParameterName = "xmin", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };
                Devart.Data.Oracle.OracleParameter ymin = new Devart.Data.Oracle.OracleParameter { ParameterName = "ymin", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };

                DbParameter[] Parameters =
                            {
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "perDate", Value = perDate, DbType=System.Data.DbType.Date, OracleDbType = Devart.Data.Oracle.OracleDbType.Date},
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysvg", Value =sysVg},
                                xmax,ymax, xmin, ymin};

                try
                {
                    _context.ExecuteProcedure("CIC_COMMON_UTILS.CICMAXVALUE", Parameters);
                    _xmax = ((System.Decimal)xmax.Value);
                    _ymax = ((System.Decimal)ymax.Value);
                    _xmin = ((System.Decimal)xmin.Value);
                    _ymin = ((System.Decimal)ymin.Value);
                    br = new BoundResult();
                    br.xmax = _xmax;
                    br.xmin = _xmin;
                    br.ymax = _ymax;
                    br.ymin = _ymin;
                    bdCache[key] = br;
                }
                catch (Exception ex)
                {
                    _Log.Error("VGDao.deliverVGBoundaries failed - no data found " + ex.Message);
                    throw new InvalidOperationException("deliverVGBoundaries failed - no data found", ex);
                }
            }

            BoundResult brr = bdCache[key];
            _xmax = brr.xmax;
            _ymax = brr.ymax;
            _xmin = brr.xmin;
            _ymin = brr.ymin;

        }


    }
    public class BoundResult
    {
        public decimal xmin { get; set; }
        public decimal xmax{ get; set; }
        public decimal ymin { get; set; }

        public decimal ymax { get; set; }
    }
}


