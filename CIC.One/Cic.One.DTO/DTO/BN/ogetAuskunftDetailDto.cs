using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO.BN;

namespace Cic.One.DTO.BN
{
    /// <summary>
    /// Returns the detail of EKW/BA/GA
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetAuskunftDetailDto : oBaseDto
    {
        public AuskunftDetailDto ewk { get;set;}
        public AuskunftDetailDto ba { get; set; }
        public AuskunftDetailDto ga { get; set; }
    }
}