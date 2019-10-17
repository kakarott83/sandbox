using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class ocreatemTanUserDto
    {
        public mTanStatusDto status {get;set;}
        public mTanUserDto[] users { get; set; }
    }
}