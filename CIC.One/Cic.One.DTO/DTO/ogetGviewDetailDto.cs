using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of generic view
    /// </summary>
    public class ogetGviewDetailDto : oBaseDto
    {
        public GviewDto gview
        {
            get;
            set;
        }
    }
}

