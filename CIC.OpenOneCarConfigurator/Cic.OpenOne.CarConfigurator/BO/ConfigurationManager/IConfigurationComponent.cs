// OWNER: BK, 25-08-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    /// <summary>
    /// Configuration Component Base Interface
    /// </summary>
	[System.CLSCompliant(true)]
	public interface IConfigurationComponent : Cic.P000001.Common.ConfigurationManager.IConfigurationComponentBase
	{
		#region Properties
        /// <summary>
        /// component
        /// </summary>
		Cic.P000001.Common.Component Component
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Component Details
        /// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.P000001.Common.ComponentDetail[] ComponentDetails
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
