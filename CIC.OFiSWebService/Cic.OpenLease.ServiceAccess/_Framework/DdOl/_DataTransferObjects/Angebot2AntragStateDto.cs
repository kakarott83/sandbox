// OWNER WB, 22-04-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System.Xml.Serialization;
    using Cic.OpenLease.ServiceAccess.Merge.OlClient;
    #endregion
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public sealed class Angebot2AntragStateDto
    {

        #region Properties
        [System.Runtime.Serialization.DataMember]
        public long SysEaiHot
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public EaiHotStatusConstants Status
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public EaiErrorDto[] Errors
        {
            get;
            set;
        }
        #endregion

    }
}
