// OWNER: BK, 31-01-2009
namespace CicTest.OpenLease.Model.DdOw.WorkflowAreaHelper
{
	[NUnit.Framework.TestFixture]
	[System.CLSCompliant(true)]
	public sealed class ParseToWorkflowAreaCodeConstantWithDefaultTestFixture
	{
		#region Methods
		[NUnit.Framework.Test]
		public void CheckWithNullValue()
		{
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Default;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Expected;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Result;
			string Value;

			// Set default
			Default = Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.PERSON;
			// Set value
			Value = null;
			// Set expected
			Expected = Default;
			// Parse
			Result = Cic.OpenLease.Model.DdOw.WorkflowAreaHelper.ParseToWorkflowAreaCodeConstant(Value, Default);

			// Assert
			NUnit.Framework.Assert.AreEqual(Expected, Result);
		}

		[NUnit.Framework.Test]
		public void CheckWithEmptyValue()
		{
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Default;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Expected;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Result;
			string Value;

			// Set default
			Default = Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.PERSON;
			// Set value
			Value = string.Empty;
			// Set expected
			Expected = Default;
			// Parse
			Result = Cic.OpenLease.Model.DdOw.WorkflowAreaHelper.ParseToWorkflowAreaCodeConstant(Value, Default);

			// Assert
			NUnit.Framework.Assert.AreEqual(Expected, Result);
		}

		[NUnit.Framework.Test]
		public void CheckWithSpaceCharsValue()
		{
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Default;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Expected;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Result;
			string Value;

			// Set default
			Default = Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.PERSON;
			// Set value
			Value = "  ";
			// Set expected
			Expected = Default;
			// Parse
			Result = Cic.OpenLease.Model.DdOw.WorkflowAreaHelper.ParseToWorkflowAreaCodeConstant(Value, Default);

			// Assert
			NUnit.Framework.Assert.AreEqual(Expected, Result);
		}

		[NUnit.Framework.Test]
		public void CheckWithInvalidValue()
		{
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Default;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Expected;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Result;
			string Value;

			// Set default
			Default = Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.PERSON;
			// Set value
			Value = "%";
			// Set expected
			Expected = Default;
			// Parse
			Result = Cic.OpenLease.Model.DdOw.WorkflowAreaHelper.ParseToWorkflowAreaCodeConstant(Value, Default);

			// Assert
			NUnit.Framework.Assert.AreEqual(Expected, Result);
		}

		[NUnit.Framework.Test]
		public void CheckWithValidValue()
		{
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Default;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Expected;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Result;
			string Value;

			// Set default
			Default = Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.PERSON;
			// Set value
			Value = Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.SYSTEM.ToString();
			// Set expected
			Expected = Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.SYSTEM;
			// Parse
			Result = Cic.OpenLease.Model.DdOw.WorkflowAreaHelper.ParseToWorkflowAreaCodeConstant(Value, Default);

			// Assert
			NUnit.Framework.Assert.AreEqual(Expected, Result);
		}

		[NUnit.Framework.Test]
		public void CheckWithValidValueAndSpaceChars()
		{
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Default;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Expected;
			Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants Result;
			string Value;

			// Set default
			Default = Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.PERSON;
			// Set value
			Value = (" " + Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.SYSTEM.ToString() + " ");
			// Set expected
			Expected = Cic.OpenLease.Model.DdOw.WorkflowAreaCodeConstants.SYSTEM;
			// Parse
			Result = Cic.OpenLease.Model.DdOw.WorkflowAreaHelper.ParseToWorkflowAreaCodeConstant(Value, Default);

			// Assert
			NUnit.Framework.Assert.AreEqual(Expected, Result);
		}
		#endregion

		#region My methods
		private void MyUseInstanceForCodeAnalyse()
		{
			// Use this
			this.GetType();
		}
		#endregion
	}
}
