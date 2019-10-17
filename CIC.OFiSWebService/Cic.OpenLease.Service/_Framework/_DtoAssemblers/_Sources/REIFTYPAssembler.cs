// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    using Cic.OpenLease.Model.DdOl;
    using Cic.OpenLease.ServiceAccess.DdOl;
    #endregion

    [System.CLSCompliant(true)]
    public class REIFTYPAssembler : IDtoAssembler<TireDto, REIFTYP>
    {
        #region IDtoAssembler<ITSearchResultDto,REIFTYP> Members (Methods)
        public bool IsValid(TireDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public REIFTYP Create(TireDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public REIFTYP Update(TireDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public TireDto ConvertToDto(REIFTYP domain)
        {
            TireDto TireDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            TireDto = new TireDto();
            MyMap(domain, TireDto);

            return TireDto;
        }

        public REIFTYP ConvertToDomain(TireDto dto)
        {
            REIFTYP REIFTYP;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            REIFTYP = new REIFTYP();
            MyMap(dto, REIFTYP);

            return REIFTYP;
        }
        #endregion

        #region IDtoAssembler<ITSearchResultDto,IT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(TireDto fromTireDto, REIFTYP toREIFTYP)
        {
            toREIFTYP.CODE = fromTireDto.Code;
            toREIFTYP.HERSTELLER = fromTireDto.Manufacturer;
            toREIFTYP.NETTO = fromTireDto.Price;
        }

        private void MyMap(REIFTYP fromREIFTYP, TireDto toTireDto)
        {
            toTireDto.Code = fromREIFTYP.CODE;
            toTireDto.Manufacturer = fromREIFTYP.HERSTELLER;
            toTireDto.Price = fromREIFTYP.NETTO;
        }
        #endregion
    }
}