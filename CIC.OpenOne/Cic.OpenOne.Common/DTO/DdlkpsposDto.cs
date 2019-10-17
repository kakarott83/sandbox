using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    public class DdlkpsposDto : EntityDto
    {

        public DdlkpsposDto()
        {
        }
        public DdlkpsposDto(DdlkpsposDto org)
        {
            this.sysddlkpcol = org.sysddlkpcol;
            this.sysddlkppos = org.sysddlkppos;
            this.sysddlkpspos = org.sysddlkpspos;
            this.sysid = org.sysid;
            this.activeFlag = org.activeFlag;
            this.area = org.area;
            this.entityBezeichnung = org.entityBezeichnung;
            this.entityId = org.entityId;
            this.value = org.value;
            this.content = org.content;
        }
        /*Primärschlüssel */
        public long sysddlkpspos { get; set; }
        /*Verweis zum Wert (wenn aus Liste gewählt) */
        public long sysddlkppos { get; set; }
        /*Verweis zur Collection (immer) */
        public long sysddlkpcol { get; set; }
        /*Gebiet (zb Person …) */
        public String area { get; set; }
        /*Verweis zum Satz im Gebiet (zb Person.sysperson …) */
        public long sysid { get; set; }
        /*Gewählter bzw geschriebener Wert (bei Colltyp = 1 > geschrieben, Coltyp =2, gewählt) */
        public String value { get; set; }
        /// <summary>
        /// CLOB
        /// </summary>
        public String content { get; set; }
        /*Active Flag*/
        public int activeFlag { get; set; }

        override public long getEntityId()
        {
            return sysddlkpspos;
        }


    }
}
