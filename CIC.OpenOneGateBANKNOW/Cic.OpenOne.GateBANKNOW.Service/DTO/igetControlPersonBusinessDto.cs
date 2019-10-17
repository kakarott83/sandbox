using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Input for Kontrollinhaber Auskunft Service
    /// </summary>
    public class igetControlPersonBusinessDto
    {
        /// <summary>
        /// Antrag für den die Kontrollinhaber geprüft werden sollen
        /// </summary>
        public long sysantrag { get; set; }
        /// <summary>
        /// Falls nicht eindeutiger Kunde und eine Trefferliste angezeigt wurde
        /// hier die gewählte adressid übergeben und erneut anfragen
        /// </summary>
        public String adressid { get; set; }
    }
}