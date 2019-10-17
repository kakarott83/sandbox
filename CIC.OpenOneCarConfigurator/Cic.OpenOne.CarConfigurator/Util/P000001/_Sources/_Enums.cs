// OWNER: BK, 06-06-2008
namespace Cic.P000001.Common
{
	#region Enums
	// TESTED BY EnumTreeNodeDetailTypeConstantsTestFixture.CheckLength
	// TESTED BY EnumTreeNodeDetailTypeConstantsTestFixture.CheckMinValue
	// TESTED BY EnumTreeNodeDetailTypeConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum TreeNodeDetailTypeConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		Undefined = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Hierarchy = 1,
        [System.Runtime.Serialization.EnumMemberAttribute]
		General = 2,
        [System.Runtime.Serialization.EnumMemberAttribute]
		StandardEquipment = 3,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Technic = 4,
	}

	// TESTED BY EnumTreeNodeDetailValueTypeConstantsTestFixture.CheckLength
	// TESTED BY EnumTreeNodeDetailValueTypeConstantsTestFixture.CheckMinValue
	// TESTED BY EnumTreeNodeDetailValueTypeConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum TreeNodeDetailValueTypeConstants : int
	{
		Undefined = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		// Hierarchiedaten, 1000, Hie	
		HieManufacturer = 1005,
        [System.Runtime.Serialization.EnumMemberAttribute]
		HieMake = 1010,
        [System.Runtime.Serialization.EnumMemberAttribute]
		HieModel = 1020,
        [System.Runtime.Serialization.EnumMemberAttribute]
		HieBodyType = 1030,
        [System.Runtime.Serialization.EnumMemberAttribute]
		HieFuelType = 1040,
        [System.Runtime.Serialization.EnumMemberAttribute]
		HieGearType = 1050,
        [System.Runtime.Serialization.EnumMemberAttribute]
        HieType = 1060,
        [System.Runtime.Serialization.EnumMemberAttribute]
		// Basisdaten, 2000, Bas	[System.Runtime.Serialization.EnumMemberAttribute]
		BasMsrPrice = 2010,
        [System.Runtime.Serialization.EnumMemberAttribute]
		BasBestPrice = 2020,
        
		// Serienausstattung, 3000, Std	
        
		// Technische Daten, 4000, Tec	
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecCountOfSeats = 4010,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecCountOfDoors = 4020,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecCountOfGears = 4030,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecCylinderCapacity = 4040,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecCountOfCylinder = 4050,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecEnginePerformancePS = 4060,
        
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecEnginePerformancekW = 4070,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecMaximumSpeed = 4080,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecAcceleration0To100 = 4090,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecTankCapacity = 4100,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecComsumptionUrban = 4110,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecConsumptionInterurban = 4120,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecConsumptionCombined = 4130,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecCO2EmissionUrban = 4140,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecCO2EmissionInterurban = 4150,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecCO2EmissionCombined = 4160,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecTotalLength = 4170,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecTotalWidth = 4180,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecTotalHeightUnloaded = 4190,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecWheelBase = 4200,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecLuggageSpace = 4210,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecGrossVehicleWeight = 4220,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecTotalUnloadedWeight = 4230,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecTowingCapacityBraked = 4240,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecTowingCapacityUnbraked = 4250,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecNoVARate = 4260,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecEngineConfiguration = 4270,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecTransmissionType = 4280,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecNoxEmission = 4290,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecParticlesEmission = 4300,
        [System.Runtime.Serialization.EnumMemberAttribute]
		TecActuation = 4310,
        
	}


	// TESTED BY EnumImageFileTypeConstantsTestFixture.CheckLength
	// TESTED BY EnumImageFileTypeConstantsTestFixture.CheckMinValue
	// TESTED BY EnumImageFileTypeConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum ImageFileTypeConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		Gif = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Jpeg = 1,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Png = 2,
	}

	// TESTED BY EnumPictureTypeConstantsTestFixture.CheckLength
	// TESTED BY EnumPictureTypeConstantsTestFixture.CheckMinValue
	// TESTED BY EnumPictureTypeConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum PictureTypeConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		Undefined = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Exterior = 1,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Interior = 2,
	}

	// TESTED BY EnumComponentTypeConstantsTestFixture.CheckLength
	// TESTED BY EnumComponentTypeConstantsTestFixture.CheckMinValue
	// TESTED BY EnumComponentTypeConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum ComponentTypeConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		Undefined = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Equipment = 1,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Accessory = 2,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Package = 3,
        [System.Runtime.Serialization.EnumMemberAttribute]
		ExteriorDesign = 4,
        [System.Runtime.Serialization.EnumMemberAttribute]
		InteriorDesign = 5,
        
	}


	// TESTED BY EnumComponentDetailTypeConstantsTestFixture.CheckLength
	// TESTED BY EnumComponentDetailTypeConstantsTestFixture.CheckMinValue
	// TESTED BY EnumComponentDetailTypeConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum ComponentDetailTypeConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		Undefined = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		General = 1,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Technic = 2,
		// NOTE BK, Textual removed at 04.06.2008
		//Textual,
	}

	// TESTED BY EnumComponentDetailValueTypeConstantsTestFixture.CheckLength
	// TESTED BY EnumComponentDetailValueTypeConstantsTestFixture.CheckMinValue
	// TESTED BY EnumComponentDetailValueTypeConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum ComponentDetailValueTypeConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		Undefined = 0,
		// Basisdaten, 1000, Bas	
		// Technische Daten, 2000, Tec	
	}

	// TESTED BY PriceInclusionConstantsTestFixture.CheckLength
	// TESTED BY PriceInclusionConstantsTestFixture.CheckMinValue
	// TESTED BY PriceInclusionConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
	[System.FlagsAttribute]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum PriceInclusionConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		None = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		ValueAddedTax = 1,
        [System.Runtime.Serialization.EnumMemberAttribute]
		SpecialCarTax = 2,
        [System.Runtime.Serialization.EnumMemberAttribute]
		ImportDuty = 4,
        [System.Runtime.Serialization.EnumMemberAttribute]
		ShippingCost = 8

	}

	// TESTED BY GetTreeNodeSearchModeConstantsTestFixture.CheckLength
	// TESTED BY GetTreeNodeSearchModeConstantsTestFixture.CheckMinValue
	// TESTED BY GetTreeNodeSearchModeConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum GetTreeNodeSearchModeConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		PreviousLevels = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		PreviousLevel = 1,
        [System.Runtime.Serialization.EnumMemberAttribute]
		NextLevel = 2,
        [System.Runtime.Serialization.EnumMemberAttribute]
		NextLevels = 3,
	}

	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum SearchBy : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		Id = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Level0Name,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Level1Name,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Level2Name,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Level3Name
	}

	#endregion
}
