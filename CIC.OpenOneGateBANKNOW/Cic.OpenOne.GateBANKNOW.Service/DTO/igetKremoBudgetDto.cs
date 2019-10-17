using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class igetKremoBudgetDto
    {
        
        /// <summary>
        /// Budgeteingabeparameter Hauptantragsteller
        /// </summary>
        public KremoBudgetDto budget1 { get;set;}

        /// <summary>
        /// Budgeteingabeparamter 2. Antragsteller
        /// </summary>
        public KremoBudgetDto budget2 { get; set; }

        /// <summary>
        /// Product id
        /// </summary>
        public long sysprproduct { get; set; }

    }
}