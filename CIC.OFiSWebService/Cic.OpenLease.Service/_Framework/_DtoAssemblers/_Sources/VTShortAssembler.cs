// OWNER WB, 19-03-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;

    #endregion

    [System.CLSCompliant(true)]
    public class VTShortAssembler : IDtoAssemblerShortVT<VTShortDto, VT, OB, PERSON, OBHALTER, PERSON, KDTYP>
    {
        #region Methods
        public bool IsValid(VTShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public VT Create(VTShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public VT Update(VTShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public VTShortDto ConvertToDto(VT domain, OB ob, PERSON person, OBHALTER obhalter, PERSON verkaeufer, KDTYP kdtyp)
        {
            VTShortDto VTSearchResultDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            VTSearchResultDto = new VTShortDto();
            MyMap(domain, ob, person, obhalter,verkaeufer, kdtyp, VTSearchResultDto);

            return VTSearchResultDto;
        }

        public VT ConvertToDomain(VTShortDto dto, OB ob, PERSON person, OBHALTER obhalter, PERSON verkaeufer)
        {
            VT VT;
            OB OB;
            PERSON PERSON;
            OBHALTER OBHALTER;
           
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            VT = new VT();
            OB = new OB();
            PERSON = new PERSON();
            OBHALTER = new OBHALTER();
            
            MyMap(dto, VT, OB, PERSON, OBHALTER);

            return VT;
        }
        #endregion

        #region  Properties
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(VTShortDto fromVTSearchResultDto, VT toVT, OB toOB, PERSON toPERSON, OBHALTER toOBHALTER)
        {
            
            // Mapping
            // Ids
            fromVTSearchResultDto.VTSYSID = toVT.SYSID;
            fromVTSearchResultDto.VTSYSKD = toVT.SYSKD;

            // Property     
            toVT.BEGINN = fromVTSearchResultDto.VTBEGINN;
            toVT.ENDE = fromVTSearchResultDto.VTENDE;
            toVT.VART = fromVTSearchResultDto.VTVART;
            toVT.VERTRAG = fromVTSearchResultDto.VTVERTRAG;
            toVT.ZUSTAND = fromVTSearchResultDto.VTZUSTAND;
            toOB.KALK.ANZAHLUNG = fromVTSearchResultDto.KALKANZAHLUNG;
            toOB.KALK.BGEXTERN = fromVTSearchResultDto.KALKBGEXTERN;
            toOB.KALK.DEPOT = fromVTSearchResultDto.KALKDEPOT;
            toOB.JAHRESKM = fromVTSearchResultDto.OBJAHRESKM;
            toOB.KALK.LZ = fromVTSearchResultDto.KALKLZ;
            toOB.KALK.RATE = fromVTSearchResultDto.KALKRATE;
            toOB.KALK.RW = fromVTSearchResultDto.KALKRW;
            toOB.KALK.SZ = fromVTSearchResultDto.KALKSZ;
            toOB.FABRIKAT = fromVTSearchResultDto.OBFABRIKAT;
            toOBHALTER.KFZ = fromVTSearchResultDto.OBHALTERKFZ;
            toOB.HERSTELLER = fromVTSearchResultDto.OBHERSTELLER;
            toPERSON.NAME = fromVTSearchResultDto.PERSONNAME;
            toPERSON.ORT = fromVTSearchResultDto.PERSONORT;
            toPERSON.PLZ = fromVTSearchResultDto.PERSONPLZ;
            toPERSON.VORNAME = fromVTSearchResultDto.PERSONVORNAME;
            toPERSON.ZUSATZ = fromVTSearchResultDto.PERSONZUSATZ;
           
        }

        private void MyMap(VT fromVT, OB fromOB, PERSON fromPERSON, OBHALTER fromOBHALTER, PERSON fromVERKAEUFER, KDTYP kdtyp, VTShortDto toVTSearchResultDto)
        {
            // Mapping
            // Ids
            toVTSearchResultDto.VTSYSID = fromVT.SYSID;
            toVTSearchResultDto.VTSYSKD = fromPERSON.SYSPERSON;

            // Property     
            

            toVTSearchResultDto.VTBEGINN = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromVT.BEGINN);
            toVTSearchResultDto.VTENDE = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromVT.ENDE);
            toVTSearchResultDto.VTVART = fromVT.VART;
            toVTSearchResultDto.VTVERTRAG = fromVT.VERTRAG;
            toVTSearchResultDto.VTZUSTAND = fromVT.ZUSTAND;
            toVTSearchResultDto.KALKANZAHLUNG = fromOB.KALK.ANZAHLUNG;
            toVTSearchResultDto.KALKBGEXTERN = fromOB.KALK.BGEXTERN;
            toVTSearchResultDto.KALKDEPOT = fromOB.KALK.DEPOT;
            toVTSearchResultDto.OBJAHRESKM = fromOB.JAHRESKM;
            toVTSearchResultDto.KALKLZ = fromOB.KALK.LZ;
            toVTSearchResultDto.KALKRATE = fromOB.KALK.RATE;
            toVTSearchResultDto.KALKRW = fromOB.KALK.RW;
            toVTSearchResultDto.KALKSZ = fromOB.KALK.SZ;
            toVTSearchResultDto.OBFABRIKAT = fromOB.FABRIKAT;
            toVTSearchResultDto.OBHALTERKFZ = fromOBHALTER.KFZ;
            toVTSearchResultDto.OBHERSTELLER = fromOB.HERSTELLER;
            toVTSearchResultDto.PERSONNAME = fromPERSON.NAME;
            toVTSearchResultDto.PERSONORT = fromPERSON.ORT;
            toVTSearchResultDto.PERSONPLZ = fromPERSON.PLZ;
            toVTSearchResultDto.PERSONVORNAME = fromPERSON.VORNAME;
            toVTSearchResultDto.PERSONZUSATZ = fromPERSON.ZUSATZ;
            toVTSearchResultDto.VERKAEUFERNAME = fromVERKAEUFER.NAME;
            toVTSearchResultDto.VERKAEUFERVORNAME = fromVERKAEUFER.VORNAME;
            if (kdtyp != null)
            {
                toVTSearchResultDto.KDTYPNAME = kdtyp.NAME;
            }
            else toVTSearchResultDto.KDTYPNAME = "";
        }
        #endregion
    }
}