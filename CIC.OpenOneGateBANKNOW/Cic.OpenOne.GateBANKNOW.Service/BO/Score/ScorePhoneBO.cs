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
	public class ScorePhoneBO
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
		/// <param name="personID">customer data</param>
		/// <param name="phoneType">phone data</param>
		/// <returns></returns>
		public ScorePhoneDto getPhone (long personID, String phoneType)
        {
			ScorePhoneDto phone = null;

			// NOT YET: using (DdOlExtended ctx = new DdOlExtended ())
			{
				// GET the connection
				// NOT YET: EntityConnection EntityConnection = ctx.Connection as EntityConnection;

				// NOT YET:
				//try
				//{// Get the instance information
				//	person = ctx.ExecuteStoreQuery<ScoreCustomer> (QUERY_ANY1, personID).FirstOrDefault ();
				//}
				//catch
				//{// person information is NOT available
				//	person = null;
				//}

				phone = new ScorePhoneDto ();
			}

			// RETURN constant DUMMY data
			if (phone != null)
			{
				phone.phoneNumber = "+43 210 987";
				phone.phoneType = "mobil";
				phone.validFrom = new DateTime (2001, 2, 3);
				phone.validTo = new DateTime (2011, 2, 3);

			}
			return phone;

		}
	}
}