// OWNER: BK, 25-08-2008
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public interface ICheckComponentResultBase
	{
		#region Properties
		Cic.P000001.Common.DataProvider.CheckComponentResultConstants CheckComponentResultConstant
		{
			// TODO BK 0 BK, Not tested
			get;
		}

		string Message
		{
			// TODO BK 0 BK, Not tested
			get;
		}
		#endregion
	}
}
