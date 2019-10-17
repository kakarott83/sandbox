using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.BN
{
    public class osolveBNKalkulationDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Kalkulation
        /// </summary>
        public KalkulationDto kalkulation
        {
            get;
            set;
        }

       
    }
}
