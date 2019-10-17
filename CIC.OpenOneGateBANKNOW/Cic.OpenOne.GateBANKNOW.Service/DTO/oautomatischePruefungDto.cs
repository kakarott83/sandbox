using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Outputparameter für automatischePruefung Methode
    /// </summary>
    public class oautomatischePruefungDto : oBaseDto
    {

        /// <summary>
        /// antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }



        public ocheckAntAngDto checkAntAngDto
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