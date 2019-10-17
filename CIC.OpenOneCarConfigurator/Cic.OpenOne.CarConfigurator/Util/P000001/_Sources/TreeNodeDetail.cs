// OWNER: BK, 22-04-2008
namespace Cic.P000001.Common
{
	[System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	[System.CLSCompliant(true)]
	public sealed class TreeNodeDetail : Cic.P000001.Common.ITreeNodeDetail, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
		private bool _Parameterless=true;
		private Cic.P000001.Common.TreeNodeDetailTypeConstants _TreeNodeDetailTypeConstant;
		private Cic.P000001.Common.TreeNodeDetailValueTypeConstants _TreeNodeDetailValueTypeConstant;
		private string _Category;
		private string _Name;
		private string _Value;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY ComponentTestFixture.CheckProperties
		public TreeNodeDetail()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY TreeNodeDetailTestFixture.ConstructWithInvalidTreeNodeDetailTypeConstant
		// TESTEDBY TreeNodeDetailTestFixture.ConstructWithInvalidTreeNodeDetailValueTypeConstant
		// TESTEDBY TreeNodeDetailTestFixture.ConstructWithEmptyCategory
		// TESTEDBY TreeNodeDetailTestFixture.ConstructWithSpaceCategory
		// TESTEDBY TreeNodeDetailTestFixture.ConstructWithEmptyName
		// TESTEDBY TreeNodeDetailTestFixture.ConstructWithSpaceName
		// TESTEDBY TreeNodeDetailTestFixture.ConstructWithoutValue
		// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
		public TreeNodeDetail(Cic.P000001.Common.TreeNodeDetailTypeConstants treeNodeDetailTypeConstants, Cic.P000001.Common.TreeNodeDetailValueTypeConstants treeNodeDetailValueTypeConstants, string category, string name, string value)
		{
			// Check component type constant
			if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.TreeNodeDetailTypeConstants), treeNodeDetailTypeConstants))
			{
				// Throw exception
				throw  new System.ArgumentException("treeNodeDetailTypeConstants");
			}

			// Check component value type constant
			if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.TreeNodeDetailValueTypeConstants), treeNodeDetailValueTypeConstants))
			{
				// Throw exception
				throw  new System.ArgumentException("treeNodeDetailValueTypeConstants");
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
			_TreeNodeDetailTypeConstant = treeNodeDetailTypeConstants;
			_TreeNodeDetailValueTypeConstant = treeNodeDetailValueTypeConstants;
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.TreeNodeDetail")]
		// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct
					// Create new instance
					new Cic.P000001.Common.TreeNodeDetail(_TreeNodeDetailTypeConstant, _TreeNodeDetailValueTypeConstant, _Category, _Name, _Value);
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

		#region ITreeNodeDetailBase properties
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.TreeNodeDetailTypeConstants TreeNodeDetailTypeConstant
		{
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _TreeNodeDetailTypeConstant;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_TreeNodeDetailTypeConstant = value;
				}
			}
		}

        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.TreeNodeDetailValueTypeConstants TreeNodeDetailValueTypeConstant
		{
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _TreeNodeDetailValueTypeConstant;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_TreeNodeDetailValueTypeConstant = value;
				}
			}
		}
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Category
		{
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _Category;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
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
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _Name;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
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
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
			get
			{
				// Return
				return _Value;
			}
			// NOTE BK, For serialization
			// TESTEDBY TreeNodeDetailTestFixture.CheckProperties
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
