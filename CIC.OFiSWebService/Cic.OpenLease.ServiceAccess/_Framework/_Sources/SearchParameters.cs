// OWNER MK, 26-02-2009
namespace Cic.OpenLease.ServiceAccess
{
    /// <summary>
    /// Eingabeparameter für verschiedene Such-Methoden, zur Bestimmung des Bereiches der Datensätze, welcher zurückgegeben werden soll. 
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class SearchParameters
    {
        #region Constructors
        public SearchParameters(int skip, int top)
        {
            Skip = skip;
            Top = top;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gibt an, wieviele Sätze übersprungen werden sollen.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int Skip
        {
            get;
            set;
        }

        /// <summary>
		/// Gibt an, wieviele Sätze geliefert werden sollen.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int Top
        {
            get;
            set;
        }
        #endregion
    }
}
