using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of Checklisten Arten
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetPrunartDetailDto : oBaseDto
    {
        public PrunartDto prunart
        {
            get;
            set;
        }
    }
}