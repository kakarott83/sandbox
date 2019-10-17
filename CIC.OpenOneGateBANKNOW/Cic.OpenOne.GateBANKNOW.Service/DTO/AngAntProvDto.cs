using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Angebot Antrag Provision Dto
    /// </summary>
    public class AngAntProvDto
    {
        /// <summary>
        /// Primary key Prov
        /// </summary>
        public long sysprov { get; set; }

        /// <summary>
        /// flag für fixe provision
        /// </summary>
        public int flaglocked { get; set; }
        /// <summary>
        /// Verweis zum Variante
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
        /// Provisionsbetrag Umsatzsteuer 
        /// </summary>
        public double provisionUst { get; set; }
        /// <summary>
        /// Provisionsbetrag Brutto 
        /// </summary>
        public double provisionBrutto { get; set; }
        /// <summary>
        /// ProvisionProzent
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
       
    }
}
