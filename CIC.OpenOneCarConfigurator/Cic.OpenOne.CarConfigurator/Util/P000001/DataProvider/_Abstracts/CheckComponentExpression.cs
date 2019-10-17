// OWNER: BK, 12-06-2008
using Cic.OpenOne.CarConfigurator.Util.Expressions;
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public abstract class CheckComponentExpression
	{
		#region Private variables
		private Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants _CheckComponentExpressionConstant;
		private object _Value;
		private Expression<string> _Expression;
		#endregion

		#region Constructors
		// TESTEDBY CheckComponentExpressionTestFixture.ConstructWithInvalidCheckComponentExpressionConstant
		// TESTEDBY CheckComponentExpressionTestFixture.ConstructWithoutExpression
		// TESTEDBY CheckComponentExpressionTestFixture.ConstructWithValidValues
		internal CheckComponentExpression(Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants checkComponentExpressionConstant, object value, Cic.OpenOne.CarConfigurator.Util.Expressions.Expression<string> expression)
		{
			// Check check component expression constant
			if (!System.Enum.IsDefined(typeof(Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants), checkComponentExpressionConstant))
			{
				// Throw exception
				throw  new System.ArgumentException("checkComponentExpressionConstant");
			}

			// Check expression
			if (expression == null)
			{
				// Throw exception
				throw new System.ArgumentException("expression");
			}

			// Set values
			_CheckComponentExpressionConstant = checkComponentExpressionConstant;
			_Value = value;
			_Expression = expression;
		}
		#endregion

		#region Properties
		public Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants CheckComponentExpressionConstant
		{
			// TESTEDBY CheckComponentExpressionTestFixture.ConstructWithValidValues
			get { return _CheckComponentExpressionConstant; }
		}

		public object Value
		{
			// TESTEDBY CheckComponentExpressionTestFixture.ConstructWithValidValues
			get { return _Value; }
		}

		public Cic.OpenOne.CarConfigurator.Util.Expressions.Expression<string> Expression
		{
			// TESTEDBY CheckComponentExpressionTestFixture.ConstructWithValidValues
			get { return _Expression; }
		}
		#endregion
	}
}
