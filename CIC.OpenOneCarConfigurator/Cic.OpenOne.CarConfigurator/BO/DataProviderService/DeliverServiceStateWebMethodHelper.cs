// OWNER: BK, 17-04-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
	//[System.CLSCompliant(true)]
	internal static class DeliverServiceStateWebMethodHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static Cic.P000001.Common.ServiceState Execute()
		{
			System.Diagnostics.Stopwatch Stopwatch;
			Cic.P000001.Common.DataProvider.IAdapter Adapter = null;
			Cic.P000001.Common.AdapterState AdapterState = null;
			bool IsServiceable = true;
			string ExceptionMessage = null;
			string Message;
			long ProcessingTime = 0;

			
			// New stopwatch
			Stopwatch = new System.Diagnostics.Stopwatch();

			// Start stop watch
			Stopwatch.Start();

			try
			{
				// Get adapter
                Adapter = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderAdapterLoaderHelper().LoadAdapter();
			}
			catch (System.Exception ex)
			{
				// Get message
				ExceptionMessage = ex.Message;
			}

			// Check adapter
			if (Adapter == null)
			{
				// Set state
				IsServiceable = false;
				// TODO BK 0 BK, Localize text
				// Set message
				Message = "Adapter is not available.";
				// Check exception message
				if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(ExceptionMessage))
				{
					// Add exception message
					Message = (Message + " " + ExceptionMessage.Trim());
				}
			}
			else
			{
				try
				{
					// Get adapter state
					AdapterState = Adapter.DeliverAdapterState();
				}
				catch
				{
					// Ignore exception
				}

				// Check adapter state
				if (AdapterState == null)
				{
					// Set state
					IsServiceable = false;
					// TODO BK 0 BK, Localize text
					// Set message
					Message = "Adapter state is not available.";
				}
				else
				{
					// Set state
					IsServiceable = AdapterState.IsServiceable;
					// Set message
					Message = AdapterState.AdapterName;
					// Check message
					if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(AdapterState.Message))
					{
						// Set message
						Message = (Message + ": " + AdapterState.Message.Trim());
					}
				}
			}

			// Stop stop watch
			Stopwatch.Stop();
			// Get processing time
			ProcessingTime = Stopwatch.ElapsedMilliseconds;

			try
			{
				// New service state
				//return new Cic.P000001.Common.ServiceState(IsServiceable, Message, ProcessingTime);
                Cic.P000001.Common.ServiceState ServiceState = new Cic.P000001.Common.ServiceState();
                ServiceState.IsServiceable = IsServiceable;
                ServiceState.Message = Message;
                ServiceState.ProcessingTime = ProcessingTime;
                return ServiceState;
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion
	}
}
