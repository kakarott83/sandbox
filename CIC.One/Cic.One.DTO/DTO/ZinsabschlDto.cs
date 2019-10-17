using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class ZinsabschlDto : EntityDto
    {
        public long NKKABSCHL { get; set; }
        public DateTime zeitraumVon { get; set; }
        public DateTime zeitraumBis { get; set; }
        public Boolean stornoflag { get; set; }
        public double zinssatz { get; set; }
        public double betrag { get; set; }


        override public long getEntityId()
        {
            return NKKABSCHL;
        }

    }
}
