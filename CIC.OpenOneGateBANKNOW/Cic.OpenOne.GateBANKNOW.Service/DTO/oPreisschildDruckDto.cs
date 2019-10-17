using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class oPreisschildDruckDto : oBaseDto
    {

        /// <summary>
        /// Frontid des eai results
        /// </summary>
        public string frontid { get; set; }

        /// <summary>
        /// Dokument
        /// </summary>
        public byte[] hfile { get; set; }


    }
}