using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Abstract Class: Aggregation Business Object
    /// </summary>
    public abstract class AbstractAggregationBo : IAggregationBo
    {
        /// <summary>
        /// Aggregation Service Data Access Object
        /// </summary>
        protected IAggregationDao aggregationDao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected IAuskunftDao auskunftDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aggregationDao">Aggregation Service Data Access Object</param>
        /// <param name="auskunftDao">Information Data Access Object</param>
        public AbstractAggregationBo(IAggregationDao aggregationDao, IAuskunftDao auskunftDao)
        {
            this.aggregationDao = aggregationDao;
            this.auskunftDao = auskunftDao;            
        }

        /// <summary>
        /// abstract method to fill input values for Aggregation 
        /// </summary>
        /// <param name="InDto">Input Data</param>
        /// <exception cref="System.NotImplementedException">if not implemented</exception>
        /// <returns>Output Data</returns>
        public abstract AuskunftDto callByValues(AggregationInDto InDto);

        /// <summary>
        /// abstract method to fill input values for Aggregation
        /// </summary>
        /// <param name="sysAuskunft">information ID</param>
        /// <returns>Information Data</returns>
        public abstract AuskunftDto callByValues(long sysAuskunft);

    }
    
}
