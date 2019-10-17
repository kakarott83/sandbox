// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class PERSONAssembler : IDtoAssembler<PERSONDto, PERSON>
    {
        #region IDtoAssembler<PERSONDto, PERSON> Members (Methods)
        public bool IsValid(PERSONDto dto)
        {            
            // TODO JJ 8 JJ, Implement
            throw new NotImplementedException();
        }

        public PERSON Create(PERSONDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public PERSON Update(PERSONDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public PERSONDto ConvertToDto(PERSON domain)
        {
            PERSONDto PERSONDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            PERSONDto = new PERSONDto();
            MyMap(domain, PERSONDto);

            return PERSONDto;
        }

        public PERSON ConvertToDomain(PERSONDto dto)
        {
            PERSON PERSON;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            PERSON = new PERSON();
            MyMap(dto, PERSON);

            return PERSON;
        }
        #endregion 

        #region IDtoAssembler<PERSONDto, PERSON> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(PERSONDto fromPERSONDto, PERSON toPERSON)
        {
            // Mapping
            // Ids
            toPERSON.SYSPUSER = fromPERSONDto.SYSPUSER;

            // Flags
            toPERSON.PRIVATFLAG = (fromPERSONDto.PRIVATFLAG ? 1 : 0);

            // Properties
            toPERSON.CODE = fromPERSONDto.CODE;
            toPERSON.NAME = fromPERSONDto.NAME;
            toPERSON.VORNAME = fromPERSONDto.VORNAME;
        }

        private void MyMap(PERSON fromPERSON, PERSONDto toPERSONDto)
        {
            // Mapping
            // Ids 
            toPERSONDto.SYSPERSON = fromPERSON.SYSPERSON;
            toPERSONDto.SYSPUSER = fromPERSON.SYSPUSER;

            // Flags
            toPERSONDto.PRIVATFLAG = (fromPERSON.PRIVATFLAG.HasValue && fromPERSON.PRIVATFLAG == 1 ? true : false);

            // Properties
            toPERSONDto.CODE = fromPERSON.CODE;
            toPERSONDto.NAME = fromPERSON.NAME;
            toPERSONDto.VORNAME = fromPERSON.VORNAME;
        }
        #endregion
    }
}