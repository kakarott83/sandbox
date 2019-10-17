using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// Eurotax GetForecast ServiceFacade
    /// </summary>
    [System.CLSCompliant(false)]
    public class EurotaxGetForecast : AbstractAuskunftBo<EurotaxInDto, EurotaxListOutDto>
    {
        /// <summary>
        /// Gets AuskunftDto by SysAuskunft and calls Eurotax Webservice GetForecast
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultEurotaxBo().GetForecast(sysAuskunft);
        }

        /// <summary>
        /// Gets input from database by Area and SYSId and calls Eurotax Webservice GetForecast
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets input by filled EurotaxInDto and calls Eurotax Webservice GetForecast
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override EurotaxListOutDto doAuskunft(EurotaxInDto inDto)
        {
            EurotaxListOutDto outDto = new EurotaxListOutDto();
            outDto.eurotaxListOutDto = AuskunftBoFactory.CreateDefaultEurotaxBo().GetRemo(inDto);
            return outDto;
        }
    }
}
