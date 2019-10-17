using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// ScoreContract (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreContractDto
	{
		/// <summary>
		/// customerReference
		/// vt.syskd
		/// </summary>
		[Required]
		public long CustomerReference { get; set; }
		
		/// <summary>
		/// customerRelationshipType
		/// 
		/// 'KD' v 'HD' v 'MH'
		/// 
		/// VT:SYSKD, VT:SYSVPFIL, 
		/// select vto.sysmh from person p, vt, vtobsich vto 
		/// where vt.sysid = vto.sysvt  and vto.SysMH = p.sysperson   and   vto.sysmh = :p1 
		/// and vto.rang >= 100 and vto.rang lower 110
		/// 
		/// Ist noch zu klären, wie die Personen nach TM kommen - Darstellung 1:N Beziehung
		/// 
		/// </summary>
		public String CustomerRelationshipType { get; set; }

		/// <summary>
		/// contractReference
		/// </summary>
		public String ContractReference { get; set; }
		
		/// <summary>
		/// ContractNumber (vllt NICHT NÖTIG)
		/// </summary>
		public String ContractNumber { get; set; }

		/// <summary>
		/// MandatorID
		/// </summary>
		public String MandatorID { get; set; }
		
		/// <summary>
		/// contractType
		/// </summary>
		public String ContractType { get; set; }
		
		/// <summary>
		/// CMSStatus
		/// </summary>
		public String CMSStatus { get; set; }
		
		/// <summary>
		/// DDPossible
		/// </summary>
		[NotMapped]
		public bool DDPossible
		{
			get { return DDPossibleAsInt == 1; }
			set { DDPossibleAsInt = value ? 1 : 0; }
		}
		private int? DDPossibleAsInt { get; set; }
		
		/// <summary>
		/// businessLine
		/// </summary>
		public string BusinessLine { get; set; }
		
		/// <summary>
		/// privateBusinessIndicator
		/// </summary>
		public string PrivateBusinessIndicator { get; set; }
		
		/// <summary>
		/// contractApplicationDate
		/// </summary>
		public DateTime? ContractApplicationDate { get; set; }
		
		/// <summary>
		/// contractStartDate
		/// </summary>
		public DateTime? ContractStartDate { get; set; }
		
		/// <summary>
		/// contractEndDate
		/// </summary>
		public DateTime? ContractEndDate { get; set; }
		
		/// <summary>
		/// productType
		/// </summary>
		public string ProductType { get; set; }
		
		/// <summary>
		/// insurance
		/// </summary>
		public string Insurance { get; set; }

		/// <summary>
		/// LoanContractType
		/// </summary>
		// [DisplayFormat(ConvertEmptyStringToNull = false, NullDisplayText = "[sirNull]")]
		public string LoanContractType { get; set; }
		
		/// <summary>
		/// contractOriginalValue
		/// </summary>
		public double ContractOriginalValue { get; set; }
		
		/// <summary>
		/// contractTerm
		/// </summary>
		public long ContractTerm { get; set; }
		
		/// <summary>
		/// currency
		/// </summary>
		public string Currency { get; set; }
		
		/// <summary>
		/// regularInstalmentAmountGross
		/// </summary>
		public double RegularInstalmentAmountGross { get; set; }
		
		/// <summary>
		/// balloonRateAmount
		/// </summary>
		public double BalloonRateAmount { get; set; }

		/// <summary>
		/// balloonRateDate
		/// </summary>
		public DateTime? BalloonRateDate { get; set; }

		/// <summary>
		/// finalInvoiceDate
		/// </summary>
		public DateTime? FinalInvoiceDate { get; set; }
		
		/// <summary>
		/// finalInvoiceAmount
		/// 
		/// vtrpos.wert = NettoBetrag 
		/// VTR muss Status BEENDET haben 
		/// und VTRPOS hat den Rang 9999 
		/// VT-Vertrag: 1004944
		/// 
		/// select * from vtr 
		/// inner join vtrpos on vtrpos.sysvtr = vtr.sysvtr 
		/// where zustand like 'BEENDET' and vtr.sysvt = 242795 and vtrpos.rang = 9999 order by vtr.sysvtr desc;
		/// 
		/// Im Feld vtr.Str01 steht die Rechnungsnummer (=RN.Rechnung) 
		/// select * from rn where rn.rechnung like '1402525290' 
		/// EA-Saldo netto 374.27      MWST 80.33
		/// </summary>
		public double FinalInvoiceAmount { get; set; }
		
		/// <summary>
		/// contractMileage
		/// </summary>
		public string ContractMileage { get; set; }
		
		/// <summary>
		/// iban
		/// </summary>
		public string IBAN { get; set; }
		
		///// <summary>
		///// bic
		///// </summary>
		//public string BIC { get; set; }
		
		///// <summary>
		///// houseBankAccountHolder
		///// </summary>
		//public string HouseBankAccountHolder { get; set; }
		
		///// <summary>
		///// houseBankName
		///// </summary>
		//public string HouseBankName { get; set; }
		
		/// <summary>
		/// prefComChannel 
		/// peoption.str01
		/// </summary>
		public string PrefComChannel { get; set; }

		//——————————————————————————————————————————————————
		// Contract Dunning Data 
		//——————————————————————————————————————————————————
		
		/// <summary>
		/// arrearsBalance
		/// </summary>
		public double ArrearsBalance { get; set; }
		
		/// <summary>
		/// remainingBalance
		/// </summary>
		public double RemainingBalance { get; set; }
		
		///// <summary>
		///// dueDateOldestInvoice
		///// </summary>
		//public DateTime? DueDateOldestInvoice { get; set; }
		
		///// <summary>
		///// interest
		///// </summary>
		//public double Interest { get; set; }

		///// <summary>
		///// dunningActivityDate
		///// </summary>
		//public DateTime? DunningActivityDate { get; set; }
		
		/// <summary>
		/// applicationRating
		/// </summary>
		public string ApplicationRating { get; set; }
		
		///// <summary>
		///// instalmentsInArrears
		///// </summary>
		//public long InstalmentsInArrears { get; set; }
		
		///// <summary>
		///// numberFormerContracts
		///// </summary>
		//public long NumberFormerContracts { get; set; }
		
		///// <summary>
		///// highestDPD
		///// </summary>
		//public long HighestDPD { get; set; }
		
		///// <summary>
		///// dateLastMissedPayment
		///// </summary>
		//public DateTime? DateLastMissedPayment { get; set; }
		
		///// <summary>
		///// percentageOfContractPaid
		///// </summary>
		//public double PercentageOfContractPaid { get; set; }
		
		/// <summary>
		/// lateInterest
		/// </summary>
		public double LateInterest { get; set; }
		
		///// <summary>
		///// feesCharges
		///// </summary>
		//public double FeesCharges { get; set; }
		
		///// <summary>
		///// currentAccountBalance --> nach "SettlementAmountForTermination" VERSCHOBEN
		///// </summary>
		//public double CurrentAccountBalance { get; set; }
		
		/// <summary>
		/// SettlementAmountForTermination --> von "currentAccountBalance" VERSCHOBEN
		/// </summary>
		public double SettlementAmountForTermination { get; set; }
		
		///// <summary>
		///// purchaseFlagCustomer
		///// Wenn obvwrt.syskaeufer = vt.syskd dann = Y
		///// V 2.9: ALS BOOL nach ColCaseMngmt ausgelagert
		///// retailContract.thirdPartyPurchasingOption.isOptionChoosen
		///// </summary>
		//public string PurchaseFlagCustomer { get; set; }
		
		///// <summary>
		///// outboundCampaignName
		///// </summary>
		//public string OutboundCampaignName { get; set; }

		//——————————————————————————————————————————————————
		// Contract Termination and Final Invoice Details
		//——————————————————————————————————————————————————
		
		///// <summary>
		///// terminationDate
		///// </summary>
		//public DateTime? TerminationDate { get; set; }
		
		///// <summary>
		///// requestIDSettlementAmount
		///// (Ablösewertanfrage ID)
		///// </summary>
		//public string RequestIDSettlementAmount { get; set; }
		
		///// <summary>
		///// settlementAmountForTermination
		///// </summary>
		//public double SettlementAmountForTermination { get; set; }
		
		///// <summary>
		///// interestRebate
		///// </summary>
		//public double InterestRebate { get; set; }
		
		/// <summary>
		/// agreedResidualValue
		/// </summary>
		public double AgreedResidualValue { get; set; }
		
		///// <summary>
		///// remainingDebt
		///// </summary>
		//public double RemainingDebt { get; set; }
		
		///// <summary>
		///// remainingContractValue
		///// </summary>
		//public double RemainingContractValue { get; set; }
		
		///// <summary>
		///// repossessionOrderCMS
		///// OL::ausfall.aktive
		///// V 2.7 --> type --> double --> bool
		///// V 2.9: ALS BOOL nach ColCaseMngmt ausgelagert
		///// RetailContract.detailedReturnInfo.order
		///// 
		///// </summary>
		//[NotMapped]
		//public bool RepossessionOrderCMS
		//{
		//	get { return RepossessionOrderCMSAsInt == 1; }
		//	set { RepossessionOrderCMSAsInt = value ? 1 : 0; }
		//}
		//private int? RepossessionOrderCMSAsInt { get; set; }



		///// <summary>
		///// repossessionFollowupDate
		///// OL::wfmmemo.dat01
		///// V 2.9: nach ColCaseMngmt ausgelagert
		///// </summary>
		//public DateTime? RepossessionFollowupDate { get; set; }

		//public ScoreVehicleDto Vehicle { get; set; }

		//public ScoreInvoiceDto Invoice { get; set; }
		

		///// <summary>
		///// Flag, indicates whether vehicle has been repossessed
		///// OL: sicher.sicherstellung
		///// V 2.7 --> type --> DateTime? --> bool
		///// V 2.9: nach ColCaseMngmt ausgelagert
		///// </summary>
		//[NotMapped]
		//public bool VehicleRepossessed
		//{
		//	get { return VehicleRepossessedAsInt == 1; }
		//	set { VehicleRepossessedAsInt = value ? 1 : 0; }
		//}
		//private int? VehicleRepossessedAsInt { get; set; }
		
		///// <summary>
		///// dateRepossessionOrder
		///// </summary>
		//public DateTime? DateRepossessionOrder { get; set; }

		///// <summary>
		///// vehicleExpertiseOrder
		///// V 2.9: nach ColCaseMngmt ausgelagert
		///// RetailContract.financedAsset.vehicleAppraisal.status
		///// </summary>
		//public string VehicleExpertiseOrder { get; set; }
		
		///// <summary>
		///// vehicleExpertiseFollowupDate
		///// V 2.9: nach ColCaseMngmt ausgelagert
		///// RetailContract.financedAsset.vehicleAppraisal.followUpDate
		///// </summary>
		//public DateTime? VehicleExpertiseFollowupDate { get; set; }
		
		///// <summary>
		///// dateVehicleExpertiseCompleted
		///// V 2.9: nach ColCaseMngmt ausgelagert
		///// RetailContract.financedAsset.vehicleAppraisal.document.creationDate
		///// </summary>
		//public DateTime? DateVehicleExpertiseCompleted { get; set; }
		
		///// <summary>
		///// estimatedSalesPriceVehicle
		///// V 2.9: nach ColCaseMngmt ausgelagert
		///// RetailContract.collectionCase.retailRecovery.estimatedRecoveryValue.amount
		///// </summary>
		//public decimal EstimatedSalesPriceVehicle { get; set; }
		
		///// <summary>
		///// salesPriceVehicle
		///// </summary>
		//public double SalesPriceVehicle { get; set; }
		
		///// <summary>
		///// dateOfSale
		///// obvwrt und obvwrtan (PROJEKT EA_NEU Excel mit Felddefinitionen)
		///// V 2.9: nach ColCaseMngmt ausgelagert
		///// Vehicle.usedVehicleSalesPricing.salesPrice.dateOfSale
		///// </summary>
		//public DateTime? DateOfSale { get; set; }
		
		/// <summary>
		/// finalInvoiceReason
		/// </summary>
		public string FinalInvoiceReason { get; set; }
		
		 
		//——————————————————————————————————————————————————
		// Collateral Details
		//——————————————————————————————————————————————————
		/// <summary>
		/// dealerCommitmentToBuyback
		/// </summary>
		public string DealerCommitmentToBuyback { get; set; }
		
		 
		/// <summary>
		/// migrated
		/// </summary>
		[NotMapped]
		public bool Migrated
		{
			get { return MigratedAsInt == 1; }
			set { MigratedAsInt = value ? 1 : 0; }
		}
		private int? MigratedAsInt { get; set; }
		
		 
		/// <summary>
		/// noteType 
		/// </summary>
		public string NoteType { get; set; }

		/// <summary>
		/// noteText 
		/// </summary>
		public string NoteText { get; set; }


		/// <summary>
		/// DealerNumber 
		/// V 2.9: neu aus Partner
		/// Person.Code (VT.SYSVPFIL AS DealerNumber)
		/// </summary>
		public long DealerNumber { get; set; }
		
		/// <summary>
		/// PaymentTargetInvoice 
		/// V 2.9: neu aus Partner
		/// person.zahlmodus
		/// /// </summary>
		public decimal PaymentTargetInvoice { get; set; }
		
		/// <summary>
		/// PaymentTargetRegularInvoice 
		/// V 2.9: neu aus Partner
		/// person.vornamegen
		/// </summary>
		public string PaymentTargetRegularInvoice { get; set; }
		

	}
}