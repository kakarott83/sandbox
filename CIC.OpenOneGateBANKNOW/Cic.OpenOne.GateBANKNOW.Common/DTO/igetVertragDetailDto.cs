using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für getVertragDetail Methode
    /// </summary>
    public class igetVertragDetailDto
    {
        /// <summary>
        /// Persistenzobjekt Vertrag
        /// </summary>
        public DTO.VertragDto vertrag
        {
            get;
            set;
        }
    }
}
