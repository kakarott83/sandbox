using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Extension;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DTO;


namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Axentyp der Wertetabelle
    /// </summary>
    public enum VGAxisType
    {
        /// <summary>
        /// X-Achse
        /// </summary>
        XAXIS,
        /// <summary>
        /// Y-Achse
        /// </summary>
        YAXIS
    }

    /// <summary>
    /// Interpolation Mode for calculation of value groups
    /// </summary>
    public enum VGInterpolationMode
    {
        /// <summary>
        /// keine Interpolation
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Lineare Interpolation
        /// </summary>
        LINEAR = 1,
        /// <summary>
        /// Minimale Interpolation
        /// </summary>
        MIN = 2,
        /// <summary>
        /// Maximale Interpolation
        /// </summary>
        MAX = 3,
        /// <summary>
        /// Interpolate horizontal, when just one y-value is present but different x-values
        /// </summary>
        HORIZONTAL = 4,
        /// <summary>
        /// Interpolate vertical, when just one x-value is present but different y-values
        /// </summary>
        VERTICAL = 5
    }

   
    /// <summary>
    /// Data Access Object for Value Groups
    /// </summary>
    [System.CLSCompliant(true)]
    public class VGDao : IVGDao
    {
        const String GETVGDATA = "SELECT * FROM CIC.VGVALUES_V WHERE sysvg=:sysvg";
        const String QUERYYAXIS = "select distinct scalevalue from vgvalid, vgline where vgvalid.sysvg=:pSysvg and vgline.sysvgvalid=vgvalid.sysvgvalid and (validfrom<=:perDate  or validfrom is null   or validfrom=to_date('01.01.0111' , 'dd.MM.yyyy') ) and (validuntil>=:perDate or validuntil is null  or validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))";

        const String QUERYXAXIS = "select distinct scalevalue from vgvalid, vgclmn where vgvalid.sysvg=:pSysvg and vgclmn.sysvgvalid=vgvalid.sysvgvalid and (validfrom<=:perDate  or validfrom is null or validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and (validuntil>=:perDate or validuntil is null or validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))";

        const String VGQUERYSALDO = "SELECT V1, ValidFrom, ValidUntil from cic.VGVALUES_V  matrix where matrix.sysvg =:sysvg and :Saldo >= linescale and linescale = (select max (linescale) from cic.VGVALUES_V maxline where matrix.sysvg=maxline.sysvg and linescale <= :Saldo) and :LZ >= colscale and colscale = (select max (colscale) from cic.VGVALUES_V maxline where matrix.sysvg=maxline.sysvg and colscale <= :LZ)";

        const String VGQUERYPRAEMIE = "SELECT V1, ValidFrom, ValidUntil, colscale lz from cic.VGVALUES_V  matrix where matrix.sysvg =:sysvg and 'Prämiensatz' = linescale";
        private DateTime nullDate = new DateTime(1800, 1, 1);

        

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Konstruktor
        /// </summary>
        public VGDao()
        {
        }

        /// <summary>
        /// New Signature for cicvalue without format parameters
        /// Description see other getVGValue-method
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <param name="interpolationMode"></param>
        /// <returns></returns>
        public virtual double getVGValue(long sysVg, DateTime perDate, string xval, string yval, VGInterpolationMode interpolationMode)
        {
            return getVGValue(sysVg, perDate, xval, yval, interpolationMode, plSQLVersion.V1);
        }

        /// <summary>
        /// New Signature for cicvalue without format parameters
        /// Description see other getVGValue-method
        /// </summary>
        /// <param name="perDate"></param>
        /// <param name="sysVg"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <param name="interpolationMode"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public virtual double getVGValue(long sysVg, DateTime perDate, string xval, string yval, VGInterpolationMode interpolationMode, plSQLVersion version)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                DbParameter[] pars = 
                        { 
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "perDate", Value = perDate}, 
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysvg", Value =sysVg},
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "xval", Value = xval}, 
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "yval", Value = yval}, 
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "interpolation", Value= interpolationMode} ,
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "formatX", Value = ""} ,
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "formatY", Value = ""} ,
                                new Devart.Data.Oracle.OracleParameter{ OracleDbType = Devart.Data.Oracle.OracleDbType.Number, Direction = System.Data.ParameterDirection.ReturnValue} 
                        };
                //Execute Query
                double rval;
                try
                {
                    object v = null;

                    switch (version)
                    {
                        case plSQLVersion.V1:
                            v = ((ObjectContext)ctx).ExecuteFunction("CIC_COMMON_UTILS.CICVALUE", pars);
                            break;
                        case plSQLVersion.V2:
                            v = ((ObjectContext)ctx).ExecuteFunction("CIC_COMMON_UTILS.CICVALUE2", pars);
                            break;
                    }
                    if (v == null || v.ToString().Equals(String.Empty))
                    {
                        _log.Warn("VGDao.getVGValue returned no value for sysVG=" + sysVg + ", x=" + xval + ", y=" + yval + ", interpolationMode: " + interpolationMode);
                        return 0;
                    }
                    rval = Convert.ToDouble(v);

                }
                catch (InvalidOperationException ex)
                {
                    String exmsg = ex.Message;
                    if (exmsg.IndexOf("no data found") > -1)
                        exmsg = "no data found";

                    string msg = "VGDao.getVGValue failed for sysVG=" + sysVg + ", x=" + xval + ", y=" + yval + " - ";

                    _log.Error(msg + exmsg);
                    throw new Exception("Wertegruppe sysVg=" + sysVg + ": für x=" + xval + ", y=" + yval + " keinen Wert gefunden.");
                }
                catch (Exception ex)
                {
                    String exmsg = ex.Message;
                    if (exmsg.IndexOf("no data found") > -1)
                        exmsg = "no data found";
                    string msg = "VGDao.getVGValue failed for sysVG=" + sysVg + ", x=" + xval + ", y=" + yval + " - ";
                    _log.Error(msg + exmsg);
                    throw new InvalidOperationException("Wertegruppe sysVg=" + sysVg + ": für x=" + xval + ", y=" + yval + " keinen Wert gefunden.");
                }
                return rval;
            }
        }

        /// <summary>
        /// Abfrage nach Stas 
        /// Liest die Zinssätze nach Produkt ID, Saldo und Laufzeit aus.
        /// Und berücksichtigt das Gültigkeitsdatum
        /// </summary>
        /// <param name="perDate">Aktuelles Datum</param>
        /// <param name="sysvg">Wertegruppen ID</param>
        /// <param name="Saldo">Saldo</param>
        /// <param name="lz">Laufzeit</param>
        /// <returns>Zinssatz</returns>
        public virtual double getVGValueSaldo(long sysvg, double Saldo, long lz, DateTime perDate)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvg", Value = sysvg });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "Saldo", Value = Saldo });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "LZ", Value = lz });

                List<VGStasData> Data = ctx.ExecuteStoreQuery<VGStasData>(VGQUERYSALDO, parameters.ToArray()).ToList();

                double v = (from a in Data
                            where (a.ValidFrom == null || a.ValidFrom <= perDate || a.ValidFrom <= nullDate)
                               && (a.ValidUntil == null || a.ValidUntil >= perDate || a.ValidUntil <= nullDate)
                            select a.V1).FirstOrDefault();
                return v;
            }
        }

        /// <summary>
        /// Abfrage nach Stas 
        /// Liest die Prämienzinssätze nach Produkt ID und Laufzeit aus.
        /// Und berücksichtigt das Gültigkeitsdatum
        /// </summary>
        /// <param name="perDate">Aktuelles Datum</param>
        /// <param name="sysvg">Wertegruppen ID</param>
        /// <param name="lz">Laufzeit</param>
        /// <returns>Zinssatz</returns>
        public virtual double getVGValuePraemie(long sysvg, long lz, DateTime perDate)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvg", Value = sysvg });

                List<VGStasData> Data = ctx.ExecuteStoreQuery<VGStasData>(VGQUERYPRAEMIE, parameters.ToArray()).ToList();

                double v = (from a in Data
                            where (a.ValidFrom == null || a.ValidFrom <= perDate || a.ValidFrom <= nullDate)
                               && (a.ValidUntil == null || a.ValidUntil >= perDate || a.ValidUntil <= nullDate)
                               && (lz >= a.lz)
                            orderby a.lz descending
                            select a.V1).FirstOrDefault();
                return v;
            }
        }

        /// <summary>
        /// New Signature for cicvalue without format parameters
        /// Description see other getVGValue-method
        /// </summary>
        /// <param name="perDate"></param>
        /// <param name="sysVg"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <param name="interpolationMode"></param>
        /// <returns></returns>
        public virtual double getVGValueExt(long sysVg, DateTime perDate, string xval, string yval, VGInterpolationMode interpolationMode)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                double rval;
                try
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvg", Value = sysVg });
                    List<VGValuesDto> Data = ctx.ExecuteStoreQuery<VGValuesDto>(GETVGDATA, parameters.ToArray()).ToList();

                    if (Data == null)
                    {
                        _log.Warn("VGDao.getVGValue returned no value for sysVG=" + sysVg + ", x=" + xval + ", y=" + yval + ", interpolationMode: " + interpolationMode);
                        return 0;
                    }

                    double? v = null;

                    switch (interpolationMode)
                    {
                        case VGInterpolationMode.NONE:
                            v = (from a in Data
                                 where a.LineScale == xval && a.ColScale == yval
                                 && (a.ValidFrom == null || a.ValidFrom <= perDate || a.ValidFrom <= nullDate)
                                 && (a.ValidUntil == null || a.ValidUntil >= perDate || a.ValidUntil <= nullDate)
                                 select a.V1).FirstOrDefault();
                            break;
                        case VGInterpolationMode.MIN:
                            //v = (from a in Data
                            //        where a.LineScale <= xval && a.ColScale <= yval
                            //        && (a.ValidFrom == null || a.ValidFrom <= perDate || a.ValidFrom <= nullDate)
                            //        && (a.ValidUntil == null || a.ValidUntil >= perDate || a.ValidUntil <= nullDate)
                            //        select a.V1).FirstOrDefault();
                            break;
                        case VGInterpolationMode.MAX:
                            break;
                        case VGInterpolationMode.LINEAR:
                            break;
                    }

                    rval = Convert.ToDouble(v);

                }
                catch (InvalidOperationException ex)
                {
                    string msg = "VGDao.getVGValue failed for sysVG=" + sysVg + ", x=" + xval + ", y=" + yval;
                    _log.Error(msg + ex.Message);
                    throw new Exception("Wertegruppe sysVg=" + sysVg + ": für x=" + xval + ", y=" + yval + " keinen Wert gefunden.",ex);
                }
                catch (Exception ex)
                {

                    string msg = "VGDao.getVGValue failed for sysVG=" + sysVg + ", x=" + xval + ", y=" + yval;
                    _log.Error(msg + ex.Message);
                    throw new InvalidOperationException("Wertegruppe sysVg=" + sysVg + ": für x=" + xval + ", y=" + yval + " keinen Wert gefunden.", ex);
                }
                return rval;
            }
        }

        /// <summary>
        /// Delivers the scale values for the given axis
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual String[] getVGScaleValues(long sysVg, DateTime perDate, VGAxisType type)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                DbParameter[] Parameters = 
                { 
                        new Devart.Data.Oracle.OracleParameter{ ParameterName = "perDate", Value = perDate}, 
                        new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysvg", Value =sysVg}
                };

                try
                {
                    switch (type)
                    {
                        case (VGAxisType.YAXIS):
                            return ctx.ExecuteStoreQuery<String>(QUERYYAXIS, Parameters).OrderBy(p => p).ToArray<String>();

                        case (VGAxisType.XAXIS):
                            return ctx.ExecuteStoreQuery<String>(QUERYXAXIS, Parameters).OrderBy(p => p).ToArray<String>();
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    _log.Error("VGDao.deliverVGScaleValues failed - no data found " + ex.Message);
                    throw new InvalidOperationException("deliverVGScaleValues failed - no data found", ex);
                }
            }
        }

        /// <summary>
        /// Delivers the the value group boundaries
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public virtual VGBoundaries getVGBoundaries(long sysVg, DateTime perDate)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                Devart.Data.Oracle.OracleParameter xmax = new Devart.Data.Oracle.OracleParameter { ParameterName = "xmax", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };
                Devart.Data.Oracle.OracleParameter ymax = new Devart.Data.Oracle.OracleParameter { ParameterName = "ymax", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };
                Devart.Data.Oracle.OracleParameter xmin = new Devart.Data.Oracle.OracleParameter { ParameterName = "xmin", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };
                Devart.Data.Oracle.OracleParameter ymin = new Devart.Data.Oracle.OracleParameter { ParameterName = "ymin", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Decimal };

                DbParameter[] Parameters = 
                        { 
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "perDate", Value = perDate}, 
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "pSysvg", Value =sysVg},
                                xmax,ymax, xmin, ymin};
                try
                {
                    ((ObjectContext)ctx).ExecuteProcedure("CIC_COMMON_UTILS.CICMAXVALUE", Parameters);
                    VGBoundaries rval = new VGBoundaries();

                    rval.xmax = Convert.ToDouble(xmax.Value);
                    rval.ymax = Convert.ToDouble(ymax.Value);
                    rval.xmin = Convert.ToDouble(xmin.Value);
                    rval.ymin = Convert.ToDouble(ymin.Value);
                    return rval;
                }
                catch (Exception ex)
                {
                    _log.Error("VGDao.deliverVGBoundaries failed - no data found " + ex.Message);
                    throw new InvalidOperationException("deliverVGBoundaries failed - no data found", ex);
                }
            }

        }

        /// <summary>
        /// alters the x/v values for a vg to its outer boundaries
        /// </summary>
        /// <param name="sysvg"></param>
        /// <param name="perDate"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        public virtual void checkBoundaries(long sysvg, DateTime perDate, ref double xval, ref double yval)
        {
            VGBoundaries bounds = getVGBoundaries(sysvg, perDate);

            if (xval < bounds.xmin) xval = bounds.xmin;
            if (xval > bounds.xmax) xval = bounds.xmax;
            if (yval < bounds.ymin) yval = bounds.ymin;
            if (yval > bounds.ymax) yval = bounds.ymax;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysvg"></param>
        /// <returns></returns>
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

    /// <summary>
    /// Klasse VG Grenzen
    /// </summary>
    public class VGBoundaries
    {
        /// <summary>
        /// X Minimum
        /// </summary>
        public double xmin { get; set; }
        /// <summary>
        /// X Maximum
        /// </summary>
        public double xmax { get; set; }
        /// <summary>
        /// Y Minimum
        /// </summary>
        public double ymin { get; set; }
        /// <summary>
        /// Y Maximum
        /// </summary>
        public double ymax { get; set; }
    }

    /// <summary>
    /// Datenstruktur für Zins-Datenabfrage nach Stas
    /// </summary>
    public class VGStasData
    {
        /// <summary>
        /// Value 1
        /// </summary>
        public double V1 { get; set; }
        /// <summary>
        /// Gülig Von
        /// </summary>
        public DateTime ValidFrom { get; set; }
        /// <summary>
        /// Gülig Bis
        /// </summary>
        public DateTime ValidUntil { get; set; }
        /// <summary>
        /// Laufzeit
        /// </summary>
        public long lz { get; set; }
    }


}