namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class ITKNEDto
    {
        [System.Runtime.Serialization.DataMember]
        public long SYSOBER{get;set;}

        [System.Runtime.Serialization.DataMember]
        public long SYSUNTER { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string relateTypeCode { get; set; }

        [System.Runtime.Serialization.DataMember]
        public long SYSANGEBOT { get; set; }

        [System.Runtime.Serialization.DataMember]
        public decimal? QUOTE { get; set; }
        [System.Runtime.Serialization.DataMember]
        public string codeRelateKind { get; set; }

    }
}