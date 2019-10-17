using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.BO
{
	public class ScoreBO
	{
		public class ScoreContractBO
		{
			/// <summary>
			/// SQL
			/// </summary>
			const String QUERY_ANY1 = "SELECT * FROM ANYXY";
			const String QUERY_ANY2 = "SELECT * FROM ANYXY";
			const String QUERY_ANY3 = "SELECT * FROM ANYXY";

			/// <summary>
			///OBJECT-information auslesen
			/// </summary>
			/// <param name="sysID">sysID</param>
			/// <returns></returns>
			public ScoreContractDto getObject (long sysID)
			{
				ScoreContractDto objectDUMMY = null;

				// NOT YET: using (DdOlExtended ctx = new DdOlExtended ())
				{
					// GET the connection
					// NOT YET: EntityConnection EntityConnection = ctx.Connection as EntityConnection;

					// NOT YET:
					//try
					//{// Get the instance information
					//	objectDUMMY = ctx.ExecuteStoreQuery<ScoreCustomer> (QUERY_ANY1, personID).FirstOrDefault ();
					//}
					//catch
					//{// person information is NOT available
					//	objectDUMMY = null;
					//}

					objectDUMMY = new ScoreContractDto ();
				}

				// RETURN constant DUMMY data
				if (objectDUMMY != null)
				{
					objectDUMMY.contractReference = sysID.ToString();
					objectDUMMY.customerReference = 123;
					objectDUMMY.contractStartDate = new DateTime (2001, 2, 3);
				}
				return objectDUMMY;
			}

			// Dummy-dummy :)
			public ScoreInvoiceDto getDummy (long sysID)
			{
				ScoreInvoiceDto objectDUMMY = null;
				return objectDUMMY;
			}


		}
	}
}