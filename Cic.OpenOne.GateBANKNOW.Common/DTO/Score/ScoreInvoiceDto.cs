using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// ScoreInvoice (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreInvoiceDto
	{
		
		/// <summary>
		/// ContractReference
		/// VT.Vertrag (ReCheck: RN.SysVT = VT.SysID)
		/// </summary>
		public string ContractReference { get; set; }
		
		/// <summary>
		/// CustomerReference
		/// RN.SysPerson = VT.SysKD
		/// </summary>
		public long CustomerReference { get; set; }
		
		/// <summary>
		/// InvoiceID
		/// RN.SysID
		/// </summary>
		public string InvoiceID { get; set; }
		
		/// <summary>
		/// InvoiceNumber
		/// RN.Rechnung oder RN.Erechnung 
		/// </summary>
		public string InvoiceNumber { get; set; }

		/// <summary>
		/// InvoiceType
		/// 
		/// </summary>
		public long InvoiceType { get; set; }

		/// <summary>
		/// InvoiceOriginalAmount
		/// rn.gbetrag + rn.gsteuer (eventuell Steuern und Mahnbetrag zusätzlich)
		/// </summary>
		public decimal InvoiceOriginalAmount { get; set; }

		/// <summary>
		/// InvoiceOpenAmount
		/// RN:GBetrag + RN:GSteuer + RN:Mahnbetrag + RN:Zinsen +  RN:Gebuehr) - (RN:Teilzahlung + RN:Anzahlung + RN:Skonto + RN:Storno)
		/// </summary>
		public decimal InvoiceOpenAmount { get; set; }

		/// <summary>
		/// LateInterestAmount
		/// RN.Zinsen
		/// </summary>
		public decimal LateInterestAmount { get; set; }

		/// <summary>
		/// invoiceText
		/// RN.Text
		/// </summary>
		public string InvoiceText { get; set; }
		
		/// <summary>
		/// InvoiceNumberAlphabet
		/// RN.Beleg2
		/// </summary>
		public string InvoiceNumberAlphabet { get; set; }
		
		/// <summary>
		/// InvoiceDueDate
		/// rn.valuta oder rn.einzugab
		/// </summary>
		public DateTime? InvoiceDueDate { get; set; }
		
		/// <summary>
		/// InvoicePostingDate
		/// rn.Belegdatum?
		/// </summary>
		public DateTime? InvoicePostingDate { get; set; }
		
		/// <summary>
		/// DDReturnReasonCode
		/// </summary>
		public string DDReturnReasonCode { get; set; }

		/// <summary>
		/// LateInterestUpdateDate
		/// RN.Zinsdatum
		/// </summary>
		public DateTime? LateInterestUpdateDate { get; set; }
	}
}