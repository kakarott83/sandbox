using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// Aggregation CallByValues ServiceFacade
    /// </summary>
    public class AggregationCallByValues : AbstractAuskunftBo<AggregationInDto, AuskunftDto>
    {
        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// overwritten IAuskunftBo method to get input values by a filled AUSKUNFT set 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultAggregationBo().callByValues(sysAuskunft);
        }

        /// <summary>
        /// overwritten IAuskunftBo method to get input values from database by Area and SYSId
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// overwritten AbstractAuskunftBo method to get input values by InDto 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(AggregationInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultAggregationBo().callByValues(inDto);
        }
    }
}
