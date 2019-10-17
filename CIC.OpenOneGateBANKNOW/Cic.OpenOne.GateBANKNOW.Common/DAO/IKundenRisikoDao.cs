using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.BO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public interface IKundenRisikoDao
    {

        /// <summary>
        /// returns all available laufzeiten
        /// </summary>
        /// <returns></returns>
        List<int> getLaufzeiten();

        /// <summary>
        /// Returns the kredit value for the proposal
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        ELCalcVars getAntragDaten(long sysid);

        /// <summary>
        /// Returns true if Antrag has a MA
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        bool hasMA(long sysid);

        /// <summary>
        /// Budget Daten aus KREMO für Simulation / risikoprüfungsrelevanten Daten für DecisionEngine 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        Budget4DESimDto getBudget4DESim(long sysid);


        /// <summary>
        /// Returns sysvg for zinsertrag calc
        /// </summary>
        /// <returns></returns>
        long getZinsertragWertegruppe();

        /// <summary>
        /// Returns the needed DE Values for EL Calculation
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        DEValues4KR getDecisionValues(long sysid);

        /// <summary>
        /// Calculates expected Loss
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="lz"></param>
        /// <param name="pd"></param>
        /// <param name="kreditbetrag"></param>
        /// <returns></returns>
        double? calculateEL(Flags4KR flags, long lz, double pd, double kreditbetrag, long sysantrag);

        /// <summary>
        /// Calculates Profitability
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="lz"></param>
        /// <param name="pd"></param>
        /// <param name="kreditbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <param name="el"></param>
        /// <returns></returns>
        double? calculateELPROF(Flags4KR flags, long lz, double pd, double kreditbetrag, double zinsertrag, double el, long sysantrag);
    }
}
