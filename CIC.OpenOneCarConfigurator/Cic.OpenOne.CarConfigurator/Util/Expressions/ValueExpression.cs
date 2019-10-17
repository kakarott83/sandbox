namespace Cic.OpenOne.CarConfigurator.Util.Expressions
{
    [System.CLSCompliant(true)]
    // Represents the value expression
    // For example: Represents the id {101}
	public sealed class ValueExpression<T> : Expression<T>
    {
		#region Private constants
		private const string CnstToStringPrefix = "Value: ";
		#endregion

		#region Private variables
		private T _Value;
        #endregion

        #region Constructors
        public ValueExpression(T value)
        {
            if (value == null)
            {
                throw new System.ArgumentNullException("value");
            }

            _Value = value;
        }
        #endregion

		#region Methods
		public override string[] ToLines()
		{
			// Return
			return new string[1] { (CnstToStringPrefix + _Value.ToString()) };
		}

		public override bool ToBoolean(System.IFormatProvider formatProvider)
		{
			bool Result = false;

			try
			{
				Result = ConvertHelper.ToBoolean(_Value, formatProvider);
			}
			catch
			{
				// Ignore xecpetion
			}

			return Result;
		}
		#endregion

        #region Properties
        public T Value
        {
            get { return _Value; }
        }
        #endregion
    }
}
