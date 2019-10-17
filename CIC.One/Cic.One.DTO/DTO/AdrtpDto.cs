using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class AdrtpDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysAdrtp { get; set; }
        /*Bezeichnung */
        public String bezeichnung { get; set; }
        /*Beschreibung */
        public String beschreibung { get; set; }
        /*Rang (für Kompatibilität mit bestehendem Backoffice) */
        public int rang { get; set; }
        /*Active Flag*/
        public int activeFlag { get; set; }
        override public long getEntityId()
        {
            return sysAdrtp;
        }

    }
}
