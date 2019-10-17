using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using System.Data.EntityClient;
using Devart.Data.Oracle;

namespace Cic.OpenOne.GateBANKNOW.Service.BO
{
	public class ScoreInvoiceBO
	{
		/// <summary>
		/// SQL
		/// </summary>
		const String QUERY_ANY1 = "SELECT * FROM ANY";
		const String QUERY_ANY2 = "SELECT * FROM ANY";
		const String QUERY_ANY3 = "SELECT * FROM ANY";

		/// <summary>
		/// falls contractReference wirklich ein String ist:
		/// Umwandlung
		/// </summary>
		/// <param name="contractReference"></param>
		/// <returns></returns>
		public long getContractID (String contractReference)
		{
			long id = 0;
			bool bOK = long.TryParse (contractReference, out id);		// ID: from string to long
			return id;
		}

		/// <summary>
		/// invoice-information auslesen
		/// </summary>
		/// <param name="customerReference">customer reference</param>
		/// <param name="contractReference">contract reference</param>
		/// <returns></returns>
		public ScoreInvoiceDto getInvoice (long customerReference, String contractReference)
		{
			// ScoreCustomer person = null;
			ScoreInvoiceDto invoice = null;

			// NOT YET: using (DdOlExtended ctx = new DdOlExtended ())
			{
				// GET the connection
				// NOT YET: EntityConnection EntityConnection = ctx.Connection as EntityConnection;

				// NOT YET:
				//try
				//{// Get the instance information
				//	person = ctx.ExecuteStoreQuery<ScoreInvoice> (QUERY_ANY1, personID).FirstOrDefault ();
				//}
				//catch
				//{// person information is NOT available
				//	person = null;
				//}

				invoice = new ScoreInvoiceDto ();
			}

			// RETURN constant DUMMY data
			if (invoice != null)
			{
				invoice.contractReference = getContractID (contractReference);
				invoice.customerReference = customerReference;

				invoice.invoiceText = "InvoiceTextXY";
				invoice.mandatorIDInvoice = "ABC";
				invoice.number = "ABC";
				invoice.amount = 4321.09;
				invoice.numberAlphabet = "ABC";
				invoice.dueDate = new DateTime (2001, 2, 3);
				invoice.postingDate = new DateTime (2001, 2, 3);
				invoice.ddReturnReasonCode = "XYZ";
				invoice.dunningLevelInvoice = 123;
				invoice.actualityDate = new DateTime (2001, 2, 3);


			}
			return invoice;

		}
	}
}