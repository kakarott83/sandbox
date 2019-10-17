using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Search
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
