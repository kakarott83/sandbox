using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	/// <summary>
	/// ScoreCustomer (laut Def. Score DataFields.xlsx 20170602)
	/// </summary>
	public class ScoreCustomerDto
	{
		/// <summary>
		/// customerReference
		/// </summary>
		public long customerReference { get; set; }

		///// <summary>
		///// ID als string (gewünscht von TallyMan?)
		///// </summary>
		//public String strSysCustomer { get; set; }

		/// <summary>
		/// DealerNumber (NICHT von uns!)
		/// </summary>
		public long dealerNumber { get; set; }

		/// <summary>
		/// Titel
		/// </summary>
		public String title { get; set; }

		/// <summary>
		/// Saluation
		/// </summary>
		public String saluation { get; set; }

		/// <summary>
		/// forename
		/// </summary>
		public String forename { get; set; }

		/// <summary>
		/// MiddleName
		/// TM Standardfeld MiddleName wird für Namenszusatz verwendet
		/// </summary>
		public String middleName { get; set; }

		/// <summary>
		/// Surname
		/// </summary>
		public String surname { get; set; }

		/// <summary>
		/// CompanyName
		/// cpi (31.05.17):  Für Firmennamen werden in OL bis zu drei einzelne Felder verwendet 
		/// und auf Vorname, Namenszusatz und Nachname gemappt. 
		/// Soll auch in TM so beibehalten werden, da dies Standard Contact Model entspricht.
		/// </summary>
		public String companyName { get; set; }

		/// <summary>
		/// Birthday
		/// </summary>
		public DateTime birthday { get; set; }

		/// <summary>
		/// Nationality
		/// </summary>
		public String nationality { get; set; }

		/// <summary>
		/// NameOfBirth
		/// </summary>
		public String nameOfBirth { get; set; }

		/// <summary>
		/// Sex
		/// 0 = kein Geschlecht, 1 = männlich, 2= weiblich (natürliche Person --> Gebdatum gefüllt)
		/// </summary>
		public String sex { get; set; }

		/// <summary>
		/// customerType
		/// </summary>
		public String customerType { get; set; }

		/// <summary>
		/// Since
		/// ältestes Aktivierungsdatum des Kunden
		/// </summary>
		public DateTime since { get; set; }

		/// <summary>
		/// BMWEmployee
		/// </summary>
		public bool bmwEmployee { get; set; }

		/// <summary>
		/// SelfEmployed
		/// person.privatflag=0 und person.gebdatum != leer
		/// </summary>
		public bool selfEmployed { get; set; }

		/// <summary>
		/// ScoreAddress
		/// </summary>
		public ScoreAddressDto address { get; set; }

		/// <summary>
		/// ScoreEmail
		/// </summary>
		public ScoreEmailDto email { get; set; }

		/// <summary>
		/// ScorePhone
		/// </summary>
		public ScorePhoneDto phone { get; set; }
		

		/// <summary>
		/// CompanyFoundingDate
		/// </summary>
		public DateTime companyFoundingDate { get; set; }

		/// <summary>
		/// CompanyLegalForm
		/// </summary>
		public String companyLegalForm { get; set; }

		/// <summary>
		/// CompanyTaxID
		/// </summary>
		public String companyTaxID { get; set; }

		/// <summary>
		/// CompanyContactPos
		/// </summary>
		public String companyContactPos { get; set; }

		/// <summary>
		/// Employer
		/// </summary>
		public String employer { get; set; }

		/// <summary>
		/// CreditLimit
		/// mehrere k-linien mögl. 1.Treffer (höchste ID)
		/// </summary>
		public long creditLimit { get; set; }

		/// <summary>
		/// ClaimedCreditLimit
		/// </summary>
		public long claimedCreditLimit { get; set; }

		/// <summary>
		/// NumberFormerContracts
		/// </summary>
		public long numberFormerContracts { get; set; }

		/// <summary>
		/// RiskMonitoring
		/// </summary>
		public String riskMonitoring { get; set; }

		/// <summary>
		/// Rating
		/// </summary>
		public long rating { get; set; }

		/// <summary>
		/// PaymentTargetInvoice
		/// </summary>
		public DateTime paymentTargetInvoice { get; set; }

		/// <summary>
		/// PaymentTargetRegularInvoice
		/// </summary>
		public DateTime paymentTargetRegularInvoice { get; set; }

		/// <summary>
		/// InvoicesSummary
		/// Flag indicating that for the customer exists a summarized invoicing (Sammelrechnung)
		/// </summary>
		public bool invoicesSummary { get; set; }

		/// <summary>
		/// VIPDealer
		/// </summary>
		public bool vipDealer { get; set; }

		/// <summary>
		/// eMail_HR
		/// </summary>
		public String eMail_HR { get; set; }

		/// <summary>
		/// ScoreInsolvency
		/// </summary>
		public ScoreInsolvencyDto insolvency { get; set; }

		//
		// Insolvency
		//
		/// <summary>
		/// Insolvent
		/// </summary>
		public bool Insolvent { get; set; }

		/// <summary>
		/// InsolventFollowupDate
		/// </summary>
		public DateTime InsolventFollowupDate { get; set; }

		/// <summary>
		/// Deceased
		/// </summary>
		public bool Deceased { get; set; }

		/// <summary>
		/// DeceasedCaseStarted
		/// </summary>
		public DateTime DeceasedCaseStarted { get; set; }

		/// <summary>
		/// DeceasedFollowupDate
		/// </summary>
		public DateTime DeceasedFollowupDate { get; set; }

		/// <summary>
		/// Migrated
		/// </summary>
		public bool Migrated { get; set; }

		/// <summary>
		/// IrregularitiesSuspected
		/// AFM Flag from Open Lease.
		/// In contrast to TM Kernel setup this flag is set in AT in Open Lease.
		/// </summary>
		public bool IrregularitiesSuspected { get; set; }

		/// <summary>
		/// Irregularities
		/// </summary>
		public bool Irregularities { get; set; }

		/// <summary>
		/// AddressUnknown
		/// </summary>
		public bool AddressUnknown { get; set; }

		/// <summary>
		/// SpecialAccount
		/// </summary>
		public bool SpecialAccount { get; set; }

		
	}
}