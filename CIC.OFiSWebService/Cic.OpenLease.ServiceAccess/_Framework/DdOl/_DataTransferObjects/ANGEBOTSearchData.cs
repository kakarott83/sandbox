// OWNER MK, 05-03-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{


    #region Using
    using System;
    #endregion

    /// <summary>
    /// Suchwerte für Angebote
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANGEBOTSearchData
    {
        #region Methods
        /// <summary>
        /// Übernimmt die Werte einer anderen Instanz.
        /// </summary>
        public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData angebotSearchData)
        {
            if (angebotSearchData == null)
            {
                throw new Exception("angebotSearchData");
            }

            // Set data

            this.ANGEBOT1 = angebotSearchData.ANGEBOT1;

            this.PersonIt = angebotSearchData.PersonIt;
            this.ZUSTAND = angebotSearchData.ZUSTAND;
        }

        /// <summary>
        /// Setzt alle Werte zurück.
        /// </summary>
        public void ResetValues()
        {
            // Adopt
            this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData());
        }
        #endregion

        #region Properties


        /// <summary>
        /// Code
        /// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string ZUSTAND
        {
            get;
            set;
        }


        /// <summary>
        /// Code
        /// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string ANGEBOT1
        {
            get;
            set;
        }


        /// <summary>
        /// Kunde (IT/PERSON)
        /// Werte gleich null werden bei der Suche ignoriert.
        /// Zurzeit nur suchen in IT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string PersonIt
        {
            get;
            set;
        }


        /// <summary>
        /// Kunde (IT/PERSON)
        /// Werte gleich null werden bei der Suche ignoriert.
        /// Zurzeit nur suchen in IT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long? SYSIT
        {
            get;
            set;
        }


        /// <summary>
        /// Kunde (VERKAUFER/PERSON), Name
        /// Werte gleich null werden bei der Suche ignoriert.
        /// Zurzeit nur suchen in IT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string PersonVerkaufer
        {
            get;
            set;
        }

        /// <summary>
        /// Sonderkalkulation
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool SPECIALCALC
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

        /// <summary>
        /// Angebot, Antrag, Vertrag Datum Von
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public DateTime? AngVon
        {
            get;
            set;
        }

        /// <summary>
        /// Angebot, Antrag, Vertrag Datum Bis
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public DateTime? AngBis
        {
            get;
            set;
        }


        #endregion
    }
}