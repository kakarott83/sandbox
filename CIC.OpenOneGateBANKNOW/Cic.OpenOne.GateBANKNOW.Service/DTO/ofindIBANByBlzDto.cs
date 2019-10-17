using System;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Ausgabe DTO Banken
    /// </summary>
    public class ofindIBANByBlzDto : oBaseDto
    {
        /// <summary>
        /// IBAN
        /// </summary>
        public String iban
        {
            get;
            set;
        }

    }
}