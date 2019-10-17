// OWNER MP, 26-02-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class OBKATAssembler : IDtoAssembler<OBKATDto, OBKAT>
    {

        #region IDtoAssembler<OBKATDto,OBKAT> Members

        public bool IsValid(OBKATDto dto)
        {
            throw new System.NotImplementedException();
        }

        public OBKAT Create(OBKATDto dto)
        {
            throw new System.NotImplementedException();
        }

        public OBKAT Update(OBKATDto dto)
        {
            throw new System.NotImplementedException();
        }

        public OBKATDto ConvertToDto(OBKAT domain)
        {
            OBKATDto Dto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            Dto = new OBKATDto();

            MyMap(domain, Dto);

            return Dto;
        }

        public OBKAT ConvertToDomain(OBKATDto dto)
        {
            OBKAT Domain;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            Domain = new OBKAT();

            MyMap(dto, Domain);

            return Domain;
        }

        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region
        private void MyMap(OBKATDto dto, OBKAT domain)
        {
            domain.DESCRIPTION = dto.DESCRIPTION;
            domain.SYSOBKAT = dto.SYSOBKAT;
            domain.NAME = dto.NAME;
        }

        private void MyMap(OBKAT domain, OBKATDto dto)
        {
            dto.DESCRIPTION = domain.DESCRIPTION;
            dto.SYSOBKAT = domain.SYSOBKAT;
            dto.NAME = domain.NAME;
        }
        #endregion
    }
}

   

    
