// OWNER BK, 18-03-2009
namespace Cic.OpenLease.Model.DdOl
{
	public partial class OB : Cic.OpenLease.Model.DdOl.IVehicle
    {
		#region IVehicle properties
		public long ExtId
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return this.SYSOB;
			}
		}

		public string ExtConfigurationKey
		{
			// TEST BK 0 BK, Not tested
			get
			{
                return this.BESCHRENGLISCH;
			}
			// TEST BK 0 BK, Not tested
			set
			{
                this.BESCHRENGLISCH = value;
			}
		}

		// TODO MK 5 BK, Remove, if EF property is available.
        public decimal? NEBENKOSTEN
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

		// TODO MK 5 BK, Remove, if EF property is available.
        public decimal? SUBVENTION
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

		// TODO MK 5 BK, Remove, if EF property is available.
		public string EUROTAX
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}
		#endregion
	}
}