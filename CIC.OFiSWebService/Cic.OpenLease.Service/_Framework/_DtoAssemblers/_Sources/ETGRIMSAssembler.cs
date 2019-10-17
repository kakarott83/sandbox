// OWNER WB, 12-05-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.ET.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class ETGRIMSAssembler : IDtoAssembler<RimDto, ETGRIMS>
    {
        #region IDtoAssembler<RimDto,ETGRIMS> Members (Methods)
        public bool IsValid(RimDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ETGRIMS Create(RimDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ETGRIMS Update(RimDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public RimDto ConvertToDto(ETGRIMS domain)
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

        public ETGRIMS ConvertToDomain(RimDto dto)
        {
            ETGRIMS ETGRIMS;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            ETGRIMS = new ETGRIMS();
            MyMap(dto, ETGRIMS);

            return ETGRIMS;
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
        private void MyMap(RimDto fromRimDto, ETGRIMS toETGRIMS)
        {
            toETGRIMS.WIDTH = fromRimDto.Code;
        }

        private void MyMap(ETGRIMS fromETGRIMS, RimDto toRimDto)
        {
            

            
           
            toRimDto.Width = fromETGRIMS.WIDTH;
            if (toRimDto.Width.Contains(".0"))
            {
                toRimDto.Width = toRimDto.Width.Substring(0, toRimDto.Width.LastIndexOf(".0"));
            }
            if (toRimDto.Width.Contains("00"))
            {
                toRimDto.Width = toRimDto.Width.Substring(0, toRimDto.Width.LastIndexOf("00"));
            }
            toRimDto.Diameter = fromETGRIMS.DIAMETER;
            if (toRimDto.Diameter.Contains("."))
            {
                toRimDto.Diameter = toRimDto.Diameter.Substring(0, toRimDto.Diameter.LastIndexOf("."));
            }
            toRimDto.Code = toRimDto.Width + "R" + toRimDto.Diameter;

        }
        #endregion
    }
}