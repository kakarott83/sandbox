// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.Merge.Dictionary;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class LANDAssembler : IDtoAssembler<LANDDto, LAND>
    {
        #region IDtoAssembler<LANDDto,LAND> Members (Methods)
        public bool IsValid(LANDDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public LAND Create(LANDDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public LAND Update(LANDDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public LANDDto ConvertToDto(LAND domain)
        {
            LANDDto LANDDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            LANDDto = new LANDDto();
            MyMap(domain, LANDDto);

            return LANDDto;
        }

        public LAND ConvertToDomain(LANDDto dto)
        {
            LAND LAND;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            LAND = new LAND();
            MyMap(dto, LAND);

            return LAND;
        }
        #endregion 

        #region IDtoAssembler<LANDDto,LAND> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(LANDDto fromLANDDto, LAND toLAND)
        {
            // Mapping
            // Ids            
            toLAND.SYSLAND = fromLANDDto.SYSLAND;
            toLAND.EG = fromLANDDto.EG;
            // Properties
            toLAND.COUNTRYNAME = fromLANDDto.COUNTRYNAME;
            toLAND.DEFAULTFLAG = fromLANDDto.DEFAULTFLAG;
            toLAND.BESTFIVEFLAG = fromLANDDto.BESTFIVEFLAG;
        }

        private void MyMap(LAND fromLAND, LANDDto toLANDDto)
        {
            // Mapping
            // Ids
            toLANDDto.SYSLAND = fromLAND.SYSLAND;
            toLANDDto.EG = fromLAND.EG;

            // Property
            toLANDDto.COUNTRYNAME = fromLAND.COUNTRYNAME;
            toLANDDto.DEFAULTFLAG = fromLAND.DEFAULTFLAG;
            toLANDDto.BESTFIVEFLAG = fromLAND.BESTFIVEFLAG;
        }
        #endregion
    }
}