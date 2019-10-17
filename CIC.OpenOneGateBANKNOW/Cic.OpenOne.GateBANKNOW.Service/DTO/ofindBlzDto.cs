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
    public class ofindBlzDto : oBaseDto
    {
        /// <summary>
        /// Liste von Banken
        /// </summary>
        public List<BlzDto> blz
        {
            get;
            set;
        }
    }
}