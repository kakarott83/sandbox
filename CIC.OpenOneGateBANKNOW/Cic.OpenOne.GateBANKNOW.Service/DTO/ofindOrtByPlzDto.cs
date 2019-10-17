using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Outputdto für findOrtByPlz
    /// </summary>
    public class ofindOrtByPlzDto : oBaseDto
    {
        /// <summary>
        /// PostleitzahlDto
        /// </summary>
        public PlzDto[] plzDto { get; set; }
    }
}