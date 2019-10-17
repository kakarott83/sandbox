using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Wraps Provision condition links to a common ConditionLink Class
    /// 
    /// This is a template for the pattern
    ///     availability for one ConditionType, where ConditionType and all possible ConditionValues are in one row
    ///     e.g.  PRCLPARSET (sysA, sysB, sysC, AREA)
    ///     the conditionid is determined from the correct condition value column while reading the conditionid
    /// </summary>
    public class ProvisionAdjustConditionLink : ConditionLink
    {
        private long _sysprproduct;
        private long _sysprprovadjstep;

        //Triggers:
        private long _sysbrand;
        private long _sysprhgroup;
        private long _sysprkgroup;
        private long _sysobtyp;
        private long _sysperole;

        private long _adjustmenttrigger;
        

        /// <summary>
        /// Condition ID
        /// </summary>
        new
        public long CONDITIONID
        {
            get
            {
                switch (_adjustmenttrigger)
                {
                    case (0):
                        return _sysbrand; 
                    case (1):
                        return _sysobtyp;
                    case (2):
                        return _sysprhgroup;
                    case (3):
                        return _sysprkgroup;
                    case (4):
                        return _sysperole;
                }
                return 0;
            }
        }

        /// <summary>
        /// sysprprovadjstep
        /// </summary>
        public long sysprprovadjstep
        {
            get { return _sysprprovadjstep; }
            set { TARGETID = value; _sysprprovadjstep = value; }
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
        /// sysperole
        /// </summary>
        public long sysperole
        {
            get { return _sysperole; }
            set {  _sysperole = value; }
        }

        /// <summary>
        /// adjustmentrigger
        /// </summary>
        public long adjustmenttrigger
        {
            get { return _adjustmenttrigger; }
            set { _adjustmenttrigger = value; }
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
