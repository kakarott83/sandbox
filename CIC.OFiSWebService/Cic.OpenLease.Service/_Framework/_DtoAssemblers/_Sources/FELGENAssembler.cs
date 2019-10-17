// OWNER WB, 12-05-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class FELGENAssembler : IDtoAssembler<RimDto, FELGEN>
    {
        #region IDtoAssembler<RimDto,FELGEN> Members (Methods)
        public bool IsValid(RimDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public FELGEN Create(RimDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public FELGEN Update(RimDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public RimDto ConvertToDto(FELGEN domain)
        {
            RimDto RimDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            RimDto = new RimDto();
            MyMap(domain, RimDto);

            return RimDto;
        }

        public FELGEN ConvertToDomain(RimDto dto)
        {
            FELGEN FELGEN;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            FELGEN = new FELGEN();
            MyMap(dto, FELGEN);

            return FELGEN;
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
        private void MyMap(RimDto fromRimDto, FELGEN toFELGEN)
        {
            toFELGEN.BEZEICHNUNG = fromRimDto.Code;
            toFELGEN.HERSTELLER = fromRimDto.Manufacturer;
            toFELGEN.PREIS = fromRimDto.Price;
        }

        private void MyMap(FELGEN fromFELGEN, RimDto toRimDto)
        {
            toRimDto.Code = fromFELGEN.BEZEICHNUNG;
            toRimDto.Manufacturer = fromFELGEN.HERSTELLER;
            toRimDto.Price = fromFELGEN.PREIS;
            toRimDto.Diameter = RestOfTheHelpers.GetDiameter(fromFELGEN.SYSFELGTYP).ToString();
            toRimDto.Width = RestOfTheHelpers.GetWidth(fromFELGEN.SYSFELGTYP).ToString();
        }
        #endregion
    }
}