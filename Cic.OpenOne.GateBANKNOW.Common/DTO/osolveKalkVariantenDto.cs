using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Outputparameter für SolveKalkVarianten Methode
    /// </summary>
    public class osolveKalkVariantenDto
    {
            /// <summary>
            /// antrag
            /// </summary>
            public DTO.AntragDto antrag
            {
                get;
                set;
            }

            /// <summary>
            /// Frontid results
            /// </summary>
            public string frontid { get; set; }
        }
}
