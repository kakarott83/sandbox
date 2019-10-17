using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of Wfsignature
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetWfsignatureDetailDto : oBaseDto
    {
        public WfsignatureDto wfsignature
        {
            get;
            set;
        }
    }
}
