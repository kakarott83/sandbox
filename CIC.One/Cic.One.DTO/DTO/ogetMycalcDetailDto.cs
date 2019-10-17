using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of MyCalc
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetMycalcDetailDto : oBaseDto
    {
        public MycalcDto mycalc
        {
            get;
            set;
        }
    }
}