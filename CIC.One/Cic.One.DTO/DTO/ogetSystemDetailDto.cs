using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetSystemDetailDto : oBaseDto
    {
        public SystemDto system
        {
            get;
            set;
        }
    }
}