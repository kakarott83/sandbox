using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class AppRegistryDto 
    {
        public string code { get; set; }
        public string wert { get; set; }
        public string bezeichnung { get; set; }
        public string level { get; set; } 
        public string beschreibung { get; set; }
        public long sysid { get; set; }
        public string blobwert { get; set; }
        public string area { get; set; }
        public long areaid { get; set; }
        public long syswfuser { get; set; }
    }
}