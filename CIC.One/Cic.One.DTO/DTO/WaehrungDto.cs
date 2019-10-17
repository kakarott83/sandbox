using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class WaehrungDto : EntityDto
    {
        override public long getEntityId()
        {
            return sysWaehrung;
        }

        /// <summary>
        /// sysWaehrung
        /// </summary>
        public long	sysWaehrung {get;set;}

        /// <summary>
        /// code
        /// </summary>
        public string code {get;set;}

        /// <summary>
        /// bezeichnung
        /// </summary>
        public string bezeichnung { get; set; }

    }
}