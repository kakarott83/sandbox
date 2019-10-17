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
	public class ScorePersonBO
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
		/// <returns></returns>
		public ScoreCustomerDto getPerson (long personID)
        {
			// ScoreCustomer person = null;
			ScoreCustomerDto personDUMMY = null;

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

				personDUMMY = new ScoreCustomerDto ();
			}

			// RETURN constant DUMMY data
			if (personDUMMY != null)
			{
				personDUMMY.customerReference = personID;

				personDUMMY.dealerNumber = 123;
				personDUMMY.birthday = new DateTime (2001, 2, 3);
				personDUMMY.since = new DateTime (2001, 2, 3);
				personDUMMY.bmwEmployee = false;

				personDUMMY.selfEmployed = false;
				
				// address
				personDUMMY.address = new Cic.OpenOne.GateBANKNOW.Service.BO.ScoreAddressBO ().getAddress (personID);
				
				//personDUMMY.address = new ScoreAddress ();
				//personDUMMY.address.houseNo = "77a";
				//personDUMMY.address.sysID = 234;


				
				personDUMMY.companyFoundingDate = new DateTime (2001, 2, 3);
				personDUMMY.creditLimit = 123000;
				personDUMMY.claimedCreditLimit = 12300;
				personDUMMY.numberFormerContracts = 0;
				personDUMMY.rating = 1;
				personDUMMY.paymentTargetInvoice = new DateTime (2001, 2, 3);
				personDUMMY.paymentTargetRegularInvoice = new DateTime (2001, 2, 3);
				personDUMMY.invoicesSummary = false;
				personDUMMY.vipDealer = false;
				personDUMMY.Insolvent = false;
				personDUMMY.InsolventFollowupDate = new DateTime (2001, 2, 3);
				personDUMMY.Deceased = false;
				personDUMMY.DeceasedCaseStarted = new DateTime (2001, 2, 3);
				personDUMMY.DeceasedFollowupDate = new DateTime (2001, 2, 3);
				personDUMMY.Migrated = false;
				personDUMMY.IrregularitiesSuspected = false;
				personDUMMY.Irregularities = false;
				personDUMMY.AddressUnknown = false;
				personDUMMY.SpecialAccount = false;

			}
			return personDUMMY;

		}
	}
}