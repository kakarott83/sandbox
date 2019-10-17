using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// DTO for BPLISTENER BPE Process
    /// </summary>
    public class ProcessDto : EntityDto
    {
        public long sysbplistener { get; set; }
        public DateTime? datum { get; set; }
        public String benutzer { get; set; }
        public String process { get; set; }
        public String nummer { get; set; }

        public String description { get; set; }
        public String stepdescription { get; set; }

        public String casestep { get; set; }

        public long syswfuser { get; set; }
        public long sysbprole { get; set; }
        public String namebprole { get; set; }
        public long syskd { get; set; }
        public DateTime? faelligkeit { get; set; }
        public DateTime? eskalation { get; set; }
        public String processdefcode { get; set; }
        public String oltable { get; set; }
        public long sysoltable { get; set; }


        
        public String eventcode { get; set; }
        public String evaluatecode { get; set; }

        override public long getEntityId()
        {
            return sysbplistener;
        }

    }
}