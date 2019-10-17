using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.MergeDataProvider
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Cic.P000001.Common;
    using Cic.P000001.Common.DataProvider;
    #endregion

    internal static class MultipleAdaptersHelper
    {
        #region Private constants
        private const string CnstParameterCategory = "MergeDataProvider";
        private const string CnstAdapterTypeParameter = "AdapterType";
        private const string CnstPrimaryNodeKeyParameter = "PrimaryNodeKey";
        private const string CnstSecondaryNodeKeyParameter = "SecondaryNodeKey";
        private const string CnstPrimaryNodeParentKeyParameter = "PrimaryNodeParentKey";
        private const string CnstSecondaryNodeParentKeyParameter = "SecondaryNodeParentKey";
        #endregion


        private static IDualAdapter getAdapter()
        {
            //TODO make configurable
            return new EmbeddedDualAdapter();
            //return new DualAdapter();
        }
        #region Methods
        public static AdapterState DeliverAdapterState()
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            // Get primary adapter state
            AdapterState PrimaryAdapterState = DualAdapter.PrimaryAdapter.DeliverAdapterState();

            // Get secondary adapter state
            AdapterState SecondaryAdapterState = DualAdapter.SecondaryAdapter.DeliverAdapterState();

            // Assume the message is empty
            string Message = null;

            if (!PrimaryAdapterState.IsServiceable)
            {
                Message = PrimaryAdapterState.AdapterName + ": " + PrimaryAdapterState.Message;
            }

            if (!SecondaryAdapterState.IsServiceable)
            {
                if (string.IsNullOrEmpty(Message))
                {
                    Message = string.Empty;
                }
                else
                {
                    Message += "\n";
                }

                Message += SecondaryAdapterState.AdapterName + ": " + SecondaryAdapterState.Message;
            }

            // Get the adapter name
            string AdapterName = "MergeDataProvider (" + PrimaryAdapterState.AdapterName + ", " + SecondaryAdapterState.AdapterName + ")";

            // Combine IsServiceable
            bool IsServiceable = PrimaryAdapterState.IsServiceable && SecondaryAdapterState.IsServiceable;

            // Return a combined adapter state
            return new AdapterState(AdapterName, IsServiceable, Message);
        }

        public static DataSourceInformation DeliverDataSourceInformation()
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            // Get primary adapter data source information
            DataSourceInformation PrimaryAdapterSourceInformation = DualAdapter.PrimaryAdapter.DeliverDataSourceInformation();

            // Get secondary adapter data source application
            DataSourceInformation SecondaryAdapterSourceInformation = DualAdapter.SecondaryAdapter.DeliverDataSourceInformation();

            // Create the result
            DataSourceInformation DataSourceInfo = new DataSourceInformation();

            // Fill DataSourceInformation
            DataSourceInfo.Identifier = new Guid("{d7a4cd75-c3a4-480b-aa54-81bc20d6e16c}");
            DataSourceInfo.Version = "1.0";
            DataSourceInfo.VersionDescription = "Cic.P000001.MergeDataProvider";
            DataSourceInfo.Description = null;
            DataSourceInfo.Designation = "Merge (" + PrimaryAdapterSourceInformation.Designation + ", " + SecondaryAdapterSourceInformation.Designation + ")";
            DataSourceInfo.Copyright = "Copyright © C.I.C. Software GmbH 2010";
            DataSourceInfo.PriceInclusionConstant = PrimaryAdapterSourceInformation.PriceInclusionConstant;
            DataSourceInfo.ExteriorPicturesSupported = PrimaryAdapterSourceInformation.ExteriorPicturesSupported;
            DataSourceInfo.InteriorPicturesSupported = PrimaryAdapterSourceInformation.InteriorPicturesSupported;
            DataSourceInfo.StandardEquipmentDetailsSupported = PrimaryAdapterSourceInformation.StandardEquipmentDetailsSupported;
            DataSourceInfo.TechnicalDataDetailsSupported = PrimaryAdapterSourceInformation.TechnicalDataDetailsSupported;
            DataSourceInfo.ValueAddedTaxInfo = PrimaryAdapterSourceInformation.ValueAddedTaxInfo;
            DataSourceInfo.SpecialCarTaxInfo = PrimaryAdapterSourceInformation.SpecialCarTaxInfo;
            DataSourceInfo.ShippingCostInfo = PrimaryAdapterSourceInformation.ShippingCostInfo;
            DataSourceInfo.ImportDutyInfo = PrimaryAdapterSourceInformation.ImportDutyInfo;
            DataSourceInfo.PriceInfo = PrimaryAdapterSourceInformation.PriceInfo;
            DataSourceInfo.AvailableCurrencies = PrimaryAdapterSourceInformation.AvailableCurrencies;
            DataSourceInfo.AvailableLanguages = PrimaryAdapterSourceInformation.AvailableLanguages;

            // Return the information
            return DataSourceInfo;
        }

        public static TreeNode[] GetNextLevel(Setting setting, TreeNode treeNode)
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            TreeNode[] PrimaryAdapterNodes = new TreeNode[0];
            TreeNode[] SecondaryAdapterNodes = new TreeNode[0];

            // Check if tree node is null
            if (treeNode == null)
            {
                PrimaryAdapterNodes = DualAdapter.PrimaryAdapter.GetTreeNodes(setting, null, GetTreeNodeSearchModeConstants.NextLevel);
                SecondaryAdapterNodes = DualAdapter.SecondaryAdapter.GetTreeNodes(setting, null, GetTreeNodeSearchModeConstants.NextLevel);
            }
            else
            {
                // Parse the keys
                DualKey Key = new DualKey(treeNode.Key);
                DualKey ParentKey = new DualKey(string.IsNullOrEmpty(treeNode.ParentKey) ? null : treeNode.ParentKey);

                // Get temporary nodes
                TreeNode TempPrimaryNode = MyGetTempTreeNode(treeNode, Key.PrimaryKey, ParentKey.PrimaryKey);
                TreeNode TempSecondaryNode = MyGetTempTreeNode(treeNode, Key.SecondaryKey, ParentKey.SecondaryKey);

                // Get primary adapter nodes
                if (TempPrimaryNode != null)
                {
                    PrimaryAdapterNodes = DualAdapter.PrimaryAdapter.GetTreeNodes(setting, TempPrimaryNode, GetTreeNodeSearchModeConstants.NextLevel);
                }

                // Get secondary adapter nodes
                if (TempSecondaryNode != null)
                {
                    SecondaryAdapterNodes = DualAdapter.SecondaryAdapter.GetTreeNodes(setting, TempSecondaryNode, GetTreeNodeSearchModeConstants.NextLevel);
                }
            }

            // Merge the nodes
            TreeNode[] Nodes = MyMergeTreeNodes(PrimaryAdapterNodes, SecondaryAdapterNodes);

            foreach (var LoopNode in Nodes)
            {
                LoopNode.ParentKey = treeNode == null ? null : treeNode.Key;
            }

            // Return the nodes
            return Nodes;
        }

        public static TreeNode[] GetPreviousLevels(Setting setting, TreeNode treeNode, GetTreeNodeSearchModeConstants getTreeNodeSearchModeConstant)
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            DualKey Key = new DualKey(treeNode.Key);
            DualKey ParentKey = new DualKey(string.IsNullOrEmpty(treeNode.ParentKey) ? null : treeNode.ParentKey);

            TreeNode[] PrimaryNodes = new TreeNode[0];
            TreeNode[] SecondaryNodes = new TreeNode[0];

            // Get temporary nodes
            TreeNode TempPrimaryNode = MyGetTempTreeNode(treeNode, Key.PrimaryKey, ParentKey.PrimaryKey);
            TreeNode TempSecondaryNode = MyGetTempTreeNode(treeNode, Key.SecondaryKey, ParentKey.SecondaryKey);


            if (TempPrimaryNode != null)
            {
                PrimaryNodes = DualAdapter.PrimaryAdapter.GetTreeNodes(setting, TempPrimaryNode, getTreeNodeSearchModeConstant);

                // If there is no OBTYP key
                if (TempSecondaryNode == null)
                {
                    // Order the nodes by level
                    var OrderedNodes = (from Node in PrimaryNodes
                                        orderby Node.Level.Number descending
                                        select Node).ToArray();


                    // Iterate thorugh all ordered nodes
                    foreach (var LoopNode in OrderedNodes)
                    {
                        // Search in the primary adapter
                        Search Search = new Search();
                        Search.SimpleSearch = true;
                        Search.SimpleSearchParam = new SimpleSearchParam();
                        Search.SimpleSearchParam.SearchBy = MyGetSerchByFromLevelNumber(LoopNode.Level.Number);
                        Search.SimpleSearchParam.SearchPattern = LoopNode.DisplayName;

                        TreeNode[] FoundNodes = DualAdapter.SecondaryAdapter.SearchTreeNodes(setting, null, Search);

                        // If anything was found
                        if (FoundNodes.Length > 0)
                        {
                            List<TreeNode> TempNodes = new List<TreeNode>();
                            TempNodes.AddRange(FoundNodes);
                            TempNodes.AddRange(DualAdapter.SecondaryAdapter.GetTreeNodes(setting, TempNodes[0], GetTreeNodeSearchModeConstants.PreviousLevels));
                            SecondaryNodes = TempNodes.ToArray();

                            break;
                        }

                    }
                }
            }

            if (TempSecondaryNode != null)
            {
                SecondaryNodes = DualAdapter.SecondaryAdapter.GetTreeNodes(setting, TempSecondaryNode, getTreeNodeSearchModeConstant);

                // If there is no eurotax key
                if (TempPrimaryNode == null)
                {
                    // Order the nodes by level
                    var OrderedNodes = (from Node in SecondaryNodes
                                       orderby Node.Level.Number descending
                                       select Node).ToArray();


                    // Iterate thorugh all ordered nodes
                    foreach (var LoopNode in OrderedNodes)
                    {
                        // Search in the primary adapter
                        Search Search = new Search();
                        Search.SimpleSearch = true;
                        Search.SimpleSearchParam = new SimpleSearchParam();
                        Search.SimpleSearchParam.SearchBy = MyGetSerchByFromLevelNumber(LoopNode.Level.Number);
                        Search.SimpleSearchParam.SearchPattern = LoopNode.DisplayName;

                        TreeNode[] FoundNodes = DualAdapter.PrimaryAdapter.SearchTreeNodes(setting, null, Search);

                        // If anything was found
                        if (FoundNodes.Length > 0)
                        {
                            List<TreeNode> TempNodes = new List<TreeNode>();
                            TempNodes.AddRange(FoundNodes);
                            TempNodes.AddRange(DualAdapter.PrimaryAdapter.GetTreeNodes(setting, TempNodes[0], GetTreeNodeSearchModeConstants.PreviousLevels));
                            PrimaryNodes = TempNodes.ToArray();
                            break;
                        }

                    }
                }
            }

            TreeNode[] Nodes = MyMergeTreeNodes(PrimaryNodes, SecondaryNodes);
            Nodes = Nodes.OrderByDescending(Node => Node.Level.Number).ToArray();
            MyUpdateParent(Nodes);
            return Nodes;
        }

        public static TreeNode[] SearchTreeNodes(Setting setting, FilterParam[] filter, Search search)
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            TreeNode[] PrimaryAdapterNodes = DualAdapter.PrimaryAdapter.SearchTreeNodes(setting, filter, search);

            TreeNode[] SecondaryAdapterNodes = DualAdapter.SecondaryAdapter.SearchTreeNodes(setting, filter, search);

            // Merge the nodes
            TreeNode[] Nodes = MyMergeTreeNodes(PrimaryAdapterNodes, SecondaryAdapterNodes);

            // Return the nodes
            return Nodes;
        }

        public static TreeNodeDetail[] GetTreeNodeDetails(Setting setting, TreeNode treeNode, TreeNodeDetailTypeConstants treeNodeDetailTypeConstant)
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            DualKey Key = new DualKey(treeNode.Key);
            DualKey ParentKey = new DualKey(string.IsNullOrEmpty(treeNode.ParentKey) ? null : treeNode.ParentKey);

            if (!string.IsNullOrEmpty(Key.PrimaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.PrimaryKey, ParentKey.PrimaryKey);
                return DualAdapter.PrimaryAdapter.GetTreeNodeDetails(setting, TempNode, treeNodeDetailTypeConstant);
            }
            else if (!string.IsNullOrEmpty(Key.SecondaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.SecondaryKey, ParentKey.SecondaryKey);
                return DualAdapter.SecondaryAdapter.GetTreeNodeDetails(setting, TempNode, treeNodeDetailTypeConstant);
            }

            return new TreeNodeDetail[0];
        }

        public static Component[] GetComponents(Setting setting, TreeNode treeNode, ComponentTypeConstants componentTypeConstant)
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            DualKey Key = new DualKey(treeNode.Key);
            DualKey ParentKey = new DualKey(string.IsNullOrEmpty(treeNode.ParentKey) ? null : treeNode.ParentKey);

            if (!string.IsNullOrEmpty(Key.PrimaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.PrimaryKey, ParentKey.PrimaryKey);
                return DualAdapter.PrimaryAdapter.GetComponents(setting, TempNode, componentTypeConstant);
            }
            else if (!string.IsNullOrEmpty(Key.SecondaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.SecondaryKey, ParentKey.SecondaryKey);
                return DualAdapter.SecondaryAdapter.GetComponents(setting, TempNode, componentTypeConstant);
            }

            return new Component[0];
        }

        public static CheckComponentResult CheckComponent(Setting setting, TreeNode treeNode, Component component, Component[] components)
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            DualKey Key = new DualKey(treeNode.Key);
            DualKey ParentKey = new DualKey(string.IsNullOrEmpty(treeNode.ParentKey) ? null : treeNode.ParentKey);

            if (!string.IsNullOrEmpty(Key.PrimaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.PrimaryKey, ParentKey.PrimaryKey);
                return DualAdapter.PrimaryAdapter.CheckComponent(setting, TempNode, component, components);
            }
            else if (!string.IsNullOrEmpty(Key.SecondaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.SecondaryKey, ParentKey.SecondaryKey);
                return DualAdapter.SecondaryAdapter.CheckComponent(setting, TempNode, component, components);
            }

            return new CheckComponentResult(CheckComponentResultConstants.InvalidTreeNode, null, "Adapter is undefined.");
        }

        public static ComponentDetail[] GetComponentDetails(Setting setting, TreeNode treeNode, Component component, ComponentDetailTypeConstants componentDetailTypeConstant)
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            DualKey Key = new DualKey(treeNode.Key);
            DualKey ParentKey = new DualKey(string.IsNullOrEmpty(treeNode.ParentKey) ? null : treeNode.ParentKey);

            if (!string.IsNullOrEmpty(Key.PrimaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.PrimaryKey, ParentKey.PrimaryKey);
                return DualAdapter.PrimaryAdapter.GetComponentDetails(setting, TempNode, component, componentDetailTypeConstant);
            }
            else if (!string.IsNullOrEmpty(Key.SecondaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.SecondaryKey, ParentKey.SecondaryKey);
                return DualAdapter.SecondaryAdapter.GetComponentDetails(setting, TempNode, component, componentDetailTypeConstant);
            }

            return new ComponentDetail[0];
        }

        public static Picture[] GetComponentPictures(Setting setting, Component component, PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            return DualAdapter.PrimaryAdapter.GetComponentPictures(setting, component, pictureTypeConstant, top, withoutContent);
        }

        public static Picture[] GetTreeNodePictures(Setting setting, TreeNode treeNode, PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            // Create a dual adapter
            IDualAdapter DualAdapter = getAdapter();

            DualKey Key = new DualKey(treeNode.Key);
            DualKey ParentKey = new DualKey(string.IsNullOrEmpty(treeNode.ParentKey) ? null : treeNode.ParentKey);

            if (!string.IsNullOrEmpty(Key.PrimaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.PrimaryKey, ParentKey.PrimaryKey);
                return DualAdapter.PrimaryAdapter.GetTreeNodePictures(setting, TempNode, pictureTypeConstant, top, withoutContent);
            }
            else if (!string.IsNullOrEmpty(Key.SecondaryKey))
            {
                TreeNode TempNode = MyGetTempTreeNode(treeNode, Key.SecondaryKey, ParentKey.SecondaryKey);
                return DualAdapter.SecondaryAdapter.GetTreeNodePictures(setting, TempNode, pictureTypeConstant, top, withoutContent);
            }

            return new Picture[0];
        }
        #endregion

        #region My methods
        private static TreeNode MyGetTempTreeNode(TreeNode treeNode, string key, string parentKey)
        {
            // Check if the node is null
            if (treeNode == null)
            {
                // Also return null
                return null;
            }

            // Check if the key is null or empty
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            // Create a temporary node
            TreeNode TempNode = new TreeNode();
            TempNode.DisplayName = treeNode.DisplayName;
            TempNode.HasDetails = treeNode.HasDetails;
            TempNode.HasPictures = treeNode.HasPictures;
            TempNode.IsType = treeNode.IsType;
            TempNode.Key = key;
            TempNode.ParentKey = string.IsNullOrEmpty(parentKey) ? null : parentKey; ;
            TempNode.Level = treeNode.Level;
            TempNode.NewPrice = treeNode.NewPrice;
            TempNode.Parameters = treeNode.Parameters;
            TempNode.Price = treeNode.Price;

            return TempNode;
        }

        private static TreeNode[] MyMergeTreeNodes(TreeNode[] primaryAdapterNodes, TreeNode[] secondaryAdapterNodes)
        {
            // Check if primary adapter nodes array os valid
            if (primaryAdapterNodes == null)
            {
                // Throw an exception
                throw new ArgumentNullException("primaryAdapterNodes", "Primary adapter nodes array is null.");
            }

            // Check if secondary adapter nodes array os valid
            if (secondaryAdapterNodes == null)
            {
                // Throw an exception
                throw new ArgumentNullException("secondaryAdapterNodes", "Secondary adapter nodes array is null.");
            }

            // Create a nodes list
            List<TreeNode> Nodes = new List<TreeNode>();

            List<TreeNode> SecondaryAdapterNodes = secondaryAdapterNodes.ToList();

            // Iterate through all primary adapter nodes
            foreach (var LoopNode in primaryAdapterNodes)
            {
                // Find a duplicate
                TreeNode Duplicate = MyGetNodeDuplicate(LoopNode, SecondaryAdapterNodes);

                DualKey MergedKey = new DualKey(LoopNode.Key, Duplicate == null ? null : Duplicate.Key);
                DualKey MergedParentKey = new DualKey(LoopNode.ParentKey, Duplicate == null ? null : Duplicate.ParentKey);

                TreeNode NewNode = new TreeNode();
                NewNode.DisplayName = LoopNode.DisplayName;
                NewNode.HasDetails = LoopNode.HasDetails;
                NewNode.HasPictures = LoopNode.HasPictures;
                NewNode.IsType = LoopNode.IsType;
                NewNode.Key = MergedKey.ToString();
                NewNode.ParentKey = MergedParentKey.IsEmpty ? null : MergedParentKey.ToString();
                NewNode.Level = new Level(LoopNode.Level.Number, LoopNode.Level.Designation);
                NewNode.Price = LoopNode.Price;
                NewNode.NewPrice = LoopNode.NewPrice;

                if (Duplicate != null)
                {
                    SecondaryAdapterNodes.Remove(Duplicate);
                }

                // Add the node to the nodes list
                Nodes.Add(NewNode);
            }

            // Iterate through all secondary nodes (duplicates are already removed)
            foreach (var LoopNode in SecondaryAdapterNodes)
            {
                DualKey MergedKey = new DualKey(null, LoopNode.Key);
                DualKey MergedParentKey = new DualKey(null, LoopNode.ParentKey);

                LoopNode.Key = MergedKey.ToString();
                LoopNode.ParentKey = MergedParentKey.IsEmpty ? null : MergedParentKey.ToString();

                // Add the node to the nodes list
                Nodes.Add(LoopNode);
            }

            // Return the nodes
            return Nodes.ToArray();
        }

        private static TreeNode MyGetNodeDuplicate(TreeNode node, List<TreeNode> nodes)
        {
            // Find the node
            var SearchedNode = (from Node in nodes
                                where Node.DisplayName == node.DisplayName
                                && Node.Level.Number == node.Level.Number
                                select Node).FirstOrDefault();

            // Return the node
            return SearchedNode;
        }

        private static void MyMarkNodes(TreeNode[] nodes, AdapterType adapterType, string key, string parentKey)
        {
            // Iterate through all nodes
            foreach (var LoopNode in nodes)
            {
                // Create a parameters list
                List<Parameter> Parameters = new List<Parameter>();

                // Set adapter type
                Parameters.Add(new Parameter(CnstParameterCategory, CnstAdapterTypeParameter, adapterType.ToString()));

                // Check if adapter type is primary
                if (adapterType == AdapterType.Primary)
                {
                    // Add the key parameter
                    Parameters.Add(new Parameter(CnstParameterCategory, CnstPrimaryNodeKeyParameter, LoopNode.Key));

                    // Check if the parent key exists
                    if (!string.IsNullOrEmpty(LoopNode.ParentKey))
                    {
                        // Add the parent key parameter
                        Parameters.Add(new Parameter(CnstParameterCategory, CnstPrimaryNodeParentKeyParameter, LoopNode.ParentKey));
                    }
                }
                else
                {
                    // Add the key parameter
                    Parameters.Add(new Parameter(CnstParameterCategory, CnstSecondaryNodeKeyParameter, LoopNode.Key));

                    // Check if the parent key exists
                    if (!string.IsNullOrEmpty(LoopNode.ParentKey))
                    {
                        // Add the parent key parameter
                        Parameters.Add(new Parameter(CnstParameterCategory, CnstSecondaryNodeParentKeyParameter, LoopNode.ParentKey));
                    }
                }

                // Assign the parameters
                LoopNode.Parameters = Parameters.ToArray();
            }
        }

        private static SearchBy MyGetSerchByFromLevelNumber(int levelNumber)
        {
            switch (levelNumber)
            {
                case 0:
                    return SearchBy.Level0Name;

                case 1:
                    return SearchBy.Level1Name;

                case 2:
                    return SearchBy.Level2Name;

                default:
                    return SearchBy.Level3Name;
            }
        }

        private static void MyUpdateParent(TreeNode[] nodes)
        {
            for (int NodeIndex = 0; NodeIndex < nodes.Length; NodeIndex++)
            {
                DualKey Key = new DualKey(nodes[NodeIndex].Key);

                int NumberOfKeys = 0;

                if(!string.IsNullOrEmpty(Key.PrimaryKey))
                {
                    NumberOfKeys++;
                }
                if (!string.IsNullOrEmpty(Key.SecondaryKey))
                {
                    NumberOfKeys++;
                }

                if (NumberOfKeys == 2 && NodeIndex > 0)
                {
                    nodes[NodeIndex - 1].ParentKey = nodes[NodeIndex].Key;
                    break;
                }
            }
        }
        #endregion
    }
}
