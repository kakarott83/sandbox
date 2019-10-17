using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.BN
{
    /// <summary>
    /// Class for holding offer rule information
    /// </summary>
    public class AuskunftRegelDto
    {
        public String code { get; set; }
        public String text { get; set; }
        public int level { get; set; }
        public String displaytype { get; set; }
    }
}
