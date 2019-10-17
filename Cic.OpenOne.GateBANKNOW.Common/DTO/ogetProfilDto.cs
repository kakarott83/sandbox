using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für getProfil Methode
    /// </summary>
    public class ogetProfilDto
    {
        /// <summary>
        /// Allgemeines Messageobjekt
        /// </summary>
        public DTO.Message message
        {
            get;
            set;
        }

        /// <summary>
        /// Name des Händlers
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Vorname des Händlers
        /// </summary>
        public string vorname
        {
            get;
            set;
        }

        /// <summary>
        /// Strasse des Händlers
        /// </summary>
        public string strasse
        {
            get;
            set;
        }

        /// <summary>
        /// Hausnummer des Händlers
        /// </summary>
        public string hausnummer
        {
            get;
            set;
        }

        /// <summary>
        /// Postleitzahl des Händlers
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
        /// Telefon Direktwahl des Händlers
        /// </summary>
        public string telefon
        {
            get;
            set;
        }

        /// <summary>
        /// Telefon Mobile des Händlers
        /// </summary>
        public string mobil
        {
            get;
            set;
        }

        /// <summary>
        /// Telefax des Händlers
        /// </summary>
        public string telefax
        {
            get;
            set;
        }

        /// <summary>
        /// E-Mail des Händlers
        /// </summary>
        public string eMail
        {
            get;
            set;
        }

        /// <summary>
        /// URL des Händlers
        /// </summary>
        public string internet
        {
            get;
            set;
        }

        /// <summary>
        /// Aktuelle Systemmeldung
        /// </summary>
        public string informationUeber
        {
            get;
            set;
        }

        /// <summary>
        /// Benutzer ID des Händlers
        /// </summary>
        public string benutzerId
        {
            get;
            set;
        }

        /// <summary>
        /// Personen ID (person.sysperson) des Händlers
        /// </summary>
        public string personenId
        {
            get;
            set;
        }
    }
}
