// using AutoMapper;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using AutoMapper;
    using Cic.OpenOne.Common.Model.DdIc;
    using Cic.OpenOne.Common.Util.Extension;
    using Cic.OpenOne.Common.Util.Logging;
    using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa;
    using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Schufa.Type;
    using CIC.Database.IC.EF6.Model;

    /// <summary>
    /// Schufa DB Data Access Object
    /// </summary>
    public class SchufaDBDao : ISchufaDBDao
    {
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const String AUSKUNFTCFGBEZ = "SCHUFA";

        private readonly string[] meldungen = new[]
        {
            AuskunfttypDao.SchufaKorrekturAdresse,
            AuskunfttypDao.SchufaMeldungVertragsdaten,
            AuskunfttypDao.SchufaKorrekturVerbraucherdaten,
            AuskunfttypDao.SchufaLoeschungTodesfall,
            AuskunfttypDao.SchufaNamensAenderung,
            AuskunfttypDao.SchufaNeumeldungAdresse,
            AuskunfttypDao.SchufaNeumeldungTodesfall,
        };

        public NetworkCredential GetCredentials()
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                CIC.Database.OW.EF6.Model.AUSKUNFTCFG auskunft = context.AUSKUNFTCFG.Single(par => par.BEZEICHNUNG.ToUpper() == AUSKUNFTCFGBEZ);
                NetworkCredential rval = new NetworkCredential();
                rval.Password = auskunft.KEYVALUE;
                rval.UserName = auskunft.USERNAME;
                return rval;
            }
        }

        /// <summary>
        /// FindBySysId
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="auskunftTyp"></param>
        /// <returns>Schufa Information Data</returns>
        public SchufaInDto FindBySysId(long sysAuskunft, string auskunftTyp = null)
        {
            using (var context = new DdIcExtended())
            {
                try
                {
                    SchufaInDto schufaInDto = new SchufaInDto();

                    // Hole den AuskunftTyp
                    if (string.IsNullOrEmpty(auskunftTyp))
                    {
                        var auskunft = (from AuskTyp in context.AUSKUNFTTYP
                                        join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                        where Ausk.SYSAUSKUNFT == sysAuskunft
                                        select AuskTyp).Single();
                        auskunftTyp = auskunft.BEZEICHNUNG;
                    }

                    if (meldungen.Any(a => a == auskunftTyp))
                    {
                        MyFindBySysId_SchufaMeldung(context, sysAuskunft, schufaInDto);
                    }
                    else if (auskunftTyp == AuskunfttypDao.SchufaAbrufNachmeldung)
                    {
                        MyFindBySysId_SchufaAbrufNachmeldung(context, sysAuskunft, schufaInDto);
                    }
                    else if (auskunftTyp == AuskunfttypDao.SchufaAnfrageBonitaetsAuskunft)
                    {
                        MyFindBySysId_SchufaAnfrageBonitaetsAuskunft(context, sysAuskunft, schufaInDto);
                    }
                    else if (auskunftTyp == AuskunfttypDao.SchufaAbrufManuelleWeiterverarbeitung)
                    {
                        MyFindBySysId_SchufaAbrufManuelleWeiterverarbeitung(context, sysAuskunft, schufaInDto);
                    }
                    return schufaInDto;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Laden von Information Data in FindBySysId. Error Message. ", ex);
                    throw ex;
                }
            }
        }

        public void SaveInput(long sysAuskunft, SchufaInDto inDto, string auskunftTyp)
        {
            try
            {
                if (meldungen.Any(a => a == auskunftTyp))
                {
                    SaveMeldungInput(sysAuskunft, inDto);
                }
                else if (auskunftTyp == AuskunfttypDao.SchufaAbrufNachmeldung)
                {
                    SaveAbrufNachmeldungInput(sysAuskunft, inDto);
                }
                else if (auskunftTyp == AuskunfttypDao.SchufaAnfrageBonitaetsAuskunft)
                {
                    SaveAnfrageBonitaetsAuskunftInput(sysAuskunft, inDto);
                }
                else if (auskunftTyp == AuskunfttypDao.SchufaAbrufManuelleWeiterverarbeitung)
                {
                    SaveAbrufManuelleWeiterverarbeitungInput(sysAuskunft, inDto);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Fehler beim Speichern von Schufa Input (SchufaDBDao.{0}) : ", auskunftTyp), ex);
                throw ex;
            }
        }

        public void SaveOutput(long sysAuskunft, SchufaOutDto outDto, string auskunftTyp)
        {
            try
            {
                if (meldungen.Any(a => a == auskunftTyp))
                {
                    SaveMeldungOutput(sysAuskunft, outDto);
                }
                else if (auskunftTyp == AuskunfttypDao.SchufaAbrufNachmeldung)
                {
                    SaveAbrufNachmeldungOutput(sysAuskunft, outDto);
                }
                else if (auskunftTyp == AuskunfttypDao.SchufaAnfrageBonitaetsAuskunft || auskunftTyp == AuskunfttypDao.SchufaAbrufManuelleWeiterverarbeitung)
                {
                    SaveAnfrageBonitaetsAuskunftOutput(sysAuskunft, outDto);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Fehler beim Speichern von Schufa Output (SchufaDBDao.{0}) : ", auskunftTyp), ex);
                throw ex;
            }
        }

        private void MyFindBySysId_SchufaMeldung(DdIcExtended context, long sysAuskunft, SchufaInDto schufaInDto)
        {
            SFINPMELDUNG input = context.SFINPMELDUNG
                                        .Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);

            schufaInDto.Meldung = Mapper.Map<SFINPMELDUNG, SchufaMeldungInDto>(input);
        }

        private void MyFindBySysId_SchufaAbrufNachmeldung(DdIcExtended context, long sysAuskunft, SchufaInDto schufaInDto)
        {
            SFINPNACHMELDG input = context.SFINPNACHMELDG
                                          .Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);

            schufaInDto.AbrufNachmeldung = Mapper.Map<SFINPNACHMELDG, SchufaAbrufNachmeldungInDto>(input);
        }

        private void MyFindBySysId_SchufaAnfrageBonitaetsAuskunft(DdIcExtended context, long sysAuskunft, SchufaInDto schufaInDto)
        {
            SFINPBONIAUSK input = context.SFINPBONIAUSK
                                         .Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);

            schufaInDto.AnfrageBonitaetsauskunft = Mapper.Map<SFINPBONIAUSK, SchufaAnfrageBonitaetsauskunftInDto>(input);
        }

        private void MyFindBySysId_SchufaAbrufManuelleWeiterverarbeitung(DdIcExtended context, long sysAuskunft, SchufaInDto schufaInDto)
        {
            SFINPBONIAUSK input = context.SFINPBONIAUSK
                                         .Single(sfinp => sfinp.AUSKUNFT.SYSAUSKUNFT == sysAuskunft);

            schufaInDto.AbrufManuelleWeiterverarbeitung = Mapper.Map<SFINPBONIAUSK, SchufaAbrufManuelleWeiterverarbeitungInDto>(input);
        }

        public void SaveMeldungInput(long sysAuskunft, SchufaInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                var toSave = inDto.Meldung.Data;
                var inp = new SFINPMELDUNG();
                Mapper.Map(toSave, inp);
                Mapper.Map(toSave.Sterbedaten, inp);
                inp.SYSAUSKUNFT= sysAuskunft;
                context.SaveChanges();
            }
        }

        private void SaveAbrufNachmeldungInput(long sysAuskunft, SchufaInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                var toSave = inDto.AbrufNachmeldung.Data;
                var inp = new SFINPNACHMELDG();
                Mapper.Map(toSave, inp);
                inp.SYSAUSKUNFT = sysAuskunft;
                context.SaveChanges();
            }
        }

        private void SaveAnfrageBonitaetsAuskunftInput(long sysAuskunft, SchufaInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                var toSave = inDto.AnfrageBonitaetsauskunft.Data;
                var inp = new SFINPBONIAUSK();
                Mapper.Map(toSave, inp);
                inp.SYSAUSKUNFT = sysAuskunft;
                context.SaveChanges();
            }
        }

        private void SaveAbrufManuelleWeiterverarbeitungInput(long sysAuskunft, SchufaInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                var toSave = inDto.AbrufManuelleWeiterverarbeitung.Data;
                var inp = new SFINPBONIAUSK();
                Mapper.Map(toSave, inp);
                inp.SYSAUSKUNFT = sysAuskunft;
                context.SaveChanges();
            }
        }

        public void SaveMeldungOutput(long sysAuskunft, SchufaOutDto outDto)
        {
            SaveEntityOutput(sysAuskunft,
                outDto,
                outDto.Meldung,
                new SFOUTMELDUNG(),
                (table, ausnahme, auskunftKey) =>
                {
                    table.SFAUSNAHME = ausnahme;
                    table.SYSAUSKUNFT= auskunftKey;
                });
        }

        private void SaveAbrufNachmeldungOutput(long sysAuskunft, SchufaOutDto outDto)
        {
            SaveEntityOutput(sysAuskunft,
                outDto,
                outDto.AbrufNachmeldung,
                new SFOUTNACHMELDG(),
                (table, ausnahme, auskunftKey) =>
                {
                    table.SFAUSNAHME = ausnahme;
                    table.SYSAUSKUNFT = auskunftKey;
                });
        }

        private void SaveAnfrageBonitaetsAuskunftOutput(long sysAuskunft, SchufaOutDto outDto)
        {
            SaveEntityOutput(sysAuskunft,
                outDto,
                outDto.AnfrageBonitaetsauskunft,
                new SFOUTBONIAUSK(),
                (table, ausnahme, auskunftKey) =>
                {
                    table.SFAUSNAHME = ausnahme;
                    table.SYSAUSKUNFT = auskunftKey;
                });
        }

        /// <summary>
        /// Speichert einen Fehler in der Datenbank
        /// </summary>
        /// <typeparam name="TEntity">Typ, bei welchem der Fehler gespeichert werden soll</typeparam>
        /// <typeparam name="TOutput">Typ vom Output, das auf TEntity gemapt werden soll</typeparam>
        /// <param name="sysAuskunft">Id</param>
        /// <param name="outDto">SchufaOutDto</param>
        /// <param name="table">Tabellenobjekt</param>
        /// <param name="act">Die Aktion muss über eine anonyme Methode gemacht werden, damit man wieder die Information von T zur Verfügung hat.</param>
        private void SaveEntityOutput<TOutput, TEntity>(long sysAuskunft,
                                                        SchufaOutDto outDto,
                                                        TOutput entityToSave,
                                                        TEntity table,
                                                        Action<TEntity, SFAUSNAHME, long> act)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    Mapper.Map(entityToSave, table);
                    context.getObjectContext().AddObject(typeof(TEntity).Name, table);

                    if (outDto.Error == null)
                    {
                        act(table, null, sysAuskunft);
                        context.SaveChanges();
                    }
                    else
                    {
                        var error = outDto.Error;

                        SFAUSNAHME ausnahme = new SFAUSNAHME();
                        context.SFAUSNAHME.Add(ausnahme);
                        Mapper.Map(error, ausnahme);

                        act(table, ausnahme,   sysAuskunft);

                        context.SaveChanges();

                        foreach (var fehler in error.FehlerlisteFachlich ?? Enumerable.Empty<SchufaTFehler>())
                        {
                            SFFEHLER sffehler = Mapper.Map(fehler, new SFFEHLER());
                            sffehler.SYSSFAUSNAHMFACHL= ausnahme.SYSSFAUSNAHME;
                            context.SFFEHLER.Add(sffehler);
                        }
                        foreach (var fehler in error.FehlerlisteTechnisch ?? Enumerable.Empty<SchufaTFehler>())
                        {
                            SFFEHLER sffehler = Mapper.Map(fehler, new SFFEHLER());
                            sffehler.SYSSFAUSNAHMTECHN = ausnahme.SYSSFAUSNAHME;
                            context.SFFEHLER.Add(sffehler);
                        }

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Fehler beim Speichern von Schufa (SchufaDBDao.SaveEntity: {0}) : ", typeof(TEntity).Name), ex);
                throw ex;
            }
        }
    }
}