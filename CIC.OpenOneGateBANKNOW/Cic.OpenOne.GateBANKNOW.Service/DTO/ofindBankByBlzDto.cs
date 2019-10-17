using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
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