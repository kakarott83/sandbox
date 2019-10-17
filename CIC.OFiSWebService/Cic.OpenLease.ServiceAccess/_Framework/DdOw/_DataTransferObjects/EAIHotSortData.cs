// OWNER JJ, 01-09-2009
namespace Cic.OpenLease.ServiceAccess.DdOw
{
    /// <summary>
    /// Suchwerte für Interessenten
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class EAIHotSortData
    {
        #region Enums
        public enum SortDataConstants : int
        {
            INPUTPARAMETER4 = 1,
            FINISHDATE,
            FILEFLAGOUT,
            INPUTPARAMETER1,
            INPUTPARAMETER2,
            INPUTPARAMETER3,
            INPUTPARAMETER5,
            OUTPUTPARAMETER1,
            OUTPUTPARAMETER2,
            OUTPUTPARAMETER3,
            OUTPUTPARAMETER4,
            OUTPUTPARAMETER5
        }
        #endregion

        #region Constructors
        public EAIHotSortData(SortDataConstants sortDataConstant, SortOrderConstants sortOrderConstant)
        {
            this.SortDataConstant = sortDataConstant;
            this.SortOrderConstant = sortOrderConstant;
        }
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