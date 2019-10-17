// OWNER: BK, 12-06-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    /// <summary>
    /// Check Component Expression
    /// </summary>
	[System.CLSCompliant(true)]
	public abstract class CheckComponentExpression
	{
		#region Private variables
		private Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject[] _ExpressionDataTransferObjects;
		#endregion

		#region Constructors
        /// <summary>
        /// A constructor without parameters is needed for serialization
        /// </summary>
        /// <todo>BK 0 BK, Not tested</todo>
		protected CheckComponentExpression()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="checkComponentExpression"></param>
        ///<todo>BK 0 BK, Not tested</todo>
		internal CheckComponentExpression(Cic.P000001.Common.DataProvider.CheckComponentExpression checkComponentExpression)
		{
			// Check object
			if (checkComponentExpression == null)
			{
				// Throw exception
				throw new System.ArgumentNullException("checkComponentExpression");
			}

			try
			{
				// Set values
                _ExpressionDataTransferObjects = Cic.OpenOne.CarConfigurator.BO.DataProviderService.DataProviderCommonToWebConvertHelper.ConvertExpression(checkComponentExpression.Expression);
			}
			catch
			{
				// Throw caught exception
				throw;
			}
		}
		#endregion

		#region Properties
		// NOTE 
        /// <summary>
        /// Expression Data Transfer Objects
        /// </summary>
        /// <note>
        /// BK, Sure te return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public Cic.OpenOne.CarConfigurator.BO.DataProviderService.ExpressionDataTransferObject[] ExpressionDataTransferObjects
		{
			// TODO BK 0 BK, Not tested
			get
			{
				// Return
				return _ExpressionDataTransferObjects;
			}
			// TODO BK 0 BK, Not tested
			set
			{
				// Set value
				_ExpressionDataTransferObjects = value;
			}
		}
		#endregion
	}
}
