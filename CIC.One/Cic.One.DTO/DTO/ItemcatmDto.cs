using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class ItemcatmDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysItemCatm { get; set; }
        /*Verweis zur Kategorie */
        public long sysItemCat { get; set; }
        /*Gebiet (zb E-Mail, Termin, Aufgabe) */
        public String area { get; set; }
        /*Verweis zum Satz im Gebiet */
        public long sysid { get; set; }

        /// <summary>
        /// Enthält den Namen der zugehörigen Itemcat
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Enthält die Description der zugehörigen Itemcat
        /// </summary>
        public string description { get; set; }

        override public long getEntityId()
        {
            return sysItemCatm;
        }
    }
}