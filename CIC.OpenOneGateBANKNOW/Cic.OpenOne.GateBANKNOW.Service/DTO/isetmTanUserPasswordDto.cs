using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAntragService.setStammnummer"/> Methode
    /// </summary>
    public class isetmTanUserPasswordDto
    {
        /// <summary>
        /// Anwendungs Id
        /// </summary>
        public String applicationId { get; set; }
        /// <summary>
        /// Neues Passwort
        /// </summary>
        public String newPassword { get; set; }
        /// <summary>
        /// Altes Passwort
        /// </summary>
        public String oldPassword { get; set; }
    }
}
