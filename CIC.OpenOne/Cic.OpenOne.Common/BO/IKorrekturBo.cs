using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Korrektur Typen
    /// </summary>
    public enum KorrekturType
    {
        /// <summary>
        /// Automatisch
        /// </summary>
        TYPE_AUTO = 0,
        /// <summary>
        /// Dezimal
        /// </summary>
        TYPE_DECIMAL = 1,
        /// <summary>
        /// Zeichenfolge
        /// </summary>
        TYPE_STRING = 2
    }

    /// <summary>
    /// Korrektur Business Object Interface
    /// </summary>
    public interface IKorrekturBo
    {
        /// <summary>
        /// corresponds to the clarion correct method, selecting the type - parameters automatically
        /// </summary>
        /// <param name="korrtypName"></param>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <param name="perDate"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        double Correct(string korrtypName, decimal value, string op, DateTime perDate, String p1, String p2);

        /// <summary>
        /// corresponds to the clarion correct method
        /// </summary>
        /// <param name="korrtypName"></param>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <param name="perDate"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        double Correct(string korrtypName, decimal value, string op, DateTime perDate, String p1, String p2, KorrekturType type1, KorrekturType type2);
    }
}
