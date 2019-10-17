// OWNER MK, 16-02-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANGKALKShortDto
    {
        // Fahrzeugbezeichnung ANGOB:BEZEICHNUNG
        
        [System.Runtime.Serialization.DataMember]
        public string BEZEICHNUNG
        {
            get;
            set;
        }
    }
}
