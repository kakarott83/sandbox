// OWNER BK, 01-09-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Suchwerte für Antraege
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANTRAGSortData
    {
        #region Enums
        public enum SortDataConstants : int
        {
            ANTRAG = 1,
            ZUSTAND,
            ERFASSUNG,
            BEGINN,
            ENDE,
            //ITNAME,
            //ITVORNAME,
            //ITMATCHCODE,
            //ITPRIVATFLAG,
            PERSONNAME,
            PERSONVORNAME,
            PERSONCODE,
            PERSONPRIVATFLAG,
        }
        #endregion

        #region Constructors
        public ANTRAGSortData(SortDataConstants sortDataConstant, SortOrderConstants sortOrderConstant)
        {
            this.SortDataConstant = sortDataConstant;
            this.SortOrderConstant = sortOrderConstant;
        }
        #endregion

        #region Methods

        #endregion

        #region Properties
        /// <summary>
        /// Sorttierunsfeld.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public SortDataConstants SortDataConstant
        {
            get;
            set;
        }

        /// <summary>
        /// Sorttierreihenfolge .
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public SortOrderConstants SortOrderConstant
        {
            get;
            set;
        }
        #endregion
    }
}
