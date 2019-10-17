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
	public class ScoreDunningLevelBO
	{
		/// <summary>
        /// SQL
        /// </summary>
        const String QUERY_ANY1 = "SELECT * FROM ANY";
        const String QUERY_ANY2 = "SELECT * FROM ANY";
        const String QUERY_ANY3 = "SELECT * FROM ANY";


		/// <summary>
		/// DunningLevel-information auslesen
		/// </summary>
		/// <param name="contractReference">contract data</param>
		/// <param name="customerReference">customer data</param>
		/// <returns></returns>
		public ScoreDunningLevelDto getDunningLevel (long contractReference, long customerReference)
        {
			// ScoreCustomer person = null;
			ScoreDunningLevelDto dunningLevel = null;

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

				dunningLevel = new ScoreDunningLevelDto ();
			}

			// RETURN constant DUMMY data
			if (dunningLevel != null)
			{
				dunningLevel.ContractReference = "7654321";
				dunningLevel.InvoiceReference = "Invoice 234";
				dunningLevel.ContractStatusCollections = 345;
				dunningLevel.DunnigFee = 123.45;
				dunningLevel.DunningActivityCounter = 234;
				dunningLevel.DunningActivityDate = new DateTime (2001, 2, 3);
				dunningLevel.DunningHoldEndDate = new DateTime (2001, 2, 3);
				dunningLevel.DunningHoldFlag = false;
				dunningLevel.InvoiceReference = "Invoice 789";
			}
			return dunningLevel;

		}
	}
}