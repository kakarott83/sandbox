using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of an Eaihot
    /// </summary>
    public class ogetEaihotDto : oBaseDto
    {
        public EaihotDto eaihot
        {
            get;
            set;
        }
    }
}