using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class PrunstepDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysPrunStep { get; set; }
        /*Aktiv */
        public int activeFlag { get; set; }
        /*Verweis um ChecksTyp */
        public long sysPstep { get; set; }
        /*Verweis zur Checkliste */
        public long sysPrun { get; set; }
        /*Code */
        public String syscode { get; set; }
        /*Rang für Sortierung der Anzeige (eindeutig zum Gebiet) */
        public int rank { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Beschreibung */
        public String description { get; set; }
        /*(0=nicht begkonnen, 1=in Bearbeitung, 2=erledigt, 3=warten auf jemand anderen, 4=zurückgestellt) */
        public int prozessStatus { get; set; }

        override public long getEntityId()
        {
            return sysPrunStep;
        }
    }
}