// OWNER WB, 16-03-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class PRFLDARTAssembler : IDtoAssembler<PrFldArtDto, PRFLDART>
    {
        #region IDtoAssembler<PRPARAMDto,PRPARAM> Members

        public bool IsValid(PrFldArtDto dto)
        {
            throw new System.NotImplementedException();
        }

        public PRFLDART Create(PrFldArtDto dto)
        {
            throw new System.NotImplementedException();
        }

        public PRFLDART Update(PrFldArtDto dto)
        {
            throw new System.NotImplementedException();
        }

        public PrFldArtDto ConvertToDto(PRFLDART domain)
        {
            PrFldArtDto Dto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            Dto = new PrFldArtDto();

            MyMap(domain, Dto);

            return Dto;
        }

        public PRFLDART ConvertToDomain(PrFldArtDto dto)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region My Methods
        private void MyMap(PrFldArtDto dto, PRFLDART domain)
        {
            domain.DESCRIPTION = dto.DESCRIPTION;
            domain.NAME = dto.NAME;
            
        }

        private void MyMap(PRFLDART domain, PrFldArtDto dto)
        {
            dto.DESCRIPTION = domain.DESCRIPTION;
            dto.NAME = domain.NAME;
            
        }
        #endregion
    }
}
