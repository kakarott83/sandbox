using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.Common.DTO
{
    public class igetAppSettingsItemsDto
    {
        public long syswfuser { get; set; }
        public long sysreg { get; set; }
        public long sysregsec { get; set; }
        public long sysregvar{ get; set; }
        public string bezeichnung { get; set; }
        public long sysid { get; set; }
        public string area {get;set;}
    }
}