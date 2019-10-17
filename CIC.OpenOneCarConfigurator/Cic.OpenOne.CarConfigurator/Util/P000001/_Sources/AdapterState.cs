// OWNER: BK, 15-04-2008
namespace Cic.P000001.Common
{
    [System.Serializable]
	[System.CLSCompliant(true)]
	public sealed class AdapterState : Cic.P000001.Common.IAdapterState
	{
		#region Private variables
		private string _AdapterName;
		private bool _IsServiceable;
		private string _Message;
		#endregion

		#region Constructors
        public AdapterState()
        {
        }
		// TESTEDBY AdapterStateTestFixture.ConstructWithoutAdapterName
		// TESTEDBY AdapterStateTestFixture.ConstructWithEmptyAdapterName
		// TESTEDBY AdapterStateTestFixture.ConstructWithSpaceAdapterName
		// TESTEDBY AdapterStateTestFixture.CheckProperties
		public AdapterState(string adapterName, bool isServiceable, string message)
		{
			// Check adapter name
			if (Cic.OpenOne.Util.StringHelper.IsTrimedNullOrEmpty(adapterName))
			{
				// Throw exception
				throw  new System.ArgumentException("adapterName");
			}

			// Set values
			_AdapterName = adapterName.Trim();
			_IsServiceable = isServiceable;
			_Message = message;
		}
		#endregion

		#region IAdapterStateBase properties
		public string AdapterName
		{
			// TESTEDBY AdapterStateTestFixture.CheckProperties
			get
			{
				// Return
				return _AdapterName;
			}
		}

		public bool IsServiceable
		{
			// TESTEDBY AdapterStateTestFixture.CheckProperties
			get
			{
				// Return
				return _IsServiceable;
			}
		}

		public string Message
		{
			// TESTEDBY AdapterStateTestFixture.CheckProperties
			get
			{
				// Return
				return _Message;
			}
		}
		#endregion
	}
}
