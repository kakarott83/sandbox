using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Gruppierungsergebnis
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class GroupResult<R>
    {
        // <summary>
        // Gruppierungswert
        // </summary>
        //String groupValue;

        /// <summary>
        /// Gruppierungs ergebnisse
        /// </summary>
        GroupResult<R>[] groupResults { get; set; }

        /// <summary>
        /// Ergebnisliste
        /// </summary>
        R[] results { get; set; }
    }
}
