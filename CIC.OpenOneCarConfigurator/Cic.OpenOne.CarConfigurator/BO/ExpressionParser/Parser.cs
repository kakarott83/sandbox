using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
using Cic.OpenOne.CarConfigurator.Util.Expressions;


namespace Cic.OpenOne.CarConfigurator.BO.ExpressionParser
{
    // [System.CLSCompliant(true)]
    public class Parser
    {
        #region Private variables
        private Lexem _Lexem;
        private Lexemizer _Lexemizer;
        #endregion

        #region Constructors
        public Parser(System.IO.TextReader expression)
        {
            // Create lexemizer
            _Lexemizer = new Lexemizer(expression);

            // Move to next lexem
            _Lexem = _Lexemizer.ReadNextLexem();
        }
        #endregion

        #region Methods
        // Reads the expression
		public Expression<string> ReadExpression()
        {
			Expression<string> Expression = null;

            if (!MyCheckEndOfExpression())
            {
                Expression = MyParseExpression();
            }

            return Expression;
        }
        #endregion

        #region My methods

        // Expression parser chain start
		private Expression<string> MyParseExpression()
        {
            return MyParseOrExpression();
        }

		private Expression<string> MyParseOrExpression()
        {
			Expression<string> statement;
            statement = MyParseAndExpression();

            while (!MyCheckEndOfExpression() && _Lexem.Equals(JatoLexemConstants.Bool, "OR"))
            {
                MyReadNextLexem(); // skip 'OR'
				statement = new OrExpression<string>(statement, MyParseAndExpression());
            }

            return statement;
        }

		private Expression<string> MyParseAndExpression()
        {
			Expression<string> statement;
            statement = MyParseLowestLevelExpression();

            while (!MyCheckEndOfExpression() && _Lexem.Equals(JatoLexemConstants.Bool, "AND"))
            {
                MyReadNextLexem(); // skip 'AND'
				statement = new AndExpression<string>(statement, MyParseLowestLevelExpression());
            }

            return statement;
        }

		private Expression<string> MyParseLowestLevelExpression()
        {
            string value;

            if (_Lexem.LexemType == JatoLexemConstants.Id)
            {
                value = _Lexem.Value;
                MyReadNextLexem();
                return new ValueExpression<string>(value);
            }
            else if (_Lexem.LexemType == JatoLexemConstants.Parenthesis)
            {
                return MyParseParenthesis();
            }
            else if (_Lexem.Equals(JatoLexemConstants.Bool, "NOT"))
            {
                MyReadNextLexem(); // skip 'NOT'
				return new NotExpression<string>(MyParseLowestLevelExpression());
            }
            else 
            {
                throw new ParserException("Expression expected.");
            }
        }

		private Expression<string> MyParseParenthesis()
        {
			Expression<string> Expression;

            MyReadNextLexem(); // skip '('

            Expression = MyParseExpression();

            MyReadNextLexem(); // skip ')'
            
            return Expression;
        }

        private void MyReadNextLexem()
        {
            _Lexem = _Lexemizer.ReadNextLexem();
        }

        private bool MyCheckEndOfExpression()
        {
            return _Lexem == null;
        }
        #endregion
    }
}
