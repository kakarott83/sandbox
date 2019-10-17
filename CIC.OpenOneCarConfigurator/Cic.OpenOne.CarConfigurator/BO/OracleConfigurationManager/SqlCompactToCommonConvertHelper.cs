using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
using Cic.OpenOne.CarConfigurator.Util.Xml;
namespace Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager
{
    // [System.CLSCompliant(true)]
    internal class SqlCompactToCommonConvertHelper
	{
		#region Private constants
		// TODO BK 0 BK, Localize text
		private const string CnstDataSourceInformationCopyright = "Unknown";
		#endregion

		#region Constructors
		// NOTE BK, Private Constructur, to avoid the creation of a standard constructor through the compiler
		// TODO BK 0 BK, Not tested
		private SqlCompactToCommonConvertHelper()
		{
		}
		#endregion

		#region Methods
        internal static Cic.P000001.Common.ConfigurationManager.CatalogItem[] ConvertCatalogItems(Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS configurationPackage)
		{
			System.Collections.Generic.Dictionary<string, Cic.P000001.Common.ConfigurationManager.CatalogItem> CatalogItemDictionary;
			Cic.P000001.Common.ConfigurationManager.CatalogItem CatalogItem;
			Cic.P000001.Common.ConfigurationManager.CatalogItem[] CatalogItems = null;
			string Name;
			int Index;

			// Check configurationPackage and catalog configurationPackage
			if ((configurationPackage != null) && (configurationPackage.CMCATCPKGSList.Count > 0))
			{
				// New dictionary
				CatalogItemDictionary = new System.Collections.Generic.Dictionary<string, Cic.P000001.Common.ConfigurationManager.CatalogItem>();

				try
				{
					// Loop through catalog configurations
                    foreach (Cic.OpenOne.Common.Model.DdCcCm.CMCATCPKGS LoopCatalogConfiguration in configurationPackage.CMCATCPKGSList)
					{
						// Get name of catalog item
						Name = LoopCatalogConfiguration.CMCATITEMS.NAME;
						// Check containing
						if (!CatalogItemDictionary.ContainsKey(Name))
						{
							// Create new catalog item
							CatalogItem = new Cic.P000001.Common.ConfigurationManager.CatalogItem(Name, LoopCatalogConfiguration.CMCATITEMS.DESCRIPTION);
							// Add to dictionary
							CatalogItemDictionary.Add(Name, CatalogItem);
						}
					}
				}
				catch
				{
					// Throw caught exception
					throw;
				}

				// Check count
				if (CatalogItemDictionary.Count > 0)
				{
					// Resize
					System.Array.Resize<Cic.P000001.Common.ConfigurationManager.CatalogItem>(ref CatalogItems, CatalogItemDictionary.Count);
					// Reset index
					Index = -1;
					// Loop through dictionary
					foreach (Cic.P000001.Common.ConfigurationManager.CatalogItem LoopCatalogItem in CatalogItemDictionary.Values)
					{
						// Increase indes
						Index += 1;
						// Set value
						CatalogItems[Index] = LoopCatalogItem;
					}
				}
			}

			// Return
			return CatalogItems;
		}

		internal static Cic.P000001.Common.DataSourceInformation ConvertDataSourceInformation(Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS configurationPackage)
		{
            Cic.OpenOne.Common.Model.DdCcCm.CMDSRCVERS DataSourceVersion;
            Cic.OpenOne.Common.Model.DdCcCm.CMDATASRCS  DataSource;
			Cic.P000001.Common.DataSourceInformation DataSourceInformation = null;
			string[] AvailableLanguages;
			string[] AvailableCurrencies;

			// Check configurationPackage
			if (configurationPackage != null)
			{
				// Set objects
				DataSourceVersion = configurationPackage.CMDSRCVERS;
				DataSource = DataSourceVersion.CMDATASRCS;

				// Set string arrays
				AvailableLanguages = new string[1] { configurationPackage.SELECTEDLANGUAGE };
				AvailableCurrencies = new string[1] { configurationPackage.SELECTEDCURRENCY };

				try
				{
					// Create new data source information
					// TODO BK 2 BK, Add proprties to database
                    
					DataSourceInformation = new Cic.P000001.Common.DataSourceInformation(new System.Guid(DataSource.IDENTIFIER), DataSourceVersion.VERSION, DataSourceVersion.DESCRIPTION, DataSource.DESIGNATION, DataSource.DESCRIPTION, CnstDataSourceInformationCopyright, AvailableLanguages, AvailableCurrencies, false, false, false, false, Cic.P000001.Common.PriceInclusionConstants.None, null, null, null, null, null);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return DataSourceInformation;
		}

        internal static Cic.P000001.Common.Setting ConvertSetting(Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS configurationPackage)
		{
			Cic.P000001.Common.Setting Setting = null;

			// Check configurationPackage
			if (configurationPackage != null)
			{
				try
				{
					// Create new setting
					Setting = new Cic.P000001.Common.Setting(configurationPackage.SELECTEDLANGUAGE, configurationPackage.SELECTEDCURRENCY);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return Setting;
		}

        internal static Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode ConvertConfigurationTreeNode(Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS configurationPackage)
		{
			Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode ConfigurationTreeNode = null;

			// Check configurationPackage
			if (configurationPackage != null)
			{
				try
				{
					// Deserialize configurationPackage tree node
					ConfigurationTreeNode = (Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode)SerializationHelper.DeserializeUTF8(configurationPackage.XMLTREENODE, typeof(Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode));
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return ConfigurationTreeNode;
		}

        internal static Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] ConvertConfigurationComponents(Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS configurationPackage)
		{
			Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] ConfigurationComponents = null;

			// Check configurationPackage
			if (configurationPackage != null)
			{
				// Check value
				if (configurationPackage.XMLCOMPONENTS != null)
				{
					try
					{
						// Deserialize configurationPackage components
						ConfigurationComponents = (Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[])SerializationHelper.DeserializeUTF8(configurationPackage.XMLCOMPONENTS, typeof(Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[]));
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}
			}

			// Return
			return ConfigurationComponents;
		}

        internal static Cic.P000001.Common.ConfigurationManager.ConfigurationPackage ConvertConfiguration(Cic.OpenOne.Common.Model.DdCcCm.CMCPKGS configurationPackage)
		{
			Cic.P000001.Common.ConfigurationManager.ConfigurationPackage CommonConfigurationPackage = null;
			Cic.P000001.Common.ConfigurationManager.CatalogItem[] CommonCatalogItems;
			Cic.P000001.Common.DataSourceInformation CommonDataSourceInformation;
			Cic.P000001.Common.Setting CommonSetting;
			Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode CommonConfigurationTreeNode;
			Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] CommonConfigurationComponents;

			// Check configurationPackage
			if (configurationPackage != null)
			{
				try
				{
					// Convert catalog items
					CommonCatalogItems =SqlCompactToCommonConvertHelper.ConvertCatalogItems(configurationPackage);
					// Convert datasource information
					CommonDataSourceInformation =SqlCompactToCommonConvertHelper.ConvertDataSourceInformation(configurationPackage);
					// Convert setting
					CommonSetting =SqlCompactToCommonConvertHelper.ConvertSetting(configurationPackage);
					// Convert configurationPackage tree node
					CommonConfigurationTreeNode =SqlCompactToCommonConvertHelper.ConvertConfigurationTreeNode(configurationPackage);
					// Convert configurationPackage components
					CommonConfigurationComponents =SqlCompactToCommonConvertHelper.ConvertConfigurationComponents(configurationPackage);
					// Create new configurationPackage
                    bool IsPublic = configurationPackage.ISPUBLIC == 1;
                    bool IsLocked = configurationPackage.ISLOCKED == 1;
                    CommonConfigurationPackage = new Cic.P000001.Common.ConfigurationManager.ConfigurationPackage(new System.Guid(configurationPackage.CMCIDENTS.IDENTIFIER), configurationPackage.CMUSERS.CODE, configurationPackage.DESIGNATION, configurationPackage.DESCRIPTION, configurationPackage.CRATEDAT.GetValueOrDefault(), configurationPackage.LASTMODIFIEDAT.GetValueOrDefault(), IsPublic, IsLocked, configurationPackage.CMCONFGRPS.NAME, configurationPackage.CMCONFGRPS.DESCRIPTION, CommonCatalogItems, CommonDataSourceInformation, CommonSetting, CommonConfigurationTreeNode, CommonConfigurationComponents);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonConfigurationPackage;
		}
		#endregion
	}
}
