using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// ScoreCustomer (laut Def. Score DataFields.xlsx 20170602)
	/// </summary>
	public class ScoreCustomerDto
	{
		/// <summary>
		/// Kunden ID (as String!)
		/// </summary>
		// public String CustomerID { get; set; }
		[NotMapped]
		public String CustomerID
		{
			get { return CustomerIDAsInt.ToString (); }
			set 
			{
				int iTester = 0;
				if (int.TryParse (value, out iTester))
				{
					CustomerIDAsInt = iTester;
				}
				else
				{
					CustomerIDAsInt = 0;
				}
			}
		}

		// [Column ("MyColumn")]
		// [System.ComponentModel.EditorBrowsable (System.ComponentModel.EditorBrowsableState.Never)]
		private int CustomerIDAsInt { get; set; }

		///// <summary>
		///// CustomerReference als String (gewünscht von TallyMan?)
		///// </summary>
		//public String CustomerReference { get; set; }

		// V 2.9 NOT relevant
		///// <summary>
		///// DealerNumber (NICHT von uns!)
		///// </summary>
		//public long DealerNumber { get; set; }

		/// <summary>
		/// Titel  (enum)
		/// </summary>
		public String Title { get; set; }

		/// <summary>
		/// Salutation
		/// </summary>
		public String Salutation { get; set; }

		/// <summary>
		/// forename
		/// </summary>
		public String Forename { get; set; }

		/// <summary>
		/// MiddleName
		/// TM Standardfeld MiddleName wird für Namenszusatz verwendet
		/// </summary>
		public String MiddleName { get; set; }

		/// <summary>
		/// Surname
		/// </summary>
		public String Surname { get; set; }

		///// <summary>
		///// CompanyName
		///// cpi (31.05.17):  Für Firmennamen werden in OL bis zu drei einzelne Felder verwendet 
		///// und auf Vorname, Namenszusatz und Nachname gemappt. 
		///// Soll auch in TM so beibehalten werden, da dies Standard Contact Model entspricht.
		///// </summary>
		public String CompanyName { get; set; }

		/// <summary>
		/// Birthday
		/// </summary>
		public DateTime? Birthday { get; set; }

		/// <summary>
		/// Nationality  (enum)
		/// </summary>
		public String Nationality { get; set; }

		// V 2.9 NOT relevant
		///// <summary>
		///// customerType
		///// </summary>
		//public String CustomerType { get; set; }

		/// <summary>
		/// Since
		/// ältestes Aktivierungsdatum des Kunden
		/// </summary>
		public DateTime? Since { get; set; }

		/// <summary>
		/// BMWEmployee  (enum)
		/// </summary>
		[NotMapped]
		public bool BMWEmployee
		{
			get { return BMWEmployeeAsInt == 1; }
			set { BMWEmployeeAsInt = value ? 1 : 0; }
		}
		private int? BMWEmployeeAsInt { get; set; }

		/// <summary>
		/// SelfEmployed
		/// person.privatflag=0 und person.gebdatum !=leer
		/// </summary>
		[NotMapped]
		public bool SelfEmployed
		{
			get { return SelfEmployedAsInt == 1; }
			set { SelfEmployedAsInt = value ? 1 : 0; }
		}
		private int? SelfEmployedAsInt { get; set; }

		/// <summary>
		/// ScoreAddress
		/// </summary>
		public ScoreAddressDto Address { get; set; }
		
		/// <summary>
		/// emailAddress
		/// </summary>
		public String EmailAddress { get; set; }

		/// <summary>
		/// ScorePhone
		/// </summary>
		public ScorePhoneDto[] Phone { get; set; }
		

		/// <summary>
		/// CompanyFoundingDate
		/// </summary>
		public DateTime? CompanyFoundingDate { get; set; }

		/// <summary>
		/// CompanyLegalForm (enum)
		/// </summary>
		public String CompanyLegalForm { get; set; }

		/// <summary>
		/// CompanyTaxID
		/// </summary>
		public String CompanyTaxID { get; set; }

		///// <summary>
		///// CompanyContactPos
		///// </summary>
		//public String CompanyContactPos { get; set; }

		/// <summary>
		/// Employer
		/// </summary>
		public String Employer { get; set; }

		/// <summary>
		/// CreditLimit
		/// mehrere k-linien mögl. 1.Treffer (höchste ID)
		/// </summary>
		public long CreditLimit { get; set; }

		/// <summary>
		/// ClaimedCreditLimit
		/// </summary>
		public long ClaimedCreditLimit { get; set; }

		/// <summary>
		/// NumberFormerContracts
		/// </summary>
		public long NumberFormerContracts { get; set; }

		///// <summary>
		///// RiskMonitoring
		///// </summary>
		//public String RiskMonitoring { get; set; }

		/// <summary>
		/// Rating
		/// </summary>
		public long Rating { get; set; }

		// V 2.9 NOT relevant --> moved to contract
		///// <summary>
		///// PaymentTargetInvoice
		///// </summary>
		//// public DateTime? PaymentTargetInvoice { get; set; }
		//public int? PaymentTargetInvoice { get; set; }

		// V 2.9 NOT relevant --> moved to contract
		///// <summary>
		///// PaymentTargetRegularInvoice
		///// </summary>
		//// public DateTime? PaymentTargetRegularInvoice { get; set; }
		//public String PaymentTargetRegularInvoice { get; set; }

		/// <summary>
		/// InvoicesSummary
		/// Flag indicating that for the customer exists a summarized invoicing (Sammelrechnung)
		/// </summary>
		[NotMapped]
		public bool InvoicesSummary
		{
			get { return InvoicesSummaryAsInt == 1; }
			set { InvoicesSummaryAsInt = value ? 1 : 0; }
		}
		private int? InvoicesSummaryAsInt { get; set; }

		/// <summary>
		/// InsolvencyStartDate
		/// DB: ausfall.beginnam 
		/// 
		/// AUSFALL und WFMMEMO:
		/// ausfall.SYSID => VT.sysid daraus SYSAUSFALLTYP ermitteln
		///	Logik: 
		///		wfmmemo.notiz
		///		wfmmemo.syslease => SYSAUSFALL
		/// 
		/// </summary>
		public DateTime? InsolvencyStartDate { get; set; }
        

		/// <summary>
		/// Insolvent (rh: 20170811)
		/// Wenn SYSAUSFALLTYP ID = 4 vorhanden -> 1, sonst 0
		/// </summary>
		[NotMapped]
		public bool Insolvent
		{
			get { return InsolventAsInt == 1; }
			set { InsolventAsInt = value ? 1 : 0; }
		}
		private int InsolventAsInt { get; set; }

		/// <summary>
		/// InsolventFollowupDate
		/// wfmmemo.dat01
		/// Hier müssen  weitere Parameter des Memoeintrags abgefragt werden
		/// </summary>
		public DateTime? InsolventFollowupDate { get; set; }

		/// <summary>
		/// Deceased
		/// AUSFALL: ausfall.SYSID => VT.sysid daraus SYSAUSFALLTYP ermitteln
		///	Logik: 
		///		wfmmemo.syslease => SYSAUSFALL
		///		wfmmemo.sysfttable == 220;
		/// 
		/// </summary>
		[NotMapped]
		public bool Deceased
		{
			get { return DeceasedAsInt == 1; }
			set { DeceasedAsInt = value ? 1 : 0; }
		}
		private int? DeceasedAsInt { get; set; }

		/// <summary>
		/// DeceasedCaseStarted
		/// </summary>
		public DateTime? DeceasedCaseStarted { get; set; }

		/// <summary>
		/// DeceasedFollowupDate
		/// Hier müssen  weitere Parameter des Memoeintrags abgefragt werden
		/// </summary>
		public DateTime? DeceasedFollowupDate { get; set; }

		///// <summary>
		///// Migrated
		///// HINWEIS: das Feld  wird im Rahmen der Umsetzung definiert
		///// </summary>
		//[NotMapped]
		//public bool Migrated
		//{
		//	get { return MigratedAsInt == 1; }
		//	set { MigratedAsInt = value ? 1 : 0; }
		//}
		//private int? MigratedAsInt { get; set; }

		/// <summary>
		/// IrregularitiesSuspected
		/// in DB = person.flaglu19
		/// AFM Flag from Open Lease.
		/// In contrast to TM Kernel setup this flag is set in AT in Open Lease.
		/// </summary>
		[NotMapped]
		public bool IrregularitiesSuspected
		{
			get { return IrregularitiesSuspectedAsInt == 1; }
			set { IrregularitiesSuspectedAsInt = value ? 1 : 0; }
		}
		private int? IrregularitiesSuspectedAsInt { get; set; }

		/// <summary>
		/// SpecialAccount 
		/// in DB person.MAHNGRUPPE  [VARCHAR2(20)]
		/// </summary>
		public String SpecialAccount { get; set; }
		
	}
}