using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.BO.Versicherung;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Versicherung
{
    /// <summary>
    /// Factory to create different Calculators dependend on the codeMethod of the VSTYP
    /// </summary>
    [System.CLSCompliant(true)]
    public class VSCalcFactory
    {
        #region Constants
        /// <summary>
        /// Types of different calculation methods used for creating the implementations
        /// Standard
        /// </summary>
        public const string Cnst_CALC_DEFAULT = "DEFAULT";
        /// <summary>
        /// Leasing Todesfall
        /// </summary>
        public const string Cnst_CALC_TODESFALL_LEASING = "L_RIP";
        /// <summary>
        /// Leasing Altersueberschreitung
        /// </summary>
        public const string Cnst_CALC_AUEUAL_LEASING = "L_AUEUAL";
        /// <summary>
        /// Kredit Todesfall
        /// </summary>
        public const string Cnst_CALC_TODESFALL_KREDIT = "K_RIP";
        /// <summary>
        /// Kredit Altersueberschreitung
        /// </summary>
        public const string Cnst_CALC_AUEUAL_KREDIT = "K_AUEUAL";

        /// <summary>
        /// Restwertabsicherung Betrag
        /// </summary>
        public const string Cnst_CALC_RWA_B = "RWA_B";

        /// <summary>
        /// Restwertabsicherung Prozent
        /// </summary>
        public const string Cnst_CALC_RWA_P = "RWA_P";

        /// <summary>
        /// Externe Versicherung Zuerich
        /// </summary>
        public const string Cnst_CALC_EXT_VS_ZUERICH = "EXT_VS_ZUERICH";







        #endregion


        /// <summary>
        /// Rechner erzeugen
        /// </summary>
        /// <param name="codeMethod">Methode</param>
        /// <param name="dao">Versicherungs DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        /// <returns>Rechner</returns>
        public static IVSCalculator createCalculator(string codeMethod, IInsuranceDao dao, IQuoteDao quoteDao)
        {
            if (codeMethod == null || "".Equals(codeMethod))
            {
                throw new NullReferenceException("CODEMETHOD not defined: "+codeMethod);
            }
            codeMethod = codeMethod.ToUpper();

            switch (codeMethod)
            {
               
                case Cnst_CALC_DEFAULT:
                    return new DefaultCalculator(dao, quoteDao);
                case Cnst_CALC_TODESFALL_LEASING:
                    return new TodesfallCalculator(dao, quoteDao);
                case Cnst_CALC_AUEUAL_LEASING:
                    return new AUCalculator(dao, quoteDao);
                case Cnst_CALC_AUEUAL_KREDIT:
                    return new AUKCalculator(dao, quoteDao);
                case Cnst_CALC_TODESFALL_KREDIT:
                    return new RSVCalculator(dao, quoteDao);
                case Cnst_CALC_RWA_B:
                    return new DefaultCalculator(dao, quoteDao);
                case Cnst_CALC_RWA_P:
                    return new DefaultCalculator(dao, quoteDao);
                case Cnst_CALC_EXT_VS_ZUERICH:
                    return new DefaultCalculator(dao, quoteDao);
            }
            throw new NotSupportedException(codeMethod + " not yet supported");
        }
    }
}
