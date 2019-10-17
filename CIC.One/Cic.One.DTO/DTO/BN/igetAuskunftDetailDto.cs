using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.BN
{
  public enum AuskunftDetailMode
    {
        NONE=0,
        EWK=1,
        BA = 2,
        GA=3,
        ALL=4
    }
    public class igetAuskunftDetailDto
    {
        /// <summary>
        /// sysid Antrag
        /// </summary>
        public long sysantrag { get; set; }

        public long sysperson { get; set; }
        /// <summary>
        /// Antragsteller 1 oder 2
        /// </summary>
        public int manr { get; set; }
        public AuskunftDetailMode fetchMode { get; set; } 

    }
}
