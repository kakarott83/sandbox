using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.ExpressionParser
{
    // [System.CLSCompliant(true)]
    internal class Lexemizer
    {
        #region Private variables
        private System.IO.TextReader _ExpressionTextReader;
        private char _CurrentChar;
        #endregion

        #region Constructors
        internal Lexemizer(System.IO.TextReader expressionReader)
        {
            if (expressionReader == null)
            {
                throw new ParserException("ExpressionReader is null");
            }

            _ExpressionTextReader = expressionReader;

            MyReadNextCharacter();
        }
        #endregion

        #region Methods
        internal Lexem ReadNextLexem()
        {
            MySkipWhiteSprace();

            if (MyIsEndOfExpression())
            {
                return null;
            }

            // Read JATO Id
            if (_CurrentChar.Equals('{'))
            {
                return MyReadId();
            }

            // It is a letter so it schould by AND, OR or NOT
            if (char.IsLetter(_CurrentChar))
            {
                return MyReadBoolExpression();
            }

            // if not it is most likely parentheisi
            return MyReadParenthesis();
        }
        #endregion

        #region My methods
        private void MyReadNextCharacter()
        {
            int Character;

            try
            {
                Character = _ExpressionTextReader.Read();

                if (Character > 0)
                {
                    // set next character
                    _CurrentChar = (char)Character;
                }
                else
                {
                    // end of expression
                    _CurrentChar = char.MinValue;
                }
            }
            catch (System.Exception e)
            {
                throw new ParserException("Could not read from ExpressionReader", e);
            }
        }

        // Skip space, tab and other white spaces
        private void MySkipWhiteSprace()
        {
            if (char.IsWhiteSpace(_CurrentChar))
            {
                MyReadNextCharacter();
            }
        }

        // Are we at the end of expression
        private bool MyIsEndOfExpression()
        {
            if (_CurrentChar == char.MinValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // reade JatoId - for example {101} 
        private Lexem MyReadId()
        {
            string Id = string.Empty;

            do
            {
                MyReadNextCharacter();

                MySkipWhiteSprace();

                if (char.IsNumber(_CurrentChar))
                {
                    Id += _CurrentChar;
                }

            } while (!_CurrentChar.Equals('}'));

            // set "cursor" to next character
            MyReadNextCharacter();

            if (!string.IsNullOrEmpty(Id))
            {
                // create id lexem
                return new Lexem(JatoLexemConstants.Id, Id);
            }
            else
            {
                // is empty return null
                return null;
            }

        }

        // read boolean Jato expression - it could only by a AND, OR or NOT
        private Lexem MyReadBoolExpression()
        {
            string Symbol = string.Empty;

            do
            {
                Symbol += _CurrentChar;

                MyReadNextCharacter();

            } while (char.IsLetter(_CurrentChar));

            switch (Symbol.ToUpper())
            {
                case "AND" :
                    return new Lexem(JatoLexemConstants.Bool, "AND");
                case "OR" :
                    return new Lexem(JatoLexemConstants.Bool, "OR");
                case "NOT":
                    return new Lexem(JatoLexemConstants.Bool, "NOT");
                default:
                    return null;
            }
        }

        // read parentheisis
        private Lexem MyReadParenthesis()
        {
            switch (_CurrentChar)
            {
                case '(' :
                    MyReadNextCharacter();
                    return new Lexem(JatoLexemConstants.Parenthesis, "(");
                case ')' :
                    MyReadNextCharacter();
                    return new Lexem(JatoLexemConstants.Parenthesis, ")");
                default :
                    return null;
            }
        }
        #endregion
    }
}
