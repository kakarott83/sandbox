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
	public class ScoreWriteOffBO
	{
		/// <summary>
        /// SQL
        /// </summary>
        const String QUERY_ANY1 = "SELECT * FROM ANY";
        const String QUERY_ANY2 = "SELECT * FROM ANY";
        const String QUERY_ANY3 = "SELECT * FROM ANY";


		/// <summary>
		/// WriteOff-information auslesen
		/// </summary>
		/// <param name="personID">customer data</param>
		/// <returns></returns>
		public ScoreWriteOffDto getWriteOff (long contractReference, long customerReference)
        {
			// ScoreCustomer person = null;
			ScoreWriteOffDto writeOff = null;

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

				writeOff = new ScoreWriteOffDto ();
			}

			// RETURN constant DUMMY data
			if (writeOff != null)
			{
				writeOff.contractReference = 789;
				writeOff.customerReference = 234;
			}
			return writeOff;
		}
	}
}