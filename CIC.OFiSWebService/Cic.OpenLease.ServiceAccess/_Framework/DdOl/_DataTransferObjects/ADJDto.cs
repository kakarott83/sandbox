using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Container Class to hold return values for the VGADJDao Method
    /// 
    /// </summary>
    [System.CLSCompliant(true)]
    public class ADJDto
    {
        private double _adjvalue;
        private long _sysvg;

        /// <summary>
        /// The correction Value of a correction table
        /// </summary>
        public double adjvalue
        {
            get { return _adjvalue; }
            set { _adjvalue = value; }
        }

        /// <summary>
        /// the Value Group key linked to the correction value
        /// </summary>
        public long sysvg
        {
            get { return _sysvg; }
            set { _sysvg = value; }
        }
    }
}