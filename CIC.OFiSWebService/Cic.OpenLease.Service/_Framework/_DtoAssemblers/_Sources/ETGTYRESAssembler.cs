// OWNER WB, 12-05-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.ET.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class ETGTYRESAssembler : IDtoAssembler<TireDto, ETGTYRES>
    {
        #region IDtoAssembler<ITSearchResultDto,IT> Members (Methods)
        public bool IsValid(TireDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ETGTYRES Create(TireDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ETGTYRES Update(TireDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public TireDto ConvertToDto(ETGTYRES domain)
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


        public List<TireDto> ConvertToListDto(List<ETGTYRES> EtgTyres)
        {
            //Create new tire dto list
            List<TireDto> TireDtoList = null;
            TireDtoList = new List<TireDto>();
            TireDto TireDto;

            var q = from t in EtgTyres
                    group t by new { t.WIDTH, t.CROSSSEC, t.DIAMETER }
                        into r
                        select new
                        {
                            width = r.Key.WIDTH,
                            crosssec = r.Key.CROSSSEC,
                            diameter = r.Key.DIAMETER
                        };


            foreach (var group in q)
            {
                TireDto = new TireDto();
                TireDto.Code =  RestOfTheHelpers.GetTyreCodeWithR(group.width, group.crosssec, group.diameter);
                TireDtoList.Add(TireDto);
            }
            return TireDtoList;
        }



        public ETGTYRES ConvertToDomain(TireDto dto)
        {
            ETGTYRES ETGTYRES;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            ETGTYRES = new ETGTYRES();
            MyMap(dto, ETGTYRES);

            return ETGTYRES;
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
        private void MyMap(TireDto fromTireDto, ETGTYRES toETGTYRES)
        {
 
        }

        private void MyMap(ETGTYRES fromETGTYRES, TireDto toTireDto)
        {
            toTireDto.Code =  RestOfTheHelpers.GetTyreCode(fromETGTYRES);
        }
        #endregion
    }
}