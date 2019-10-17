using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public abstract class AdrDto : EntityDto 
    {
        override public String getEntityBezeichnung()
        {
            return strasse;
        }

     
        /*Verweis zum Land */
        public long sysLand { get; set; }

        /*Verweis zum Land Bezeichnung*/
        public String landBez { get; set; }

        /*Verweis zur Sprache */
        public long sysCtLang { get; set; }


        public long sysCtLangKorr { get; set; }
        public long sysStaat { get; set; }

        /*Strasse */
        public String strasse { get; set; }

        /*Hausnummer */
        public String hsnr { get; set; }

        /*Postleitzahl */
        public String plz { get; set; }

        /*Ort */
        public String ort { get; set; }

        /*Telefonnummer */
        public String telefon { get; set; }

        /*Telefaxnummer */
        public String fax { get; set; }

        /*Mobilnummer */
        public String handy { get; set; }

        /*Emailadresse */
        public String email { get; set; }

        /*Homepage, Url */
        public String url { get; set; }

        /*Active Flag*/
        public int activeFlag { get; set; }

        /// <summary>
        /// Gueltigb
        /// </summary>
        public DateTime? gueltigab { get; set; }
                         
        /// <summary>
        /// Gueltigbis
        /// </summary>
        public DateTime? gueltigbis { get; set; }

        /// <summary>
        /// Rang
        /// </summary>
        public int? rang { get; set; }
        public String anrede { get; set; }
        public String anredeCode { get; set; }
        public String titel { get; set; }
        public String titelCode { get; set; }
        public String name { get; set; }
        public String vorname { get; set; }
        public DateTime? gebdatum { get; set; }

     
      
    }
}