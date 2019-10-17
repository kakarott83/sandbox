using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für createZusatzdaten Methode
    /// </summary>
    public class icreateZusatzdatenDto
    {
        /// <summary>
        /// ID der Zusatzdaten (leer für neue Zusatzdaten)
        /// </summary>
        public ZusatzdatenDto ZusatzdatenDto
        {
            get;
            set;
        }
    }
}
