using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    public enum VGRefType
    {
        /// <summary>
        /// Restwert
        /// </summary>
        RW,
        /// <summary>
        /// Restwertabsicherung
        /// </summary>
        RWA
    }
    /// <summary>
    /// Eurotax-Quelle
    /// </summary>
    public enum EurotaxSource
    {
        /// <summary>
        /// Eurotax
        /// </summary>
        EurotaxForecast,
        /// <summary>
        /// RW Table
        /// </summary>
        InternalTableRW,
        /// <summary>
        /// Remo
        /// </summary>
        InternalTableRemo,
        /// <summary>
        /// Valuation
        /// </summary>
        EurotaxValuation,
        /// <summary>
        /// RW-Zuordnung VGREF-Tabelle
        /// </summary>
        InternalTableVGREF_RW
    }

    /// <summary>
    /// Eurotax Output Data Transfer Objects
    /// </summary>
    [System.CLSCompliant(false)]
    public class EurotaxOutDto
    {
        /// <summary>
        /// Restwert Brutto (incl Mwst) (GetForecast)
        /// </summary>
        [AttributeFilter("GetForecast")]
        public double TradeAmount { get; set; }

        /// <summary>
        /// Restwert "Handel" in Prozent (GetForecast)
        /// </summary>
        [AttributeFilter("GetForecast")]
        public double TradeValueInPercentage { get; set; }

        /// <summary>
        /// Restwert "Einzelhandel" Brutto (incl Mwst) (GetForecast)
        /// </summary>
        [AttributeFilter("GetForecast")]
        public double RetailAmount { get; set; }

        /// <summary>
        /// Restwert "Einzelhandel" in Prozent (GetForecast)
        /// </summary>
        [AttributeFilter("GetForecast")]
        public double RetailValueInPercentage { get; set; }

        /// <summary>
        /// Fehlerbeschreibung (GetForecast / GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Fehlercode (GetForecast / GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public int ErrorCode { get; set; }

        /// <summary>
        ///  (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double BasicResidualTradeAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double BasicResidualRetailAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double BasicResidualB2BAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double MileageAdjustmentTradeAmount { get; set; }

        ///<summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double MileageAdjustmentRetailAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double MileageAdjustmentB2BAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double MonthlyAdjustmentTradeAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double MonthlyAdjustmentRetailAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double MonthlyAdjustmentB2BAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double ActualNewPrice { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public string ActualNewPriceIndicator { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        /// <remarks>HR: uint is not CLS compliant MP is informed we agreed to ignore this for now</remarks>
        [AttributeFilter("GetValuation")]
        public uint AverageMileage { get; set; }

        /// <summary>
        /// ForecastPeriod
        /// </summary>
        public long ForecastPeriod { get; set; }

        /// <summary>
        /// true when data comes from internal rv tables
        /// </summary>
        public EurotaxSource source { get; set; }


        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double TotalValuationB2BAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double TotalValuationTradeAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        [AttributeFilter("GetValuation")]
        public double TotalValuationRetailAmount { get; set; }
    }
}