using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO 
{
    public class osendRiskmailDto   : oBaseDto
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
