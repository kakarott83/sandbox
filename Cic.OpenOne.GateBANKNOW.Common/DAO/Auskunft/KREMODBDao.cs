using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using CIC.Database.IC.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// KREMO DB Data Access Object
    /// </summary>
    public class KREMODBDao : IKREMODBDao
    {
        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates new KREMO, filled with data from KREMOInDto
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>SYSKREMO</returns>
        public long SaveKREMOInDto(KREMOInDto inDto)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    KREMO Kremo  = MyMapKremoInDtoToKremo(inDto);
                    context.KREMO.Add(Kremo);
                   
                    context.SaveChanges();
                    return Kremo.SYSKREMO;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von KREMO. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Creates or Updates KREMO, filled with data from KREMOInDto
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>SYSKREMO</returns>
        public long CreateOrUpdateKREMOInDto(KREMOInDto inDto)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    KREMO Kremo = null;
                    if (inDto.SysKremo > 0)
                        Kremo = context.KREMO.Where(par => par.SYSKREMO == inDto.SysKremo).Single();

                    if (Kremo == null)
                    {
                        Kremo = MyMapKremoInDtoToKremo(inDto);
                        context.KREMO.Add(Kremo);
                    }
                    else//Update
                    {
                        Kremo = MyMapKremoInDtoToKremo(inDto, Kremo);
                        Kremo.SYSANGEBOT = inDto.SysAngebot;
                        Kremo.SYSANTRAG = inDto.SysAntrag;
                        Kremo.SYSIT = inDto.SysIt;
                    }
                    context.SaveChanges();
                    return Kremo.SYSKREMO;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von KREMO. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Creates new KREMOInp linked with AUSKUNFT and KREMO
        /// </summary>
        /// <param name="sysAuskuft"></param>
        /// <param name="sysKremo"></param>
        public void SaveKREMOInp(long sysAuskuft, long sysKremo)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    KREMOINP KremoInp = new KREMOINP();
                    KremoInp.KREMO = context.KREMO.Where(par => par.SYSKREMO == sysKremo).Single();
                    KremoInp.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskuft).Single();
                    context.KREMOINP.Add(KremoInp);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von KREMOInp. ", ex);
                throw ex;
            }
        }        

        /// <summary>
        /// Updates existing KREMO and creates or updates KREMOOut
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="sysAuskunft"></param>
        /// <param name="sysKremo"></param>
        public void SaveKREMOOutDto(KREMOOutDto outDto, long sysAuskunft, long sysKremo)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    KREMO Kremo = context.KREMO.Where(par => par.SYSKREMO == sysKremo).Single();
                    if (Kremo != null)
                    {
                        Kremo.FEHLERCODE = outDto.ReturnCode.ToString();
                        Kremo.GRUNDBETRAG = (decimal?)outDto.Grundbetrag;
                        Kremo.SOZIALAUSL1 = (decimal?)outDto.Sozialausl1;
                        Kremo.SOZIALAUSL2 = (decimal?)outDto.Sozialausl2;
                        Kremo.SOZIALAUSL12 = (decimal?)(outDto.Sozialausl1 + outDto.Sozialausl2);

                        Kremo.BERECHKRANKENKASSE = (decimal?)outDto.Berechkrankenkasse;
                        Kremo.STEUERN = (decimal?)outDto.Steuern;
                        Kremo.STEUERN2 = (decimal?)outDto.Steuern2;
                        Kremo.BETRKIND1 = (decimal?)outDto.Kind1;
                        Kremo.BETRKIND2 = (decimal?)outDto.Kind2;
                        Kremo.BETRKIND3 = (decimal?)outDto.Kind3;

                        // Load KREMOOUTList, check if a KremoOut already exists
                        if(!context.Entry(Kremo).Collection(f => f.KREMOOUTList).IsLoaded)
                            context.Entry(Kremo).Collection(f => f.KREMOOUTList).Load();
                        
                        if (Kremo.KREMOOUTList.Count() == 0)
                        {
                            KREMOOUT Kremoout = new KREMOOUT();
                            Kremoout.KREMO = Kremo;
                            Kremoout.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                            context.KREMOOUT.Add(Kremoout);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von KREMOOut. Error Message. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets KREMO by sysAuskunft and returns KREMOInDto filled with data from KREMO
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>KremoInDto</returns>
        public KREMOInDto FindBySysId(long sysAuskunft)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    var KremoQuery = from Kremo in context.KREMO // Selektiere alle Kremos
                                     join KremoInp in context.KREMOINP on Kremo.SYSKREMO equals KremoInp.KREMO.SYSKREMO // die in KremoInp
                                     join Auskunft in context.AUSKUNFT on KremoInp.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT // mit Auskunft verbunden sind und
                                     where Auskunft.SYSAUSKUNFT == sysAuskunft // der gesuchten sysAuskunft entsprechen.
                                     select Kremo;
                    KREMO KREMO = KremoQuery.Single();
                    KREMOInDto KremoInDto = MyMapKremoToKremoInDto(KREMO);
                    return KremoInDto;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Laden von KREMO. Error Message. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Returns the latest Kremo for the proposal
        /// </summary>
        /// <param name="sysAntrag"></param>
        /// <returns></returns>
        public KREMOInDto FindBySysAntrag(long sysAntrag)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    var KremoQuery = (from Kremo in context.KREMO // Selektiere alle Kremos
                                      where Kremo.SYSANTRAG == sysAntrag // der gesuchten sysAuskunft entsprechen.
                                      select Kremo).OrderByDescending(f => f.SYSKREMO);
                    KREMO KREMO = KremoQuery.FirstOrDefault();
                    if (KREMO == null || KREMO.SYSKREMO == 0) return null;
                    KREMOInDto KremoInDto = MyMapKremoToKremoInDto(KREMO);
                    return KremoInDto;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Laden von KREMO. Error Message. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Returns the latest Kremo for the offer
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        public KREMOInDto FindBySysAngebot(long sysAngebot)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    var KremoQuery = (from Kremo in context.KREMO // Selektiere alle Kremos
                                     where Kremo.SYSANGEBOT==sysAngebot // der gesuchten sysAuskunft entsprechen.
                                     select Kremo).OrderByDescending(f=>f.SYSKREMO);
                    KREMO KREMO = KremoQuery.FirstOrDefault();
                    if (KREMO == null || KREMO.SYSKREMO == 0) return null;
                    KREMOInDto KremoInDto = MyMapKremoToKremoInDto(KREMO);
                    return KremoInDto;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Laden von KREMO. Error Message. ", ex);
                throw ex;
            }
        }


        #region My Methods
        private KREMOInDto MyMapKremoToKremoInDto(KREMO Kremo)
        {
            double outValue;
            KREMOInDto inDto = new KREMOInDto();
            inDto.SysKremo = Kremo.SYSKREMO;
            inDto.Anredecode = (double) Kremo.ANREDECODE;
            inDto.Anredecode2 = (double)Kremo.ANREDECODE2;
            inDto.Anzkind1 = (double)Kremo.ANZKIND1;
            inDto.Anzkind2 = (double) Kremo.ANZKIND2;
            inDto.Anzkind3 = (double) Kremo.ANZKIND3;
            inDto.Anzkind4 = (double)Kremo.ANZKIND4;
            inDto.Einkbrutto = (double)Kremo.EINKBRUTTO;
            inDto.Einkbrutto2 = (double)Kremo.EINKBRUTTO2;
            inDto.Einknetto = (double)Kremo.EINKNETTO;
            inDto.Einknetto2 = (double)Kremo.EINKNETTO2;
            inDto.Famstandcode = (double)Kremo.FAMSTANDCODE;
            inDto.Famstandcode2 = (double)Kremo.FAMSTANDCODE2;
            inDto.Kantoncode = (double)Kremo.KANTONCODE;
            inDto.Kantoncode2 = (double)Kremo.KANTONCODE2;
            if (Kremo.GEBDATUM.HasValue)
                inDto.GebDatum = Kremo.GEBDATUM.Value.Year * 10000 + Kremo.GEBDATUM.Value.Month * 100 + Kremo.GEBDATUM.Value.Day;
            if (Kremo.GEBDATUM2.HasValue)
                inDto.GebDatum2 = Kremo.GEBDATUM2.Value.Year * 10000 + Kremo.GEBDATUM2.Value.Month * 100 + Kremo.GEBDATUM2.Value.Day;
            inDto.Glz = (double)Kremo.GLZ;
            inDto.Grundcode = (double)Kremo.GRUNDCODE;
            inDto.Kreditsumme = (double) Kremo.KREDITSUMME;
            inDto.Miete = (double) Kremo.MIETE;
            inDto.Nebeneinkbrutto = (double)Kremo.NEBEINKBRUTTO;
            inDto.Nebeneinkbrutto2 = (double) Kremo.NEBEINKBRUTTO2;
            inDto.Nebeneinknetto = (double)Kremo.NEBEINKNETTO;
            inDto.Nebeneinknetto2 = (double)Kremo.NEBEINKNETTO2;
            inDto.Unterhalt = (double)Kremo.UNTERHALT;
            inDto.Unterhalt2 = (double)Kremo.UNTERHALT2;
            Double.TryParse(Kremo.PLZ, out outValue);
            inDto.Plz = outValue;
            Double.TryParse(Kremo.PLZ2, out outValue);
            inDto.Plz2 = outValue;
            inDto.Qstflag = (double)Kremo.QSTFLAG;
            inDto.Qstflag2 = (double)Kremo.QSTFLAG2;
            inDto.Rw = (double)Kremo.RW;
            inDto.Zins = (double)Kremo.ZINS;
            inDto.Zinsnomflag = (double)Kremo.ZINSNOMFLAG;
            inDto.Fininstcode = (double)Kremo.FININSTCODE.GetValueOrDefault();

            inDto.betreuungskosten = (double)Kremo.EXTBETRKOSTENTAT.GetValueOrDefault();
            inDto.arbeitswegpauschale1 = (double)Kremo.ARBEITSWEGAUSLAGEN.GetValueOrDefault();
            inDto.arbeitswegpauschale2 = (double)Kremo.ARBEITSWEGAUSLAGEN2.GetValueOrDefault();
            inDto.krankenkasse = (double)Kremo.KRANKENKASSE.GetValueOrDefault();

            inDto.SysIt = Kremo.SYSIT;
            inDto.SysAngebot = Kremo.SYSANGEBOT;
            inDto.SysAntrag = Kremo.SYSANTRAG;
            return inDto;
        }

        /// <summary>
        /// Creates a new KREMO and maps all inDto
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        private KREMO MyMapKremoInDtoToKremo(KREMOInDto inDto)
        {

            KREMO Kremo = new KREMO();
            Kremo.SYSANGEBOT = inDto.SysAngebot;
            Kremo.SYSANTRAG = inDto.SysAntrag;
            Kremo.SYSIT = inDto.SysIt;
            return MyMapKremoInDtoToKremo(inDto, Kremo);
        }
        /// <summary>
        /// Maps all Fields of inDto to Kremo
        /// </summary>
        /// <param name="inDto"></param>
        /// <param name="Kremo"></param>
        /// <returns></returns>
        private KREMO MyMapKremoInDtoToKremo(KREMOInDto inDto, KREMO Kremo)
        {
            Kremo.ANREDECODE = (int?)inDto.Anredecode;
            Kremo.ANREDECODE2 = (int?)inDto.Anredecode2;
            Kremo.ANZKIND1 = (int?)inDto.Anzkind1;
            Kremo.ANZKIND2 = (int?)inDto.Anzkind2;
            Kremo.ANZKIND3 = (int?)inDto.Anzkind3;
            Kremo.ANZKIND4 = (int?)inDto.Anzkind4;
            Kremo.EINKBRUTTO = (decimal?)inDto.Einkbrutto;
            Kremo.EINKBRUTTO2 = (decimal?)inDto.Einkbrutto2;
            Kremo.EINKNETTO = (decimal?)inDto.Einknetto;
            Kremo.EINKNETTO2 = (decimal?)inDto.Einknetto2;
            Kremo.FAMSTANDCODE = (int?)inDto.Famstandcode;
            Kremo.FAMSTANDCODE2 = (int?)inDto.Famstandcode2;
            Kremo.KANTONCODE = (int)inDto.Kantoncode;
            Kremo.KANTONCODE2 = (int)inDto.Kantoncode2;


            DateTime birthDate;
            string gebDate = inDto.GebDatum.ToString();
            if (DateTime.TryParseExact(gebDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out birthDate))
                Kremo.GEBDATUM = (DateTime?)birthDate;

            gebDate = inDto.GebDatum2.ToString();
            if (DateTime.TryParseExact(gebDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out birthDate))
                Kremo.GEBDATUM2 = (DateTime?)birthDate;
            
            Kremo.GLZ = (int?)inDto.Glz;
            Kremo.GRUNDCODE = (int?)inDto.Grundcode;
            Kremo.KREDITSUMME = (decimal?)inDto.Kreditsumme;
            Kremo.MIETE = (decimal?)inDto.Miete;
            Kremo.NEBEINKBRUTTO = (decimal?)inDto.Nebeneinkbrutto;
            Kremo.NEBEINKBRUTTO2 = (decimal?)inDto.Nebeneinkbrutto2;
            Kremo.NEBEINKNETTO = (decimal?)inDto.Nebeneinknetto;
            Kremo.NEBEINKNETTO2 = (decimal?)inDto.Nebeneinknetto2;
            Kremo.UNTERHALT = (decimal?)inDto.Unterhalt;
            Kremo.UNTERHALT2 = (decimal?)inDto.Unterhalt2;
            Kremo.PLZ = inDto.Plz.ToString();
            Kremo.PLZ2 = inDto.Plz2.ToString();
            Kremo.QSTFLAG = (int?)inDto.Qstflag;
            Kremo.QSTFLAG2 = (int?)inDto.Qstflag2;
            Kremo.RW = (decimal?)inDto.Rw;
            Kremo.ZINS = (decimal)inDto.Zins;
            Kremo.ZINSNOMFLAG = (int?)inDto.Zinsnomflag;

            Kremo.ARBEITSWEGAUSLAGEN = (decimal)inDto.arbeitswegpauschale1;
            Kremo.ARBEITSWEGAUSLAGEN2 = (decimal)inDto.arbeitswegpauschale2;
            Kremo.EXTBETRKOSTENTAT = (decimal)inDto.betreuungskosten;

            Kremo.KRANKENKASSE = (decimal)inDto.krankenkasse;

            return Kremo;
        }
        #endregion
    }
}
