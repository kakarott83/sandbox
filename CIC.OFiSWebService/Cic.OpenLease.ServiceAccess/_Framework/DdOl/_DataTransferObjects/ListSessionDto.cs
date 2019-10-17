
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class ListSessionDto
    {
        /// <summary>
        /// Gets or sets the session id
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string Sid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Bezeichner
        /// Bezeichner consists of Angebotsnr / Fahrzeughersteller / Fahrzeugbezeichnung / Kundenname
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string Bezeichner
        {
            get;
            set;
        }

    }
}
