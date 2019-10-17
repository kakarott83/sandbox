using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Transferobjekt für <see cref="Cic.OpenOne.GateBANKNOW.Service.AuskunftService.EurotaxGetValuation"/> Methode
    /// </summary>
    [System.CLSCompliant(false)]
    public class EurotaxGetValuationDto
    {
        /// <summary>
        /// Fehlerbeschreibung (GetValuation)
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Fehlercode (GetValuation)
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

        /// <summary>
        /// true when data comes from internal rv tables
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxSource source { get; set; }
    }
}
