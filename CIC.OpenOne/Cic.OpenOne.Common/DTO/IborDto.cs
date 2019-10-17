using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Dto for Ibor
    /// </summary>
    public class IborDto
    {
        /// <summary>
        /// PR Product
        /// </summary>
        public long sysprproduct
        {
            get;
            set;
        }

        /// <summary>
        /// Gültig von
        /// </summary>
        public DateTime validFrom
        {
            get;
            set;
        }

        /// <summary>
        /// Name
        /// </summary>
        public String name
        {
            get;
            set;
        }

        /// <summary>
        /// Währung
        /// </summary>
        public long sysswaehrung
        {
            get;
            set;
        }

        /// <summary>
        /// OVN
        /// </summary>
        public double ovn
        {
            get;
            set;
        }

        /// <summary>
        /// TN
        /// </summary>
        public double tn
        {
            get;
            set;
        }

        /// <summary>
        /// W1
        /// </summary>
        public double w1
        {
            get;
            set;
        }

        /// <summary>
        /// M1
        /// </summary>
        public double m1
        {
            get;
            set;
        }

        /// <summary>
        /// M3
        /// </summary>
        public double m3
        {
            get;
            set;
        }

        /// <summary>
        /// M6
        /// </summary>
        public double m6
        {
            get;
            set;
        }

        /// <summary>
        /// M9
        /// </summary>
        public double m9
        {
            get;
            set;
        }

        /// <summary>
        /// OVM12N
        /// </summary>
        public double ovm12n
        {
            get;
            set;
        }

    }
}
