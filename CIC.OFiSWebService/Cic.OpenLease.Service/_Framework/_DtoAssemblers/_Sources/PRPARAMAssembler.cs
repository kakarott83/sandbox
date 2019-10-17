// OWNER MK, 16-03-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using System;
    using CIC.Database.OL.EF6.Model;
    #endregion

    [System.CLSCompliant(true)]
    public class PRPARAMAssembler : IDtoAssembler<PRPARAMDto, PRPARAM>
    {
        #region IDtoAssembler<PRPARAMDto,PRPARAM> Members

        public bool IsValid(PRPARAMDto dto)
        {
            throw new System.NotImplementedException();
        }

        public PRPARAM Create(PRPARAMDto dto)
        {
            throw new System.NotImplementedException();
        }

        public PRPARAM Update(PRPARAMDto dto)
        {
            throw new System.NotImplementedException();
        }

        public PRPARAMDto ConvertToDto(PRPARAM domain)
        {
            PRPARAMDto Dto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            Dto = new PRPARAMDto();

            MyMap(domain, Dto);

            return Dto;
        }

        public PRPARAM ConvertToDomain(PRPARAMDto dto)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region My Methods
        private void MyMap(PRPARAMDto dto, PRPARAM domain)
        {
            domain.DESCRIPTION = dto.DESCRIPTION;
            domain.NAME = dto.NAME;
            domain.TYP = dto.TYP;
            domain.STEPSIZE = dto.STEPSIZE;
            domain.MINVALN = dto.MINVALN;
            domain.MAXVALN = dto.MAXVALN;
            domain.DEFVALN = dto.DEFVALN;
            domain.MINVALP = dto.MINVALP;
            domain.MAXVALP = dto.MAXVALP;
            domain.DEFVALP = dto.DEFVALP;
            domain.MINVALD = dto.MINVALD;
            domain.MAXVALD = dto.MAXVALD;
            domain.DEFVALD = dto.DEFVALD;
            domain.STEPSIZE = domain.STEPSIZE;
            domain.VISIBILITYFLAG = dto.VISIBILITYFLAG;
            domain.DISABLEDFLAG = dto.DISABLEDFLAG;
            domain.PRFLD.SYSPRFLD = dto.SYSPRFLD;
            domain.PRFLD.NAME = dto.PRFLDNAME;
            domain.PRFLD.OBJECTMETA = dto.PRFLDOBJECTMETA;
        }

        private void MyMap(PRPARAM domain, PRPARAMDto dto)
        {
            dto.DESCRIPTION = domain.DESCRIPTION;
            dto.NAME = domain.NAME;
            dto.MINVALN = domain.MINVALN;
            dto.MAXVALN = domain.MAXVALN;
            dto.DEFVALN = domain.DEFVALN;
            dto.MINVALP = domain.MINVALP;
            dto.MAXVALP = domain.MAXVALP;
            dto.DEFVALP = domain.DEFVALP;
            dto.MINVALD = domain.MINVALD;
            dto.MAXVALD = domain.MAXVALD;
            dto.DEFVALD = domain.DEFVALD;
            dto.STEPSIZE = domain.STEPSIZE;
            dto.VISIBILITYFLAG = domain.VISIBILITYFLAG;
            dto.DISABLEDFLAG = domain.DISABLEDFLAG;
            dto.SYSPRFLD = domain.PRFLD.SYSPRFLD;
            dto.PRFLDNAME = domain.PRFLD.NAME;
            dto.PRFLDOBJECTMETA = domain.PRFLD.OBJECTMETA;
            dto.TYP = domain.TYP;
            dto.SYSPRPARAM = domain.SYSPRPARAM;
        }
        #endregion
    }
}
