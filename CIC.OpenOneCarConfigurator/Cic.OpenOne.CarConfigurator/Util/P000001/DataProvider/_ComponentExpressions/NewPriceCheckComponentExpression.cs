// OWNER: BK, 12-06-2008
using Cic.OpenOne.CarConfigurator.Util.Expressions;
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public sealed class NewPriceCheckComponentExpression : Cic.P000001.Common.DataProvider.GenericCheckComponentExpression<double>
	{
		#region Constructors
		// TESTEDBY NewPriceCheckComponentExpressionTestFixture.ConstructWithoutExpression
		// TESTEDBY NewPriceCheckComponentExpressionTestFixture.CheckProperties
		public NewPriceCheckComponentExpression(double newPrice, Expression<string> expression)
			: base(Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants.NewPrice, newPrice, expression)
		{
		}
		#endregion
	}
}
