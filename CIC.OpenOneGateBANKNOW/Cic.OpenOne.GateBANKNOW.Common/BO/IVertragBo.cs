using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Vertags BO Schnittstelle
    /// </summary>
    public interface IVertragBo
    {
        /// <summary>
        /// Fertrag via ID holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Daten</returns>
        VertragDto getVertrag(long sysid);

        /// <summary>
        /// isRREChangeAllowed
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        bool isRREChangeAllowed(long sysid, long sysperson);

        /// <summary>
        /// performKaufofferte
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        operformKaufofferte performKaufofferte(iperformKaufofferteDto input);

        /// <summary>
        /// changeRReceiver / Änderung des Restwertrechnungsempfänger
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        ochangeRRReceiver changeRRReceiver(long sysid);

        /// <summary>
        /// Prüfung auf Restwertrechnungsempfänger möglich, pendente Auflösung, Restwertrechnung verschickt
        /// </summary>
        /// <param name="result">result</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns></returns>
        oSearchDto<VertragDto> zustandPruefung(oSearchDto<VertragDto> result, long sysperole);


        /// <summary>
        /// Returns true when order purchase offer in ePOS-now allowed
        /// </summary>
        /// <param name="sysid">sysid</param>
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
        /// get MWST by sysvt
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        double getMWST(long sysvt, DateTime perDate);
    }
}
