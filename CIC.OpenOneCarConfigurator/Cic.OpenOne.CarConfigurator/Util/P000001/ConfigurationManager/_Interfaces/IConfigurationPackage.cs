// OWNER: BK, 25-08-2008
namespace Cic.P000001.Common.ConfigurationManager
{
	[System.CLSCompliant(true)]
	public interface IConfigurationPackage : Cic.P000001.Common.ConfigurationManager.IConfigurationPackageBase
	{
		#region Properties
		// NOTE BK, Sure the return of an array is realy bad, but this interface is also defined for webservice usage, so the property cannnot be replaced by a method and cannot use a collection data type. If you know a better way, please tell me.
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.P000001.Common.ConfigurationManager.CatalogItem[] CatalogItems
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		Cic.P000001.Common.DataSourceInformation DataSourceInformation
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		Cic.P000001.Common.Setting Setting
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		Cic.P000001.Common.ConfigurationManager.ConfigurationTreeNode ConfigurationTreeNode
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		// NOTE BK, Sure the return of an array is realy bad, but this interface is also defined for webservice usage, so the property cannnot be replaced by a method and cannot use a collection data type. If you know a better way, please tell me.
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.P000001.Common.ConfigurationManager.ConfigurationComponent[] ConfigurationComponents
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
		#endregion
	}
}
