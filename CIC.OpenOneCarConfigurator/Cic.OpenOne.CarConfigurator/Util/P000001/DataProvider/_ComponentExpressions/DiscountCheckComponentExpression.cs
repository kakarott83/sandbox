// OWNER: BK, 12-06-2008
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public sealed class DiscountCheckComponentExpression : Cic.P000001.Common.DataProvider.GenericCheckComponentExpression<double>
	{
		#region Constructors
		// TESTEDBY DiscountCheckComponentExpressionTestFixture.ConstructWithoutExpression
		// TESTEDBY DiscountCheckComponentExpressionTestFixture.CheckProperties
		public DiscountCheckComponentExpression(double discount, Cic.OpenOne.CarConfigurator.Util.Expressions.Expression<string> expression)
			: base(Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants.Discount, discount, expression)
		{
		}
		#endregion
	}
}
