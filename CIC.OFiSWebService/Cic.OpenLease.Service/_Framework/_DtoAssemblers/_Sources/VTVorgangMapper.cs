using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.OpenLease.Service
{
    public class VTVorgangMapper : IDtoMapper<VorgangDto, VTDto>
    {
        public void mapToDto(VorgangDto dto, VTDto domain)
        {
            //dto.ANGEBOT = domain;
            //dto.ANTRAG = domain.ANTRAG;
            dto.JAHRESKM = (long)(domain.ObJahresKM.HasValue?domain.ObJahresKM:0);
            dto.LAUFZEIT = (int)(domain.KalkLZ.HasValue?domain.KalkLZ:0);
            dto.OBFABRIKAT = domain.ObFabrikat;
            dto.OBHERSTELLER = domain.ObHersteller;
            //dto.OBJEKTVT = ;
            
            //dto.PRODUCTNAME = ;
            dto.RW = (decimal)domain.KalkRW;
            dto.RATE = (decimal)(domain.RATE.HasValue ? domain.RATE : 0);
            dto.SYSID = (long)domain.SysId;
            dto.VERTRAG = domain.VERTRAG;
            dto.ZUSTAND = domain.ZUSTAND;
            dto.VART = domain.VART;
        }


        public void mapFromDto(VorgangDto dto, VTDto domain)
        {
           
        }
    }
}