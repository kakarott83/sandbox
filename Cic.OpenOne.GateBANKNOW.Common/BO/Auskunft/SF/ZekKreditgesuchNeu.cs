using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// ZEK KreditgesuchNeu ServiceFacade
    /// </summary>
    public class ZekKreditgesuchNeu : AbstractAuskunftBo<ZekInDto, AuskunftDto>
    {
        /// <summary>
        /// Gets input values by a filled AUSKUNFT entity and calls KreditgesuchNeu Webservice (EC1)
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultZekBo().kreditgesuchNeu(sysAuskunft);
        }
        /// <summary>
        /// Gets input values from database by Area and sysId and calls KreditgesuchNeu Webservice (EC1)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets input values by inDto and calls KreditgesuchNeu Webservice (EC1) 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(ZekInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultZekBo().kreditgesuchNeu(inDto);
        }
    }
}
