using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    
    
    /// <summary>
    /// Holds all input  values to calculate a subvention
    /// </summary>
    public class iSubventionDto
    {
        
        /// <summary>
        /// Subvention Source
        /// </summary>
        public SubventionSourceField subventionSource
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the mode of calculation (just implicit, just explicit or both)
        /// </summary>
        public SubventionCalcMode calcMode
        {
            get;
            set;
        }

        /// <summary>
        /// area id corresponding to source (eg sysfstyp, sysvstyp) for explicit subventions (calcMode!) needed
        /// </summary>
        public long? areaId
        {
            get;
            set;
        }

        /// <summary>
        /// Area of the sourceId
        /// </summary>
        public ExplicitSubventionArea sourceArea
        {
            get;
            set;
        }

        /// <summary>
        /// Per Date
        /// </summary>
        public DateTime perDate
        {
            get;
            set;
        }

        /// <summary>
        /// Laufzeit
        /// </summary>
        public long laufzeit
        {
            get;
            set;
        }

        /// <summary>
        /// Subventionsgeber  (für Subventionen an denen keine Person hinterlegt ist)
        /// </summary>
        public long sysperson
        {
            get;
            set;
        }

        /// <summary>
        /// Für Netto/Brutto berechnung
        /// </summary>
        public double mwst
        {
            get;
            set;
        }

        /// <summary>
        /// The gross value without applied subventions
        /// </summary>
        public double defaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// The implicitly gained subvention
        /// </summary>
        public double nachlass
        {
            get;
            set;
        }
    }
}
