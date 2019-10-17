using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// Zek RegisterLeasingMietvertrag (EC4) ServiceFacade
    /// </summary>
    public class ZekRegisterLeasingMietvertrag : AbstractAuskunftBo<ZekInDto, AuskunftDto>
    {
        /// <summary>
        /// Gets input values by a filled AUSKUNFT entity and calls RegisterLeasingMietvertrag Webservice (EC4)
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultZekBo().registerLeasingMietvertrag(sysAuskunft);
        }
        /// <summary>
        /// Gets input values from database by Area and sysId and calls RegisterLeasingMietvertrag Webservice (EC4)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets input values by inDto and calls RegisterLeasingMietvertrag Webservice (EC4)
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(ZekInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultZekBo().registerLeasingMietvertrag(inDto);
        }
    }
}
