using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class PrkgroupmDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysPrkgroupm { get; set; }
        /*Verweis zur Kundengruppe */
        public long sysPrkgroup { get; set; }
        /*Verweis zur Person */
        public long sysPerson { get; set; }
        /*Aktiv */
        public int activeFlag { get; set; }
        /*Beginn */
        public DateTime? validFrom { get; set; }
        /*Manuelle Zuweisung, aus Identifizierungsregl ausschliessen */
        public int flagManuell { get; set; }


        override public long getEntityId()
        {
            return sysPrkgroupm;
        }

        /*PRKGROUP Name */
        public String name { get; set; }
        /*PRKGROUP Beschreibung */
        public String description { get; set; }

        public String kundenname { get; set; }

        public String kundenvorname { get; set; }

        public int addToPerson { get; set; }

    }
}