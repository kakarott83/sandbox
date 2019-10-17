// OWNER MK, 03-11-2008
namespace Cic.OpenLease.Model.DdOl
{
    public partial class RN
	{
		#region Dummy properties
        #endregion

		#region Extended properties
		public double NetAmount
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return (this.GBETRAG == null) ? 0.0 : (double)this.GBETRAG;
			}
		}

		public double ValueAddedTax
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return (this.GSTEUER == null) ? 0.0 : (double)this.GSTEUER;
			}
		}

		public double GrossAmount
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return (this.NetAmount + this.ValueAddedTax);
			}
		}
		#endregion

		#region Flag properties
		public bool IsPayed
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.BEZAHLT);
			}
		}

		public bool IsIncomingMandantorOutgoingCustomer
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.ART);
			}
		}
	#endregion
    }
}
