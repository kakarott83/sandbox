using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO.BN;

namespace Cic.One.DTO
{
    public class ogetBNAngebotDetailDto : oBaseDto
    {
        public BNAngebotDto BNAngebot
        {
            get;
            set;
        }
    }
}