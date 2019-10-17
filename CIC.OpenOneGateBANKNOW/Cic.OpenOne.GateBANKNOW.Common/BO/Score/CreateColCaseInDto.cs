using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    class ColCaseRNDto
    {
        public long CustomerReference { get; set; }
        public String InvoiceID { get; set; }
        public String InvoiceNumber { get; set; }
        public decimal InvoiceOriginalAmount { get; set; }
        public decimal InvoiceOpenAmount { get; set; }
        public decimal LateInterestAmount { get; set; }
        public String InvoiceText { get; set; }
        public String InvoiceNumberAlphabet { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public DateTime InvoicePostingDate { get; set; }
        public DateTime lateinterestupdatedate { get; set; }
        public String DDReturnReasonCode { get; set; }
    }
    class ColCaseOBDto
    {
        public String CarBrand { get; set; }
        public String CarModel { get; set; }
        public String CarVIN { get; set; }
        public DateTime Car1stRegistrationDate { get; set; }
        public String CarLicencePlate { get; set; }
        public String CarColor { get; set; }
    }
    class ColCaseVTDto
    {
        public String ContractReference { get; set; }
        public decimal Deposit { get; set; }
        public long syskd { get; set; }
        public int isSpecialCase { get; set; }
    }
	class CreateColCaseInDto
	{
        //type of this data like PERSON,VT,OB,RN
        public String DataType { get; set; }
        public String DataId { get; set; }
        public String DataAlias { get; set; }
        //Person
        public String CreditLimit { get; set; }
        public String ClaimedCreditLimit { get; set; }
        public String Insolvent { get; set; }
        public String InsolventFollowupDate { get; set; }
        public String Deceased { get; set; }
        public String DeceasedCaseStarted { get; set; }
        public String DeceasedFollowupDate { get; set; }
        //VT
        public String ArrearsBalance { get; set; }
        public String RemainingBalance { get; set; }
        public String LateInterest { get; set; }
        public String 	Interest	 { get; set; }
        public String 	DunningActivityDate	 { get; set; }
        public String 	PurchaseFlagCustomer	{ get; set; }
        public String 	RepossessionOrderCMS	 { get; set; }
        public String 	RepossessionFollowupDate	 { get; set; }
        public String 	VehicleRepossessed 	 { get; set; }
        public String 	VehicleExpertiseOrder	 { get; set; }
        public String 	VehicleExpertiseFollowupDate	 { get; set; }
        public String 	DateVehicleExpertiseCompleted	 { get; set; }
        public String 	EstimatedSalesPriceVehicle	 { get; set; }
        public String 	DateOfSale	 { get; set; }
        public String 	WriteOffDate	 { get; set; }
        public String 	WriteOffAmount	 { get; set; }
        public String 	WriteOffFollowupDate	{ get; set; }
        public String 	SolicitorFlag	 { get; set; }
        public String 	SolicitorHandoverDate	 { get; set; }
        public String 	SolicitorFollowupDate	 { get; set; }

		public String FuturePayArrFlag { get; set; }			// 20180111
		public String FuturePayArrStartDate { get; set; }       
		public String FuturePayArrEndDate { get; set; }        
		public String DetailFuturePayArr { get; set; }

		public String FuturePtPFlag { get; set; }               // 20180111
		public String FuturePtPStartDate { get; set; }          
		public String FuturePtPEndDate { get; set; }           
		public String DetailFuturePtP { get; set; }



		//RN
		public String InvoiceType { get; set; }
        public String InvoiceOpenAmount { get; set; }
        /*
        public String CustomerReference { get; set; }
        public String InvoiceID { get; set; }
        public String InvoiceNumber { get; set; }
        
        public String InvoiceOriginalAmount { get; set; }
        
        public String LateInterestAmount { get; set; }
        public String InvoiceText { get; set; }
        public String InvoiceNumberAlphabet { get; set; }
        public String InvoiceDueDate { get; set; }
        public String InvoicePostingDate { get; set; }
        public String DDReturnReasonCode { get; set; }*/
        //OB
        
        public String CarBrand { get; set; }
        public String CarModel { get; set; }
        public String CarVIN { get; set; }
        public String Car1stRegistrationDate { get; set; }
        public String CarLicencePlate { get; set; }
        public String CurrentCarValue { get; set; }
        public String CarColor { get; set; }

        
	}
}
