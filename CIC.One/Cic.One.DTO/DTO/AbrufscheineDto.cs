using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    // Abrufscheine   SNAG034
    public class AbrufscheineDto : EntityDto
    {
        override public long getEntityId()
        {
            return sysid;
        }

        override public String getEntityBezeichnung()
        {
            return bezeichnung;
        }

        public string hd_code { get; set; }
        public string hd_name { get; set; }
        public string kd_code { get; set; }
        public string kd_name { get; set; }
        public long kd_sysperson { get; set; }
        public string vt_vertrag { get; set; }
        public long vt_sysid { get; set; }
        public string antrag_antrag { get; set; }
        public long antrag_sysid { get; set; }
        public DateTime? c_date { get; set; }
        public int status { get; set; }
        public DateTime? finisheddate { get; set; }
        public string zustand { get; set; }
        public string benutzer { get; set; }
        public long sysid { get; set; }
        public long sysadm { get; set; }
        public string bezeichnung { get; set; }
        public string admadd_bezeichnung { get; set; }
        public long hd_sysperson { get; set; }
    }
}