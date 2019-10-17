using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.OpenLease.Service
{
    public class ANTRAGVorgangMapper : IDtoMapper<VorgangDto, ANTRAGDto>
    {
        public void mapToDto(VorgangDto dto, ANTRAGDto domain)
        {
            //dto.ANGEBOT = domain;
            dto.ANTRAG = domain.ANTRAG;
            dto.JAHRESKM = (long)(domain.AntObJahresKM.HasValue ? domain.AntObJahresKM : 0);
            dto.LAUFZEIT = (int)(domain.AntKalkLZ.HasValue ? domain.AntKalkLZ : 0);
            dto.OBFABRIKAT = domain.AntObFabrikat;
            dto.OBHERSTELLER = domain.AntObHersteller;
            
            //dto.OBJEKTVT = ;
            //dto.PRODUCTNAME = ;
            dto.RW = (decimal)domain.AntKalkRW;
            dto.RATE = (decimal)(domain.RATE.HasValue?domain.RATE:0);
            dto.SYSID = (long)domain.SysId;
            //dto.VERTRAG = domain.VERTRAG;
            dto.ZUSTAND = domain.ZUSTAND;
            dto.VART = domain.VART;
        }


        public void mapFromDto(VorgangDto dto, ANTRAGDto domain)
        {
           
        }
    }
}