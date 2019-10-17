// OWNER JJ, 25-02-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    using Cic.OpenLease.Model.DdOl;
    using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
    #endregion

    [System.CLSCompliant(true)]
    public class BERUFAssembler : IDtoAssembler<BERUFDto, VC_BERUF>
    {
        #region IDtoAssembler<BERUFDto,VC_BERUF> Members (Methods)
        public bool IsValid(BERUFDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public VC_BERUF Create(BERUFDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public VC_BERUF Update(BERUFDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public BERUFDto ConvertToDto(VC_BERUF domain)
        {
            BERUFDto BERUFDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            BERUFDto = new BERUFDto();
            MyMap(domain, BERUFDto);

            return BERUFDto;
        }

        public VC_BERUF ConvertToDomain(BERUFDto dto)
        {
            VC_BERUF VC_BERUF;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            VC_BERUF = new VC_BERUF();
            MyMap(dto, VC_BERUF);

            return VC_BERUF;
        }
        #endregion

        #region IDtoAssembler<BERUFDto,VC_BERUF> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(BERUFDto fromBERUFDto, VC_BERUF toVC_BERUF)
        {
            // Mapping
            // Properties
            toVC_BERUF.ID = fromBERUFDto.ID;
            toVC_BERUF.CODE = fromBERUFDto.CODE;
            toVC_BERUF.DOMAINID = fromBERUFDto.DOMAINID;
            toVC_BERUF.RANK = fromBERUFDto.RANK;
            toVC_BERUF.TOOLTIP = fromBERUFDto.TOOLTIP;
            toVC_BERUF.VALUE = fromBERUFDto.VALUE;
        }

        private void MyMap(VC_BERUF fromVC_BERUF, BERUFDto toBERUFDto)
        {
            // Mapping
            // Properties
            toBERUFDto.ID = fromVC_BERUF.ID;
            toBERUFDto.CODE = fromVC_BERUF.CODE;
            toBERUFDto.DOMAINID = fromVC_BERUF.DOMAINID;
            toBERUFDto.RANK = fromVC_BERUF.RANK;
            toBERUFDto.TOOLTIP = fromVC_BERUF.TOOLTIP;
            toBERUFDto.VALUE = fromVC_BERUF.VALUE;
        }
        #endregion
    }
}