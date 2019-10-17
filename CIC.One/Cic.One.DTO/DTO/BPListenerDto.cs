using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    /// <summary>
    /// Describes a bplistener for breadcrumbs
    /// </summary>
    public class BPListenerDto
    {
        public long sysbplistener { get; set; }
        public String description { get; set; }
        public String stepdescription { get; set; }

        public String evaluatecode { get; set; }
        public String eventcode { get; set; }
        public String processdefcode { get; set; }


    }
}
