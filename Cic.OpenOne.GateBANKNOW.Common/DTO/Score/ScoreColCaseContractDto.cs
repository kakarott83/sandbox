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
	public class ScoreColCaseContractDto
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
		
		 
		/// <summary>
		/// applicationRating
		/// </summary>
		public int? ApplicationRating { get; set; }
		
		 
		/// <summary>
		/// lateInterest
		/// </summary>
		public double LateInterest { get; set; }
		
	 
		/// <summary>
		/// SettlementAmountForTermination --> von "currentAccountBalance" VERSCHOBEN
		/// </summary>
		public double SettlementAmountForTermination { get; set; }
		 
		/// <summary>
		/// agreedResidualValue
		/// </summary>
		public double AgreedResidualValue { get; set; }
		 
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
		
		///// <summary>
		///// deposit
		///// V 2.9: nach ColCaseMngmt ausgelagert
		///// RetailContract.(Deposit)collateral.amount.value
		///// </summary>
		//public double Deposit { get; set; }
		
		///// <summary>
		///// guarantorAmountGuaranteed
		///// </summary>
		//public double GuarantorAmountGuaranteed { get; set; }
		
		///// <summary>
		///// guarantorEndDateGuarantee
		///// </summary>
		//public DateTime? GuarantorEndDateGuarantee { get; set; }
		
		//——————————————————————————————————————————————————
		// Contract related Flags
		//——————————————————————————————————————————————————
		///// <summary>
		///// shortfall
		///// </summary>
		//public double Shortfall { get; set; }

		/// <summary>
		/// FuturePayArrFlag
		/// </summary>
		public string FuturePayArrFlag { get; set; }

		/// <summary>
		/// FuturePayArrStartDate
		/// </summary>
		public DateTime? FuturePayArrStartDate { get; set; }

		/// <summary>
		/// FuturePayArrEndDate
		/// </summary>
		public DateTime? FuturePayArrEndDate { get; set; }

		/// <summary>
		/// DetailFuturePayArr
		/// </summary>
		public string DetailFuturePayArr { get; set; }

		/// <summary>
		/// FuturePtPFlag
		/// </summary>
		public string FuturePtPFlag { get; set; }

		/// <summary>
		/// FuturePtPStartDate
		/// </summary>
		public DateTime? FuturePtPStartDate { get; set; }

		/// <summary>
		/// FuturePtPEndDate
		/// </summary>
		public DateTime? FuturePtPEndDate { get; set; }

		/// <summary>
		/// DetailFuturePtP (wfmmemo.notiz)
		/// </summary>
		public string DetailFuturePtP { get; set; }
		
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
		
		///// <summary>
		///// specialCaseTermination
		///// </summary>
		//[NotMapped]
		//public bool SpecialCaseTermination
		//{
		//	get { return SpecialCaseTerminationAsInt == 1; }
		//	set { SpecialCaseTerminationAsInt = value ? 1 : 0; }
		//}
		//private int? SpecialCaseTerminationAsInt { get; set; }
		
		/////// <summary>
		/////// ignoreCustomerInsolvent
		/////// </summary>
		//[NotMapped]
		//public bool IgnoreCustomerInsolvent
		//{
		//	get { return IgnoreCustomerInsolventAsInt == 1; }
		//	set { IgnoreCustomerInsolventAsInt = value ? 1 : 0; }
		//}
		//private int? IgnoreCustomerInsolventAsInt { get; set; }

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