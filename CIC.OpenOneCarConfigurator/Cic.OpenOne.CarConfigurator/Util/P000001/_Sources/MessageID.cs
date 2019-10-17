using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cic.P000001.Common
{

    // This is the custom SoapHeader to get the MessageId and
    // conform to WS-Addressing specifications.
    [XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
    [System.CLSCompliant(true)]
    public class MessageID : System.Web.Services.Protocols.SoapHeader
    {
        [XmlText()]
        public string messageId;

        // This is the property to set and get the GUID value.
        [XmlIgnore]
        public Guid Id
        {
            get
            {
                if (messageId == null) return Guid.Empty;
                string[] parts = messageId.Split(':');
                return new Guid(parts[1]);
            }
            set
            {
                this.messageId = "uuid:" + value.ToString();
            }
        }
    }
}