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
	public class ScoreContractBO
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
		/// contract-information auslesen
		/// liest auch Vehicle Daten
		/// </summary>
		/// <param name="customerReference">customer reference</param>
		/// <param name="contractReference">contract reference</param>
		/// <returns></returns>
		public ScoreContractDto getContract (long customerReference, long contractReference)
		{
			ScoreContractDto contract = null;

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

				contract = new ScoreContractDto ();
			}

			// RETURN constant DUMMY data
			if (contract != null)
			{
				contract.customerReference = customerReference;

				contract.customerRelationshipType = "321";
				contract.contractReference = "432";

				contract.addressDispatchReference = "543";

				// GET Vehicle data
				contract.vehicle = new Cic.OpenOne.GateBANKNOW.Service.BO.ScoreVehicleBO ().getVehicle (contract.contractReference);

				// GET invoice data
				contract.invoice = new Cic.OpenOne.GateBANKNOW.Service.BO.ScoreInvoiceBO ().getInvoice (contract.customerReference, contract.contractReference);

				contract.lateInterest = 321.09;


			}
			return contract;
		}
	}
}