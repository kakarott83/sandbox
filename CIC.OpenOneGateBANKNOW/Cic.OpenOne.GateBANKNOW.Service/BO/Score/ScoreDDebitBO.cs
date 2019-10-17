using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.BO
{
	public class ScoreDDebitBO
	{
		/// <summary>
		/// SQL
		/// </summary>
		const String QUERY_ANY1 = "SELECT * FROM ANY";
		const String QUERY_ANY2 = "SELECT * FROM ANY";
		const String QUERY_ANY3 = "SELECT * FROM ANY";


		/// <summary>
		/// get Next 2ndDDebit-Reference
		/// </summary>
		/// <param name="contractID">contract data</param>
		/// <returns></returns>
		public long getNextDDebitReference (long contractID)
		{
			// RETURN DUMMY data (id + 1)
			return contractID + 1;
		}


		/// <summary>
		/// DDebit-information auslesen
		/// </summary>
		/// <param name="contractID">contract data</param>
		/// <returns></returns>
		public ScoreDDebitDto getDDebit (long contractID)
		{
			// ScoreCustomer person = null;
			ScoreDDebitDto ddebit = null;

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

				ddebit = new ScoreDDebitDto ();
			}

			// RETURN constant DUMMY data
			if (ddebit != null)
			{
				ddebit.contractReference = contractID;
				ddebit.DirectDebitID = 432;
				ddebit.amount = 4321.09;
				ddebit.currency = "€";
				ddebit.ddType = "XYZ";
				ddebit.ddInvoiceReference = "Invoive-XYZ";
				ddebit.ddDate = new DateTime (2001, 2, 3);
			}
			return ddebit;
		}
	}
}