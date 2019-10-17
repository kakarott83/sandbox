using System;
using System.Collections.Generic;
using System.Linq;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using AutoMapper;

using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Util.Security;
using System.Text.RegularExpressions;
using Cic.One.Web.BO.Search;
using System.Data.Common;
using System.Data.EntityClient;
using Devart.Data.Oracle;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.Util;
using Cic.One.GateBANKNOW.BO;
using Cic.One.DTO.BN;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO;
using System.Globalization;
using Cic.OpenOne.Common.Model.Prisma;
using Dapper;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Model.DdCt;

namespace Cic.One.GateBANKNOW.DAO
{
    using OpenOne.GateBANKNOW.Common.BO;

    /// <summary>
    /// Allows modifications to the standard get/createorupdate of Entities by overriding base methods
    /// </summary>
    public class EntityDao : Cic.One.Web.DAO.EntityDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static String QUERYOB = "select * from cic.ob where sysvt = :sysvt";
        //private static String QUERYANTOB = "select * from cic.antob where sysvt = :sysantrag";
        //private static String QUERYVPFILADDANTRAG = "select * from cic.vpfiladd where cic.vpfiladd.sysvpfiladd in (Select cic.antrag.sysvpfil from cic.antrag where cic.antrag.sysid = :sysid)";
        //private static String QUERYOBBRIEF = "select * from obbrief where sysobbrief=:sysobbrief";
        private static String QUERYVPFILADDVERTRAG = "select * from cic.vpfiladd where cic.vpfiladd.sysvpfiladd in (Select cic.vt.sysvpfil from cic.vt where cic.vt.sysid = :sysid)";
        private static String QUERYVTDEPOT = "Select depot from kalk where sysob = :sysob order by syskalk desc";
        private static String QUERYOBINI = "select * from obini where sysobini=:sysobini";
        private static String QUERYVTOBSL= "select cic.vtobsl.sysid, cic.vtobsl.sysvt, cic.vtobsl.folgerate, cic.vtobsl.ppy, cic.vtobsl.rang, cic.vtobsl.bezeichnung, cic.vtobsl.faellig from cic.vtobsl where cic.vtobsl.sysvt = :sysvt and vtobsl.inaktiv =0 and vtobsl.flagopt2=2 and vtobsl.syssltyp=958  order by 1 desc ";
        private static String QUERYVTOBSLRATE  = "SELECT vtobslpos.betrag, vtobslpos.valuta "+
                                                 "FROM vtobslpos, vtobsl " +
                                                 "WHERE vtobslpos.sysvtobsl = vtobsl.sysid AND vtobsl.inaktiv = 0 AND vtobsl.rang IN(500,520,540,550) AND vtobsl.sysvt = :psysvt ORDER BY vtobslpos.valuta";
       
        private static String QUERYVTOBSLPOS = "select cic.vtobslpos.sysid, cic.vtobslpos.sysvtobsl, cic.vtobslpos.rang, cic.vtobslpos.anzahl, cic.vtobslpos.betrag, VTOBSLPOS.VALUTA from cic.vtobslpos where cic.vtobslpos.sysvtobsl = :sysvtobsl order by rang";
        private static String QUERYINTSBAND = "SELECT intsband.lowerb, intsband.intrate, intsband.addrate, intsband.redrate, intsband.minrate, intsband.maxrate  "+
                                               "FROM intsband  " +
                                               "WHERE intsband.sysintsdate = (SELECT min(intsdate.sysintsdate) FROM intsdate WHERE intsdate.sysintstrct = " +
                                               "(SELECT intstrct.sysintstrct FROM intstrct, prproduct WHERE prproduct.sysprproduct=:SysPROD AND prproduct.sysintstrct = intstrct.sysintstrct)  and intsdate.validfrom<=sysdate )";

        private const String QUERY_EPOSRAUM = @"select  wftzvar.syswftzvar
                                        from wftzust, wftzvar, perole, wfzust
                                        where perole.sysperson = wftzust.syslease
                                        and wftzust.syswftable = 2
                                        and wfzust.syscode = 'EPOS_BEDINGUNGEN'
                                        and wfzust.syswfzust = wftzust.syswfzust
                                        and wftzvar.syswftzust = wftzust.syswftzust
                                        and wftzust.status = 0
                                        and wftzvar.code=:code
                                        and perole.sysperole = :sysperole
                                        order by decode (wftzvar.code, 'BESTAETIGT',1, 'BESTAETIGT_AM',2,3)";
        private const String QUERY_EPOSRAUMZUST = @"select  wftzust.syswftzust
                                        from wftzust, wftzvar, perole, wfzust
                                        where perole.sysperson = wftzust.syslease
                                        and wftzust.syswftable = 2
                                        and wfzust.syscode = 'EPOS_BEDINGUNGEN'
                                        and wfzust.syswfzust = wftzust.syswfzust
                                        and wftzvar.syswftzust = wftzust.syswftzust
                                        and wftzust.status = 0
                                        and perole.sysperole = :sysperole
                                        order by decode (wftzvar.code, 'BESTAETIGT',1, 'BESTAETIGT_AM',2,3)";

		/// <summary>
        /// Returns the syscamptp for ANGEBOT with sysid
        /// </summary>
        /// <param name="syscamp"></param>
        /// <returns></returns>
        public long getSysCampTp(long sysid)
        {
			if (sysid == 0)
				return 0;
            
            using (PrismaExtended ctx = new PrismaExtended())
            {
				return ctx.ExecuteStoreQuery<long> ("select syscamptp from ANGEBOT where sysid=" + sysid, null).FirstOrDefault ();
            }
        }

        /// <summary>
        /// Returns all active versicherungen for the antrag
        /// </summary>
        /// <param name="sysant"></param>
        /// <returns></returns>
        public List<Cic.OpenOne.Common.DTO.AngAntVsDto> getAntragVersicherung(long sysant)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                Cic.OpenOne.Common.DAO.ITranslateDao tdao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                List<Cic.OpenOne.Common.DTO.CTLUT_Data> translations = tdao.readoutTranslationList("'VSTYP', 'RSVTYP', 'FSTYP'", isoCode);
                List<Cic.OpenOne.Common.DTO.AngAntVsDto> rval = ctx.ExecuteStoreQuery<Cic.OpenOne.Common.DTO.AngAntVsDto>("select antvs.*,vstyp.BEZEICHNUNG vsTypBezeichnung,person.name vsBezeichnung from antvs, vstyp, vsart, person where person.sysperson(+)=antvs.sysvs and antvs.sysvstyp = vstyp.sysvstyp and vstyp.sysvsart = vsart.sysvsart and antvs.sysantrag =" + sysant).ToList();
                foreach (AngAntVsDto vs in rval)
                {
                    Cic.OpenOne.Common.DTO.CTLUT_Data Translation = tdao.RetrieveEntry(vs.sysvstyp, "VSTYP", translations);
                    if (Translation != null)
                    {
                        vs.vsTypBezeichnung = Translation.Bezeichnung;
                        //item.code = Translation.Name;
                        //item.beschreibung = Translation.Description;
                    }
                }
                return rval;
            }
        }

        /// <summary>
        /// retruns the interest rate band for the product
        /// </summary>
        /// <param name="sysPROD"></param>
        /// <returns></returns>
        public IntsbandDto getIntsband(long sysPROD)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {

                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                IntsbandDto rval = con.Query<IntsbandDto>(QUERYINTSBAND, new { SysPROD = sysPROD }).FirstOrDefault();
                return rval;
            }
        }

        /// <summary>
        /// Returns all active versicherungen for the vt
        /// </summary>
        /// <param name="sysant"></param>
        /// <returns></returns>
        public List<Cic.OpenOne.Common.DTO.AngAntVsDto> getVTVersicherung(long sysvt)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                Cic.OpenOne.Common.DAO.ITranslateDao tdao =Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                List<Cic.OpenOne.Common.DTO.CTLUT_Data> translations = tdao.readoutTranslationList("'VSTYP', 'RSVTYP', 'FSTYP'", isoCode);
                List<Cic.OpenOne.Common.DTO.AngAntVsDto> rval= ctx.ExecuteStoreQuery<Cic.OpenOne.Common.DTO.AngAntVsDto>("select vtvs.*,vstyp.BEZEICHNUNG vsTypBezeichnung from vtvs, vstyp, vsart where vtvs.sysvstyp = vstyp.sysvstyp and vstyp.sysvsart = vsart.sysvsart and vtvs.sysvt =" + sysvt).ToList();
                foreach(AngAntVsDto vs in rval)
                {
                    Cic.OpenOne.Common.DTO.CTLUT_Data Translation = tdao.RetrieveEntry(vs.sysvstyp, "VSTYP", translations);
                    if (Translation != null)
                    {
                        vs.vsTypBezeichnung= Translation.Bezeichnung;
                        //item.code = Translation.Name;
                        //item.beschreibung = Translation.Description;
                    }
                }
                return rval;
            }
        }

        /// <summary>
        /// Returns zustand for sysobart
        /// </summary>
        /// <param name="sysobart"></param>
        /// <returns></returns>
        public String getObjektzustand(long sysobart)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String result = ctx.ExecuteStoreQuery<String>("select description from obart where sysobart=" + sysobart).FirstOrDefault();
                return result;
            }
        }
        /// <summary>
        /// Returns Nutzungsart for sysobusetype
        /// </summary>
        /// <param name="sysobusetype"></param>
        /// <returns></returns>
        public String getObjektnutzungsart(long sysobusetype)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String result = ctx.ExecuteStoreQuery<String>("select name from obusetype where sysobusetype=" + sysobusetype).FirstOrDefault();
                return result;
            }
        }
        /// <summary>
        /// Returns kampagnencode and eingangskanal as BNAntragDto for sysant
        /// </summary>
        /// <param name="sysant"></param>
        /// <returns></returns>
        public BNAntragDto getKampagneinfo(long sysant)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                BNAntragDto result = new BNAntragDto();
                result = ctx.ExecuteStoreQuery<BNAntragDto>("select camp.name  kampagnencode, camptp.name  eingangskanal from camp, camptp, antrag where antrag.syscamp=camp.syscamp and camptp.syscamptp=camp.syscamptp and antrag.sysid=" + sysant).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// Returns Finanzierungsrt for sysant
        /// </summary>
        /// <param name="sysant"></param>
        /// <returns></returns>
        public String getFinanzierungsart(long sysant)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String result = ctx.ExecuteStoreQuery<String>("select bchannel.name from antrag, bchannel where bchannel.sysbchannel=antrag.sysprchannel and antrag.sysid=" + sysant).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// Returns SLA-flag for sysangant
        /// </summary>
        /// <param name="sysangant"></param>
        /// <returns></returns>
        public String getSla(long sysangant)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String result = ctx.ExecuteStoreQuery<String>(@"
					SELECT 
						CASE WHEN (SLA.STATUS = 'Warnung') THEN 'orange' WHEN (SLA.STATUS = 'Ziel überschritten') THEN 'red' 
							ELSE 'black' END indicatorContent
						FROM SLA
						WHERE ((SLA.SYSANGEBOT = " + sysangant + " OR SLA.SYSANTRAG = " + sysangant + ") AND activeflag = 1)" 
					).FirstOrDefault ();
                return result;
            }
        }


        /// <summary>
        /// Returns QuelleRW (Grundlage) for antob.sysob
        /// </summary>
        /// <param name="sysant"></param>
        /// <returns></returns>
        public String getQuellerw(long sysob)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String result = ctx.ExecuteStoreQuery<String>("select quellerw from antob where sysob=" + sysob).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// Returns Vertrag Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        override public VertragDto getVertragDetails(long sysvertrag)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
               /* VT vertragOutput = (from ant in ctx.VT
                                    where ant.SYSID == sysvertrag
                                    select ant).FirstOrDefault();
                if (vertragOutput == null)
                {
                    throw new ArgumentException("Vertrag does not exist: sysId = " + sysvertrag);
                }
                */

                VertragDto rval = con.Query<VertragDto>("select vt.*,person.name kundename, syskd sysperson from cic.vt,person where person.sysperson=vt.syskd and vt.sysid=:sysid", new { sysid = sysvertrag }).FirstOrDefault();
                    //Mapper.Map<VT, VertragDto>(vertragOutput);
                if (rval.konstellation != null)
                {
                    rval.konstellation = rval.konstellation.Trim();
                }


                VertragDto rval2 = con.Query<VertragDto>("select auftrag.gbetrag zins, ulon04 aufschubfrist, dat13 zinsGarantBis, option8+ option7 ratenpause, mwst.prozent mwst, vart.bezeichnung vart,  wfuser.sysperson sysberater from cic.vt,person,vart,mwst,vtoption,auftrag,wfuser where vt.sysberater = wfuser.syswfuser(+) and  vt.sysid=auftrag.sysvt(+) and auftrag.typ(+)=141 and vt.sysid=vtoption.sysid(+) and vt.sysmwst=mwst.sysmwst(+) and vart.sysvart=vt.sysvart and person.sysperson=vt.syskd and vt.sysid=:sysid", new { sysid = sysvertrag }).FirstOrDefault();
                rval.auftrag = rval2.auftrag;
                rval.aufschubfrist = rval2.aufschubfrist;
                rval.zinsGarantBis = rval2.zinsGarantBis;
                rval.ratenpause = rval2.ratenpause;
                rval.mwst = rval2.mwst;
                rval.vart = rval2.vart;
                rval.auftrag = new AuftragDto();
                rval.auftrag.gbetrag = rval2.zins;//mapped in query to fetch it
                rval.sysBerater = rval2.sysBerater;
               
                //TODO EDMX Mapping
                //rval.gesamt = ctx.ExecuteStoreQuery<double>("SELECT GESAMT from VT where sysid=" + sysvertrag, null).FirstOrDefault();
                //rval.sysVpfil = ctx.ExecuteStoreQuery<long>("SELECT sysVpfil from VT where sysid=" + sysvertrag, null).FirstOrDefault();

               /* if (!vertragOutput.PERSONReference.IsLoaded)
                    vertragOutput.PERSONReference.Load();
                if (vertragOutput.PERSON != null)
                {
                    rval.kundeName = vertragOutput.PERSON.NAME;
                    rval.sysPerson = vertragOutput.PERSON.SYSPERSON;
                }*/

                //if (!vertragOutput.LSADDReference.IsLoaded)
                //    vertragOutput.LSADDReference.Load();
                //rval.ls = Mapper.Map<LSADD, LsaddDto>(vertragOutput.LSADD);
                //rval.waehrung = getWaehrungDetail(vertragOutput.SYSWAEHRUNG);

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = sysvertrag });

                //ANTOB
                List<ObDto> obList = null;

                obList = ctx.ExecuteStoreQuery<ObDto>(QUERYOB, parameters.ToArray()).ToList();
                rval.obList = obList;

                if (obList != null)
                {

                    foreach (ObDto angob in obList)
                    {
                        List<Devart.Data.Oracle.OracleParameter> obpar = new List<Devart.Data.Oracle.OracleParameter>();
                        obpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobini", Value = angob.sysOb });
                        angob.zusatzdaten = ctx.ExecuteStoreQuery<ObIniDto>(QUERYOBINI, obpar.ToArray()).FirstOrDefault();

                    }

                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysob", Value = sysvertrag });
                    rval.depot = ctx.ExecuteStoreQuery<double>(QUERYVTDEPOT, parameters.ToArray()).FirstOrDefault();

                }

                //ANTOBSL
                List<VtobslDto> vtobslList = null;
                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = sysvertrag });
                vtobslList = ctx.ExecuteStoreQuery<VtobslDto>(QUERYVTOBSL, parameters.ToArray()).ToList();
                //ANTOBSLPOS
                rval.vtobslList = new List<VtobslDto>();

                foreach (VtobslDto vtobslItem in vtobslList)
                {

                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvtobsl", Value = vtobslItem.sysid });
                    List<VtobslposDto> vtoblsposList = null;
                    List<VtobslposDto> vtoblsposListAll = new List<VtobslposDto>();
                    vtoblsposList = ctx.ExecuteStoreQuery<VtobslposDto>(QUERYVTOBSLPOS, parameters.ToArray()).ToList();
                    foreach (VtobslposDto vtoblspositem in vtoblsposList)
                    {

                        vtoblsposListAll.Add(vtoblspositem);
                    };

                    vtobslItem.vtobslpostList = vtoblsposListAll;

                    rval.vtobslList.Add(vtobslItem);
                }

                //ANTOBSLPOS - RATE
                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysvt", Value = sysvertrag });
                VtobslposDto vtoblsposRate = null;
                vtoblsposRate = ctx.ExecuteStoreQuery<VtobslposDto>(QUERYVTOBSLRATE, parameters.ToArray()).FirstOrDefault();
                if (vtoblsposRate != null) 
                {
                    rval.ursprRateExcl = vtoblsposRate.betrag;
                    double mwstUrs = getMWST(sysvertrag, vtoblsposRate.valuta);
                    rval.ursprRateIncl = Math.Round(rval.ursprRateExcl * (1 + mwstUrs / 100),4);

                }


            
                VpfiladdDto vertriebspartner = getVpfiladdDetail(sysvertrag, QUERYVPFILADDVERTRAG);
                if (vertriebspartner != null)
                    rval.vertriebspartner = vertriebspartner;

                if(rval.sysAntrag>0)
                { 
                    long? pma = (from obsich in ctx.ANTOBSICH
                                 where obsich.SYSANTRAG == rval.sysAntrag && obsich.SYSPERSON > 0 && obsich.SYSPERSON != rval.sysKd
                                  && (obsich.RANG == 120 ||
                                          obsich.RANG == 130 ||
                                          obsich.RANG == 800)
                                 select obsich.SYSPERSON).FirstOrDefault();
                    if (pma != null && pma.HasValue)
                        rval.sysMa = (long)pma.Value;
                }
                
                //Produkt Info
                rval.produkt = getProduktInfoVTDetails(rval.sysID);
                rval.produkt.nameAufKarte = ctx.ExecuteStoreQuery<String>("SELECT card.emboss nameAufKarte FROM card WHERE card.sysvt=" + rval.sysID).FirstOrDefault();

                String queryvttypName = "select name from prproduct where sysprproduct in (select sysprproduct from antrag where sysid in (select sysantrag from vt where sysid = :psysid))";

                
                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysvertrag });
                rval.produkt.productName = ctx.ExecuteStoreQuery<string>(queryvttypName, parameters.ToArray()).FirstOrDefault();

                rval.produktName = rval.produkt.productName;

             



                String MWSTBEGINN = "_MWST:XTD(VT:SysMWST,1,VT:BEGINN)";



                String MWSTENDE= "_MWST:XTD(VT:SysMWST,1,VT:ENDE)";


                


                String RLZ = "_RLZ:STD('VT', VT:SYSID, 'VTOBSL', 0, 1, 'VTOBSL:inaktiv = 0 AND INLIST(VTOBSL:Rang,520,620,640,650,720,740,750,820,840,850,920,940,950)>0', 0, 'VSR:Valuta > VSR:Stand')";

                

                //RLZ = "_DDIFF(NKK:STAND,VT:ENDE)";
                String queryRLZ = "SELECT CASE " +
                                    " WHEN result.months - result.months_r = 0 " +
                                    " THEN result.months_r " +
                                    " ELSE " +
                                    " CASE " +
                                    " WHEN result.months_r - result.months > 0 " +
                                    " THEN result.months_r " +
                                    " ELSE result.months_r + 1 " +
                                    " END " +
                                    " END rlz" +
                                    " FROM (SELECT months_between(vt.ende, nkk.stand) months, " +
                                    " ROUND(months_between(vt.ende, nkk.stand)) months_r " +
                                    " FROM vt, penkonto, nkonto, nkk " +
                                    " WHERE vt.sysid = penkonto.sysvt AND penkonto.rang = 10000 " +
                                    " AND penkonto.sysnkonto = nkonto.sysnkonto " +
                                    " AND nkonto.sysnkonto = nkk.sysnkonto AND nkk.sysnkktyp = :psysnkktyp " +
                                    " AND vt.sysid = :sysvertrag) result ";


                if (rval.sysVart == 7)
                {
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvertrag", Value = sysvertrag });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysnkktyp", Value = 1 });

                    rval.rlz = ctx.ExecuteStoreQuery<int>(queryRLZ, parameters.ToArray()).FirstOrDefault();
                    
                }


                if (rval.sysVart == 9)
                {
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvertrag", Value = sysvertrag });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysnkktyp", Value = 10 });
                    rval.rlz = ctx.ExecuteStoreQuery<int>(queryRLZ, parameters.ToArray()).FirstOrDefault();
                    
                }


                

                ICASBo bo = Cic.One.Web.BO.BOFactoryFactory.getInstance().getCASBo();
                if (bo != null)
                {
                    iCASEvaluateDto ieval = new iCASEvaluateDto();
                    ieval.area = "VT";
                      String GEBUEHR = "_GETGEBUEHR(1, 'GEB_AUSZ_LP', VT:Beginn,0)";
                      if (rval.sysVart != 7 && rval.sysVart != 9)
                        ieval.expression = new String[] { RLZ,GEBUEHR, MWSTBEGINN,MWSTENDE};
                      else
                        ieval.expression = new String[] {GEBUEHR, MWSTBEGINN, MWSTENDE };
                    ieval.sysID = new long[] {rval.sysID };
                    CASEvaluateResult[] er = null;
                   
                    
                    try
                    {
                        er = bo.getEvaluation(ieval, sysWfuser);

                        if (rval.sysVart != 7 && rval.sysVart != 9)
                        { 
                            if (er[0].clarionResult[0] != null && !"".Equals(er[0].clarionResult[0]))
                                rval.rlz = int.Parse(er[0].clarionResult[0], CultureInfo.InvariantCulture);
                            if (er[0].clarionResult[1] != null && !"".Equals(er[0].clarionResult[1]))
                                rval.gebuehr = Double.Parse(er[0].clarionResult[1], CultureInfo.InvariantCulture);
                            if (er[0].clarionResult[2] != null && !"".Equals(er[0].clarionResult[2]))
                                rval.mwstbeginn = Double.Parse(er[0].clarionResult[2], CultureInfo.InvariantCulture);
                            if (er[0].clarionResult[3] != null && !"".Equals(er[0].clarionResult[3]))
                                rval.mwstende = Double.Parse(er[0].clarionResult[3], CultureInfo.InvariantCulture);
                        }
                        else
                        {
                           
                            if (er[0].clarionResult[0] != null && !"".Equals(er[0].clarionResult[0]))
                                rval.gebuehr = Double.Parse(er[0].clarionResult[0], CultureInfo.InvariantCulture);
                            if (er[0].clarionResult[1] != null && !"".Equals(er[0].clarionResult[1]))
                                rval.mwstbeginn = Double.Parse(er[0].clarionResult[1], CultureInfo.InvariantCulture);
                            if (er[0].clarionResult[2] != null && !"".Equals(er[0].clarionResult[2]))
                                rval.mwstende = Double.Parse(er[0].clarionResult[2], CultureInfo.InvariantCulture);
                        }

                        
                    }
                    catch (Exception e)
                    {
                        _log.Warn("CAS-Evaluation failed for VT rlz/gebuehr", e);
                    }

                    rval.beanspruchterKredit = 0;
                    long sysklinie = ctx.ExecuteStoreQuery<long>(@"SELECT nkklinie.sysklinie 
                                        FROM penkonto, nkonto, nkklinie
                                        WHERE penkonto.sysnkonto = nkonto.sysnkonto AND nkonto.sysnkonto = nkklinie.sysnkonto 
                                        AND penkonto.rang = 10000 AND penkonto.sysvt =" + rval.sysID ).FirstOrDefault();
                    if (sysklinie>0)
                    { 
                        ieval.area = "KLINIE";
                        String KLINIE = "_KLINIE('KLINIE', KLINIE:SYSKLINIE, 'UTIL', '', '', 1, 0)";
                        ieval.expression = new String[] { KLINIE };
                        ieval.sysID = new long[] { sysklinie };
                        try
                        {
                            er = bo.getEvaluation(ieval, sysWfuser);
                            if (er[0].clarionResult[0] != null && !"".Equals(er[0].clarionResult[0]))
                                rval.beanspruchterKredit = double.Parse(er[0].clarionResult[0], CultureInfo.InvariantCulture);
                    
                        }
                        catch (Exception e)
                        {
                            _log.Warn("CAS-Evaluation failed for KLINIE", e);
                        }
                    }

                    
                 
                }

                
                rval.beanspruchterKreditProz = rval.beanspruchterKredit*100.0/(double)rval.bgExtern;

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysID });
                rval.gesperrt = ctx.ExecuteStoreQuery<int>("SELECT nkklinie.gesperrt FROM penkonto JOIN nkklinie using (sysnkonto) WHERE penkonto.rang = 10000 AND penkonto.sysvt = :sysvt", parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new OracleParameter { ParameterName = "sysvt", Value = rval.sysID });
                rval.segment = ctx.ExecuteStoreQuery<string>("select name from seg where sysseg in (select sysseg from Person where sysperson in (select syshd from vtobhd where vtobhd.sysvt =:sysvt ))", parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = rval.sysAntrag });
                rval.version = ctx.ExecuteStoreQuery<string>("select wftzvar.value FROM wftzust JOIN wftable using (syswftable) JOIN wftzvar using (syswftzust) WHERE wftzust.syslease=:sysantrag AND wftable.syscode='ANTRAG' AND wftzvar.code='Version Vertrag'", parameters.ToArray()).FirstOrDefault();

                rval.produkt.indikativerRestwertBrutto = Math.Round(rval.produkt.indikativerRestwert * (1 + rval.mwstende / 100.0));
            

                return rval;
            }


        }

       


      
        /// <summary>
        /// Returns Angebot Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
      /*  override public AntragDto getAntragDetails(long sysantrag)
        {
            {
                using (DdCtExtended ctx = new DdCtExtended())
                {

                    ANTRAG antragOutput = (from ant in ctx.ANTRAG
                                           where ant.SYSID == sysantrag
                                           select ant).FirstOrDefault();
                    if (antragOutput == null)
                    {
                        throw new ArgumentException("Antrag does not exist: sysId = " + sysantrag);
                    }

                    AntragDto rval = Mapper.Map<ANTRAG, AntragDto>(antragOutput);
                    rval.ls = getLsaddDetail(antragOutput.SYSLS);
                    rval.waehrung = getWaehrungDetail(antragOutput.SYSWAEHRUNG);

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });

                    //ANTOB

                    rval.antob = ctx.ExecuteStoreQuery<AntobDto>(QUERYANTOB, parameters.ToArray()).FirstOrDefault();

                    

                    // 
                    long? pma = (from obsich in ctx.ANTOBSICH
                                        where obsich.SYSANTRAG == sysantrag && obsich.SYSPERSON > 0 && obsich.SYSPERSON != rval.sysKd
                                         && (obsich.RANG == 120 ||
                                                 obsich.RANG == 130 ||
                                                 obsich.RANG == 800)
                                        select obsich.SYSPERSON).FirstOrDefault();
                    if(pma!=null && pma.HasValue)
                        rval.sysMa = (long)pma.Value;

                    if (rval.sysBerater > 0)
                        rval.sysBerater = ctx.ExecuteStoreQuery<long>("select sysperson from wfuser where syswfuser=" + rval.sysBerater).FirstOrDefault();
                    if (!antragOutput.ADMADDReference.IsLoaded)
                        antragOutput.ADMADDReference.Load();
                    if (antragOutput.ADMADD != null)
                    {
                        AdmaddDto am = new AdmaddDto();
                        am.sysAdmadd = antragOutput.ADMADD.SYSADMADD;
                        am.bezeichnung = antragOutput.ADMADD.BEZEICHNUNG;
                        rval.aussendienstmitarbeiter = am;
                    }
                    VpfiladdDto vertriebspartner = getVpfiladdDetail(sysantrag, QUERYVPFILADDANTRAG);
                    if (vertriebspartner != null)
                    {
                        rval.vertriebspartner = vertriebspartner;
                    }
                    rval.auflagen = getAuflagen(sysantrag, getISOLanguage());
                    rval.regeln = getAuskunftRegeln(sysantrag, getISOLanguage());
                    return rval;
                }
            }

        }*/


        /// <summary>
        /// loads zek info details from db
        /// </summary>
        /// <param name="syszek"></param>
        /// <returns></returns>
        override
        public ZekDto getZek(long syszek)
        {
            if (syszek <= 0)
                return null;

            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter
                {
                    ParameterName = "syszek",
                    Value = syszek
                });
                //TODO: Datenbankabfrage
                //ZekDto rval = ctx.ExecuteStoreQuery<ZekDto>(QUERYZEK, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);

                /*
                ZekBo zekBO = new ZekBo(new ZekDto());
                zekBO.Response = new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekResponseDescriptionDto();
                zekBO.Response.FoundPerson = new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto();
                zekBO.Response.FoundContracts = new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFoundContractsDto();
                switch (syszek)
                {
                    case 17:
                        zekBO.Response.FoundPerson = new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto();
                        zekBO.Response.FoundPerson.FirstName = "Anna";
                        zekBO.Response.FoundPerson.Name = "Musterfrau";
                        zekBO.Response.FoundPerson.Street = "Münchner Straße";
                        zekBO.Response.FoundPerson.KundenId = "17";
                        zekBO.Response.FoundContracts = new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFoundContractsDto();
                        zekBO.Response.FoundContracts.BardarlehenContracts = new List<OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto>();
                        zekBO.Response.FoundContracts.BardarlehenContracts.Add(new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto());
                        zekBO.Response.FoundContracts.BardarlehenContracts[0].anzahlMonatlicheRaten = 9;
                        zekBO.Response.FoundContracts.BardarlehenContracts[0].filiale = 17;
                        zekBO.Response.RefNo = 17;
                        break;
                    case 18:
                        zekBO.Response.FoundPerson = new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekAddressDescriptionDto();
                        zekBO.Response.FoundPerson.FirstName = "Hans";
                        zekBO.Response.FoundPerson.Name = "Mustermann";
                        zekBO.Response.FoundPerson.Street = "Gewerbestraße";
                        zekBO.Response.FoundPerson.KundenId = "18";
                        zekBO.Response.FoundContracts = new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFoundContractsDto();
                        zekBO.Response.FoundContracts.BardarlehenContracts = new List<OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto>();
                        zekBO.Response.FoundContracts.BardarlehenContracts.Add(new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto());
                        zekBO.Response.FoundContracts.BardarlehenContracts[0].anzahlMonatlicheRaten = 12;
                        zekBO.Response.FoundContracts.BardarlehenContracts[0].filiale = 17;
                        zekBO.Response.RefNo = 18;
                        break;
                    case 19:
                        zekBO.Response.FoundContracts = new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekFoundContractsDto();
                        zekBO.Response.FoundContracts.BardarlehenContracts = new List<OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto>();
                        zekBO.Response.FoundContracts.BardarlehenContracts.Add(new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekBardarlehenDescriptionDto());
                        zekBO.Response.FoundContracts.BardarlehenContracts[0].anzahlMonatlicheRaten = 15;
                        zekBO.Response.FoundContracts.BardarlehenContracts[0].filiale = 17;
                        zekBO.Response.FoundContracts.KreditgesuchContracts = new List<OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKreditgesuchDescriptionDto>();
                        zekBO.Response.FoundContracts.KreditgesuchContracts.Add(new OpenOne.GateBANKNOW.Common.DTO.Auskunft.ZekKreditgesuchDescriptionDto());
                        zekBO.Response.FoundContracts.KreditgesuchContracts[0].VertragsStatus = 15;
                        zekBO.Response.FoundContracts.KreditgesuchContracts[0].Herkunft = 17;
                        zekBO.Response.FoundContracts.KreditgesuchContracts[0].Filiale = 16;
                        zekBO.Response.RefNo = 19;
                        break;
                }
                
                zekBO.Response = zekBO.Response;
                return zekBO.ZekData;
                */
                return null;
            }
        }


        /// <summary>
        /// getMWST
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        private double getMWST(long sysvt, DateTime? perDate)
        {


            DateTime nullDate = new DateTime(1800, 1, 1);
            string QUERYMWST = "select  MWST.PROZENT, mwstdate.sysmwstdate,mwstdate.sysmwst, mwstdate.gueltigab, " +
                "mwstdate.prozent ProzentAkt, mwstdate.sysskonto, mwstdate.mwstfibu, mwstdate.evalskonto from vt, mwst, mwstdate where VT.SYSMWST=MWST.SYSMWST and vt.sysmwst=mwstdate.sysmwst and vt.sysid=:sysvt ";

            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = sysvt });

                List<MwStDataDTO> Mehrwertsteuersaetze = context.ExecuteStoreQuery<MwStDataDTO>(QUERYMWST, parameters.ToArray()).ToList();

                return (from data in Mehrwertsteuersaetze
                        where (data.GueltigAb == null || data.GueltigAb <= perDate || data.GueltigAb <= nullDate)
                        orderby data.GueltigAb descending
                        select data.ProzentAkt).FirstOrDefault();
            }
        }

        /// <summary>
        /// update smstext for ANTRAG
        /// </summary>
        /// <param name="input"></param>
        override public bool updateSMSText(iupdateSMSTextDto input)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String from = AppConfig.Instance.GetEntry("SMS", "ABSENDER", "noreply.bank-now@bank-now.ch", "MAILGATEWAY");
                if ("ANGEBOT".Equals(input.area))
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = input.sysid });
                    String handynr = ctx.ExecuteStoreQuery<String>("select handy from it,angebot where angebot.sysit=it.sysit and angebot.sysid=:sysangebot and infosmsflag=1", parameters.ToArray()).FirstOrDefault();
                    if (String.IsNullOrEmpty(handynr)) throw new Exception("Ungültige Mobilnummer beim Kunden oder Elektronische Kommmunikation SMS nicht bestätigt.");
                    sendSMS(handynr, from, input.smsText);
                }
                else if ("ANTRAG".Equals(input.area))
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "sysantrag", Value = input.sysid});
                    String handynr = ctx.ExecuteStoreQuery<String>("select handy from person,antrag where antrag.syskd=person.sysperson and antrag.sysid=:sysantrag and infosmsflag=1", parameters.ToArray()).FirstOrDefault();
                    if (String.IsNullOrEmpty(handynr))
                    {
                        parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = input.sysid });
                        handynr = ctx.ExecuteStoreQuery<String>("select handy from it,antrag where antrag.sysit=it.sysit and antrag.sysid=:sysantrag and infosmsflag=1", parameters.ToArray()).FirstOrDefault();
                        if (String.IsNullOrEmpty(handynr))
                            throw new Exception("Ungültige Mobilnummer beim Kunden oder Elektronische Kommmunikation SMS nicht bestätigt.");
                    }
                    sendSMS(handynr, from, input.smsText);
                }
                else if ("PERSON".Equals(input.area))
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = input.sysid });
                    String handynr = ctx.ExecuteStoreQuery<String>("select handy from person where person.sysperson=:sysperson and infosmsflag=1", parameters.ToArray()).FirstOrDefault();
                    if (String.IsNullOrEmpty(handynr)) throw new Exception("Ungültige Mobilnummer beim Kunden oder Elektronische Kommmunikation SMS nicht bestätigt.");
                    sendSMS(handynr, from, input.smsText);
                }
                else if ("IT".Equals(input.area))
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = input.sysid });
                    String handynr = ctx.ExecuteStoreQuery<String>("select handy from it where it.sysit=:sysit and infosmsflag=1", parameters.ToArray()).FirstOrDefault();
                    if (String.IsNullOrEmpty(handynr)) throw new Exception("Ungültige Mobilnummer beim Kunden oder Elektronische Kommmunikation SMS nicht bestätigt.");
                    sendSMS(handynr, from, input.smsText);
                }
                else return false;

                return true;
            }
        }

        /// <summary>
        /// Send an sms
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool sendSMS(String to, String from, String text)
        {
            INotificationGatewayBo Gateway = BOFactory.getInstance().createNotificationGateway();
            iNotificationGatewayServerDto cfg = Cic.OpenOne.GateBANKNOW.Common.BO.ConfigurationBO.getServerDaten();
            int Code = Gateway.sendSms(to,from, text, cfg);

            if (Code == 2)
            {
                return true;
            }
            else
            {
                switch (Code)
                {
                    case 3:
                        throw (new Exception("Preparing SMS failed!"));
                    case 4:
                        throw (new Exception("Transmission to Server failed!"));
                    default:
                        throw (new Exception("Error occured sending SMS!"));
                }
                
            }
        }

        /// <summary>
        /// accept EPOS Conditions of current user
        /// </summary>
        public override void acceptEPOSConditions()
        {
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended())
            {
                object[] pars0 =
                    {
                        new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = this.sysPerole }
                        
                    };

                long syswftzust = ctx.ExecuteStoreQuery<long>(QUERY_EPOSRAUMZUST, pars0).FirstOrDefault();
                

                object[] pars =
                    {
                        new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = this.sysPerole },
                        new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value ="BESTAETIGT" }
                    };

                long syswftzvar  = ctx.ExecuteStoreQuery<long>(QUERY_EPOSRAUM, pars).FirstOrDefault();
                ctx.ExecuteStoreCommand("update wftzvar set value=1 where syswftzvar=" + syswftzvar);

                object[] pars2 =
                    {
                        new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = this.sysPerole },
                        new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value ="BESTAETIGT_AM" }
                    };

                syswftzvar  = ctx.ExecuteStoreQuery<long>(QUERY_EPOSRAUM, pars2).FirstOrDefault();
                ctx.ExecuteStoreCommand("update wftzvar set value=to_char(sysdate,'dd.mm.yyyy hh24:mi:ss') where syswftzvar=" + syswftzvar);

                ctx.ExecuteStoreCommand("update wftzust set status=1 where syswftzust=" + syswftzust);
                
              
            }
        }

        /// <summary>
        /// Fetches Auskunft Detail Infos
        /// </summary>
        /// <param name="inp"></param>
        /// <param name="rval"></param>
        public void getAuskunftDetail(igetAuskunftDetailDto inp, ogetAuskunftDetailDto rval)
        {
             using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended())
            {
                int syskdtyp = ctx.ExecuteStoreQuery<int>("select syskdtyp from person where sysperson=" + inp.sysperson,null).FirstOrDefault();
                List<String> cmds = new List<string>();
                if(inp.fetchMode == AuskunftDetailMode.EWK||inp.fetchMode==AuskunftDetailMode.ALL)
                {
                    cmds.Add("_SCRIPT('WFScriptIN_dvEWK_"+inp.manr+"AS','N1')");
                }
                if (inp.fetchMode == AuskunftDetailMode.BA || inp.fetchMode == AuskunftDetailMode.ALL)
                {
                    cmds.Add("_SCRIPT('WFScriptIN_dvBA_" + inp.manr + "AS','N1')");
                }
                if (inp.fetchMode == AuskunftDetailMode.GA || inp.fetchMode == AuskunftDetailMode.ALL)
                {
                    cmds.Add("_SCRIPT('WFScriptIN_dvGA_" + inp.manr + "AS','N1')");
                }


                ICASBo bo = Cic.One.Web.BO.BOFactoryFactory.getInstance().getCASBo();

                if (bo != null)
                {
                    foreach (String cmd in cmds)
                    {
                        Cic.OpenOne.Common.DTO.iCASEvaluateDto ieval = new Cic.OpenOne.Common.DTO.iCASEvaluateDto();
                        ieval.area = "ANTRAG";

                        ieval.expression = new String[] { cmd };
                        ieval.sysID = new long[] { inp.sysantrag};
                        Cic.OpenOne.Common.DTO.CASEvaluateResult[] er = null;
                        try
                        {
                            er = bo.getEvaluation(ieval, sysWfuser);
                        }
                        catch (Exception e)
                        {
                            
                        }
                    }
                }



                if (syskdtyp == 1)
                {
                    rval.ewk = ctx.ExecuteStoreQuery<AuskunftDetailDto>(@"SELECT auskunft.anfragedatum, auskunft.anfrageuhrzeit, auskunft.syswfuser, wfuser.vorname, wfuser.name, pkz.creewknummer refnum
                            FROM auskunft, wfuser, pkz WHERE auskunft.syswfuser = wfuser.syswfuser and auskunft.sysauskunft = pkz.creewksysauskunft and auskunft.fehlercode = 0 and pkz.sysperson =" + inp.sysperson + "  and pkz.sysantrag =" + inp.sysantrag, null).FirstOrDefault();
                    rval.ba = ctx.ExecuteStoreQuery<AuskunftDetailDto>(@"SELECT auskunft.anfragedatum, auskunft.anfrageuhrzeit, auskunft.syswfuser, wfuser.vorname, wfuser.name, pkz.crebanummer refnum
                            FROM auskunft, wfuser, pkz WHERE auskunft.syswfuser = wfuser.syswfuser and auskunft.sysauskunft = pkz.crebasysauskunft and auskunft.fehlercode = 0 and pkz.sysperson =" + inp.sysperson + "  and pkz.sysantrag =" + inp.sysantrag, null).FirstOrDefault();
                     rval.ga = ctx.ExecuteStoreQuery<AuskunftDetailDto>(@"SELECT auskunft.anfragedatum, auskunft.anfrageuhrzeit, auskunft.syswfuser, wfuser.vorname, wfuser.name, pkz.creganummer refnum
                            FROM auskunft, wfuser, pkz WHERE auskunft.syswfuser = wfuser.syswfuser and auskunft.sysauskunft = pkz.cregasysauskunft and auskunft.fehlercode = 0 and pkz.sysperson =" + inp.sysperson + "  and pkz.sysantrag =" + inp.sysantrag, null).FirstOrDefault();
                }
                else
                {
                    rval.ewk = ctx.ExecuteStoreQuery<AuskunftDetailDto>(@"SELECT auskunft.anfragedatum, auskunft.anfrageuhrzeit, auskunft.syswfuser, wfuser.vorname, wfuser.name, ukz.creewknummer refnum
                            FROM auskunft, wfuser, ukz WHERE auskunft.syswfuser = wfuser.syswfuser and auskunft.sysauskunft = ukz.creewksysauskunft and auskunft.fehlercode = 0 and ukz.sysperson=" + inp.sysperson + "  and ukz.sysantrag =" + inp.sysantrag, null).FirstOrDefault();
                     rval.ba = ctx.ExecuteStoreQuery<AuskunftDetailDto>(@"SELECT auskunft.anfragedatum, auskunft.anfrageuhrzeit, auskunft.syswfuser, wfuser.vorname, wfuser.name, ukz.crebanummer refnum
                            FROM auskunft, wfuser, ukz WHERE auskunft.syswfuser = wfuser.syswfuser and auskunft.sysauskunft = ukz.crebasysauskunft and auskunft.fehlercode = 0 and ukz.sysperson=" + inp.sysperson + "  and ukz.sysantrag =" + inp.sysantrag, null).FirstOrDefault();
                }

            }
            
        }

    }
}