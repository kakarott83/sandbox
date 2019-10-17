// OWNER: BK, 22-04-2008
namespace Cic.P000001.Common
{
	[System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	[System.CLSCompliant(true)]
	public sealed class ComponentDetail : Cic.P000001.Common.IComponentDetail, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
		private bool _Parameterless=true;
		private Cic.P000001.Common.ComponentDetailTypeConstants _ComponentDetailTypeConstant;
		private Cic.P000001.Common.ComponentDetailValueTypeConstants _ComponentDetailValueTypeConstant;
		private string _Category;
		private string _Name;
		private string _Value;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY ComponentTestFixture.CheckProperties
		public ComponentDetail()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY ComponentDetailTestFixture.ConstructWithInvalidComponentDetailTypeConstant
		// TESTEDBY ComponentDetailTestFixture.ConstructWithInvalidComponentDetailValueTypeConstant
		// TESTEDBY ComponentDetailTestFixture.ConstructWithEmptyCategory
		// TESTEDBY ComponentDetailTestFixture.ConstructWithSpaceCategory
		// TESTEDBY ComponentDetailTestFixture.ConstructWithEmptyName
		// TESTEDBY ComponentDetailTestFixture.ConstructWithSpaceName
		// TESTEDBY ComponentDetailTestFixture.ConstructWithoutValue
		// TESTEDBY ComponentDetailTestFixture.CheckProperties
		public ComponentDetail(Cic.P000001.Common.ComponentDetailTypeConstants componentDetailTypeConstants, Cic.P000001.Common.ComponentDetailValueTypeConstants componentDetailValueTypeConstants, string category, string name, string value)
		{
			// Check component type constant
			if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.ComponentDetailTypeConstants), componentDetailTypeConstants))
			{
				// Throw exception
				throw  new System.ArgumentException("componentDetailTypeConstants");
			}

			// Check component value type constant
			if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.ComponentDetailValueTypeConstants), componentDetailValueTypeConstants))
			{
				// Throw exception
				throw  new System.ArgumentException("componentDetailValueTypeConstants");
			}

			// Check category
			if (Cic.OpenOne.Util.StringHelper.IsTrimedEmpty(category))
			{
				// Throw exception
				throw  new System.ArgumentException("category");
			}

            // Check name
			if (Cic.OpenOne.Util.StringHelper.IsTrimedEmpty(name))
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
			_ComponentDetailTypeConstant = componentDetailTypeConstants;
			_ComponentDetailValueTypeConstant = componentDetailValueTypeConstants;
			if (!string.IsNullOrEmpty(category))
			{
				_Category = category.Trim();
			}
			if (!string.IsNullOrEmpty(name))
			{
				_Name = name.Trim();
			}
			_Value = value.Trim();
		}
		#endregion

		#region IReconstructable methods
		// NOTE BK, Essential for reconstructing
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.ComponentDetail")]
		// TESTEDBY ComponentDetailTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct
					// Create new instance
					new Cic.P000001.Common.ComponentDetail(_ComponentDetailTypeConstant, _ComponentDetailValueTypeConstant, _Category, _Name, _Value);
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

		#region IComponentDetailBase properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.ComponentDetailTypeConstants ComponentDetailTypeConstant
		{
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _ComponentDetailTypeConstant;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ComponentDetailTypeConstant = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.ComponentDetailValueTypeConstants ComponentDetailValueTypeConstant
		{
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _ComponentDetailValueTypeConstant;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ComponentDetailValueTypeConstant = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Category
		{
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _Category;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
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
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _Name;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
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
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _Value;
			}
			// NOTE BK, For serialization
			// TESTEDBY ComponentDetailTestFixture.CheckProperties
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
