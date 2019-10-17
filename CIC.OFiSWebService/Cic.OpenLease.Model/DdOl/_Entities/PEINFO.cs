// OWNER BK, 12-11-2009
namespace Cic.OpenLease.Model.DdOl
{
	public partial class PEINFO
	{
		#region Dummy properties
		#endregion

		#region Flag properties
		public bool IsPrivate
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.PRIVATEFLAG);
			}
		}
		#endregion
	}
}