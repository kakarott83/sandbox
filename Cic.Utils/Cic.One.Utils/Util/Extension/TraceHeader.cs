
namespace Cic.OpenOne.Common.Util.Extension
{
    /// <summary>
    /// Used for time tracing in the response
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class TraceHeader
    {
        #region Constructors

        /// <summary>
        /// TraceHeader-Konstruktor
        /// </summary>
        /// <param name="duration"></param>
        public TraceHeader(double duration)
        {
            this.duration = duration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// duration property
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public double duration
        {
            get;
            set;
        }

        #endregion
    }
}