using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OL.EF6.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Zusatzdaten DAO
    /// </summary>
    public class ZusatzdatenDao : IZusatzdatenDao
    {
        private static String QUERY_PERSONPKZACTIVE=@"select pkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,pkz.syspkz syspkz from pkz where syspkz=(
                                        SELECT nvl(max(syspkz),0)
                                         FROM pkz,
                                         antrag
                                         WHERE pkz.sysantrag =antrag.sysid
                                         AND (antrag.zustand='Bonitätsprüfung'
                                         OR antrag.zustand = 'Risikoprüfung'
                                         OR antrag.zustand   = 'Nachbearbeitung'
                                         OR antrag.attribut  = 'Vertrag aktiviert')
                                         AND pkz.sysperson   =:sysperson)";
        private static String QUERY_PERSONPKZACTIVELAST = @"select pkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,pkz.syspkz syspkz from pkz where syspkz=(
                                        SELECT nvl(max(syspkz),0)
                                         FROM pkz
                                         WHERE pkz.sysperson   =:sysperson)";

        private static String QUERY_PERSONUKZACTIVE = @"select ukz.*,obligoeigen obliboeigen,ukz.sysukz sysukz from ukz where sysukz=(
                        SELECT nvl(max(sysukz),0)
                         FROM ukz,
                         antrag
                         WHERE ukz.sysantrag =antrag.sysid
                         AND (antrag.zustand='Bonitätsprüfung'
                         OR antrag.zustand = 'Risikoprüfung'
                         OR antrag.zustand   = 'Nachbearbeitung'
                         OR antrag.attribut  = 'Vertrag aktiviert')
                         AND ukz.sysperson = :sysperson)";

        private static String QUERY_PERSONUKZACTIVELAST = @"select ukz.*,obligoeigen obliboeigen,ukz.sysukz sysukz from ukz where sysukz=(
                        SELECT nvl(max(sysukz),0)
                         FROM ukz
                         WHERE ukz.sysperson = :sysperson)";

        /// <summary>
        /// Neue PKZ erzeugen
        /// </summary>
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public PkzDto createPkz(PkzDto pkzInput, KundeDto kunde)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ITPKZ pkzOutput = new ITPKZ();
                context.ITPKZ.Add(pkzOutput);
                
                pkzOutput = Mapper.Map<PkzDto, ITPKZ>(pkzInput, pkzOutput);
                pkzOutput.ANZAHLVOLLSTR = pkzInput.anzvollstr;
                if (pkzOutput.ANZAHLVOLLSTR == 0)
                    pkzOutput.BETRAGVOLLSTR = 0;
                pkzOutput.KREDRATE = (decimal?)pkzInput.kredtrate;
                pkzOutput.SYSIT=kunde.sysit;
                context.SaveChanges();
                PkzDto rval = getPkz(pkzOutput.SYSITPKZ);
                //Set rvalue for later KREMO-Update in AngAntBO!
                rval.betreuungskosten = pkzInput.betreuungskosten;
                rval.krankenkasse = pkzInput.krankenkasse;
                rval.arbeitswegpauschale1 = pkzInput.arbeitswegpauschale1;
                rval.arbeitswegpauschale2 = pkzInput.arbeitswegpauschale2;
                return rval;
            }
        }

        /// <summary>
        /// Kundendaten aktualisieren
        /// </summary>
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public PkzDto updatePkz(PkzDto pkzInput, KundeDto kunde)
        {
            if (pkzInput == null)
            {
                throw new ApplicationException("No Input Pkz delivered.");
            }
            ITPKZ pkzOutput = new ITPKZ();
            using (DdOlExtended context = new DdOlExtended())
            {
                pkzOutput = (from itpkz in context.ITPKZ
                             where itpkz.SYSITPKZ == pkzInput.syspkz
                             select itpkz).FirstOrDefault();
                if (pkzOutput == null)
                {
                    throw new ApplicationException("No Pkz found for: " + pkzInput.syspkz);
                }

                pkzOutput = Mapper.Map<PkzDto, ITPKZ>(pkzInput, pkzOutput);
                pkzOutput.KREDRATE = (decimal?)pkzInput.kredtrate;
                pkzOutput.ANZAHLVOLLSTR = pkzInput.anzvollstr;
                if (pkzOutput.ANZAHLVOLLSTR == 0)
                    pkzOutput.BETRAGVOLLSTR = 0;
                pkzOutput.SYSITPKZ = pkzInput.syspkz;
                pkzOutput.SYSIT=kunde.sysit;

                context.SaveChanges();
            }
            PkzDto rval= getPkz(pkzOutput.SYSITPKZ);
            //Set rvalue for later KREMO-Update in AngAntBO!
            rval.betreuungskosten = pkzInput.betreuungskosten;
            rval.krankenkasse = pkzInput.krankenkasse;
            rval.arbeitswegpauschale1 = pkzInput.arbeitswegpauschale1;
            rval.arbeitswegpauschale2 = pkzInput.arbeitswegpauschale2;
            return rval;
        }

        /// <summary>
        /// UKZ erzeugen
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public UkzDto createUkz(UkzDto ukzInput, KundeDto kunde)
        {
            ITUKZ ukzOutput = new ITUKZ();
            using (DdOlExtended context = new DdOlExtended())
            {
                //IT kundeSave = new IT();
                context.ITUKZ.Add(ukzOutput);
                //Mapper.CreateMap<UkzDto, ITUKZ>();
                //ukzOutput = Mapper.Map<UkzDto, ITUKZ>(ukzInput, ukzOutput);
                ukzOutput.ANZMA = ukzInput.anzma;
                ukzOutput.ANZVOLLSTR = ukzInput.anzvollstr;
                if (ukzOutput.ANZVOLLSTR == 0)
                    ukzOutput.BETRAGVOLLSTR = 0;
                else
                    ukzOutput.BETRAGVOLLSTR = (decimal?)ukzInput.betragvollstr;
                ukzOutput.BILANZWERT = (decimal?)ukzInput.bilanzwert;
                ukzOutput.EKAPITAL = (decimal?)ukzInput.ekapital;
                ukzOutput.ERGEBNIS1 = (long?)ukzInput.ergebnis1;
                ukzOutput.JUMSATZ = (decimal?)ukzInput.jumsatz;
                ukzOutput.KONKURSFLAG = Convert.ToInt32(ukzInput.konkursFlag);
                ukzOutput.LIQUIDITAET = (decimal?)ukzInput.liquiditaet;
                ukzOutput.LJABSCHL = ukzInput.ljabschl;
                ukzOutput.OBLIGOEIGEN = (decimal?)ukzInput.obliboeigen;
                ukzOutput.SYSANGEBOT = ukzInput.sysangebot;
                ukzOutput.SYSANTRAG = ukzInput.sysantrag;
                ukzOutput.LEGITABNEHMER = ukzInput.legitAbnehmer;
                ukzOutput.LEGITDATUM = ukzInput.legitDatum;
                ukzOutput.LEGITMETHODCODE = ukzInput.legitMethodCode;
                ukzOutput.SYSIT=kunde.sysit;
                ukzOutput.EXTREFERENZ = ukzInput.extreferenz;
                context.SaveChanges();
            }
            return getUkz(ukzOutput.SYSITUKZ);
        }

        /// <summary>
        /// UKZ ändern
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public UkzDto updateUkz(UkzDto ukzInput, KundeDto kunde)
        {
            if (ukzInput == null)
            {
                throw new ApplicationException("No Input Ukz delivered");
            }
            ITUKZ ukzOutput = new ITUKZ();
            using (DdOlExtended context = new DdOlExtended())
            {
                ukzOutput = (from itukz in context.ITUKZ
                             where itukz.SYSITUKZ == ukzInput.sysukz
                             select itukz).FirstOrDefault();

                if (ukzOutput != null)
                {
                    ukzOutput.ANZMA = ukzInput.anzma;
                    ukzOutput.ANZVOLLSTR = ukzInput.anzvollstr;
                    if (ukzOutput.ANZVOLLSTR == 0)
                        ukzOutput.BETRAGVOLLSTR = 0;
                    else
                        ukzOutput.BETRAGVOLLSTR = (decimal?)ukzInput.betragvollstr;
                    ukzOutput.BILANZWERT = (decimal?)ukzInput.bilanzwert;
                    ukzOutput.EKAPITAL = (decimal?)ukzInput.ekapital;
                    ukzOutput.ERGEBNIS1 = (long?)ukzInput.ergebnis1;
                    ukzOutput.JUMSATZ = (decimal?)ukzInput.jumsatz;
                    ukzOutput.KONKURSFLAG = Convert.ToInt32(ukzInput.konkursFlag);
                    ukzOutput.LIQUIDITAET = (decimal?)ukzInput.liquiditaet;
                    ukzOutput.LJABSCHL = ukzInput.ljabschl;
                    ukzOutput.OBLIGOEIGEN = (decimal?)ukzInput.obliboeigen;
                    ukzOutput.SYSITUKZ = ukzInput.sysukz;
                    ukzOutput.SYSANGEBOT = ukzInput.sysangebot;
                    ukzOutput.SYSANTRAG = ukzInput.sysantrag;
                    ukzOutput.LEGITABNEHMER = ukzInput.legitAbnehmer;
                    ukzOutput.LEGITDATUM = ukzInput.legitDatum;
                    ukzOutput.LEGITMETHODCODE = ukzInput.legitMethodCode;
                    ukzOutput.SYSIT=kunde.sysit;
                    ukzOutput.EXTREFERENZ = ukzInput.extreferenz;
                }
                else
                {
                    throw new ApplicationException("No Ukz found for " + ukzInput.sysukz);
                }
                context.SaveChanges();
            }
            return getUkz(ukzOutput.SYSITUKZ);
        }


        /// <summary>
        /// Neue PKZ erzeugen
        /// </summary>
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public PkzDto createPkzPerson(PkzDto pkzInput, KundeDto kunde)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                PKZ pkzOutput = new PKZ();
                context.PKZ.Add(pkzOutput);

                pkzOutput = Mapper.Map<PkzDto, PKZ>(pkzInput, pkzOutput);
                pkzOutput.ANZAHLVOLLSTR = pkzInput.anzvollstr;
                if (pkzOutput.ANZAHLVOLLSTR == 0)
                    pkzOutput.BETRAGVOLLSTR = 0;
                pkzOutput.KREDRATE = (decimal?)pkzInput.kredtrate;
                pkzOutput.SYSPERSON= kunde.syskd;
                context.SaveChanges();
                return getPkzPerson(pkzOutput.SYSPKZ);
            }
        }

        /// <summary>
        /// Kundendaten aktualisieren
        /// </summary>
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public PkzDto updatePkzPerson(PkzDto pkzInput, KundeDto kunde)
        {
            if (pkzInput == null)
            {
                throw new ApplicationException("No Input Pkz delivered.");
            }
            PKZ pkzOutput = new PKZ();
            using (DdOlExtended context = new DdOlExtended())
            {
                pkzOutput = (from pkz in context.PKZ
                             where pkz.SYSPKZ == pkzInput.syspkz
                             select pkz).FirstOrDefault();
                if (pkzOutput == null)
                {
                    throw new ApplicationException("No Pkz found for: " + pkzInput.syspkz);
                }

                pkzOutput = Mapper.Map<PkzDto, PKZ>(pkzInput, pkzOutput);
                pkzOutput.KREDRATE = (decimal?)pkzInput.kredtrate;
                pkzOutput.ANZAHLVOLLSTR = pkzInput.anzvollstr;
                if (pkzOutput.ANZAHLVOLLSTR == 0)
                    pkzOutput.BETRAGVOLLSTR = 0;
                pkzOutput.SYSPKZ = pkzInput.syspkz;
                pkzOutput.SYSPERSON=kunde.syskd;

                context.SaveChanges();
            }
            return getPkzPerson(pkzOutput.SYSPKZ);
        }

        /// <summary>
        /// UKZ erzeugen
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public UkzDto createUkzPerson(UkzDto ukzInput, KundeDto kunde)
        {
            UKZ ukzOutput = new UKZ();
            using (DdOlExtended context = new DdOlExtended())
            {
                //IT kundeSave = new IT();
                context.UKZ.Add(ukzOutput);
                //Mapper.CreateMap<UkzDto, ITUKZ>();
                //ukzOutput = Mapper.Map<UkzDto, ITUKZ>(ukzInput, ukzOutput);
                ukzOutput.ANZMA = ukzInput.anzma;
                ukzOutput.ANZVOLLSTR = ukzInput.anzvollstr;
                if (ukzOutput.ANZVOLLSTR == 0)
                    ukzOutput.BETRAGVOLLSTR = 0;
                else 
                    ukzOutput.BETRAGVOLLSTR = (decimal?)ukzInput.betragvollstr;
                ukzOutput.BILANZWERT = (decimal?)ukzInput.bilanzwert;
                ukzOutput.EKAPITAL = (decimal?)ukzInput.ekapital;
                ukzOutput.ERGEBNIS1 = (long?)ukzInput.ergebnis1;
                ukzOutput.JUMSATZ = (decimal?)ukzInput.jumsatz;
                ukzOutput.KONKURSFLAG = Convert.ToInt32(ukzInput.konkursFlag);
                ukzOutput.LIQUIDITAET = (decimal?)ukzInput.liquiditaet;
                ukzOutput.LJABSCHL = ukzInput.ljabschl;
                ukzOutput.OBLIGOEIGEN = (decimal?)ukzInput.obliboeigen;
                ukzOutput.SYSANGEBOT = ukzInput.sysangebot;
                ukzOutput.SYSANTRAG = ukzInput.sysantrag;
                ukzOutput.LEGITABNEHMER = ukzInput.legitAbnehmer;
                ukzOutput.LEGITDATUM = ukzInput.legitDatum;
                ukzOutput.LEGITMETHODCODE = ukzInput.legitMethodCode;
                ukzOutput.SYSPERSON= kunde.syskd;

                context.SaveChanges();
            }
            return getUkzPerson(ukzOutput.SYSUKZ);
        }

        /// <summary>
        /// UKZ ändern
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        public UkzDto updateUkzPerson(UkzDto ukzInput, KundeDto kunde)
        {
            if (ukzInput == null)
            {
                throw new ApplicationException("No Input Ukz delivered");
            }
            UKZ ukzOutput = new UKZ();
            using (DdOlExtended context = new DdOlExtended())
            {
                ukzOutput = (from itukz in context.UKZ
                             where itukz.SYSUKZ == ukzInput.sysukz
                             select itukz).FirstOrDefault();

                if (ukzOutput != null)
                {
                    ukzOutput.ANZMA = ukzInput.anzma;
                    ukzOutput.ANZVOLLSTR = ukzInput.anzvollstr;
                    if (ukzOutput.BETRAGVOLLSTR == 0)
                        ukzOutput.BETRAGVOLLSTR = 0;
                    else
                        ukzOutput.BETRAGVOLLSTR = (decimal?)ukzInput.betragvollstr;
                    ukzOutput.BILANZWERT = (decimal?)ukzInput.bilanzwert;
                    ukzOutput.EKAPITAL = (decimal?)ukzInput.ekapital;
                    ukzOutput.ERGEBNIS1 = (long?)ukzInput.ergebnis1;
                    ukzOutput.JUMSATZ = (decimal?)ukzInput.jumsatz;
                    ukzOutput.KONKURSFLAG = Convert.ToInt32(ukzInput.konkursFlag);
                    ukzOutput.LIQUIDITAET = (decimal?)ukzInput.liquiditaet;
                    ukzOutput.LJABSCHL = ukzInput.ljabschl;
                    ukzOutput.OBLIGOEIGEN = (decimal?)ukzInput.obliboeigen;
                    ukzOutput.SYSUKZ = ukzInput.sysukz;
                    ukzOutput.LEGITABNEHMER = ukzInput.legitAbnehmer;
                    ukzOutput.LEGITDATUM = ukzInput.legitDatum;
                    ukzOutput.LEGITMETHODCODE = ukzInput.legitMethodCode;
                    ukzOutput.SYSANGEBOT = ukzInput.sysangebot;
                    ukzOutput.SYSANTRAG = ukzInput.sysantrag;
                    ukzOutput.SYSPERSON= kunde.syskd;

                }
                else
                {
                    throw new ApplicationException("No Ukz found for " + ukzInput.sysukz);
                }
                context.SaveChanges();
            }
            return getUkzPerson(ukzOutput.SYSUKZ);
        }

        /// <summary>
        /// PKZ via ID holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Daten</returns>
        public PkzDto getPkz(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ITPKZ pkzOutput = (from itpkz in context.ITPKZ
                                   where itpkz.SYSITPKZ == sysid
                                   select itpkz).FirstOrDefault();
                
                PkzDto rval = new PkzDto();
                Mapper.Map<ITPKZ, PkzDto>(pkzOutput, rval);
                rval.syspkz = pkzOutput.SYSITPKZ;
                rval.anzvollstr = (short)(pkzOutput.ANZAHLVOLLSTR.HasValue ? pkzOutput.ANZAHLVOLLSTR.Value : 0);
                if (pkzOutput.KREDRATE.HasValue)
                    rval.kredtrate = (double?)pkzOutput.KREDRATE.Value;

                rval.zeinkCode = fixStringCode(rval.zeinkCode);
                rval.wohnverhCode = fixStringCode(rval.wohnverhCode);
                rval.beruflichCode = fixStringCode(rval.beruflichCode);
                rval.auslagenCode = fixStringCode(rval.auslagenCode);
                return rval;
            }
        }

        /// <summary>
        /// Trims the String value codes
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String fixStringCode(String val)
        {
            if (val == null) return null;
            if (val.Trim().Length == 0) return null;
            return val.Trim();
        }

        /// <summary>
        /// returns the youngest checked approval pkz/ukz for the person or the newest one
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public ZusatzdatenDto[] getPersonZusatzdaten(long sysperson)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                DbConnection con = (context.Database.Connection);
                ZusatzdatenDto[] rval = new ZusatzdatenDto[1];
                rval[0] = new ZusatzdatenDto();
                int kdtyptyp = context.ExecuteStoreQuery<int>("select typ from kdtyp,person where person.syskdtyp=kdtyp.syskdtyp and person.sysperson="+ sysperson, null).FirstOrDefault();

                switch (kdtyptyp)
                {
                    case 1: rval[0].kdtyp = AngAntBo.KDTYPID_PRIVAT; rval[0].kdtyptyp = AngAntBo.KDTYPTYP_PRIVAT; break;
                    case 3: rval[0].kdtyp = AngAntBo.KDTYPID_FIRMA; rval[0].kdtyptyp = AngAntBo.KDTYPTYP_FIRMA; break;
                    default: rval[0].kdtyp = 0; break;
                }
                if(kdtyptyp==1)
                {
                    PkzDto pkz = con.Query<PkzDto>(QUERY_PERSONPKZACTIVE, new { sysperson = sysperson }).FirstOrDefault();
                    if(pkz==null)
                        pkz = con.Query<PkzDto>(QUERY_PERSONPKZACTIVELAST, new { sysperson = sysperson }).FirstOrDefault();
                    if(pkz!=null){

                        pkz.zeinkCode = fixStringCode(pkz.zeinkCode);
                        pkz.wohnverhCode = fixStringCode(pkz.wohnverhCode);
                        pkz.beruflichCode = fixStringCode(pkz.beruflichCode);
                        pkz.auslagenCode = fixStringCode(pkz.auslagenCode);
                        pkz.sysangebot = null;
                        pkz.sysantrag = null;
                        pkz.syspkz = 0;
                        rval[0].pkz = new PkzDto[1];
                        rval[0].pkz[0]=pkz;
                    }
                }
                if (kdtyptyp == 3)
                {
                    UkzDto ukz = con.Query<UkzDto>(QUERY_PERSONUKZACTIVE, new { sysperson = sysperson }).FirstOrDefault();
                    if (ukz == null)
                        ukz = con.Query<UkzDto>(QUERY_PERSONUKZACTIVELAST, new { sysperson = sysperson }).FirstOrDefault();

                    if (ukz != null)
                    {
                        ukz.sysangebot = null;
                        ukz.sysantrag = null;
                        ukz.sysukz = 0;
                        rval[0].ukz = new UkzDto[1];
                        rval[0].ukz[0] = ukz;


                    }
                }
                return rval;
            }
        }

        /// <summary>
        /// UKZ via ID holen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <returns>Daten</returns>
        public UkzDto getUkz(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ITUKZ ukzOutput = (from itukz in context.ITUKZ
                                   where itukz.SYSITUKZ == sysid
                                   select itukz).FirstOrDefault();
                
                UkzDto rval = new UkzDto();
                Mapper.Map<ITUKZ, UkzDto>(ukzOutput, rval);
                rval.sysukz = ukzOutput.SYSITUKZ;
                rval.liquiditaet = (double?)ukzOutput.LIQUIDITAET;
                rval.obliboeigen = (double?)ukzOutput.OBLIGOEIGEN;

                return rval;
            }
        }

        /// <summary>
        /// PKZ via ID holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Daten</returns>
        public PkzDto getPkzPerson(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                PKZ pkzOutput = (from itpkz in context.PKZ
                                   where itpkz.SYSPKZ == sysid
                                   select itpkz).FirstOrDefault();
                
                PkzDto rval = new PkzDto();
                Mapper.Map<PKZ, PkzDto>(pkzOutput, rval);
                rval.syspkz = pkzOutput.SYSPKZ;
                rval.anzvollstr = (short)(pkzOutput.ANZAHLVOLLSTR.HasValue ? pkzOutput.ANZAHLVOLLSTR.Value : 0);
                if (pkzOutput.KREDRATE.HasValue)
                    rval.kredtrate = (double?)pkzOutput.KREDRATE.Value;
                rval.zeinkCode = fixStringCode(rval.zeinkCode);
                rval.wohnverhCode = fixStringCode(rval.wohnverhCode);
                rval.beruflichCode = fixStringCode(rval.beruflichCode);
                rval.auslagenCode = fixStringCode(rval.auslagenCode);
                return rval;
            }
        }

        /// <summary>
        /// UKZ via ID holen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <returns>Daten</returns>
        public UkzDto getUkzPerson(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                UKZ ukzOutput = (from itukz in context.UKZ
                                   where itukz.SYSUKZ == sysid
                                   select itukz).FirstOrDefault();
                
                UkzDto rval = new UkzDto();
                Mapper.Map<UKZ, UkzDto>(ukzOutput, rval);
                rval.sysukz = ukzOutput.SYSUKZ;
                rval.liquiditaet = (double?)ukzOutput.LIQUIDITAET;
                rval.obliboeigen = (double?)ukzOutput.OBLIGOEIGEN;
                return rval;
            }
        }

        public PkzDto getITPkzAktiv(long sysit)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                PkzDto rval = null;
                ITPKZ pkzOutput = (from itpkz in context.ITPKZ
                    join antrag in context.ANTRAG on itpkz.SYSANTRAG equals antrag.SYSID
                    where antrag.ATTRIBUT.Equals("Vertrag aktiviert") && itpkz.IT.SYSIT == sysit orderby antrag.ZUSTANDAM descending 
                    select itpkz).FirstOrDefault();
                if (pkzOutput == null)
                {
                    pkzOutput = (from itpkz in context.ITPKZ
                        where itpkz.IT.SYSIT == sysit 
                        && itpkz.SYSANGEBOT>0
                        orderby itpkz.SYSITPKZ descending 
                        select itpkz).FirstOrDefault();
                }
                if (pkzOutput == null)
                {
                    pkzOutput = (from itpkz in context.ITPKZ
                                 where itpkz.IT.SYSIT == sysit
                                 orderby itpkz.SYSITPKZ descending
                                 select itpkz).FirstOrDefault();
                }
                if (pkzOutput != null)
                {
                    
                    rval = new PkzDto();
                    Mapper.Map<ITPKZ, PkzDto>(pkzOutput, rval);
                    rval.syspkz = pkzOutput.SYSITPKZ;
                    rval.anzvollstr = (short) (pkzOutput.ANZAHLVOLLSTR.HasValue ? pkzOutput.ANZAHLVOLLSTR.Value : 0);
                    if (pkzOutput.KREDRATE.HasValue)
                        rval.kredtrate = (double?) pkzOutput.KREDRATE.Value;
                }
                if (rval != null)
                {
                    rval.zeinkCode = fixStringCode(rval.zeinkCode);
                    rval.wohnverhCode = fixStringCode(rval.wohnverhCode);
                    rval.beruflichCode = fixStringCode(rval.beruflichCode);
                    rval.auslagenCode = fixStringCode(rval.auslagenCode);
                }
                return rval;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public UkzDto getITUkzAktiv(long sysit)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                UkzDto rval = null;
                ITUKZ ukzOutput = (from itukz in context.ITUKZ
                            join antrag in context.ANTRAG on itukz.SYSANTRAG equals antrag.SYSID
                             where antrag.ATTRIBUT.Equals("Vertrag aktiviert") && itukz.IT.SYSIT == sysit
                             orderby antrag.ZUSTANDAM descending 
                             select itukz).FirstOrDefault();
                            
                if (ukzOutput == null)
                {
                    ukzOutput = (from itukz in context.ITUKZ
                                 where itukz.IT.SYSIT == sysit
                                 orderby itukz.SYSITUKZ descending 
                                 select itukz).FirstOrDefault();
                }
                if (ukzOutput == null)
                {
                    ukzOutput = (from itukz in context.ITUKZ
                                 where itukz.IT.SYSIT == sysit
                                 && itukz.SYSANGEBOT>0
                                 orderby itukz.SYSITUKZ descending
                                 select itukz).FirstOrDefault();
                }
                if (ukzOutput != null)
                {
                    
                    rval = new UkzDto();
                    Mapper.Map<ITUKZ, UkzDto>(ukzOutput, rval);
                    rval.sysukz = ukzOutput.SYSITUKZ;
                    rval.liquiditaet = (double?)ukzOutput.LIQUIDITAET;
                    rval.obliboeigen = (double?)ukzOutput.OBLIGOEIGEN;                   
                }

                return rval;
            }
        }


        /// <summary>
        /// Creates/Updates or deletes the kne
        /// </summary>
        /// <param name="kne"></param>
        /// <returns></returns>
        public KneDto createOrUpdateKne(KneDto kne)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                
                //String knetypcode = "OHNE";
                String relatetypecode = "WB";
                if (kne.flagdelete > 0)
                {
                    context.ExecuteStoreCommand("delete from itkne where sysitkne=" + kne.sysitkne);
                    return null;
                }
                //long sysknetyp = context.ExecuteStoreQuery<long>("select sysknetyp from knetyp where code='"+ knetypcode+"'").FirstOrDefault();

                if (kne.sysitkne == 0)
                {

                    List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = kne.sysunter });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = kne.sysober });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = relatetypecode });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = kne.sysarea });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p5", Value = kne.coderelatekind });

                    context.ExecuteStoreCommand("insert into itkne(sysunter,sysober,relatetypecode,sysarea,area,bezeichnung,geprueftam,activeflag,coderelatekind) values(:p1,:p2,:p3,:p4,'ANTRAG','Kontrollinhaber',sysdate,1 ,:p5)", parameters2.ToArray());
                    context.SaveChanges();

                    return context.ExecuteStoreQuery<KneDto>("select * from itkne where area='ANTRAG' and sysarea=" + kne.sysarea + " and sysober=" + kne.sysober + " and sysunter=" + kne.sysunter).FirstOrDefault();
                }
                else
                {
                    //updates not necessary when pkey already known
                    return context.ExecuteStoreQuery<KneDto>("select * from itkne where sysitkne=" + kne.sysitkne).FirstOrDefault();
                }


            }
        }

    }
}