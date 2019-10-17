using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Angebot Antrag Insurance Dto
    /// </summary>
    public class AngAntVsDto
    {
        /// <summary>
        /// PKEY ANTRAG
        /// </summary>
        public long sysantvs { get; set; }
        /// <summary>
        /// PKEY ANGEBOT
        /// </summary>
        public long sysangvs { get; set; }


        /// <summary>
        /// Verweis zur Variante 
        /// </summary>
        public long sysangvar { get; set; }
        /// <summary>
        /// Verweis zum Antrag 
        /// </summary>
        public long sysantrag { get; set; }
        
        /// <summary>
        /// Verweis zum Versicherungstyp 
        /// </summary>
        public long sysvstyp { get; set; }
        /// <summary>
        /// Versicherungstyp-Bezeichnung
        /// </summary>
        public String vsTypBezeichnung { get; set; }

        /// <summary>
        /// Verweis zum Versicherungstyp 
        /// </summary>
        public int mitfinflag { get; set; }

        /// <summary>
        /// Verweis zum Versicherer 
        /// </summary>
        public long sysvs { get; set; }
        /// <summary>
        /// Versicherer-Bezeichnung
        /// </summary>
        public String vsBezeichnung { get; set; }

        /// <summary>
        /// Prämie brutto gemäß Prämienmodell aus Versicherungstyp 
        /// </summary>
        public double praemie { get; set; }

        /// <summary>
        /// Prämie brutto gemäß Prämienmodell aus Versicherungstyp ungerundet
        /// </summary>
        public double praemieOrg { get; set; }

        /// <summary>
        /// Prämie Prozent aus Prämienmodell
        /// </summary>
        public double praemiep { get; set; }
        /// <summary>
        /// Laufzeit
        /// </summary>
        public int lz { get; set; }
        /// <summary>
        /// Zahlungen pro Jahr
        /// </summary>
        public int ppy { get; set; }
        /// <summary>
        /// Versicherungspaketcode
        /// </summary>
        public String code { get; set; }

        /// <summary>
        /// Type of Service
        /// </summary>
        public ServiceType serviceType { get; set; }
    }
}
