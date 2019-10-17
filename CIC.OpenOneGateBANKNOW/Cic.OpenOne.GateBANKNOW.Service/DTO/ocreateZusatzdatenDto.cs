using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für createZusatzdaten Methode
    /// </summary>
    public class ocreateZusatzdatenDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Zusatzdaten
        /// </summary>
        public ZusatzdatenDto zusatzdaten
        {
            get;
            set;
        }
    }
}
