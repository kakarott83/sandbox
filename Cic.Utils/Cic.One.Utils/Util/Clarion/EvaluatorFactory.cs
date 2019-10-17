
namespace Cic.OpenOne.Common.Util.Clarion
{
    /// <summary>
    /// EvaluatorFactory-Klasse
    /// </summary>
    public class EvaluatorFactory
    {
        /// <summary>
        /// createEvaluator
        /// </summary>
        /// <returns></returns>
        public static IEvaluator createEvaluator()
        {
            return new Evaluator();
        }
    }
}