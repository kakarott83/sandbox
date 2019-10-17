// OWNER: BK, 12-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    /// <summary>
    /// Conflict Check Component Expression
    /// </summary>
	[System.CLSCompliant(true)]
	public sealed class ConflictCheckComponentExpression : Cic.OpenOne.CarConfigurator.BO.DataProviderService.CheckComponentExpression
	{
		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		public ConflictCheckComponentExpression()
			: base()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
        /// <param name="conflictCheckComponentExpression"></param>
		internal ConflictCheckComponentExpression(Cic.P000001.Common.DataProvider.ConflictCheckComponentExpression conflictCheckComponentExpression)
			: base(conflictCheckComponentExpression)
		{
		}
		#endregion
	}
}
