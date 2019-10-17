using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO 
{
    public class TilgungDto : EntityDto
    {
        public long SYSSLPOS { get; set; }
        public DateTime? VALUTA { get; set; }
        public double BETRAG { get; set; }
        public String FIDENT { get; set; }
        public String NAME { get; set; }
        public String EREIGNIS { get; set; }
        public long TAGE { get; set; }
        public double TILGUNGP { get; set; }
        public double TILGUNG { get; set; }

        override public long getEntityId()
        {
            return SYSSLPOS;
        }

    }
}