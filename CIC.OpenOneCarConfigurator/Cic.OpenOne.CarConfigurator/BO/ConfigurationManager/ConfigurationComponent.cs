// OWNER: BK, 09-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    /// <summary>
    /// Configuration Component
    /// </summary>
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public class ConfigurationComponent : Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.IConfigurationComponent, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.IInputData
	{
		#region Private variables
		private Cic.P000001.Common.Component _Component;
		private Cic.P000001.Common.ComponentDetail[] _ComponentDetails;
		private Cic.P000001.Common.Picture[] _Pictures;
		#endregion

		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		public ConfigurationComponent()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurationComponent"></param>
        /// <todo>BK 0 BK, Not tested</todo>
		internal ConfigurationComponent(Cic.P000001.Common.ConfigurationManager.ConfigurationComponent configurationComponent)
		{
			// Check object
			if (configurationComponent == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("configurationComponent");
			}

			try
			{
				// Set values
				_Component = configurationComponent.Component;
                _ComponentDetails = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerCommonToWebConvertHelper.ConvertComponentDetails(configurationComponent.ComponentDetails);
                _Pictures = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerCommonToWebConvertHelper.ConvertPictures(configurationComponent.Pictures);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region IInputData methods
        /// <summary>
        /// Check Properties
        /// </summary>
		public void CheckProperties()
		{
			try
			{
				// Create configurationPackage component
                Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertConfigurationComponent(this);
			}
			// TODO BK 0 BK, catch specific exceptions
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region IConfigurationComponent properties
        /// <summary>
        /// Component
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Component Component
		{
			// TODO BK 0 BK, Not tested
			get { return _Component; }
			// TODO BK 0 BK, Not tested
			set { _Component = value; }
		}

        /// <summary>
        /// Component Details
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.ComponentDetail[] ComponentDetails
		{
			// TODO BK 0 BK, Not tested
			get { return _ComponentDetails; }
			// TODO BK 0 BK, Not tested
			set { _ComponentDetails = value; }
		}

        /// <summary>
        /// Pictures
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Picture[] Pictures
		{
			// TODO BK 0 BK, Not tested
			get { return _Pictures; }
			// TODO BK 0 BK, Not tested
			set { _Pictures = value; }
		}
		#endregion
	}
}
