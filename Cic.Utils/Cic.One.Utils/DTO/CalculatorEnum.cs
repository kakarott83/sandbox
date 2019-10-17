using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Utils.DTO
{
    public enum CalculationMode : int
    {
        /// <summary>
        /// Nachschüssig.
        /// MK
        /// </summary>
        End = 1,
        /// <summary>
        /// Vorschussig.
        /// MK
        /// </summary>
        Begin = 2,
    }
    public enum CalculationTargets : int
    {
        /// <summary>
        /// Berechnungsgrundlage Kalkulation,
        /// MK
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        CalculateBase = 1,
        /// <summary>
        /// Laufzeit Kalkulation.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        CalculateTerm = 2,
        /// <summary>
        /// Rate Kalkulation.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        CalculateRate = 3,
        /// <summary>
        /// Restwert Kalkulation (Leasing), Restrate Kalkulation (Finanz).
        /// MK
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        CalculateResidualValueOrRemainingDebt = 4,
        /// <summary>
        /// Nominalzins Kalkulation.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        CalculateNominalInterest = 5,
        /// <summary>
        /// Sonderzahlung Kalkulation.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        CalculateFirstPayment = 6,
        /// <summary>
        /// Effektivzins Kalkulation.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.EnumMember]
        CalculateEffectiveInterest = 7,
    }
}
