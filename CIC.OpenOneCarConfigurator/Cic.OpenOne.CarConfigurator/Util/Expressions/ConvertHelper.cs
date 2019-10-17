// OWNER BK, 12-06-2008
namespace Cic.OpenOne.CarConfigurator.Util.Expressions
{
	//[System.CLSCompliant(true)]
    /// <summary>
    /// Provide additional methods for Converting
    /// </summary>
	internal static class ConvertHelper
	{
		#region Methods
		// TEST BK 0 BK, Not tested
        /// <summary>
        /// Converts the value to boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>The converted boolean</returns>
		internal static bool ToBoolean(object value, System.IFormatProvider formatProvider)
		{
			System.Type Type;
			bool Result = false;
			bool ExceptionOccured = false;
			string ValueAsString;

			// Check value
			if (value != null)
			{
				try
				{
					// Convert
					Result = System.Convert.ToBoolean(value, formatProvider);
				}
				catch
				{
					// Set state
					ExceptionOccured = true;
				}

				// Check state
				if (ExceptionOccured)
				{
					// Get type
					Type = value.GetType();
					// Check type
					if (Type == typeof(string))
					{
						// Get value as string
						ValueAsString = ((string)value).Trim();
						// Get result
						Result = (ValueAsString == "1");
					}
				}
			}

			// Return
			return Result;
		}
		#endregion
	}
}
