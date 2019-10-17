using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für CreateOrUpdateDMSAkte Methode
    /// </summary>
    public class icreateOrUpdateDMSAkteDto
    {
        /// <summary>
        /// Gebiet ID
        /// </summary>
        public long sysid { get; set; }
        /// <summary>
        /// Gebiet
        /// </summary>
        public String area { get; set; }
       
    }
}
