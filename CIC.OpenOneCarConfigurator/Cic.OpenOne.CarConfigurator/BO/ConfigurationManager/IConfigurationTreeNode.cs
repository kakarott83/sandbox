// OWNER: BK, 25-08-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    /// <summary>
    /// Configuration Tree Node Base Interface
    /// </summary>
	[System.CLSCompliant(true)]
	public interface IConfigurationTreeNode : Cic.P000001.Common.ConfigurationManager.IConfigurationTreeNodeBase
	{
		#region Properties
        /// <summary>
        /// Tree Node
        /// </summary>
		Cic.P000001.Common.TreeNode TreeNode
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Tree Node Details
        /// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.P000001.Common.TreeNodeDetail[] TreeNodeDetails
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Pictures
        /// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.P000001.Common.Picture[] Pictures
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
		#endregion
	}
}
