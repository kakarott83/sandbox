using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    public enum Prprodtype
    {
        /// <summary>
        /// Default Behaviour - B2B Products
        /// </summary>
        [Description("STANDARD")]
        STANDARD=0,
        /// <summary>
        /// RW Extension
        /// </summary>
        [Description("RWV")]
        RWV=1,
        /// <summary>
        /// Schnellkalk Products
        /// </summary>
        [Description("SCHNELLCALC")]
        SCHNELLCALC=2,
        /// <summary>
        /// B2C Products
        /// </summary>
        [Description("B2C")]
        B2C=3,
        /// <summary>
        /// B2C and B2B Products
        /// </summary>
        [Description("B2CANTRAG")]
        B2CANTRAG = 4,

    }
}
