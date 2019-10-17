using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    /// <summary>
    /// Flache Sortierbar DTO
    /// </summary>
    public class FlatSortableDto : IFlatSortableDto
    {
        /// <summary>
        /// Primärschlüssel
        /// </summary>
        public long sysid;
        /// <summary>
        /// Daten
        /// </summary>
        public object data;

        /// <summary>
        /// ID holen
        /// </summary>
        /// <returns>ID</returns>
        public long getSortTargetId()
        {
            return sysid;
        }

        /// <summary>
        /// Daten holen
        /// </summary>
        /// <returns>Daten</returns>
        public object getData()
        {
            return data;
        }
    }
}
