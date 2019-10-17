using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
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
    }
}
