// OWNER: BK, 25-08-2008
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public interface ITreeInfoBase
	{
		#region Properties
		bool IsUnique
		{
			// TODO BK 0 BK, Not tested
			get;
		}

		int CountOfLevels
		{
			// TODO BK 0 BK, Not tested
			get;
		}

		int CountOfNodes
		{
			// TODO BK 0 BK, Not tested
			get;
		}
		#endregion
	}
}
