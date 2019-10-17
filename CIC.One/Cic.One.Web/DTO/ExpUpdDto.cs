using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class ExpUpdDto
    {
        public long? sysexpval { get; set; }
        public long sysexptyp { get; set; }
        public long sysid { get; set; }
        public String expression { get; set; }
        public int archivflag { get; set; }
        public int expired { get; set; }
        public int method { get; set; }
    }
}