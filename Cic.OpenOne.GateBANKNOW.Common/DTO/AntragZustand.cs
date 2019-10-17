using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Zustand des Antrags
    /// </summary>
    public enum AntragZustand
    {
        /// <summary>
        /// Zustand Neu
        /// </summary>
        [StringValue("Neu")]
        Neu,
        
    }
    /// <summary>
    /// Zustand des Antrags
    /// </summary>
    public enum AntragAttribut
    {
        /// <summary>
        /// Attirbut neu
        /// </summary>
        [StringValue("Neu")]
        Neu,
        /// <summary>
        /// Attribut Eingereicht
        /// </summary>
        [StringValue("Eingereicht")]
        Eingereicht
    }

    /// <summary>
    /// Antrag Varianten 
    /// </summary>
    public enum AntragVarianten
    {
        /// <summary>
        /// GleichbleibendeRate
        /// </summary>
        GleichbleibendeRate = 1,

        /// <summary>
        /// Mindestanzahlung
        /// </summary>
        Mindestanzahlung = 2,

        /// <summary>
        /// FreieKalkulation
        /// </summary>
        FreieKalkulation = 3,

        /// <summary>
        /// Ursprungskalkulation
        /// </summary>
        Ursprungskalkulation = 4
    }
}
