﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.DeltavistaRef;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// Deltavista AddressIdentification ServiceFacade, implements AbstractAuskunftBo
    /// </summary>
    public class DVAddressIdentificationArb : AbstractAuskunftBo<DeltavistaInDto, AuskunftDto>
    {
        /// <summary>
        /// Overwritten AbstractAuskunftBo method, gets input values by inDto and calls Deltavista Webservice getIdentifiedAddress() 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(DeltavistaInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultDeltavistaBo().getIdentifiedAddressArb(inDto);
        }

        /// <summary>
        /// Overwritten IAuskunftBo method, gets input values by a filled AUSKUNFT entity and calls Deltavista Webservice getIdentifiedAddress() 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultDeltavistaBo().getIdentifiedAddressArb(sysAuskunft);
        }

        /// <summary>
        /// Overwritten IAuskunftBo method, gets input values from database by Area and SYSId and calls Deltavista Webservice getIdentifiedAddress() 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }
    }
}
