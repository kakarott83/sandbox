using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für Cic.OpenOne.GateBANKNOW.Service.getVSLink Methode
    /// </summary>
    public class igetVSLinkDto
    {
        /// <summary>
        /// Antrags-Id
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Versicherungscode externes Versicherungsprodukt
        /// </summary>
        public String extvscode { get; set; }

    }
}
