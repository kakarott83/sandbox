using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.GateWKT.DTO
{
    /// <summary>
    /// Holds vehicle techdata
    /// </summary>
    public class TechDataDto
    {


        public int kw { get; set; }
        public int ps { get; set; }
        public string eek { get; set; }

        public int kwe { get; set; }

        public int kwgesamt { get; set; }

        public int kwh { get; set; }

        public int pse { get; set; }

        public int psgesamt { get; set; }

        public int reichweite { get; set; }

    }
}