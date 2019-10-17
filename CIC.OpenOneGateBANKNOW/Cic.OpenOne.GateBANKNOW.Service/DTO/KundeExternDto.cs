using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Parameterklasse für KundeExternResultDto
    /// </summary>
    public class KundeExternResultDto : KundeExternDto
    {
        /// <summary>
        /// CRIF ADDRESS_ID
        /// </summary>
        public string adressId { get; set; }

        /// <summary>
        /// Kandidaten Rang
        /// </summary>
        public int rang { get; set; }

        /// <summary>
        /// Gruppen Id
        /// </summary>
        public int groupId { get; set; }

        /// <summary>
        /// Identifikations typ: z.B. HOUSE_CONFIRMED
        /// </summary>
        public string identifikationsTyp { get; set; }
    }

    /// <summary>
    /// Parameterklasse für KundeExternDto
    /// </summary>
    public class KundeExternDto
    {
        /// <summary>
        /// 0: Privatperson, 1: Firma
        /// </summary>
        public int firma { get; set; }

        /// <summary>
        /// 1: Frau, 2: Mann, 3: Unbekannt
        /// </summary>
        public String anredeCode { get; set; }

        /// <summary>
        /// Name/CompanyName
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// CoName
        /// </summary>
        public String coname { get; set; }

        /// <summary>
        /// Vorname
        /// </summary>
        public String vorname { get; set; }

        /// <summary>
        /// Middle Name - Zweiter Vorname
        /// </summary>
        public String zweiterVorname { get; set; }

        /// <summary>
        /// Maiden Name - Mädchenname
        /// </summary>
        public String geburtsName { get; set; }

        /// <summary>
        /// Geburtsdatum
        /// </summary>
        public DateTime? gebdatum { get; set; }

        /// <summary>
        /// Telefonnummer
        /// </summary>
        public string telefonnummer { get; set; }

        /// <summary>
        /// Handynummer
        /// </summary>
        public string handynummer { get; set; }

        /// <summary>
        /// Faxnummer
        /// </summary>
        public string faxnummer { get; set; }

        /// <summary>
        /// Email-Adresse
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Telefonnummer
        /// </summary>
        public string web { get; set; }



        /// <summary>
        /// Strasse
        /// </summary>
        public String strasse { get; set; }
        /// <summary>
        /// Hausnummer
        /// </summary>
        public String hsnr { get; set; }
        /// <summary>
        /// Postleitzahl
        /// </summary>
        public String plz { get; set; }
        /// <summary>
        /// Ort
        /// </summary>
        public String ort { get; set; }


        /// <summary>
        /// Region nach ISO-3166-2
        /// </summary>
        public string regionCode { get; set; }

        /// <summary>
        /// Subregion nach ISO-3166-2
        /// </summary>
        public string subRegionCode { get; set; }

        /// <summary>
        /// Land ISO-3166-1 3-alpha oder ISO-3166-1 2-alpha
        /// </summary>
        public string land { get; set; }
    }
}