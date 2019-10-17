// OWNER: BK, 06-06-2008
namespace Cic.P000001.Common.ConfigurationManager
{
	#region Using directives
	
	#endregion

	[System.Serializable]
	[System.CLSCompliant(true)]
	public sealed class ConfigurationPackage : Cic.P000001.Common.ConfigurationManager.IConfigurationPackage, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
		private bool _Parameterless=true;
		private System.Guid _Identifier;
		private string _UserCode;
		private string _Designation;
		private string _Description;
		private System.DateTime _CreatedAt;
		private System.DateTime _LastModifiedAt;
		private bool _IsPublic;
		private bool _IsLocked;
		private string _GroupName;
		private string _GroupDescription;
		private Cic.P000001.Common.ConfigurationManager.CatalogItem[] _CatalogItems;
		private Cic.P000001.Common.DataSourceInformation _DataSourceInformation;
		private Cic.P000001.Common.Setting _Setting;
		private Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode _ConfigurationTreeNode;
		private Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] _ConfigurationComponents;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
		public ConfigurationPackage()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithoutUserCode
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithEmptyUserCode
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithSpaceUserCode
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithoutDesignation
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithEmptyDesignation
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithSpaceDesignation
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithSmallerLastModifiedAtThenCreatedAt
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithoutGroupName
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithEmptyGroupName
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithSpaceGroupName
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithInvalidCatalogItems
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithoutDataSourceInformation
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithoutSetting
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithoutConfigurationTreeNode
		// TESTEDBY ConfigurationPackageTestFixture.ConstructWithInvalidConfigurationComponents
		// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
		public ConfigurationPackage(System.Guid identifier, string userCode, string designation, string description, System.DateTime createdAt, System.DateTime lastModifiedAt, bool isPublic, bool isLocked, string groupName, string groupDescription, Cic.P000001.Common.ConfigurationManager.CatalogItem[] catalogItems, Cic.P000001.Common.DataSourceInformation dataSourceInformation, Cic.P000001.Common.Setting setting, Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode configurationTreeNode, Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] configurationComponents)
		{
			int Index;

			// Check user code
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(userCode))
			{
				// Throw exception
				throw  new System.ArgumentException("userCode");
			}

			// Check designation
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(designation))
			{
				// Throw exception
				throw  new System.ArgumentException("designation");
			}

			// Check created at and last modified at
			if (lastModifiedAt < createdAt)
			{
				// Throw exception
                throw new System.ArgumentException("lastModifiedAt", "createdAt");
			}

			// Check group name
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(groupName))
			{
				// Throw exception
				throw  new System.ArgumentException("groupName");
			}

			// Check catalog items
			if ((catalogItems != null) && (catalogItems.GetLength(0) > 0))
			{
				// Reset index
				Index = -1;
				// Loop through catalog items
				foreach (Cic.P000001.Common.ConfigurationManager.CatalogItem LoopCatalogItem in catalogItems)
				{
					// Increase index
					Index += 1;
					// Check catalog item
					if (LoopCatalogItem == null)
					{
						// Throw exception
						throw new System.ArgumentException("catalogItems[" + Index.ToString() + "]");
					}
				}
			}

			// Check data source informations
			if (dataSourceInformation == null)
			{
				// Throw exception
				throw new System.ArgumentException("dataSourceInformation");
			}

			// Check setting
			if (setting == null)
			{
				// Throw exception
				throw new System.ArgumentException("setting");
			}

			// Check configurationPackage tree node
			if (configurationTreeNode == null)
			{
				// Throw exception
				throw new System.ArgumentException("configurationTreeNode");
			}

			// Check configurationPackage components
			if ((configurationComponents != null) && (configurationComponents.GetLength(0) > 0))
			{
				// Reset index
				Index = -1;
				// Loop through configurationPackage components
				foreach (Cic.P000001.Common.ConfigurationManager.ConfigurationComponent LoopConfigurationComponent in configurationComponents)
				{
					// Increase index
					Index += 1;
					// Check configurationPackage component
					if (LoopConfigurationComponent == null)
					{
						// Throw exception
						throw new System.ArgumentException("configurationComponents[" + Index.ToString() + "]");
					}
				}
			}

			// Set values
			_Identifier = identifier;
			_UserCode = userCode.Trim();
			_Designation = designation.Trim();
			_Description = description;
			_CreatedAt = createdAt;
			_LastModifiedAt = lastModifiedAt;
			_IsPublic = isPublic;
			_IsLocked = isLocked;
			_GroupName = groupName.Trim();
			_GroupDescription = groupDescription;
			_CatalogItems = catalogItems;
			_DataSourceInformation = dataSourceInformation;
			_Setting = setting;
			_ConfigurationTreeNode = configurationTreeNode;
			_ConfigurationComponents = configurationComponents;
		}
		#endregion

		#region IReconstructable methods
		// NOTE BK, Essential for reconstructing
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.ConfigurationManager.ConfigurationPackage")]
		// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct catalog items
					if ((_CatalogItems != null) && (_CatalogItems.GetLength(0) > 0))
					{
						// Loop through catalog items
						foreach (Cic.P000001.Common.ConfigurationManager.CatalogItem LoopCatalogItem in _CatalogItems)
						{
							// Check object
							if (LoopCatalogItem != null)
							{
								// Reconstruct
								LoopCatalogItem.Reconstruct();
							}
						}
					}
					// Reconstruct data source information
					if (_DataSourceInformation != null)
					{
						// Reconstruct
						_DataSourceInformation.Reconstruct();
					}
					// Reconstruct setting
					if (_Setting != null)
					{
						// Reconstruct
						_Setting.Reconstruct();
					}
					// Reconstruct configurationPackage tree node
					if (_ConfigurationTreeNode != null)
					{
						// Reconstruct
						_ConfigurationTreeNode.Reconstruct();
					}
					// Reconstruct configurationPackage components
					if ((_ConfigurationComponents != null) && (_ConfigurationComponents.GetLength(0) > 0))
					{
						// Loop through configurationPackage components
						foreach (Cic.P000001.Common.ConfigurationManager.ConfigurationComponent LoopConfigurationComponent in _ConfigurationComponents)
						{
							// Check object
							if (LoopConfigurationComponent != null)
							{
								// Reconstruct
								LoopConfigurationComponent.Reconstruct();
							}
						}
					}

					// Create new instance
					new Cic.P000001.Common.ConfigurationManager.ConfigurationPackage(_Identifier, _UserCode, _Designation, _Description, _CreatedAt, _LastModifiedAt, _IsPublic, _IsLocked, _GroupName, _GroupDescription, _CatalogItems, _DataSourceInformation, _Setting, _ConfigurationTreeNode, _ConfigurationComponents);
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

		#region IConfigurationPackageBase Properties
		public System.Guid Identifier
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _Identifier; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Identifier = value;
				}
			}
		}

		public string UserCode
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _UserCode; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_UserCode = value;
				}
			}
		}

		public string Designation
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _Designation; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Designation = value;
				}
			}
		}

		public string Description
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _Description; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
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

		public System.DateTime CreatedAt
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _CreatedAt; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_CreatedAt = value;
				}
			}
		}

		public System.DateTime LastModifiedAt
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _LastModifiedAt; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_LastModifiedAt = value;
				}
			}
		}

		public bool IsPublic
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _IsPublic; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_IsPublic = value;
				}
			}
		}

		public bool IsLocked
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _IsLocked; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_IsLocked = value;
				}
			}
		}

		public string GroupName
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _GroupName; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_GroupName = value;
				}
			}
		}

		public string GroupDescription
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _GroupDescription; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_GroupDescription = value;
				}
			}
		}
		#endregion

		#region IConfigurationPackage Properties
		public Cic.P000001.Common.ConfigurationManager.CatalogItem[] CatalogItems
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _CatalogItems; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_CatalogItems = value;
				}
			}
		}

		public Cic.P000001.Common.DataSourceInformation DataSourceInformation
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _DataSourceInformation; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_DataSourceInformation = value;
				}
			}
		}

		public Cic.P000001.Common.Setting Setting
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _Setting; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Setting = value;
				}
			}
		}

		public Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode ConfigurationTreeNode
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _ConfigurationTreeNode; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ConfigurationTreeNode = value;
				}
			}
		}

		public Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] ConfigurationComponents
		{
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			get { return _ConfigurationComponents; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationPackageTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_ConfigurationComponents = value;
				}
			}
		}
		#endregion
	}
}
