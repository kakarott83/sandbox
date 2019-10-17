namespace Cic.OpenLease.ServiceAccess.DdOl
{

    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class PriceDto
    {

        #region Properties
        public decimal NetPrice
        {
            get;
            set;
        }

        public decimal GrossPrice
        {
            get;
            set;
        }

        public decimal Tax
        {
            get;
            set;
        }

        public decimal NetUnitPrice
        {
            get;
            set;
        }

        public decimal GrossUnitPrice
        {
            get;
            set;
        }

        public decimal UnitTax
        {
            get;
            set;
        }

        public decimal Price_Default
        {
            get;
            set;
        }
        public decimal Price_Subvention
        {
            get;
            set;
        }
        public decimal Price_SubventionUst
        {
            get;
            set;
        }

        public decimal Price_SubventionNetto
        {
            get;
            set;
        }
        #endregion
    }
}
