// OWNER: BK, 15-04-2008
namespace Cic.P000001.Common
{
	[System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	[System.CLSCompliant(true)]
    public sealed class Parameter : Cic.P000001.Common.IParameter, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
		private bool _Parameterless=true;
		private string _Category;
		private string _Name;
		private string _Value;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY ParameterTestFixture.CheckProperties
		public Parameter()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY ParameterTestFixture.ConstructWithoutCategory
		// TESTEDBY ParameterTestFixture.ConstructWithEmptyCategory
		// TESTEDBY ParameterTestFixture.ConstructWithSpaceCategory
		// TESTEDBY ParameterTestFixture.ConstructWithoutName
		// TESTEDBY ParameterTestFixture.ConstructWithEmptyName
		// TESTEDBY ParameterTestFixture.ConstructWithSpaceName
		// TESTEDBY ParameterTestFixture.ConstructWithoutValue
		// TESTEDBY ParameterTestFixture.CheckProperties
		public Parameter(string category, string name, string value)
		{
			// Check category
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(category))
			{
				// Throw exception
				throw  new System.ArgumentException("category");
			}

			// Check name
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(name))
			{
				// Throw exception
				throw  new System.ArgumentException("name");
			}

			// Check value
			if (value == null)
			{
				// Throw exception
				throw new System.ArgumentException("value");
			}

			// Set values
			_Category = category.Trim();
			_Name = name.Trim();
			_Value = value.Trim();
		}
		#endregion

		#region IReconstructable methods
		// NOTE BK, Essential for reconstructing
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.Parameter")]
		// TESTEDBY ParameterTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct
					// Create new instance
					new Cic.P000001.Common.Parameter(_Category, _Name, _Value);
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

		#region IParameterBase properties
        
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Category
		{
			// TESTEDBY ParameterTestFixture.CheckProperties
			get
			{
				// Return
				return _Category;
			}
			// NOTE BK, For serialization
			// TESTEDBY ParameterTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Category = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Name
		{
			// TESTEDBY ParameterTestFixture.CheckProperties
			get
			{
				// Return
				return _Name;
			}
			// NOTE BK, For serialization
			// TESTEDBY ParameterTestFixture.CheckProperties
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
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Value
		{
			// TESTEDBY ParameterTestFixture.CheckProperties
			get
			{
				// Return
				return _Value;
			}
			// NOTE BK, For serialization
			// TESTEDBY ParameterTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Value = value;
				}
			}
		}
		#endregion
	}
}
