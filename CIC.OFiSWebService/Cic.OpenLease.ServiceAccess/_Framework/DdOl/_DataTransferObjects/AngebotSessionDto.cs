// OWNER WB, 22-04-2010
using System.Xml.Serialization; 
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    [XmlRootAttribute(ElementName = "AngebotSessionDto", IsNullable = true)] 
    public sealed class AngebotSessionDto
    {
        #region Ids properties
        
        [System.Runtime.Serialization.DataMember]
        public ANGEBOTDto ANGEBOTDto
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public int Step
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public MitantragstellerDto[] Mitantragsteller
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Session ID
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string Sid
        {
            get;
            set;
        }
        #endregion

    }
}
