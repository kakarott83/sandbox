using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.DTO;




namespace Cic.One.Web.BO.Search
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
