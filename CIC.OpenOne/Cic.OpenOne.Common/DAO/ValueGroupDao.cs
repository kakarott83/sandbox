using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// data from a view for the cicvalues function, which contains a rectangle of  values/coordinates for the stored value group values
    /// the view contains the value group id (sysvg, the x/y formats, the validity date range, the stored x/y scale (colscale, linescale) and the value/coordinate rectangle:
    /// given x/y-coordinate is on top left position
    /// [x1,y1,v1] - [x2,y1,v2]
    /// [x1,y2,v3] - [x2,y2,v4]
    /// all positions/values are used in the interpolation function to interpolate the linear value, if the given x/y position is not stored in the value group table
    /// </summary>
    class VGValueInfo
    {
        private String _X1, _X2, _Y1, _Y2, _COLSCALE, _LINESCALE;
        private double _X1N, _X2N, _Y1N, _Y2N;

        public long SYSVG { get; set; }
        public String XFORMAT { get; set; }
        public String YFORMAT { get; set; }
        public DateTime VALIDFROM { get; set; }
        public DateTime VALIDUNTIL { get; set; }
        public String LINESCALE { get { return _LINESCALE; } set { _LINESCALE = value; } }
        public String COLSCALE { get { return _COLSCALE; } set { _COLSCALE = value; } }
        public double? V1 { get; set; }
        public double? V2 { get; set; }
        public double? V3 { get; set; }
        public double? V4 { get; set; }
        public String X1 { get { return _X1; } set { _X1 = value; _X1N = 0; double.TryParse(value, out _X1N); } }
        public String X2 { get { return _X2; } set { _X2 = value; _X2N = 0; double.TryParse(value, out _X2N); } }
        public String Y1 { get { return _Y1; } set { _Y1 = value; _Y1N = 0; double.TryParse(value, out _Y1N); } }
        public String Y2 { get { return _Y2; } set { _Y2 = value; _Y2N = 0; double.TryParse(value, out _Y2N); } }
        public double X1N { get { return _X1N; } }
        public double Y1N { get { return _Y1N; } }
        public double X2N { get { return _X2N; } }
        public double Y2N { get { return _Y2N; } }
        public double LINESCALEN
        {
            get
            {
                double rval = 0;
                double.TryParse(_LINESCALE, out rval);
                return rval;
            }
        }
        public double COLSCALEN
        {
            get
            {
                double rval = 0;
                double.TryParse(_COLSCALE, out rval);
                return rval;
            }
        }
    }

    /// <summary>
    /// ValueGroupDao-Klasse
    /// </summary>
    public class ValueGroupDao : IVGDao
    {
        private const String VGQUERY = "select * from  CIC.VGVALUES_V where sysvg=:sysvg order by x1,y1";
        private static CacheDictionary<long, List<VGValueInfo>> vgCache = CacheFactory<long, List<VGValueInfo>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private DateTime nullDate = new DateTime(1800, 1, 1);
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// ValueGroupDao-Konstruktor
        /// </summary>
        public ValueGroupDao()
        {
        }

        /// <summary>
        /// interpolates linear between two scalar values
        /// </summary>
        /// <param name="fx0">lower function  value</param>
        /// <param name="fx1">upper function value</param>
        /// <param name="x0">lower x boundary</param>
        /// <param name="x1">upper x boundary</param>
        /// <param name="x">x-pos to interpolate</param>
        /// <param name="m">interpolation mode [0=return fx0, 1=linear interpolation, 2=return fx0, 3=returnfx1]</param>
        /// <returns></returns>
        private static double interpolate(double? fx0, double? fx1, String x0, String x1, String x, int m)
        {
            switch (m)
            {
                case 0://no interpolation, return lower boundary of 
                    return fx0.Value;
                case 2:// return lower function value boundary and check bounds
                    return (!fx0.HasValue ? fx1.Value : fx0.Value);
                case 3://return upper function value boundary and check bounds
                    return (!fx1.HasValue ? fx0.Value : fx1.Value);
                default://interpolate function value linear

                    if (x1 == null || x1.Equals(x0) || x0.Equals(x))
                    {
                        if (!fx0.HasValue) return 0;
                        else return fx0.Value;
                    }
                    else if (x.Equals(x1))
                    {
                        if (!fx1.HasValue) return 0;
                        else return fx1.Value;
                    }
                    else
                    {
                        if (!fx0.HasValue || !fx1.HasValue) return 0;
                        return fx0.Value + ((fx1.Value - fx0.Value) / (double.Parse(x1, System.Globalization.CultureInfo.InvariantCulture) - double.Parse(x0, System.Globalization.CultureInfo.InvariantCulture))) * (double.Parse(x, System.Globalization.CultureInfo.InvariantCulture) - double.Parse(x0, System.Globalization.CultureInfo.InvariantCulture));
                    }
            }
        }

        private List<VGValueInfo> getVGData(long sysVg)
        {
            if (!vgCache.ContainsKey(sysVg))
            {
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvg", Value = sysVg });


                    vgCache[sysVg] = ctx.ExecuteStoreQuery<VGValueInfo>(VGQUERY, parameters.ToArray()).ToList();
                }

            }
            return vgCache[sysVg];
        }

        private List<VGValueInfo> getVGData(long sysVg, DateTime perDate)
        {
            List<VGValueInfo> vgData = getVGData(sysVg);
            vgData = (from v in vgData
                      where (v.VALIDFROM == null || v.VALIDFROM <= perDate || v.VALIDFROM <= nullDate)
                               && (v.VALIDUNTIL == null || v.VALIDUNTIL >= perDate || v.VALIDUNTIL <= nullDate)
                      select v).ToList<VGValueInfo>();
            return vgData;
        }

        /// <summary>
        /// getVGValue
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <param name="interpolationMode"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public double getVGValue(long sysVg, DateTime perDate, string xval, string yval, VGInterpolationMode interpolationMode, plSQLVersion version)
        {

            List<VGValueInfo> vgData = getVGData(sysVg, perDate);

            double xvalue = 0;
            bool xisNumber = double.TryParse(xval, out xvalue);
            if (!xisNumber)
                xvalue = Double.MinValue;//avoid a match in the query for numerical comparison then
            double yvalue = 0;
            bool yisNumber = double.TryParse(yval, out yvalue);
            if (!yisNumber)
                yvalue = Double.MinValue;//avoid a match in the query for numerical comparison then
            VGValueInfo vgInfo = null;

            if (version == plSQLVersion.V1)
            {
                //V1:
                //select INTERPOLATE(INTERPOLATE(v1,v2, x1, x2,xval,interpolation),INTERPOLATE(v3,v4,x1,x2,xval,interpolation), y1,y2,yval ,interpolation) into retVal from CIC.VGVALUES_V where sysvg=pSysvg  and (TO_NUM(x1)<TO_NUM(xval) or x1=xval) and (TO_NUM(x2)>=TO_NUM(xval)) and (TO_NUM(y1)<TO_NUM(yval) or y1=yval) and (TO_NUM(y2)>=TO_NUM(yval) or y2 is null)  and ROWNUM<=1  and (validfrom<=perDate  or validfrom is null) and (validuntil>=perDate or validuntil is null);
                //(TO_NUM(x1)<TO_NUM(xval) or x1=xval) and (TO_NUM(x2)>=TO_NUM(xval)) and (TO_NUM(y1)<TO_NUM(yval) or y1=yval) and (TO_NUM(y2)>=TO_NUM(yval) or y2 is null)  and ROWNUM<=1
                vgInfo = (from v in vgData
                          where (v.X1N < xvalue || v.X1.Equals(xval)) && (v.X2N >= xvalue) &&
                                (v.Y1N < yvalue || v.Y1.Equals(yval)) && (v.Y2N >= yvalue || v.Y2 == null)
                          select v).FirstOrDefault();
            }
            else if(version==plSQLVersion.V2)
            {
                //V2:
                //select INTERPOLATE(INTERPOLATE(v1,v2, x1, x2,xval,interpolation),INTERPOLATE(v3,v4,x1,x2,xval,interpolation), y1,y2,yval ,interpolation) into retVal from CIC.VGVALUES_V where sysvg=pSysvg  and (TO_NUM(x1)<=TO_NUM(xval) or x1=xval) and (TO_NUM(x2)>TO_NUM(xval) or x2 is null ) and (TO_NUM(y1)<=TO_NUM(yval) or y1=yval) and (TO_NUM(y2)>TO_NUM(yval) or y2 is null)  and ROWNUM<=1  and (validfrom<=perDate  or validfrom is null or validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and (validuntil>=perDate or validuntil is null or validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'));
                // (TO_NUM(x1)<=TO_NUM(xval) or x1=xval) and (TO_NUM(x2)>TO_NUM(xval) or x2 is null ) and (TO_NUM(y1)<=TO_NUM(yval) or y1=yval) and (TO_NUM(y2)>TO_NUM(yval) or y2 is null)  and ROWNUM<=1 
                vgInfo = (from v in vgData
                          where (v.X1N <= xvalue || v.X1.Equals(xval)) && (v.X2N > xvalue || v.X2 == null)
                             && (v.Y1N <= yvalue || v.Y1.Equals(yval)) && (v.Y2N > yvalue || v.Y2 == null)
                          select v).FirstOrDefault();
            }
         
            if (vgInfo == null)
            {
                string msg = "VGDao.getVGValue failed for sysVG=" + sysVg + ", x=" + xval + ", y=" + yval + ", interpolationMode: " + interpolationMode + " - no data found!";
                _log.Error(msg);
                throw new Exception("Wertegruppe sysVg="+sysVg+": für x=" + xval + ", y=" + yval+" keinen Wert gefunden.");
            }

            try
            {
                if (interpolationMode == VGInterpolationMode.NONE || interpolationMode == VGInterpolationMode.MIN)
                    return vgInfo.V1.Value;

                if (interpolationMode == VGInterpolationMode.MAX)
                    return vgInfo.V2.HasValue?vgInfo.V2.Value:vgInfo.V1.Value;

                double rval = 0;
                if (interpolationMode == VGInterpolationMode.HORIZONTAL)
                {
                    rval = interpolate(vgInfo.V1, vgInfo.V2, vgInfo.X1, vgInfo.X2, xval, (int)VGInterpolationMode.LINEAR);
                }
                else if (interpolationMode == VGInterpolationMode.VERTICAL)
                {
                    rval = interpolate(vgInfo.V1, vgInfo.V3, vgInfo.Y1, vgInfo.Y2, yval, (int)VGInterpolationMode.LINEAR);
                }
                else
                {
                    double i1 = interpolate(vgInfo.V1, vgInfo.V2, vgInfo.X1, vgInfo.X2, xval, (int)interpolationMode);
                    double i2 = interpolate(vgInfo.V3, vgInfo.V4, vgInfo.X1, vgInfo.X2, xval, (int)interpolationMode);

                    rval = interpolate(i1, i2, vgInfo.Y1, vgInfo.Y2, yval, (int)interpolationMode);
                }

                return Math.Round(rval, 5);
            }
            catch (Exception ex)
            {
                string msg = "VGDao.getVGValue interpolation failed for sysVG=" + sysVg + ", x=" + xval + ", y=" + yval + ", interpolationMode: " + interpolationMode + " - no data found!";
                _log.Error(msg + ex.Message);
                throw new InvalidOperationException("Wertegruppe sysVg=" + sysVg + ": für x=" + xval + ", y=" + yval + ", interpolationMode: " + interpolationMode + " keinen Wert gefunden.",ex);
            }
        }

        /// <summary>
        /// getVGValue
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <param name="interpolationMode"></param>
        /// <returns></returns>
        public double getVGValue(long sysVg, DateTime perDate, string xval, string yval, VGInterpolationMode interpolationMode)
        {
            return getVGValue(sysVg, perDate, xval, yval, interpolationMode, plSQLVersion.V2);
        }

        /// <summary>
        /// getVGScaleValues
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public String[] getVGScaleValues(long sysVg, DateTime perDate, VGAxisType type)
        {
            List<VGValueInfo> vgData = getVGData(sysVg, perDate);

            switch (type)
            {
                case (VGAxisType.YAXIS):
                    return (from v in vgData
                            select v.LINESCALE).Distinct().ToArray();

                case (VGAxisType.XAXIS):
                    return (from v in vgData
                            select v.COLSCALE).Distinct().ToArray();
            }
            return null;
        }

        /// <summary>
        /// getVGBoundaries
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public VGBoundaries getVGBoundaries(long sysVg, DateTime perDate)
        {
            List<VGValueInfo> vgData = getVGData(sysVg, perDate);
            var t = (from v in vgData
                     select v);
            VGBoundaries rval = new VGBoundaries();
            rval.xmax = t.Max(v => v.X1N);
            rval.xmin = t.Min(v => v.X1N);
            rval.ymax = t.Max(v => v.Y1N);
            rval.ymin = t.Min(v => v.Y1N);
            return rval;
        }

        /// <summary>
        /// Delivers the the value group boundaries
        /// </summary>
        /// <param name="sysvg"></param>
        /// <param name="perDate"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        public void checkBoundaries(long sysvg, DateTime perDate, ref double xval, ref double yval)
        {
            VGBoundaries bounds = getVGBoundaries(sysvg, perDate);

            if (xval < bounds.xmin) xval = bounds.xmin;
            if (xval > bounds.xmax) xval = bounds.xmax;
            if (yval < bounds.ymin) yval = bounds.ymin;
            if (yval > bounds.ymax) yval = bounds.ymax;
        }

        /// <summary>
        /// getVGValueSaldo
        /// </summary>
        /// <param name="sysvg"></param>
        /// <param name="saldo"></param>
        /// <param name="lz"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public double getVGValueSaldo(long sysvg, double saldo, long lz, DateTime perDate)
        {
            List<VGValueInfo> vgData = getVGData(sysvg, perDate);
            double maxline = (from v in vgData
                              where v.LINESCALEN <= saldo
                              select v.LINESCALEN).Max();
            double maxcol = (from v in vgData
                             where v.COLSCALEN <= lz
                             select v.COLSCALEN).Max();
            return (from v in vgData
                    where v.LINESCALEN <= saldo
                    && v.COLSCALEN <= lz
                    && v.LINESCALEN == maxline
                    && v.COLSCALEN == maxcol
                    select v.V1.Value).FirstOrDefault();

            //const String VGQUERYSALDO = "SELECT V1, ValidFrom, ValidUntil  from cic.VGVALUES_V  matrix where matrix.sysvg =:sysvg and :Saldo >= linescale
            //and linescale = (select max (linescale) from cic.VGVALUES_V maxline where matrix.sysvg=maxline.sysvg and linescale <= :Saldo) 
            //and :LZ >= colscale 
            //and colscale = (select max (colscale) from cic.VGVALUES_V maxline where matrix.sysvg=maxline.sysvg and colscale <= :LZ)";
        }

        /// <summary>
        /// getVGValuePraemie
        /// </summary>
        /// <param name="sysvg"></param>
        /// <param name="lz"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public double getVGValuePraemie(long sysvg, long lz, DateTime perDate)
        {
            List<VGValueInfo> vgData = getVGData(sysvg, perDate);
            return (from v in vgData
                    where v.COLSCALEN <= lz
                    orderby v.COLSCALEN descending
                    select v.V1.Value).FirstOrDefault();
        }


        public virtual VgDto getVg(long sysvg)
        {
            VgDto vgdto = new VgDto();
            using (PrismaExtended ctx = new PrismaExtended())
            {
                VG vg = (from v in ctx.VG
                         where v.SYSVG == sysvg
                         select v).FirstOrDefault();
                if (vg != null)
                {
                    vgdto.sysVgDto = vg.SYSVG;
                    vgdto.name = vg.NAME;
                    vgdto.sysvgtype = vg.SYSVGTYPE;
                    vgdto.description = vg.DESCRIPTION;
                    vgdto.mappingext = vg.MAPPINGEXT;
                    return vgdto;
                }
            }
            return null;
        }
    }
}