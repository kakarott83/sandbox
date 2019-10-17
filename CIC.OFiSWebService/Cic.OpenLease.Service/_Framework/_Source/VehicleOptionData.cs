namespace Cic.OpenLease.Service
{

    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    #endregion

    public class VehicleOptionData
    {

        #region Properties
        public string Code
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public OptionTypeConstants Type
        {
            get;
            set;
        }

        public decimal Price
        {
            get;
            set;
        }

        public decimal VatPercentage
        {
            get;
            set;
        }
        #endregion

    }
}