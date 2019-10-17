namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class ManagementFeeDto : PriceDto
    {
        #region Properties
        public BearbeitungsgebuhrDto Bearbaitungsgebuhr
        {
            get;
            set;
        }
        #endregion
    }
}
