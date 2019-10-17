using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenLease.Service.DdOl;

namespace Cic.OpenLease.Service
{
    public class ANGEBOTVorgangMapper : IDtoMapper<VorgangDto, ANGEBOTDto>
    {
        public void mapToDto(VorgangDto dto, ANGEBOTDto domain)
        {
            dto.ANGEBOT = domain.ANGEBOT1;
           
            dto.JAHRESKM = (long)(domain.ANGOBJAHRESKM.HasValue ? domain.ANGOBJAHRESKM : 0);
            dto.LAUFZEIT = (int)(domain.ANGKALKLZ.HasValue ? domain.ANGKALKLZ : 0);
           
            dto.OBJEKTVT = domain.OBJEKTVT;


            dto.RW = (decimal)domain.ANGKALKRWKALKBRUTTO;
            dto.RATE = (decimal)(domain.ANGKALKRATEBRUTTO.HasValue ? domain.ANGKALKRATEBRUTTO : 0);
            dto.SYSID = (long)domain.SYSID;
            
            dto.ZUSTAND = domain.ZUSTAND;
            dto.VART = domain.VART;
        }


        public void mapFromDto(VorgangDto dto, ANGEBOTDto domain)
        {
           
        }
    }
}