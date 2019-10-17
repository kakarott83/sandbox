// OWNER JJ, 10-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using System;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    #endregion

    [System.CLSCompliant(true)]
    public class ANGEBOTShortAssembler : IDtoAssemblerShortAngebot<ANGEBOTShortDto, ANGEBOT, IT, ANGKALK, ANGOB, PRPRODUCT, PERSON, ANGOBINI >
    {
        #region IDtoAssemblerAngebot<ANGEBOTSearchResultDto,ANGEBOT> Members (Methods)
        public bool IsValid(ANGEBOTShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ANGEBOT Create(ANGEBOTShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        public ANGEBOT Update(ANGEBOTShortDto dto)
        {
            // NOTE JJ, Not necessary
            throw new NotImplementedException();
        }

        
        public ANGEBOTShortDto ConvertToDto(ANGEBOT domain, IT it, ANGKALK angk, ANGOB ob, PRPRODUCT pr, PERSON pers, ANGOBINI obini)
        {
            ANGEBOTShortDto ANGEBOTSearchResultDto;

            
            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            ANGEBOTSearchResultDto = new ANGEBOTShortDto();
            MyMap(domain, ANGEBOTSearchResultDto, it, angk, ob, pr, pers, obini);

            return ANGEBOTSearchResultDto;
        }
        public ANGEBOTShortDto ConvertToDto(ANGEBOTShortDto ANGEBOTSearchResultDto, ANGEBOT domain, IT it, ANGKALK angk, ANGOB ob, PRPRODUCT pr, PERSON pers, ANGOBINI obini)
        {
            
            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            MyMap(domain, ANGEBOTSearchResultDto, it, angk, ob, pr, pers, obini);

            return ANGEBOTSearchResultDto;
        }

        public ANGEBOT ConvertToDomain(ANGEBOTShortDto dto, IT it, ANGKALK angk, ANGOB ob, PRPRODUCT pr, PERSON pers, ANGOBINI obini, DdOlExtended context)
        {
            ANGEBOT ANGEBOT;
            if (dto == null)
            {
                throw new ArgumentException("dto");
            }
            ANGEBOT = new ANGEBOT();
            MyMap(dto, ANGEBOT, it, angk, ob, pr, pers, obini,context);
            return ANGEBOT;
        }

     /*   public ANGEBOT GetAngebotFromShortDto(ANGEBOTShortDto angebotShortDto)
        {
            ANGEBOT ANGEBOT = new ANGEBOT();

            ANGEBOT.ANGEBOT1 = angebotShortDto.ANGEBOT1;
            ANGEBOT.DATANGEBOT = angebotShortDto.DATANGEBOT;
            ANGEBOT.ERFASSUNG = angebotShortDto.ERFASSUNG;
            ANGEBOT.OBJEKTVT = angebotShortDto.OBJEKTVT;
            ANGEBOT.SYSID = angebotShortDto.SYSID;
            ANGEBOT.SYSIT = angebotShortDto.SYSIT;
            ANGEBOT.SYSVK = angebotShortDto.SYSVK;
            ANGEBOT.SYSPRPRODUCT = angebotShortDto.SYSPRPRODUCT;
            if(angebotShortDto.SPECIALCALCCOUNT.HasValue)
                ANGEBOT.SPECIALCALCCOUNT = (int)angebotShortDto.SPECIALCALCCOUNT;
            ANGEBOT.ZUSTAND = angebotShortDto.ZUSTAND;
            
            ANGEBOT.SYSBRAND = angebotShortDto.SYSBRAND;
            if (angebotShortDto.ABTRETUNGVON!=null && angebotShortDto.ABTRETUNGVON.Length > 40)
                angebotShortDto.ABTRETUNGVON = angebotShortDto.ABTRETUNGVON.Substring(0, 40);
            ANGEBOT.ABTRETUNGVON = angebotShortDto.ABTRETUNGVON;
            ANGEBOT.SPECIALCALCSTATUS = angebotShortDto.SPECIALCALCSTATUS;
            return ANGEBOT;
        }*/
        
        #endregion 

        #region IDtoAssemblerAngebot<ANGEBOTSearchResultDto,ANGEBOT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            // NOTE JJ, Not necessary
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region My methods
        private void MyMap(ANGEBOTShortDto fromANGEBOTSearchResultDto, ANGEBOT toANGEBOT, IT toIT, ANGKALK toANGKALK, ANGOB toANGOB, PRPRODUCT toPRPRODUCT, PERSON toPERSON, ANGOBINI toANGOBINI, DdOlExtended context)
        {
            // Mapping
            // Ids            
            toANGEBOT.SYSID = fromANGEBOTSearchResultDto.SYSID;
            
            // Properties
            toANGEBOT.ANGEBOT1 = fromANGEBOTSearchResultDto.ANGEBOT1;
            toANGEBOT.DATANGEBOT = fromANGEBOTSearchResultDto.DATANGEBOT;
            toANGEBOT.ZUSTAND = fromANGEBOTSearchResultDto.ZUSTAND;
            toANGEBOT.ERFASSUNG = fromANGEBOTSearchResultDto.ERFASSUNG;
            toANGEBOT.SYSPRPRODUCT = fromANGEBOTSearchResultDto.SYSPRPRODUCT;
            toANGEBOT.SYSIT = fromANGEBOTSearchResultDto.SYSIT;
            toANGEBOT.SYSVK = fromANGEBOTSearchResultDto.SYSVK;
            if(fromANGEBOTSearchResultDto.SYSBRAND.HasValue)
            toANGEBOT.SYSBRAND=(long)fromANGEBOTSearchResultDto.SYSBRAND; 
            if (fromANGEBOTSearchResultDto.ABTRETUNGVON!=null && fromANGEBOTSearchResultDto.ABTRETUNGVON.Length > 40)
                fromANGEBOTSearchResultDto.ABTRETUNGVON = fromANGEBOTSearchResultDto.ABTRETUNGVON.Substring(0, 40);
            toANGEBOT.ABTRETUNGVON = fromANGEBOTSearchResultDto.ABTRETUNGVON;

            toIT.NAME = fromANGEBOTSearchResultDto.ITNAME;
            toIT.ORT = fromANGEBOTSearchResultDto.ITORT;
            toIT.PLZ = fromANGEBOTSearchResultDto.ITPLZ;
            toIT.VORNAME = fromANGEBOTSearchResultDto.ITVORNAME;
            toANGEBOT.OBJEKTVT = fromANGEBOTSearchResultDto.OBJEKTVT;
            toPRPRODUCT.NAME = fromANGEBOTSearchResultDto.PRPRODUCTNAME;
            toANGKALK.LZ = fromANGEBOTSearchResultDto.ANGKALKLZ;
            toANGKALK.RATE = fromANGEBOTSearchResultDto.ANGKALKRATE;  
            toANGOB.JAHRESKM = fromANGEBOTSearchResultDto.ANGOBJAHRESKM;
            toANGKALK.DEPOT = fromANGEBOTSearchResultDto.ANGKALKDEPOT;
            toPERSON.NAME = fromANGEBOTSearchResultDto.VERKAUFERNAME;
            toPERSON.VORNAME = fromANGEBOTSearchResultDto.VERKAUFERVORNAME;
            toANGKALK.AHK = fromANGEBOTSearchResultDto.AHKEXTERNBRUTTO;
            toANGKALK.RWKALKBRUTTO = fromANGEBOTSearchResultDto.RWKALKBRUTTO;

            toANGKALK.AHK = fromANGEBOTSearchResultDto.AHKEXTERNBRUTTO;
            toANGKALK.BGEXTERNBRUTTO = fromANGEBOTSearchResultDto.ANGKALKBGEXTERNBRUTTO;
        }

        private void MyMap(ANGEBOT fromANGEBOT, ANGEBOTShortDto toANGEBOTSearchResultDto, IT fromIT, ANGKALK fromANGKALK, ANGOB fromANGOB, PRPRODUCT fromPRPRODUCT, PERSON fromPERSON, ANGOBINI fromANGOBINI)
        {
            // Mapping
            // Ids
            toANGEBOTSearchResultDto.SYSID = fromANGEBOT.SYSID;
            
            // Property
            toANGEBOTSearchResultDto.ANGEBOT1 = fromANGEBOT.ANGEBOT1;
            toANGEBOTSearchResultDto.DATANGEBOT = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromANGEBOT.DATANGEBOT);
            toANGEBOTSearchResultDto.ERFASSUNG = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromANGEBOT.ERFASSUNG);
            toANGEBOTSearchResultDto.ZUSTAND = fromANGEBOT.ZUSTAND;
            toANGEBOTSearchResultDto.SYSIT = fromANGEBOT.SYSIT.GetValueOrDefault();
            toANGEBOTSearchResultDto.SYSPRPRODUCT = fromANGEBOT.SYSPRPRODUCT.GetValueOrDefault();
            toANGEBOTSearchResultDto.SYSVK = fromPERSON.SYSPERSON;
            toANGEBOTSearchResultDto.ITNAME = fromIT.NAME;
            toANGEBOTSearchResultDto.ITORT = fromIT.ORT;
            toANGEBOTSearchResultDto.ITPLZ = fromIT.PLZ;
            toANGEBOTSearchResultDto.ITVORNAME = fromIT.VORNAME;
            toANGEBOTSearchResultDto.OBJEKTVT = fromANGEBOT.OBJEKTVT;
            toANGEBOTSearchResultDto.PRPRODUCTNAME = fromPRPRODUCT.NAME;
            toANGEBOTSearchResultDto.ANGKALKLZ = fromANGKALK.LZ;
            toANGEBOTSearchResultDto.ANGOBJAHRESKM = fromANGOB.JAHRESKM;
            toANGEBOTSearchResultDto.ANGKALKRATE = fromANGKALK.RATEBRUTTO;            
            toANGEBOTSearchResultDto.ANGKALKSZ = fromANGKALK.SZ;
            toANGEBOTSearchResultDto.ANGKALKSZBRUTTO = fromANGKALK.SZBRUTTO;
            toANGEBOTSearchResultDto.ANGKALKDEPOT = fromANGKALK.DEPOT;         
            toANGEBOTSearchResultDto.VERKAUFERNAME = fromPERSON.NAME;
            toANGEBOTSearchResultDto.VERKAUFERVORNAME = fromPERSON.VORNAME;
            toANGEBOTSearchResultDto.AHKEXTERNBRUTTO = fromANGKALK.AHK;
            toANGEBOTSearchResultDto.ANGKALKBGEXTERNBRUTTO =  fromANGKALK.BGEXTERNBRUTTO;
            toANGEBOTSearchResultDto.RWKALKBRUTTO = fromANGKALK.RWKALKBRUTTO;
            toANGEBOTSearchResultDto.SYSBRAND = fromANGEBOT.SYSBRAND.GetValueOrDefault();
            toANGEBOTSearchResultDto.SPECIALCALCCOUNT = fromANGEBOT.SPECIALCALCCOUNT;
            toANGEBOTSearchResultDto.SPECIALCALCSTATUS = fromANGEBOT.SPECIALCALCSTATUS;
            if (!toANGEBOTSearchResultDto.SPECIALCALCSTATUS.HasValue || toANGEBOTSearchResultDto.SPECIALCALCSTATUS == 0)
                toANGEBOTSearchResultDto.SPECIALCALCSTATUSTEXT = "";
            else if (toANGEBOTSearchResultDto.SPECIALCALCSTATUS == 1)
                toANGEBOTSearchResultDto.SPECIALCALCSTATUSTEXT = "angefordert";
            else if (toANGEBOTSearchResultDto.SPECIALCALCSTATUS == 2)
                toANGEBOTSearchResultDto.SPECIALCALCSTATUSTEXT = "in Bearbeitung";
            else if (toANGEBOTSearchResultDto.SPECIALCALCSTATUS == 3)
                toANGEBOTSearchResultDto.SPECIALCALCSTATUSTEXT = "durchgeführt";

            toANGEBOTSearchResultDto.ABTRETUNGVON = fromANGEBOT.ABTRETUNGVON;
        }
        #endregion
    }
}