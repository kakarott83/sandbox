using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.MergeDataProvider
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Cic.P000001.Common.DataProvider;
    using Cic.P000001.Common;
    #endregion

    public class MergeDataProvider : Cic.P000001.Common.DataProvider.IAdapter
    {

        #region IAdapter methods
        public DataSourceInformation DeliverDataSourceInformation()
        {
            return MultipleAdaptersHelper.DeliverDataSourceInformation();
        }

        public TreeNode[] GetTreeNodes(Setting setting, TreeNode treeNode, GetTreeNodeSearchModeConstants getTreeNodeSearchModeConstant)
        {
            switch (getTreeNodeSearchModeConstant)
            {
                case GetTreeNodeSearchModeConstants.NextLevel:
                    return MultipleAdaptersHelper.GetNextLevel(setting, treeNode);

                case GetTreeNodeSearchModeConstants.PreviousLevel:
                case GetTreeNodeSearchModeConstants.PreviousLevels:
                    return MultipleAdaptersHelper.GetPreviousLevels(setting, treeNode, getTreeNodeSearchModeConstant);

                default:
                    throw new NotSupportedException();
            }

        }

        public TreeNode[] SearchTreeNodes(Setting setting, FilterParam[] filter, Search search)
        {
            return MultipleAdaptersHelper.SearchTreeNodes(setting, filter, search);
        }

        public TreeInfo GetTreeInfo(Setting setting, TreeNode treeNode)
        {
            // Create a dual adapter
            DualAdapter DualAdapter = new DualAdapter();

            return DualAdapter.PrimaryAdapter.GetTreeInfo(setting, treeNode);
        }

        public TreeNodeDetail[] GetTreeNodeDetails(Setting setting, TreeNode treeNode, TreeNodeDetailTypeConstants treeNodeDetailTypeConstant)
        {
            return MultipleAdaptersHelper.GetTreeNodeDetails(setting, treeNode, treeNodeDetailTypeConstant);
        }

        public Picture[] GetTreeNodePictures(Setting setting, TreeNode treeNode, PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            // Create a dual adapter
            DualAdapter DualAdapter = new DualAdapter();

            return DualAdapter.PrimaryAdapter.GetTreeNodePictures(setting, treeNode, pictureTypeConstant, top, withoutContent);
        }

        public Component[] GetComponents(Setting setting, TreeNode treeNode, ComponentTypeConstants componentTypeConstant)
        {
            return MultipleAdaptersHelper.GetComponents(setting, treeNode, componentTypeConstant);
        }

        public CheckComponentResult CheckComponent(Setting setting, TreeNode treeNode, Component component, Component[] components)
        {
            return MultipleAdaptersHelper.CheckComponent(setting, treeNode, component, components);
        }

        public ComponentDetail[] GetComponentDetails(Setting setting, TreeNode treeNode, Component component, ComponentDetailTypeConstants componentDetailTypeConstant)
        {
            return MultipleAdaptersHelper.GetComponentDetails(setting, treeNode, component, componentDetailTypeConstant);
        }

        public Picture[] GetComponentPictures(Setting setting, Component component, PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            return MultipleAdaptersHelper.GetComponentPictures(setting, component, pictureTypeConstant, top, withoutContent);
        }

        public AdapterState DeliverAdapterState()
        {
            return MultipleAdaptersHelper.DeliverAdapterState();
        }
        #endregion
    }
}
