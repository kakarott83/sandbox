using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Wraps the Provision condition link  (PRCLPROVSET) to a common ConditionLink Class
    /// </summary>
    public class ProvisionConditionLink : ConditionLink
    {
        private long _sysprprovset;
        private long _sysgebiet;
        private long _area;
       
        /// <summary>
        /// Area
        /// </summary>
        public long gebiet
        {
            get { return _area; }
            set { _area = value; }
        }

        /// <summary>
        /// Area
        /// </summary>
        public long area
        {
            get { return _area; }
            set { _area = value; }
        }

        /// <summary>
        /// SysID
        /// </summary>
        public long sysgebiet
        {
            get { return _sysgebiet; }
            set { CONDITIONID = value; _sysgebiet = value; }
        }
        /// <summary>
        /// SysID
        /// </summary>
        public long sysid
        {
            get { return _sysgebiet; }
            set { CONDITIONID = value; _sysgebiet = value; }
        }

        /// <summary>
        /// sysprprovset
        /// </summary>
        public long sysprprovset
        {
            get { return _sysprprovset; }
            set { TARGETID = value; _sysprprovset = value; }
        }
       

    }
}
