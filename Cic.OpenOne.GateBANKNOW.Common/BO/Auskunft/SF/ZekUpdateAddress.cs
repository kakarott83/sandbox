﻿using System;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// ZEK Update Address Class
    /// </summary>
    public class ZekUpdateAddress : AbstractAuskunftBo<ZekInDto, AuskunftDto>
    {
        /// <summary>
        /// do Auskunft
        /// </summary>
        /// <param name="sysAuskunft">Auskunft ID</param>
        /// <returns>Auskunft Dto</returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultZekBo().updateAddress(sysAuskunft);
        }

        /// <summary>
        /// do Auskunft
        /// </summary>
        /// <param name="area">Area Name</param>
        /// <param name="sysId">Sys ID</param>
        /// <returns>Auskunft Dto</returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// do Auskunft
        /// </summary>
        /// <param name="inDto">ZEK Input Dto</param>
        /// <returns>ZEK Output Dto</returns>
        public override AuskunftDto doAuskunft(ZekInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultZekBo().updateAddress(inDto);
        }
    }
}