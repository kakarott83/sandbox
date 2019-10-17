using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.ExpressionParser
{
    // [System.CLSCompliant(true)]
    internal class Lexem 
    {
        #region Private variables
        private JatoLexemConstants _TokenType;
        private string _Value;
        #endregion

        #region Properties
        public JatoLexemConstants LexemType
        {
            get { return _TokenType; }
            set { _TokenType = value; }
        }

        public string Value
        {
            get { return this._Value; }
            set { this._Value = value; }
        }
        #endregion

        #region Constructors
        internal Lexem(JatoLexemConstants lexemType, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ParserException("Lexar value argument empty");
            }

            LexemType = lexemType;
            Value = value;
        }

        internal bool Equals(JatoLexemConstants tokenType, string value)
        {
            return _TokenType == tokenType && _Value.Equals(value);
        }
        #endregion
    }
}
