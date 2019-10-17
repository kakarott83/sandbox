// OWNER BK, 15-06-2008
namespace Cic.OpenLease.Model.DdOl
{
    public partial class IT
	{
        #region Enums
        /// <summary>        
        /// Erreichbarkeit, IT:
        /// - Undefined
        /// Nicht definiert.
        /// - PrivateTelephone
        /// Telefon Privat.
        /// - BusinessTelephone
        /// Telefon Geschäft.
        /// - MobileTelephone
        /// Telefon Mobil.
        /// </summary>
        public enum AvailabilityStatuses : int
        {
            Undefined = 0,                  // Nicht definiert
            PrivateTelephone = 1,           // Telefon Privat
            BusinessTelephone = 2,          // Telefon Geschäft
            MobileTelephone = 3             // Telefon Mobil
        }
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

		public string ExtBankAccountCompleteName
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverBankAccountCompleteName(this.KONTONR, this.IBAN);
			}
		}

		public string ExtBankCompleteName
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverBankCodeCompleteName(this.BLZ, this.BANKNAME);
			}
		}

		public string ExtContactCompleteName
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverPersonCompleteName(true, this.TITELKONT, this.VORNAMEKONT, this.NAMEKONT, this.ZUSATZKONT);
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

		public bool HasTradeRegisterEntry
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.HREGISTERFLAG);
			}
		}

		public bool IsInsolvent
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.KONKURSFLAG);
			}
		}

		public bool HasEnteredWithFamily
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.FAMEINRFLAG);
			}
		}

		public bool HasUmlimitedEmployment1
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.UNBEFRAG1);
			}
		}

		public bool HasUmlimitedEmployment2
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.UNBEFRAG2);
			}
		}

		public bool HasChildrenAtHome
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.KINDERIMHAUS);
			}
		}

		public bool IsNetIncome
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.EINKNETTOFLAG);
			}
		}

		public bool HasExtraCostsInsideRent
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.NEBENINMIETE);
			}
		}

		public bool HasDistraints
		{
			get
			{
				return Cic.Basic.OpenLease.BooleanHelper.Convert(this.PFAENDUNGFLAG);
			}
		}
		#endregion
	}
}