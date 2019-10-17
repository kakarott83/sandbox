using Cic.OpenLease.ServiceAccess.DdOl;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenLease.Service
{
    public class PROVTARIFMapper : IDtoMapper<PROVTARIFDto, PROVTARIF>
    {
        public void mapToDto(PROVTARIFDto dto, PROVTARIF domain)
        {
            dto.NAME = domain.NAME;
            dto.STANDARDFLAG = domain.STANDARDFLAG;
            dto.SYSPROVTARIF = domain.SYSPROVTARIF;
        }


        public void mapFromDto(PROVTARIFDto dto, PROVTARIF domain)
        {
            domain.NAME = dto.NAME;
            dto.STANDARDFLAG = domain.STANDARDFLAG;
            domain.SYSPROVTARIF = dto.SYSPROVTARIF;
        }
    }
}