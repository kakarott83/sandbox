using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für createZusatzdaten Methode
    /// </summary>
    public class icreateZusatzdatenDto
    {
        /// <summary>
        /// ID der Zusatzdaten (leer für neue Zusatzdaten)
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
    }
}
