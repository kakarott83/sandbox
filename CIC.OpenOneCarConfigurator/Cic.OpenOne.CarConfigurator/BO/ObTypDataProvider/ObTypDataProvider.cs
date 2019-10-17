using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.ObTypDataProvider
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Cic.P000001.Common;
    using Cic.P000001.Common.DataProvider;
    using Cic.OpenOne.Common.Model.DdOl;
   
    #endregion

    public class ObTypDataProvider : Cic.P000001.Common.DataProvider.IAdapter
    {

        #region IAdapter Methods
        public DataSourceInformation DeliverDataSourceInformation()
        {
            // Create the result
            DataSourceInformation DataSourceInfo = new DataSourceInformation();

            // Fill DataSourceInformation
            DataSourceInfo.Identifier = new Guid("{c8be561b-e3eb-4a10-b3f6-a5b0f42da426}");
            DataSourceInfo.Version = "1.0";
            DataSourceInfo.VersionDescription = "Cic.P000001.ObTypDataProvider";
            DataSourceInfo.Description = null;
            DataSourceInfo.Designation = "ObTyp";
            DataSourceInfo.Copyright = "Copyright © C.I.C. Software GmbH 2010";
            DataSourceInfo.PriceInclusionConstant = PriceInclusionConstants.None;
            DataSourceInfo.ExteriorPicturesSupported = true;
            DataSourceInfo.InteriorPicturesSupported = true;
            DataSourceInfo.StandardEquipmentDetailsSupported = true;
            DataSourceInfo.TechnicalDataDetailsSupported = true;
            DataSourceInfo.ValueAddedTaxInfo = null;
            DataSourceInfo.SpecialCarTaxInfo = null;
            DataSourceInfo.ShippingCostInfo = null;
            DataSourceInfo.ImportDutyInfo = null;
            DataSourceInfo.PriceInfo = null;
            DataSourceInfo.AvailableCurrencies = new string[] { "EUR" };
            DataSourceInfo.AvailableLanguages = new string[] { "de-AT" };

            // Return the information
            return DataSourceInfo;
        }

        public TreeNode[] GetTreeNodes(Setting setting, TreeNode treeNode, GetTreeNodeSearchModeConstants getTreeNodeSearchModeConstant)
        {
            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {
                // Check for search mode
                switch (getTreeNodeSearchModeConstant)
                {
                    // Get the next level
                    case GetTreeNodeSearchModeConstants.NextLevel:
                        return TreeNodeHelper.GetNextLevel(treeNode, Context, setting);

                    // Get the previous level
                    case GetTreeNodeSearchModeConstants.PreviousLevel:
                        return new TreeNode[] { TreeNodeHelper.GetPreviousLevel(treeNode, Context, setting.SelectedLanguage) };

                    // Get the previous levels
                    case GetTreeNodeSearchModeConstants.PreviousLevels:
                        return TreeNodeHelper.GetPreviousLevels(treeNode, Context, setting.SelectedLanguage);

                    // Any other options are not supported
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public TreeNode[] SearchTreeNodes(Setting setting, FilterParam[] filter, Search search)
        {
            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {
                // Search the tree nodes
                return TreeNodeHelper.SearchTreeNodes(filter, search, Context, setting);
            }
        }

        public TreeInfo GetTreeInfo(Setting setting, TreeNode treeNode)
        {
            return TreeNodeHelper.GetTreeInfo(treeNode,setting.SelectedLanguage);
        }

        public TreeNodeDetail[] GetTreeNodeDetails(Setting setting, TreeNode treeNode, TreeNodeDetailTypeConstants treeNodeDetailTypeConstant)
        {
            using (DdOlExtended Context = new DdOlExtended())
            {
                // Check what type of details is needed
                switch (treeNodeDetailTypeConstant)
                {
                    // Technical details
                    case TreeNodeDetailTypeConstants.Technic:
                        return TreeNodeHelper.GetTechnicalDetails(treeNode, Context, setting.SelectedLanguage);

                    // Other type of details
                    default:
                        return new TreeNodeDetail[0];
                }
            }
        }

        public Picture[] GetTreeNodePictures(Setting setting, TreeNode treeNode, PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            throw new NotImplementedException();
        }

        public Component[] GetComponents(Setting setting, TreeNode treeNode, ComponentTypeConstants componentTypeConstant)
        {
            return new Component[0];
        }

        public CheckComponentResult CheckComponent(Setting setting, TreeNode treeNode, Component component, Component[] components)
        {
            throw new NotImplementedException();
        }

        public ComponentDetail[] GetComponentDetails(Setting setting, TreeNode treeNode, Component component, ComponentDetailTypeConstants componentDetailTypeConstant)
        {
            throw new NotImplementedException();
        }

        public Picture[] GetComponentPictures(Setting setting, Component component, PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            throw new NotImplementedException();
        }

        public AdapterState DeliverAdapterState()
        {
            // Assume the message is not needed
            string Message = null;

            // Assume the service is running
            bool IsServiceable = true;

            try
            {
                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    // Try to open the connection
                    Context.Connection.Open();
                }
            }
            catch (Exception exception)
            {
                // Copy the message
                Message = exception.Message;

                // Unfortunately, the service is not running
                IsServiceable = false;
            }

            return new AdapterState("ObTypDataProvider", IsServiceable, Message);
        }
        #endregion
    }
}