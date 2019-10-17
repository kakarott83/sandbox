using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	/// <summary>
	/// ScoreInvoice (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreInvoiceDto
	{
		
		/// <summary>
		/// contractReference
		/// RN.SysVT = VT.SysID
		/// </summary>
		public long contractReference { get; set; }
		
		/// <summary>
		/// customerReference
		/// RN.SysPerson = VT.SysKD
		/// </summary>
		public long customerReference { get; set; }
		
		/// <summary>
		/// mandatorIDInvoice
		/// RN.SysLS
		/// </summary>
		public String mandatorIDInvoice { get; set; }
		
		/// <summary>
		/// number
		/// RN.Rechnung oder RN.Erechnung
		/// </summary>
		public String number { get; set; }
		
		/// <summary>
		/// amount
		/// rn.gbetrag+rn.gsteuer (eventuell Steuern und Mahnbetrag zusätzlich)
		/// </summary>
		public double amount { get; set; }
		
		/// <summary>
		/// invoiceText
		/// RN.Text
		/// </summary>
		public String invoiceText { get; set; }
		
		/// <summary>
		/// numberAlphabet
		/// RN.Beleg2
		/// </summary>
		public String numberAlphabet { get; set; }
		
		/// <summary>
		/// dueDate
		/// rn.valuta oder rn.einzugab
		/// </summary>
		public DateTime dueDate { get; set; }
		
		/// <summary>
		/// postingDate
		/// rn.Belegdatum?
		/// </summary>
		public DateTime postingDate { get; set; }
		
		/// <summary>
		/// ddReturnReasonCode
		/// </summary>
		public String ddReturnReasonCode { get; set; }
		
		/// <summary>
		/// dunningLevelInvoice
		/// </summary>
		public long dunningLevelInvoice { get; set; }
		
		/// <summary>
		/// actualityDate
		/// </summary>
		public DateTime actualityDate { get; set; }
	}
}