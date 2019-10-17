using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Returns the created user from the  BNOW User Management User Data
    /// </summary>
    public class ocreatemTanUserDto: oBaseDto
    {
        public mTanUserDto[] users { get; set; }
    }
}