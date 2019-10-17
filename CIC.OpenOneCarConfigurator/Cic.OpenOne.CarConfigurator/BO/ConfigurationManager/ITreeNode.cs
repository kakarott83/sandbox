// OWNER: BK, 25-08-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    /// <summary>
    /// Tree Node Interface
    /// </summary>
    [System.CLSCompliant(true)]
	public interface ITreeNode : Cic.P000001.Common.ITreeNodeBase
	{
		#region Properties
        /// <summary>
        /// Level
        /// </summary>
		Cic.P000001.Common.Level Level
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		/// <summary>
		/// Parameters
		/// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.P000001.Common.Parameter[] Parameters
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
		#endregion
	}
}
