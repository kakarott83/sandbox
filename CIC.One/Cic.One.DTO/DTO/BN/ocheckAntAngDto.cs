using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.BN
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.checkAntrag"/> Methode
    /// </summary>
    public class ocheckAntAngDto : oBaseDto
    {
        /// <summary>
        /// Status (rot, grün, gelb)
        /// </summary>
        public String status { get; set; }

        /// <summary>
        ///  Code der getroffenen Regel 
        /// </summary>
        public List<String> code { get; set; }


        /// <summary>
        /// Liste mit Fehlermeldungen
        /// </summary>
        public List<String> errortext { get; set; }

    }
}
