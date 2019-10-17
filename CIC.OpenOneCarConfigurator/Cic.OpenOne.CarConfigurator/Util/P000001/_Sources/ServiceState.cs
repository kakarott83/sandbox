// OWNER: BK, 15-04-2008
namespace Cic.P000001.Common
{
	[System.CLSCompliant(true)]
    [System.Serializable]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
	public sealed class ServiceState : Cic.P000001.Common.IServiceState
	{
		#region Private variables
		private bool _IsServiceable;
		private string _Message;
		private long _ProcessingTime;
		#endregion

		#region Constructors
        public ServiceState()
        {
        }
		// TESTEDBY ServiceStateTestFixture.ConstructWithInvalidProcessingTime
		// TESTEDBY ServiceStateTestFixture.CheckProperties
		public ServiceState(bool isServiceable, string message, long processingTime)
		{
			// Check processing time
			if (processingTime < 0)
			{
				// Throw exception
				throw  new System.ArgumentException("processingTime");
			}

			// Set values
			_IsServiceable = isServiceable;
			_Message = message;
			_ProcessingTime = processingTime;
		}
		#endregion

		#region IServiceStateBase properties
        [System.Runtime.Serialization.DataMember]
		public bool IsServiceable
		{
			// TESTEDBY ServiceStateTestFixture.CheckProperties
			get
			{
				// Return
				return _IsServiceable;
			}
            set
            {
                _IsServiceable = value;
            }
		}
        [System.Runtime.Serialization.DataMember]
		public string Message
		{
			// TESTEDBY ServiceStateTestFixture.CheckProperties
			get
			{
				// Return
				return _Message;
			}
            set
            {
                _Message = value;
            }
		}
        [System.Runtime.Serialization.DataMember]
		public long ProcessingTime
		{
			// TESTEDBY ServiceStateTestFixture.CheckProperties
			get
			{
				// Return
				return _ProcessingTime;
			}
            set
            {
                _ProcessingTime = value;
            }
		}
		#endregion
	}
}
