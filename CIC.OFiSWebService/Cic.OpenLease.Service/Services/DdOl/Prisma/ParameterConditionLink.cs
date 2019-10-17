using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Wraps all possible Prisma Param Condition Link Database Tables (PRCLPARSET) to a common ConditionLink Class
    /// 
    /// This is a template for the pattern
    ///     availability for one ConditionType, where ConditionType and all possible ConditionValues are in one row
    ///     e.g.  PRCLPARSET (sysA, sysB, sysC, AREA)
    ///     the conditionid is determined from the correct condition value column while reading the conditionid
    /// </summary>
    public class ParameterConditionLink : ConditionLink
    {
        private long _sysprproduct;
        private long _sysbrand;
        private long _sysprhgroup;
        private long _sysprkgroup;
        private long _sysobtyp;
        private long _sysprparset;
        private long _area;
           
        /// <summary>
        /// Condition ID
        /// </summary>
        new
        public long CONDITIONID
        {
            get
            {
                switch (_area)
                {
                    case (2):
                        return _sysbrand; 
                    case (14):
                        return _sysprproduct;
                    case (31):
                        return _sysobtyp;
                    case (20):
                        return _sysprhgroup;
                    case (40):
                        return _sysprkgroup;
                }
                return 0;
            }
        }

        /// <summary>
        /// sysprproduct
        /// </summary>
        public long sysprproduct
        {
            get { return _sysprproduct; }
            set { _sysprproduct = value; }
        }

        /// <summary>
        /// sysprparset
        /// </summary>
        public long sysprparset
        {
            get { return _sysprparset; }
            set { TARGETID = value;  _sysprparset = value; }
        }

        /// <summary>
        /// area
        /// </summary>
        public long area
        {
            get { return _area; }
            set { _area = value; }
        }

        /// <summary>
        /// sysbrand
        /// </summary>
        public long sysbrand
        {
            get { return _sysbrand; }
            set { _sysbrand = value; }
        }

        /// <summary>
        /// sysprhgroup
        /// </summary>
        public long sysprhgroup
        {
            get { return _sysprhgroup; }
            set { _sysprhgroup = value; }
        }

        /// <summary>
        /// sysobtyp
        /// </summary>
        public long sysobtyp
        {
            get { return _sysobtyp; }
            set {  _sysobtyp = value; }
        }

        /// <summary>
        /// sysprkgroup
        /// </summary>
        public long sysprkgroup
        {
            get { return _sysprkgroup; }
            set {  _sysprkgroup = value; }
        }


    }
}
