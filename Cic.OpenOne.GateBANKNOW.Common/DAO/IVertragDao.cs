using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Vertrag DAO Schnittstelle
    /// </summary>
    public interface IVertragDao
    {
        /// <summary>
        /// Vertrag via ID holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Daten</returns>
        VertragDto getVertragDetails(long sysid);

        /// <summary>
        /// get Vertragid
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        long getVertragSysId(long sysid);

        /// <summary>
        /// Der Restsaldo (inkl. dem Restwert) des Vertrags wurde  in Rechnung gestellt ?
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        bool restsaldoInRechnung(long sysid);

        /// <summary>
        /// bool isRREChangeAllowed(long sysid, long sysperson)
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        bool isRREChangeAllowed(long sysid, long sysperson);

        /// <summary>
        /// Verweis zur Person-Restwertgarant
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        long? getRwga(long sysid);

        /// <summary>
        /// Verweis zur Kunde
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        long? getKunde(long sysid);

        /// <summary>
        /// Kaufofferte bestellen allowed
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        bool isPerformKaufofferteAllowed(long sysid);

        
        /// <summary>
        /// gets contract details by its id.
        /// function created so that we can change the query and thus decide what data is given in what field, customizable.
        /// </summary>
        /// <param name="sysvt">contract id</param>
        /// <returns>contract details</returns>
        VertragDto getVertragForExtension(long sysvt);

        /// <summary>
        /// getMWST
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        double getMWST(long sysvt, DateTime perDate);

        /// <summary>
        /// Vertragsnummer holen
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        String getVertragNummer(long sysid);
    }
}
