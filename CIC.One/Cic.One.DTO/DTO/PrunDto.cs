using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class PrunDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysPrun { get; set; }
        /*Aktiv */
        public int activeFlag { get; set; }
        /*Verweis zum Checklistentyp */
        public long sysPtype { get; set; }
        /*Code */
        public String syscode { get; set; }
        /*Gebiet (zb Aufgabe) */
        public String area { get; set; }
        /*Verweis zum Satz im Gebiet */
        public long sysId { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Beschreibung */
        public String description { get; set; }
        /*(0=nicht begkonnen, 1=in Bearbeitung, 2=erledigt, 3=warten auf jemand anderen, 4=zurückgestellt) */
        public int prozessStatus { get; set; }

        override public long getEntityId()
        {
            return sysPrun;
        }
    }
}