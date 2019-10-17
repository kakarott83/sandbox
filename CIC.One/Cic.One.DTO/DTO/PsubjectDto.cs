using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class PsubjectDto : EntityDto
    {
        public long syspsubject { get; set; }
        public long sysptask { get; set; }
        public long syslease { get; set; }
        public long syspchecker { get; set; }
        public String area { get; set; }

        public String rolle { get;set;}
        public String name { get; set; }
        public String vorname { get; set; }

        public String result { get; set; }
        public DateTime? resultdate { get; set; }
        public long resulttime { get; set; }

        override public long getEntityId()
        {
            return syspsubject;
        }
    }
}
