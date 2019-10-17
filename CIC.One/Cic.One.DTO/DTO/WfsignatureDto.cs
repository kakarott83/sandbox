using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public enum WfsignatureType : int
    {
        /// <summary>
        /// Allgmeine Signatur
        /// </summary>
        Allgemein = 1,

        /// <summary>
        /// Standard Neue E-mail
        /// </summary>
        NeueMail = 2,

        /// <summary>
        /// Standard Antwort
        /// </summary>
        Antwort = 3,

        /// <summary>
        /// Standard Weiterleitung
        /// </summary>
        Weiterleitung = 4
    }

    public class WfsignatureDto : EntityDto 
    {
        /// <summary>
        /// SysId
        /// </summary>
        public long sysWfsignature { get; set; }

        /// <summary>
        /// Wfuser
        /// </summary>
        public long sysWfuser { get; set; }

        /// <summary>
        /// Typ der Signatur
        /// </summary>
        public WfsignatureType typ { get; set; }

        /// <summary>
        /// Signatur-Text
        /// </summary>
        public string mailsignature { get; set; }

        public override long getEntityId()
        {
            return sysWfsignature;
        }
    }
}