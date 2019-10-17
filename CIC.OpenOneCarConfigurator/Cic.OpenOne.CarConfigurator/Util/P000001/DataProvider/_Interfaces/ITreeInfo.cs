// OWNER: BK, 25-08-2008
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public interface ITreeInfo : Cic.P000001.Common.DataProvider.ITreeInfoBase
	{
		#region Properties
		Cic.P000001.Common.Level PreviousLevel
		{
			// TODO BK 0 BK, Not tested
			get;
		}

		Cic.P000001.Common.Level CurrentLevel
		{
			// TODO BK 0 BK, Not tested
			get;
		}

		Cic.P000001.Common.Level NextLevel
		{
			// TODO BK 0 BK, Not tested
			get;
		}

		// NOTE BK, Sure the return of an array is realy bad, but this interface is also defined for webservice usage, so the property cannnot be replaced by a method and cannot use a collection data type. If you know a better way, please tell me.
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.P000001.Common.Level[] Levels
		{
			// TODO BK 0 BK, Not tested
			get;
		}
		#endregion
	}
}
