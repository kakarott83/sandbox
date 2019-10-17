using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Angebot Antrag Subvention Dto
    /// </summary>
    public class AngAntSubvDto
    {
        /// <summary>
        /// PKEY angebot
        /// </summary>
        public long sysangsubv { get; set; }
        /// <summary>
        /// PKEY antrag
        /// </summary>
        public long sysantsubv { get; set; }

        /// <summary>
        /// Verweis zur Variante 
        /// </summary>
        public long sysangvar { get; set; }
        /// <summary>
        /// Verweis zum Antrag 
        /// </summary>
        public long sysantrag { get; set; }

        /// <summary>
        /// Verweis zum Subventionstyp 
        /// </summary>
        public long syssubvtyp { get; set; }
        /// <summary>
        /// Subventionstyp-Bezeichnung
        /// </summary>
        public String subvTypBezeichnung { get; set; }

        /// <summary>
        /// Verweis zum Subventionsgeber (Händler bei Differenzleasing) 
        /// </summary>
        public long syssubvg { get; set; }
        /// <summary>
        /// Subventionsgeber-Bezeichnung
        /// </summary>
        public String subvGBezeichnung { get; set; }

        /// <summary>
        /// Subventionsbetrag (wird vom Auszahlungsbetrag abgezogen) 
        /// </summary>
        public double betragBrutto { get; set; }

        //unused but yet important fields for later usage---------------------------------------------
        /// <summary>
        /// Anfangs Zeitstempel
        /// </summary>
        public DateTime beginn { get; set; }
        /// <summary>
        /// Subventionsbetrag Ust
        /// </summary>
        public double betragust { get; set; }
        /// <summary>
        /// Subventionsbetrag Netto
        /// </summary>
        public double betrag { get; set; }
        /// <summary>
        /// Defaultbetrag
        /// </summary>
        public double betragdef { get; set; }
        /// <summary>
        /// Verweis zur Subvention
        /// </summary>
        public long sysprsubv { get; set; }
        /// <summary>
        /// Laufzeit
        /// </summary>
        public long lz { get; set; }
    }
}
