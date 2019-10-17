// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class PLZAssembler : IDtoAssembler<PLZDto, PLZ>
    {
        #region IDtoAssembler<PLZDto,PLZ> Members (Methods)
        public bool IsValid(PLZDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public PLZ Create(PLZDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public PLZ Update(PLZDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public PLZDto ConvertToDto(PLZ domain)
        {
            PLZDto PLZDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            PLZDto = new PLZDto();
            MyMap(domain, PLZDto);

            return PLZDto;
        }

        public PLZ ConvertToDomain(PLZDto dto)
        {
            PLZ PLZ;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            PLZ = new PLZ();
            MyMap(dto, PLZ);

            return PLZ;
        }
        #endregion 

        #region IDtoAssembler<PLZDto,PLZ> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(PLZDto fromPLZDto, PLZ toPLZ)
        {
            // Mapping
            // Ids            
            toPLZ.SYSPLZ = fromPLZDto.SYSPLZ;

            // Properties
            toPLZ.PLZ1 = fromPLZDto.PLZ1;
            toPLZ.ORT = fromPLZDto.ORT;
        }

        private void MyMap(PLZ fromPLZ, PLZDto toPLZDto)
        {
            // Mapping
            // Ids
            toPLZDto.SYSPLZ = fromPLZ.SYSPLZ;

            // Property
            toPLZDto.PLZ1 = fromPLZ.PLZ1;
            toPLZDto.ORT = fromPLZ.ORT;
        }
        #endregion
    }
}