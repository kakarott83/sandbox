using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    
    /// <summary>
    /// Zustand des Angebots
    /// </summary>
    public enum AngebotZustand
    {
        /// <summary>
        /// Angebotszustand Neu
        /// </summary>
        [StringValue("Neu")]
        Neu,
        /// <summary>
        /// Angebotszustand Gedruckt
        /// </summary>
        [StringValue("Gedruckt")]
        Gedruckt,
        /// <summary>
        /// Angebotszustand Abgeschlossen
        /// </summary>
        [StringValue("Abgeschlossen")]
        Abgeschlossen

        

       
    }
    /// <summary>
    /// Attribut des Angebots
    /// </summary>
    public enum AngebotAttribut
    {
        /// <summary>
        /// Angebotsattribut Neu
        /// </summary>
        [StringValue("Neu")]
        Neu,
        /// <summary>
        /// Angebotsattribut Gültig
        /// </summary>
        [StringValue("Gültig")]
        Gueltig,
        /// <summary>
        /// Angebotsattribut Eingereicht
        /// </summary>
        [StringValue("Antrag eingereicht")]
        Antrageingereicht,
        /// <summary>
        /// Angebotsattribut Abgelaufen
        /// </summary>
        [StringValue("Abgelaufen")]
        Abgelaufen,
        /// <summary>
        /// Angebotsattribut Neu/Gueltig
        /// </summary>
        [StringValue("Neu/gültig")]
        NeuGueltig,
        /// <summary>
        /// Angebotsattribut Gedruck/Gueltig
        /// </summary>
        [StringValue("Gedruckt/gültig")]
        GedrucktGueltig

    }
}
