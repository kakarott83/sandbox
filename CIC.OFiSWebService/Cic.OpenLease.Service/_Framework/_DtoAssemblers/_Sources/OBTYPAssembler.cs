// OWNER MP, 26-02-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class OBTYPAssembler : IDtoAssembler<OBTYPDto, OBTYP>
    {

        #region IDtoAssembler<OBTYPDto,OBTYP> Members

        public bool IsValid(OBTYPDto dto)
        {
            throw new System.NotImplementedException();
        }

        public OBTYP Create(OBTYPDto dto)
        {
            throw new System.NotImplementedException();
        }

        public OBTYP Update(OBTYPDto dto)
        {
            throw new System.NotImplementedException();
        }

        public OBTYPDto ConvertToDto(OBTYP domain)
        {
            OBTYPDto Dto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            Dto = new OBTYPDto();

            MyMap(domain, Dto);

            return Dto;
        }

        public OBTYP ConvertToDomain(OBTYPDto dto)
        {
            OBTYP Domain;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            Domain = new OBTYP();

            MyMap(dto, Domain);

            return Domain;
        }

        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        public static FahrzeugArt getFahrzeugArt(string aklasse)
        {
            switch(aklasse)
            {
                case ("10"): return FahrzeugArt.PKW;
                case ("20"): return FahrzeugArt.LKW;
                case ("40"): return FahrzeugArt.MOTORRAD;
                default: return FahrzeugArt.PKW;
            }
        }

       
        #endregion

        #region My methods
        private void MyMap(OBTYPDto dto, OBTYP domain)
        {
            domain.BEZEICHNUNG = dto.DESCRIPTION;
            domain.SYSOBTYP = dto.SYSOBTYP;
            // TODO MK: Implement
            // domain.SYSKALKTYP = dto.SYSKALKTYP;
        }

        private void MyMap(OBTYP domain, OBTYPDto dto)
        {
            dto.NAME = domain.BEZEICHNUNG;
            dto.SYSOBTYP = domain.SYSOBTYP;
            dto.FABRIKAT = domain.BEZEICHNUNG;
            //XXX change when field has been mapped
            dto.FAHRZEUGART = getFahrzeugArt(domain.AKLASSE);
            // TODO MK: Implement
            // dto.SYSKALKTYP = 0;
        }
        #endregion
    }
}

   
