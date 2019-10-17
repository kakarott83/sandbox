// OWNER: BK, 12-06-2008
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public sealed class ConflictCheckComponentExpression : Cic.P000001.Common.DataProvider.GenericCheckComponentExpression<object>
	{
		#region Constructors
		// TESTEDBY ConflictCheckComponentExpressionTestFixture.ConstructWithoutExpression
		// TESTEDBY ConflictCheckComponentExpressionTestFixture.CheckProperties
		public ConflictCheckComponentExpression(Cic.OpenOne.CarConfigurator.Util.Expressions.Expression<string> expression)
			: base(Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants.Conflict, null, expression)
		{
		}
		#endregion
	}
}
