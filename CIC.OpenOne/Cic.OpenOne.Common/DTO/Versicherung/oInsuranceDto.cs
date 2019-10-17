using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.Common.DTO.Versicherung
{
    

    /// <summary>
    /// Holds all output  values from calculating an insurance
    /// </summary>
    public class oInsuranceDto
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
        /// The base value for calculating the insurance
        /// </summary>
        public double insuranceOutputValue
        {
            get;
            set;
        }

        /// <summary>
        /// The percentage for calculating the insurance
        /// </summary>
        public double insuranceOutputPercentValue
        {
            get;
            set;
        }
        
        /// <summary>
        /// optional additional inputvalues needed for some calculations
        /// </summary>
        public List<InsuranceResultComponent> additionalOutputValues { get; set; }

        /// <summary>
        /// Returns a certain outputvalue
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public double getOutputValue(InsuranceResultComponentType type)
        {
            if (additionalOutputValues == null) return 0;
            InsuranceResultComponent r = additionalOutputValues.Where(t => t.type == type).FirstOrDefault();
            if (r == null) return 0;
            return r.value;
        }

    }
}
