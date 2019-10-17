// OWNER BK, 01-04-2009
namespace Cic.OpenLease.Model.DdOl
{
	public partial class OBINI : Cic.OpenLease.Model.DdOl.IVehicleIni
    {
		#region IVehicle properties
		public long ExtId
		{
			get 
			{
				return this.SYSOBINI;
			}
		}

		#endregion
	}
}