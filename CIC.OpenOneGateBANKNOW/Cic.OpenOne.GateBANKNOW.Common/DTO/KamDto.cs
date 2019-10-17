using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für getKam Methode
    /// </summary>
    public class KamDto
    {
        /// <summary>
        /// Name des Key Account Managers
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Vorname des Key Account Managers
        /// </summary>
        public string vorname
        {
            get;
            set;
        }

        /// <summary>
        /// Telefon Direktwahl des Key Account Managers
        /// </summary>
        public string telefon
        {
            get;
            set;
        }
    }
}
