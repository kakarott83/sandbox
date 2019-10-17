namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class BasicProvisionDto
    {
        #region Properties
        public decimal BasicPrice
        {
            get;
            set;
        }

        public decimal ProvisionPercentage
        {
            get;
            set;
        }

        public decimal ProvisionValue
        {
            get;
            set;
        }
        #endregion
    }
}
