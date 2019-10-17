// OWNER MK, 05-03-2009
namespace Cic.OpenLease.Model.DdOl
{
    public partial class PARTNER
	{
		#region Private constants
		private const bool CnstIsPrivate = true;
		#endregion

		#region Dummy properties
		// TODO MK 5 BK, Remove, if EF property is available
		public string NameSuffix
		{
			get;
			set;
		}
		#endregion

		#region Extended properties
		public string ExtCompleteName
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverPersonCompleteName(CnstIsPrivate, this.TITEL, this.FIRSTNAME, this.LASTNAME, this.NameSuffix);
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