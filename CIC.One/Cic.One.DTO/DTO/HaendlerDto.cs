using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class HaendlerDto : EntityDto
    {
        public long SYSPERSON { get; set; }
        public String NAME { get; set; }
        public String VORNAME { get; set; }
        public String STRASSE { get; set; }
        public String PLZ { get; set; }
        public String ORT { get; set; }
        public String LAND { get; set; }
        public String ANREDE { get; set; }
        public String MATCHCODE { get; set; }
        public String CODE { get; set; }
        public String TITEL { get; set; }
        public String TELEFON { get; set; }
        override public long getEntityId()
        {
            return SYSPERSON;
        }
    }
}