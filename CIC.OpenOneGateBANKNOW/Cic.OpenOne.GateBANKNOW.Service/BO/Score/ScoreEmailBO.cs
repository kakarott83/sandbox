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
	public class ScoreEmailBO
	{
		/// <summary>
		/// SQL
		/// </summary>
		const String QUERY_ANY1 = "SELECT * FROM ANY";
		const String QUERY_ANY2 = "SELECT * FROM ANY";
		const String QUERY_ANY3 = "SELECT * FROM ANY";


		/// <summary>
		/// email-information auslesen
		/// </summary>
		/// <param name="personID">customer data</param>
		/// <returns></returns>
		public ScoreEmailDto getEmail (long personID)
		{
			ScoreEmailDto personEmail = null;

			// NOT YET: using (DdOlExtended ctx = new DdOlExtended ())
			{
				// GET the connection
				// NOT YET: EntityConnection EntityConnection = ctx.Connection as EntityConnection;

				// NOT YET:
				//try
				//{// Get the instance information
				//	person = ctx.ExecuteStoreQuery<ScoreEmail> (QUERY_ANY1, personID).FirstOrDefault ();
				//}
				//catch
				//{// person information is NOT available
				//	person = null;
				//}

				personEmail = new ScoreEmailDto ();
			}

			// RETURN constant DUMMY data
			if (personEmail != null)
			{
				personEmail.emailAddress = "any@XY.com";

			}
			return personEmail;

		}
	}
}