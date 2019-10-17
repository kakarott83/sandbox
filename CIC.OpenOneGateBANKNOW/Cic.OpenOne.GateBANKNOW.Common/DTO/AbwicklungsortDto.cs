using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für getAbwicklungsort Methode
    /// </summary>
    public class AbwicklungsortDto
    {
        /// <summary>
        /// Strasse des Abwicklungsortes
        /// </summary>
        public string strasse
        {
            get;
            set;
        }

        /// <summary>
        /// Hausnummer des Abwicklungsortes
        /// </summary>
        public string hausnummer
        {
            get;
            set;
        }

        /// <summary>
        /// Postleitzahl des Abwicklungsortes
        /// </summary>
        public string plz
        {
            get;
            set;
        }

        /// <summary>
        /// Ort des Händlers
        /// </summary>
        public string ort
        {
            get;
            set;
        }

        /// <summary>
        /// Telefon des Abwicklungsortes
        /// </summary>
        public string telefon
        {
            get;
            set;
        }
    }
}
