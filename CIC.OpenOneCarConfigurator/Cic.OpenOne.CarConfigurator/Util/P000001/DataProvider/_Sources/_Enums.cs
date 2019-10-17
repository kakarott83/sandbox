// OWNER: BK, 04-02-2008
namespace Cic.P000001.Common.DataProvider
{
	#region Enums
	// TESTEDBY CheckComponentExpressionConstantsTestFixture.CheckLength
	// TESTEDBY CheckComponentExpressionConstantsTestFixture.CheckMinValue
	// TESTEDBY CheckComponentExpressionConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum CheckComponentExpressionConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
        Conflict = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
        Requirement = 1,
        [System.Runtime.Serialization.EnumMemberAttribute]
		NewPrice = 2,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Discount = 3,
	}

	// TESTEDBY CheckComponentResultConstantsTestFixture.CheckLength
	// TESTEDBY CheckComponentResultConstantsTestFixture.CheckMinValue
	// TESTEDBY CheckComponentResultConstantsTestFixture.CheckMaxValue
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    
	public enum CheckComponentResultConstants : int
	{
        [System.Runtime.Serialization.EnumMemberAttribute]
		Valid = 0,
        [System.Runtime.Serialization.EnumMemberAttribute]
		UnknownError = 1,
        [System.Runtime.Serialization.EnumMemberAttribute]
		InvalidTreeNode = 2,
        [System.Runtime.Serialization.EnumMemberAttribute]
		InvalidComponent = 3,
        [System.Runtime.Serialization.EnumMemberAttribute]
		ComponentIsStillSelected = 4,
        [System.Runtime.Serialization.EnumMemberAttribute]
		DuplicateComponents = 5,
        [System.Runtime.Serialization.EnumMemberAttribute]
		Relations = 6,
	}
	#endregion
}
