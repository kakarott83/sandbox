using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class oFinVariantenDruckenDto  : oBaseDto
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
