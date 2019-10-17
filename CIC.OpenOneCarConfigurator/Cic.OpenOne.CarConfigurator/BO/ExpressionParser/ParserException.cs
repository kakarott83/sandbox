using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO.ExpressionParser
{
    [System.CLSCompliant(true)]
    public class ParserException : System.ApplicationException
    {
        #region Constructors
        internal ParserException(string message)
            : base(message)
        {
        }

        internal ParserException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
        #endregion
    }
}
