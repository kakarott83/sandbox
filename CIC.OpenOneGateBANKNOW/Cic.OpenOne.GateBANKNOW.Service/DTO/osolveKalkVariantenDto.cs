using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Outputparameter für SolveKalkVarianten Methode
    /// </summary>
    public class osolveKalkVariantenDto : oBaseDto
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