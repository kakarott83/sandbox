// OWNER: BK, 06-06-2008
namespace Cic.P000001.Common.ConfigurationManager
{
	#region Using directives
	
	#endregion

	[System.Serializable]
	[System.CLSCompliant(true)]
	public sealed class ConfigurationTreeNode : Cic.P000001.Common.ConfigurationManager.IConfigurationTreeNode, Cic.OpenOne.Util.Reflection.IReconstructable
	{
		#region Private variables
		private bool _Parameterless=true;
		private Cic.P000001.Common.TreeNode _TreeNode;
		private Cic.P000001.Common.TreeNodeDetail[] _TreeNodeDetails;
		private Cic.P000001.Common.Picture[] _Pictures;
		#endregion

		#region Constructors
		// NOTE BK, Parameterless constructor for serialization
		// TESTEDBY ConfigurationTreeNodeTestFixture.CheckProperties
		public ConfigurationTreeNode()
		{
			// Set state
			_Parameterless = false;
		}

		// TESTEDBY ConfigurationTreeNodeTestFixture.ConstructWithoutTreeNode
		// TESTEDBY ConfigurationTreeNodeTestFixture.ConstructWithInvalidTreeNodeDetails
		// TESTEDBY ConfigurationTreeNodeTestFixture.ConstructWithInvalidPictures
		// TESTEDBY ConfigurationTreeNodeTestFixture.CheckProperties
		public ConfigurationTreeNode(Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.TreeNodeDetail[] treeNodeDetails, Cic.P000001.Common.Picture[] pictures)
		{
			int Index;

			// Check tree node
			if (treeNode == null)
			{
				// Throw exception
				throw new System.ArgumentException("treeNode");
			}

			// Check tree node type
			if (!treeNode.IsType)
			{
				// Throw exception
				throw  new System.ArgumentException("treeNode.IsType");
			}

			// Check tree node details
			if ((treeNodeDetails != null) && (treeNodeDetails.GetLength(0) > 0))
			{
				// Reset index
				Index = -1;
				// Loop through tree node details
				foreach (Cic.P000001.Common.TreeNodeDetail LoopTreeNodeDetail in treeNodeDetails)
				{
					// Increase index
					Index += 1;
					// Check tree node detail
					if (LoopTreeNodeDetail == null)
					{
						// Throw exception
						throw new System.ArgumentException("treeNodeDetails[" + Index.ToString() + "]");
					}
				}
			}

			// Check pictures
			if ((pictures != null) && (pictures.GetLength(0) > 0))
			{
				// Reset index
				Index = -1;
				// Loop through pictures
				foreach (Cic.P000001.Common.Picture LoopPicture in pictures)
				{
					// Increase index
					Index += 1;
					// Check picture
					if (LoopPicture == null)
					{
						// Throw exception
						throw new System.ArgumentException("pictures[" + Index.ToString() + "]");
					}
				}
			}

			// Set values
			_TreeNode = treeNode;
			_TreeNodeDetails = treeNodeDetails;
			_Pictures = pictures;
		}
		#endregion

		#region IReconstructable methods
		// NOTE BK, Essential for reconstructing
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode")]
		// TESTEDBY ConfigurationTreeNodeTestFixture.CheckProperties
		public void Reconstruct()
		{
			// Check state
			if (!_Parameterless)
			{
				try
				{
					// Reconstruct tree node
					if (_TreeNode != null)
					{
						// Reconstruct
						_TreeNode.Reconstruct();
					}
					// Reconstruct tree node details
					if ((_TreeNodeDetails != null) && (_TreeNodeDetails.GetLength(0) > 0))
					{
						// Loop through tree node details
						foreach (Cic.P000001.Common.TreeNodeDetail LoopTreeNodeDetail in _TreeNodeDetails)
						{
							// Check object
							if (LoopTreeNodeDetail != null)
							{
								// Reconstruct
								LoopTreeNodeDetail.Reconstruct();
							}
						}
					}
					// Reconstruct pictures
					if ((_Pictures != null) && (_Pictures.GetLength(0) > 0))
					{
						// Loop through tree node details
						foreach (Cic.P000001.Common.Picture LoopPicture in _Pictures)
						{
							// Check object
							if (LoopPicture != null)
							{
								// Reconstruct
								LoopPicture.Reconstruct();
							}
						}
					}

					// Create new instance
					new Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode(_TreeNode, _TreeNodeDetails, _Pictures);
				}
				catch
				{
					// Throw caught exception
					throw;
				}

				// Reset state
				_Parameterless = false;
			}
		}
		#endregion

		#region IConfigurationTreeNode properties
		public Cic.P000001.Common.TreeNode TreeNode
		{
			// TESTEDBY ConfigurationTreeNodeTestFixture.CheckProperties
			get { return _TreeNode; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationTreeNodeTestFixture.CheckProperties
			set 
			{ 
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_TreeNode = value;
				}
			}
		}

		public Cic.P000001.Common.TreeNodeDetail[] TreeNodeDetails
		{
			// TESTEDBY ConfigurationTreeNodeTestFixture.CheckProperties
			get { return _TreeNodeDetails; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationTreeNodeTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_TreeNodeDetails = value;
				}
			}
		}

		public Cic.P000001.Common.Picture[] Pictures
		{
			// TESTEDBY ConfigurationTreeNodeTestFixture.CheckProperties
			get { return _Pictures; }
			// NOTE BK, For serialization
			// TESTEDBY ConfigurationTreeNodeTestFixture.CheckProperties
			set
			{
				// Check state
				if (!_Parameterless)
				{
					// Set value
					_Pictures = value;
				}
			}
		}
		#endregion
	}
}
