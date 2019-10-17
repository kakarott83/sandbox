// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    
    using Cic.OpenLease.ServiceAccess.DdOw;
    using CIC.Database.OW.EF6.Model;
    #endregion

    [System.CLSCompliant(true)]
    public class PUSERAssembler : IDtoAssembler<PUSERDto, PUSER>
    {
        #region IDtoAssembler<PUSERDto, PUSER> Members (Methods)
        public bool IsValid(PUSERDto dto)
        {            
            // TODO JJ 8 JJ, Implement
            throw new NotImplementedException();
        }

        public PUSER Create(PUSERDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public PUSER Update(PUSERDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public PUSERDto ConvertToDto(PUSER domain)
        {
            PUSERDto PUSERDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            PUSERDto = new PUSERDto();
            MyMap(domain, PUSERDto);

            return PUSERDto;
        }

        public PUSER ConvertToDomain(PUSERDto dto)
        {
            PUSER PUSER;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            PUSER = new PUSER();
            MyMap(dto, PUSER);

            return PUSER;
        }
        #endregion 

        #region IDtoAssembler<PUSERDto, PUSER> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(PUSERDto fromPUSERDto, PUSER toPUSER)
        {
            // Mapping
            // Ids
            toPUSER.SYSWFUSER = fromPUSERDto.SYSWFUSER;
            toPUSER.SYSDEFAULTPEROLE = fromPUSERDto.SYSDEFAULTPEROLE;

            // Properties
            toPUSER.EXTERNEID = fromPUSERDto.EXTERNEID;
            toPUSER.NAME = fromPUSERDto.NAME;
            toPUSER.VORNAME = fromPUSERDto.VORNAME;            
        }

        private void MyMap(PUSER fromPUSER, PUSERDto toPUSERDto)
        {
            // Mapping
            // Ids
            toPUSERDto.SYSPUSER = fromPUSER.SYSPUSER;
            toPUSERDto.SYSWFUSER = fromPUSER.SYSWFUSER;
            toPUSERDto.SYSDEFAULTPEROLE = fromPUSER.SYSDEFAULTPEROLE;

            // Properties
            toPUSERDto.EXTERNEID = fromPUSER.EXTERNEID;
            toPUSERDto.NAME = fromPUSER.NAME;
            toPUSERDto.VORNAME = fromPUSER.VORNAME;
        }
        #endregion
    }
}