// OWNER: BK, 12-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    /// <summary>
    /// Requirement Check Component Expression
    /// </summary>
	[System.CLSCompliant(true)]
	public sealed class RequirementCheckComponentExpression : Cic.OpenOne.CarConfigurator.BO.DataProviderService.CheckComponentExpression
	{
		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		public RequirementCheckComponentExpression()
			: base()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requirementCheckComponentExpression"></param>
        /// <todo>BK 0 BK, Not tested</todo>
		internal RequirementCheckComponentExpression(Cic.P000001.Common.DataProvider.RequirementCheckComponentExpression requirementCheckComponentExpression)
			: base(requirementCheckComponentExpression)
		{
		}
		#endregion
	}
}
