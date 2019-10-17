// OWNER JJ, 15-12-2009
namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdOl;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class ANTRAGAssembler : IDtoAssemblerAntrag<ANTRAGDto, ANTRAG, ANTKALK, ANTOB, PERSON, KDTYP>
    {
        #region Private variables
        private System.Collections.Generic.Dictionary<string, string> _Errors;
        private long? _SysPEROLE;
        #endregion

        #region Constructors
        public ANTRAGAssembler(long? sysPEROLE)
        {
            _SysPEROLE = sysPEROLE;
            _Errors = new System.Collections.Generic.Dictionary<string, string>();
        }
        #endregion

        #region IDtoAssembler<ANGEBOTDto,ANGEBOT> Members (Methods)
        public bool IsValid(ANTRAGDto dto)
        {
            // NOTE WB, Not necessary
            throw new NotImplementedException();       
        }

        public void Create(ANTRAGDto dto)
        {
            // NOTE WB, Not necessary
            throw new NotImplementedException();
        }

        public void Update(ANTRAGDto dto)
        {
            // NOTE WB, Not necessary
            throw new NotImplementedException();
        }

        public ANTRAGDto ConvertToDto(ANTRAG domain, ANTKALK antkalk, ANTOB antob, PERSON person, KDTYP typ, DdOlExtended context)
        {
            ANTRAGDto ANTRAGDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            ANTRAGDto = new ANTRAGDto();
            MyMap(domain, antkalk, antob, person, ANTRAGDto,  context);

            return ANTRAGDto;
        }

        public ANTRAG ConvertToDomain(ANTRAGDto dto, ANTKALK antkalk, ANTOB antob, PERSON person)
        {
            ANTRAG ANTRAG;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            ANTRAG = new ANTRAG();
            MyMap(dto, antkalk, antob, ANTRAG, person);

            return ANTRAG;
        }
        #endregion

        #region IDtoAssembler<ANGEBOTDto,ANGEBOT> Members (Properties)
        public System.Collections.Generic.Dictionary<string, string> Errors
        {
            get
            {
                return _Errors;
            }
        }
        #endregion

        #region My methods
        private void MyMap(ANTRAGDto fromAntragDto, ANTKALK toAntkalk, ANTOB toAntob, ANTRAG toAntrag, PERSON toPerson)
        {
            //Ids
            /*toAntrag.SYSIT = (fromAntragDto.SYSIT.HasValue && fromAntragDto.SYSIT.Value < 1L ? null : fromAntragDto.SYSIT); ;

            //Properties
            toAntrag.ANTRAG1 = fromAntragDto.ANTRAG1;
            toAntrag.BEGINN = fromAntragDto.BEGINN;
            toAntrag.ENDE = fromAntragDto.ENDE;
            toAntrag.ERFASSUNG = fromAntragDto.ERFASSUNG;
            toAntrag.ZUSTAND = fromAntragDto.ZUSTAND;
            toAntrag.ZUSTANDAM = fromAntragDto.ZUSTANDAM;
            toAntkalk.BGEXTERN = fromAntragDto.ANTKALKBGEXTERN;

            //If line is commented it means that it's not in db
            //toAntkalk.BGEXTERNNACHLBRUTTO = fromAntragDto.ANTKALKBGEXTERNNACHLBRUTTO;
            //toAntkalk.BGEXTERNBRUTTO = fromAntragDto.ANTKALKBGEXTERNBRUTTO;
            //toAntkalk.BGEXTERNUST = fromAntragDto.ANTKALKBGEXTERNUST;
            toAntkalk.DEPOT = fromAntragDto.ANTKALKDEPOT;
            //toAntkalk.DEPOTP = fromAntragDto.ANTKALKDEPOTP;

            toAntkalkfs.ANABMLDFLAG = fromAntragDto.ANTKALKFSANABMLDFLAG;
            toAntkalkfs.INSASSENFLAG = fromAntragDto.ANTKALKFSINSASSENFLAG;
            //toAntkalkfs.ANABBRUTTO = fromAntragDto.ANTKALKFSANABBRUTTO;
            toAntkalkfs.ANABMELDUNG = fromAntragDto.ANTKALKFSANABMELDUNG;
            toAntkalkfs.ANABMLDFLAG = fromAntragDto.ANTKALKFSANABMLDFLAG;
            //toAntkalkfs.ANABUST = fromAntragDto.ANTKALKFSANABUST;
            //toAntkalkfs.EXTRASBRUTTO = fromAntragDto.ANTKALKFSEXTRASBRUTTO;
            toAntkalkfs.EXTRASFLAG = fromAntragDto.ANTKALKFSEXTRASFLAG;
            toAntkalkfs.EXTRASPRICE = fromAntragDto.ANTKALKFSEXTRASPRICE;
            //toAntkalkfs.EXTRASUST = fromAntragDto.ANTKALKFSEXTRASUST;
            //toAntkalkfs.FUELBRUTTO = fromAntragDto.ANTKALKFSFUELBRUTTO;
            toAntkalkfs.FUELFLAG = fromAntragDto.ANTKALKFSFUELFLAG;
            toAntkalkfs.FUELPRICE = fromAntragDto.ANTKALKFSFUELPRICE;
            //toAntkalkfs.FUELUST = fromAntragDto.ANTKALKFSFUELUST;
            toAntkalkfs.HPFLAG = fromAntragDto.ANTKALKFSHPFLAG;
            toAntkalkfs.INSASSENFLAG = fromAntragDto.ANTKALKFSINSASSENFLAG;
            toAntkalkfs.MAINTENANCE = fromAntragDto.ANTKALKFSMAINTENANCE;
            //toAntkalkfs.MAINTENANCEBRUTTO = fromAntragDto.ANTKALKFSMAINTENANCEBRUTTO;
            toAntkalkfs.MAINTENANCEFLAG = fromAntragDto.ANTKALKFSMAINTENANCEFLAG;
            //toAntkalkfs.MAINTENANCEUST = fromAntragDto.ANTKALKFSMAINTENANCEUST;
            toAntkalkfs.MAINTFIXFLAG = fromAntragDto.ANTKALKFSMAINTFIXFLAG;
            toAntkalkfs.MEHRKM = fromAntragDto.ANTKALKFSMEHRKM;
            toAntkalkfs.MINDERKM = fromAntragDto.ANTKALKFSMINDERKM;
            toAntkalkfs.RECHTSCHUTZFLAG = fromAntragDto.ANTKALKFSRECHTSCHUTZFLAG;
            toAntkalkfs.REPCARCOUNT = fromAntragDto.ANTKALKFSREPCARCOUNT;
            toAntkalkfs.REPCARFLAG = fromAntragDto.ANTKALKFSREPCARFLAG;
            toAntkalkfs.REPCARPRICE = fromAntragDto.ANTKALKFSREPCARPRICE;
            //toAntkalkfs.REPCARRATE = fromAntragDto.ANTKALKFSREPCARRATE;
            //toAntkalkfs.REPCARRATEBRUTTO = fromAntragDto.ANTKALKFSREPCARRATEBRUTTO;
            //toAntkalkfs.REPCARRATEUST = fromAntragDto.ANTKALKFSREPCARRATEUST;
            toAntkalkfs.RIMSCODEH = fromAntragDto.ANTKALKFSRIMSCODEH;
            toAntkalkfs.RIMSCODEV = fromAntragDto.ANTKALKFSRIMSCODEV;
            toAntkalkfs.RIMSCOUNTH = fromAntragDto.ANTKALKFSRIMSCOUNTH;
            toAntkalkfs.RIMSCOUNTV = fromAntragDto.ANTKALKFSRIMSCOUNTV;
            toAntkalkfs.RIMSPRICEH = fromAntragDto.ANTKALKFSRIMSPRICEH;
            toAntkalkfs.RIMSPRICEV = fromAntragDto.ANTKALKFSRIMSPRICEV;
            toAntkalkfs.STIRESCODEH = fromAntragDto.ANTKALKFSSTIRESCODEH;
            toAntkalkfs.STIRESCODEV = fromAntragDto.ANTKALKFSSTIRESCODEV;
            toAntkalkfs.STIRESCOUNTH = fromAntragDto.ANTKALKFSSTIRESCOUNTH;
            toAntkalkfs.STIRESCOUNTV = fromAntragDto.ANTKALKFSSTIRESCOUNTV;
            toAntkalkfs.STIRESMODH = fromAntragDto.ANTKALKFSSTIRESMODH;
            toAntkalkfs.STIRESMODV = fromAntragDto.ANTKALKFSSTIRESMODV;
            toAntkalkfs.STIRESPRICE = fromAntragDto.ANTKALKFSSTIRESPRICE;
            toAntkalkfs.STIRESPRICEH = fromAntragDto.ANTKALKFSSTIRESPRICEH;
            //toAntkalkfs.STIRESPRICEHBRUTTO = fromAntragDto.ANTKALKFSSTIRESPRICEHBRUTTO;
            //toAntkalkfs.STIRESPRICEHUST = fromAntragDto.ANTKALKFSSTIRESPRICEHUST;
            toAntkalkfs.STIRESPRICEV = fromAntragDto.ANTKALKFSSTIRESPRICEV;
            //toAntkalkfs.STIRESPRICEVBRUTTO = fromAntragDto.ANTKALKFSSTIRESPRICEVBRUTTO;
            //toAntkalkfs.STIRESPRICEVUST = fromAntragDto.ANTKALKFSSTIRESPRICEVUST;
            toAntkalkfs.TIRESADDITION = fromAntragDto.ANTKALKFSTIRESADDITION;
            //toAntkalkfs.TIRESADDITIONBRUTTO = fromAntragDto.ANTKALKFSTIRESADDITIONBRUTTO;
            //toAntkalkfs.TIRESADDITIONUST = fromAntragDto.ANTKALKFSTIRESADDITIONUST;
            toAntkalkfs.TIRESFIXFLAG = fromAntragDto.ANTKALKFSTIRESFIXFLAG;
            toAntkalkfs.TIRESFLAG = fromAntragDto.ANTKALKFSTIRESFLAG;
            toAntkalkfs.VKFLAG = fromAntragDto.ANTKALKFSVKFLAG;
            toAntkalkfs.WTIRESCODEH = fromAntragDto.ANTKALKFSWTIRESCODEH;
            toAntkalkfs.WTIRESCODEV = fromAntragDto.ANTKALKFSWTIRESCODEV;
            toAntkalkfs.WTIRESCOUNTH = fromAntragDto.ANTKALKFSWTIRESCOUNTH;
            toAntkalkfs.WTIRESCOUNTV = fromAntragDto.ANTKALKFSWTIRESCOUNTV;
            toAntkalkfs.WTIRESMODH = fromAntragDto.ANTKALKFSWTIRESMODH;
            toAntkalkfs.WTIRESMODV = fromAntragDto.ANTKALKFSWTIRESMODV;
            toAntkalkfs.WTIRESPRICEH = fromAntragDto.ANTKALKFSWTIRESPRICEH;
            //toAntkalkfs.WTIRESPRICEHBRUTTO = fromAntragDto.ANTKALKFSWTIRESPRICEHBRUTTO;
            //toAntkalkfs.WTIRESPRICEHUST = fromAntragDto.ANTKALKFSWTIRESPRICEHUST;
            toAntkalkfs.WTIRESPRICEV = fromAntragDto.ANTKALKFSWTIRESPRICEV;
            //toAntkalkfs.WTIRESPRICEVBRUTTO = fromAntragDto.ANTKALKFSWTIRESPRICEVBRUTTO;
            //toAntkalkfs.WTIRESPRICEVUST = fromAntragDto.ANTKALKFSWTIRESPRICEVUST;


            toAntkalk.GEBUEHR = fromAntragDto.ANTKALKGEBUEHR;
            //toAntkalk.GEBUEHRBRUTTO = fromAntragDto.ANTKALKGEBUEHRBRUTTO;
            //toAntkalk.GRUNDBRUTTO = fromAntragDto.ANTKALKGRUNDBRUTTO;
            //toAntkalk.GRUNDNACHLBRUTTO = fromAntragDto.ANTKALKGRUNDNACHLBRUTTO;
            //toAntkalk.GRUNDNETTO = fromAntragDto.ANTKALKGRUNDNETTO;
            //toAntkalk.HERZUBBRUTTO = fromAntragDto.ANTKALKHERZUBBRUTTO;
            //toAntkalk.HERZUBNACHLBRUTTO = fromAntragDto.ANTKALKHERZUBNACHLBRUTTO;
            //toAntkalk.HERZUBNETTO = fromAntragDto.ANTKALKHERZUBNETTO;
            //toAntkalk.HERZUBRABO = fromAntragDto.ANTKALKHERZUBRABO;
            //toAntkalk.HERZUBRABOP = fromAntragDto.ANTKALKHERZUBRABOP;
            toAntob.JAHRESKM = fromAntragDto.ANTOBJAHRESKM;
            toAntkalk.LZ = fromAntragDto.ANTKALKLZ;
            //toAntkalk.MITFINBRUTTO = fromAntragDto.ANTKALKMITFINBRUTTO;
            //toAntkalk.MITFINUST = fromAntragDto.ANTKALKMITFINUST;
            //toAntkalk.PAKETEBRUTTO = fromAntragDto.ANTKALKPAKETEBRUTTO;
            //toAntkalk.PAKETENETTO = fromAntragDto.ANTKALKPAKETENETTO;
            //toAntkalk.PAKETEUST = fromAntragDto.ANTKALKPAKETEUST;
            //toAntkalk.PAKETENACHLBRUTTO = fromAntragDto.ANTKALKPAKETENACHLBRUTTO;
            toAntkalk.PAKRABO = fromAntragDto.ANTKALKPAKRABO;
            toAntkalk.PAKRABOP = fromAntragDto.ANTKALKPAKRABOP;
            toAntkalk.PPY = fromAntragDto.ANTKALKPPY;
            toAntkalk.RABATTO = fromAntragDto.ANTKALKRABATTO;
            toAntkalk.RABATTOP = fromAntragDto.ANTKALKRABATTOP;
            toAntkalk.RATE = fromAntragDto.ANTKALKRATE;
            //toAntkalk.RATEBRUTTO = fromAntragDto.ANTKALKRATEBRUTTO;
            //toAntkalk.RATEUST = fromAntragDto.ANTKALKRATEUST;
            toAntkalk.RGGEBUEHR = fromAntragDto.ANTKALKRGGEBUEHR;
            //toAntkalk.RGGFREI = fromAntragDto.ANTKALKRGGFREI;
            toAntkalk.RGGVERR = fromAntragDto.ANTKALKRGGVERR;
            toAntkalk.RWBASE = fromAntragDto.ANTKALKRWBASE;
            //toAntkalk.RWBASEBRUTTO = fromAntragDto.ANTKALKRWBASEBRUTTO;
            //toAntkalk.RWBASEBRUTTOP = fromAntragDto.ANTKALKRWBASEBRUTTOP;
            //toAntkalk.RWBASEUST = fromAntragDto.ANTKALKRWBASEUST;
            toAntkalk.RWCRV = fromAntragDto.ANTKALKRWCRV;
            //toAntkalk.RWCRVBRUTTO = fromAntragDto.ANTKALKRWCRVBRUTTO;
            //toAntkalk.RWCRVBRUTTOP = fromAntragDto.ANTKALKRWCRVBRUTTOP;
            //toAntkalk.RWCRVUST = fromAntragDto.ANTKALKRWCRVUST;
            toAntkalk.RWKALK = fromAntragDto.ANTKALKRWKALK;
            //toAntkalk.RWKALKBRUTTO = fromAntragDto.ANTKALKRWKALKBRUTTO;
            //toAntkalk.RWKALKBRUTTOP = fromAntragDto.ANTKALKRWKALKBRUTTOP;
            //toAntkalk.RWKALKUST = fromAntragDto.ANTKALKRWKALKUST;
            //toAntkalk.SONZUBBRUTTO = fromAntragDto.ANTKALKSONZUBBRUTTO;
            //toAntkalk.SONZUBNACHLBRUTTO = fromAntragDto.ANTKALKSONZUBNACHLBRUTTO;
            //toAntkalk.SONZUBNETTO = fromAntragDto.ANTKALKSONZUBNETTO;
            toAntkalk.SONZUBRABO = fromAntragDto.ANTKALKSONZUBRABO;
            toAntkalk.SONZUBRABOP = fromAntragDto.ANTKALKSONZUBRABOP;
            toAntkalk.SZ = fromAntragDto.ANTKALKSZ;
            //toAntkalk.SZBRUTTO = fromAntragDto.ANTKALKSZBRUTTO;
            //toAntkalk.SZBRUTTOP = fromAntragDto.ANTKALKSZBRUTTOP;
            //toAntkalk.SZUST = fromAntragDto.ANTKALKSZUST;
            toAntkalk.ZUBEHOERBRUTTO = fromAntragDto.ANTKALKZUBEHOERBRUTTO;
            toAntkalk.ZUBEHOERNETTO = fromAntragDto.ANTKALKZUBEHOERNETTO;
            toAntkalk.ZUBEHOEROR = fromAntragDto.ANTKALKZUBEHOEROR;
            toAntkalk.ZUBEHOERORP = fromAntragDto.ANTKALKZUBEHOERORP;
            toAntob.AUTOMATIK = fromAntragDto.ANTOBAUTOMATIK;
            toAntob.FABRIKAT = fromAntragDto.ANTOBFABRIKAT;
            toAntob.FARBEA = fromAntragDto.ANTOBFARBEA;
            toAntob.FZART = fromAntragDto.ANTOBFZART;
            toAntob.FZNR = fromAntragDto.ANTOBFZNR;
            toAntob.HERSTELLER = fromAntragDto.ANTOBHERSTELLER;
            toAntobini.ACTUATION = fromAntragDto.ANTOBINIACTUATION;
            toAntob.BAUJAHR = fromAntragDto.ANTOBBAUJAHR;
            toAntob.CCM = fromAntragDto.ANTOBCCM;
            toAntobini.CO2 = fromAntragDto.ANTOBINICO2;
            toAntobini.KMSTAND = fromAntragDto.ANTOBINIKMSTAND;
            toAntobini.NOX = fromAntragDto.ANTOBININOX;
            toAntobini.PARTICLES = fromAntragDto.ANTOBINIPARTICLES;
            toAntobini.VERBRAUCH_D = fromAntragDto.ANTOBINIVERBRAUCH_D;
            toAntobini.VORBESITZER = fromAntragDto.ANTOBINIVORBESITZER;
            toAntob.KMTOLERANZ = fromAntragDto.ANTOBKMTOLERANZ;
            toAntob.KW = fromAntragDto.ANTOBKW;
            toAntob.LIEFERUNG = fromAntragDto.ANTOBLIEFERUNG;
            toAntob.NOVA = fromAntragDto.ANTOBNOVA;
            toAntob.NOVAP = fromAntragDto.ANTOBNOVAP;
            //toAngebotDto.ANTOBNOVAUST = fromAngob.
            //toAngebotDto.ANTOBNOVAZUAB
            //toAngebotDto.ANTOBNOVAZUABBRUTTO
            //toAngebotDto.ANTOBNOVAZUABUST
            //toAngebotDto.ANTOBSATZMEHRKM = fromAngob
            //toAngebotDto.ANTOBSATZMINDERKM = fromAngob
            toAntob.SERIE = fromAntragDto.ANTOBSERIE;
            //toAngebotDto.ANTOBSYSOBKAT = fromAngob.OBKAT.SYSOBKAT;
            toAntob.TYP = fromAntragDto.ANTOBTYP;

            */
        }

        private void MyMap(ANTRAG fromAntrag, ANTKALK fromAntkalk, ANTOB fromAntob, PERSON fromPerson,
            ANTRAGDto toAntragDto, DdOlExtended context)
        {
            //Ids
            toAntragDto.SysId = fromAntrag.SYSID;
            toAntragDto.ZUSTAND = fromAntrag.ZUSTAND;
            toAntragDto.VART = fromAntrag.VART;
            toAntragDto.ANTRAG = fromAntrag.ANTRAG1;
            toAntragDto.KONSTELLATION = fromAntrag.KONSTELLATION;

            //Properties

            if (fromPerson != null)
            {
                if (fromPerson.LAND == null)
                    context.Entry(fromPerson).Reference(f => f.LAND).Load();
                

                toAntragDto.PersonAnrede = fromPerson.ANREDE;
                toAntragDto.PersonTitelVornameName = fromPerson.TITEL + " " + fromPerson.VORNAME + " " + fromPerson.NAME;
                toAntragDto.PersonStrasse = fromPerson.STRASSE;
                toAntragDto.PersonPlzOrt = fromPerson.PLZ + " " + fromPerson.ORT;
                toAntragDto.PersonSysLand = (fromPerson.LAND != null) ? fromPerson.LAND.SYSLAND : 0;
                toAntragDto.PersonLand = (fromPerson.LAND != null) ? fromPerson.LAND.COUNTRYNAME : "";
                if (fromPerson.SYSLANDNAT != null && fromPerson.SYSLANDNAT != 0)
                {
                    var query = from land in context.LAND
                        where land.SYSLAND == fromPerson.SYSLANDNAT
                        orderby land.COUNTRYNAME
                        select land;

                    toAntragDto.PersonLandNat = query.FirstOrDefault().COUNTRYNAME;
                }
                else
                    toAntragDto.PersonLandNat = "";
                toAntragDto.PersonTelefon = fromPerson.TELEFON;
                toAntragDto.PersonPTelefon = fromPerson.PTELEFON;
                toAntragDto.PersonHandy = fromPerson.HANDY;
                toAntragDto.PersonEmail = fromPerson.EMAIL;
                toAntragDto.PersonSysLandNat = fromPerson.SYSLANDNAT;
                toAntragDto.PersonGebDatum = fromPerson.GEBDATUM;
            }
            // Get the tax rate
            decimal TaxRate = LsAddHelper.GetTaxRate(context, fromAntrag.SYSVART);
            toAntragDto.AntragGrund =
                Cic.OpenLease.Service.MwStFacade.getInstance()
                    .CalculateGrossValue(fromAntrag.GRUND.GetValueOrDefault(), TaxRate);


            toAntragDto.RATE = fromAntkalk.RATEBRUTTO;
            if (fromAntob != null)
            {
            
                toAntragDto.AntObFzArt = fromAntob.FZART;
                toAntragDto.AntObHersteller = fromAntob.HERSTELLER;
                toAntragDto.AntObFabrikat = fromAntob.FABRIKAT;
                toAntragDto.AntObAbNahmeKm = fromAntob.UBNAHMEKM;
                toAntragDto.AntObLieferung = fromAntob.LIEFERUNG;
                toAntragDto.AntObJahresKM = fromAntob.JAHRESKM;
                toAntragDto.AntObSatzMehrKM =
                    Cic.OpenLease.Service.MwStFacade.getInstance()
                        .CalculateGrossValue(fromAntob.SATZMEHRKM.GetValueOrDefault(), TaxRate);
                toAntragDto.AntObSatzMinderKM =
                    Cic.OpenLease.Service.MwStFacade.getInstance()
                        .CalculateGrossValue(fromAntob.SATZMINDERKM.GetValueOrDefault(), TaxRate);
                toAntragDto.AntObKmToleranz = fromAntob.KMTOLERANZ;

                if (fromAntob.ANTKALK  != null)
                {
                    toAntragDto.RATE = fromAntob.ANTKALK .RATEBRUTTO;
                }

                toAntragDto.AntObNoVAzuab = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromAntob.NOVAZUAB.GetValueOrDefault(), 0);//Tel Wagner NOVA 2014 keine UST mehr!
            }
            if (fromAntkalk != null)
            {
                toAntragDto.AntKalkSonder =
                    Cic.OpenLease.Service.MwStFacade.getInstance()
                        .CalculateGrossValue(fromAntkalk.SONDER.GetValueOrDefault(), TaxRate);
                toAntragDto.AntKalkSPaket =
                    Cic.OpenLease.Service.MwStFacade.getInstance()
                        .CalculateGrossValue(fromAntkalk.SPAKET.GetValueOrDefault(), TaxRate);
                toAntragDto.AntKalkZnovaf =
                    Cic.OpenLease.Service.MwStFacade.getInstance()
                        .CalculateGrossValue(fromAntkalk.ZNOVAF.GetValueOrDefault(), TaxRate);
                toAntragDto.AntKalkRabattO =
                    Cic.OpenLease.Service.MwStFacade.getInstance()
                        .CalculateGrossValue(fromAntkalk.RABATTO.GetValueOrDefault(), TaxRate);




                toAntragDto.AntKalkBGExtern =
                    Cic.OpenLease.Service.MwStFacade.getInstance()
                        .CalculateGrossValue(fromAntkalk.BGEXTERN.GetValueOrDefault(), TaxRate);
                toAntragDto.AntKalkSZ = (fromAntkalk.SZBRUTTO.HasValue && fromAntkalk.SZBRUTTO > 0)
                    ? fromAntkalk.SZBRUTTO
                    : fromAntkalk.ANZAHLUNG;
                if (!toAntragDto.AntKalkSZ.HasValue)
                    toAntragDto.AntKalkSZ = 0;
                toAntragDto.AntKalkDepot = fromAntkalk.DEPOT;
                toAntragDto.AntKalkRW =
                    Cic.OpenLease.Service.MwStFacade.getInstance()
                        .CalculateGrossValue(fromAntkalk.RW.GetValueOrDefault(), TaxRate);
                toAntragDto.AntKalkLZ = fromAntkalk.LZ;
                toAntragDto.AntKalkRgGebuehr = fromAntkalk.RGGEBUEHR;
                toAntragDto.AntKalkGebuehr = fromAntkalk.GEBUEHR;
            }
            // Create AntObSich list
            List<AntObSichDto> AntObSichList = new List<AntObSichDto>();

            // Create an assembler
            AntobSichAssembler AntobSichAssembler = new AntobSichAssembler();

            // Iterate through all AntObSich
            foreach (var LoopAntObSich in fromAntrag.ANTOBSICHList)
            {
                // Convert and add
                AntObSichList.Add(AntobSichAssembler.ConvertToDto(LoopAntObSich, context));
            }

            // Assign the list
            toAntragDto.AntObSich = AntObSichList.ToArray();


            // Create AntObSl list
            List<AntObSlDto> AntObSlList = new List<AntObSlDto>();

            // Create an assembler
            AntObSlAssembler AntObSlAssembler = new AntObSlAssembler(context);

            // Set the sum to 0
            toAntragDto.AntObSlBetragSumme = 0;

            var AntObSls = from AntObSl in fromAntrag.ANTOBSLList
                           where AntObSl.RANG < 9000
                           orderby AntObSl.RANG
                           select AntObSl;

            // Iterate through all AntObSl
            foreach (var LoopAntObSl in AntObSls)
            {
                // Convert
                AntObSlDto AntObSl = AntObSlAssembler.ConvertToDto(LoopAntObSl);

                // Add the tax
                AntObSl.Betrag = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(AntObSl.Betrag, TaxRate);

                // Add to the sum
                toAntragDto.AntObSlBetragSumme += AntObSl.Betrag;

                // Add to the list
                AntObSlList.Add(AntObSl);
            }

            // Assign the list
            toAntragDto.AntObSl = AntObSlList.ToArray();
        }
        #endregion
    }
}