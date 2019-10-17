// OWNER BK, 31-01-2009
namespace Cic.OpenLease.Model.DdOw
{
	#region Using directives
	using System.Linq;
    using Cic.OpenOne.Common.Util;
	#endregion

	[System.CLSCompliant(true)]
	public static class WorkflowAreaHelper
	{
		#region Methods
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithoutDefaultTestFixture.CheckWithNullValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithoutDefaultTestFixture.CheckWithEmptyValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithoutDefaultTestFixture.CheckWithSpaceCharsValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithoutDefaultTestFixture.CheckWithInvalidValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithoutDefaultTestFixture.CheckWithValidValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithoutDefaultTestFixture.CheckWithValidValueAndSpaceChars
		public static Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants? ParseToWorkflowAreaCodeConstant(string value)
		{
			// Return
			return MyParseToWorkflowAreaCodeConstant(value);
		}

		// TESTEDBY ParseToWorkflowAreaCodeConstantWithDefaultTestFixture.CheckWithNullValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithDefaultTestFixture.CheckWithEmptyValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithDefaultTestFixture.CheckWithSpaceCharsValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithDefaultTestFixture.CheckWithInvalidValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithDefaultTestFixture.CheckWithValidValue
		// TESTEDBY ParseToWorkflowAreaCodeConstantWithDefaultTestFixture.CheckWithValidValueAndSpaceChars
		public static Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants ParseToWorkflowAreaCodeConstant(string value, Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants defaultWorkflowAreaCodeConstant)
		{
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants? WorkflowAreaCodeConstant = null;

			// Parse
			WorkflowAreaCodeConstant = MyParseToWorkflowAreaCodeConstant(value);
			// Check
			if (WorkflowAreaCodeConstant == null)
			{
				// Set value
				WorkflowAreaCodeConstant = defaultWorkflowAreaCodeConstant;
			}

			// Return
			return (Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants)WorkflowAreaCodeConstant;
		}
		#endregion

		#region My methods
		private static Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants? MyParseToWorkflowAreaCodeConstant(string value)
		{
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants? WorkflowAreaCodeConstant = null;

			// Check value
			if (!StringUtil.IsTrimedNullOrEmpty(value))
			{
				// Trim value
				value = value.Trim();

				try
				{
					// Parse
					WorkflowAreaCodeConstant = (Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants)System.Enum.Parse(typeof(Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants), value);
				}
				catch
				{
					// Ignore exception
				}

				// Check
				if (WorkflowAreaCodeConstant != null)
				{
					// Check defined
					if (!System.Enum.IsDefined(typeof(Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants), WorkflowAreaCodeConstant))
					{
						// Set null
						WorkflowAreaCodeConstant = null;
					}
				}
			}

			// Return
			return WorkflowAreaCodeConstant;
		}
		#endregion
	}
}
