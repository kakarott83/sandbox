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
    /// Kremo CallByValues ServiceFacade
    /// </summary>
    public class KREMOcallByValues : AbstractAuskunftBo<KREMOInDto, AuskunftDto>
    {
        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// overwritten IAuskunftBo method to get input values by a filled AUSKUNFT set and call KREMOWebservice KREMOcallByValues() 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultKREMOBo().callByValues(sysAuskunft);
        }

        /// <summary>
        /// overwritten IAuskunftBo method to get input values from database by Area and SYSId and call KREMOWebservice CallKREMOgetVersion() 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// overwritten AbstractAuskunftBo method to get input values by InDto and call KREMOWebservice KREMOCallByValues()
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(KREMOInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultKREMOBo().callByValues(inDto);
        }
    }
}
