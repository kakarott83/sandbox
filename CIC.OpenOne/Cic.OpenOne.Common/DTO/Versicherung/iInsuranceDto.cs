using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.Common.DTO.Versicherung
{
    

    /// <summary>
    /// Holds all input  values to calculate a insurance
    /// </summary>
    public class iInsuranceDto
    {
        /// <summary>
        /// Insurance Key
        /// </summary>
        public long sysvstyp
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
        /// optional additional inputvalues needed for some calculations
        /// </summary>
        public List<InsuranceCalcComponent> additionalInputValues { get; set; }
            

    }
}
