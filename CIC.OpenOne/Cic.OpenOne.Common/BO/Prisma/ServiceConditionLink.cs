using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Wraps all possible Prisma Product Condition Link Database Tables to a common ConditionLink Class
    /// 
    /// This is a template for the pattern
    ///     availability for one ConditionType is in a special table for the conditiontype
    ///     e.g.  prclprbchnl, prclprbr
    ///     the conditionid is assigned by setting it while reading the conditionvalue from the table
    /// </summary>
    public class ServiceConditionLink : ConditionLink
    {
        private long _sysvstyp;
        private long _sysprkgroup;
        private long _sysobart;
        private long _sysobtyp;
        private long _syskdtyp;

        /// <summary>
        /// Aktiv Flag
        /// </summary>
        public long? ACTIVEFLAG { get; set; }

        /// <summary>
        /// SYSPRODUCT
        /// </summary>
        public long SYSVSTYP
        {
            get { return _sysvstyp; }
            set { TARGETID = value; _sysvstyp = value; }
        }

        /// <summary>
        /// sysprkgroup
        /// </summary>
        public long sysprkgroup
        {
            get { return _sysprkgroup; }
            set { CONDITIONID = value; _sysprkgroup = value; }
        }
       
        /// <summary>
        /// sysobtyp
        /// </summary>
        public long sysobtyp
        {
            get { return _sysobtyp; }
            set { CONDITIONID = value; _sysobtyp = value; }
        }

        /// <summary>
        /// sysobart
        /// </summary>
        public long sysobart
        {
            get { return _sysobart; }
            set { CONDITIONID = value; _sysobart = value; }
        }

        /// <summary>
        /// syskdtyp
        /// </summary>
        public long syskdtyp
        {
            get { return _syskdtyp; }
            set { CONDITIONID = value; _syskdtyp = value; }
        }

    }
}
