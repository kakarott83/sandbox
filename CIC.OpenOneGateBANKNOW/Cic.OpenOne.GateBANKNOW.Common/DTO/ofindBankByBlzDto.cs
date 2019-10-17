using System;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Ausgabe DTO Banken
    /// </summary>
    public class ofindBankByBlzDto : oBaseDto
    {
        /// <summary>
        /// Bank-Id (BC or PC)
        /// </summary>
        public String bankId
        {
            get;
            set;
        }

        /// <summary>
        /// Bank-Name
        /// </summary>
        public String bankName
        {
            get;
            set;
        }
    }
}