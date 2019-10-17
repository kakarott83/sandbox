using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für m-Tan-Service change Password
    /// </summary>
    public class isetmTanUserPasswordDto
    {
        public String applicationId { get; set; }
        public String newPassword { get; set; }
        public String oldPassword { get; set; }
    }
}
