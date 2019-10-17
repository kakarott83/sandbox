// OWNER MK, 04-02-2009
namespace Cic.OpenLease.Model.DdOl
{
    public partial class ANTRAG
    {
        #region Enums
        /// <summary>
        /// Zustände, Antrag:
        /// -	neu (New)
        /// Im Vorgang Angebot2Antrag wird ein Antrag erstellt. Hier wird der Zustand gesetzt.
        /// -	in Prüfung (BeingChecked)
        /// Sobald der Antrag in den Genehmigungsprozess läuft wird dieser Zustand gesetzt.
        /// -	abgelehnt (Rejected)
        /// Wird durch den Genehmigungsprozess gesetzt.
        /// -	genehmigt (Approved)
        /// Wird entweder durch den Genehmigungsprozess oder nach Erfüllung aller Auflagen gesetzt.
        /// -	genehmigt mit Auflagen (ApprovedWithRequirement)
        /// Wird durch den Genehmigungsprozess gesetzt.
        /// -	nicht zu Stande gekommen (FallenThrough)
        /// Wird manuell durch den Sachbearbeiter gesetzt.
        /// -	übergeleitet (LedOver)
        /// Nur genehmigte Anträge können übergeleitet werden.
        /// Wird durch den Vorgang Antrag2Vertrag gesetzt.
        /// -   erneut eingereicht (Resubmitted)
        /// </summary>
        public enum AntragStatuses
        {
            New,                        //neu,
            BeingChecked,               //inPrüfung,
            Rejected,                   //abgelehnt,
            Approved,                   //genehmigt,
            ApprovedWithRequirement,    //genehmigt mit Auflagen,
            FallenThrough,              //nicht zu Stande gekommen,
            LedOver,                    //übergeleitet
            Resubmitted,                //erneut eingereicht
        }
        #endregion

        #region Extended properties
        public string ExtTitle
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverContractTitle(this.PERSON.IsPrivate, this.PERSON.CODE, this.PERSON.TITEL, this.PERSON.VORNAME, this.PERSON.NAME, this.PERSON.ZUSATZ, this.ANTRAG1);
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