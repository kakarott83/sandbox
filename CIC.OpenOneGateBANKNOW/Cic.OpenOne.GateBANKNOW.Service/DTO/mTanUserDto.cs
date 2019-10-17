using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// User Data for the BNOW User Management Service
    /// </summary>
    public class mTanUserDto
    {
        /// <summary>
        /// Anwendungs Id
        /// </summary>
        public String applicationId { get; set; }

        /// <summary>
        /// Erstelldatum
        /// </summary>
        public String created { get; set; }


        /// <summary>
        /// Sprache
        /// </summary>
        public String language { get; set; }

        /// <summary>
        /// EMail
        /// </summary>
        public String mail { get; set; }


        /// <summary>
        /// Handynr
        /// </summary>
        public String mobile { get; set; }


        /// <summary>
        /// Geändert-Datum
        /// </summary>
        public String modified { get; set; }

        /// <summary>
        /// Passwort
        /// </summary>
        public String password { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public String status { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public String token { get; set; }


        /// <summary>
        /// Benutzer-Identifier
        /// </summary>
        public String userId { get; set; }
    }
}