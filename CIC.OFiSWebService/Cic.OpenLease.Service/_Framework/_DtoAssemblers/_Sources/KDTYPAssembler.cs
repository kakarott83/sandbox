namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class KDTYPAssembler : IDtoAssembler<KDTYPDto, KDTYP>
    {
        #region IDtoAssembler<KDTYPDto,KDTYP> Members (Methods)
        public bool IsValid(KDTYPDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public KDTYP Create(KDTYPDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public KDTYP Update(KDTYPDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public KDTYPDto ConvertToDto(KDTYP domain)
        {
            KDTYPDto KDTYPDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            KDTYPDto = new KDTYPDto();
            MyMap(domain, KDTYPDto);

            return KDTYPDto;
        }

        public KDTYP ConvertToDomain(KDTYPDto dto)
        {
            KDTYP KDTYP;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            KDTYP = new KDTYP();
            MyMap(dto, KDTYP);

            return KDTYP;
        }
        #endregion

        #region IDtoAssembler<KDTYPDto,KDTYP> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(KDTYPDto fromKDTYPDto, KDTYP toKDTYP)
        {
            // Mapping
            toKDTYP.NAME = fromKDTYPDto.NAME;
            toKDTYP.ACTIVEFLAG = fromKDTYPDto.ACTIVEFLAG;
            toKDTYP.DESCRIPTION = fromKDTYPDto.DESCRIPTION;
            toKDTYP.SYSKDTYP = fromKDTYPDto.SYSKDTYP;
            toKDTYP.TYP = fromKDTYPDto.TYP;
            
            
        }

        private void MyMap(KDTYP fromKDTYP, KDTYPDto toKDTYPDto)
        {
            // Mapping

            toKDTYPDto.NAME = fromKDTYP.NAME;
            toKDTYPDto.ACTIVEFLAG = fromKDTYP.ACTIVEFLAG;
            toKDTYPDto.DESCRIPTION = fromKDTYP.DESCRIPTION;
            toKDTYPDto.SYSKDTYP = fromKDTYP.SYSKDTYP;
            toKDTYPDto.TYP = (int)fromKDTYP.TYP;
        }
        #endregion
    }
}