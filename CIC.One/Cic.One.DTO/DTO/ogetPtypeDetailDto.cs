using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of ChecklistenTypen
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetPtypeDetailDto : oBaseDto
    {
        public PtypeDto ptype
        {
            get;
            set;
        }
    }
}