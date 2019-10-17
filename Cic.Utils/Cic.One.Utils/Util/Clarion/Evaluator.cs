using System;
using System.Globalization;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Ciloci.Flee;

namespace Cic.OpenOne.Common.Util.Clarion
{
    /// <summary>
    /// PkzHelper-Klasse
    /// </summary>
    public static class PkzHelper
    {
        #region Methods
        /// <summary>
        /// pkz static method
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [System.CLSCompliant(true)]
        public static int pkz(string number)
        {
            // 'number' should contain only digits 0 .. 9!
            int[] table = { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };
            int carryover = 0;
            try
            {
                foreach (char digit in number)
                    carryover = table[(carryover + digit - '0') % 10];
            }
            catch
            {
                // if not a digit
            }
            return (10 - carryover) % 10;
        }
        #endregion
    }

    public class Evaluator : IEvaluator
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ExpressionContext _Context = new ExpressionContext();
        #region Methods
        public Evaluator()
        {
            // Set the parameters
            _Context.ParserOptions.DecimalSeparator = '.';
            _Context.ParserOptions.FunctionArgumentSeparator = ',';
            // tbc: _Context.ParserOptions.DateTimeFormat = "#DD.MM.YYYY#";
            _Context.ParserOptions.RecreateParser();
            this.Bind();
        }
        public void Bind()
        {
            Bind(typeof(PkzHelper));
            // some standard bindings ...
            _Context.Imports.AddType(typeof(System.Math), "sys");
            _Context.Imports.AddType(typeof(System.Environment), "sys");
            _Context.Imports.AddType(typeof(System.String), "sys");
        }

        public void Bind(string name, object value)
        {
            _Context.Variables.Remove(name);
            _Context.Variables.Add(name, value);
        }

        public void Bind(Type type)
        {
            _Context.Imports.AddType(type);
        }

        public void Bind(Type type, string nameSpace)
        {
            _Context.Imports.AddType(type, nameSpace);
        }

        public void Bind(System.Reflection.MethodInfo methodInfo, string nameSpace)
        {
            _Context.Imports.AddMethod(methodInfo, nameSpace);
        }

        public void Bind(string methodName, Type type, string nameSpace)
        {
            _Context.Imports.AddMethod(methodName, type, nameSpace);
        }

        public object SetVariable(string name, object value)
        {
            _Context.Variables[name] = value;
            return 1;
        }

        public object GetVariable(string name)
        {
            return _Context.Variables[name];
        }

        /// <summary>
        /// Evaluates the result of an expression
        /// </summary>
        /// <param name="evalExpr">a valid Flee Expression</param>
        /// <returns>the evaluated result</returns>
        /// <exception cref="Cic.Basic.EvaluateException">when evalExpr is invalid</exception>
        public string evaluate(string evalExpr)
        {
            try
            {
                IDynamicExpression ev = _Context.CompileDynamic(evalExpr);
                object result = (object)ev.Evaluate();

                NumberFormatInfo nfi = new NumberFormatInfo();

                nfi.NumberDecimalSeparator = ".";
                nfi.NumberGroupSeparator = " ";
                try
                {
                    decimal d = Decimal.Parse(result.ToString());
                    return d.ToString(nfi);
                }
                catch
                {
                    // Nicht auf Decimal parsebar
                    return result.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Evaluation of " + evalExpr + " failed", ex);
            }
        }

        /// <summary>
        /// Validates a boolean expression
        /// </summary>
        /// <param name="chkExpr">a valid Flee Expression</param>
        /// <returns>the evaluated result</returns>
        /// <exception cref="Cic.Basic.EvaluateException">when the expression is no valid boolean expression</exception>
        public bool validate(string chkExpr)
        {
            try
            {
                bool result;

                if (chkExpr == null || chkExpr == "")
                {
                    result = true;
                }
                else
                {
                    IGenericExpression<bool> ev = _Context.CompileGeneric<bool>(chkExpr);
                    result = ev.Evaluate();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Evaluation of " + chkExpr + " failed", ex);
            }
        }
        #endregion
    }
}