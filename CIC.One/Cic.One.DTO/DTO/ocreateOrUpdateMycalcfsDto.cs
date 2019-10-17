using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created MyCalcfs
    /// </summary>
    public class ocreateOrUpdateMycalcfsDto : oBaseDto
    {
        public MycalcfsDto mycalcfs { get; set; }
    }
}