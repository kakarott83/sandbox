using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO 
{
    public class isearchKundeExternDto 
    {
        /// <summary>
        /// input
        /// </summary>
        public KundeExternGUIDto input { get; set; }

        /// <summary>
        /// filter z.B. CRIF_identifyAddress
        /// </summary>
        public String filter { get; set; }

    }
}