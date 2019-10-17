// OWNER BK, 18-03-2009
namespace Cic.OpenLease.Model.DdOl
{
    public partial class ANTOBAUST : Cic.OpenLease.Model.DdOl.IVehicleEquipment
    {
		#region IVehicle properties
		public long ExtId
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return this.SYSOBAUST;
			}
		}
		#endregion

		#region Flag properties
		public bool IsPackage
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.FLAGPACKET);
			}
		}
		#endregion
	}
}