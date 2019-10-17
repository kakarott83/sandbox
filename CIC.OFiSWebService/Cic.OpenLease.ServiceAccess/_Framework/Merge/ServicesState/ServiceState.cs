// OWNER: BK, 29-04-2008
namespace Cic.OpenLease.ServiceAccess.Merge.ServicesState
{
    /// <summary>
    /// Services Status Objekt. 
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class ServiceState
    {
        #region Private variables
        private bool _IsServiceable;
        private string _Message;
        private long _ProcessingTime;
        #endregion

        #region Contructors
        
        // A constructor without parameters is needed for serialization
        public ServiceState()
        {
        }

        
        public ServiceState(bool isServiceable, string message, long processingTime)
        {
            // Set values
            _IsServiceable = isServiceable;
            _Message = message;
            _ProcessingTime = processingTime;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Bestimmt ob die Services allgemein Funktionstüchtig sind. Wenn true sind die Services Funktionstüchtig. Wenn false funktionieren die Services nicht (Einzelheiten warum die nicht funktionieren sind in Message zu finden).
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool IsServiceable
        {
            
            get
            {
                // Return
                return _IsServiceable;
            }
            set
            {
                // Set value
                _IsServiceable = value;
            }
        }

        /// <summary>
        /// Wenn die Services nicht funktionstüchtig sind beinhaltet diese Eigenschaft die Information warum.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string Message
        {
            
            get
            {
                // Return
                return _Message;
            }
            set
            {
                // Set value
                _Message = value;
            }
        }

        /// <summary>
        /// Ermittelt die Abfrage Zeit [ms].
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long ProcessingTime
        {
            
            get
            {
                // Return
                return _ProcessingTime;
            }
            set
            {
                // Set value
                _ProcessingTime = value;
            }
        }
        #endregion
    }
}
