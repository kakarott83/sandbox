// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class PEROLEAssembler : IDtoAssembler<PEROLEDto, PEROLE>
    {
        #region IDtoAssembler<PEROLEDto, PEROLE> Members (Methods)
        public bool IsValid(PEROLEDto dto)
        {            
            // TODO JJ 8 JJ, Implement
            throw new NotImplementedException();
        }

        public PEROLE Create(PEROLEDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public PEROLE Update(PEROLEDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public PEROLEDto ConvertToDto(PEROLE domain)
        {
            PEROLEDto PEROLEDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            PEROLEDto = new PEROLEDto();
            MyMap(domain, PEROLEDto);

            return PEROLEDto;
        }

        public PEROLE ConvertToDomain(PEROLEDto dto)
        {
            PEROLE PEROLE;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            PEROLE = new PEROLE();
            MyMap(dto, PEROLE);

            return PEROLE;
        }
        #endregion 

        #region IDtoAssembler<PEROLEDto, PEROLE> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(PEROLEDto fromPEROLEDto, PEROLE toPEROLE)
        {
            // Mapping
            // Ids 
            toPEROLE.SYSPERSON = fromPEROLEDto.SYSPERSON;
            toPEROLE.SYSPARENT = fromPEROLEDto.SYSPARENT;

            // Properties
            toPEROLE.NAME = fromPEROLEDto.NAME;
            toPEROLE.DESCRIPTION = fromPEROLEDto.DESCRIPTION;
        }

        private void MyMap(PEROLE fromPEROLE, PEROLEDto toPEROLEDto)
        {
            // Mapping
            // Ids 
            toPEROLEDto.SYSPEROLE = fromPEROLE.SYSPEROLE;
            toPEROLEDto.SYSPERSON = fromPEROLE.SYSPERSON;
            toPEROLEDto.SYSPARENT = fromPEROLE.SYSPARENT;

            // Properties
            toPEROLEDto.NAME = fromPEROLE.NAME;
            toPEROLEDto.DESCRIPTION = fromPEROLE.DESCRIPTION;            
        }
        #endregion
    }
}