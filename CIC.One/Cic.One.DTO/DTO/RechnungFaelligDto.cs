using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class RechnungFaelligDto :  EntityDto
    {
        public DateTime? VALUTADATUM { get; set; }
        public double BETRAG { get; set; }
        public String RECHNUNG { get; set; }
        public String TEXT { get; set; }
        public String TYP { get; set; }
        public String NAME { get; set; }
        public long SYSID { get; set; }

        override public long getEntityId()
        {
            return SYSID;
        }

    }
}