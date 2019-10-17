using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getBuchwertService.getBuchwert"/> Methode
    /// </summary>
    public class ogetBuchwertDto : oBaseDto
    {
        /// <summary>
        /// Transferobjekt Buchwerte zu Vertrag
        /// </summary>
        public BuchwertDto BuchwertDto
        {
            get;
            set;
        }
    }
}
