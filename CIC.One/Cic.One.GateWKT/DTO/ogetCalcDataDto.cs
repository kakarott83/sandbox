using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.One.DTO
{
    /// <summary>
    /// Output for calculation constants
    /// </summary>
    public class ogetCalcDataDto : oBaseDto
    {
        /// <summary>
        /// mwst  (percentage) 
        /// </summary>
        public double mwst { get; set; }
        /// <summary>
        /// mehrkm factor (percentage) 
        /// </summary>
        public double mehrkmfactor { get; set; }
        /// <summary>
        /// minderkm factor percentage
        /// </summary>
        public double minderkmfactor { get; set; }

        /// <summary>
        /// Reifen Wechselrythmus daten
        /// </summary>
        public double wechselrhythmus_def { get; set; }

        /// <summary>
        /// Reifen Risikofaktor
        /// </summary>
        public double tirerisk { get; set; }
        /// <summary>
        /// Reifen FixLimitiert
        /// </summary>
        public double tirefix { get; set; }
        /// <summary>
        /// Reifen FixUnLimitiert
        /// </summary>
        public double tirefixunlimited { get; set; }
        /// <summary>
        /// Reifen Variabel
        /// </summary>
        public double tirevariabel { get; set; }
        /// <summary>
        /// Angebot gültig bis
        /// </summary>
        public double angebotvaliduntil { get; set; }
    }
}