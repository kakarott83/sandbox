using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// PKZ/UKZ aus dem letzten Antrag im Zustand 'Vertrag aktiviert' 
    /// </summary>
    public class ogetZusatzDatenAktivDto : oBaseDto
    {
         /// <summary>
        /// Zusatzdaten 
        /// </summary>
        public ZusatzdatenDto zusatzdaten { get; set; }
           

    }
}
