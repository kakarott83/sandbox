using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Parameterklasse für ParsetDto
    /// </summary>
    public class ParsetDto
    {
        /// <summary>
        /// ID des Parametersets
        /// </summary>
        public long sysID
        {
            get;
            set;
        }

        /// <summary>
        /// Bezeichnung des Parametersets
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Parameterobjekt
        /// </summary>
        public ParamDto[] parameter
        {
            get;
            set;
        }
    }
}
