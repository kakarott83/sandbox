using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Parameterklasse für Adresse
    /// </summary>
    public class AdresseDto
    {
        /// <summary>
        /// PKEY 
        /// </summary>
        public long sysadresse { get; set; }

        /// <summary>
        /// FKEY zur Person/Interessent 
        /// </summary>
        public long sysperson { get; set; }

        /// <summary>
        /// Anrede 
        /// </summary>
        public String anrede { get; set; }
        /// <summary>
        /// Verweis zur Anrede (Lookup und Übersetzung) 
        /// </summary>
        public String anredeCode { get; set; }
        /// <summary>
        /// Titel 
        /// </summary>
        public String titel { get; set; }
        /// <summary>
        /// Verweis zum Titel (Lookup und Übersetzung) 
        /// </summary>
        public String titelCode { get; set; }
        /// <summary>
        /// Name 
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// Vorname 
        /// </summary>
        public String vorname { get; set; }
        /// <summary>
        /// Geburtsdatum 
        /// </summary>
        public DateTime? gebdatum { get; set; }
        /// <summary>
        /// Korrespondenzadresse Strasse 
        /// </summary>
        public String strasse { get; set; }
        /// <summary>
        /// Korrespondenzadresse Hausnummer 
        /// </summary>
        public String hsnr { get; set; }
        /// <summary>
        /// Korrespondenzadresse Postleitzahl 
        /// </summary>
        public String plz { get; set; }
        /// <summary>
        /// Korrespondenzadresse Ort 
        /// </summary>
        public String ort { get; set; }

        /// <summary>
        /// Korrespondenzadresse Land (Schweiz, Deutschland …) 
        /// </summary>
        public long sysland { get; set; }
        /// <summary>
        /// Land-Bezeichnung
        /// </summary>
        public String landBezeichnung { get; set; }
        /// <summary>
        /// Korrespondenzadresse Kanton (AA .. ZH) 
        /// </summary>
        public long sysstaat { get; set; }
        /// <summary>
        /// Staat-Bezeichnung
        /// </summary>
        public String staatBezeichnung { get; set; }
        /// <summary>
        /// Korrespondenzsprache (wenn abweichend) 
        /// </summary>
        public long sysctlang { get; set; }
        /// <summary>
        /// Sprache-Bezeichnung
        /// </summary>
        public String langBezeichnung { get; set; }

        /// <summary>
        /// CRIF Adress-Id
        /// </summary>
        public String adressid { get; set; }
    }
}
