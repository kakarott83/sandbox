
namespace Cic.OpenOne.Common.Util.Clarion
{
    /// <summary>
    /// Evaluator Interface
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// Bind
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void Bind(string name, object value);

        /// <summary>
        /// evaluate
        /// </summary>
        /// <param name="evalExpr"></param>
        /// <returns></returns>
        string evaluate(string evalExpr);

        /// <summary>
        /// validate
        /// </summary>
        /// <param name="chkExpr"></param>
        /// <returns></returns>
        bool validate(string chkExpr);
    }
}