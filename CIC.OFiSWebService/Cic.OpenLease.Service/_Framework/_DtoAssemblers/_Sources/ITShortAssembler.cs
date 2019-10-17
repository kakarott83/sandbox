// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class ITShortAssembler : IDtoAssembler<ITShortDto, IT>
    {
        #region IDtoAssembler<ITSearchResultDto,IT> Members (Methods)
        public bool IsValid(ITShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public IT Create(ITShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public IT Update(ITShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ITShortDto ConvertToDto(IT domain)
        {
            ITShortDto ITSearchResultDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            ITSearchResultDto = new ITShortDto();
            MyMap(domain, ITSearchResultDto);

            return ITSearchResultDto;
        }

        public IT ConvertToDomain(ITShortDto dto)
        {
            IT IT;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            IT = new IT();
            MyMap(dto, IT);

            return IT;
        }
        #endregion 

        #region IDtoAssembler<ITSearchResultDto,IT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(ITShortDto fromITSearchResultDto, IT toIT)
        {
            // Mapping
            // Ids            
            toIT.SYSIT = fromITSearchResultDto.SYSIT;
            toIT.SYSKDTYP = fromITSearchResultDto.SYSKDTYP;

            // Properties
            toIT.NAME = fromITSearchResultDto.NAME;
            toIT.VORNAME = fromITSearchResultDto.VORNAME;
            toIT.STRASSE = fromITSearchResultDto.STRASSE;
            toIT.HSNR = fromITSearchResultDto.HSNR;
            toIT.PLZ = fromITSearchResultDto.PLZ;
            toIT.ORT = fromITSearchResultDto.ORT;
            toIT.PTELEFON = fromITSearchResultDto.PTELEFON;
            toIT.TELEFON = fromITSearchResultDto.TELEFON;
            toIT.HANDY = fromITSearchResultDto.HANDY;
            toIT.FAX = fromITSearchResultDto.FAX;
            toIT.EMAIL = fromITSearchResultDto.EMAIL;
            toIT.BESCHARTAG1 = fromITSearchResultDto.BESCHARTAG1;
            toIT.GEBDATUM = fromITSearchResultDto.GEBDATUM;
            toIT.GRUENDUNG = fromITSearchResultDto.GRUENDUNG;
        }

        private void MyMap(IT fromIT, ITShortDto toITSearchResultDto)
        {
            // Mapping
            // Ids
            toITSearchResultDto.SYSIT = fromIT.SYSIT;
            toITSearchResultDto.SYSKDTYP = fromIT.SYSKDTYP;

            // Property
            toITSearchResultDto.NAME = fromIT.NAME;
            toITSearchResultDto.VORNAME = fromIT.VORNAME;
            toITSearchResultDto.STRASSE = fromIT.STRASSE;
            toITSearchResultDto.HSNR = fromIT.HSNR;
            toITSearchResultDto.PLZ = fromIT.PLZ;
            toITSearchResultDto.ORT = fromIT.ORT;
            toITSearchResultDto.PTELEFON = fromIT.PTELEFON;
            toITSearchResultDto.TELEFON = fromIT.TELEFON;
            toITSearchResultDto.HANDY = fromIT.HANDY;
            toITSearchResultDto.FAX = fromIT.FAX;
            toITSearchResultDto.EMAIL = fromIT.EMAIL;
            toITSearchResultDto.BESCHARTAG1 = fromIT.BESCHARTAG1;
            toITSearchResultDto.GEBDATUM = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.GEBDATUM);
            toITSearchResultDto.GRUENDUNG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromIT.GRUENDUNG);
        }
        #endregion
    }
}