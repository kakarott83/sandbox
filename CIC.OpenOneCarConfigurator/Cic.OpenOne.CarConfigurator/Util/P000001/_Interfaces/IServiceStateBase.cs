// OWNER: BK, 25-08-2008
namespace Cic.P000001.Common
{
	[System.CLSCompliant(true)]
	public interface IServiceStateBase
	{
		#region Properties
		bool IsServiceable
		{
			// TODO BK 0 BK, Not tested
			get;
		}

		string Message
		{
			// TODO BK 0 BK, Not tested
			get;
		}

		long ProcessingTime
		{
			// TODO BK 0 BK, Not tested
			get;
		}
		#endregion
	}
}
