using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class BriefDto : EntityDto
    {
        public long SYSOBBRIEF { get; set; }
        public String TREIBSTOFF { get; set; }
        public String GETRIEBE { get; set; }
        public String MOTOR { get; set; }
        public int ZULGEW { get; set; }
        public String REIFV { get; set; }
        public String REIFMUH { get; set; }
        public String FELGV { get; set; }
        public String FELGMUH { get; set; }
        public int KW { get; set; }
        public int TANK { get; set; }
        public double VERBRAUCHGESAMT { get; set; }
        public double CO2EMI { get; set; }
        public double NOX { get; set; }
        public double DPF { get; set; }
        public String FIDENT { get; set; }
        public String MOTORNUMMER { get; set; }
        public String IMPCODE { get; set; }
        override public long getEntityId()
        {
            return SYSOBBRIEF;
        }

    }
}