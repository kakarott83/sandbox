using System;
using System.Security;

namespace Cic.One.DTO
{
    /// <summary>
    /// Enthält alle Daten die benötigt werden um sich zu einem Mailserver zu verbinden
    /// </summary>
    public class EWSUserDto
    {
        /// <summary>
        /// Email-Addresse des Service Accounts
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// User welcher imitiert werden soll
        /// </summary>
        public string ImpersonatedUser { get; set; }

        /// <summary>
        /// Username des Service Accounts
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Passwort des Service Accounts in einem SecureString
        /// </summary>
        public SecureString Password { get; set; }

        /// <summary>
        /// Die Url die zum Server zeigt. Im Fall von dem CIC Exchange Server ist das
        /// https://mail.cic-group.eu/EWS/Exchange.asmx
        /// Falls keine vorhanden ist, wird es versucht den Server automatisch zu finden.
        /// </summary>
        public Uri AutodiscoverUrl { get; set; }

        /// <summary>
        /// Wfuser, von welchem die Daten geladen wurden
        /// </summary>
        public long SysWfuser { get; set; }
    }
}