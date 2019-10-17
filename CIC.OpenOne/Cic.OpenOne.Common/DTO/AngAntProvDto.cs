using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Angebot Antrag Provision Dto
    /// </summary>
    public class AngAntProvDto
    {
        /// <summary>
        /// PKEY for angebot and antrag provision
        /// </summary>
        public long sysprov { get; set; }

        /// <summary>
        /// flag für fixe provision
        /// </summary>
        public int flaglocked { get; set; }
        /// <summary>
        /// Verweis zur Variante
        /// </summary>
        public long sysangvar { get; set; }
        /// <summary>
        /// Verweis zum Antrag
        /// </summary>
        public long sysantrag { get; set; }

        /// <summary>
        /// Verweis zum Provisionstyp (Umsatz, Zins …)
        /// </summary>
        public long sysprprovtype { get; set; }
        /// <summary>
        /// Provisionstyp-Bezeichnung
        /// </summary>
        public String prProvTypeBezeichnung { get; set; }

        /// <summary>
        /// Verweis zum Provisionsempfänger (Händler, nicht Verkäufer)
        /// </summary>
        public long syspartner { get; set; }
        /// <summary>
        /// Provisionsempfänger-Bezeichnung
        /// </summary>
        public String partnerBezeichnung { get; set; }

        /// <summary>
        /// Provisionsbetrag
        /// </summary>
        public double provision { get; set; }
        /// <summary>
        /// Provisionsbetrag ungerundet
        /// </summary>
        public double provisionOrg { get; set; }
        /// <summary>
        /// Provisionsbetrag Umsatzsteuer
        /// </summary>
        public double provisionUst { get; set; }
        /// <summary>
        /// Provisionsbetrag Brutto
        /// </summary>
        public double provisionBrutto { get; set; }
        /// <summary>
        /// ProvisionProzent DB PROVPRO
        /// </summary>
        public double provisionPro { get; set; }

       
        /// <summary>
        /// Default Provisionsbetrag
        /// </summary>
        public double defaultprovision { get; set; }
        /// <summary>
        /// Default Provision Brutto
        /// </summary>
        public double defaultprovisionbrutto { get; set; }
        /// <summary>
        /// Default Provision Ust
        /// </summary>
        public double defaultprovisionust { get; set; }
        /// <summary>
        /// Default Provision Prozent
        /// </summary>
        public double defaultprovisionp { get; set; }

        ///-------------------------------FIELDS FOR Table PROV (VT-Area)--------------------------------------------------------------//
        /// <summary>
        /// Verweis zu Vertrag
        /// </summary>
        public long sysvt { get; set; }
        /// <summary>
        /// Provisionsbasis
        /// </summary>
        public double basis { get; set; }
        /// <summary>
        /// Provisionsbetrag errechnet
        /// </summary>
        public double auszahlung { get; set; }
        public double defauszahlung { get; set; }
        /// <summary>
        /// Auszahlungsart = 0
        /// </summary>
        public int auszahlungsart { get;set;}
        /// <summary>
        /// Perdatum Abrechnung
        /// </summary>
        public DateTime abrechnung { get; set; }
        /// <summary>
        /// Area z.B. PRPROVTYPE
        /// </summary>
        public String area { get; set; }
        /// <summary>
        /// Id Provisionstyp SYSPRPROVTYPE
        /// </summary>
        public long syslease { get; set; }
    }
}
