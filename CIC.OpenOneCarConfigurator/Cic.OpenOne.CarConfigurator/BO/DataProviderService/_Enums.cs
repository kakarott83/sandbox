// OWNER: BK, 03-09-2007
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
	#region Enums
    /// <summary>
    /// Expressio nData Transfer Type Constants
    /// </summary>
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public enum ExpressionDataTransferTypeConstants : int
	{
        /// <summary>
        /// Component
		/// NOTE BK, In WSDL files the start value is always 0
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute]
		Component,
        /// <summary>
        /// And
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute]
		And,
        /// <summary>
        /// Or
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute]
		Or,
	}

    /// <summary>
    /// Search BY
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    public enum SearchBy : int
    {
        /// <summary>
        /// Id
        /// </summary>
        [System.Runtime.Serialization.EnumMemberAttribute]
        Id = 0,
    }
	#endregion
}
