using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Bankleitzahl-Typ
    /// </summary>
    public enum BlzType
    {
        /// <summary>
        /// IBAN
        /// </summary>
        IBAN=0,
        /// <summary>
        /// BIC
        /// </summary>
        BIC=1,
        /// <summary>
        /// BLZ
        /// </summary>
        BLZ=2,

        BLZENDSWITH=3,
        BICENDSWITH=4
    }
}
