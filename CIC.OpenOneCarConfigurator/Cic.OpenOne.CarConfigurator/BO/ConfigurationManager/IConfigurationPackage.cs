// OWNER: BK, 25-08-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    /// <summary>
    /// Configuration Package Base Interface
    /// </summary>
	[System.CLSCompliant(true)]
	public interface IConfigurationPackage : Cic.P000001.Common.ConfigurationManager.IConfigurationPackageBase
	{
		#region Properties
        /// <summary>
        /// Catalog Items
        /// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.CatalogItem[] CatalogItems
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Data Source Information
        /// </summary>
		Cic.P000001.Common.DataSourceInformation DataSourceInformation
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Setting
        /// </summary>
		Cic.P000001.Common.Setting Setting
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Configuration Tree Node
        /// </summary>
		Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationTreeNode ConfigurationTreeNode
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Configuration Components
        /// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationComponent[] ConfigurationComponents
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
		#endregion
	}
}
