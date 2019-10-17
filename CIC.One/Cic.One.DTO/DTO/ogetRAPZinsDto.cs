using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class ogetRAPZinsDto:oBaseDto
    {
        public double rapZins { get;set;}
        public double rapZinsMin { get; set; }
        public double rapZinsMax { get; set; }
    }
}
