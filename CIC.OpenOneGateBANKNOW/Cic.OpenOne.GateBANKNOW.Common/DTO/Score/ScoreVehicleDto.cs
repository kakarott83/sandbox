using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// ScoreVehicle (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreVehicleDto
	{
		/// <summary>
		/// ContractReference
		/// </summary>
		public long contractReference { get; set; }
		
		/// <summary>
		/// CarBrand
		/// OL::OB:HERSTELLER
		/// TM::Vehicle.(VehicleModelVariant).definition.(VehicleModel).series.(VehicleMake).make.code
		/// </summary>
		public string CarBrand { get; set; }
		
		/// <summary>
		/// CarModel
		/// OL::OB:Fabrikat
		/// TM::Vehicle.(VehicleModelVariant).definition.(VehicleModel).name
		/// </summary>
		public string CarModel { get; set; }
		
		/// <summary>
		/// CarVIN
		/// </summary>
		public string CarVIN { get; set; }
		
		/// <summary>
		/// Car1stRegistrationDate
		/// OL::OBINI:Erstzul
		/// TM::Vehicle.dateOfFirstRegistration
		/// </summary>
		public DateTime? Car1stRegistrationDate { get; set; }
		
		/// <summary>
		/// CarLicencePlate
		/// OL::OB:Kennzeichen
		/// TM::Vehicle.LicencePlate.licence
		/// </summary>
		public string CarLicencePlate { get; set; }
		
		/// <summary>
		/// CurrentCarValue
		/// TM::Vehicle.EstimatedCurrentTradeValue.estimatedValue
		/// </summary>
		public double CurrentCarValue { get; set; }
		
		/// <summary>
		/// CarColor
		/// </summary>
		public string CarColor { get; set; }

	}
}