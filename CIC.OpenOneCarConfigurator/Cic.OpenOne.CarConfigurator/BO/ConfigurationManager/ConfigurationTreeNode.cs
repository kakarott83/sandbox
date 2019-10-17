// OWNER: BK, 09-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    /// <summary>
    /// Configuration Tree Node
    /// </summary>
	[System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public class ConfigurationTreeNode : Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.IConfigurationTreeNode, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.IInputData
	{
		#region Private variables
		private Cic.P000001.Common.TreeNode _TreeNode;
		private Cic.P000001.Common.TreeNodeDetail[] _TreeNodeDetails;
		private Cic.P000001.Common.Picture[] _Pictures;
		#endregion

		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		public ConfigurationTreeNode()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurationTreeNode"></param>
        /// <todo>BK 0 BK, Not tested</todo>
		internal ConfigurationTreeNode(Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode configurationTreeNode)
		{
			// Check object
			if (configurationTreeNode == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("configurationTreeNode");
			}

			try
			{
				// Set values
				_TreeNode = configurationTreeNode.TreeNode;
                _TreeNodeDetails = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerCommonToWebConvertHelper.ConvertTreeNodeDetails(configurationTreeNode.TreeNodeDetails);
                _Pictures = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerCommonToWebConvertHelper.ConvertPictures(configurationTreeNode.Pictures);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region IInputData methods
        /// <summary>
        /// Check Properties
        /// </summary>
		public void CheckProperties()
		{
			try
			{
				// Create configurationPackage tree node
				Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertConfigurationTreeNode(this);
			}
			// TODO BK 0 BK, catch specific exceptions
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region IConfigurationTreeNode properties
        /// <summary>
        /// Treenode
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.TreeNode TreeNode
		{
			// TODO BK 0 BK, Not tested
			get { return _TreeNode; }
			// TODO BK 0 BK, Not tested
			set { _TreeNode = value; }
		}

        /// <summary>
        /// Treenode Details
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.TreeNodeDetail[] TreeNodeDetails
		{
			// TODO BK 0 BK, Not tested
			get { return _TreeNodeDetails; }
			// TODO BK 0 BK, Not tested
			set { _TreeNodeDetails = value; }
		}

        /// <summary>
        /// Pictures
        /// </summary>
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
		public Cic.P000001.Common.Picture[] Pictures
		{
			// TODO BK 0 BK, Not tested
			get { return _Pictures; }
			// TODO BK 0 BK, Not tested
			set { _Pictures = value; }
		}
		#endregion
	}
}
