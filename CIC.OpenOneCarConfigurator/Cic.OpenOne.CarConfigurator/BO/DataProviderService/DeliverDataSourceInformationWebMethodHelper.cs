// OWNER: BK, 17-04-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
	//[System.CLSCompliant(true)]
	internal static class DeliverDataSourceInformationWebMethodHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.DataSourceInformation Execute()
		{
			Cic.P000001.Common.DataProvider.IAdapter Adapter;
			Cic.P000001.Common.DataSourceInformation CommonDataSourceInformation;
			Cic.P000001.Common.DataSourceInformation WebDataSourceInformation;

			try
			{
				// Get adapter
                Adapter = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderAdapterLoaderHelper().LoadAdapter();
				// Execute adapter method
				CommonDataSourceInformation = Adapter.DeliverDataSourceInformation();
				// Create web data source information
				WebDataSourceInformation = CommonDataSourceInformation;
			}
			catch
			{
				// Throw caught exception
				throw;
			}

			// Return
			return WebDataSourceInformation;
		}
		#endregion
	}
}
