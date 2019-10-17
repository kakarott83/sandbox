using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IBANKernel
{
    public class IBANInfo
    {
        [DataMember(Order = 1)]
        public String bankId { get; set; }
        [DataMember(Order = 2)]
        public String bankName { get; set; }
        [DataMember(Order = 3)]
        public String iban { get; set; }

    }

    public class IBANVersionInfo
    {
        [DataMember(Order = 1)]
        public String majorVersion { get; set; }
        [DataMember(Order = 2)]
        public String minorVersion { get; set; }
        [DataMember(Order = 3)]
        public String validUntil { get; set; }
        [DataMember(Order = 4)]
        public int status { get; set; }

    }

}