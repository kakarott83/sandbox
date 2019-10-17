using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Container Class to hold return values for the INTTYPE
    /// 
    /// </summary>
    [System.CLSCompliant(true)]
    public class INTTYPEDto
    {
        
        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long sysINTTYPE
        {
            get ;
            set ;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public String Description
        {
            get ;
            set ;
        }

        [System.Runtime.Serialization.DataMember]
        public int DEFAULT
        {
            get;
            set;
        }
    }
}