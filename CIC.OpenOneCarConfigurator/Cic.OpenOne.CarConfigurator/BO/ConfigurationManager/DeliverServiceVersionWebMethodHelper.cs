// OWNER: BK, 09-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
	//[System.CLSCompliant(true)]
	internal static class DeliverServiceVersionWebMethodHelper
	{
		#region Methods
		// TODO BK 0 BK, Not tested
		internal static string Execute()
		{
			// Return
            return Cic.OpenOne.CarConfigurator.AssemblyInfo.GetVersion();
		}
		#endregion
	}
}
