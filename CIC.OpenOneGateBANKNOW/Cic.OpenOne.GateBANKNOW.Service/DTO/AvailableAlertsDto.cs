using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Parameterklasse für AvailableAlertsDto, wird verwendet in <see cref="olistAvailableAlertsDto"/>
    /// </summary>
    public class AvailableAlertsDto
    {
        /// <summary>
        /// AntragsID des Eintrags
        /// </summary>
        public long sysID
        {
            get;
            set;
        }

        /// <summary>
        /// Dem Antrag zugrundeliegende Vertragsart
        /// </summary>
        public string vertragsart
        {
            get;
            set;
        }

        /// <summary>
        /// Antragsteller (Kunde)
        /// </summary>
        public string kunde
        {
            get;
            set;
        }

        /// <summary>
        /// Ort des Antragstellers
        /// </summary>
        public string ort
        {
            get;
            set;
        }

        /// <summary>
        /// Telefon des Antragstellers
        /// </summary>
        public string telefon
        {
            get;
            set;
        }

        /// <summary>
        /// Vorheriger Status des Antrags
        /// </summary>
        public string statusAlt
        {
            get;
            set;
        }

        /// <summary>
        /// Geänderter (aktueller) Status des Antrags
        /// </summary>
        public string statusNeu
        {
            get;
            set;
        }

        /// <summary>
        /// Datum der Statusänderung
        /// </summary>
        public DateTime datum
        {
            get;
            set;
        }

        /// <summary>
        /// Zeitpunkt der Statusänderung
        /// </summary>
        public DateTime zeit
        {
            get;
            set;
        }

        /// <summary>
        /// Marke des Objekts
        /// </summary>
        public string marke
        {
            get;
            set;
        }

        /// <summary>
        /// Modell des Objekts
        /// </summary>
        public string modell
        {
            get;
            set;
        }

        /// <summary>
        /// antragsNummer des Eintrags
        /// </summary>
        public String antragsNummer
        {
            get;
            set;
        }

        /// <summary>
        /// Finanzierungsvorschlag anzeigen 
        /// </summary>
        public int proFinLock //BNR11 CR 149
        {
            get;
            set;
        }
    }
}
