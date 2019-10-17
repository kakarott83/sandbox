using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Returns the updated BNOW User Data
    /// </summary>
    public class osetmTanUserDataDto:oBaseDto
    {
        public mTanUserDto[] users { get; set; }
    }
}