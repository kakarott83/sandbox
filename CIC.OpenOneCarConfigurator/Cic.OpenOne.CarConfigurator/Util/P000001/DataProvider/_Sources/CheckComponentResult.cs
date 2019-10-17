// OWNER: BK, 09-04-2008
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public sealed class CheckComponentResult : Cic.P000001.Common.DataProvider.ICheckComponentResult
	{
		#region Private variables
		private Cic.P000001.Common.DataProvider.CheckComponentResultConstants _CheckComponentResultConstant;
		private Cic.P000001.Common.DataProvider.CheckComponentExpression[] _CheckComponentExpressions;
		private string _Message;
		#endregion

		#region Constructors
		// TESTEDBY CheckComponentResultTestFixture.ConstructWithInvalidCheckComponentResultConstants
		// TESTEDBY CheckComponentResultTestFixture.CheckProperties
		public CheckComponentResult(Cic.P000001.Common.DataProvider.CheckComponentResultConstants checkComponentResultConstant, Cic.P000001.Common.DataProvider.CheckComponentExpression[] checkComponentExpressions, string message)
		{
			// Check check component result constant
			if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.DataProvider.CheckComponentResultConstants), checkComponentResultConstant))
			{
				// Throw exception
				throw  new System.ArgumentException("checkComponentResultConstant");
			}

			// Set values
			_CheckComponentResultConstant = checkComponentResultConstant;
			_CheckComponentExpressions = checkComponentExpressions;
			_Message = message;
		}
		#endregion

		#region ICheckComponentResultBase properties
		public Cic.P000001.Common.DataProvider.CheckComponentResultConstants CheckComponentResultConstant
		{
			// TESTEDBY CheckComponentResultTestFixture.CheckProperties
			get
			{
				// Return
				return _CheckComponentResultConstant;
			}
		}

		public string Message
		{
			// TESTEDBY CheckComponentResultTestFixture.CheckProperties
			get
			{
				// Return
				return _Message;
			}
		}
		#endregion

		#region ICheckComponentResult properties
		public Cic.P000001.Common.DataProvider.CheckComponentExpression[] CheckComponentExpressions
		{
			// TESTEDBY CheckComponentResultTestFixture.CheckProperties
			get
			{
				// Return
				return _CheckComponentExpressions;
			}
		}
		#endregion
	}
}
