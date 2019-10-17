using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class BankdatenDto
    {
        public BankdatenDto()
        { }

       
        [System.Runtime.Serialization.DataMember]
        public string IBAN {get;set;}

        [System.Runtime.Serialization.DataMember]
        public string BIC { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string BANKNAME { get; set; }

        /// <summary>
        /// Mandatsart
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int PAYART { get; set; }

        /// <summary>
        /// SEPA-Lastschrift-Flag
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int EINZUG { get; set; }

        /// <summary>
        /// Interessent
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long SYSIT { get; set; }

        [System.Runtime.Serialization.DataMember]
        public long SYSANGEBOT { get; set; }

        /// <summary>
        /// Kontoinhaber
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long SYSKI { get; set; }

        [System.Runtime.Serialization.DataMember]
        public String MANDATSORT { get; set; }

        /// <summary>
        /// Vertragsart
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long SYSVART { get; set; }

        [System.Runtime.Serialization.DataMember]
        public long SYSLS  { get; set; }
    }
}
