// OWNER MK, 05-03-2009
namespace Cic.OpenLease.Model.DdOl
{
    public partial class BLZ
    {
		#region Extended properties
		public string ExtCompleteName
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverBankCodeCompleteName(this.BLZ1, this.NAME);
			}
		}
		#endregion
	}
}