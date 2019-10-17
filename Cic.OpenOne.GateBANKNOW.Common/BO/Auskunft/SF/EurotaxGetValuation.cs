using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// Eurotax GetValuation ServiceFacade
    /// </summary>
    [System.CLSCompliant(false)]
    public class EurotaxGetValuation : AbstractAuskunftBo<EurotaxInDto, EurotaxOutDto>
    {
        /// <summary>
        /// Gets AuskunftDto by SysAuskunft and calls Eurotax Webservice GetValuation
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultEurotaxBo().GetValuation(sysAuskunft);
        }
        /// <summary>
        /// Gets input from database by Area and sysId and calls Eurotax Webservice GetValuation
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets input by EurotaxInDto and calls Eurotax Webservice GetValuation
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override EurotaxOutDto doAuskunft(EurotaxInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultEurotaxBo().GetValuation(inDto);
        }
    }
}
