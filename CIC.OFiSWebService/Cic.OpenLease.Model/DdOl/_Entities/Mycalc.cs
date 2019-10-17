// OWNER BK, 11-03-2009
namespace Cic.OpenLease.Model.DdOl
{
	public partial class MYCALC
    {
		#region Dummy properties
		// TODO MK 5 BK, Remove, if EF property is available
		public string PersonTitle
		{
			get;
			set;
		}

		// TODO MK 5 BK, Remove, if EF property is available
		public string PersonNameSuffix
		{
			get;
			set;
		}
		#endregion

		#region Extended properties
        public long ExtId
        {
            get
            {
                return this.SYSMYCALC;
            }
        }
        
		public string ExtPersonCompleteName
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverPersonCompleteName(this.PersonIsPrivate, this.PersonTitle, this.VORNAME, this.NAME, this.PersonNameSuffix);
			}
		}

		public string ExtPersonTitle
		{
			// TEST BK 0 BK, Not tested
			get 
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverPersonTitle(this.PersonIsPrivate, this.MATCHCODE, this.PersonTitle, this.VORNAME, this.NAME, this.PersonNameSuffix);
			}
		}

		public string ExtConfigurationKey
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return this.BEMERKUNG2;
			}
			// TEST BK 0 BK, Not tested
			set
			{
				this.BEMERKUNG2 = value;
			}
		}
		#endregion

		#region Flag properties
		public bool PersonIsPrivate
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.PRIVATFLAG);
			}
		}

		public bool IsFlagOpt1
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.FLAGOPT1);
            }
        }

        public bool IsFlagOpt2
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.FLAGOPT2);
            }
        }

        public bool IsFlagOpt3
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.FLAGOPT3);
            }
        }

        public bool IsFlagOpt4
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.FLAGOPT4);
            }
        }

        public bool IsFlagOpt5
        {
            get
            {
                return Cic.Basic.OpenLease.BooleanHelper.Convert(this.FLAGOPT5);
            }
        }
		#endregion

        #region Formatting properties
        public System.Collections.Generic.List<string> CurrencyProperties
        {
            get
            {
                return new System.Collections.Generic.List<string>()
                {
                    FieldNames.GESAMT,
                    FieldNames.SZ,
                    FieldNames.RW,
                    FieldNames.RATE
                };
            }
        }
        #endregion
	}
}
