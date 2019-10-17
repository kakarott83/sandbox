using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für createZusatzdaten Methode
    /// </summary>
    public class ocreateZusatzdatenDto
    {
        /// <summary>
        /// Allgemeines Messageobjekt
        /// </summary>
        public DTO.Message message
        {
            get;
            set;
        }

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
