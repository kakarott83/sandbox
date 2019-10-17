// OWNER MP, 26-02-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class OBARTAssembler : IDtoAssembler<OBARTDto, OBART>
    {

        #region IDtoAssembler<OBARTDto,OBART> Members

        public bool IsValid(OBARTDto dto)
        {
            throw new System.NotImplementedException();
        }

        public OBART Create(OBARTDto dto)
        {
            throw new System.NotImplementedException();
        }

        public OBART Update(OBARTDto dto)
        {
            throw new System.NotImplementedException();
        }

        public OBARTDto ConvertToDto(OBART domain)
        {
            OBARTDto Dto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            Dto = new OBARTDto();

            MyMap(domain, Dto);

            return Dto;
        }

        public OBART ConvertToDomain(OBARTDto dto)
        {
            OBART Domain;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            Domain = new OBART();

            MyMap(dto, Domain);

            return Domain;
        }

        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region
        private void MyMap(OBARTDto dto, OBART domain)
        {
            domain.DESCRIPTION = dto.DESCRIPTION;
            domain.SYSOBART = dto.SYSOBART;
            domain.NAME = dto.NAME;
            // TODO MK: Implement
            // domain.SYSKALKTYP = dto.SYSKALKTYP;
        }

        private void MyMap(OBART domain, OBARTDto dto)
        {
            dto.DESCRIPTION = domain.DESCRIPTION;
            dto.SYSOBART = domain.SYSOBART;
            dto.NAME = domain.NAME;
            // TODO MK: Implement
            // dto.SYSKALKTYP = 0;
        }
        #endregion
    }
}

   

   
