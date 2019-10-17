using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System.Xml.Serialization;


namespace Cic.OpenOne.GateBANKNOW.Service.DTO {
    public class osearchKundeExternDto : Cic.OpenOne.Common.DTO.oBaseDto 
    {
        /// <summary>
        /// Liste mit Persistenzobjekten Kunde
        /// </summary>
        public oSearchDto<KundeExternGUIDto> result 
        {
            get;
            set;
        }
    }
}