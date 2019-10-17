// OWNER BK, 18-03-2009
namespace Cic.OpenLease.Model.DdOl
{
	public partial class ANTKALK : Cic.OpenLease.Model.DdOl.ICalculation
	{
		#region IVehicleLeasingCalculation properties
		public long ExtId
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return this.SYSKALK;
			}
		}

		// TODO MK 5 BK, Remove, if EF property is available.
		public double? ProvisionSeller
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
		public double? ProvisionSellerInPercent
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
		public double? CaseOfDeathInsurance
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
		public double? UnemploymentInsurance
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
		public double? RateInPercent
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