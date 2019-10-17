using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Wraps Interest Conditions to a common ConditionLink Class
    /// 
    /// This is a template for the pattern
    ///     availability for one ConditionType, where ConditionType and all possible ConditionValues are in one row
    ///     e.g.  PRCLPARSET (sysA, sysB, sysC, AREA)
    ///     the conditionid is determined from the correct condition value column while reading the conditionid
    /// </summary>
    public class InterestConditionLink : ConditionLink
    {
        //condition-ids
        private long _sysbrand;
        private long _sysobtyp;
        private long _sysobart;
        private long _sysprkgroup;
        private long _sysprhgroup;
        private long _sysperole;
        private long _sysinttype;

        //condition-switch
        private long _adjustmenttrigger;
        

        //target-references
        /// <summary>
        /// source base
        /// </summary>
        public long sourcebasis { get; set; }

        /// <summary>
        /// sysibor
        /// </summary>
        public long sysibor { get; set; }

        /// <summary>
        /// sysintstrct
        /// </summary>
        public long sysintstrct { get; set; }

        /// <summary>
        /// sysvg
        /// </summary>
        public long sysvg { get; set; }

        /// <summary>
        /// intrate
        /// </summary>
        public double intrate { get; set; }

        /// <summary>
        /// rank
        /// </summary>
        public long rank { get; set; }

        /// <summary>
        /// method
        /// </summary>
        public long method { get; set; }

        /// <summary>
        /// sysprintset
        /// </summary>
        public long sysprintset { get;set;}
     

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
                    case (5):
                        return _sysobart;
                    case (6):
                        return _sysinttype;
                }
                return 0;
            }
        }

        /// <summary>
        /// Condition Type
        /// </summary>
        public ConditionLinkType CONDITIONTYPE
        {
            get
            {
                switch (_adjustmenttrigger)
                {
                    case (0):
                        return ConditionLinkType.BRAND;
                    case (1):
                        return ConditionLinkType.OBTYP;
                    case (2):
                        return ConditionLinkType.PRHGROUP;
                    case (3):
                        return ConditionLinkType.PRKGROUP;
                    case (4):
                        return ConditionLinkType.PEROLE;
                    case (5):
                        return ConditionLinkType.OBART;
                    case (6):
                        return ConditionLinkType.INTTYPE;
                    case (99):
                        return ConditionLinkType.COMMON;
                }
                return ConditionLinkType.NONE;
            }

            set
            {
                switch(value)
                {
                    case (ConditionLinkType.BRAND):
                        _adjustmenttrigger = 0;
                        break;
                    case (ConditionLinkType.OBTYP):
                        _adjustmenttrigger = 1;
                        break;
                    case (ConditionLinkType.PRHGROUP):
                        _adjustmenttrigger = 2;
                        break;
                    case (ConditionLinkType.PRKGROUP):
                        _adjustmenttrigger = 3;
                        break;
                    case (ConditionLinkType.PEROLE):
                        _adjustmenttrigger = 4;
                        break;
                    case (ConditionLinkType.OBART):
                        _adjustmenttrigger = 5;
                        break;
                    case (ConditionLinkType.INTTYPE):
                        _adjustmenttrigger = 6;
                        break;
                    case (ConditionLinkType.COMMON):
                        _adjustmenttrigger = 99;
                        break;
                }
            }
        }

        /// <summary>
        /// Target ID
        /// </summary>
        new
        public long TARGETID
        {
            get
            {
                switch (sourcebasis)
                {
                    case (0):
                        return sysibor;
                    case (1):
                        return sysintstrct;
                    case (2):
                        return sysvg;
                    case (3):
                        return -1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Adjustment Trigger
        /// </summary>
        public long adjustmenttrigger
        {
            get { return _adjustmenttrigger; }
            set { _adjustmenttrigger = value; }
        }

        /// <summary>
        /// SysIntType
        /// </summary>
        public long sysinttype
        {
            get { return _sysinttype; }
            set {  _sysinttype = value; }
        }

        /// <summary>
        /// SysPerole
        /// </summary>
        public long sysperole
        {
            get { return _sysperole; }
            set { _sysperole = value; }
        }

        /// <summary>
        /// SysBrand
        /// </summary>
        public long sysbrand
        {
            get { return _sysbrand; }
            set {  _sysbrand = value; }
        }

        /// <summary>
        /// SysPrhGroup
        /// </summary>
        public long sysprhgroup
        {
            get { return _sysprhgroup; }
            set {  _sysprhgroup = value; }
        }

        /// <summary>
        /// SysPrkGroup
        /// </summary>
        public long sysprkgroup
        {
            get { return _sysprkgroup; }
            set {  _sysprkgroup = value; }
        }

        /// <summary>
        /// SysObTyp
        /// </summary>
        public long sysobtyp
        {
            get { return _sysobtyp; }
            set {  _sysobtyp = value; }
        }

        /// <summary>
        /// SysObArt
        /// </summary>
        public long sysobart
        {
            get { return _sysobart; }
            set {  _sysobart = value; }
        }
       

    }
}
