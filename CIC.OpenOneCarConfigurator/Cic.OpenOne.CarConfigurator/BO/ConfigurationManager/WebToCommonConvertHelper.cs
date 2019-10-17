// OWNER: BK, 10-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class ConfigurationManagerWebToCommonConvertHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ConfigurationManager.CatalogItem ConvertCatalogItem(Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem webCatalogItem)
		{
			Cic.P000001.Common.ConfigurationManager.CatalogItem CommonCatalogItem = null;

			// Check web catalog item
			if (webCatalogItem != null)
			{
				try
				{
					// New catalog item
					CommonCatalogItem = new Cic.P000001.Common.ConfigurationManager.CatalogItem(webCatalogItem.Name, webCatalogItem.Description);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonCatalogItem;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ConfigurationManager.CatalogItem[] ConvertCatalogItems(Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem[] webCatalogItems)
		{
			Cic.P000001.Common.ConfigurationManager.CatalogItem[] CommonCatalogItems = null;
			int Index;

			// Check web catalog item
			if (webCatalogItems != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.ConfigurationManager.CatalogItem>(ref CommonCatalogItems, webCatalogItems.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through catalog items
				foreach (Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem LoopCatalogItem in webCatalogItems)
				{
					// Increase index
					Index++;
					try
					{
						// Add new catalog item
						CommonCatalogItems[Index] = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertCatalogItem(LoopCatalogItem);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return CommonCatalogItems;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.DataSourceInformation ConvertDataSourceInformation(Cic.P000001.Common.DataSourceInformation webDataSourceInformation)
		{
			Cic.P000001.Common.DataSourceInformation CommonDataSourceInformation = null;

			// Check web data source information
			if (webDataSourceInformation != null)
			{
				try
				{
					// New data source information
					CommonDataSourceInformation = new Cic.P000001.Common.DataSourceInformation(webDataSourceInformation.Identifier, webDataSourceInformation.Version, webDataSourceInformation.VersionDescription, webDataSourceInformation.Designation, webDataSourceInformation.Description, webDataSourceInformation.Copyright, webDataSourceInformation.AvailableLanguages, webDataSourceInformation.AvailableCurrencies, webDataSourceInformation.StandardEquipmentDetailsSupported, webDataSourceInformation.TechnicalDataDetailsSupported, webDataSourceInformation.ExteriorPicturesSupported, webDataSourceInformation.InteriorPicturesSupported, webDataSourceInformation.PriceInclusionConstant, webDataSourceInformation.ValueAddedTaxInfo, webDataSourceInformation.SpecialCarTaxInfo, webDataSourceInformation.ImportDutyInfo, webDataSourceInformation.ShippingCostInfo, webDataSourceInformation.PriceInfo);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonDataSourceInformation;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Setting ConvertSetting(Cic.P000001.Common.Setting webSetting)
		{
			Cic.P000001.Common.Setting CommonSetting = null;

			// Check web setting
			if (webSetting != null)
			{
				try
				{
					// New setting
					CommonSetting = new Cic.P000001.Common.Setting(webSetting.SelectedLanguage, webSetting.SelectedCurrency);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonSetting;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Level ConvertLevel(Cic.P000001.Common.Level webLevel)
		{
			Cic.P000001.Common.Level CommonLevel = null;

			// Check web level
			if (webLevel != null)
			{
				try
				{
					// New level
					CommonLevel = new Cic.P000001.Common.Level(webLevel.Number, webLevel.Designation);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonLevel;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Picture ConvertPicture(Cic.P000001.Common.Picture webPicture)
		{
			Cic.P000001.Common.Picture CommonPicture = null;

			// Check web picture
			if (webPicture != null)
			{
				try
				{
					// New picture
					CommonPicture = new Cic.P000001.Common.Picture(webPicture.Title, webPicture.PictureTypeConstant, webPicture.Content, webPicture.ImageFileTypeConstant, webPicture.Width, webPicture.Height, webPicture.Hash);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonPicture;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Picture[] ConvertPictures(Cic.P000001.Common.Picture[] webPictures)
		{
			Cic.P000001.Common.Picture[] CommonPictures = null;
			int Index;

			// Check web picture
			if (webPictures != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Picture>(ref CommonPictures, webPictures.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through pictures
				foreach (Cic.P000001.Common.Picture LoopPicture in webPictures)
				{
					// Increase index
					Index++;
					try
					{
						// Add new picture
                        CommonPictures[Index] = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertPicture(LoopPicture);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return CommonPictures;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Parameter ConvertParameter(Cic.P000001.Common.Parameter webParameter)
		{
			Cic.P000001.Common.Parameter CommonParameter = null;

			// Check web parameter
			if (webParameter != null)
			{
				try
				{
					// New parameter
					CommonParameter = new Cic.P000001.Common.Parameter(webParameter.Category, webParameter.Name, webParameter.Value);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonParameter;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Parameter[] ConvertParameters(Cic.P000001.Common.Parameter[] webParameters)
		{
			Cic.P000001.Common.Parameter[] CommonParameters = null;
			int Index;

			// Check web parameter
			if (webParameters != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Parameter>(ref CommonParameters, webParameters.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through parameters
				foreach (Cic.P000001.Common.Parameter LoopParameter in webParameters)
				{
					// Increase index
					Index++;
					try
					{
						// Add new parameter
                        CommonParameters[Index] = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertParameter(LoopParameter);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return CommonParameters;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.TreeNode ConvertTreeNode(Cic.P000001.Common.TreeNode webTreeNode)
		{
			Cic.P000001.Common.TreeNode CommonTreeNode = null;

			// Check web tree node
			if (webTreeNode != null)
			{
				try
				{
					// New tree node
                    CommonTreeNode = new Cic.P000001.Common.TreeNode(webTreeNode.Key, webTreeNode.ParentKey, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertLevel(webTreeNode.Level), webTreeNode.DisplayName, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertParameters(webTreeNode.Parameters), webTreeNode.IsType, webTreeNode.HasDetails, webTreeNode.HasPictures, webTreeNode.Price, webTreeNode.NewPrice);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonTreeNode;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Component ConvertComponent(Cic.P000001.Common.Component webComponent)
		{
			Cic.P000001.Common.Component CommonComponent = null;

			// Check web component
			if (webComponent != null)
			{
				try
				{
					// New component
                    CommonComponent = new Cic.P000001.Common.Component(webComponent.Key, webComponent.ComponentTypeConstant, webComponent.Category, webComponent.DisplayName, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertParameters(webComponent.Parameters), webComponent.HasDetails, webComponent.HasPictures, webComponent.Price, webComponent.NewPrice, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertComponents(webComponent.Components), webComponent.Selectable);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonComponent;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Component[] ConvertComponents(Cic.P000001.Common.Component[] webComponents)
		{
			Cic.P000001.Common.Component[] CommonComponents = null;
			int Index;

			// Check web component
			if (webComponents != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Component>(ref CommonComponents, webComponents.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through components
				foreach (Cic.P000001.Common.Component LoopComponent in webComponents)
				{
					// Increase index
					Index++;
					try
					{
						// Add new component
                        CommonComponents[Index] = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertComponent(LoopComponent);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return CommonComponents;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.TreeNodeDetail ConvertTreeNodeDetail(Cic.P000001.Common.TreeNodeDetail webTreeNodeDetail)
		{
			Cic.P000001.Common.TreeNodeDetail CommonTreeNodeDetail = null;

			// Check web tree node detail
			if (webTreeNodeDetail != null)
			{
				try
				{
					// New tree node detail
					CommonTreeNodeDetail = new Cic.P000001.Common.TreeNodeDetail(webTreeNodeDetail.TreeNodeDetailTypeConstant, webTreeNodeDetail.TreeNodeDetailValueTypeConstant, webTreeNodeDetail.Category, webTreeNodeDetail.Name, webTreeNodeDetail.Value);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonTreeNodeDetail;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.TreeNodeDetail[] ConvertTreeNodeDetails(Cic.P000001.Common.TreeNodeDetail[] webTreeNodeDetails)
		{
			Cic.P000001.Common.TreeNodeDetail[] CommonTreeNodeDetails = null;
			int Index;

			// Check web tree node detail
			if (webTreeNodeDetails != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.TreeNodeDetail>(ref CommonTreeNodeDetails, webTreeNodeDetails.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through tree node details
				foreach (Cic.P000001.Common.TreeNodeDetail LoopTreeNodeDetail in webTreeNodeDetails)
				{
					// Increase index
					Index++;
					try
					{
						// Add new tree node detail
                        CommonTreeNodeDetails[Index] = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertTreeNodeDetail(LoopTreeNodeDetail);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return CommonTreeNodeDetails;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ComponentDetail ConvertComponentDetail(Cic.P000001.Common.ComponentDetail webComponentDetail)
		{
			Cic.P000001.Common.ComponentDetail CommonComponentDetail = null;

			// Check web component detail
			if (webComponentDetail != null)
			{
				try
				{
					// New component detail
					CommonComponentDetail = new Cic.P000001.Common.ComponentDetail(webComponentDetail.ComponentDetailTypeConstant, webComponentDetail.ComponentDetailValueTypeConstant, webComponentDetail.Category, webComponentDetail.Name, webComponentDetail.Value);
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonComponentDetail;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ComponentDetail[] ConvertComponentDetails(Cic.P000001.Common.ComponentDetail[] webComponentDetails)
		{
			Cic.P000001.Common.ComponentDetail[] CommonComponentDetails = null;
			int Index;

			// Check web component detail
			if (webComponentDetails != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.ComponentDetail>(ref CommonComponentDetails, webComponentDetails.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through component details
				foreach (Cic.P000001.Common.ComponentDetail LoopComponentDetail in webComponentDetails)
				{
					// Increase index
					Index++;
					try
					{
						// Add new component detail
                        CommonComponentDetails[Index] = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertComponentDetail(LoopComponentDetail);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return CommonComponentDetails;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode ConvertConfigurationTreeNode(Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationTreeNode webConfigurationTreeNode)
		{
			Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode CommonConfigurationTreeNode = null;

			// Check web configurationPackage tree node
			if (webConfigurationTreeNode != null)
			{
				try
				{
					// New configurationPackage tree node
                    CommonConfigurationTreeNode = new Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode(Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertTreeNode(webConfigurationTreeNode.TreeNode), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertTreeNodeDetails(webConfigurationTreeNode.TreeNodeDetails), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertPictures(webConfigurationTreeNode.Pictures));
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonConfigurationTreeNode;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ConfigurationManager.ConfigurationComponent ConvertConfigurationComponent(Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent webConfigurationComponent)
		{
			Cic.P000001.Common.ConfigurationManager.ConfigurationComponent CommonConfigurationComponent = null;

			// Check web configurationPackage component
			if (webConfigurationComponent != null)
			{
				try
				{
					// New configurationPackage component
                    CommonConfigurationComponent = new Cic.P000001.Common.ConfigurationManager.ConfigurationComponent(Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertComponent(webConfigurationComponent.Component), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertComponentDetails(webConfigurationComponent.ComponentDetails), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertPictures(webConfigurationComponent.Pictures));
				}
				catch
				{
					// Throw caught exception
					throw;
				}
			}

			// Return
			return CommonConfigurationComponent;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] ConvertConfigurationComponents(Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent[] webConfigurationComponents)
		{
			Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] CommonConfigurationComponents = null;
			int Index;

			// Check web configurationPackage component
			if (webConfigurationComponents != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.ConfigurationManager.ConfigurationComponent>(ref CommonConfigurationComponents, webConfigurationComponents.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through configurationPackage components
				foreach (Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent LoopConfigurationComponent in webConfigurationComponents)
				{
					// Increase index
					Index++;
					try
					{
						// Add new configurationPackage component
                        CommonConfigurationComponents[Index] = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertConfigurationComponent(LoopConfigurationComponent);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return CommonConfigurationComponents;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ConfigurationManager.ConfigurationPackage ConvertConfiguration(Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage webConfigurationPackage)
		{
			Cic.P000001.Common.ConfigurationManager.ConfigurationPackage CommonConfigurationPackage = null;

			// Check web configurationPackage
			if (webConfigurationPackage != null)
			{
				try
				{
					// New configurationPackage
                    CommonConfigurationPackage = new Cic.P000001.Common.ConfigurationManager.ConfigurationPackage(webConfigurationPackage.Identifier, webConfigurationPackage.UserCode, webConfigurationPackage.Designation, webConfigurationPackage.Description, webConfigurationPackage.CreatedAt, webConfigurationPackage.LastModifiedAt, webConfigurationPackage.IsPublic, webConfigurationPackage.IsLocked, webConfigurationPackage.GroupName, webConfigurationPackage.GroupDescription, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertCatalogItems(webConfigurationPackage.CatalogItems), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertDataSourceInformation(webConfigurationPackage.DataSourceInformation), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertSetting(webConfigurationPackage.Setting), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertConfigurationTreeNode(webConfigurationPackage.ConfigurationTreeNode), Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertConfigurationComponents(webConfigurationPackage.ConfigurationComponents));
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
