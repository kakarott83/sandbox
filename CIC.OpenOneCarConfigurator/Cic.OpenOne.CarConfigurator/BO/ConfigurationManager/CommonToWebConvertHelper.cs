// OWNER: BK, 10-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class ConfigurationManagerCommonToWebConvertHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem[] ConvertCatalogItems(Cic.P000001.Common.ConfigurationManager.CatalogItem[] commonCatalogItems)
		{
			Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem[] WebCatalogItems = null;
			int Index;

			// Check common catalog items
			if (commonCatalogItems != null)
			{
				// Resize
				System.Array.Resize<Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem>(ref WebCatalogItems, commonCatalogItems.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through catalog items
				foreach (Cic.P000001.Common.ConfigurationManager.CatalogItem LoopCatalogItem in commonCatalogItems)
				{
					// Increase index
					Index++;
					try
					{
						// Add new catalog item
						WebCatalogItems[Index] = new Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem(LoopCatalogItem);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebCatalogItems;
		}

		//// TODO BK 0 BK, Not tested
		//internal static Cic.P000001.Common.Level[] ConvertLevels(Cic.P000001.Common.Level[] commonLevels)
		//{
		//    Cic.P000001.Common.Level[] WebLevels = null;
		//    int Index;

		//    // Check common level
		//    if (commonLevels != null)
		//    {
		//        // Resize
		//        System.Array.Resize<Cic.P000001.Common.Level>(ref WebLevels, commonLevels.GetLength(0));
		//        // Reset index
		//        Index = -1;
		//        // Loop through levels
		//        foreach (Cic.P000001.Common.Level LoopLevel in commonLevels)
		//        {
		//            // Increase index
		//            Index++;
		//            try
		//            {
		//                // Add new level
		//                WebLevels[Index] = new Cic.P000001.Common.Level(LoopLevel);
		//            }
		//            catch
		//            {
		//                // Throw caught exception
		//                throw;
		//            }
		//        }

		//    }

		//    // Return
		//    return WebLevels;
		//}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Parameter[] ConvertParameters(Cic.P000001.Common.Parameter[] commonParameters)
		{
			Cic.P000001.Common.Parameter[] WebParameters = null;
			int Index;

			// Check common parameters
			if (commonParameters != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Parameter>(ref WebParameters, commonParameters.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through parameters
				foreach (Cic.P000001.Common.Parameter LoopParameter in commonParameters)
				{
					// Increase index
					Index++;
					try
					{
						// Add new parameter
						WebParameters[Index] = LoopParameter;
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebParameters;
		}

		//// TODO BK 0 BK, Not tested
		//internal static Cic.P000001.Common.TreeNode[] ConvertTreeNodes(Cic.P000001.Common.TreeNode[] commonTreeNodes)
		//{
		//    Cic.P000001.Common.TreeNode[] WebTreeNodes = null;
		//    int Index;

		//    // Check common tree nodes
		//    if (commonTreeNodes != null)
		//    {
		//        // Resize
		//        System.Array.Resize<Cic.P000001.Common.TreeNode>(ref WebTreeNodes, commonTreeNodes.GetLength(0));
		//        // Reset index
		//        Index = -1;
		//        // Loop through tree nodes
		//        foreach (Cic.P000001.Common.TreeNode LoopTreeNode in commonTreeNodes)
		//        {
		//            // Increase index
		//            Index++;
		//            try
		//            {
		//                // Add new tree node
		//                WebTreeNodes[Index] = new Cic.P000001.Common.TreeNode(LoopTreeNode);
		//            }
		//            catch
		//            {
		//                // Throw caught exception
		//                throw;
		//            }
		//        }

		//    }

		//    // Return
		//    return WebTreeNodes;
		//}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Picture[] ConvertPictures(Cic.P000001.Common.Picture[] commonPictures)
		{
			Cic.P000001.Common.Picture[] WebPictures = null;
			int Index;

			// Check common pictures
			if (commonPictures != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Picture>(ref WebPictures, commonPictures.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through pictures
				foreach (Cic.P000001.Common.Picture LoopPicture in commonPictures)
				{
					// Increase index
					Index++;
					try
					{
						// Add new picture
						WebPictures[Index] = LoopPicture;
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebPictures;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.Component[] ConvertComponents(Cic.P000001.Common.Component[] commonComponents)
		{
			Cic.P000001.Common.Component[] WebComponents = null;
			int Index;

			// Check common components
			if (commonComponents != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.Component>(ref WebComponents, commonComponents.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through components
				foreach (Cic.P000001.Common.Component LoopComponent in commonComponents)
				{
					// Increase index
					Index++;
					try
					{
						// Add new component
						WebComponents[Index] = LoopComponent;
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebComponents;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.TreeNodeDetail[] ConvertTreeNodeDetails(Cic.P000001.Common.TreeNodeDetail[] commonTreeNodeDetails)
		{
			Cic.P000001.Common.TreeNodeDetail[] WebTreeNodeDetails = null;
			int Index;

			// Check common tree node details
			if (commonTreeNodeDetails != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.TreeNodeDetail>(ref WebTreeNodeDetails, commonTreeNodeDetails.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through tree node details
				foreach (Cic.P000001.Common.TreeNodeDetail LoopTreeNodeDetail in commonTreeNodeDetails)
				{
					// Increase index
					Index++;
					try
					{
						// Add new tree node detail
						WebTreeNodeDetails[Index] = LoopTreeNodeDetail;
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebTreeNodeDetails;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ComponentDetail[] ConvertComponentDetails(Cic.P000001.Common.ComponentDetail[] commonComponentDetails)
		{
			Cic.P000001.Common.ComponentDetail[] WebComponentDetails = null;
			int Index;

			// Check common component details
			if (commonComponentDetails != null)
			{
				// Resize
				System.Array.Resize<Cic.P000001.Common.ComponentDetail>(ref WebComponentDetails, commonComponentDetails.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through component details
				foreach (Cic.P000001.Common.ComponentDetail LoopComponentDetail in commonComponentDetails)
				{
					// Increase index
					Index++;
					try
					{
						// Add new component detail
						WebComponentDetails[Index] = LoopComponentDetail;
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebComponentDetails;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent[] ConvertConfigurationComponents(Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] commonConfigurationComponents)
		{
			Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent[] WebConfigurationComponents = null;
			int Index;

			// Check common configurationPackage components
			if (commonConfigurationComponents != null)
			{
				// Resize
				System.Array.Resize<Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent>(ref WebConfigurationComponents, commonConfigurationComponents.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through configurationPackage components
				foreach (Cic.P000001.Common.ConfigurationManager.ConfigurationComponent LoopConfigurationComponent in commonConfigurationComponents)
				{
					// Increase index
					Index++;
					try
					{
						// Add new configurationPackage component
						WebConfigurationComponents[Index] = new Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent(LoopConfigurationComponent);
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebConfigurationComponents;
		}

		// TODO BK 0 BK, Not tested
		internal static Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage[] ConvertConfigurations(Cic.P000001.Common.ConfigurationManager.ConfigurationPackage[] commonConfigurations)
		{
			Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage[] WebConfigurations = null;
			int Index;

			// Check configurations
			if (commonConfigurations != null)
			{
				// Resize
				System.Array.Resize<Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage>(ref WebConfigurations, commonConfigurations.GetLength(0));
				// Reset index
				Index = -1;
				// Loop through configurations
				foreach (Cic.P000001.Common.ConfigurationManager.ConfigurationPackage LoopConfiguration in commonConfigurations)
				{
					// Increase index
					Index++;
					try
					{
						// Add new configurationPackage
						// TODO BK 0 BK, Check null for tree node and components
						WebConfigurations[Index] = new Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage(LoopConfiguration); ;
					}
					catch
					{
						// Throw caught exception
						throw;
					}
				}

			}

			// Return
			return WebConfigurations;
		}
		#endregion
	}
}
