using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of Angkalk
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetAngkalkDetailDto : oBaseDto
    {
        public AngkalkDto angkalk
        {
            get;
            set;
        }
    }
}