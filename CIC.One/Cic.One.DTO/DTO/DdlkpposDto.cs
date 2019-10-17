using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class DdlkpposDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysddlkppos { get; set; }
        /*Verweis zur Collection */
        public long sysddlkpcol { get; set; }
        /*Aktiv */
        public int activeFlag { get; set; }
        /*Rang für Sortierung der Liste (eindeutig zur Collection) */
        public int rank { get; set; }
        /*Wert in Liste */
        public String value { get; set; }
        /*Tooltip in Liste */
        public String tooltip { get; set; }

        override public long getEntityId()
        {
            return sysddlkppos;
        }
        
    }
}