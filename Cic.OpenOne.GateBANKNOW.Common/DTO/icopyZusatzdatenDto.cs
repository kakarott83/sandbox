using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für copyZusatzdaten Methode
    /// </summary>
    public class icopyZusatzdatenDto
    {
        /// <summary>
        /// Persistenzobjekt zu kopierende Zusatzdaten
        /// </summary>
        public DTO.ZusatzdatenDto zusatzdaten
        {
            get;
            set;
        }
    }
}
