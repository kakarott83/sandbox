// OWNER: BK, 12-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    /// <summary>
    /// Discount Check Component Expression
    /// </summary>
	[System.CLSCompliant(true)]
	public sealed class DiscountCheckComponentExpression : Cic.OpenOne.CarConfigurator.BO.DataProviderService.CheckComponentExpression
	{
		#region Private variables
		private double _Discount;
		#endregion

		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		public DiscountCheckComponentExpression()
			: base()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="discountCheckComponentExpression"></param>
        /// <todo>BK 0 BK, Not tested</todo>
		internal DiscountCheckComponentExpression(Cic.P000001.Common.DataProvider.DiscountCheckComponentExpression discountCheckComponentExpression)
			: base(discountCheckComponentExpression)
		{
			// Set values
			_Discount = discountCheckComponentExpression.Value;
		}
		#endregion

		#region Properties
        /// <summary>
        /// Discount
        /// </summary>
		public double Discount
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _Discount;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_Discount = value;
			}
		}
		#endregion
	}
}
