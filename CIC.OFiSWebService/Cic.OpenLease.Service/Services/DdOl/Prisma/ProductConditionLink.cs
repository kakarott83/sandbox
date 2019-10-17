﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Wraps all possible Prisma Product Condition Link Database Tables to a common ConditionLink Class
    /// 
    /// This is a template for the pattern
    ///     availability for one ConditionType is in a special table for the conditiontype
    ///     e.g.  prclprbchnl, prclprbr
    ///     the conditionid is assigned by setting it while reading the conditionvalue from the table
    /// </summary>
    public class ProductConditionLink : ConditionLink
    {
        private long _sysprproduct;
        private long _sysbchannel;
        private long _sysbrand;
        private long _sysprhgroup;
        private long _sysobtyp;
        private long _sysobart;
        private long _sysobusetype;
        private long _syskdtyp;
        private long _sysperole;
        private long _sysprmart;

        /// <summary>
        /// Aktiv Flag
        /// </summary>
        public long? ACTIVEFLAG { get; set; }

        /// <summary>
        /// SYSPRODUCT
        /// </summary>
        public long SYSPRPRODUCT
        {
            get { return _sysprproduct; }
            set { TARGETID = value; _sysprproduct = value; }
        }

        /// <summary>
        /// SYSPRODUCT
        /// </summary>
        public long SYSPEROLE
        {
            get { return _sysperole; }
            set { CONDITIONID = value; _sysperole = value; }
        }

        /// <summary>
        /// SYSMART
        /// </summary>
        public long sysprmart
        {
            get { return _sysprmart; }
            set { CONDITIONID = value; _sysprmart = value; }
        }


        /// <summary>
        /// Sysbchannel
        /// </summary>
        public long sysbchannel
        {
            get { return _sysbchannel; }
            set { CONDITIONID = value; _sysbchannel = value; }
        }

        /// <summary>
        /// sysbrand
        /// </summary>
        public long sysbrand
        {
            get { return _sysbrand; }
            set { CONDITIONID = value; _sysbrand = value; }
        }

        /// <summary>
        /// sysprhgroup
        /// </summary>
        public long sysprhgroup
        {
            get { return _sysprhgroup; }
            set { CONDITIONID = value; _sysprhgroup = value; }
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
        /// sysobusetype
        /// </summary>
        public long sysobusetype
        {
            get { return _sysobusetype; }
            set { CONDITIONID = value; _sysobusetype = value; }
        }

        /// <summary>
        /// sysobusetype
        /// </summary>
        public long syskdtyp
        {
            get { return _syskdtyp; }
            set { CONDITIONID = value; _syskdtyp = value; }
        }
    }
}
