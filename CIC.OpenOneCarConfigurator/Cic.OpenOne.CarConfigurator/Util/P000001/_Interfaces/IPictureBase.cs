// OWNER: BK, 25-08-2008
namespace Cic.P000001.Common
{
	[System.CLSCompliant(true)]
	public interface IPictureBase
	{
		#region Properties
		string Title
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		Cic.P000001.Common.PictureTypeConstants PictureTypeConstant
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		// NOTE BK, Sure the return of an array is realy bad, but this interface is also defined for webservice usage, so the property cannnot be replaced by a method and cannot use a collection data type. If you know a better way, please tell me.
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		byte[] Content
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		Cic.P000001.Common.ImageFileTypeConstants ImageFileTypeConstant
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		int Width
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		int Height
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}

		string Hash
		{
			// TODO BK 0 BK, Not tested
			get;
			// TODO BK 0 BK, Not tested
			set;
		}
		#endregion
	}
}
