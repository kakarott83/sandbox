using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.AuskunftService.EurotaxGetForecast"/> Methode
    ///                 und <see cref="Cic.OpenOne.GateBANKNOW.Service.AuskunftService.EurotaxGetValuation"/> Methode
    /// </summary>
    public class oEurotaxDto : oBaseDto
    {
        /// <summary>
        /// Restwert Brutto (incl Mwst) (GetForecast)
        /// </summary>
        public double TradeAmount { get; set; }

        /// <summary>
        /// Restwert "Handel" in Prozent (GetForecast)
        /// </summary>
        public double TradeValueInPercentage { get; set; }

        /// <summary>
        /// Restwert "Einzelhandel" Brutto (incl Mwst) (GetForecast)
        /// </summary>
        public double RetailAmount { get; set; }

        /// <summary>
        /// Restwert "Einzelhandel" in Prozent (GetForecast)
        /// </summary>
        public double RetailValueInPercentage { get; set; }

        /// <summary>
        /// Fehlerbeschreibung (GetForecast / GetValuation)
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Fehlercode (GetForecast / GetValuation)
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        ///  (GetValuation)
        /// </summary>
        public double BasicResidualTradeAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        public double BasicResidualRetailAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        public double BasicResidualB2BAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        public double MileageAdjustmentTradeAmount { get; set; }

        ///<summary>
        /// (GetValuation)
        /// </summary>
        public double MileageAdjustmentRetailAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        public double MileageAdjustmentB2BAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        public double MonthlyAdjustmentTradeAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        public double MonthlyAdjustmentRetailAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        public double MonthlyAdjustmentB2BAmount { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        public double ActualNewPrice { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        public string ActualNewPriceIndicator { get; set; }

        /// <summary>
        /// (GetValuation)
        /// </summary>
        /// <remarks>HR: uint is not CLS compliant MP is informed we agreed to ignore this for now</remarks>
        public uint AverageMileage { get; set; }
    }
}
