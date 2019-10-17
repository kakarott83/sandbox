namespace Cic.OpenOne.CarConfigurator.Util.Expressions
{
	[System.CLSCompliant(true)]
	// Represents the NOT expression
	// For example: NOT {101}
	public sealed class NotExpression<T> : Expression<T>
	{
		#region Private constants
		private const string CnstToStringPrefix = "NOT";
		#endregion

		#region Private variables
		private Expression<T> _InnerExpression;
		#endregion

		#region Constructors
		public NotExpression(Expression<T> innerExpression)
		{
			// Check inner expression
			if (innerExpression == null)
			{
				// Throw exception
                throw new System.ArgumentNullException("innerExpression");
			}

			// Set value
			_InnerExpression = innerExpression;
		}
		#endregion

		#region Methods
		public override string[] ToLines()
		{
			string[] InnerLines;
			string[] NewLines = null;
			int Count;
			int Index;

			// Reset count
			Count = 0;
			// Get inner lines
			InnerLines = InnerExpression.ToLines();
			// Check inner lines
			if ((InnerLines != null) && (InnerLines.GetLength(0) > 0))
			{
				// Increase count
				Count = (Count + InnerLines.GetLength(0));
			}
			// Resize
			System.Array.Resize<string>(ref NewLines, (Count + 1));
			// Reset index
			Index = 0;
			// Add new line
			NewLines[Index] = CnstToStringPrefix;
			// Check inner lines
			if ((InnerLines != null) && (InnerLines.GetLength(0) > 0))
			{
				// Loop through inner lines
				foreach (string LoopInnerLine in InnerLines)
				{
					// Increase index
					Index++;
					// Add line with indent
					NewLines[Index] = (Expression<T>.LineIndent + LoopInnerLine);
				}
			}

			// Return
			return NewLines;
		}

		public override bool ToBoolean(System.IFormatProvider formatProvider)
		{
			return !_InnerExpression.ToBoolean(formatProvider);
		}
		#endregion

		#region Properties
		public Expression<T> InnerExpression
		{
			get { return _InnerExpression; }
		}
		#endregion
	}
}
