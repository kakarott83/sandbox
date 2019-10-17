using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Holds data for setting/getting an extra value for a certain db entity
    /// </summary>
    public class ExtraValueDto
    {
        public long sysExtraValue { get; set;}
        public long sysId { get; set; }
        public String area { get; set; }
        public String colcode { get; set; }
        public byte[] content { get; set; }
    }
}