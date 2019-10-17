// OWNER: BK, 02-07-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    /// <summary>
    /// Catalog Item
    /// </summary>
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public sealed class CatalogItem : Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ICatalogItem, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.IInputData
	{
		#region Private variables
		private string _Name;
		private string _Description;
		#endregion

		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		public CatalogItem()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="catalogItem"></param>
        /// <todo> BK 0 BK, Not tested</todo>
		internal CatalogItem(Cic.P000001.Common.ConfigurationManager.CatalogItem catalogItem)
		{
			// Check object
			if (catalogItem == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("catalogItem");
			}

			// Set values
			_Name = catalogItem.Name;
			_Description = catalogItem.Description;
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
				// Create catalog item
				Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertCatalogItem(this);
			}
			// TODO BK 0 BK, catch specific exceptions
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region ICatalogItemBase properties
        /// <summary>
        /// Name
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Name
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _Name;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_Name = value;
			}
		}

        /// <summary>
        /// Description
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Description
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _Description;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_Description = value;
			}
		}
		#endregion
	}
}
