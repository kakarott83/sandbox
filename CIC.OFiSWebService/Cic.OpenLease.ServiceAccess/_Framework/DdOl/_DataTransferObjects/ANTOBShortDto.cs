// OWNER MK, 01-09-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANTOBShortDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public ANTKALKShortDto[] ANTKALKShortDtos
        {
            get;
            set;
        }
        #endregion
    }
}
