using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Supported Types of Conditions
    /// </summary>
    public enum ConditionLinkType
    {
        /// <summary>
        /// No Condition set
        /// </summary>
        NONE,
        /// <summary>
        /// Brand
        /// </summary>
        BRAND,
        /// <summary>
        /// Handelsgruppe
        /// </summary>
        PRHGROUP,
        /// <summary>
        /// Erweiterte Handelsgruppe (BankNow)
        /// </summary>
        PRHGROUPEXT,
        /// <summary>
        /// Objektart
        /// </summary>
        OBART,
        /// <summary>
        /// Objekttyp
        /// </summary>
        OBTYP,
        /// <summary>
        /// Kanal
        /// </summary>
        BCHNL,
        /// <summary>
        /// PRUSETYPE
        /// </summary>
        USETYPE,
        /// <summary>
        /// Kundengrupper
        /// </summary>
        PRKGROUP,
        /// <summary>
        /// Mandant
        /// </summary>
        LS,
        /// <summary>
        /// 
        /// </summary>
        VART,
        /// <summary>
        /// 
        /// </summary>
        VARTTAB,
        /// <summary>
        /// 
        /// </summary>
        VTTYP,
        /// <summary>
        /// Kalkulations Typ
        /// </summary>
        KALKTYP,
        /// <summary>
        /// Produkt
        /// </summary>
        PRODUCT,
        /// <summary>
        /// Personen Rolle
        /// </summary>
        PEROLE,
        /// <summary>
        /// Kunden Typ
        /// </summary>
        KDTYP,
        /// <summary>
        /// 
        /// </summary>
        FSTYP,
        /// <summary>
        /// 
        /// </summary>
        VSTYP,
        /// <summary>
        /// Interest type
        /// </summary>
        INTTYPE,
        /// <summary>
        /// Use independent of conditionid
        /// </summary>
        COMMON
    
    }

}
