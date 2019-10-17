using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class ItemcatDto : EntityDto
    {

        /*Primärschlüssel */
        public long sysItemCat { get; set; }
        /*Verweis zu Wfuser, Kategorieinhaber */
        public long sysOwner { get; set; }
        /*Privat, nur für Inhaber sichtbar (trotz Gesichtskreis) */
        public int privateFlag { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Beschreibung */
        public String description { get; set; }
        /*Code für Icon, Farbe */
        public String designCode { get; set; }
        /*Active Flag*/
        public int activeFlag { get; set; }
        /// <summary>
        /// Beinhaltet die Kategorien
        /// </summary>
        public string[] Categories { get; set; }
        override public long getEntityId()
        {
            return sysItemCat;
        }
    }
}