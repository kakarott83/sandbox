﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAntragService.setStammnummer"/> Methode
    /// </summary>
    public class isetStammnummerDto
    {
        /// <summary>
        /// ID des Antrags
        /// </summary>
        public long sysID
        {
            get;
            set;
        }

        /// <summary>
        /// Zu setzende Stammnummer
        /// </summary>
        public string stammnummer
        {
            get;
            set;
        }
    }
}
