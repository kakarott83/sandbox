using System;
using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Kalkulation Data Access Object
    /// </summary>
    public class KalkulationDao : IKalkulationDao
    {
        public KalkulationDao() { }

        /// <summary>
        /// Create a new Kalkulation Dto
        /// </summary>
        /// <param name="sysId">Primary Key des Angebots</param>
        /// <returns>Primary Key</returns>
        public AngAntVarDto createKalkulation(long sysId)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                Conversion Converter = Conversion.Create();

                ANGEBOT angebot = (from ang in context.ANGEBOT
                                   where ang.SYSID == sysId
                                   select ang).FirstOrDefault();

                AngAntVarDto newVar = new AngAntVarDto();
                ANGVAR angvar = new ANGVAR();

                angvar = Converter.Convert<AngAntVarDto, ANGVAR>(newVar);

                newVar.kalkulation = new KalkulationDto();
                newVar.kalkulation.angAntKalkDto = new AngAntKalkDto();
                newVar.kalkulation.angAntProvDto = new List<AngAntProvDto>();
                newVar.kalkulation.angAntSubvDto = new List<AngAntSubvDto>();
                newVar.kalkulation.angAntVsDto = new List<AngAntVsDto>();

                context.ANGVAR.Add(angvar);
                angvar.ANGEBOT = angebot;

                //Calculation
                ANGKALK angkalk = new ANGKALK();
                angkalk = Converter.Convert<AngAntKalkDto, ANGKALK>(newVar.kalkulation.angAntKalkDto);
                context.ANGKALK.Add(angkalk);
                angkalk.SYSANGEBOT = angebot.SYSID;
                angkalk.ANGOB = null;

                //provisions
                foreach (AngAntProvDto prov in newVar.kalkulation.angAntProvDto)
                {
                    ANGPROV angprov = new ANGPROV();
                    angprov = Converter.Convert<AngAntProvDto, ANGPROV>(prov);
                    context.ANGPROV.Add(angprov);
                    angprov.ANGKALK = angkalk;
                }
                //insurances
                foreach (AngAntVsDto vs in newVar.kalkulation.angAntVsDto)
                {
                    ANGVS angvs = new ANGVS();
                    angvs = Converter.Convert<AngAntVsDto, ANGVS>(vs);
                    context.ANGVS.Add(angvs);
                    angvs.SYSANGEBOT = angebot.SYSID;
                }
                //subventions
                foreach (AngAntSubvDto subv in newVar.kalkulation.angAntSubvDto)
                {
                    ANGSUBV angsub = new ANGSUBV();
                    angsub = Converter.Convert<AngAntSubvDto, ANGSUBV>(subv);
                    angsub.ANGEBOT = angebot;
                }
                int iChanged = context.SaveChanges();
                newVar.sysangvar = angvar.SYSANGVAR;
                newVar.kalkulation.angAntKalkDto.syskalk = angkalk.SYSKALK;

                return newVar;
            }
        }

        /// <summary>
        /// Return existing Kalkulation Dto 
        /// </summary>
        /// <param name="sysVar">Primary Key der Variante</param>
        /// <returns></returns>
        public AngAntVarDto getKalkulation(long sysVar)
        {
            Conversion Converter = Conversion.Create();

            AngAntVarDto rval = null;

            using (DdOlExtended context = new DdOlExtended())
            {
                ANGVAR angvar = (from angv in context.ANGVAR
                                 where angv.SYSANGVAR == sysVar
                                 select angv).FirstOrDefault();

                if (angvar == null)
                {
                    throw new ApplicationException("No Calculation found on Database for this ID:" + sysVar);
                }
                rval = Converter.Convert<ANGVAR, AngAntVarDto>(angvar);
               
                
                rval.sysangebot = angvar.SYSANGEBOT.GetValueOrDefault();
                

                //Calculation
                ANGKALK angkalk = (from akalk in context.ANGKALK
                                   where akalk.ANGVAR.SYSANGVAR == sysVar
                                   select akalk).FirstOrDefault();

                rval.kalkulation = new KalkulationDto();
                rval.kalkulation.angAntKalkDto = Converter.Convert<ANGKALK, AngAntKalkDto>(angkalk);
                rval.kalkulation.angAntKalkDto.sysangvar = sysVar;

                //Provision
                rval.kalkulation.angAntProvDto = new List<AngAntProvDto>();
                List<ANGPROV> angprovList = (from avar in context.ANGPROV
                                             where avar.SYSANGVAR == sysVar
                                             select avar).ToList();
                foreach (ANGPROV angprov in angprovList)
                {
                    AngAntProvDto angprovDto = Converter.Convert<ANGPROV, AngAntProvDto>(angprov);
                    angprovDto.sysangvar = sysVar;
                    angprovDto.sysantrag = angvar.SYSANGEBOT.GetValueOrDefault();
                    
                    angprovDto.sysprov = angprov.SYSPROV;
                    rval.kalkulation.angAntProvDto.Add(angprovDto);
                }

                //Subvention
                rval.kalkulation.angAntSubvDto = new List<AngAntSubvDto>();
                List<ANGSUBV> angsubvList = (from avar in context.ANGSUBV
                                             where avar.ANGVAR.SYSANGVAR == sysVar
                                             select avar).ToList();
                foreach (ANGSUBV angsubv in angsubvList)
                {
                    AngAntSubvDto angsubvDto = Converter.Convert<ANGSUBV, AngAntSubvDto>(angsubv);
                    angsubvDto.sysangvar = sysVar;
                    angsubvDto.sysantsubv = angsubv.SYSANGSUBV;
                    rval.kalkulation.angAntSubvDto.Add(angsubvDto);
                }

                //Insurances
                rval.kalkulation.angAntVsDto = new List<AngAntVsDto>();
                List<ANGVS> angvsList = (from avar in context.ANGVS
                                         where avar.ANGVAR.SYSANGVAR == sysVar
                                         select avar).ToList();
                foreach (ANGVS angvs in angvsList)
                {
                    AngAntVsDto angvsDto = Converter.Convert<ANGVS, AngAntVsDto>(angvs);
                    angvsDto.sysangvar = sysVar;
                    angvsDto.sysantvs = angvs.SYSANGVS;
                    angvsDto.sysantrag = angvar.SYSANGEBOT.GetValueOrDefault();
                    
                    rval.kalkulation.angAntVsDto.Add(angvsDto);
                }
                return rval;
            }
        }

        /// <summary>
        /// Kalkulation aktualisieren
        /// </summary>
        /// <param name="kalkInput">Eingangs Kalkulation</param>
        /// <returns>Gespeicherte Kalkulation</returns>      
        public AngAntVarDto updateKalkulation(AngAntVarDto kalkInput)
        {
            Conversion Converter = Conversion.Create();
            using (DdOlExtended context = new DdOlExtended())
            {
                ANGEBOT angebotParent = (from ang in context.ANGEBOT
                                         where ang.SYSID == kalkInput.sysangebot
                                         select ang).FirstOrDefault();
                //Variants
                ANGVAR angvar = (from a in context.ANGVAR
                                 where a.SYSANGVAR == kalkInput.sysangvar
                                 select a).FirstOrDefault();
                if (angvar == null)//new angvar
                {
                    angvar = new ANGVAR();
                    context.ANGVAR.Add(angvar);
                }
                Converter.Convert<AngAntVarDto, ANGVAR>(kalkInput, angvar);
                angvar.ANGEBOT = angebotParent;

                //update Fields
                if (kalkInput.kalkulation != null)
                {
                    //Calculation
                    ANGKALK angkalk = (from a in context.ANGKALK
                                       where a.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                       select a).FirstOrDefault();
                    if (angkalk == null)
                    {
                        angkalk = new ANGKALK();
                        context.ANGKALK.Add(angkalk);
                        angkalk = Converter.Convert<AngAntKalkDto, ANGKALK>(kalkInput.kalkulation.angAntKalkDto);
                        angkalk.SYSANGVAR = angvar.SYSANGVAR;
                        
                    }
                    else
                    {
                        angkalk = Converter.Convert<AngAntKalkDto, ANGKALK>(kalkInput.kalkulation.angAntKalkDto);
                        angkalk.SYSANGVAR = angvar.SYSANGVAR;
                    }

                    //provisions---------------------------------------------------------------
                    if (kalkInput.kalkulation.angAntProvDto != null)
                    {
                        //get current Id's
                        List<long> currentIds = (from a in kalkInput.kalkulation.angAntProvDto
                                                 select a.sysprov).ToList();

                        //get entities that are no more in current id's
                        List<object> deleteEntities = (from a in context.ANGPROV
                                                       where !currentIds.Contains(a.SYSPROV)
                                                       && a.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                       select a).ToList<object>();
                        //delete old entities
                        foreach (object toDel in deleteEntities)
                            context.DeleteObject(toDel);

                        //Update/Insert changed ones
                        foreach (AngAntProvDto prov in kalkInput.kalkulation.angAntProvDto)
                        {
                            ANGPROV angprov = (from a in context.ANGPROV
                                               where a.SYSANGVAR == kalkInput.sysangvar
                                               select a).FirstOrDefault();
                            if (angprov == null)//new angvar
                            {
                                angprov = new ANGPROV();
                                context.ANGPROV.Add(angprov);
                            }
                            angprov = Converter.Convert<AngAntProvDto, ANGPROV>(prov);
                            angprov.SYSANGVAR = angvar.SYSANGVAR;
                            angprov.SYSVT = angvar.SYSANGVAR;
                        }
                    }
                    //insurances
                    if (kalkInput.kalkulation.angAntVsDto != null)
                    {
                        //get current Id's
                        List<long> currentIds = (from a in kalkInput.kalkulation.angAntVsDto
                                                 select a.sysangvs).ToList();

                        //get entities that are no more in current id's
                        List<object> deleteEntities = (from a in context.ANGVS
                                                       where !currentIds.Contains(a.SYSANGVS)
                                                       && a.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                       select a).ToList<object>();
                        //delete old entities
                        foreach (object toDel in deleteEntities)
                            context.DeleteObject(toDel);

                        //Update/Insert changed ones
                        foreach (AngAntVsDto prov in kalkInput.kalkulation.angAntVsDto)
                        {
                            ANGVS angvs = (from a in context.ANGVS
                                           where a.SYSANGVS == prov.sysangvs
                                           select a).FirstOrDefault();
                            if (angvs == null)//new angvar
                            {
                                angvs = new ANGVS();
                                context.ANGVS.Add(angvs);
                            }
                            angvs = Converter.Convert<AngAntVsDto, ANGVS>(prov);
                            angvs.SYSANGVAR = angvar.SYSANGVAR;

                        }
                    }
                    //subventions--------------------------------------
                    if (kalkInput.kalkulation.angAntSubvDto != null)
                    {
                        //get current Id's
                        List<long> currentIds = (from a in kalkInput.kalkulation.angAntSubvDto
                                                 select a.sysangsubv).ToList();

                        //get entities that are no more in current id's
                        List<object> deleteEntities = (from a in context.ANGSUBV
                                                       where !currentIds.Contains(a.SYSANGSUBV)
                                                       && a.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                       select a).ToList<object>();
                        //delete old entities
                        foreach (object toDel in deleteEntities)
                            context.DeleteObject(toDel);

                        //Update/Insert changed ones
                        foreach (AngAntSubvDto sub in kalkInput.kalkulation.angAntSubvDto)
                        {
                            ANGSUBV angsubv = (from a in context.ANGSUBV
                                               where a.SYSANGSUBV == sub.sysangsubv
                                               select a).FirstOrDefault();
                            if (angsubv == null)//new angvar
                            {
                                angsubv = new ANGSUBV();
                                context.ANGSUBV.Add(angsubv);
                            }
                            angsubv = Converter.Convert<AngAntSubvDto, ANGSUBV>(sub);
                            angsubv.SYSANGVAR = angvar.SYSANGVAR;
                        }
                    }
                }
                context.SaveChanges();
                return getKalkulation(angvar.SYSANGVAR);
            }
        }

        /// <summary>
        /// Kalkulation löschen
        /// Davon ausgehend, dass SYSANGVAR global eindeutig ist selektiere ich die Angebotvariante nur danach aus.
        /// </summary>
        /// <param name="sysVar">Primary Key der Variante</param>
        public void deleteKalkulation(long sysVar)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<ANGVAR> angvarList = (from angvar in context.ANGVAR
                                           where angvar.SYSANGVAR == sysVar
                                           select angvar).ToList<ANGVAR>();

                foreach (ANGVAR angvar in angvarList)
                {
                    if (angvar.ANGEBOT != null)
                    {
                        // Remove from ANGEBOT
                        ANGEBOT angebot = (from ang in context.ANGEBOT
                                           where ang.SYSID == angvar.ANGEBOT.SYSID
                                           select ang).FirstOrDefault();

                        //get Variant to delete from ANGEBOT
                        List<ANGVAR> deleteAngVars = (from a in angebot.ANGVARList
                                                      where a.SYSANGVAR == sysVar
                                                      select a).ToList();

                        //delete old entities
                        foreach (ANGVAR toDel in deleteAngVars)
                            context.DeleteObject(toDel);
                    }
                    //Select ANGKALK list
                    var ANGKALKQuery = from angkalk in context.ANGKALK
                                       where angkalk.ANGVAR.SYSANGVAR == sysVar
                                       select angkalk;

                    //Delete each ANGKALK
                    foreach (ANGKALK LoopANGKALK in ANGKALKQuery)
                    {
                        //Provision
                        List<ANGPROV> angprovList = (from avar in context.ANGPROV
                                                     where avar.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                     select avar).ToList();
                        foreach (ANGPROV angprov in angprovList)
                        {
                            context.DeleteObject(angprov);
                        }

                        //Subvention
                        List<ANGSUBV> angsubvList = (from avar in context.ANGSUBV
                                                     where avar.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                     select avar).ToList();
                        foreach (ANGSUBV angsubv in angsubvList)
                        {
                            context.DeleteObject(angsubv);
                        }

                        //Insurances
                        List<ANGVS> angvsList = (from avar in context.ANGVS
                                                 where avar.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                 select avar).ToList();
                        foreach (ANGVS angvs in angvsList)
                        {
                            context.DeleteObject(angvs);
                        }
                        context.DeleteObject(LoopANGKALK);
                    }
                    context.DeleteObject(angvar);
                }
                context.SaveChanges();
            }
        }



       
    }
}