using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Container Class to hold return values for the VGADJDao Method
    /// 
    /// </summary>
    [System.CLSCompliant(true)]
    public class ADJDto
    {
        private decimal _adjvalue;
        private long _sysvg;

        /// <summary>
        /// The correction Value of a correction table
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal adjvalue
        {
            get { return _adjvalue; }
            set { _adjvalue = value; }
        }

        /// <summary>
        /// the Value Group key linked to the correction value
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long sysvg
        {
            get { return _sysvg; }
            set { _sysvg = value; }
        }
    }
}