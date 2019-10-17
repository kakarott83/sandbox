// OWNER MK, 04-02-2009
namespace Cic.OpenLease.Model.DdOl
{
    public partial class VT
    {
		#region Extended properties
		public string ExtTitle
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverContractTitle(this.PERSON.IsPrivate, this.PERSON.CODE, this.PERSON.TITEL, this.PERSON.VORNAME, this.PERSON.NAME, this.PERSON.ZUSATZ, this.VERTRAG);
			}
		}
		#endregion

		#region Properties
		public long? SysKD
		{
			// TEST BK 0 BK, Not tested
			get
			{
				try
				{
					return (long?)this.PERSONReference.EntityKey.EntityKeyValues[0].Value;
				}
				catch
				{
					return null;
				}
			}
		}

		public System.Data.Objects.DataClasses.EntityReference CustomerReference
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return this.PERSONReference;
			}
		}

		public Cic.OpenLease.Model.DdOl.PERSON CustomerPerson
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return this.PERSON;
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

        public bool IsLocked
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.LOCKED);
            }
        }

        public bool IsChecked
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.OK);
            }
        }

        public bool IsEnded
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.ENDEKZ);
            }
        }
		#endregion

        public partial struct FieldNames
        {
            #region Properties
            public static string SysKD
            {
                get { return "PERSON.SYSPERSON"; }
            }
            #endregion
        }
    }
}