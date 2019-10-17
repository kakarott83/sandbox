using Cic.One.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.Web.BO
{
    /// <summary>
    /// Interface for main calculation logic
    /// </summary>
    public interface ICalculationBo
    {
        /// <summary>
        /// recalculates or solves an offer
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        void solveKalkulation(isolveKalkulationDto input, osolveKalkulationDto rval);

        /// <summary>
        /// From Antrag and Antkalk, create the solveKalkulation input
        /// </summary>
        /// <param name="antrag"></param>
        /// <param name="kalkulation"></param>
        /// <returns></returns>
        isolveKalkulationDto createIsolveKalkulationFromAntrag(AntragDto antrag, AntkalkDto kalkulation);
    }
}
