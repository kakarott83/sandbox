using System;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Enum for PL-SQL Version
    /// </summary>
    public enum plSQLVersion
    {
        /// <summary>
        /// V1
        /// </summary>
        V1,

        /// <summary>
        /// V2
        /// </summary>
        V2

        
    };

    /// <summary>
    /// Schnittstelle VG DAO
    /// </summary>
    public interface IVGDao
    {
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
        double getVGValue(long sysVg, System.DateTime perDate, string xval, string yval, VGInterpolationMode interpolationMode, plSQLVersion version);

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
        double getVGValue(long sysVg, System.DateTime perDate, string xval, string yval, VGInterpolationMode interpolationMode);

        /// <summary>
        /// Delivers the scale values for the given axis
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        String[] getVGScaleValues(long sysVg, System.DateTime perDate, VGAxisType type);

        /// <summary>
        /// Delivers the the value group boundaries
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        VGBoundaries getVGBoundaries(long sysVg, System.DateTime perDate);

        /// <summary>
        /// alters the x/v values for a vg to its outer boundaries
        /// </summary>
        /// <param name="sysvg"></param>
        /// <param name="perDate"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        void checkBoundaries(long sysvg, DateTime perDate, ref double xval, ref double yval);

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
        double getVGValueSaldo(long sysvg, double Saldo, long lz, DateTime perDate);

        /// <summary>
        /// Abfrage nach Stas 
        /// Liest die Prämienzinssätze nach Produkt ID und Laufzeit aus.
        /// Und berücksichtigt das Gültigkeitsdatum
        /// </summary>
        /// <param name="perDate">Aktuelles Datum</param>
        /// <param name="sysvg">Wertegruppen ID</param>
        /// <param name="lz">Laufzeit</param>
        /// <returns>Zinssatz</returns>
        double getVGValuePraemie(long sysvg, long lz, DateTime perDate);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysvg"></param>
        /// <returns></returns>
        VgDto getVg(long sysvg);
    }
}