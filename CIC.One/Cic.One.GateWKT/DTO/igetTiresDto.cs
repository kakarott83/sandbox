using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.GateWKT.DTO
{
    /// <summary>
    /// Input for fetching all data about tires
    /// </summary>
    public class igetTiresDto
    {
        public String eurotaxNr {get;set;}
        public String codeWinterVorne {get;set;}
        public String codeWinterHinten { get; set; }
        public String codeSommerVorne { get; set; }
        public String codeSommerHinten { get; set; }
        public String felgenCodeVorne { get; set; }
        public String felgenCodeHinten { get; set; }
    }
}