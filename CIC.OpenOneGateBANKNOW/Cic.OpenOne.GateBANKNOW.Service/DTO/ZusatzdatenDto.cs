using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Parameterklasse für Zusatzdaten
    /// </summary>
    public class ZusatzdatenDto
    {
        /// <summary>
        /// PKZ
        /// </summary>
        public PkzDto[] pkz
        {
            get;
            set;
        }

        /// <summary>
        /// UKZ
        /// </summary>
        public UkzDto[] ukz
        {
            get;
            set;
        }

        /// <summary>
        /// Kundentyp
        /// </summary>
        public int kdtyp
        {
            get;
            set;
        }

        /// <summary>
        /// Kreditnehmereinheiten, dieser IT ist SYSUNTER
        /// </summary>
        public List<KneDto> kne { get; set; }
    }
}
