// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class BRANDAssembler : IDtoAssembler<BRANDDto, BRAND>
    {
        #region IDtoAssembler<BRANDDto, BRAND> Members (Methods)
        public bool IsValid(BRANDDto dto)
        {            
            // TODO JJ 8 JJ, Implement
            throw new NotImplementedException();
        }

        public BRAND Create(BRANDDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public BRAND Update(BRANDDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public BRANDDto ConvertToDto(BRAND domain)
        {
            BRANDDto BRANDDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            BRANDDto = new BRANDDto();
            MyMap(domain, BRANDDto);

            return BRANDDto;
        }

        public BRAND ConvertToDomain(BRANDDto dto)
        {
            BRAND BRAND;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            BRAND = new BRAND();
            MyMap(dto, BRAND);

            return BRAND;
        }
        #endregion 

        #region IDtoAssembler<BRANDDto, BRAND> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(BRANDDto fromBRANDDto, BRAND toBRAND)
        {
            // Mapping
            // Properties
            toBRAND.NAME = fromBRANDDto.NAME;
        }

        private void MyMap(BRAND fromBRAND, BRANDDto toBRANDDto)
        {
            // Mapping
            // Ids 
            toBRANDDto.SYSBRAND = fromBRAND.SYSBRAND;

            // Properties
            toBRANDDto.NAME = fromBRAND.NAME;
        }
        #endregion
    }
}