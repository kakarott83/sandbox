// OWNER: BK, 25-08-2008
namespace Cic.OpenOne.CarConfigurator.BO.DataProviderService
{
    /// <summary>
    /// Check Component Result Interface
    /// </summary>
	[System.CLSCompliant(true)]
	public interface ICheckComponentResult : Cic.P000001.Common.DataProvider.ICheckComponentResultBase
	{
		#region Properties
        /// <summary>
        /// Conflict Check Component Expressions
        /// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.OpenOne.CarConfigurator.BO.DataProviderService.ConflictCheckComponentExpression[] ConflictCheckComponentExpressions
		{
			// TODO BK 0 BK, Not tested
			get;
		}

        /// <summary>
        /// Requirement Check Component Expressions
        /// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.OpenOne.CarConfigurator.BO.DataProviderService.RequirementCheckComponentExpression[] RequirementCheckComponentExpressions
		{
			// TODO BK 0 BK, Not tested
			get;
		}

        /// <summary>
        /// New Price Check Component Expressions
        /// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.OpenOne.CarConfigurator.BO.DataProviderService.NewPriceCheckComponentExpression[] NewPriceCheckComponentExpressions
		{
			// TODO BK 0 BK, Not tested
			get;
		}

        /// <summary>
        /// Discount Check Component Expressions
        /// </summary>
        /// <note>
        /// BK
        /// Sure the return of an array is realy bad, but this interface is also defined for webservice usage, 
        /// so the property cannnot be replaced by a method and cannot use a collection data type. 
        /// If you know a better way, please tell me.
        /// </note>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		Cic.OpenOne.CarConfigurator.BO.DataProviderService.DiscountCheckComponentExpression[] DiscountCheckComponentExpressions
		{
			// TODO BK 0 BK, Not tested
			get;
		}
		#endregion
	}
}
