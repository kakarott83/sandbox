// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class BLZAssembler : IDtoAssembler<BLZDto, BLZ>
    {
        #region IDtoAssembler<BLZDto,BLZ> Members (Methods)
        public bool IsValid(BLZDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public BLZ Create(BLZDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public BLZ Update(BLZDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public BLZDto ConvertToDto(BLZ domain)
        {
            BLZDto BLZDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            BLZDto = new BLZDto();
            MyMap(domain, BLZDto);

            return BLZDto;
        }

        public BLZ ConvertToDomain(BLZDto dto)
        {
            BLZ BLZ;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            BLZ = new BLZ();
            MyMap(dto, BLZ);

            return BLZ;
        }
        #endregion 

        #region IDtoAssembler<BLZDto,BLZ> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(BLZDto fromBLZDto, BLZ toBLZ)
        {
            // Mapping
            // Ids            
            toBLZ.SYSBLZ = fromBLZDto.SYSBLZ;

            // Properties
            toBLZ.BLZ1 = fromBLZDto.BLZ1;
            toBLZ.BIC = fromBLZDto.BIC;
            toBLZ.NAME = fromBLZDto.NAME;
        }

        private void MyMap(BLZ fromBLZ, BLZDto toBLZDto)
        {
            // Mapping
            // Ids
            toBLZDto.SYSBLZ = fromBLZ.SYSBLZ;

            // Property
            toBLZDto.BLZ1 = fromBLZ.BLZ1;
            toBLZDto.BIC = fromBLZ.BIC;
            toBLZDto.NAME = fromBLZ.NAME;
        }
        #endregion
    }
}