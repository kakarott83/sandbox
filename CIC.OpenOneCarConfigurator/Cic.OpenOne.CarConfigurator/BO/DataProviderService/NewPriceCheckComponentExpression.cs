// OWNER: BK, 12-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    /// <summary>
    /// New Price Check Component Expression
    /// </summary>
	[System.CLSCompliant(true)]
	public sealed class NewPriceCheckComponentExpression : Cic.OpenOne.CarConfigurator.BO.DataProviderService.CheckComponentExpression
	{
		#region Private variables
		private double _Price;
		#endregion

		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		public NewPriceCheckComponentExpression()
			: base()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newPriceCheckComponentExpression"></param>
        /// <todo>BK 0 BK, Not tested</todo>
		internal NewPriceCheckComponentExpression(Cic.P000001.Common.DataProvider.NewPriceCheckComponentExpression newPriceCheckComponentExpression)
			: base(newPriceCheckComponentExpression)
		{
			// Set values
			_Price = newPriceCheckComponentExpression.Value;
		}
		#endregion

		#region Properties
        /// <summary>
        /// Price
        /// </summary>
		public double Price
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _Price;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_Price = value;
			}
		}
		#endregion
	}
}
