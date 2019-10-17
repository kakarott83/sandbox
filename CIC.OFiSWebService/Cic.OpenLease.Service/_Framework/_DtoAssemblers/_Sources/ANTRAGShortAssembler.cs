// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public class ANTRAGShortAssembler : IDtoAssemblerShortAntrag<ANTRAGShortDto, ANTRAG, ANTOB, PERSON>
    {
        #region IDtoAssembler<ANTRAGSearchResultDto,ANTRAG> Members (Methods)
        public bool IsValid(ANTRAGShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ANTRAG Create(ANTRAGShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ANTRAG Update(ANTRAGShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ANTRAGShortDto ConvertToDto(ANTRAG domain, ANTOB antob, PERSON person)
        {
            ANTRAGShortDto ANTRAGSearchResultDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            ANTRAGSearchResultDto = new ANTRAGShortDto();
            MyMap(domain, antob, person, ANTRAGSearchResultDto);

            return ANTRAGSearchResultDto;
        }

        public ANTRAG ConvertToDomain(ANTRAGShortDto dto, ANTOB antob, PERSON person)
        {
            ANTRAG ANTRAG;
            ANTOB ANTOB;
            PERSON PERSON;
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            ANTRAG = new ANTRAG();
            ANTOB = new ANTOB();
            PERSON = new PERSON();
            MyMap(dto, ANTRAG, ANTOB, PERSON);

            return ANTRAG;
        }

        public ANTRAG GetAntragFromShortDto(ANTRAGShortDto antragShortDto)
        {
            ANTRAG ANTRAG = new ANTRAG();
            ANTRAG.ANTRAG1 = antragShortDto.ANTRAG1;
            ANTRAG.SYSID = antragShortDto.SYSID;
            ANTRAG.VART = antragShortDto.VART;
            ANTRAG.ZUSTAND = antragShortDto.ZUSTAND;
            ANTRAG.DATANGEBOT = antragShortDto.DATANGEBOT;
            return ANTRAG;
        }
        #endregion 

        #region IDtoAssembler<ANTRAGSearchResultDto,ANTRAG> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(ANTRAGShortDto fromANTRAGSearchResultDto, ANTRAG toANTRAG, ANTOB toANTOB, PERSON toPERSON)
        {
            // Mapping
            // Ids            
            toANTRAG.SYSID = fromANTRAGSearchResultDto.SYSID;

            // Properties
            toANTRAG.ANTRAG1 = fromANTRAGSearchResultDto.ANTRAG1;
            toANTOB.ANTKALK.BGEXTERN = fromANTRAGSearchResultDto.ANTKALKBGEXTERN;
            toANTOB.ANTKALK.DEPOT = fromANTRAGSearchResultDto.ANTKALKDEPOT;
            toANTOB.ANTKALK.LZ = fromANTRAGSearchResultDto.ANTKALKLZ;
            toANTOB.ANTKALK.RATE = fromANTRAGSearchResultDto.ANTKALKRATE;
            toANTOB.ANTKALK.RW = fromANTRAGSearchResultDto.ANTKALKRW;
            toANTOB.ANTKALK.SZ = fromANTRAGSearchResultDto.ANTKALKSZ;
            toANTOB.FABRIKAT = fromANTRAGSearchResultDto.ANTOBFABRIKAT;
            toANTOB.HERSTELLER = fromANTRAGSearchResultDto.ANTOBHERSTELLER;
            toANTOB.JAHRESKM = fromANTRAGSearchResultDto.ANTOBJAHRESKM;
            toPERSON.NAME = fromANTRAGSearchResultDto.PERSONNAME;
            toPERSON.ORT = fromANTRAGSearchResultDto.PERSONORT;
            toPERSON.PLZ = fromANTRAGSearchResultDto.PERSONPLZ;
            toPERSON.VORNAME = fromANTRAGSearchResultDto.PERSONVORNAME;
            toPERSON.ZUSATZ = fromANTRAGSearchResultDto.PERSONZUSATZ;
            toANTRAG.VART = fromANTRAGSearchResultDto.VART;
            toANTRAG.ZUSTAND = fromANTRAGSearchResultDto.ZUSTAND;
        }

        private void MyMap(ANTRAG fromANTRAG, ANTOB fromANTOB, PERSON fromPERSON, ANTRAGShortDto toANTRAGSearchResultDto)
        {
            // Mapping
            // Ids
            toANTRAGSearchResultDto.SYSID = fromANTRAG.SYSID;
            toANTRAGSearchResultDto.DATANGEBOT = fromANTRAG.DATANGEBOT;
            // Property     
            toANTRAGSearchResultDto.ANTRAG1 = fromANTRAG.ANTRAG1;
            if (fromANTOB != null)
            {
                if (fromANTOB.ANTKALK != null)
                {
                    toANTRAGSearchResultDto.ANTKALKBGEXTERN = fromANTOB.ANTKALK.BGEXTERN;
                    toANTRAGSearchResultDto.ANTKALKDEPOT = fromANTOB.ANTKALK.DEPOT;
                    toANTRAGSearchResultDto.ANTKALKLZ = fromANTOB.ANTKALK.LZ;
                    toANTRAGSearchResultDto.ANTKALKRATE = fromANTOB.ANTKALK.RATE;
                    toANTRAGSearchResultDto.ANTKALKRW = fromANTOB.ANTKALK.RW;
                    toANTRAGSearchResultDto.ANTKALKSZ = fromANTOB.ANTKALK.SZ.HasValue && fromANTOB.ANTKALK.SZ.Value > 0 ? fromANTOB.ANTKALK.SZ : fromANTOB.ANTKALK.ANZAHLUNG;
                }
                toANTRAGSearchResultDto.ANTOBFABRIKAT = fromANTOB.FABRIKAT;
                toANTRAGSearchResultDto.ANTOBHERSTELLER = fromANTOB.HERSTELLER;
                toANTRAGSearchResultDto.ANTOBJAHRESKM = fromANTOB.JAHRESKM;
            }
            if (fromPERSON != null)
            {
                toANTRAGSearchResultDto.PERSONNAME = fromPERSON.NAME;
                toANTRAGSearchResultDto.PERSONORT = fromPERSON.ORT;
                toANTRAGSearchResultDto.PERSONPLZ = fromPERSON.PLZ;
                toANTRAGSearchResultDto.PERSONVORNAME = fromPERSON.VORNAME;
                toANTRAGSearchResultDto.PERSONZUSATZ = fromPERSON.ZUSATZ;
            }
            
            toANTRAGSearchResultDto.VART = fromANTRAG.VART;
            toANTRAGSearchResultDto.ZUSTAND = fromANTRAG.ZUSTAND;
            
        }
        #endregion
    }
}