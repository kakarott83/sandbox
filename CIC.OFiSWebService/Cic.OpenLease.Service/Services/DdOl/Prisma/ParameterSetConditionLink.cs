using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Wraps all possible Prisma Param Condition Link Database Tables (PRPARSET) to a common ConditionLink Class
    /// </summary>
    public class ParameterSetConditionLink : ConditionLink
    {
        private long _sysprparset;
        private long _sysid;
        private long _area;
        private long _sysparent;
        private long _level;

        /// <summary>
        /// Area
        /// </summary>
        public long area
        {
            get { return _area; }
            set { _area = value; }
        }

        /// <summary>
        /// Parent
        /// </summary>
        public long sysparent
        {
            get { return _sysparent; }
            set { _sysparent = value; }
        }

        /// <summary>
        /// Level
        /// </summary>
        public long level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// SysID
        /// </summary>
        public long sysid
        {
            get { return _sysid; }
            set { CONDITIONID = value; _sysid = value; }
        }

        /// <summary>
        /// SYSPRPARSET
        /// </summary>
        public long sysprparset
        {
            get { return _sysprparset; }
            set { TARGETID = value; _sysprparset = value; }
        }
       

    }
}
