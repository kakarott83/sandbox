using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Holds the Users fetched from the BNOW User Management Service
    /// </summary>
    public class ogetmTanUserDataDto: oBaseDto
    {
        public mTanUserDto[] users { get; set; }
    }
}