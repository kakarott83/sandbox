// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
    using CIC.Database.OW.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class CTLANGAssembler : IDtoAssembler<CTLANGDto, CTLANG>
    {
        #region IDtoAssembler<CTLANGDto,CTLANG> Members (Methods)
        public bool IsValid(CTLANGDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public CTLANG Create(CTLANGDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public CTLANG Update(CTLANGDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public CTLANGDto ConvertToDto(CTLANG domain)
        {
            CTLANGDto CTLANGDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            CTLANGDto = new CTLANGDto();
            MyMap(domain, CTLANGDto);

            return CTLANGDto;
        }

        public CTLANG ConvertToDomain(CTLANGDto dto)
        {
            CTLANG CTLANG;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            CTLANG = new CTLANG();
            MyMap(dto, CTLANG);

            return CTLANG;
        }
        #endregion 

        #region IDtoAssembler<CTLANGDto,CTLANG> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(CTLANGDto fromCTLANGDto, CTLANG toCTLANG)
        {
            // Mapping
            // Ids            
            toCTLANG.SYSCTLANG = fromCTLANGDto.SYSCTLANG;

            // Properties
            toCTLANG.LANGUAGENAME = fromCTLANGDto.LANGUAGENAME;
            toCTLANG.ISOCODE = fromCTLANGDto.ISOCODE;
        }

        private void MyMap(CTLANG fromCTLANG, CTLANGDto toCTLANGDto)
        {
            // Mapping
            // Ids
            toCTLANGDto.SYSCTLANG = fromCTLANG.SYSCTLANG;

            // Property
            toCTLANGDto.LANGUAGENAME = fromCTLANG.LANGUAGENAME;
            toCTLANGDto.ISOCODE = fromCTLANG.ISOCODE;
        }
        #endregion
    }
}