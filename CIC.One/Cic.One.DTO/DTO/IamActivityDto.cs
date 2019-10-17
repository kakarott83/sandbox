using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    /// <summary>
    /// Interaktionsmuster-Aktivität
    /// </summary>
    public class IamActivityDto : EntityDto
    {
       
        /// <summary>
        /// Primärschlüssel
        /// </summary>
        public long sysIamActivity { get; set; }
        /// <summary>
        /// Fremdschlüssel auf Interaktionsmuster
        /// </summary>
        public long sysIam { get; set; }
        /// <summary>
        /// Typ der Aktivität
        /// </summary>
        public long sysActivityType { get; set; }
        /// <summary>
        /// Benutzer
        /// </summary>
        public long syswfuser { get; set; }

        /// <summary>
        /// Fälligkeitsmonat
        /// </summary>
        public int dueMonth { get; set; }
        /// <summary>
        /// Fälligkeitstag
        /// </summary>
        public int dueDay { get; set; }
        /// <summary>
        /// Initiale Art der Aktivität
        /// </summary>
        public int art { get; set; }
        /// <summary>
        /// Initialstatus
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// Rang
        /// </summary>
        public int rang { get; set; }

        /// <summary>
        /// Fälligkeits-Bezugsfeld
        /// </summary>
        public String dueField { get; set; }

        public DateTime? validFrom { get; set; }
        public DateTime? validUntil { get; set; }

        override public long getEntityId()
        {
            return sysIamActivity;
        }
        public override string getEntityBezeichnung()
        {
            return "IAMA" + sysIamActivity;
        }

    }
}