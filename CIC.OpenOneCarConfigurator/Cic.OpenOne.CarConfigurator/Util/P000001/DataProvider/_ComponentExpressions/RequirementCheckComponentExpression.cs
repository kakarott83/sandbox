// OWNER: BK, 12-06-2008

using Cic.OpenOne.CarConfigurator.Util.Expressions;
namespace Cic.P000001.Common.DataProvider
{
	[System.CLSCompliant(true)]
	public sealed class RequirementCheckComponentExpression : Cic.P000001.Common.DataProvider.GenericCheckComponentExpression<object>
	{
		#region Constructors
		// TESTEDBY RequirementCheckComponentExpressionTestFixture.ConstructWithoutExpression
		// TESTEDBY RequirementCheckComponentExpressionTestFixture.CheckProperties
		public RequirementCheckComponentExpression(Expression<string> expression)
			: base(Cic.P000001.Common.DataProvider.CheckComponentExpressionConstants.Requirement, null, expression)
		{
		}
		#endregion
	}
}
