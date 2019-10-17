// OWNER WB, 19-05-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class WRRateAssembler : IDtoAssembler<WRRateDto, WRRate>
    {
        #region IDtoAssembler<ITSearchResultDto,IT> Members (Methods)
        public bool IsValid(WRRateDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public WRRate Create(WRRateDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public WRRate Update(WRRateDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public WRRateDto ConvertToDto(WRRate domain)
        {
            WRRateDto WRRateDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            WRRateDto = new WRRateDto();
            MyMap(domain, WRRateDto);

            return WRRateDto;
        }

        public WRRate ConvertToDomain(WRRateDto dto)
        {
            WRRate WRRate;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }
            using (DdOlExtended Context = new DdOlExtended())
            {
                WRRate = new WRRate(Context);
            }

            MyMap(dto, WRRate);

            return WRRate;
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
        private void MyMap(WRRateDto fromWRRateDto, WRRate toWRRate)
        {
            toWRRate.excessKMCharge = fromWRRateDto.excessKMCharge;
            toWRRate.KMCharge = fromWRRateDto.KMCharge;
            toWRRate.Rate = fromWRRateDto.RateNetto;
            toWRRate.recessKMCharge = fromWRRateDto.recessKMCharge;
        }

        private void MyMap(WRRate fromWRRate, WRRateDto toWRRateDto)
        {
            toWRRateDto.excessKMCharge = fromWRRate.excessKMCharge;
            toWRRateDto.KMCharge = fromWRRate.KMCharge;
            toWRRateDto.RateNetto = fromWRRate.Rate;
            toWRRateDto.recessKMCharge = fromWRRate.recessKMCharge;
        }
        #endregion
    }
}