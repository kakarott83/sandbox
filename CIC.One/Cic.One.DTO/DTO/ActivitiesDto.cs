using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Activity Entity for view vc_activities
    /// </summary>
    public class ActivitiesDto : EntityDto 
    {
        public String area { get; set; }
        public long sysid { get; set; }
        public DateTime? datum { get; set; }
        public String typ { get; set; }
        public String bezeichnung { get; set; }
        public long sysperson { get; set; }
        public String name { get; set; }
        public String vorname { get; set; }
        public long syspartner { get; set; }
        public long syswfuser { get; set; }
        public String aspvorname { get; set; }
        public String aspname { get; set; }
        public String username { get; set; }
        public String uservorname { get; set; }

        override public long getEntityId()
        {
            return sysid;
        }
        public override string getEntityBezeichnung()
        {
            return bezeichnung;
        }

      
    }
}