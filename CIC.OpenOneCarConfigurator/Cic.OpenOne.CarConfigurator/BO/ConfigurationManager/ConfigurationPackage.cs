// OWNER: BK, 09-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    /// <summary>
    /// Configuration Package Class
    /// </summary>
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public class ConfigurationPackage : Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.IConfigurationPackage, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.IInputData
	{
		#region Private variables
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
		private Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem[] _CatalogItems;
		private Cic.P000001.Common.DataSourceInformation _DataSourceInformation;
		private Cic.P000001.Common.Setting _Setting;
		private Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationTreeNode _ConfigurationTreeNode;
		private Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent[] _ConfigurationComponents;
		#endregion

		#region Constructors
        /// <summary>
        /// Standard Constructore
		/// TODO BK 0 BK, Not tested
		/// A constructor without parameters is needed for serialization
        /// </summary>
		public ConfigurationPackage()
		{
		}

        /// <summary>
        /// Constuctor
		/// TODO BK 0 BK, Not tested
        /// </summary>
        /// <param name="configurationPackage">Package</param>
		internal ConfigurationPackage(Cic.P000001.Common.ConfigurationManager.ConfigurationPackage configurationPackage)
		{
			// Check object
			if (configurationPackage == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("configurationPackage");
			}

			try
			{
				// Set values
                _CatalogItems = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerCommonToWebConvertHelper.ConvertCatalogItems(configurationPackage.CatalogItems);
				_DataSourceInformation = configurationPackage.DataSourceInformation;
				_Setting = configurationPackage.Setting;
				// TODO BK 0 BK, Check null for tree node and components
				_ConfigurationTreeNode = new Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationTreeNode(configurationPackage.ConfigurationTreeNode);
                _ConfigurationComponents = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerCommonToWebConvertHelper.ConvertConfigurationComponents(configurationPackage.ConfigurationComponents);
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Set values
			_Identifier = configurationPackage.Identifier;
			_UserCode = configurationPackage.UserCode;
			_Designation = configurationPackage.Designation;
			_Description = configurationPackage.Description;
			_CreatedAt = configurationPackage.CreatedAt;
			_LastModifiedAt = configurationPackage.LastModifiedAt;
			_IsPublic = configurationPackage.IsPublic;
			_IsLocked = configurationPackage.IsLocked;
			_GroupName = configurationPackage.GroupName;
			_GroupDescription = configurationPackage.GroupDescription;
		}
		#endregion // Constructors

		#region IInputData methods
        /// <summary>
        /// Check Properties
        /// </summary>
		public void CheckProperties()
		{
			try
			{
				// Create configuration package
                Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertConfiguration(this);
			}
			// TODO BK 0 BK, catch specific exceptions
			catch
			{
				// Throw caught exception
				throw;
			}
		}
        #endregion // IInputData methods

        #region IConfigurationBase properties

        /// <summary>
        /// Identifier
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public System.Guid Identifier
		{
			// TODO BK 0 BK, Not tested
			get { return _Identifier; }
			// TODO BK 0 BK, Not tested
			set { _Identifier = value; }
		}

        /// <summary>
        /// User Code
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string UserCode
		{
			// TODO BK 0 BK, Not tested
			get { return _UserCode; }
			// TODO BK 0 BK, Not tested
			set { _UserCode = value; }
		}

        /// <summary>
        /// Designation
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Designation
		{
			// TODO BK 0 BK, Not tested
			get { return _Designation; }
			// TODO BK 0 BK, Not tested
			set { _Designation = value; }
		}

        /// <summary>
        /// Description
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string Description
		{
			// TODO BK 0 BK, Not tested
			get { return _Description; }
			// TODO BK 0 BK, Not tested
			set { _Description = value; }
		}

        /// <summary>
        /// Created At
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public System.DateTime CreatedAt
		{
			// TODO BK 0 BK, Not tested
			get { return _CreatedAt; }
			// TODO BK 0 BK, Not tested
			set { _CreatedAt = value; }
		}

        /// <summary>
        /// Last Modified
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public System.DateTime LastModifiedAt
		{
			// TODO BK 0 BK, Not tested
			get { return _LastModifiedAt; }
			// TODO BK 0 BK, Not tested
			set { _LastModifiedAt = value; }
		}

        /// <summary>
        /// Is Public
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool IsPublic
		{
			// TODO BK 0 BK, Not tested
			get { return _IsPublic; }
			// TODO BK 0 BK, Not tested
			set { _IsPublic = value; }
		}

        /// <summary>
        /// Is Locked
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public bool IsLocked
		{
			// TODO BK 0 BK, Not tested
			get { return _IsLocked; }
			// TODO BK 0 BK, Not tested
			set { _IsLocked = value; }
		}

        /// <summary>
        /// Group Name
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string GroupName
		{
			// TODO BK 0 BK, Not tested
			get { return _GroupName; }
			// TODO BK 0 BK, Not tested
			set { _GroupName = value; }
		}

        /// <summary>
        /// Group Description
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public string GroupDescription
		{
			// TODO BK 0 BK, Not tested
			get { return _GroupDescription; }
			// TODO BK 0 BK, Not tested
			set { _GroupDescription = value; }
		}
        #endregion // IConfigurationBase properties

        #region IConfiguration properties
        /// <summary>
        /// Catalog Items
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem[] CatalogItems
		{
			// TODO BK 0 BK, Not tested
			get { return _CatalogItems; }
			// TODO BK 0 BK, Not tested
			set { _CatalogItems = value; }
		}

        /// <summary>
        /// DataSource Information
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.DataSourceInformation DataSourceInformation
		{
			// TODO BK 0 BK, Not tested
			get { return _DataSourceInformation; }
			// TODO BK 0 BK, Not tested
			set { _DataSourceInformation = value; }
		}

        /// <summary>
        /// Setting
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Setting Setting
		{
			// TODO BK 0 BK, Not tested
			get { return _Setting; }
			// TODO BK 0 BK, Not tested
			set { _Setting = value; }
		}

        /// <summary>
        /// Configuration Tree Node
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationTreeNode ConfigurationTreeNode
		{
			// TODO BK 0 BK, Not tested
			get { return _ConfigurationTreeNode; }
			// TODO BK 0 BK, Not tested
			set { _ConfigurationTreeNode = value; }
		}

        /// <summary>
        /// Configuration Components
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent[] ConfigurationComponents
		{
			// TODO BK 0 BK, Not tested
			get { return _ConfigurationComponents; }
			// TODO BK 0 BK, Not tested
			set { _ConfigurationComponents = value; }
		}
		#endregion //IConfiguration properties
	}
}
