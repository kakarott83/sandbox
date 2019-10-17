using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.BN
{
    /// <summary>
    /// Kalkulations Kontext
    /// </summary>
    public class kalkKontext
    {
        /// <summary>
        /// Ermittelter Score des Kunden für die RAP-Zinsanpassung
        /// </summary>
        public String kundenScore { get; set; }

        /// <summary>
        /// Kalkulationszins nominell (nicht über Zinsermittlung gehen)
        /// </summary>
        public double zinsNominal { get; set; }

        /// <summary>
        /// Indicates the usage of zinsNominal
        /// </summary>
        public bool useZinsNominal { get; set; }

        /// <summary>
        /// Fahrzeug-Price
        /// </summary>
        public double grundBrutto { get; set; }

        /// <summary>
        /// ZubehoerBrutto-Price
        /// </summary>
        public double zubehoerBrutto { get; set; }

        /// <summary>
        /// set Default Cust Zins
        /// </summary>
        public bool setDefaultCustZins { get; set; }
    }
}
