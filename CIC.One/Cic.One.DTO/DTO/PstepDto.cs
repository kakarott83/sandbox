using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class PstepDto : EntityDto 
    {

        /*Primärschlüssel */
        public long sysPstep { get; set; }
        /*Aktiv */
        public int activeFlag { get; set; }
        /*Code */
        public String syscode { get; set; }
        /*Verweis zum Checklistentyp */
        public long sysPtype { get; set; }
        /*Rang für Sortierung der Anzeige (eindeutig zum Gebiet) */
        public int rank { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Beschreibung */
        public String description { get; set; }
        /*Anzeigebedingung (Script) */
        public String visibilityCondition { get; set; }
        /*Ladebedingung (Script) */
        public String loadCondition { get; set; }

        override public long getEntityId()
        {
            return sysPstep;
        }
    }
}