using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.checkAntrag"/> Methode
    /// </summary>
    public class ocheckAntAngDto : oBaseDto
    {
        /// <summary>
        /// Status (rot, grün, gelb)
        /// </summary>
        public string status { get; set; }

        /// <summary>
        ///  Code der getroffenen Regel 
        /// </summary>
        public List<string> code { get; set; }


        /// <summary>
        /// Liste mit Fehlermeldungen
        /// </summary>
        public List<string> errortext { get; set; }

        /// <summary>
        /// Frontid results
        /// </summary>
        public string frontid { get; set; }

    }
}
