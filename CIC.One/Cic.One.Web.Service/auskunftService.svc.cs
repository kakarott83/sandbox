using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.Web.Contract;
using Cic.OpenLeaseAuskunftManagement;
using Cic.One.DTO;
using Cic.One.Web.BO;
using Cic.OpenLeaseAuskunftManagement.BO.SF;
using Cic.OpenLeaseAuskunftManagement.DTO;

namespace Cic.One.Web.Service
{
    // 

    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class auskunftService : IauskunftService
    {

        

        /// <summary>
        /// Ruft die Bonität von Crefo ab.
        /// </summary>
        /// <param name="input">igetBonitaetDto</param>
        /// <returns>ogetBonitaetDto</returns>
        public ogetBonitaetDto getBonitaet(igetBonitaetDto anfrage)
        {
            ServiceHandler<igetBonitaetDto, ogetBonitaetDto> ew = new ServiceHandler<igetBonitaetDto, ogetBonitaetDto>(anfrage);
            return ew.process(delegate(igetBonitaetDto input, ogetBonitaetDto rval)
            {
                if (input == null)
                    throw new ArgumentException("No search input");

                ogetBonitaetDto outputBonitaet = new ogetBonitaetDto();
                Cic.OpenLeaseAuskunftManagement.DTO.BonitaetOutDto output;
                outputBonitaet.bonitaet = new BonitaetDto();
                Cic.OpenLeaseAuskunftManagement.BO.SF.Bonitaet sfBonitaet = new Cic.OpenLeaseAuskunftManagement.BO.SF.Bonitaet();

                Cic.OpenLeaseAuskunftManagement.DTO.BonitaetInDto indto = new Cic.OpenLeaseAuskunftManagement.DTO.BonitaetInDto();
                if (anfrage.city != null && anfrage.city.Trim().Length > 0)
                    indto.city = anfrage.city.Trim();
                if (anfrage.companyname != null && anfrage.companyname.Trim().Length > 0)
                    indto.companyname = anfrage.companyname.Trim();
                if (anfrage.country != null && anfrage.country.Trim().Length > 0)
                    indto.country = anfrage.country.Trim();
                if (anfrage.postcode != null && anfrage.postcode.Trim().Length > 0)
                    indto.postcode = anfrage.postcode.Trim();

                if (anfrage.identificationnumber != null && anfrage.identificationnumber.Trim().Length > 0)
                    indto.identificationnumber = anfrage.identificationnumber.Trim();
                if (anfrage.street != null && anfrage.street.Trim().Length > 0)
                    indto.street = anfrage.street.Trim();

                indto.searchtype = anfrage.searchtype;
                
                Cic.OpenLeaseAuskunftManagement.DTO.AuskunftDto auskunftOutDto = sfBonitaet.doAuskunft(indto);


                //auskunftOutDto.CrefoOutDto.SucheOutDto.body.
                if (auskunftOutDto.BonitaetOutDto != null)
                {
                    output = auskunftOutDto.BonitaetOutDto;

                    //outputBonitaet.output = output;
                    outputBonitaet.bonitaet.companyidentification = output.companyidentification;
                    if (output.sucheresponse != null)
                        outputBonitaet.bonitaet.hit = output.sucheresponse.hit;
                    outputBonitaet.bonitaet.solvencyindex = output.solvencyindex;

                }
                rval.bonitaet = outputBonitaet.bonitaet;
                if (auskunftOutDto.CrefoOutDto.AuskunftOutDto != null)
                    rval.reportData = auskunftOutDto.CrefoOutDto.AuskunftOutDto.body.reportdata.textreport;
            });

        }


        /// <summary>
        /// Ruft die Bonität von Schufa ab.
        /// </summary>
        /// <param name="input">igetBonitaetSchufaDto</param>
        /// <returns>ogetBonitaetSchufaDto</returns>
        public ogetBonitaetSchufaDto getBonitaetSchufa(igetBonitaetSchufaDto anfrage)
        {
            ServiceHandler<igetBonitaetSchufaDto, ogetBonitaetSchufaDto> ew = new ServiceHandler<igetBonitaetSchufaDto, ogetBonitaetSchufaDto>(anfrage);
            return ew.process(delegate(igetBonitaetSchufaDto input, ogetBonitaetSchufaDto rval)
            {
                if (input == null)
                    throw new ArgumentException("No search input");

                //var res = Cic.OpenLeaseAuskunftManagement.BO.TestDtoFactory.CreateBonitaetsauskunftOutput(true);

                rval.SchufaReferenz = "Schufaref";
                rval.Teilnehmerreferenz = "Teilnehmerref";

                rval.Reaktion = CreateBonitaetTestoutput(input.Verbraucherdaten);
                return;

/*
                SchufaBonitaetsAuskunft sfBonitaet = new SchufaBonitaetsAuskunft();

                var inp = new SchufaInDto()
                {
                    BonitaetsAuskunft = (SchufaAnfrageBonitaetsAuskunftInDto)
                    new SchufaAnfrageBonitaetsAuskunftInDto()
                    {
                        Data = new SchufaTAnfrageBonitaetsauskunft()
                        {
                            Anfragemerkmal = input.Anfragemerkmal,
                            Verbraucherdaten = input.Verbraucherdaten
                        }
                    }
                    .CreateFrom(SchufaBaseAuskunftInDto.CreateStandard())
                };

                var result = sfBonitaet.doAuskunft(inp);
                var bonitaetsauskunft = result.SchufaOutDto.AnfrageBonitaetsAuskunftMapped;

                rval.SchufaReferenz = bonitaetsauskunft.SchufaReferenz;
                rval.Teilnehmerreferenz = bonitaetsauskunft.Teilnehmerreferenz;
                rval.Reaktion = bonitaetsauskunft.Reaktion;*/
            });
        }

        private SchufaTBonitaetsauskunft CreateBonitaetTestoutput(SchufaTVerbraucherdaten verbraucher)
        {
            return new SchufaTBonitaetsauskunft()
            {
                AusweisgepruefteIdentitaet = SchufaJaNeinEnum.nein,
                MerkmalsListe = new SchufaTMerkmal[]
                  {
                      CreateMerkmal(1,true),
                      CreateMerkmal(2,true),
                      CreateMerkmal(3,false),
                      CreateMerkmal(4,true),
                  },
                Scoreinformationen = new SchufaTScoreinformation[] 
                { 
                    CreateScore(1,false),
                    CreateScore(2,false),
                    CreateScore(3,true),
                },
                Teilnehmerkennung = "Teilnehmerkennung",
                Verbraucherdaten = verbraucher,
                Verarbeitungsinformation = new SchufaTVerarbeitungsinformation()
                {
                     Ergebnistyp = "Ergebnistyp",
                      Text = "Text"
                }
            };
        }

        private SchufaTScoreinformation CreateScore(int p, bool Fehler)
        {
            if (Fehler)
                return new SchufaTScoreinformation()
                {
                    Scorefehler = "Scorefehler " + p,
                };

            return new SchufaTScoreinformation()
                    {
                        Beschreibung = "Beschreibung " + p,
                        Risikoquote = "Risikoquote " + p,
                        Scorebereich = "Scorebereich " + p,
                        Scoreinfotext = new string[]
                          {
                               "Scoreinfo 1 "+p,
                               "Scoreinfo 2 "+p,
                               "Scoreinfo 3 "+p,
                               "Scoreinfo 4 "+p,
                          },
                        Scoretext = "Scoretext " + p,
                        Scorewert = "Scorewert " + p

                    };
        }

        private SchufaTMerkmal CreateMerkmal(int p,bool specified)
        {
            return new SchufaTMerkmal()
            {
                Beschreibung = "ANFRAGE ZU GIROKONTO " + p,
                Betrag = "betrag " + p,
                Datum = "1"+p+".12.2013",
                Kontonummer = "1234 " + p,
                Merkmalcode = "Code",
                Merkmalsattribute = new SchufaTMerkmalsAttribute()
                {
                    EigenesMerkmal = SchufaJaNeinEnum.JA,
                    MerkmalOhneGeburtsdatum = specified ? SchufaJaNeinEnum.Ja : new Nullable<SchufaJaNeinEnum>(),
                    Typ = specified ? SchufaMerkmalTypEnum.Erledigungsmerkmal : new Nullable<SchufaMerkmalTypEnum>(),
                },
                Ratenart = specified ? SchufaRatenartEnum.m : new Nullable<SchufaRatenartEnum>(),
                Ratenzahl = "Ratenzahl " + p,
                Text = "Text " + p,
                Waehrung = SchufaWaehrungEnum.EUR

            };
        }
    }
}
