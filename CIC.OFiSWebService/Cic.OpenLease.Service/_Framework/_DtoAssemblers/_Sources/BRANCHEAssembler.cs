// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class BRANCHEAssembler : IDtoAssembler<BRANCHEDto, BRANCHE>
    {
        #region IDtoAssembler<BRANCHEDto,BRANCHE> Members (Methods)
        public bool IsValid(BRANCHEDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public BRANCHE Create(BRANCHEDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public BRANCHE Update(BRANCHEDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public BRANCHEDto ConvertToDto(BRANCHE domain)
        {
            BRANCHEDto BRANCHEDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            BRANCHEDto = new BRANCHEDto();
            MyMap(domain, BRANCHEDto);

            return BRANCHEDto;
        }

        public BRANCHE ConvertToDomain(BRANCHEDto dto)
        {
            BRANCHE BRANCHE;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            BRANCHE = new BRANCHE();
            MyMap(dto, BRANCHE);

            return BRANCHE;
        }
        #endregion 

        #region IDtoAssembler<BRANCHEDto,BRANCHE> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(BRANCHEDto fromBRANCHEDto, BRANCHE toBRANCHE)
        {
            // Mapping
            // Ids            
            toBRANCHE.SYSBRANCHE = fromBRANCHEDto.SYSBRANCHE;

            // Properties
            toBRANCHE.BEZEICHNUNG = fromBRANCHEDto.BEZEICHNUNG;
        }

        private void MyMap(BRANCHE fromBRANCHE, BRANCHEDto toBRANCHEDto)
        {
            // Mapping
            // Ids
            toBRANCHEDto.SYSBRANCHE = fromBRANCHE.SYSBRANCHE;

            // Property
            toBRANCHEDto.BEZEICHNUNG = fromBRANCHE.BEZEICHNUNG;
        }
        #endregion
    }
}