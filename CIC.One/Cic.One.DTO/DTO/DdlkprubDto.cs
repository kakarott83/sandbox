using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class DdlkprubDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysddlkprub { get; set; }
        /*Aktiv */
        public int activeFlag { get; set; }
        /*Rang für Sortierung der Anzeige (eindeutig zum Gebiet) */
        public int rank { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Code */
        public String code { get; set; }
        /*Gebiet (zb Person oder auch Segment …) */
        public String area { get; set; }
        /*Verweis zum Satz im Gebiet (optional, zb ein konkretes Segment) */
        public long sysId { get; set; }

        override public long getEntityId()
        {
            return sysddlkprub;
        }

        public List<DdlkpcolDto> cols { get; set; }


        /**
         * Fields used for the flattened list of user saved entries
         */
        public String zusatzRub {get;set;}
        public String zusatzCol { get; set; }
        public String zusatzValue { get; set; }
        
    }
}