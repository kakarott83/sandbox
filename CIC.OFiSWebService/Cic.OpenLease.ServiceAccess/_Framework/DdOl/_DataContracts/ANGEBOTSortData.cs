// OWNER BK, 01-09-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Suchwerte für Angebote
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANGEBOTSortData
    {
        #region Enums
        public enum SortDataConstants : int
        {
            ANGEBOT = 1,
            ZUSTAND,
            ERFASSUNG,
            BEGINN,
            ENDE,
            //ITNAME,
            //ITVORNAME,
            //ITMATCHCODE,
            //ITPRIVATFLAG,
        }
        #endregion

        #region Constructors
        public ANGEBOTSortData(SortDataConstants sortDataConstant, SortOrderConstants sortOrderConstant)
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
