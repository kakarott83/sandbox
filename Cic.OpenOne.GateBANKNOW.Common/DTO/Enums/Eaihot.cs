using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Enums
{
    /// <summary>
    /// Enum AreaConstants
    /// </summary>
    [System.CLSCompliant(true)]
    public enum AreaConstants
    {
        /// <summary>
        /// Interessent
        /// </summary>
        It,
        /// <summary>
        /// Angebot
        /// </summary>
        Angebot,
        /// <summary>
        /// Antrag
        /// </summary>
        Antrag,
        /// <summary>
        /// VT
        /// </summary>
        Vt,
        /// <summary>
        /// Alle
        /// </summary>
        Alle
    }

    /// <summary>
    /// 
    /// </summary>
    [System.CLSCompliant(true)]
    public enum EaiHotStatusConstants
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Working
        /// </summary>
        Working = 1,
        /// <summary>
        /// Bereit
        /// </summary>
        Ready = 2
    }

    /// <summary>
    /// 
    /// </summary>
    [System.CLSCompliant(true)]
    public enum EaiHotOltable
    {
        /// <summary>
        /// ANTRAG
        /// </summary>
        Antrag = 0,
        /// <summary>
        /// ANGEBOT
        /// </summary>
        Angebot = 1,
    }

}
