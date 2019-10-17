using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetVorgangDetailDto : oBaseDto
    {
        public VorgangDto vorgang
        {
            get;
            set;
        }
    }
}