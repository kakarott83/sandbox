using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of Objekt
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetRecalcDetailDto : oBaseDto
    {
        public RecalcDto recalc
        {
            get;
            set;
        }
    }
}