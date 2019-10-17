// OWNER BK, 18-03-2009
namespace Cic.OpenLease.Model.DdOl
{
	public partial class ANGOB : Cic.OpenLease.Model.DdOl.IVehicle
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
            get;
            set;
        }

		// TODO MK 5 BK, Remove, if EF property is available.
        public decimal? SUBVENTION
        {
            get;
            set;
        }

		// TODO MK 5 BK, Remove, if EF property is available.
        public string EUROTAX
        {
            get;
            set;
        }
		#endregion

        public System.Collections.Generic.List<string> CurrencyProperties
        {
            get
            {
                return new System.Collections.Generic.List<string>()
                {
                    FieldNames.GESAMT,
                    FieldNames.SZ,
                    FieldNames.RW,
                    FieldNames.BGEXTERN,
                    FieldNames.GRUND,
                    FieldNames.ZUBEHOER
                };
            }
        }
	}
}