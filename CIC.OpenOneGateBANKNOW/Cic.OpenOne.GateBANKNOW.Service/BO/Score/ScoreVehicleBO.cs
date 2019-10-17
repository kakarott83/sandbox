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
	public class ScoreVehicleBO
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
		public long getVehicleID (String contractReference)
		{
			long id = 0;
			bool bOK = long.TryParse (contractReference, out id);		// ID: from string to long
			return id;
		}

		/// <summary>
		/// vehicle-information auslesen
		/// </summary>
		/// <param name="contractReference">reference</param>
		/// <returns></returns>
		public ScoreVehicleDto getVehicle (String contractReference)
        {
			ScoreVehicleDto vehicle = null;

			// NOT YET: using (DdOlExtended ctx = new DdOlExtended ())
			{
				// GET the connection
				// NOT YET: EntityConnection EntityConnection = ctx.Connection as EntityConnection;

				// NOT YET:
				//try
				//{// Get the instance information
				//	person = ctx.ExecuteStoreQuery<ScoreVehicle> (QUERY_ANY1, personID).FirstOrDefault ();
				//}
				//catch
				//{// person information is NOT available
				//	person = null;
				//}

				vehicle = new ScoreVehicleDto ();
			}

			// RETURN constant DUMMY data
			if (vehicle != null)
			{
				// HIER ist eine Umsetzung "String => long" nötig 
				// (ReCheck rh: vllt kann diese Umsetzung später mal entfernt werden, wenn alle Referenzen als long kommen würden!?!)
				vehicle.contractReference = getVehicleID (contractReference);
				vehicle.CarBrand = "BMW";
				vehicle.CarModel = "850 CSi";
				vehicle.CurrentCarValue = 98765.43;
				vehicle.CarVIN = "VF12ABCD123456789";
				vehicle.Car1stRegistrationDate = new DateTime (2001, 2, 3);
				vehicle.CarLicencePlate = "S-123";
				vehicle.CarColor = "Rot Metallic";

			}
			return vehicle;

		}
	}
}