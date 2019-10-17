using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// Deltavista CompanyDetails ServiceFacade, implements AbstractAuskunftBo
    /// </summary>
    public class DVgetCompanyDetails : AbstractAuskunftBo<DeltavistaInDto, AuskunftDto>
    {
        /// <summary>
        /// Overwritten IAuskunftBo method, gets input values by a filled AUSKUNFT entity and calls Deltavista Webservice getCompanyDetailsByAddressId() 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultDeltavistaBo().getCompanyDetailsByAddressId(sysAuskunft);
        }

        /// <summary>
        /// Overwritten IAuskunftBo method, gets input values from database by Area and SYSId and calls Deltavista Webservice getCompanyDetailsByAddressId() 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overwritten AbstractAuskunftBo method, gets input values by inDto and calls Deltavista Webservice getCompanyDetailsByAddressId()
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(DeltavistaInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultDeltavistaBo().getCompanyDetailsByAddressId(inDto);
        }
    }
}
