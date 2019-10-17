using Cic.OpenOne.Common.DTO.Prisma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    /// <summary>
    /// Input for RAP Interest calculation
    /// </summary>
    public class igetRAPZinsDto
    {
        /// <summary>
        /// Ermittelter Score des Kunden für die RAP-Zinsanpassung
        /// </summary>
        public String kundenScore { get; set; }

        /// <summary>
        /// ID des Produktes
        /// </summary>
        public long sysprproduct
        {
            get;
            set;
        }

        public prKontextDto prodCtx { get; set; }
        public long lz {get;set;}
        public double amount {get;set;}
        

    }
}
