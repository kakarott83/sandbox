using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für saveZusatzdaten Methode
    /// </summary>
    public class isaveZusatzdatenDto
    {
        /// <summary>
        /// Persistenzobjekt Zusatzdaten
        /// </summary>
        public DTO.ZusatzdatenDto zusatzdaten
        {
            get;
            set;
        }
    }
}
