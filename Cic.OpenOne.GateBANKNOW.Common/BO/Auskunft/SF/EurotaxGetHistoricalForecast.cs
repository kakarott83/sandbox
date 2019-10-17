using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    public class EurotaxGetHistoricalForecast : AbstractAuskunftBo<EurotaxInDto, EurotaxOutDto>
    {
        IEurotaxWSDao Eurotaxwsdao;
        IEurotaxBo Eurotaxbo;

        /// <summary>
        /// Gets AuskunftDto by SysAuskunft and calls Eurotax Webservice GetHistoricalForecast(InDto)
        /// </summary>
        /// <param name="SYSAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long SYSAuskunft)
        {
            Eurotaxwsdao = new EurotaxWSDao();
            Eurotaxbo = new EurotaxBo(Eurotaxwsdao);
            EurotaxInDto EurotaxInDto = new EurotaxInDto();
            // AuskunftDao  auskunftdao = new AuskunftDao();
            // AuskunftDto auskunft = auskunftdao.findById(SysAuskunft);
            // EurotaxInDto.settingType = auskunft.settingType;
            // EurotaxInDto.extendedVehicleType = auskunft.extendedVehicleType;
            EurotaxOutDto EurotaxOutDto = new EurotaxOutDto();
            EurotaxOutDto = Eurotaxbo.GetHistoricalForecast(EurotaxInDto);

            AuskunftDto auskunft = new AuskunftDto();

            return auskunft;
        }

        /// <summary>
        /// Gets vehicleType information from database by Area and SYSId and calls Eurotax Webservice GetHistoricalForecast(InDto)
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="SYSId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string Area, long SYSId)
        {
            return new AuskunftDto();
        }

        /// <summary>
        /// Calls Eurotax Webservice GetHistoricalForecast(InDto)
        /// </summary>
        /// <param name="InDto"></param>
        /// <returns></returns>
        public override EurotaxOutDto doAuskunft(EurotaxInDto InDto)
        {
            Eurotaxwsdao = new EurotaxWSDao();
            Eurotaxbo = new EurotaxBo(Eurotaxwsdao);
            return Eurotaxbo.GetHistoricalForecast(InDto);
        }
    }
}
