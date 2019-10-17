namespace Cic.OpenLease.Service
{

    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    #endregion

    public class VehicleData
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

        public string BrandName
        {
            get;
            set;
        }

        public string ManufacturerName
        {
            get;
            set;
        }

        public int ManufacturedYear
        {
            get;
            set;
        }
        /// <summary>
        /// netto, brutto will include nova and novazuschlag
        /// </summary>
        public decimal Price
        {
            get;
            set;
        }

        
        public decimal SARVPriceNetNoNova
        {
            get;
            set;
        }

        public decimal VatPercentage
        {
            get;
            set;
        }

        public VehicleTypeConstants Type
        {
            get;
            set;
        }

        public short AdditionalType
        {
            get;
            set;
        }

        public VehicleOptionData[] Options
        {
            get;
            set;
        }
        #endregion

        public decimal consumptionurban { get; set; }
        public decimal consumptionnonurban { get; set; }
        public decimal consumptionoveral { get; set; }
        /// <summary>
        /// g/km
        /// </summary>
        public decimal co2urban { get; set; }
        /// <summary>
        /// g/km
        /// </summary>
        public decimal co2nonurban { get; set; }
        /// <summary>
        /// g/km
        /// </summary>
        public decimal co2overal { get; set; }

        /// <summary>
        /// Partikelanzahl
        /// </summary>
        public decimal particlecount { get; set; }

        /// <summary>
        /// g/km Dieselpartikel
        /// </summary>
        public decimal particlemass { get; set; }

        /// <summary>
        /// mg/km
        /// </summary>
        public decimal nox { get; set; }

        public decimal novaoekobetrag { get; set; }

        public decimal PriceNettoNetto
        {
            get;
            set;
        }
        public decimal sonderMinderung
        {
            get;
            set;
        }
    }
}