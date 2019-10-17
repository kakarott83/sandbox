// OWNER: BK, 02-07-2008
namespace Cic.P000001.Common.ConfigurationManager
{
    [System.Serializable]
	[System.CLSCompliant(true)]
	public sealed class CatalogItem : Cic.P000001.Common.ConfigurationManager.ICatalogItem, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
		private bool _Parameterless=true;
		private string _Name;
		private string _Description;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY CatalogItemTestFixture.CheckProperties
		public CatalogItem()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY CatalogItemTestFixture.ConstructWithoutName
		// TESTEDBY CatalogItemTestFixture.ConstructWithEmptyName
		// TESTEDBY CatalogItemTestFixture.ConstructWithSpaceName
		// TESTEDBY CatalogItemTestFixture.CheckProperties
		public CatalogItem(string name, string description)
		{
			// Check name
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(name))
			{
				// Throw exception
				throw  new System.ArgumentException("name");
			}

			// Set values
			_Name = name.Trim();
			_Description = description;
		}
		#endregion

		#region IReconstructable methods
		// NOTE BK, Essential for reconstructing
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.ConfigurationManager.CatalogItem")]
		// TESTEDBY CatalogItemTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Create new instance
					new Cic.P000001.Common.ConfigurationManager.CatalogItem(_Name, _Description);
				}
				catch
				{
					// Throw caught exception
					throw;
				}

				// Reset state
				_Parameterless = false;
			}
		}
		#endregion

		#region ICatalogItemBase properties
		public string Name
		{
			// TESTEDBY CatalogItemTestFixture.CheckProperties
			get
			{
				// Return
				return _Name;
			}
			// NOTE BK, For serialization
			// TESTEDBY CatalogItemTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Name = value;
				}
			}
		}

		public string Description
		{
			// TESTEDBY CatalogItemTestFixture.CheckProperties
			get
			{
				// Return
				return _Description;
			}
			// NOTE BK, For serialization
			// TESTEDBY CatalogItemTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Description = value;
				}
			}
		}
		#endregion
	}
}
