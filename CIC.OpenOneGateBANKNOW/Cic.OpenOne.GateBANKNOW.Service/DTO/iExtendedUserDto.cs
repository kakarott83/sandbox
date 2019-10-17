using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.loginPartnerService.extendedValidateUser"/> Methode
    /// </summary>
    public class iExtendedUserDto
    {
        /// <summary>
        /// Benutzername
        /// </summary>
        public string username
        {
            get;
            set;
        }

        /// <summary>
        /// Technisches Passwort
        /// </summary>
        public string presharedKey
        {
            get;
            set;
        }
    }
}
