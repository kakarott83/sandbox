// OWNER MK, 22-02-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenLease.Model.DdOl;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class PRPRODUCTAssembler : IDtoAssembler<PRPRODUCTDto, PRPRODUCT>
    {

        #region IDtoAssembler<PRPRODUCTDto,PRPRODUCT> Members

        public bool IsValid(PRPRODUCTDto dto)
        {
            throw new System.NotImplementedException();
        }

        public PRPRODUCT Create(PRPRODUCTDto dto)
        {
            throw new System.NotImplementedException();
        }

        public PRPRODUCT Update(PRPRODUCTDto dto)
        {
            throw new System.NotImplementedException();
        }

        public PRPRODUCTDto ConvertToDto(PRPRODUCT domain)
        {
            PRPRODUCTDto Dto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            Dto = new PRPRODUCTDto();

            MyMap(domain, Dto);

            return Dto;
        }

        public PRPRODUCT ConvertToDomain(PRPRODUCTDto dto)
        {
            PRPRODUCT Domain;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            Domain = new PRPRODUCT();

            MyMap(dto, Domain);

            return Domain;
        }

        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region
        private void MyMap(PRPRODUCTDto dto, PRPRODUCT domain)
        {
            domain.DESCRIPTION = dto.DESCRIPTION;
            domain.NAME = dto.NAME;
            domain.SYSPRPRODUCT = dto.SYSPRPRODUCT;
            domain.NAMEINTERN = dto.NAMEINTERN;
        }

        private void MyMap(PRPRODUCT domain, PRPRODUCTDto dto)
        {
            dto.DESCRIPTION = domain.DESCRIPTION;
            dto.NAME = domain.NAME;
            dto.SYSPRPRODUCT = domain.SYSPRPRODUCT;
            dto.NAMEINTERN = domain.NAMEINTERN;
        }
        #endregion
    }
}
