using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created MyCalc
    /// </summary>
    public class ocreateOrUpdateMycalcDto : oBaseDto
    {
        public MycalcDto mycalc { get; set; }
    }
}