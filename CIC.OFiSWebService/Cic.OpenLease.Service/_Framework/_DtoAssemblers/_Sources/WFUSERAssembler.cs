// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    
    using Cic.OpenLease.ServiceAccess.DdOw;
    using CIC.Database.OW.EF6.Model;
    #endregion

    [System.CLSCompliant(true)]
    public class WFUSERAssembler : IDtoAssembler<WFUSERDto, WFUSER>
    {
        #region IDtoAssembler<WFUSERDto, WFUSER> Members (Methods)
        public bool IsValid(WFUSERDto dto)
        {            
            // TODO JJ 8 JJ, Implement
            throw new NotImplementedException();
        }

        public WFUSER Create(WFUSERDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public WFUSER Update(WFUSERDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public WFUSERDto ConvertToDto(WFUSER domain)
        {
            WFUSERDto WFUSERDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            WFUSERDto = new WFUSERDto();
            MyMap(domain, WFUSERDto);

            return WFUSERDto;
        }

        public WFUSER ConvertToDomain(WFUSERDto dto)
        {
            WFUSER WFUSER;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            WFUSER = new WFUSER();
            MyMap(dto, WFUSER);

            return WFUSER;
        }
        #endregion 

        #region IDtoAssembler<WFUSERDto, WFUSER> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(WFUSERDto fromWFUSERDto, WFUSER toWFUSER)
        {
            // Mapping
            // Properties
            toWFUSER.CODE = fromWFUSERDto.CODE;
        }

        private void MyMap(WFUSER fromWFUSER, WFUSERDto toWFUSERDto)
        {
            // Mapping
            // Ids            
            toWFUSERDto.SYSWFUSER = fromWFUSER.SYSWFUSER;

            // Properties
            toWFUSERDto.CODE = fromWFUSER.CODE;
        }
        #endregion
    }
}