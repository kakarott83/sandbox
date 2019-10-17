using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of a guardean request
    /// </summary>
    public class ogetGuardeanDetailDto : oBaseDto
    {
        public GviewDto gview
        {
            get;
            set;
        }
        public byte[] data;
    }
}

