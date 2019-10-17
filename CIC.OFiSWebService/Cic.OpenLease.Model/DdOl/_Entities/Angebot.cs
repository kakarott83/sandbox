// OWNER MK, 04-02-2009
namespace Cic.OpenLease.Model.DdOl
{
    public partial class ANGEBOT
    {
        #region Enums
        /// <summary>
        /// Zustände, Angebot:
        /// - neu (New)
        /// Das Angebot wird entweder in OpenLease oder durch einen WCF-Dienst erstellt. In beiden Fällen muss der Zustand einrichtbar sein.
        /// - gedruckt (Printed)
        /// Der WCF-Dienst zum Drucken des Angebotes muss den Zustand ändern . Der Dienst muss dementsprechend einrichtbar sein.
        /// Ab diesem Zustand dürfen die Angebotsdaten nicht mehr geändert werden.
        /// - eingereicht 
        /// Eingereicht ist ein Angebot, sobald eine seiner Kalkulationen eingereicht worden ist.
        /// Der Vorgang Angebot2Antrag setzt nach erfolgreicher Erstellung des Antrages den Zustand.
        /// - abgelehnt
        /// Der Kunde hat das Angebot abgelehnt.
        /// - abgelaufen
        /// Ggf. prüft ein Job, ob Angebote abgelaufen sind und setzt den Status.
        /// </summary>
        public enum AngebotStatuses
        {
            New,            // new
            Printed,        // gedruckt
            HandedIn,       // eingereicht
            Rejected,       // abgelehnt
            Expired,        // abgelaufen
        }
        #endregion

        #region Extended properties
        public string ExtTitle
		{
			// TEST BK 0 BK, Not tested
			get
			{
				// Check person
				if (this.PERSON == null)
				{
					// NOTE BK, More then one return
					return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverContractTitle(null, this.ANGEBOT1);
				}
				else
				{
					// NOTE BK, More then one return
					return Cic.OpenLease.Model.ExtendedPropertyHelper.DeliverContractTitle(this.PERSON.IsPrivate, this.PERSON.CODE, this.PERSON.TITEL, this.PERSON.VORNAME, this.PERSON.NAME, this.PERSON.ZUSATZ, this.ANGEBOT1);
				}
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

		public Cic.OpenLease.Model.DdOl.PERSON CustomerPERSON
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