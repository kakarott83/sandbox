using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.Merge.OlClient
{
    /// <summary>
    /// Activity State info for current user
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class ActivityInfo
    {
        [System.Runtime.Serialization.DataMember]
        public int EOTCOUNT {get;set;}

        [System.Runtime.Serialization.DataMember]
        public int CAMPCOUNT { get; set; }

        [System.Runtime.Serialization.DataMember]
        public int ACTIVITYCOUNT { get; set; }
    }
}
