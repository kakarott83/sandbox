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
	public class ScoreArrangementBO
	{
		/// <summary>
        /// SQL
        /// </summary>
        const String QUERY_ANY1 = "SELECT * FROM ANY";
        const String QUERY_ANY2 = "SELECT * FROM ANY";
        const String QUERY_ANY3 = "SELECT * FROM ANY";


		/// <summary>
		/// Personen-/Customer-information auslesen
		/// </summary>
		/// <param name="contractReference">contract data</param>
		/// <param name="customerReference">customer data</param>
		/// <returns></returns>
		public ScoreArrangementDto getArrangement (long contractReference, long customerReference)
        {
			// ScoreCustomer person = null;
			ScoreArrangementDto arrangement = null;

			// NOT YET: using (DdOlExtended ctx = new DdOlExtended ())
			{
				// GET the connection
				// NOT YET: EntityConnection EntityConnection = ctx.Connection as EntityConnection;

				// NOT YET:
				//try
				//{// Get the instance information
				//	person = ctx.ExecuteStoreQuery<ScoreAddress> (QUERY_ANY1, personID).FirstOrDefault ();
				//}
				//catch
				//{// person information is NOT available
				//	person = null;
				//}

				arrangement = new ScoreArrangementDto ();
			}

			// RETURN constant DUMMY data
			if (arrangement != null)
			{
				arrangement.ArrangementCancellationFlag = false;
				arrangement.ArrangementCancellationReason = "theArrangementCancellationReason!";
				arrangement.ArrangementEndDate = new DateTime (2001, 2, 3);
				arrangement.ArrangementStartDate = new DateTime (2001, 2, 3);
				arrangement.ArrangementStatusFlag = false;
				arrangement.PtPCancellationFlag = false;
				arrangement.PtPCancellationReason = "thePtPCancellationReason!";
				arrangement.PtPEndDate = new DateTime (2001, 2, 3);
				arrangement.PtPStartDate = new DateTime (2001, 2, 3);
				arrangement.PtPStatusFlag = false;


			}
			return arrangement;

		}
	}
}