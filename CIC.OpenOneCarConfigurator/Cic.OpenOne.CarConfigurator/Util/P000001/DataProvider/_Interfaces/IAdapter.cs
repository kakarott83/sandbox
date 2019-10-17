// OWNER: BK, 09-04-2008
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public interface IAdapter : Cic.OpenOne.Util.Reflection.IAdapter
	{
		#region Methods
		//// TESTEDBY AdapterInterfaceTestFixture.CheckDeliverAdapterState
		//Cic.P000001.Common.AdapterState DeliverAdapterState();

		// TESTEDBY AdapterInterfaceTestFixture.CheckDeliverDataSourceInformation
		Cic.P000001.Common.DataSourceInformation DeliverDataSourceInformation();

		// TESTEDBY AdapterInterfaceTestFixture.CheckGetTreeNodes
		Cic.P000001.Common.TreeNode[] GetTreeNodes(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.GetTreeNodeSearchModeConstants getTreeNodeSearchModeConstant);

        Cic.P000001.Common.TreeNode[] SearchTreeNodes(Setting setting, FilterParam[] filter, Search search);

		// TESTEDBY AdapterInterfaceTestFixture.CheckGetTreeInfo
		Cic.P000001.Common.DataProvider.TreeInfo GetTreeInfo(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode);

		// TESTEDBY AdapterInterfaceTestFixture.CheckGetTreeNodeDetails
		Cic.P000001.Common.TreeNodeDetail[] GetTreeNodeDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.TreeNodeDetailTypeConstants treeNodeDetailTypeConstant);

		// TESTEDBY AdapterInterfaceTestFixture.CheckGetTreeNodePictures
		Cic.P000001.Common.Picture[] GetTreeNodePictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent);

		// TESTEDBY AdapterInterfaceTestFixture.CheckGetComponents
		Cic.P000001.Common.Component[] GetComponents(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.ComponentTypeConstants componentTypeConstant);

		// TESTEDBY AdapterInterfaceTestFixture.CheckCheckComponent
		Cic.P000001.Common.DataProvider.CheckComponentResult CheckComponent(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.Component[] components);

		// TESTEDBY AdapterInterfaceTestFixture.CheckGetComponentDetails
		Cic.P000001.Common.ComponentDetail[] GetComponentDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.ComponentDetailTypeConstants componentDetailTypeConstant);

		// TESTEDBY AdapterInterfaceTestFixture.CheckGetComponentPictures
		Cic.P000001.Common.Picture[] GetComponentPictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.Component component, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent);
		#endregion
	}
}
