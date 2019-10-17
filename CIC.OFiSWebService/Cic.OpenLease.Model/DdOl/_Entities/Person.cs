// OWNER MK, 03-11-2008
namespace Cic.OpenLease.Model.DdOl
{
    public partial class PERSON
	{
		#region Dummy properties
		#endregion

		#region Extended properties
		public string ExtCompleteName
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverPersonCompleteName(this.IsPrivate, this.TITEL, this.VORNAME, this.NAME, this.ZUSATZ);
			}
		}

		public string ExtTitle
		{
			// TEST BK 0 BK, Not tested
			get 
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverPersonTitle(this.IsPrivate, this.CODE, this.TITEL, this.VORNAME, this.NAME, this.ZUSATZ);
			}
		}

		public string ExtZipCodeCity
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverZipCodeCity(this.PLZ, this.ORT);
			}
		}
		#endregion

		#region Flag properties
		public bool IsPrivate
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.PRIVATFLAG);
			}
		}

		public bool IsActive
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.AKTIVKZ);
			}
		}

		public bool IsCustomer
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.FLAGKD);
			}
		}
		#endregion
	}
}