using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    // Abgerechnete Verträge  SNAG033
    public class AbgerechnetevertrageDto : EntityDto
    {
        override public long getEntityId()
        {
            return sysid;
        }

        override public String getEntityBezeichnung()
        {
            return vertrag;
        }


        public long sysid { get; set; }


        public long sysls { get; set; }


        public string vertrag { get; set; }


        public double ahk { get; set; }


        public DateTime? rueckgabe { get; set; }


        public string name { get; set; }
    }
}