using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    interface ISearch<R>
    {
        /// <summary>
        /// Liefert Suchergebnis vom Typ R für alle Suchkriterien
        /// </summary>
        /// <returns>Liste</returns>
        oSearchDto<R> search(iSearchDto param);
    }
}
