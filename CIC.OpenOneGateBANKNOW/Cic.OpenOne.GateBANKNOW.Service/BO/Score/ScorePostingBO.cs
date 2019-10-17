using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.BO
{
	public class ScorePostingBO
	{
		/// <summary>
		/// SQL
		/// </summary>
		const String QUERY_ANY1 = "SELECT * FROM ANY";
		const String QUERY_ANY2 = "SELECT * FROM ANY";
		const String QUERY_ANY3 = "SELECT * FROM ANY";

		
		/// <summary>
		/// get Next Posting-Reference
		/// </summary>
		/// <param name="contractID">contract data</param>
		/// <returns></returns>
		public long getNextPosting (long contractID)
		{
			// RETURN DUMMY data (id + 1)
			return contractID + 1;
		}
	
	
		/// <summary>
		/// invoice-information auslesen
		/// </summary>
		/// <param name="contractID">customer data</param>
		/// <returns></returns>
		public ScorePostingDto getPosting (long contractID)
		{
			ScorePostingDto posting = null;

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

				posting = new ScorePostingDto ();
			}

			// RETURN constant DUMMY data
			if (posting != null)
			{
				posting.contractReference = contractID;

				posting.amount = 4321.09;
				posting.currency = "€";
				posting.postingDate = new DateTime (2001, 2, 3);
				posting.postingCode = "XYZ";
			}
			return posting;

		}
	}
}