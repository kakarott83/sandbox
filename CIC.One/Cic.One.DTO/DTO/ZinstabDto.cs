using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ZinstabDto : EntityDto
    {
        public long syszinstab { get; set; }
        public String bezeichnung { get; set; }
        public long sysvttyp { get; set; }
        public long sysbn { get; set; }
        public DateTime datum { get; set; }
        public double anzahlung { get; set; }
        public double kaution { get; set; }
        public double diskont { get; set; }
        public double lombard { get; set; }
        public double kuendigung { get; set; }
        public double diffta { get; set; }
        public double diffmk { get; set; }
        public double euribor1d { get; set; }
        public double euribor3d { get; set; }
        public double euribornominal { get; set; }
        public long syswaehrung { get; set; }
        public double fibor { get; set; }
        public double fiboraufschlag { get; set; }
        public double swap { get; set; }

        
        override public long getEntityId()
        {
            return syszinstab;
        }

    }
}
