// OWNER BK, 15-06-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    #endregion

    /// <summary>
	/// Suchwerte für Interessenten
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ITSearchData
	{
		#region Methods
		/// <summary>
		/// Übernimmt die Werte einer anderen Instanz.
		/// </summary>
		public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData, bool trimStringValues)
		{
			if (itSearchData == null)
			{
                throw new Exception("itSearchData");
			}

			// Set data
            this.SYSIT = itSearchData.SYSIT;
			this.PRIVATFLAG = itSearchData.PRIVATFLAG;
			this.CODE = itSearchData.CODE;
			this.NAME = itSearchData.NAME;
			this.VORNAME = itSearchData.VORNAME;
			this.ZUSATZ = itSearchData.ZUSATZ;
			this.STRASSE = itSearchData.STRASSE;
			this.HSNR = itSearchData.HSNR;
			this.PLZ = itSearchData.PLZ;
			this.ORT = itSearchData.ORT;
            this.SYSKDTYP = itSearchData.SYSKDTYP;
            this.AngAntVtNummer = itSearchData.AngAntVtNummer;
            this.AngAntVtVon = itSearchData.AngAntVtVon;
            this.AngAntVtBis = itSearchData.AngAntVtBis;
            this.SYSPRPRODUCT = itSearchData.SYSPRPRODUCT;
            this.FzBEZEICHNUNG = itSearchData.FzBEZEICHNUNG;
            this.FzKENNZEICHEN = itSearchData.FzKENNZEICHEN;
            this.NAMEVORNAMEZUSATZ = itSearchData.NAMEVORNAMEZUSATZ;
            this.Finanzierungsart = itSearchData.Finanzierungsart;


            // Trim
            if (trimStringValues)
            {
                this.CODE = (this.CODE != null ? this.CODE.Trim() : null);
                this.NAME = (this.NAME != null ? this.NAME.Trim() : null);
                this.VORNAME = (this.VORNAME != null ? this.VORNAME.Trim() : null);
                this.NAMEVORNAMEZUSATZ = (this.NAMEVORNAMEZUSATZ != null ? this.NAMEVORNAMEZUSATZ.Trim() : null);
                this.ZUSATZ = (this.ZUSATZ != null ? this.ZUSATZ.Trim() : null);
                this.STRASSE = (this.STRASSE != null ? this.STRASSE.Trim() : null);
                this.HSNR = (this.HSNR != null ? this.HSNR.Trim() : null);
                this.PLZ = (this.PLZ != null ? this.PLZ.Trim() : null);
                this.ORT = (this.ORT != null ? this.ORT.Trim() : null);
                this.AngAntVtNummer = (this.AngAntVtNummer != null ? this.AngAntVtNummer.Trim() : null);
                this.FzBEZEICHNUNG = (this.FzBEZEICHNUNG != null ? this.FzBEZEICHNUNG.Trim() : null);
                this.FzKENNZEICHEN = (this.FzKENNZEICHEN != null ? this.FzKENNZEICHEN.Trim() : null);
                this.Finanzierungsart= (this.Finanzierungsart != null ? this.Finanzierungsart.Trim() : null);
            }
		}

		/// <summary>
		/// Setzt alle Werte zurück.
		/// </summary>
		public void ResetValues()
		{
			// Adopt
			this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.ITSearchData(), false);
		}
		#endregion

		#region Properties
        /// <summary>
        /// IT:SYSIT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long? SYSIT
        {
            get;
            set;
        }

        /// <summary>
        /// Status, Privat
        /// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool? PRIVATFLAG
        {
            get;
            set;
        }

        /// <summary>
        /// Code
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string CODE
        {
			get;
			set;
        }

        /// <summary>
        /// Name
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
			get;
			set;
        }

        /// <summary>
        /// Vorname
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string VORNAME
        {
			get;
			set;
        }

        /// <summary>
        /// Namenszusatz
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public string ZUSATZ
		{
			get;
			set;
		}

        /// <summary>
        /// NAMEVORNAMEZUSATZ
        /// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string NAMEVORNAMEZUSATZ
        {
            get;
            set;
        }

        /// <summary>
        /// Strasse
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string STRASSE
        {
			get;
			set;
        }

        /// <summary>
        /// Hausnummer
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
		[System.Runtime.Serialization.DataMember]
        public string HSNR
        {
			get;
			set;
        }

        /// <summary>
        /// Postleitzahl
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string PLZ
        {
			get;
			set;
        }

        /// <summary>
        /// Ort
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string ORT
        {
			get;
			set;
        }

        /// <summary>
        /// Angebot, Antrag, Vertrag Nummer
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string AngAntVtNummer
        {
            get;
            set;
        }

        /// <summary>
        /// Angebot, Antrag, Vertrag Datum Von
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public DateTime? AngAntVtVon
        {
            get;
            set;
        }

        /// <summary>
        /// Angebot, Antrag, Vertrag Datum Bis
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public DateTime? AngAntVtBis
        {
            get;
            set;
        }

        /// <summary>
        /// SysPrProduct
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long? SYSPRPRODUCT
        {
            get;
            set;
        }

        /// <summary>
        /// Fahrzeugbezeichnung
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string FzBEZEICHNUNG
        {
            get;
            set;
        }

        /// <summary>
        /// Fahrzeugkennzeichen
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string FzKENNZEICHEN
        {
            get;
            set;
        }

        /// <summary>
        /// Finanzierungsart
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string Finanzierungsart
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public bool ISPERSON { get; set; }



        /// <summary>
        /// SYSKDTYP
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string SYSKDTYP
        {
            get;
            set;
        }
        #endregion
    }
}