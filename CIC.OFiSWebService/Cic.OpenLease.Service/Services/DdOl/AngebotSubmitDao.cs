using AutoMapper;
using Cic.OpenLease.Service.MediatorService;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Extension;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service.Services.DdOl
{
    /// <summary>
    /// Performs the Angebot Submit == Angebot to Antrag process, by copying all values and starting a bpe process
    /// </summary>
    public class AngebotSubmitDao
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /*public void test(long sysid)
        {
            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities Context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {
                var CurrentAngebot = (from Angebot in Context.ANGEBOT
                                      where Angebot.SYSID == sysid
                                      select Angebot).FirstOrDefault();
                submit(CurrentAngebot, "de-DE", 2914, 4399);
            }
        }*/

       /* public long getKey(System.Data.EntityKey key)
        {
            if (key == null) return 0;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return 0;
            return (long)member[0].Value;
        }*/

        /// <summary>
        /// Links all documents from angebot to the antrag
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="sysangebot"></param>
        public static void linkDocuments(String targetArea, long systarget, String sourceArea, long syssource)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                List<long> dmsidsAngebot = owCtx.ExecuteStoreQuery<long>("select dmsdoc.sysdmsdoc from dmsdoc,dmsdocarea where area='" + sourceArea + "' and sysid=" + syssource + " and dmsdoc.sysdmsdoc=dmsdocarea.sysdmsdoc", null).ToList();
                List<long> dmsidsAntrag = owCtx.ExecuteStoreQuery<long>("select dmsdoc.sysdmsdoc from dmsdoc,dmsdocarea where area='" + targetArea + "' and sysid=" + systarget + " and dmsdoc.sysdmsdoc=dmsdocarea.sysdmsdoc", null).ToList();
                if (dmsidsAngebot == null || dmsidsAngebot.Count == 0) return;
                foreach (long dmsId in dmsidsAngebot)
                {
                    if (dmsidsAntrag != null && dmsidsAntrag.Count > 0 && dmsidsAntrag.Contains(dmsId)) continue;//already there for antrag

                    //long sysantrag = owCtx.ExecuteStoreQuery<long>("select sysantrag from angebot where sysid=" + sysangebot, null).FirstOrDefault();
                    DMSDOCAREA docarea = new  DMSDOCAREA();
                    docarea.SYSDMSDOC=dmsId;
                    docarea.SYSID = systarget;
                    docarea.AREA = targetArea;
                    docarea.RANG = 1;
                    owCtx.DMSDOCAREA.Add(docarea);
                }
                owCtx.SaveChanges();
            }
        }

        /// <summary>
        /// BN-Code compatible offer submission to create a BN ANTRAG
        /// </summary>
        /// <param name="angebot"></param>
        /// <param name="isoCode"></param>
        /// <param name="sysperole"></param>
        /// <param name="syswfuser"></param>
        public long submit(ANGEBOT angebot, String isoCode, long sysperole, long syswfuser, int? zustflag)
        {
            //update IT
            Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kunde = updateIT(angebot.SYSIT.Value,angebot.SYSID,false);
            List<Tuple<Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>> mitantragsteller = new List<Tuple<ServiceAccess.DdOl.MitantragstellerDto, OpenOne.GateBANKNOW.Common.DTO.KundeDto>>();

            using (DdOlExtended Context = new DdOlExtended())
            {
                long sysit = Context.ExecuteStoreQuery<long>("select sysit from angebot where sysid=" + angebot.SYSID, null).FirstOrDefault();
                long syskdtyp = Context.ExecuteStoreQuery<long>("select syskdtyp from it where sysit=" + sysit, null).FirstOrDefault();

                String query = "select angobsich.sysid, angobsich.bezeichnung, angobsich.syshalter, angobsich.sysmh, angebot.sysid sysvt, angobsich.syssichtyp, angobsich.beginn, angobsich.ende, angobsich.sysit, angobsich.aktivflag aktivz, angobsich.rang, sichtyp.rang sichtyprang from angobsich, sichtyp,angebot where angobsich.sysvt=angebot.sysid and angobsich.syssichtyp=sichtyp.syssichtyp and sichtyp.rang in (10,80,140) and angebot.sysid=:sysid";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = angebot.SYSID });

                List<Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto> mas = Context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto>(query, parameters.ToArray()).ToList();
                foreach (Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto ma in mas)
                {
                    _Log.Debug("Updating MA " + ma.SYSIT+" for ANGEBOT "+ angebot.SYSID);

                    Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto mait = updateIT(ma.SYSIT, angebot.SYSID, syskdtyp == 3);
                    mait.sichtyprang = ma.SICHTYPRANG;
                    mitantragsteller.Add(new Tuple<Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto>(ma, mait));
                }

                
                
                if (syskdtyp == 3)
                {
                    //HCERDREI-705 ITPKZ für Gesellschafter
                    List<long> inhaber = Context.ExecuteStoreQuery<long>("select sysober from itkne where area='ANGEBOT' and relatetypecode in ('INH','GESELLS','KOMPL','PARTNER','VORSTAND','STIFTUNGSV','STIFTB','WB') and sysarea=" + angebot.SYSID + " and sysunter = " + sysit).ToList();
                    if (inhaber != null)
                    {
                        foreach (long sysinh in inhaber)
                        {
                            _Log.Debug("Updating INH/WB " + sysinh+" for ANGEBOT "+ angebot.SYSID);
                            Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto mait = updateIT(sysinh, angebot.SYSID,true);
                        }
                    }
                }
            



            //update Angebot

            try
                {
                    //Einreichung zu BNOW-Antrag

                    Cic.OpenOne.GateBANKNOW.Common.DAO.IAngAntDao angAntDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDao();
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto antrag = Mapper.Map< ANGEBOT, Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto>(angebot);

                    ANGKALK kalk = ANGEBOTHelper.GetAngkalkFromAngebot(Context, angebot.SYSID);

                    long sysob = kalk.SYSOB.GetValueOrDefault();// getKey(kalk.ANGOBReference.EntityKey);
                     ANGOB objekt = (from a in Context.ANGOB
                                                             where a.SYSOB == sysob
                                                             select a).FirstOrDefault();
                    ANGOBINI obini =  ANGOBHelper.GetAngobiniFromAngob(Context, objekt.SYSOB);

                    antrag.angAntObDto = Mapper.Map< ANGOB, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObDto>(objekt);
                    antrag.angAntObDto.bezeichnung = objekt.BEZEICHNUNG;
                    antrag.angAntObDto.ubnahmeKm = (obini.KMSTAND.HasValue ? obini.KMSTAND.Value : 0);
                    antrag.angAntObDto.erstzulassung = obini.ERSTZUL;
                    antrag.angAntObDto.brief = Mapper.Map< ANGOBINI, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntObBriefDto>(obini);
                    antrag.angAntObDto.brief.sitze = (short)(objekt.ANZAHLSITZE.HasValue ? objekt.ANZAHLSITZE.Value : 0);
                    antrag.angAntObDto.brief.kw = (short)(obini.KW.HasValue ? obini.KW.Value : 0);
                    antrag.angAntObDto.brief.ps = (short)(obini.PS.HasValue ? obini.PS.Value : 0);


                    //Kalkulation
                    antrag.kalkulation = new OpenOne.GateBANKNOW.Common.DTO.KalkulationDto();
                    antrag.kalkulation.angAntKalkDto = Mapper.Map< ANGKALK, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>(kalk);
                    antrag.kalkulation.angAntKalkDto.ll = (objekt.JAHRESKM.HasValue ? objekt.JAHRESKM.Value : 0);
                    antrag.kalkulation.angAntKalkDto.sysprproduct = angebot.SYSPRPRODUCT.Value;

                    if (kalk.RWBASE.HasValue)
                        antrag.kalkulation.angAntKalkDto.rw = (double)kalk.RWBASE.Value;
                    if (kalk.RWBASEBRUTTO.HasValue)
                        antrag.kalkulation.angAntKalkDto.rwBrutto = (double)kalk.RWBASEBRUTTO.Value;

                    //Versicherungen
                   /* antrag.kalkulation.angAntVsDto = Context.ExecuteStoreQuery<Cic.OpenOne.Common.DTO.AngAntVsDto>("select * from angvs where sysangebot=" + angebot.SYSID, null).ToList();
                    foreach (Cic.OpenOne.Common.DTO.AngAntVsDto vs in antrag.kalkulation.angAntVsDto)
                        vs.sysangvs = 0;
                    */
                    antrag.kalkulation.angAntProvDto = new List<OpenOne.Common.DTO.AngAntProvDto>();
                    antrag.kalkulation.angAntSubvDto = new List<OpenOne.Common.DTO.AngAntSubvDto>();
                    antrag.kalkulation.angAntAblDto = new List<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntAblDto>();

                    antrag.kunde = kunde;
                    
                    antrag.erfassungsclient = 1;//not b2b
                    antrag.sysid = 0;
                    antrag.mitantragsteller = null;
                    antrag.sysVM = 0;
                    if(angebot.SYSVM.HasValue)
                        antrag.sysVM = angebot.SYSVM.Value;
                    antrag = angAntDao.createAntrag(antrag, sysperole);
                    foreach (Tuple<Cic.OpenLease.ServiceAccess.DdOl.MitantragstellerDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto> ma in mitantragsteller)
                    {
                        var sysIt = ma.Item2.sysit;
                        var angobSichSysId = ma.Item1.SysId;
                        var aktivflag = 1;
                        var sysAntrag = antrag.sysid;

                        //Alles wird übernommen ausser SYSAVT
                        String antobQuery = @"insert into antobsich(SYSID,SYSVT,SYSPERSON,SYSOB,SYSRAP,SYSMH,RANG,TYP,STATUS,STATUSDAT,    AKTIVFLAG    ,SCORE,OBLIGOFLAG,BEZEICHNUNG,GEGENSTAND,AK,WERT,SYSWAEHRUNG,BEGINN,STAND,ENDE,KAUTION,BEGINNKAUTION,STANDKAUTION,ZINS,PERIODE,BRUTTOKAUTION,OPTION1,OPTION2,OPTION3,OPTION4,OPTION5,OPTION6,OPTION7,OPTION8,OPTION9,OPTION10,SYSSICHTYP,SYSNKONTO,SBBETRAG,SBPROZENT,FLAG01,FLAG02,FLAG03,FLAG04,FLAG05,FLAG06,FLAG07,FLAG08,FLAG09,FLAG10,    SYSIT        ,SYSANTRAG    ,SYSKD)
                                             select                     0,SYSVT,SYSPERSON,SYSOB,SYSRAP,SYSMH,RANG,TYP,STATUS,STATUSDAT," + aktivflag + ",SCORE,OBLIGOFLAG,BEZEICHNUNG,GEGENSTAND,AK,WERT,SYSWAEHRUNG,BEGINN,STAND,ENDE,KAUTION,BEGINNKAUTION,STANDKAUTION,ZINS,PERIODE,BRUTTOKAUTION,OPTION1,OPTION2,OPTION3,OPTION4,OPTION5,OPTION6,OPTION7,OPTION8,OPTION9,OPTION10,SYSSICHTYP,SYSNKONTO,SBBETRAG,SBPROZENT,FLAG01,FLAG02,FLAG03,FLAG04,FLAG05,FLAG06,FLAG07,FLAG08,FLAG09,FLAG10," + sysIt + "," + sysAntrag + ",SYSKD from angobsich where sysid=" + angobSichSysId;


                        Context.ExecuteStoreCommand(antobQuery, null);

                        using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new OpenOne.Common.Model.DdOl.DdOlExtended())
                        {
                            ((Cic.OpenOne.GateBANKNOW.Common.DAO.AngAntDao)angAntDao).SetAntragIdInZusatzdaten(ctx, ma.Item2, antrag.sysid);
                            ctx.SaveChanges();


                            try
                            {

                                List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysIt });
                                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = angebot.SYSID });
                                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = antrag.sysid });
                                Context.ExecuteStoreCommand("update itpkz set sysantrag=:sysantrag where sysit=:sysit and sysangebot=:sysangebot and (sysantrag=0 or sysantrag is null)", parameters2.ToArray());

                                parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysIt });
                                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = angebot.SYSID });
                                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = antrag.sysid });
                                Context.ExecuteStoreCommand("update itukz set sysantrag=:sysantrag where sysit=:sysit and sysangebot=:sysangebot and (sysantrag=0 or sysantrag is null)", parameters2.ToArray());
                            }
                            catch (Exception exs)
                            {
                                _Log.Warn("ITPKZ MA Antrag-Reference not saved: " + exs.Message);
                            }

                        }

                    }
                    Context.SaveChanges();
                    try
                    {
                        String provQuery = @"insert into antprov(SYSVT,SYSVM,SYSVTOBSL,SYSFI,BASIS,PROVISIONPRO,PROVISION,VALUTA,TEXT,USTPFLICHT,ART,ABRECHNUNG,INAKTIV,INAKTIVBIS,SYSPARTNER,SYSPRPROVTYPE,                SYSPROVTARIF,SYSANTRAG,PROVISIONUST,PROVISIONBRUTTO,RAPPROVISIONBRUTTOMIN,RAPPROVISIONBRUTTOMAX,PROVISIONP,FLAGLOCKED,DEFPROVISION,DEFPROVISIONBRUTTO,DEFPROVISIONUST,DEFPROVISIONP,SYSMWST)
                                             select              SYSVT,SYSVM,SYSVTOBSL,SYSFI,BASIS,PROVISIONPRO,PROVISION,VALUTA,TEXT,USTPFLICHT,ART,ABRECHNUNG,INAKTIV,INAKTIVBIS,SYSPARTNER,SYSPRPROVTYPE,                SYSPROVTARIF," + antrag.sysid + ",PROVISIONUST,PROVISIONBRUTTO,RAPPROVISIONBRUTTOMIN,RAPPROVISIONBRUTTOMAX,PROVISIONP,FLAGLOCKED,DEFPROVISION,DEFPROVISIONBRUTTO,DEFPROVISIONUST,DEFPROVISIONP,SYSMWST from angprov where sysob=" + kalk.SYSKALK;

                        Context.ExecuteStoreCommand(provQuery,null);
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring Provisions to Antrag ", uf);
                    }
                    try
                    {
                        String austQuery = @"insert into antobaust(SYSOB,SYSANTOB,SYSANGOB,SNR,BESCHREIBUNG,BETRAG,SYSMYCALC,FLAGRWREL,FLAGPACKET,FREITEXT,BETRAG2,FLAGSERIE)
                                             select                SYSOB," + antrag.angAntObDto.sysob + ",SYSANGOB,SNR,BESCHREIBUNG,BETRAG,SYSMYCALC,FLAGRWREL,FLAGPACKET,FREITEXT,BETRAG2,FLAGSERIE from angobaust where sysangob=" + sysob;

                        Context.ExecuteStoreCommand(austQuery, null);
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring Austs to Antrag ", uf);
                    }
                    try
                    {
                        String vsQuery = @"insert into antvs(SYSANTRAG,SYSVSTYP,PRAEMIENSTUFE,DECKUNGSSUMME,VORVERSICHERUNG,NACHLASS,PRAEMIE,PRAEMIEDEFAULT,VERSICHERUNGSSTEUER,PRAEMIENETTO,ZUBEHOERFINANZIERT,FREMDVERSICHERUNG,POLKENNZEICHEN,SB1,SB2,SYSVS,CODE,PPY,LZ,PRAEMIEP,MITFINFLAG,KOSTENABSCHL,KOSTENVERW,KOSTENSONST,PRAEMIELS,PRAEMIEVS)
                                            select " + antrag.sysid + ",SYSVSTYP,PRAEMIENSTUFE,DECKUNGSSUMME,VORVERSICHERUNG,NACHLASS,PRAEMIE,PRAEMIEDEFAULT,VERSICHERUNGSSTEUER,PRAEMIENETTO,ZUBEHOERFINANZIERT,FREMDVERSICHERUNG,POLKENNZEICHEN,SB1,SB2,SYSVS,CODE,PPY,LZ,PRAEMIEP,MITFINFLAG,KOSTENABSCHL,KOSTENVERW,KOSTENSONST,PRAEMIELS,PRAEMIEVS from angvs where sysangebot=" + angebot.SYSID;


                        Context.ExecuteStoreCommand(vsQuery, null);
                        Context.SaveChanges();
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring ANGVS to Antrag ", uf);
                    }
                    try
                    {
                        String vsQuery = @"insert into antvspos(sysAntvs,sysVSTypPos,PraemieNetto,Praemie,Versicherungssteuer,KOSTENABSCHL,KOSTENVERW,KOSTENSONST,PRAEMIELS,PRAEMIEVS)
                                            select (select sysantvs from antvs,antrag where antrag.sysid=antvs.sysantrag and antrag.sysangebot=angvs.sysangebot and antvs.sysvstyp=angvs.sysvstyp) sysantvs,sysVSTypPos,angvspos.PraemieNetto,angvspos.Praemie,angvspos.Versicherungssteuer,angvspos.KOSTENABSCHL,angvspos.KOSTENVERW,angvspos.KOSTENSONST,angvspos.PRAEMIELS,angvspos.PRAEMIEVS from angvspos,angvs where angvs.sysangvs=angvspos.sysangvs and angvs.SYSANGEBOT=" + angebot.SYSID;


                        Context.ExecuteStoreCommand(vsQuery, null);
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring ANGVSPOS to Antrag ", uf);
                    }
                    try
                    {
                        String subvQuery = @"insert into antsubv(BETRAG,BEGINN,LZ,SYSSUBVTYP,SYSSUBVG,SYSANTRAG, BETRAGBRUTTO,BETRAGUST,          CODE,SYSPRSUBV)
                                		      select BETRAG,BEGINN,LZ,SYSSUBVTYP,SYSSUBVG," + antrag.sysid + ",BETRAGBRUTTO,BETRAGUST,          CODE,SYSPRSUBV from angsubv where sysangebot=" + angebot.SYSID;

                        Context.ExecuteStoreCommand(subvQuery, null);
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring Subventions to Antrag ", uf);
                    }
                    try
                    {
                        String subvQuery = @"insert into antoboption(SYSID,PDEC1501,PDEC1502)
                                		      select " + antrag.angAntObDto.sysob + ",PDEC1501,PDEC1502 from angoboption where sysid=" + sysob;

                        Context.ExecuteStoreCommand(subvQuery, null);
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring Subventions to Antrag ", uf);
                    }
                    try
                    {
                        String ablQuery = @"insert into antabl(sysantrag,bank,iban,betrag,flagintext,aktuellerate,fremdvertrag,DATKALKPER )
                                    select " + antrag.sysid+ ",bank,iban,betrag,flagintext,aktuellerate,fremdvertrag,DATKALKPER  from angabl where sysangebot=" + angebot.SYSID;

                        Context.ExecuteStoreCommand(ablQuery, null);
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring ABL to Antrag ", uf);
                    }
                    try
                    {
                       
                        List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = angebot.SYSIT });
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = angebot.SYSID });
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = antrag.sysid });
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bwazustflag", Value = zustflag.HasValue?zustflag.Value:0 });
						//parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "prevname", Value = prevname });
						Context.ExecuteStoreCommand("update itpkz set sysantrag=:sysantrag, bwazustflag=:bwazustflag where sysit=:sysit and sysangebot=:sysangebot and (sysantrag=0 or sysantrag is null)", parameters2.ToArray());

                        parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = angebot.SYSIT });
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = angebot.SYSID });
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = antrag.sysid });
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bwazustflag", Value = zustflag.HasValue ? zustflag.Value : 0 });
                        Context.ExecuteStoreCommand("update itukz set sysantrag=:sysantrag, bwazustflag=:bwazustflag  where sysit=:sysit and sysangebot=:sysangebot and (sysantrag=0 or sysantrag is null)", parameters2.ToArray());
                    }
                    catch (Exception exs)
                    {
                        _Log.Warn("ITPKZ Antrag-Reference not saved: " + exs.Message);
                    }
                    try
                    {
                        List<long> inhaber = Context.ExecuteStoreQuery<long>("select sysober from itkne where area='ANGEBOT' and relatetypecode in ('INH','GESELLS','KOMPL','PARTNER','VORSTAND','STIFTUNGSV','STIFTB','WB') and sysarea=" + angebot.SYSID + " and sysunter = " + angebot.SYSIT).ToList();
                        foreach (long sysinh in inhaber)
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters3 = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysinh });
                            parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = angebot.SYSID });
                            parameters3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = antrag.sysid });
                            
                            
                            Context.ExecuteStoreCommand("update itpkz set sysantrag=:sysantrag  where sysit=:sysit and sysangebot=:sysangebot and (sysantrag=0 or sysantrag is null)", parameters3.ToArray());

                        }
                    }
                    catch (Exception exs)
                    {
                        _Log.Warn("ITPKZ Antrag-KNE not saved: " + exs.Message);
                    }

                    
                    try
                    {
                        List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = objekt.LIEFERUNG });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = antrag.angAntObDto.sysob});
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sonzub", Value = objekt.SONZUB });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pakete", Value = objekt.PAKETE });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "herzub", Value = objekt.HERZUB });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ahkrabatto", Value = objekt.AHKRABATTO });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ahkrabattobrutto", Value = objekt.AHKRABATTOBRUTTO });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "erstzul", Value = obini.ERSTZUL });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "grund", Value = objekt.GRUND });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "erinklmwst", Value = objekt.ERINKLMWST });

                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ahk", Value = objekt.AHK });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ahkust", Value = objekt.AHKUST });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ahkbrutto", Value = objekt.AHKBRUTTO });


                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "satzmehrkm", Value = objekt.SATZMEHRKM });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "SatzMehrKmBrutto", Value = objekt.SATZMEHRKMBRUTTO });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "SatzMinderKm", Value = objekt.SATZMINDERKM });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "SatzMinderKmBrutto", Value = objekt.SATZMINDERKMBRUTTO });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "KMToleranz", Value = objekt.KMTOLERANZ });

                        long fztyp = Context.ExecuteStoreQuery<long>("select fztyp from angob where sysangebot="+angebot.SYSID,null).FirstOrDefault();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "fztyp", Value = fztyp });

                        Context.ExecuteStoreCommand("UPDATE antob set fztyp=:fztyp, satzmehrkm=:satzmehrkm,SatzMehrKmBrutto=:SatzMehrKmBrutto,KMToleranz=:KMToleranz,SatzMinderKmBrutto=:SatzMinderKmBrutto,SatzMinderKm=:SatzMinderKm,ahk=:ahk,ahkust=:ahkust,ahkbrutto=:ahkbrutto, erinklmwst=:erinklmwst,erstzul=:erstzul,lieferung=:p1,sonzub=:sonzub,pakete=:pakete,herzub=:herzub,ahkrabatto=:ahkrabatto,grund=:grund,ahkrabattobrutto=:ahkrabattobrutto  where sysob=:id", pars.ToArray());

                        pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = antrag.angAntObDto.sysob});
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = angebot.SYSID });

                        Context.ExecuteStoreCommand(@"merge into antob antob
                                                                    using (select * from angob where sysangebot=:sysangebot) angob
                                                                    on (antob.sysob=:id and angob.sysangebot=:sysangebot)
                                                                    when matched then update set 
                                                                    antob.typ=angob.typ,
                                                                    antob.WerkAbgabePreisrabatto=angob.WerkAbgabePreisrabatto,
                                                                    antob.WerkAbgabePreisrabattop=angob.WerkAbgabePreisrabattop,
                                                                    antob.werkabgabepreisbrutto=angob.werkabgabepreisbrutto,
                                                                    antob.WerkAbgabePreis=angob.WerkAbgabePreis,
                                                                    antob.WerkAbgabePreisExternBrutto=angob.WerkAbgabePreisExternBrutto,
                                                                    antob.WerkAbgabePreisExternUSt=angob.WerkAbgabePreisExternUSt,
                                                                    antob.WerkAbgabePreisExtern=angob.WerkAbgabePreisExtern,
                                                                    antob.Gesamt=angob.Gesamt,
                                                                    antob.GesamtUSt=angob.GesamtUSt,
                                                                    antob.GesamtBrutto=angob.GesamtBrutto", pars.ToArray());
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring lieferung to Antob ", uf);
                    }

                    try
                    {
                        if (kalk.RWKALK.HasValue && kalk.RWKALKBRUTTO.HasValue)
                        {
                            List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = kalk.RWKALK.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = kalk.RWKALKBRUTTO.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = kalk.AHK.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = kalk.AHKBRUTTO.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p5", Value = kalk.RWKALK.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p6", Value = kalk.RWKALKBRUTTO.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = antrag.kalkulation.angAntKalkDto.syskalk });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p7", Value = kalk.SZBRUTTOP.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p8", Value = kalk.RWBASE.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p9", Value = kalk.RATEGESAMTBRUTTO.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p10", Value = kalk.ERSTERATE.GetValueOrDefault() });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p11", Value = kalk.LETZTERATE.GetValueOrDefault() });
                            Context.ExecuteStoreCommand("UPDATE antkalk set ersterate=:p10, letzterate=:p11,RATEGESAMTBRUTTO =:p9, rwbase=:p8,szbruttop=:p7, rwbrutto=:p6,rw=:p5, rwkalk=:p1,rwkalkbrutto=:p2,ahk=:p3,ahkbrutto=:p4  where syskalk=:id", pars.ToArray());
                        }
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring Zielrate to Antrag ", uf);
                    }
                    List<Devart.Data.Oracle.OracleParameter> parsf = new List<Devart.Data.Oracle.OracleParameter>();
                    parsf.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = kalk.BWFEHLER.Value });
                    parsf.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = antrag.kalkulation.angAntKalkDto.syskalk });
                    Context.ExecuteStoreCommand("UPDATE antkalk set bwfehler=:p1  where syskalk=:id", parsf.ToArray());
                
                    try
                    {
                        if (kalk.RWKALK.HasValue && kalk.RWKALKBRUTTO.HasValue)
                        {
                            List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = kalk.RWBASEBRUTTO.Value });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = kalk.RWBASEBRUTTOP.Value });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = kalk.RWKALKBRUTTODEF.Value });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = kalk.RWKALKBRUTTOPDEF.Value });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = antrag.kalkulation.angAntKalkDto.syskalk });
                            Context.ExecuteStoreCommand("UPDATE antkalk set rwbasebrutto=:p1, rwbasebruttop=:p2,rwkalkbruttodef=:p3,rwkalkbruttopdef=:p4 where syskalk=:id", pars.ToArray());
                        }
                    }
                    catch (Exception uf)
                    {
                        _Log.Warn("Transferring rwbasebrutto/rwbasebruttop to Antrag failed: "+uf.Message);
                    }

                    //Sicherheit ZBII
                    SICHTYP sich = SICHTYPHelper.GetSichTyp(Context, ANGEBOTAssembler.ZBIISICHRANG);
                    ANTOBSICH sicherheit = new ANTOBSICH();
                    Context.ANTOBSICH.Add(sicherheit);
                    if (sicherheit != null)
                    {
                        sicherheit.RANG = ANGEBOTAssembler.ZBIISICHRANG;
                        sicherheit.BEZEICHNUNG = "Zulassungsbescheinigung";
                        sicherheit.SYSIT=angebot.SYSIT.Value;
                        sicherheit.SYSSICHTYP= sich.SYSSICHTYP;
                        sicherheit.SYSANTRAG = antrag.sysid;
                        sicherheit.AKTIVFLAG = 1;
                    }

                    Context.SaveChanges();

                    _Log.Debug("Antrag created");
                    
                    //link angebot/antrag
                    Context.ExecuteStoreCommand("UPDATE ANTRAG set sysangebot=" + angebot.SYSID + " where sysid=" + antrag.sysid, null);
                    Context.ExecuteStoreCommand("UPDATE ANGEBOT set sysantrag=" + antrag.sysid + ", GUELTIGBIS=sysdate+90 where sysid=" + angebot.SYSID, null);

                    try
                    {
                        List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "data", Value = angebot.DATANGEBOT });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = antrag.sysid });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbrand", Value =  angebot.SYSBRAND });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysoppo", Value = angebot.SYSOPPO });
                        Context.ExecuteStoreCommand("UPDATE ANTRAG set sysoppo=:sysoppo, datangebot=:data, sysbrand=:sysbrand where sysid=:sysid", pars.ToArray());
                    }
                    catch (Exception uf)
                    {
                        _Log.Error("Error transferring some angebot-fields to antrag ", uf);
                    }
                    linkDocuments("ANTRAG", antrag.sysid, "ANGEBOT", angebot.SYSID);

                    Cic.OpenOne.GateBANKNOW.Common.DAO.EaihotDao eaihotDao = new OpenOne.GateBANKNOW.Common.DAO.EaihotDao();
                    CIC.Database.OW.EF6.Model.EAIART art = eaihotDao.getEaiArt("MO_START_BPE");
                    Cic.OpenOne.Common.DTO.EaihotDto eaioutput = new Cic.OpenOne.Common.DTO.EaihotDto()
                    {
                        CODE = "MO_START_BPE",
                        OLTABLE = "ANTRAG",
                        SYSOLTABLE = antrag.sysid,
                        SYSEAIART = art.SYSEAIART,
                        SUBMITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                        SUBMITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                        EVE = 1,
                        PROZESSSTATUS = (int)Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotStatusConstants.Pending,
                        INPUTPARAMETER1 = antrag.sysid.ToString(),
                        INPUTPARAMETER2 = kunde.sysit.ToString(),
                        HOSTCOMPUTER = "*"
                    };
                    eaioutput = eaihotDao.createEaihot(eaioutput);
                    _Log.Debug("EVE Job created");
                    return antrag.sysid;

                }
                catch (Exception e)
                {
                    _Log.Error("Angebot to Antrag failed with " + e.Message, e);
                    throw new Exception("Angebot to Antrag failed",e);
                }
            }

        }



        /// <summary>
        /// BN-Code compatible offer re-submission to create a BN ANTRAG
        /// </summary>
        /// <param name="angebot"></param>
        /// <param name="isoCode"></param>
        /// <param name="sysperole"></param>
        /// <param name="syswfuser"></param>
        public long resubmit( ANGEBOT angebot, String isoCode, long sysperole, long syswfuser)
        {

            return submit(angebot, isoCode, sysperole, syswfuser,null);
           

        }

        /// <summary>
        /// Check Error and return exception with message if error found
        /// </summary>
        /// <param name="resp"></param>
        private static ResponseBase checkError(ResponseBase resp)
        {
            if (resp is ErrorResponse)
            {
                ErrorResponse errInfo = (ErrorResponse)resp;
                throw new Exception(errInfo.Error);
            }
            return resp;
        }

        /// <summary>
        /// update the it-structure used from bn with the aida values. IT-Data will be merged in ITPKZ/ITUKZ, Compliance created for ITPKZ-Area Compliance
        /// </summary>
        /// <param name="sysit"></param>
        private Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto updateIT(long sysit, long sysangebot, bool skipHaushalt)
        {

            //load currently saved it into aida-itdto-structure (==all fields written from aida)
            //load it (kundedto) with bn logic (==fields needed for antrag-submission without some needed fields)
            //map needed fields from itdto-structure to kundedto
            //save with bn logic
            _Log.Debug("Updating IT-Structure for " + sysit);
            try
            {
                Cic.OpenLease.ServiceAccess.DdOl.ITDto it;
            
                //hregisterort, gebort
                using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                {
                    it = ctx.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.DdOl.ITDto>("select * from it where sysit=" + sysit, null).FirstOrDefault();
                }

                Cic.OpenOne.GateBANKNOW.Common.DAO.KundeDao kddao = new Cic.OpenOne.GateBANKNOW.Common.DAO.KundeDao();
                Cic.OpenOne.GateBANKNOW.Common.BO.KundeBo kundeBo = new Cic.OpenOne.GateBANKNOW.Common.BO.KundeBo(kddao);

                //get the IT mapped to bnow-structure
                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto kd = kddao.getKunde(sysit, sysangebot);
                int? orgsmsflag = kd.infoSmsFlag;
                kd.telefon2 = it.TELEFON;//telefon2-field of KundeDto will go to TELEFON!
                kd.telefon = it.PTELEFON;//telefon-field of KundeDto will go to PTELEFON!
                if (kd.syskdtyp == 1 || kd.syskdtyp == 2)//PKZ
                {
                    if (kd.zusatzdaten[0].pkz == null || kd.zusatzdaten[0].pkz.Length == 0||kd.zusatzdaten[0].pkz[0].sysangebot!=sysangebot)
                        kd.zusatzdaten[0].pkz = new OpenOne.GateBANKNOW.Common.DTO.PkzDto[1];
                    Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto pkz = kd.zusatzdaten[0].pkz[0];
                    if (pkz == null)
                    {
                        pkz = new Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto();
                        kd.zusatzdaten[0].pkz[0] = pkz;
                    }

                    //map it-data to pkz
                    pkz = Mapper.Map<Cic.OpenLease.ServiceAccess.DdOl.ITDto, Cic.OpenOne.GateBANKNOW.Common.DTO.PkzDto>(it, pkz);
                    pkz.wohnart = it.WOHNUNGART;
                    pkz.sysangebot = sysangebot;
                    if (skipHaushalt)
                        pkz.wohnverhCode = "0";
                    kd.zusatzdaten[0].pkz[0] = pkz;
                    //map it-data to IT-fields
                    kd.auslausweisGueltig = it.AHBEWILLIGUNGBIS;
                    
                    //it.AUSWEISBEHOERDE;
                    //kd.iban = it.IBAN;
                    //kd.wohnungart = it.WOHNUNGART;

                    kd.zusatzdaten[0].kdtyp = 1;//PRIVAT! auch für firmen, damit hier die daten geschrieben werden
                }else if(kd.syskdtyp==3)//unternehmen
                {
                    if (kd.zusatzdaten[0].ukz == null || kd.zusatzdaten[0].ukz.Length == 0 || kd.zusatzdaten[0].ukz[0].sysangebot != sysangebot)
                        kd.zusatzdaten[0].ukz = new OpenOne.GateBANKNOW.Common.DTO.UkzDto[1];
                    Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto ukz = kd.zusatzdaten[0].ukz[0];
                    if (ukz == null)
                    {
                        ukz = new Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto();
                        kd.zusatzdaten[0].ukz[0] = ukz;
                    }

                    //map it-data to pkz
                    ukz = Mapper.Map<Cic.OpenLease.ServiceAccess.DdOl.ITDto, Cic.OpenOne.GateBANKNOW.Common.DTO.UkzDto>(it, ukz);
                    ukz.sysangebot = sysangebot;
                    kd.zusatzdaten[0].ukz[0] = ukz;
                    //map it-data to IT-fields
                    kd.auslausweisGueltig = it.AHBEWILLIGUNGBIS;
                }




                //Map lookuplist values back to codes:
                // Cic.OpenOne.Common.BO.DictionaryListsBo dicts = new Cic.OpenOne.Common.BO.DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), isoCode);
                // dicts.listByCode(OpenOne.Common.DTO.DDLKPPOSType.BERUFAUSLART);
                Cic.OpenOne.GateBANKNOW.Common.DTO.KundeDto rval = kundeBo.updateKunde(kd);
                using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new OpenOne.Common.Model.DdOl.DdOlExtended())
                {
                    try
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = kd.sysit });
                        parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "flag", Value = orgsmsflag });
                        ctx.ExecuteStoreCommand("update it set infosmsflag=:flag where sysit=:sysit ", parameters2.ToArray());
                    }
                    catch (Exception exs)
                    {
                        _Log.Warn("IT reset infosmsflag failed: " + exs.Message);
                    }
                }


                //WRITE ALL IT-Fields into ITPKZ/ITUKZ:
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysangebot });
                List<Devart.Data.Oracle.OracleParameter> pars2 = new List<Devart.Data.Oracle.OracleParameter>();
                pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                pars2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysangebot });

                String wvh = "dest.WOHNVERHCODE=src.WOHNVERH,";
                if (skipHaushalt)
                    wvh = "";


                String mergepkz = @"merge into itpkz dest
                                using (select * from it where sysit=:sysit) src
                                on (src.sysit=dest.sysit and dest.sysangebot=:sysangebot)
                                when matched then update set
                                dest.ABBEWILLIGUNGBIS=src.ABBEWILLIGUNGBIS,
                                dest.AHBEWILLIGUNGBIS=src.AHBEWILLIGUNGBIS,
                                dest.ANREDECODE=src.ANREDECODE,
                                dest.AUSLAUSWEISCODE=src.AUSLAUSWEISCODE,
                                dest.AUSWEISABLAUF=src.AUSWEISABLAUF,
                                dest.AUSWEISART=src.AUSWEISART,
                                dest.AUSWEISBEHOERDE=src.AUSWEISBEHOERDE,
                                dest.AUSWEISDATUM=src.AUSWEISDATUM,
                                dest.AUSWEISNR=src.AUSWEISNR,
                                dest.BANKNAME=src.BANKNAME,
                                dest.BERUF=src.BERUF,
                                dest.BESCHBISAG=src.BESCHBISAG,
                                dest.BESCHSEITAG=src.BESCHSEITAG,
                                dest.EINKNETTO=src.EINKNETTO,
                                dest.EMAIL=src.EMAIL,
                                dest.FAMILIENSTAND=src.FAMILIENSTAND,
                                dest.FAX=src.FAX,
                                dest.GRUENDUNG=src.GRUENDUNG,
                                dest.GEBDATUM=src.GEBDATUM,
                                dest.GEBORT=src.GEBORT,
                                dest.HANDY=src.HANDY,
                                dest.HSNR=src.HSNR,
                                dest.HSNR2=src.HSNR2,
                                dest.HSNRAG=src.HSNRAG,
                                dest.IBAN=src.IBAN,
                                dest.INFOMAILFLAG=src.INFOMAILFLAG,
                                dest.INFOSMSFLAG=src.INFOSMSFLAG,
                                dest.INFOTELFLAG=src.INFOTELFLAG,
                                --dest.KDIDENTFLAG=src.KDIDENTFLAG,
                                dest.KINDERIMHAUS=src.KINDERIMHAUS,
                                dest.KREDRATE1=src.KREDRATE1,
                                dest.LEGITABNEHMER=src.LEGITABNEHMER,
                                dest.LEGITDATUM=src.LEGITDATUM,
                                dest.MIETE=src.MIETE,
                                dest.NAME=src.NAME,
                                dest.NAMEAG=src.NAMEAG,
                                dest.NEBEINKNETTO=src.NEBENEINKNETTO,
                                dest.ORT=src.ORT,
                                dest.ORT2=src.ORT2,
                                dest.ORTAG=src.ORTAG,
                                dest.PLZ=src.PLZ,
                                dest.PLZ2=src.PLZ2,
                                dest.PLZAG=src.PLZAG,
                                dest.PTELEFON=src.PTELEFON,
                                dest.SCHUFAFLAG=src.SCHUFAFLAG,
                                dest.STRASSE=src.STRASSE,
                                dest.STRASSE2=src.STRASSE2,
                                dest.STRASSEAG=src.STRASSEAG,
                                dest.SYSBRANCHE=src.SYSBRANCHE,
                                dest.SYSLAND=src.SYSLAND,
                                dest.SYSLAND2=src.SYSLAND2,
                                dest.SYSLANDAG=src.SYSLANDAG,
                                dest.SYSLANDNAT=src.SYSLANDNAT,
                                dest.TELEFON=src.TELEFON,
                                dest.TITELCODE=src.TITELCODE,
                                dest.VORNAME=src.VORNAME,
                                dest.WERBECODE=src.WERBECODE,
                                dest.WOHNSEIT=src.WOHNSEIT,
                                dest.WOHNART=src.WOHNUNGART,"+wvh+@"dest.ZEINKNETTO=src.ZEINKNETTO,
								dest.PREVNAME=src.PREVNAME,
								dest.IDENTEG=src.IDENTEG,
                                dest.SYSKDTYP=src.SYSKDTYP,dest.SYSPERSON=src.SYSPERSON,dest.URL=src.URL,dest.ZUSATZ=src.ZUSATZ";
                String mergeukz = @"merge into itukz dest
                                using (select * from it where sysit=:sysit) src
                                on (src.sysit=dest.sysit and dest.sysangebot=:sysangebot)
                                when matched then update set
                                dest.ANREDECODE=src.ANREDECODE,
                                dest.BANKNAME=src.BANKNAME,
                                dest.EMAIL=src.EMAIL,
                                dest.FAX=src.FAX,
                                dest.GRUENDUNG=src.GRUENDUNG,
                                dest.HANDY=src.HANDY,
                                dest.HREGISTER=src.HREGISTER,
                                dest.HSNR=src.HSNR,
                                dest.IBAN=src.IBAN,
                                dest.INFOMAILFLAG=src.INFOMAILFLAG,
                                dest.INFOSMSFLAG=src.INFOSMSFLAG,
                                dest.INFOTELFLAG=src.INFOTELFLAG,
                                --dest.KDIDENTFLAG=src.KDIDENTFLAG,
                                dest.LEGITABNEHMER=src.LEGITABNEHMER,
                                dest.LEGITDATUM=src.LEGITDATUM,
                                dest.NAME=src.NAME,
                                dest.NAMEKONT=src.NAMEKONT,
                                dest.ORT=src.ORT,
                                dest.PLZ=src.PLZ,
                                dest.PTELEFON=src.PTELEFON,
                                dest.RECHTSFORM=src.RECHTSFORM,
                                dest.RECHTSFORMCODE=src.RECHTSFORMCODE,
                                dest.HREGISTERORT=src.HREGISTERORT,
                                dest.HREGISTERART =src.HREGISTERART ,
                                dest.STRASSE=src.STRASSE,
                                dest.SYSBRANCHE=src.SYSBRANCHE,
                                dest.SYSLAND=src.SYSLAND,
                                dest.SYSLANDNAT=src.SYSLANDNAT,
                                dest.TELEFON=src.TELEFON,
                                dest.TITELCODE=src.TITELCODE,
                                dest.WERBECODE=src.WERBECODE,
                                dest.ZUSATZ=src.ZUSATZ,
                                dest.IDENTUST=src.IDENTUST,
                                dest.SYSKDTYP=src.SYSKDTYP,dest.SYSPERSON=src.SYSPERSON,dest.URL=src.URL";

                String mergecompliance = @"merge into compliance dest
                                using (select * from compliance where area='IT' and sysid=:sysit and flagaktiv=1) src
                                on (dest.sysid=:sysitpkz2 and dest.area='ITPKZ')
                                when matched then update set
                                dest.BEZEICHNUNG=src.BEZEICHNUNG,
                                dest.FLAGAKTIV=src.FLAGAKTIV,
                                dest.BEGINN=src.BEGINN,
                                dest.ENDE=src.ENDE,
                                dest.SYSLAND=src.SYSLAND,
                                dest.SYSCOMPLIANCETYPE=src.SYSCOMPLIANCETYPE
                                when not matched then
                                insert(bezeichnung,flagaktiv,beginn,ende,sysland,syscompliancetype,area,sysid) values(src.BEZEICHNUNG,src.FLAGAKTIV,src.BEGINN,src.ENDE,src.SYSLAND,src.SYSCOMPLIANCETYPE,'ITPKZ',:sysitpkz)";
                using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                {
                    ctx.ExecuteStoreCommand(mergepkz, pars.ToArray());
                    ctx.ExecuteStoreCommand(mergeukz, pars2.ToArray());
                    long sysitpkz = ctx.ExecuteStoreQuery<long>("select sysitpkz from itpkz where sysangebot=" + sysangebot + " and sysit=" + sysit).FirstOrDefault();
                    List<Devart.Data.Oracle.OracleParameter> pars3 = new List<Devart.Data.Oracle.OracleParameter>();
                    pars3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    pars3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysitpkz", Value = sysitpkz });
                    pars3.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysitpkz2", Value = sysitpkz });
                    if (sysitpkz > 0)
                    {
                        ctx.ExecuteStoreCommand(mergecompliance, pars3.ToArray());
                    }
                }

                return kundeBo.getKunde(sysit);
            }
            catch (Exception e)
            {
                _Log.Error("Updating of IT-Structure for " + sysit + " failed", e);
                throw e;
            }
        }
    }
}