using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// incentivation level
    /// </summary>
    public enum IncentivierungStufe
    {
        Basic,
        Bronze,
        Silber,
        Gold,
        Platin,
    }

    /// <summary>
    /// Values of commissions depending on the current incentivation level
    /// </summary>
    public class IncentivierungParameterDto
    {
        /// <summary>
        /// bonus level
        /// </summary>
        public IncentivierungStufe Stufe { get; set; }

        /// <summary>
        /// overall budget. Reaching this, you reach the bonus level.
        /// </summary>
        public double Finanzierungsvolumen { get; set; }

        /// <summary>
        /// Kickback: When reaching the next bonus level, you get this
        /// </summary>
        public double Provisionsbetrag { get; set; }

        /// <summary>
        /// When concluding a contract, you get a bonus depending on the contract's budget.
        /// Key: lower limit of scope of application
        /// Value: bonus money
        /// </summary>
        public Dictionary<double, double> ProvisionsbetragFinanzierungsvolumen { get; set; }

        /// <summary>
        ///  When concluding a contract, if the contract is "ppi", you get this as a bonus
        /// </summary>
        public double ZusatzprovisionPPI { get; set; }
    }

    /// <summary>
    /// Matrix for commission calculation depending on current incentivation level
    /// </summary>
    public class IncentivierungMatrixDto
    {
        /// <summary>
        /// sales goal (overall budget). You get the cummulated bonus money after reaching multiples of the sales goal.
        /// </summary>
        public double VerkaufszielFinanzierungsvolumen { get; set; }
        /// <summary>
        /// sales goal (number of contracts). You get the cummulated bonus money after reaching multiples of the sales goal.
        /// </summary>
        public int VerkaufszielAnzahlVertraege { get; set; }

        /// <summary>
        /// calculation values depending on current level
        /// </summary>
        public Dictionary<IncentivierungStufe, IncentivierungParameterDto> Matrix { get; set; }
    }
}