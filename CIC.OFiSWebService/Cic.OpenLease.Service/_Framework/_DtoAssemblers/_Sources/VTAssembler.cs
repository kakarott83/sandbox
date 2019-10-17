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
    public class VTAssembler : IDtoAssemblerAntrag<VTDto, VT, KALK, OB, PERSON, KDTYP>
    {
        #region Private variables
        private System.Collections.Generic.Dictionary<string, string> _Errors;
        private long? _SysPEROLE;
        #endregion

        #region Constructors
        public VTAssembler(long? sysPEROLE)
        {
            _SysPEROLE = sysPEROLE;
            _Errors = new System.Collections.Generic.Dictionary<string, string>();
        }
        #endregion

        #region IDtoAssembler<ANGEBOTDto,ANGEBOT> Members (Methods)
        public bool IsValid(VTDto dto)
        {
            // NOTE WB, Not necessary
            throw new NotImplementedException();
        }

        public void Create(VTDto dto)
        {
            // NOTE WB, Not necessary
            throw new NotImplementedException();
        }

        public void Update(VTDto dto)
        {
            // NOTE WB, Not necessary
            throw new NotImplementedException();
        }

 /*       public VTDto ConvertToDto(VT domain, KALK kalk, OB ob, IT it, OlExtendedEntities context)
        {
            VTDto VTDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            VTDto = new VTDto();
            MyMap(domain, kalk, ob, it, VTDto, context);

            return VTDto;
        }
*/

        public VTDto ConvertToDto(VT domain, KALK kalk, OB ob, PERSON kd, KDTYP kdtyp, DdOlExtended context)
        {
            VTDto VTDto;

            if (domain == null)
            {
                throw new ArgumentException("domain");
            }

            VTDto = new VTDto();
            MyMap(domain, kalk, ob, kd, VTDto,kdtyp, context);

            return VTDto;
        }



  /*      public VT ConvertToDomain(VTDto dto, KALK kalk, OB ob, IT it)
        {
            VT VT;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            VT = new VT();
            MyMap(dto, kalk, ob, VT, it);

            return VT;
        }
*/
        public VT ConvertToDomain(VTDto dto, KALK kalk, OB ob, PERSON person)
        {
            VT VT;

            if (dto == null)
            {
                throw new ArgumentException("dto");
            }

            VT = new VT();
            MyMap(dto, kalk, ob, VT, person);

            return VT;
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
 /*       private void MyMap(VTDto fromVTDto, KALK toKalk, OB toOb, VT toVT, IT toIT)
        {
            //Ids
            toVT.SYSIT = (fromVTDto.SysIt.HasValue && fromVTDto.SysIt.Value < 1L ? null : fromVTDto.SysIt); ;

            //Properties

            toIT.ANREDE = fromVTDto.ITAnrede;
            toIT.STRASSE = fromVTDto.ITStrasse;
            toIT.SYSLAND = fromVTDto.ITSysLand;
            toIT.TELEFON = fromVTDto.ITTelefon;
            toIT.PTELEFON = fromVTDto.ITPTelefon;
            toIT.HANDY = fromVTDto.ITHandy;
            toIT.EMAIL = fromVTDto.ITEmail;
            toIT.SYSLANDNAT = fromVTDto.ITSysLandNat;
            toIT.GEBDATUM = fromVTDto.ITGebDatum;

            //toOb.ObFzArt = toOb.FZART;
            toOb.HERSTELLER = fromVTDto.ObHersteller;
            toOb.FABRIKAT = fromVTDto.ObFabrikat;
            toOb.ABNAHMEKM = fromVTDto.ObAbNahmeKm;
            toOb.LIEFERUNG = fromVTDto.ObLieferung;
            toOb.JAHRESKM = fromVTDto.ObJahresKM;
            toOb.SATZMEHRKM = fromVTDto.ObSatzMehrKM;
            toOb.SATZMINDERKM = fromVTDto.ObSatzMinderKM;
            toOb.KMTOLERANZ = fromVTDto.ObKmToleranz;

            toVT.GRUND = fromVTDto.VTGrund;
            toKalk.SONDER = fromVTDto.KalkSonder;
            toKalk.SPAKET = fromVTDto.KalkSPaket;
            toKalk.ZNOVAF = fromVTDto.KalkZnovaf;
            toKalk.RABATTO = fromVTDto.KalkRabattO;
            toOb.NOVAZUAB = fromVTDto.ObNoVAzuab;

            toKalk.BGEXTERN = fromVTDto.KalkBGExtern;
            toKalk.SZ = fromVTDto.KalkSZ;
            toKalk.DEPOT = fromVTDto.KalkDepot;
            toKalk.RW = fromVTDto.KalkRW;
            toKalk.LZ = fromVTDto.KalkLZ;
            toKalk.RGGEBUEHR = fromVTDto.KalkRgGebuehr;
            toKalk.GEBUEHR = fromVTDto.KalkGebuehr;
            toKalk.LZ = fromVTDto.KalkLZ;
            toKalk.ANZAHLUNG = fromVTDto.KalkAnzahlung;
            
        }
*/

        private void MyMap(VTDto fromVTDto, KALK toKalk, OB toOb, VT toVT, PERSON toPERSON)
        {
            //Ids
            //toVT.SysKD = (fromVTDto.SysIt.HasValue && fromVTDto.SysIt.Value < 1L ? null : fromVTDto.SysIt);
            //toVT.SYSIT = (fromVTDto.SysIt.HasValue && fromVTDto.SysIt.Value < 1L ? null : fromVTDto.SysIt); 

            //Properties

            toPERSON.ANREDE = fromVTDto.PersonAnrede;
            toPERSON.STRASSE = fromVTDto.PersonStrasse;
            toPERSON.SYSLANDNAT = fromVTDto.PersonSysLandNat;
            toPERSON.TELEFON = fromVTDto.PersonTelefon;
            toPERSON.PTELEFON = fromVTDto.PersonTelefon;
            toPERSON.HANDY = fromVTDto.PersonHandy;
            toPERSON.EMAIL = fromVTDto.PersonEmail;
            toPERSON.SYSLANDNAT = fromVTDto.PersonSysLandNat;
            toPERSON.GEBDATUM = fromVTDto.PersonGebDatum;

            //toVTDto.ObFzArt = toOb.FZART;
            toOb.HERSTELLER = fromVTDto.ObHersteller;
            toOb.FABRIKAT = fromVTDto.ObFabrikat;
            toOb.ABNAHMEKM = fromVTDto.ObAbNahmeKm;
            toOb.LIEFERUNG = fromVTDto.ObLieferung;
            toOb.JAHRESKM = fromVTDto.ObJahresKM;
            toOb.SATZMEHRKM = fromVTDto.ObSatzMehrKM;
            toOb.SATZMINDERKM = fromVTDto.ObSatzMinderKM;
            toOb.KMTOLERANZ = fromVTDto.ObKmToleranz;

            toVT.GRUND = fromVTDto.VTGrund;
            toKalk.SONDER = fromVTDto.KalkSonder;
            toKalk.SPAKET = fromVTDto.KalkSPaket;
            toKalk.ZNOVAF = fromVTDto.KalkZnovaf;
            toKalk.RABATTO = fromVTDto.KalkRabattO;
            toOb.NOVAZUAB = fromVTDto.ObNoVAzuab;

            toKalk.BGEXTERN = fromVTDto.KalkBGExtern;
            toKalk.SZ = fromVTDto.KalkSZ;
            toKalk.DEPOT = fromVTDto.KalkDepot;
            toKalk.RW = fromVTDto.KalkRW;
            toKalk.LZ = fromVTDto.KalkLZ;
            toKalk.RGGEBUEHR = fromVTDto.KalkRgGebuehr;
            toKalk.GEBUEHR = fromVTDto.KalkGebuehr;
            toKalk.LZ = fromVTDto.KalkLZ;
            toKalk.ANZAHLUNG = fromVTDto.KalkAnzahlung;

        }

   /*     private void MyMap(VT fromVT, KALK fromKalk, OB fromOb, IT fromIT, VTDto toVTDto, OlExtendedEntities context)
        {
            //Ids
            toVTDto.SysId = fromVT.SYSID;
            toVTDto.SysIt = fromVT.SYSIT;
            //Properties

            if (fromIT != null)
            {
                toVTDto.ITAnrede = fromIT.ANREDE;
                toVTDto.ITTitelVornameName = fromIT.TITEL + " " + fromIT.VORNAME + " " + fromIT.NAME;
                toVTDto.ITStrasse = fromIT.STRASSE;
                toVTDto.ITPlzOrt = fromIT.PLZ + " " + fromIT.ORT;
                toVTDto.ITSysLand = fromIT.SYSLAND;
                toVTDto.ITTelefon = fromIT.TELEFON;
                toVTDto.ITPTelefon = fromIT.PTELEFON;
                toVTDto.ITHandy = fromIT.HANDY;
                toVTDto.ITEmail = fromIT.EMAIL;
                toVTDto.ITSysLandNat = fromIT.SYSLANDNAT;
                toVTDto.ITGebDatum = fromIT.GEBDATUM;
            }

            // Get the tax rate
            decimal TaxRate = LsAddHelper.GetTaxRate(context, (long)fromVT.SYSVART);
            
            toVTDto.ObFzArt = fromOb.FZART;
            toVTDto.ObHersteller = fromOb.HERSTELLER;
            toVTDto.ObFabrikat = fromOb.FABRIKAT;
            toVTDto.ObAbNahmeKm = fromOb.ABNAHMEKM;
            toVTDto.ObLieferung = fromOb.LIEFERUNG;
            toVTDto.ObJahresKM = fromOb.JAHRESKM;
            toVTDto.ObSatzMehrKM = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromOb.SATZMEHRKM.GetValueOrDefault(), TaxRate);
            toVTDto.ObSatzMinderKM = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromOb.SATZMINDERKM.GetValueOrDefault(), TaxRate);
            toVTDto.ObKmToleranz = fromOb.KMTOLERANZ;

            toVTDto.VTGrund = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromVT.GRUND.GetValueOrDefault(), TaxRate);
            toVTDto.KalkSonder = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.SONDER.GetValueOrDefault(), TaxRate);
            toVTDto.KalkSPaket = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.SPAKET.GetValueOrDefault(), TaxRate);
            toVTDto.KalkZnovaf = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.ZNOVAF.GetValueOrDefault(), TaxRate);
            toVTDto.KalkRabattO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.RABATTO.GetValueOrDefault(), TaxRate);
            toVTDto.ObNoVAzuab = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromOb.NOVAZUAB.GetValueOrDefault(), TaxRate);

            toVTDto.KalkBGExtern = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.BGEXTERN.GetValueOrDefault(), TaxRate);
            toVTDto.KalkSZ = fromKalk.SZ;
            toVTDto.KalkDepot = fromKalk.DEPOT;
            toVTDto.KalkRW = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.RW.GetValueOrDefault(), TaxRate);
            toVTDto.KalkLZ = fromKalk.LZ;
            toVTDto.KalkRgGebuehr = fromKalk.RGGEBUEHR;
            toVTDto.KalkGebuehr = fromKalk.GEBUEHR;
            toVTDto.KalkLZ = fromKalk.LZ;
            toVTDto.KalkAnzahlung = fromKalk.ANZAHLUNG;

            // Create AntObSl list
            List<ObSlDto> ObSlList = new List<ObSlDto>();

            // Create an assembler
            ObSlAssembler ObSlAssembler = new ObSlAssembler();

            // Set the sum to 0
            toVTDto.ObSlBetragSumme = 0;

            var ObSls = from ObSl in fromVT.OBList
                           where ObSl.RANG < 9000
                           orderby ObSl.RANG
                           select ObSl;

            // Iterate through all AntObSl
            foreach (var LoopObSl in ObSls)
            {
                // Convert
                ObSlDto ObSlDto = ObSlAssembler.ConvertToDto(LoopObSl);

                // Add the tax
                ObSlDto.Betrag = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(ObSlDto.Betrag, TaxRate);

                // Add to the sum
                toVTDto.ObSlBetragSumme += ObSlDto.Betrag;

                // Add to the list
                ObSlList.Add(ObSlDto);
            }

            // Assign the list
            toVTDto.ObSl = ObSlList.ToArray();
        }
*/

        private void MyMap(VT fromVT, KALK fromKalk, OB fromOb, PERSON fromPerson, VTDto toVTDto, KDTYP kdtyp, DdOlExtended context)
        {
            //Ids
            toVTDto.SysId = fromVT.SYSID;
            toVTDto.SysIt= fromVT.SYSKD;
            toVTDto.VERTRAG = fromVT.VERTRAG;
            toVTDto.ZUSTAND = fromVT.ZUSTAND;
            toVTDto.VART = fromVT.VART;
            if(fromOb.KALK!=null)
            toVTDto.RATE = fromOb.KALK.RATE;
            toVTDto.KONSTELLATION = fromVT.KONSTELLATION;
            toVTDto.KENNZEICHEN = fromOb.KENNZEICHEN;
            toVTDto.VTENDE =  Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromVT.ENDE);
            toVTDto.VTBEGINN = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(fromVT.BEGINN);
            //Properties

            if (fromPerson != null)
            {
                if (fromPerson.LAND == null)
                    context.Entry(fromPerson).Reference(f => f.LAND).Load();
                
                toVTDto.PersonAnrede = fromPerson.ANREDE;

                toVTDto.PersonTitelVornameName = fromPerson.TITEL + " " + fromPerson.VORNAME + " " + fromPerson.NAME;//Privat+Einzelunternehmer
                toVTDto.PersonTitelNameVorname = fromPerson.TITEL + " " + fromPerson.NAME + " " + fromPerson.VORNAME;//Unternehmen

                toVTDto.PersonStrasse = fromPerson.STRASSE;
                toVTDto.PersonPlzOrt = fromPerson.PLZ + " " + fromPerson.ORT;
                toVTDto.PersonSysLand = (fromPerson.LAND != null) ? fromPerson.LAND.SYSLAND : 0;
                toVTDto.PersonSysLandNat = fromPerson.SYSLANDNAT;
                
                toVTDto.PersonLand = (fromPerson.LAND != null) ? fromPerson.LAND.COUNTRYNAME : "";
                if (fromPerson.SYSLANDNAT != null && fromPerson.SYSLANDNAT!= 0)
                {
                    toVTDto.PersonLandNat = (from land in context.LAND
                    where land.SYSLAND == fromPerson.SYSLANDNAT
                    orderby land.COUNTRYNAME
                    select land.COUNTRYNAME).FirstOrDefault();
                }
                else
                    toVTDto.PersonLandNat = "";
                toVTDto.PersonTelefon = fromPerson.TELEFON;
                toVTDto.PersonPTelefon = fromPerson.PTELEFON;
                toVTDto.PersonHandy = fromPerson.HANDY;
                toVTDto.PersonEmail = fromPerson.EMAIL;
                toVTDto.PersonGebDatum = fromPerson.GEBDATUM;
            }
            toVTDto.KDTYP = 1;
            if (kdtyp != null && kdtyp.TYP.HasValue)
            {
                toVTDto.KDTYP =  (int)kdtyp.TYP;
            }
            // Get the tax rate
            decimal TaxRate = LsAddHelper.GetTaxRate(context, (long)fromVT.SYSVART);

            toVTDto.ObFzArt = fromOb.FZART;
            toVTDto.ObHersteller = fromOb.HERSTELLER;
            toVTDto.ObFabrikat = fromOb.FABRIKAT;
            toVTDto.ObAbNahmeKm = fromOb.ABNAHMEKM;
            toVTDto.ObLieferung = fromOb.LIEFERUNG;
            toVTDto.ObJahresKM = fromOb.JAHRESKM;
            toVTDto.ObSatzMehrKM = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromOb.SATZMEHRKM.GetValueOrDefault(), TaxRate);
            toVTDto.ObSatzMinderKM = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromOb.SATZMINDERKM.GetValueOrDefault(), TaxRate);
            toVTDto.ObKmToleranz = fromOb.KMTOLERANZ;
            

            toVTDto.VTGrund = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromVT.GRUND.GetValueOrDefault(), TaxRate);
            toVTDto.ObNoVAzuab = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromOb.NOVAZUAB.GetValueOrDefault(), 0);//Tel Wagner NOVA 2014 keine UST mehr!

            if (fromKalk != null)
            {
                toVTDto.KalkSonder = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.SONDER.GetValueOrDefault(), TaxRate);
                toVTDto.KalkSPaket = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.SPAKET.GetValueOrDefault(), TaxRate);
                toVTDto.KalkZnovaf = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.ZNOVAF.GetValueOrDefault(), TaxRate);
                toVTDto.KalkRabattO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.RABATTO.GetValueOrDefault(), TaxRate);
                toVTDto.KalkBGExtern = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.BGEXTERN.GetValueOrDefault(), TaxRate);
                toVTDto.KalkSZ = (fromKalk.SZ.HasValue && fromKalk.SZ > 0) ? fromKalk.SZ : fromKalk.ANZAHLUNG;
                toVTDto.KalkSZBRUTTO = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(toVTDto.KalkSZ.GetValueOrDefault(), TaxRate);
                toVTDto.KalkDepot = fromKalk.DEPOT;
                toVTDto.KalkRW = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.RW.GetValueOrDefault(), TaxRate);
                toVTDto.KalkLZ = fromKalk.LZ;
                toVTDto.KalkRgGebuehr = fromKalk.RGGEBUEHR;
                toVTDto.KalkGebuehr = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fromKalk.GEBUEHR.GetValueOrDefault(), TaxRate);
                toVTDto.KalkLZ = fromKalk.LZ;
                toVTDto.KalkAnzahlung = fromKalk.ANZAHLUNG;
            }
            toVTDto.RATE = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(toVTDto.RATE.GetValueOrDefault(), TaxRate); 

            // Create AntObSl list
            List<ObSlDto> ObSlList = new List<ObSlDto>();

            // Create an assembler
            ObSlAssembler ObSlAssembler = new ObSlAssembler();

            // Set the sum to 0
            toVTDto.ObSlBetragSumme = 0;

            var ObSls = from ObSl in fromVT.OBList
                        where ObSl.RANG < 9000
                        orderby ObSl.RANG
                        select ObSl;

            // Iterate through all AntObSl
            foreach (var LoopObSl in ObSls)
            {
                // Convert
                ObSlDto ObSlDto = ObSlAssembler.ConvertToDto(LoopObSl);

                // Add the tax
                ObSlDto.Betrag = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(ObSlDto.Betrag, TaxRate);

                // Add to the sum
                toVTDto.ObSlBetragSumme += ObSlDto.Betrag;

                // Add to the list
                ObSlList.Add(ObSlDto);
            }

            // Assign the list
            toVTDto.ObSl = ObSlList.ToArray();
            toVTDto.ERSTZULASSUNG = fromOb.ERSTZUL;
        }
        #endregion
    }
}