using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Decision Engine Data Transfer Object
    /// </summary>
    public class DecisionEngineOutDto
    {
        #region Header      
        /// <summary>
        ///  Getter/Setter Inquiry Code
        /// </summary>
        public string InquiryCode { get; set; }

        /// <summary>
        ///  Getter/Setter Process Code
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        ///  Getter/Setter Organization Code
        /// </summary>
        public string OrganizationCode { get; set; }

        /// <summary>
        ///  Getter/Setter Process Version
        /// </summary>
        public int? ProcessVersion { get; set; }
        #endregion

        #region Body
        #region RecordNR

        /// <summary>
        ///  Getter/Setter Inquiry Date
        /// </summary>
        public DateTime? InquiryDate { get; set; }

        /// <summary>
        ///  Getter/Setter Inquiry Time
        /// </summary>
        public string InquiryTime { get; set; }

        /// <summary>
        ///  Getter/Setter System Decision Time
        /// </summary>
        public string System_Decision { get; set; }

        /// <summary>
        ///  Getter/Setter System Decision Group
        /// </summary>
        public string System_Decision_Group { get; set; }

        /// <summary>
        ///  Getter/Setter Random Number
        /// </summary>
        public decimal? RandomNumber { get; set; }

        /// <summary>
        ///  Getter/Setter Error code
        /// </summary>
        public decimal? Fehlercode { get; set; }

        /// <summary>
        ///  Getter/Setter Reserve Number 1
        /// </summary>
        public decimal? ReserveNumber1 { get; set; }

        /// <summary>
        ///  Getter/Setter Reserve Number 2
        /// </summary>
        public decimal? ReserveNumber2 { get; set; }

        /// <summary>
        ///  Getter/Setter reserve Number 3
        /// </summary>
        public decimal? ReserveNumber3 { get; set; }

        /// <summary>
        ///  Getter/Setter Reserve Date 1
        /// </summary>
        public DateTime? ReserveDate1 { get; set; }

        /// <summary>
        ///  Getter/Setter Reserve Date 2
        /// </summary>
        public DateTime? ReserveDate2 { get; set; }

        /// <summary>
        ///  Getter/Setter Reserve Date 3
        /// </summary>
        public DateTime? ReserveDate3 { get; set; }

        /// <summary>
        ///  Getter/Setter Reserve Text 1
        /// </summary>
        public string ReserveText1 { get; set; }

        /// <summary>
        /// Getter/Setter Reserve Text 2
        /// </summary>
        public string ReserveText2 { get; set; }

        /// <summary>
        /// Getter/Setter Reserve Text 3
        /// </summary>
        public string ReserveText3 { get; set; }

        /// <summary>
        /// Getter/Setter DEC Processstep ID
        /// </summary>
        public decimal? DEC_ProzessschrittID { get; set; }

        /// <summary>
        /// Getter/Setter DEC State
        /// </summary>
        public string DEC_Status { get; set; }

        /// <summary>
        /// Getter/Setter State ID
        /// </summary>
        public decimal? DEC_StatusID { get; set; }

        /// <summary>
        /// Getter/Setter DEC Ampel
        /// </summary>
        public string DEC_Ampel { get; set; }

        /// <summary>
        /// Getter/Setter DEC Rules Code
        /// </summary>
        public string DEC_Regelwerk_Code { get; set; }

        /// <summary>
        ///  Getter/Setter DEC Kundengruppe
        /// </summary>
        public string DEC_Kundengruppe { get; set; }

        /// <summary>
        /// Getter/Setter _Freibetrag
        /// </summary>
        public decimal? DEC_Freibetrag { get; set; }

        /// <summary>
        /// Getter/Setter Mindestmiete
        /// </summary>
        public decimal? DEC_Mindestmiete { get; set; }

        /// <summary>
        /// Getter/Setter FaktorBP
        /// </summary>
        public decimal? DEC_FaktorBP { get; set; }

        /// <summary>
        /// Getter/Setter FaktorZ
        /// </summary>
        public decimal? DEC_FaktorZ { get; set; }

        /// <summary>
        /// Getter/Setter Fraud_Flag
        /// </summary>
        public decimal? DEC_Fraud_Flag { get; set; }

        /// <summary>
        /// Getter/Setter Fraud_Score
        /// </summary>
        public decimal? DEC_Fraud_Score { get; set; }

        /// <summary>
        /// Getter/Setter TR_Segment
        /// </summary>
        public string DEC_TR_Segment { get; set; }

        /// <summary>
        /// Getter/Setter Betreuungskosten
        /// </summary>
        public decimal? DEC_Betreuungskosten { get; set; }

        #endregion
        #region RecordRR

        /// <summary>
        /// Getter/Setter RecordRRResponseDto
        /// </summary>
        public RecordRRResponseDto[] RecordRRResponseDto { get; set; }
       
        
        #endregion

        #endregion

        #region Error

        /// <summary>
        /// Getter/Setter Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Getter/Setter Error Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Getter/Setter Engine version
        /// </summary>
        public string EngineVersion { get; set; }

        /// <summary>
        /// Getter/Setter Engine Stack Trace
        /// </summary>
        public string EngineStackTrace { get; set; }

        /// <summary>
        /// Getter/Setter Java Stack Trace
        /// </summary>
        public string JavaStackTrace { get; set; }
        #endregion

        /// <summary>
        /// Getter/Setter input Data Transfer Object
        /// </summary>
        public DecisionEngineInDto inDto { get; set; }

        /// <summary>
        /// Getter/Setter RR Data Transfer Object
        /// </summary>
        public RecordRRDto rrDto { get; set; }
      
        /// <summary>
        /// Getter/Setter Response Object
        /// </summary>
        public DTO.StrategyOneResponse response { get; set; }

        /// <summary>
        /// Getter/Setter Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Getter/Setter Errorcode
        /// </summary>
        public int ErrorCode { get; set; }
    }
}
