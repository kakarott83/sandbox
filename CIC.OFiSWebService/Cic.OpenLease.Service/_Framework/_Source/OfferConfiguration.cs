namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    #endregion

    public class OfferConfiguration
    {
        #region Properties
        public string OfferId
        {
            get;
            set;
        }

        public OfferTypeConstants Type
        {
            get;
            set;
        }

        public int DealerId
        {
            get;
            set;
        }

        public decimal Nova
        {
            get;
            set;
        }
        //HEK-Fields
        public long sysobart
        {
            get;
            set;
        }
        public long sysobtyp
        {
            get;
            set;
        }
        public string serie
        {
            get;
            set;
        }
        public int? erinklmwst
        {
            get;
            set;
        }
        public DateTime? baujahr
        {
            get;
            set;
        }
        public string schwacke
        {
            get;
            set;
        }

        public decimal TotalPrice
        {
            get
            {
                // Check if vehicle is valid
                if (this.Vehicle == null)
                {
                    // Return price 0
                    return 0;
                }

                // Get the model price
                decimal Price = this.Vehicle.Price;

                // Loop through all options
                foreach (var LoopOption in this.Vehicle.Options)
                {
                    // Update the price
                    Price += LoopOption.Price;
                }

                // Return the price
                return Price;
            }
        }
        /// <summary>
        ///  brutto will include nova and novazuschlag
        /// </summary>
        public decimal TotalDiscount
        {
            get;
            set;
        }
        /// <summary>
        ///  brutto will include nova and novazuschlag
        /// </summary>
        public decimal ModelDiscount
        {
            get;
            set;
        }

        public decimal DownPayment
        {
            get;
            set;
        }
        private decimal _optionPriceOverride = 0;
        /// <summary>
        /// netto, brutto will include nova and novazuschlag
        /// </summary>
        public decimal OptionsPrice
        {
            get
            {
                if (_optionPriceOverride > 0) return _optionPriceOverride;
                return MyGetOptionsPrice(OptionTypeConstants.Option);
            }
            set
            {
                _optionPriceOverride = value;
            }
        }
        /// <summary>
        /// brutto will include nova and novazuschlag
        /// </summary>
        public decimal OptionsDiscount
        {
            get;
            set;
        }
        private decimal _orgmiscPriceOverride = 0;
        public decimal MiscellaneousOptionsPrice
        {
            get
            {
                if (_orgmiscPriceOverride > 0) return _orgmiscPriceOverride;
                return MyGetOptionsPrice(OptionTypeConstants.Miscellaneous);
            }
            set
            {
                _orgmiscPriceOverride = value;
            }
        }

        public decimal MiscellaneousOptionsDiscount
        {
            get;
            set;
        }
        private decimal _orgdealerPriceOverride = 0;
        public decimal DealerAccessoriesPrice
        {
            get
            {
                if (_orgdealerPriceOverride > 0) return _orgdealerPriceOverride;
                return MyGetOptionsPrice(OptionTypeConstants.DealerAccessory);
            }
            set
            {
                _orgdealerPriceOverride = value;
            }
        }

        public decimal DealerAccessoriesDiscount
        {
            get;
            set;
        }
        private decimal _orgaccPriceOverride = 0;

        public decimal OriginalAccessoriesPrice
        {
            get
            {
                if (_orgaccPriceOverride > 0) return _orgaccPriceOverride;
                return MyGetOptionsPrice(OptionTypeConstants.OriginalAccessory);
            }
            set
            {
                _orgaccPriceOverride = value;
            }
        }

        public decimal OriginalAccessoriesDiscount
        {
            get;
            set;
        }
        private decimal _orgPackagePriceOverride = 0;
        /// <summary>
        /// netto, brutto will include nova and novazuschlag
        /// </summary>
        public decimal PackagesPrice
        {
            get
            {
                if (_orgPackagePriceOverride > 0) return _orgPackagePriceOverride;
                return MyGetOptionsPrice(OptionTypeConstants.Package);
            }
            set
            {
                _orgPackagePriceOverride = value;
            }
        }

        /// <summary>
        ///  brutto will include nova and novazuschlag
        /// </summary>
        public decimal PackagesDiscount
        {
            get;
            set;
        }

        public decimal UeberfuehrungBrutto
        {
            get
            {

                return MyGetOptionsPrice(OptionTypeConstants.Ueberfuehrung);
            }
            set
            {

            }
        }
        public decimal ZulassungBrutto
        {
            get
            {
               
                return MyGetOptionsPrice(OptionTypeConstants.Zulassung);
            }
            set
            {
                
            }
        }

        public string Currency
        {
            get;
            set;
        }

        public long Kilometer
        {
            get;
            set;
        }

        public DateTime Erstzulassung
        {
            get;
            set;
        }

        public VehicleData Vehicle
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        /// <summary>
        /// set true when from carconfigurator and the node is not from eurotax-provider but from obtypprovider
        /// </summary>
        public bool IsFromObTyp
        {
            get;
            set;
        }
        /// <summary>
        /// set true when ObTyp.NOEXTID = 1 (when vehicle is marked as manual configured)
        /// </summary>
        public bool UseFzData
        {
            get;
            set;
        }
        public string PolsterCode
        {
            get;
            set;
        }
        public string PolsterText
        {
            get;
            set;
        }
        public string Farbea
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public OfferConfiguration(OfferTypeConstants type)
        {
            this.Type = type;
        }
        public OfferConfiguration()
        {
            
        }
        #endregion

        #region My methods
        private decimal MyGetOptionsPrice(OptionTypeConstants type)
        {
            // Check if vehicle data exists
            if (this.Vehicle == null)
            {
                // Return 0
                return 0;
            }

            // Assume the price is 0
            decimal Price = 0;

            // Loop through all options
            foreach (var LoopOption in this.Vehicle.Options)
            {
                // Check if the option qualifies to be counted
                if (LoopOption.Type == type)
                {
                    // Update the price
                    Price += LoopOption.Price;
                }
            }

            // Retrn the price
            return Price;
        }
        #endregion
    }
}