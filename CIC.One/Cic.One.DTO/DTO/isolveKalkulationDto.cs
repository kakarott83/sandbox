using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    public class isolveKalkulationDto
    {
        public CalculationCommand command {get;set;}
        public AntkalkDto antkalk { get; set; }
        public AngkalkDto angkalk { get; set; }
        public List<double> raten {get;set;}
        public String range {get;set;}
        public String zinsVGCode { get; set; }
        public String margeVGCode { get; set; }
        public long sysobtyp { get; set; }

        public long kmstand { get; set; }
        public long age { get; set; }
        
        /// <summary>
        /// Kunden-Score
        /// </summary>
        public String score { get; set; }
        public String isoLanguageCode { get; set; }
    }
}
