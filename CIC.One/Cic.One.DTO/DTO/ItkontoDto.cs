using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ItkontoDto : EntityDto
    {
       
        /*Primärschlüssel */
        public long sysitkonto { get; set; }
        /*Verweis zum Kontotyp */
        public long sysit { get; set; }
        /*Verweis zum Kontotypbeschreibung */
        public long syskontoTpBezeichnung { get; set; }
        /*Bankleitzahl */
        public String blz { get; set; }
        /*BIC */
        public String bic { get; set; }
        /*Verweis zur Bankleitzahl */
        public long sysblz { get; set; }
        /*Verweis zur Person */
        public long sysperson { get; set; }
        /*Iban */
        public String iban { get; set; }
        /*Kontonummer */
        public String kontoNr { get; set; }
        /*Kontoinhaber */
        public String kontoInhaber { get; set; }
        /*Active Flag*/
        public int activeFlag { get; set; }

        /// <summary>
        /// Rang
        /// </summary>
        public int? rang  { get; set; }

        override public long getEntityId()
        {
            return sysitkonto;
        }

        /*Flag ob Konto geändert werden darf*/
        public int readOnly { get; set; }


    }
}