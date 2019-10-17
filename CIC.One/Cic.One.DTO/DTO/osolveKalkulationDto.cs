using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class osolveKalkulationDto:oBaseDto
    {
        
        public AngkalkDto angkalk { get; set; }
        public AntkalkDto antkalk { get; set; }
        public List<double> raten { get; set; }
    }
}
