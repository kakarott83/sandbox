using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für S1 Auskunft Methode
    /// </summary>
    [System.CLSCompliant(true)]
    public class iS1GetResponseDto
    {
        public string InquiryCode { get; set; }
        public string OrganizationCode { get; set; }
        public string DecisionProcessCode { get; set; }
        public decimal ProcessVersion { get; set; }
        public decimal LayoutVersion { get; set; }
        public decimal RandomNumber { get; set; }
        public DateTime InquiryDate { get; set; }
        public string InquiryTime { get; set; }
        public decimal O_ErrorCode { get; set; }
        public string O_ErrorDescription { get; set; }
        public decimal O_Reserve_Number_1 { get; set; }
        public decimal O_Reserve_Number_2 { get; set; }
        public decimal O_Reserve_Number_3 { get; set; }
        public DateTime O_Reserve_Date_1 { get; set; }
        public DateTime O_Reserve_Date_2 { get; set; }
        public DateTime O_Reserve_Date_3 { get; set; }
        public string O_Reserve_Text_1 { get; set; }
        public string O_Reserve_Text_2 { get; set; }
        public string O_Reserve_Text_3 { get; set; }
        public decimal O_sysVT { get; set; }
        public decimal O_syskd { get; set; }
        public decimal O_DEC_Risikoklasse_VT { get; set; }
        public decimal O_DEC_Kapital_WB { get; set; }
        public decimal O_DEC_Zins_WB { get; set; }
        public decimal O_DEC_Kapital_WB_ACP { get; set; }
        public decimal O_DEC_Zins_WB_ACP { get; set; }
        public decimal O_DEC_Parameter_ID { get; set; }
        public decimal O_DEC_PDt1 { get; set; }
        public decimal O_DEC_PDt1_frac { get; set; }
    }
}