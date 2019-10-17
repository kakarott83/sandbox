namespace Cic.OpenOne.CarConfigurator.Util.Expressions
{
	[System.CLSCompliant(true)]
	// Represents the OR expression
	// For example: {101} OR {30}
	public sealed class OrExpression<T> : Expression<T>
	{
		#region Private constants
		private const string CnstToStringPrefix = "OR";
		#endregion

		#region Private variables
		private Expression<T> _LeftExpression;
		private Expression<T> _RightExpression;
		#endregion

		#region Constructors
		public OrExpression(Expression<T> leftExpression, Expression<T> rightExpression)
			: base()
		{
			// Check left expression
			if (leftExpression == null)
			{
				// Throw exception
                throw new System.ArgumentNullException("leftExpression");
			}

			// Check right expression
			if (rightExpression == null)
			{
				// Throw exception
                throw new System.ArgumentNullException("rightExpression");
			}

			// Set values
			_LeftExpression = leftExpression;
			_RightExpression = rightExpression;
		}
		#endregion

		#region Methods
		public override string[] ToLines()
		{
			string[] LeftLines;
			string[] RightLines;
			string[] NewLines = null;
			int Count;
			int Index;

			// Reset count
			Count = 0;
			// Get left lines
			LeftLines = LeftExpression.ToLines();
			// Check left lines
			if ((LeftLines != null) && (LeftLines.GetLength(0) > 0))
			{
				// Increase count
				Count = (Count + LeftLines.GetLength(0));
			}
			// Get right lines
			RightLines = RightExpression.ToLines();
			// Check right lines
			if ((RightLines != null) && (RightLines.GetLength(0) > 0))
			{
				// Increase count
				Count = (Count + RightLines.GetLength(0));
			}
			// Resize
			System.Array.Resize<string>(ref NewLines, (Count + 1));
			// Reset index
			Index = 0;
			// Add new line
			NewLines[Index] = CnstToStringPrefix;
			// Check left lines
			if ((LeftLines != null) && (LeftLines.GetLength(0) > 0))
			{
				// Loop through left lines
				foreach (string LoopLeftLine in LeftLines)
				{
					// Increase index
					Index++;
					// Add line with indent
					NewLines[Index] = (Expression<T>.LineIndent + LoopLeftLine);
				}
			}
			// Check right lines
			if ((RightLines != null) && (RightLines.GetLength(0) > 0))
			{
				// Loop through right lines
				foreach (string LoopRightLine in RightLines)
				{
					// Increase index
					Index++;
					// Add line with indent
					NewLines[Index] = (Expression<T>.LineIndent + LoopRightLine);
				}
			}

			// Return
			return NewLines;
		}

		public override bool ToBoolean(System.IFormatProvider formatProvider)
		{
			return (_LeftExpression.ToBoolean(formatProvider) | _RightExpression.ToBoolean(formatProvider));
		}
		#endregion

		#region Properties
		public Expression<T> LeftExpression
		{
			get { return _LeftExpression; }
		}

		public Expression<T> RightExpression
		{
			get { return _RightExpression; }
		}
		#endregion
	}
}
