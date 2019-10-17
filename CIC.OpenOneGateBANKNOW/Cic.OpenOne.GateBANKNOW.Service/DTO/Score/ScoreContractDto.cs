using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	/// <summary>
	/// ScoreContract (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreContractDto
	{
		/// <summary>
		/// customerReference
		/// </summary>
		public long customerReference { get; set; }
		
		/// <summary>
		/// customerRelationshipType
		/// </summary>
		public String customerRelationshipType { get; set; }
		
		/// <summary>
		/// addressDispatchReference
		/// </summary>
		public String addressDispatchReference { get; set; }
		
		/// <summary>
		/// contractReference
		/// </summary>
		public String contractReference { get; set; }
		
		/// <summary>
		/// contractType
		/// </summary>
		public String contractType { get; set; }
		
		/// <summary>
		/// cmsStatus
		/// </summary>
		public String cmsStatus { get; set; }
		
		/// <summary>
		/// ddPossible
		/// </summary>
		public bool ddPossible { get; set; }
		
		/// <summary>
		/// businessLine
		/// </summary>
		public String businessLine { get; set; }
		
		/// <summary>
		/// privateBusinessIndicator
		/// </summary>
		public String privateBusinessIndicator { get; set; }
		
		/// <summary>
		/// contractApplicationDate
		/// </summary>
		public DateTime contractApplicationDate { get; set; }
		
		/// <summary>
		/// contractStartDate
		/// </summary>
		public DateTime contractStartDate { get; set; }
		
		/// <summary>
		/// contractEndDate
		/// </summary>
		public DateTime contractEndDate { get; set; }
		
		/// <summary>
		/// contractEndDateNear
		/// </summary>
		public long contractEndDateNear { get; set; }
		
		/// <summary>
		/// productType
		/// </summary>
		public String productType { get; set; }
		
		/// <summary>
		/// insurance
		/// </summary>
		public String insurance { get; set; }
		
		/// <summary>
		/// contractOriginalValue
		/// </summary>
		public double contractOriginalValue { get; set; }
		
		/// <summary>
		/// contractTerm
		/// </summary>
		public long contractTerm { get; set; }
		
		/// <summary>
		/// currency
		/// </summary>
		public String currency { get; set; }
		
		/// <summary>
		/// dateNextInstalment
		/// </summary>
		public DateTime dateNextInstalment { get; set; }
		
		/// <summary>
		/// regularInstalmentAmountGross
		/// </summary>
		public double regularInstalmentAmountGross { get; set; }
		
		/// <summary>
		/// balloonRateAmount
		/// </summary>
		public double balloonRateAmount { get; set; }
		
		/// <summary>
		/// balloonRateDate
		/// </summary>
		public DateTime balloonRateDate { get; set; }
		
		/// <summary>
		/// finalInvoiceDate
		/// </summary>
		public DateTime finalInvoiceDate { get; set; }
		
		/// <summary>
		/// finalInvoiceAmount
		/// </summary>
		public double finalInvoiceAmount { get; set; }
		
		/// <summary>
		/// contractMileage
		/// </summary>
		public String contractMileage { get; set; }
		
		/// <summary>
		/// iban
		/// </summary>
		public String iban { get; set; }
		
		/// <summary>
		/// bic
		/// </summary>
		public String bic { get; set; }
		
		/// <summary>
		/// houseBankAccountHolder
		/// </summary>
		public String houseBankAccountHolder { get; set; }
		
		/// <summary>
		/// houseBankName
		/// </summary>
		public String houseBankName { get; set; }
		
		/// <summary>
		/// prefComChannel
		/// </summary>
		public String prefComChannel { get; set; }

		//——————————————————————————————————————————————————
		// Contract Dunning Data 
		//——————————————————————————————————————————————————
		
		/// <summary>
		/// arrearsBalance
		/// </summary>
		public double arrearsBalance { get; set; }
		
		/// <summary>
		/// remainingBalance
		/// </summary>
		public double remainingBalance { get; set; }
		
		/// <summary>
		/// dueDateOldestInvoice
		/// </summary>
		public DateTime dueDateOldestInvoice { get; set; }
		
		/// <summary>
		/// interest
		/// </summary>
		public double interest { get; set; }
		
		/// <summary>
		/// dunningActivityDate
		/// </summary>
		public DateTime dunningActivityDate { get; set; }
		
		/// <summary>
		/// applicationRating
		/// </summary>
		public long applicationRating { get; set; }
		
		/// <summary>
		/// instalmentsInArrears
		/// </summary>
		public long instalmentsInArrears { get; set; }
		
		/// <summary>
		/// numberFormerContracts
		/// </summary>
		public long numberFormerContracts { get; set; }
		
		/// <summary>
		/// highestDPD
		/// </summary>
		public long highestDPD { get; set; }
		
		/// <summary>
		/// dateLastMissedPayment
		/// </summary>
		public DateTime dateLastMissedPayment { get; set; }
		
		/// <summary>
		/// percentageOfContractPaid
		/// </summary>
		public double percentageOfContractPaid { get; set; }
		
		/// <summary>
		/// lateInterest
		/// </summary>
		public double lateInterest { get; set; }
		
		/// <summary>
		/// feesCharges
		/// </summary>
		public double feesCharges { get; set; }
		
		/// <summary>
		/// currentAccountBalance
		/// </summary>
		public double currentAccountBalance { get; set; }
		
		/// <summary>
		/// purchaseFlagCustomer
		/// Wenn obvwrt.syskaeufer = vt.syskd dann = Y
		/// </summary>
		public String purchaseFlagCustomer { get; set; }
		
		/// <summary>
		/// outboundCampaignName
		/// </summary>
		public String outboundCampaignName { get; set; }

		//——————————————————————————————————————————————————
		// Contract Termination and Final Invoice Details
		//——————————————————————————————————————————————————
		/// <summary>
		/// terminationDate
		/// </summary>
		public DateTime terminationDate { get; set; }
		
		/// <summary>
		/// requestIDSettlementAmount
		/// (Ablösewertanfrage ID)
		/// </summary>
		public String requestIDSettlementAmount { get; set; }
		
		/// <summary>
		/// settlementAmountForTermination
		/// </summary>
		public double settlementAmountForTermination { get; set; }
		
		/// <summary>
		/// interestRebate
		/// </summary>
		public double interestRebate { get; set; }
		
		/// <summary>
		/// agreedResidualValue
		/// </summary>
		public double agreedResidualValue { get; set; }
		
		/// <summary>
		/// remainingDebt
		/// </summary>
		public double remainingDebt { get; set; }
		
		/// <summary>
		/// remainingContractValue
		/// </summary>
		public double remainingContractValue { get; set; }
		
		/// <summary>
		/// repossessionOrderCMS
		/// OL::ausfall.aktive
		/// </summary>
		public double repossessionOrderCMS { get; set; }
		
		/// <summary>
		/// repossessionFollowupDate
		/// OL::wfmmemo.dat01
		/// </summary>
		public DateTime repossessionFollowupDate { get; set; }

		public ScoreVehicleDto vehicle { get; set; }

		public ScoreInvoiceDto invoice { get; set; }
		

		/// <summary>
		/// vehicleRepossessed
		/// </summary>
		public bool vehicleRepossessed { get; set; }
		
		/// <summary>
		/// dateRepossessionOrder
		/// </summary>
		public DateTime dateRepossessionOrder { get; set; }



		/// <summary>
		/// vehicleExpertiseOrder
		/// </summary>
		public bool vehicleExpertiseOrder { get; set; }
		
		/// <summary>
		/// vehicleExpertiseFollowupDate
		/// </summary>
		public DateTime vehicleExpertiseFollowupDate { get; set; }
		
		/// <summary>
		/// dateVehicleExpertiseCompleted
		/// </summary>
		public DateTime dateVehicleExpertiseCompleted { get; set; }
		
		/// <summary>
		/// estimatedSalesPriceVehicle
		/// </summary>
		public double estimatedSalesPriceVehicle { get; set; }
		
		/// <summary>
		/// salesPriceVehicle
		/// </summary>
		public double salesPriceVehicle { get; set; }
		
		/// <summary>
		/// dateOfSale
		/// obvwrt und obvwrtan (PROJEKT EA_NEU Excel mit Felddefinitionen)
		/// </summary>
		public DateTime dateOfSale { get; set; }
		
		/// <summary>
		/// finalInvoiceReason
		/// </summary>
		public String finalInvoiceReason { get; set; }
		
		/// <summary>
		/// writeOffDate
		/// </summary>
		public DateTime writeOffDate { get; set; }
		
		/// <summary>
		/// writeOffAmount
		/// </summary>
		public double writeOffAmount { get; set; }
		
		/// <summary>
		/// writeOffFollowupDate
		/// </summary>
		public DateTime writeOffFollowupDate { get; set; }

		//——————————————————————————————————————————————————
		// Handover to Solicitor
		//——————————————————————————————————————————————————
		
		/// <summary>
		/// solicitorFlag
		/// </summary>
		public String solicitorFlag { get; set; }
		
		/// <summary>
		/// solicitorHandoverDate
		/// </summary>
		public String solicitorHandoverDate { get; set; }
		
		/// <summary>
		/// solicitorFollowupDate
		/// </summary>
		public DateTime solicitorFollowupDate { get; set; }

		//——————————————————————————————————————————————————
		// Collateral Details
		//——————————————————————————————————————————————————
		/// <summary>
		/// dealerCommitmentToBuyback
		/// </summary>
		public String dealerCommitmentToBuyback { get; set; }
		
		/// <summary>
		/// deposit
		/// </summary>
		public double deposit { get; set; }
		
		/// <summary>
		/// guarantorAmountGuaranteed
		/// </summary>
		public double guarantorAmountGuaranteed { get; set; }
		
		/// <summary>
		/// guarantorEndDateGuarantee
		/// </summary>
		public DateTime guarantorEndDateGuarantee { get; set; }
		
		//——————————————————————————————————————————————————
		// Contract related Flags
		//——————————————————————————————————————————————————
		/// <summary>
		/// shortfall
		/// </summary>
		public double shortfall { get; set; }
		
		/// <summary>
		/// migrated
		/// </summary>
		public bool migrated { get; set; }
		
		/// <summary>
		/// specialCaseTermination
		/// </summary>
		public bool specialCaseTermination { get; set; }
		
		/// <summary>
		/// ignoreCustomerInsolvent
		/// </summary>
		public bool ignoreCustomerInsolvent { get; set; }




	}
}