using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für getBuchwert Methode
    /// </summary>
    public class ogetBuchwertDto : oBaseDto
    {
        /// <summary>
        /// Transferobjekt Buchwerte zu Vertrag
        /// </summary>
        public DTO.BuchwertDto BuchwertDto
        {
            get;
            set;
        }
    }
}
