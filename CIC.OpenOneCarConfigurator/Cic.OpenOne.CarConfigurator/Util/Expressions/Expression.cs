namespace Cic.OpenOne.CarConfigurator.Util.Expressions
{
    [System.CLSCompliant(true)]
    // Base Expression
    public abstract class Expression<T>
    {
		#region Private constants
		private const string CnstLineIndent = "  ";
		#endregion
		
		#region Methods
        public override string ToString()
        {
			System.Text.StringBuilder StringBuilder;
			string[] Lines;
			string Result = "";

			// Get lines
			Lines = this.ToLines();

			// Check lines
			if ((Lines != null) && (Lines.GetLength(0) > 0))
			{
				// New string builder
				StringBuilder = new System.Text.StringBuilder();
				// Loop through lines
				foreach (string LoopLine in Lines)
				{
					// Append line
					StringBuilder.Append(LoopLine);
					// Append new line
					StringBuilder.Append(System.Environment.NewLine);
				}
				// Get result
				Result = StringBuilder.ToString();
			}

			// Return
			return Result;
		}
        #endregion

		#region Abstract methods
		public abstract string[] ToLines();
		public abstract bool ToBoolean(System.IFormatProvider formatProvider);
		#endregion

		#region Properties
		internal static string LineIndent
		{
			get { return CnstLineIndent; }
		}
		#endregion
	}
}
