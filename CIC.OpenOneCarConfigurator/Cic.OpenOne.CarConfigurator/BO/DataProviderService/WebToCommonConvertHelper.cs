// OWNER: BK, 10-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
	//[System.CLSCompliant(true)]
	internal static class DataProviderWebToCommonConvertHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.DataSourceInformation ConvertDataSourceInformation(Cic.P000001.Common.DataSourceInformation webDataSourceInformation)
		{
			Cic.P000001.Common.DataSourceInformation CommonDataSourceInformation = null;

			// Check web data source information
			if (webDataSourceInformation != null)
			{
				try
				{
					// New level
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
					CommonSetting = new Cic.P000001.Common.Setting(webSetting);
                    
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
					// New level
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
						CommonParameters[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderWebToCommonConvertHelper.ConvertParameter(LoopParameter);
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
					CommonTreeNode = new Cic.P000001.Common.TreeNode(webTreeNode.Key, webTreeNode.ParentKey, Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderWebToCommonConvertHelper.ConvertLevel(webTreeNode.Level), webTreeNode.DisplayName, Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderWebToCommonConvertHelper.ConvertParameters(webTreeNode.Parameters), webTreeNode.IsType, webTreeNode.HasDetails, webTreeNode.HasPictures, webTreeNode.Price, webTreeNode.NewPrice);
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
					CommonComponent = new Cic.P000001.Common.Component(webComponent.Key, webComponent.ComponentTypeConstant, webComponent.Category, webComponent.DisplayName, Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderWebToCommonConvertHelper.ConvertParameters(webComponent.Parameters), webComponent.HasDetails, webComponent.HasPictures, webComponent.Price, webComponent.NewPrice, Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderWebToCommonConvertHelper.ConvertComponents(webComponent.Components), webComponent.Selectable);
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
						CommonComponents[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderWebToCommonConvertHelper.ConvertComponent(LoopComponent);
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

		//// TODO BK 0 BK, Not tested
		//internal static Cic.P000001.Common.TreeNodeDetail[] ConvertTreeNodeDetails(Cic.P000001.Common.TreeNodeDetail[] webTreeNodeDetails)
		//{
		//    Cic.P000001.Common.TreeNodeDetail[] CommonTreeNodeDetails = null;
		//    int Index;

		//    // Check web tree node detail
		//    if (webTreeNodeDetails != null)
		//    {
		//        // Resize
		//        System.Array.Resize<Cic.P000001.Common.TreeNodeDetail>(ref CommonTreeNodeDetails, webTreeNodeDetails.GetLength(0));
		//        // Reset index
		//        Index = -1;
		//        // Loop through tree node details
		//        foreach (Cic.P000001.Common.TreeNodeDetail LoopTreeNodeDetail in webTreeNodeDetails)
		//        {
		//            // Increase index
		//            Index++;
		//            try
		//            {
		//                // Add new tree node detail
		//                CommonTreeNodeDetails[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderWebToCommonConvertHelper.ConvertTreeNodeDetail(LoopTreeNodeDetail);
		//            }
		//            catch
		//            {
		//                // Throw caught exception
		//                throw;
		//            }
		//        }

		//    }

		//    // Return
		//    return CommonTreeNodeDetails;
		//}

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

        internal static Cic.P000001.Common.SimpleSearchParam ConvertSimpleSearchParam(Cic.P000001.Common.SimpleSearchParam webSimpleSearchParam)
        {
            Cic.P000001.Common.SimpleSearchParam CommonSimpleSearchParam = null;

            // Check web search param
            if (webSimpleSearchParam != null)
            {
                try
                {
                    CommonSimpleSearchParam = new Cic.P000001.Common.SimpleSearchParam();
                    CommonSimpleSearchParam.SearchBy = (Cic.P000001.Common.SearchBy)webSimpleSearchParam.SearchBy;
                    CommonSimpleSearchParam.SearchPattern = webSimpleSearchParam.SearchPattern;
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }
            }

            // Return
            return CommonSimpleSearchParam;
        }

        internal static Cic.P000001.Common.Search ConvertSearch(Cic.P000001.Common.Search webSearch)
        {
            Cic.P000001.Common.Search CommonSearch = null;

            // Check web search param
            if (webSearch != null)
            {
                try
                {
                    CommonSearch = new Cic.P000001.Common.Search();

                    // Convert the parameters
                    CommonSearch.SearchParams = ConvertSearchParams(webSearch.SearchParams);
                    CommonSearch.SimpleSearch = webSearch.SimpleSearch;
                    CommonSearch.SimpleSearchParam = ConvertSimpleSearchParam(webSearch.SimpleSearchParam);
                    
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }
            }

            // Return
            return CommonSearch;
        }

        internal static Cic.P000001.Common.SearchParam ConvertSearchParam(Cic.P000001.Common.SearchParam webSearchParam)
        {
            Cic.P000001.Common.SearchParam CommonSearchParam = null;

            // Check web search param
            if (webSearchParam != null)
            {
                try
                {
                    // New search param
                    CommonSearchParam = new Cic.P000001.Common.SearchParam(DataProviderWebToCommonConvertHelper.ConvertLevel(webSearchParam.SearchAtLevel), webSearchParam.Pattern);
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }
            }

            // Return
            return CommonSearchParam;
        }

        internal static Cic.P000001.Common.SearchParam[] ConvertSearchParams(Cic.P000001.Common.SearchParam[] webSearchParams)
        {
            Cic.P000001.Common.SearchParam[] CommonSearchParams = null;

            // Check web search param
            if (webSearchParams != null)
            {
                try
                {
                    CommonSearchParams = new Cic.P000001.Common.SearchParam[webSearchParams.Length];

                    for(int ParamIndex=0; ParamIndex<webSearchParams.Length; ParamIndex++)
                        CommonSearchParams[ParamIndex] = new Cic.P000001.Common.SearchParam(DataProviderWebToCommonConvertHelper.ConvertLevel(webSearchParams[ParamIndex].SearchAtLevel), webSearchParams[ParamIndex].Pattern);
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }
            }

            // Return
            return CommonSearchParams;
        }

        internal static Cic.P000001.Common.FilterParam ConvertFilterParam(Cic.P000001.Common.FilterParam webFilterParam)
        {
            Cic.P000001.Common.FilterParam CommonFilterParam = null;

            // Check web search param
            if (webFilterParam != null)
            {
                try
                {
                    // New search param
                    CommonFilterParam = new Cic.P000001.Common.FilterParam(DataProviderWebToCommonConvertHelper.ConvertLevel(webFilterParam.FilterAtLevel), webFilterParam.Filter);
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }
            }

            // Return
            return CommonFilterParam;
        }

        internal static Cic.P000001.Common.FilterParam[] ConvertFilterParams(Cic.P000001.Common.FilterParam[] webFilterParams)
        {
            Cic.P000001.Common.FilterParam[] CommonFilterParams = null;

            // Check web search param
            if (webFilterParams != null)
            {
                try
                {
                    CommonFilterParams = new Cic.P000001.Common.FilterParam[webFilterParams.Length];

                    for(int ParamIndex=0; ParamIndex<webFilterParams.Length; ParamIndex++)
                        CommonFilterParams[ParamIndex] = new Cic.P000001.Common.FilterParam(DataProviderWebToCommonConvertHelper.ConvertLevel(webFilterParams[ParamIndex].FilterAtLevel), webFilterParams[ParamIndex].Filter);
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }
            }

            // Return
            return CommonFilterParams;
        }

		//// TODO BK 0 BK, Not tested
		//internal static Cic.P000001.Common.ComponentDetail[] ConvertComponentDetails(Cic.P000001.Common.ComponentDetail[] webComponentDetails)
		//{
		//    Cic.P000001.Common.ComponentDetail[] CommonComponentDetails = null;
		//    int Index;

		//    // Check web component detail
		//    if (webComponentDetails != null)
		//    {
		//        // Resize
		//        System.Array.Resize<Cic.P000001.Common.ComponentDetail>(ref CommonComponentDetails, webComponentDetails.GetLength(0));
		//        // Reset index
		//        Index = -1;
		//        // Loop through component details
		//        foreach (Cic.P000001.Common.ComponentDetail LoopComponentDetail in webComponentDetails)
		//        {
		//            // Increase index
		//            Index++;
		//            try
		//            {
		//                // Add new component detail
		//                CommonComponentDetails[Index] = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderWebToCommonConvertHelper.ConvertComponentDetail(LoopComponentDetail);
		//            }
		//            catch
		//            {
		//                // Throw caught exception
		//                throw;
		//            }
		//        }

		//    }

		//    // Return
		//    return CommonComponentDetails;
		//}
		#endregion
	}
}
