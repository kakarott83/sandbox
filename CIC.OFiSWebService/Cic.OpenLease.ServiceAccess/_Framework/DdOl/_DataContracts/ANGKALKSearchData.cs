// OWNER MK, 26-08-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Suchwerte für Angebotkalkulation
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANGKALKSearchData
    {
        #region Methods
        /// <summary>
        /// Übernimmt die Werte einer anderen Instanz.
        /// </summary>
        public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.ANGKALKSearchData angKalkSearchData)
        {
            // Check object
            if (angKalkSearchData == null)
            {
                // Throw 
                throw new Exception("angebotSearchData");
            }

            // Set data
            this.SYSANGEBOT = angKalkSearchData.SYSANGEBOT;
        }

        // TEST BK 0 BK, Not tested 
        /// <summary>
        /// Setzt alle Werte zurück.
        /// </summary>
        public void ResetValues()
        {
            // Adopt
            this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.ANGKALKSearchData());
        }
        #endregion

        #region Properties
        /// <summary>
        /// Angebot ID
        /// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long? SYSANGEBOT
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }
        #endregion
    }
}
