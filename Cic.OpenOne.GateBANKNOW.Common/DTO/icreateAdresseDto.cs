using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für createAdresse Methode
    /// </summary>
    public class icreateAdresseDto
    {
        /// <summary>
        /// ID der Adresse (leer für neue Adresse)
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
    }
}
