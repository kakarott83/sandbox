using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    /// <summary>
    /// Interaktionsmuster
    /// </summary>
    public class IamDto : EntityDto
    {
    
        /// <summary>
        /// Primärschlüssel
        /// </summary>
        public long sysIam { get; set; }
        /// <summary>
        /// Typ des Interaktionsmusters
        /// </summary>
        public long sysIamType { get; set; }

        public int triggerMonth { get; set; }
        public int triggerDay { get; set; }
        
        /// <summary>
        /// Fälligkeits-Bezugsfeld
        /// </summary>
        public String triggerField { get; set; }

        public DateTime? validFrom { get; set; }
        public DateTime? validUntil { get; set; }

        public String code { get; set; }
        public String beschreibung { get; set; }

        override public long getEntityId()
        {
            return sysIam;
        }
        public override string getEntityBezeichnung()
        {
            return "IAM" + sysIam;
        }

    }
}