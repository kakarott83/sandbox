using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// inputParameter für automatischePruefung Methode
    /// </summary>
    public class iautomatischePruefungDto
    {   /// <summary>
        /// antrag
        /// </summary>
        public long sysid
        {
            get;
            set;
        }
    }
}