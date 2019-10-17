// OWNER MK, 05-03-2009
namespace Cic.OpenLease.Model.DdOl
{
    public partial class KONTO
    {
		#region Extended properties
		public string ExtCompleteName
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverBankAccountCompleteName(this.KONTONR, this.IBAN);
			}
		}
		#endregion

		#region Flag properties
		public bool IsActive
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.AKTIVKZ);
			}
		}
		#endregion
	}
}