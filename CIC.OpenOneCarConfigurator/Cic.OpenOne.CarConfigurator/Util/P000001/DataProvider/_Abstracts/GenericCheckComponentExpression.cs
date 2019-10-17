// OWNER: BK, 12-06-2008
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public abstract class GenericCheckComponentExpression<T> : Cic.P000001.Common.DataProvider.CheckComponentExpression
	{
		#region Constructors
		// TESTEDBY GenericCheckComponentExpressionTestFixture.ConstructWithInvalidCheckComponentExpressionConstant
		// TESTEDBY GenericCheckComponentExpressionTestFixture.ConstructWithoutExpression
		// TESTEDBY GenericCheckComponentExpressionTestFixture.ConstructWithValidValues
		internal GenericCheckComponentExpression(Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants checkComponentExpressionConstant, T value, Cic.OpenOne.CarConfigurator.Util.Expressions.Expression<string> expression)
			: base(checkComponentExpressionConstant, value, expression)
		{
		}
		#endregion

		#region Properties
		public new T Value
		{
			// TESTEDBY CheckComponentExpressionTestFixture.ConstructWithValidValues
			get { return (T)base.Value; }
		}
		#endregion
	}
}
