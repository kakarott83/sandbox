// OWNER MK, 03-11-2008
namespace Cic.OpenLease.Model.DdOl
{
    public partial class KREMO
	{
		#region Flag properties
		public bool HasPartner
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.PARTNERFLAG);
			}
		}

		public bool IncomeIsNetto
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.EINKNETTOFLAG);
			}
		}

		public bool PartnerIncomeIsNetto
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.EINKNETTOFLAG2);
			}
		}
		#endregion
	}
}