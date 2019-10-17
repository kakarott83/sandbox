using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter TransactionRisikoprüfung Methode
    /// </summary>
    public class ocheckTrRiskDto : oBaseDto
    {

        /// <summary>
        /// antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }


        /// <summary>
        /// Frontid results
        /// </summary>
        public string frontid { get; set; }
    }
}