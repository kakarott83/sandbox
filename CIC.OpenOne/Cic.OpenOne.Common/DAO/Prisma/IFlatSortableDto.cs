using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    /// <summary>
    /// Flache Sortierung Schnittstelle
    /// </summary>
    public interface IFlatSortableDto
    {
        /// <summary>
        /// Id auslesen
        /// </summary>
        /// <returns>ID</returns>
        long getSortTargetId();

        /// <summary>
        /// Daten auslesen
        /// </summary>
        /// <returns>Daten</returns>
        object getData();
    }
}
