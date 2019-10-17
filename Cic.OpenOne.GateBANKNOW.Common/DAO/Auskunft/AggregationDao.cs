using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
using CIC.Database.IC.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Aggregation Data Access Object
    /// </summary>
    public class AggregationDao : IAggregationDao
    {
        // Schlechtester ZEK Bonitätscode: aggoutzek.worstbc
        // 06 (schlechteste), 05, 04, 03, 02, 01, 00, 21, 88, 99 (beste)
        private int[] ZEK_Bonitaet = { 06, 05, 04, 61, 71, 03, 02, 01, 00, 21, 88, 99 };

        // Schlechtester ZEK Ablehnungscode: aggoutzek.worstgac
        // 12 (schlechtester), 09, 08, 06, 05, 04, 99, 07, 14, 13, 10 (bester)
        private int[] ZEK_Ablehnungscode = { 12, 09, 08, 06, 05, 04, 99, 07, 14, 13, 10 };

        private AntragstellerInfo ASInfo = new AntragstellerInfo();
        private class AntragstellerInfo
        {
            public AntragstellerInfo() { }

            public long sysPerson { get; set; }
            public long sysPersonAS1 { get; set; }
            public long sysPersonAS2 { get; set; }

        }

        const String ANTRAG_OLATSTAT_ABGESCHLOSSEN = "abgeschlossen";
        const String ANTRAG_OLATSTAT_ANULLIERT = "anulliert";

        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GetOLDatenBySysAntrag
        /// </summary>
        /// <param name="auskunftId"></param>
        /// <param name="sysAntragId"></param>
        /// <returns></returns>
        public AggregationOLOutDto GetOLDatenBySysAntrag(long auskunftId, long sysAntragId)
        {
            try
            {
                AggregationOLOutDto outDto = new AggregationOLOutDto();

                using (DdOlExtended context = new DdOlExtended())
                {
                    String queryString = String.Empty;

                    // Doublette für den aktuellen AS holen
                    String kdDoubletten = MyGetAggregationOLDoublettenForAS(context, sysAntragId, ASInfo.sysPerson);

                    // ------- Die Felder ---------
                    // Anzahl KundenIDs
                    queryString = "select nvl(count(sysperson),0) AS ANZKD from person where sysperson in ( :pDoubletten )";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL1"), outDto);

                    // Maximaler Badlisteintrag
                    queryString = "select nvl(max(ausschluss),0) AS MAXBADLIST from person where sysperson in ( :pDoubletten )";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL2"), outDto);

                    // Maximale aktuelle Risikoklasse 
                    queryString = "select nvl(max(sysrisikokl),0) AS MAXCURRISIKOKL from person where sysperson in ( :pDoubletten )";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL3"), outDto);

                    // Maximale Risikoklasse 
                    queryString = "select nvl(max(sysrisikoklneu),0) AS MAXRISIKOKL from rklhist where sysperson in ( :pDoubletten )";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL4"), outDto);

                    // Minimales Datum Kunde seit
                    queryString = "SELECT MIN(person.KOOPVON) AS MINKUNDESEIT from person where sysperson in ( :pDoubletten )";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL5"), outDto);

                    // Anzahl Anträge
                    // Anzahl Mehrfachantraege
                    queryString = "select nvl(count(sysid),0) AS ANZAT, nvl(sum(decode(ZUSTAND,'Abgeschlossen',0,1)),0) AS ANZMAT " +
                                    " from antrag where syskd in ( :pDoubletten ) and sysid <> :Param0 ";
                    queryString = MyBuildQueryWithDoubletten(queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL6");
                    Mapper.Map(MyCalcFeldBySQL<AggregationOLOutDto>(context, queryString, sysAntragId), outDto);


                    // Anzahl Verzichte in den letzten 12 Monaten
                    // Anzahl Annulierungen in den letzten 12 Monaten
                    queryString = "select nvl(sum(decode(Attribut,'Verzichtet',1,0)),0) AS ANZVZL12, nvl(sum(decode(Attribut,'Gelöscht',1,0)),0) AS ANZANL12 " +
                                    " from antrag where syskd in ( :pDoubletten ) and months_between(sysdate,endeam) < 12";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL7"), outDto);


                    // Eventual Engagement
                    queryString = "select nvl(sum(bginternbrutto),0) AS EVTLENGAGEMENT " +
                                    " from antkalk, antrag where antkalk.sysantrag=antrag.sysid and antrag.sysid in ( " +
                                               "select sysid from antrag where zustand <> 'Abgeschlossen' and syskd in  ( :pDoubletten )) " +
                                    " and antrag.sysid <> :Param0 ";
                    queryString = MyBuildQueryWithDoubletten(queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL8");
                    Mapper.Map(MyCalcFeldBySQL<AggregationOLOutDto>(context, queryString, sysAntragId), outDto);


                    //BNRDR-2072 Im Gesamtengagement muss die Anzahlung von Betrag abgezogen werden
                    // Gesamt Engagement
                    queryString = "select round(DECODE(ENGAGEMENT,null,0,ENGAGEMENT)+DECODE(FINBETRAG,null,0,FINBETRAG)-DECODE(ABLÖSE,null,0,ABLÖSE),0)-DECODE(ANZ,NULL,0,ANZ) AS GESAMTENGAGEMENT " +
                                " from (select nvl(sum(antkalk.bginternbrutto),0) FINBETRAG , nvl(SUM(DECODE(antabl.sysabltyp,1,betrag,0)),0) ABLÖSE, " +
                                " nvl(sum(antkalk.szbrutto),0) ANZ " +
                                " from antabl,antrag,antkalk where antrag.sysid = antabl.sysantrag and antrag.sysid=antkalk.sysantrag and antrag.sysid = :Param0 ), " +
                                " (SELECT nvl(sum(fivkznm.sbsaldos - fivkznm.sbsaldoh),0) ENGAGEMENT " +
                                " FROM penkonto, nkonto, fivkznm, vt " +
                                " WHERE fivkznm.sysskonto = nkonto.sysnkonto AND nkonto.sysnkonto = penkonto.sysnkonto AND penkonto.sysvt = vt.sysid AND penkonto.rang = 10000 " +
                                " AND fivkznm.sysperiod = EXTRACT (YEAR FROM sysdate) - 2005 AND fivkznm.periode = EXTRACT (MONTH FROM sysdate) AND (fivkznm.sbsaldos - fivkznm.sbsaldoh <> 0) " +
                                " AND vt.syskd in ( :pDoubletten )) ";

                    queryString = MyBuildQueryWithDoubletten(queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL9");
                    Mapper.Map(MyCalcFeldBySQL<AggregationOLOutDto>(context, queryString, sysAntragId), outDto);


                    // Letzte AntragstellerInfo -----------------------
                    // LETZTE (PKZ)  (2.AS nicht dabei)
                    // Ticket#2012091810000114 : SQL-Anpassung
                    queryString = "select trim(to_char(pkz.familienstand)) AS LETZTERZIVILSTAND, trim(to_char(pkz.wohnverhcode)) AS LETZTESWV, " +
                                    " trim(to_char(pkz.beruflichcode)) AS LETZTESAV, pkz.miete AS LETZTEMIETE, " +
                                    " case when pkz.einknettoflag=1 then pkz.einknetto ELSE pkz.einkbrutto end LETZTESHE, " +
                                    " case when pkz.einknettoflag=1 then pkz.nebeinknetto ELSE pkz.nebeinkbrutto end LETZTESNE, " +
                                    " case when pkz.einknettoflag=1 then pkz.zeinknetto ELSE pkz.zeinkbrutto end LETZTESZE, " +
                                    " case when pkz.einknettoflag=1 then pkz.jbonusnetto ELSE pkz.jbonusbrutto end LETZTERBONUS, " +
                                    " (select land.iso from person, land where land.sysland = person.syslandnat and person.sysperson = pkz.sysperson) AS LETZTENAT " +
                                    " from pkz " +
                                    " where sysperson in ( :pDoubletten ) " +
                                            " and sysantrag = (select sysid from " +
                                                                " (select sysid from antrag " +
                                                                        " where syskd in (:pDoubletten) " +
                                                                        " and attribut = 'Vertrag aktiviert' " +
                                                                        " order by adatum desc) " +
                                                               " where rownum < 2) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL10"), outDto);


                    // Letzte Risikoklasse
                    // Ticket#2012091810000114 : SQL-Anpassung
                    queryString = " select nvl(max(dedetail.risikoklasseid),0) AS LETZTERISIKOKL from auskunft, deoutexec, dedetail, antrag " +
                                    " where auskunft.sysauskunft = deoutexec.sysauskunft AND auskunft.area = 'ANTRAG' " +
                                    " and dedetail.sysdeoutexec = deoutexec.sysdeoutexec and auskunft.sysid = antrag.sysid " +
                                    " and auskunft.sysauskunfttyp = 3 and auskunft.statusnum = 0 and antrag.syskd in ( :pDoubletten ) " +
                                    " and antrag.sysid = " +
                                            " (SELECT MAX(sysid) FROM antrag " +
                                                " WHERE antrag.syskd IN ( :pDoubletten ) " +
                                                " and attribut = 'Vertrag aktiviert' " +
                                                " AND adatum = " +
                                                "   (SELECT MAX(antrag.adatum) " +
                                                "   FROM antrag " +
                                                "   WHERE antrag.syskd IN ( :pDoubletten ) ) )";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL11"), outDto);


                    // Engagement
                    queryString = " SELECT nvl(sum(fivkznm.sbsaldos - fivkznm.sbsaldoh),0) AS ENGAGEMENT " +
                                    " FROM penkonto, nkonto, fivkznm, vt " +
                                    " WHERE fivkznm.sysskonto = nkonto.sysnkonto " +
                                    " AND nkonto.sysnkonto = penkonto.sysnkonto " +
                                    " AND penkonto.sysvt = vt.sysid " +
                                    " AND penkonto.rang = 10000 " +
                                    " AND fivkznm.sysperiod = EXTRACT (YEAR FROM sysdate) - 2005 " +
                                    " AND fivkznm.periode = EXTRACT (MONTH FROM sysdate) " +
                                    " AND (fivkznm.sbsaldos - fivkznm.sbsaldoh <> 0) " +
                                    " AND vt.syskd in ( :pDoubletten ) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL12"), outDto);


                    // Anzahl Vertraege
                    // Anzahl laufende Vertraege
                    // Anzahl Vertraege im Recovery
                    queryString = " select nvl(count(sysid),0) ANZVT, nvl(count(case when endekz = 0 then sysid end),0) AS ANZVTL, " +
                                  " count(case when zustand = 'aktiv Recovery' then sysid end) AS ANZVTR " +
                                  " from vt where syskd in ( :pDoubletten ) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL13"), outDto);

                    //
                    // Anzahl Mahnungen1, 2, 3
                    queryString = " SELECT sum(vtmahn.MZAEHLER1) AS ANZMAHN1, " +
                                  " case when sum(vtmahn.MZAEHLER1)>0 " +
                                  " then max(MDATUM1) " +
                                  " End AS DATEMAHN1, " +
                                  " sum(vtmahn.MZAEHLER2) AS ANZMAHN2, " +
                                  " case when sum(vtmahn.MZAEHLER2)>0 "  +
                                  " then max(MDATUM2) " +
                                  " End AS DATEMAHN2, " +
                                  " sum(vtmahn.MZAEHLER3) AS ANZMAHN3, " +
                                  " case when sum(vtmahn.MZAEHLER3)>0 " +
                                  " then max(MDATUM3) " +
                                  " End AS DATEMAHN3 " +
                                  " FROM vt, vtmahn WHERE vtmahn.SYSVTMAHN = vt.SYSID and vt.syskd  in   ( :pDoubletten ) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL14"), outDto);
                   

                    // Maximale Mahnstufe
                    queryString = " SELECT case when nvl(sum(vtmahn.MZAEHLER3),0) > 0 then '3' " +
                                    " when nvl(sum(vtmahn.MZAEHLER3),0) = 0 and nvl(sum(vtmahn.MZAEHLER2),0) > 0 then '2' " +
                                    " when nvl(sum(vtmahn.MZAEHLER3),0) = 0 and nvl(sum(vtmahn.MZAEHLER2),0) = 0 and nvl(sum(vtmahn.MZAEHLER1),0) > 0 then '1' " +
                                    " else '0' end AS MAXMAHNSTUFE " +
                                    " FROM vt, vtmahn WHERE vtmahn.SYSVTMAHN = vt.SYSID and vt.syskd in ( :pDoubletten ) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL15"), outDto); 


                   // Anzahl Zahlungsvereinbarungen
                    queryString = " select count(vtobsl.sysid) AS ANZZVB, max(srdate) AS DATEZVB from vt,vtobsl where vtobsl.sysvt = vt.sysid and vtobsl.flagzvb = 1 and vt.syskd in ( :pDoubletten ) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL16"), outDto);

                    // Anzahl Stundungen
                    queryString = "select nvl(sum(vtoption.int01),0) AS ANZSTUNDUNGEN from vtoption,vt where vtoption.sysid=vt.sysid AND vt.syskd in ( :pDoubletten ) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL17"), outDto);


                    // Anzahl OP, Summe OP
                    queryString = " select nvl(sum(rn.gbetrag+rn.gsteuer-rn.teilzahlung),0) AS SUMOP, nvl(count(rn.sysid),0) AS ANZOP " +
                                    " from rn, vt " +
                                    " where rn.sysvt = vt.sysid " +
                                    " and rn.sysperson = vt.syskd " +
                                    " and rn.art = 0 " +
                                    " and rn.valutadatum < trunc(sysdate) " +
                                    " and rn.bezahlt = 0 " +
                                    " and rn.stornokz = 0 " +
                                    " and rn.gbetrag+rn.gsteuer-rn.teilzahlung > 0 " +
                                    " and rn.kreis in (201,10200,20200,30200,3000,6000,7000) " +
                                    " and not (rn.mahnsperre = 1 and rn.zahlsperre = 1 " +
                                    " and rn.msperrebis >= trunc(sysdate) " +
                                    " and rn.msperrevon <= trunc(sysdate)) " +
                                    " and rn.sysperson in ( :pDoubletten ) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL18"), outDto);


                    // Effektive Kundenbeziehung und Dauer Kundenbeziehun
                    queryString =
                    " with " +
                    "     AT as  " +
                    "         (select antrag, syskd, sysid from antrag where sysid = :Param0) " +
                    "     , Tage As   " +
                    "         (select trunc (to_date('01.01.1990','dd.mm.yyyy'),'Month') +(rownum - 1) As Datum from dual " +
                    "         connect by trunc (to_date('01.01.1990','dd.mm.yyyy'),'month') + (rownum - 1) <= last_day(sysdate))  " +
                    "     , Monate As " +
                    "         (select 1, trunc(datum,'month') as Monat from Tage group by  trunc(datum,'month') order by  trunc(datum,'month')) " +
                    "     , VorVerträge as  " +
                    "         (select vt.syskd, vt.sysid, vt.beginn, 1, endeam " +
                    "                 from vt, AT " +
                    "                 where vt.syskd in ( :pDoubletten ) " +
                    "                 and vt.beginn > to_date('01.01.1990','DD.MM.YY') " +
                    "                 order by sysid) " +
                    "     , Aktiv as " +
                    "         (select Monate.Monat " +
                    "         , (case when VorVerträge.beginn <= Monate.monat and decode(endeam,to_date('01.01.0111','DD.MM.YY'),sysdate,endeam) >= Monate.Monat Then 1 else 0 end) VT_aktiv " +
                    "         from Monate, VorVerträge " +
                    "         where 1 = 1 " +
                    "         order by monat desc) " +
                    "     , EffBez AS " +
                    "         (select Aktiv.Monat, max(Aktiv.VT_aktiv) laufend from Aktiv group by Aktiv.Monat order by Monat) " +
                    "     , KdBez AS " +
                    "         (select Aktiv.Monat, sum(Aktiv.VT_aktiv) laufend from Aktiv group by Aktiv.Monat order by Monat) " +
                    " select sum(EffBez.laufend) AS EFFKUNDENBEZIEHUNG, sum(KdBez.laufend) AS DAUERKUNDENBEZIEHUN from EffBez, AT, KdBez where EffBez.Monat = KdBez.Monat group by antrag ";

                    queryString = MyBuildQueryWithDoubletten(queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL19");
                    Mapper.Map(MyCalcFeldBySQL<AggregationOLOutDto>(context, queryString, sysAntragId), outDto);

                    // •	Anzahl Stundung BNRSIEBEN
                    queryString = " select " +
                                  " max(vtoption.dat14) AS DATESTUNDUNGEN " +
                                  " from vtoption, vt " +
                                  " where vtoption.sysid=vt.sysid " +
                                  " and vt.syskd in (:pDoubletten) ";
                    

                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL21"), outDto);


                    //Anzahl der Stops aus allen laufenden Verträge
                    //Maximale Datum der Stops

                    //BNRACHT-424
                    queryString = " select "+
                                  " case when cic.CIC_SYS.TO_CLADATE(max(vt.aufstockstopbis))>0 "+
                                  " then to_char(max(vt.aufstockstopbis),'dd.MM.yyyy') "+
                                  " else null  END DATEAUFSTOCKSTOP, "+
                                  " sum(decode(vt.aufstockstop,1,1,0)) ANZAUFSTOCKSTOP " +
                                  " from vt where endekz=0 and syskd in ( :pDoubletten ) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL22"), outDto);


                    //Anz_manAblehnungen_l6M ANZMANABL6M                                                                                                                                                                                                                                      
                    queryString = " select count(antrag.sysid)  ANZMANABL6M " +
                                  "  from rating,antrag, antobsich o" +
                                  "  where rating.sysid=antrag.sysid " +
                                  "  and rating.FLAG1=0 " +
                                  "  and rating.SYSCHGUSER>0 " +
                                  "  and rating.status=4 " +
                                  "  and antrag.sysid = o.sysantrag(+) " +
                                  "  and (antrag.syskd in (:pDoubletten) OR o.sysperson in (:pDoubletten))" +
                                  "  and trunc(rating.SYSCHGDATE,'dd') between trunc(add_months(sysdate,-6),'dd') and trunc(sysdate,'dd') ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL23"), outDto);
                    //Anz_manAblehnungen_l12M ANZMANABL12M 

                    queryString = " select count(antrag.sysid)  ANZMANABL12M " +
                                  "  from rating,antrag, antobsich o" +
                                  "  where rating.sysid=antrag.sysid " +
                                  "  and rating.FLAG1=0 " +
                                  "  and rating.SYSCHGUSER>0 " +
                                  "  and rating.status=4 " +
                                  "  and antrag.sysid = o.sysantrag(+) " +
                                  "  and (antrag.syskd in (:pDoubletten) OR o.sysperson in (:pDoubletten))" +
                                  "  and trunc(rating.SYSCHGDATE,'dd') between trunc(add_months(sysdate,-12),'dd') and trunc(sysdate,'dd') ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL24"), outDto);



                    //Anz_Vertraege_mit_Spezialfall
                    queryString = "select nvl(count(vtoption.sysid),0) ANZVTSPEZ from vtoption,vt where (OPTION6 is not null or LENGTH(TRIM (option6)) > 0) AND vtoption.sysid=vt.sysid   AND vt.syskd in ( :pDoubletten ) ";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL26"), outDto);

                    //Anz_lfd_Vertraege_mit_Spezialfall
                    queryString = "select nvl(count(vtoption.sysid),0)  ANZVTSPEZLFD  from vtoption,vt where (OPTION6 is not null or LENGTH(TRIM (option6)) > 0) AND nvl(vt.endekz,0)=0 AND vtoption.sysid=vt.sysid   AND vt.syskd in ( :pDoubletten )";
                    Mapper.Map(MyExecuteQuery<AggregationOLOutDto>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL27"), outDto);


                    //Datum letzter Antrag des Kunden DATUMLETZTERANTRAG//Datum erster Antrag des Kunden DATUMERSTERANTRAG
                    //Durchschnittliche Anzahl erster Mahnungen pro Vertrag (auf Person/Dublette aggregiert) in den letzten 6 Monaten à (rnmahn.mahnstufe = 1) ANZMAHN1AVG6M
                    //Durchschnittliche Anzahl zweiter Mahnungen pro Vertrag (auf Person/Dublette aggregiert) in den letzten 6 Monaten à (rnmahn.mahnstufe = 2) ANZMAHN2AVG6M
                    //Durchschnittliche Anzahl dritte Mahnungen pro Vertrag (auf Person/Dublette aggregiert) in den letzten 6 Monaten à (rnmahn.mahnstufe = 3) ANZMAHN3AVG6M
                    //Durchschnittliche Anzahl Einzahlung pro Vertrag (auf Person/Dublette aggregiert) in den letzten 12 Monaten à count(fi.gebuchtdatum) ANZZAHLAVG12M
                    //Durchschnittliche Reduktion Buchsaldo pro Vertrag (auf Person/Dublette aggregiert) in den letzten 12 Monaten RUECKSTANDAVG
                    //Durchschnittlicher offene Posten (Rückstand) über alle Verträge, zum aktuellen Zeitpunkt BUCHSALDOAVG
                    //queryString = "Select 0 ANZMAHN1AVG6M, 0 ANZMAHN2AVG6M, 0 ANZMAHN3AVG6M, 0  ANZZAHLAVG12M, 0 RUECKSTANDAVG, 0 BUCHSALDOAVG  from dual";
                    queryString = @"WITH ANTRAG_AKTUELL AS (
                                              SELECT /*+materialize*/ sysid, syskd, erfassung DATUM_AGG
                                              FROM antrag
                                              WHERE sysid = :Param0
                                              )
                                            , Vorantraege as (
                                              select /*+materialize*/ aa.sysid, aa.DATUM_AGG, a.syskd VA_SYSKD
                                                , a.sysid VA_SYSID, a.erfassung VA_ERFASSUNG, a.zustand VA_ZUSTAND, a.zustandam VA_ZUSTANDAM, a.attribut VA_ATTRIBUT
                                                , a.vertriebsweg VA_VERTRIEBSWEG, a.sysvart VA_SYSVART, p.privatflag VA_PRIVATFLAG
                                              from ANTRAG_AKTUELL aa join antrag a on a.sysid != aa.sysid  -- Exkl. akt. Antrag
                                                left join person p on p.sysperson = a.syskd
                                              where 
                                                a.attribut <> 'Gelöscht' and a.testflag = 0 and a.antrag is not null 
                                                and (p.privatflag = 1 or p.privatflag is null)
                                                and a.syskd in (:pDoubletten)
                                                )
                                            , Vorvertraege as (
                                              select /*+materialize*/ va.*
                                                , vt.sysid VV_SYSID
                                                , vt.sysvart VV_SYSVART
                                                , vt.beginn VV_BEGINN
                                                , nvl(to_date(decode(vt.endeam, to_date('01.01.0111','dd.mm.yyyy'), NULL, vt.endeam), 'dd.mm.yy'), vt.ende) VV_ENDE
                                                , last_day(ADD_MONTHS((case when nvl(to_date(decode(vt.endeam, to_date('01.01.0111','dd.mm.yyyy'), NULL, vt.endeam), 'dd.mm.yy'), trunc(vt.ende)) > va.DATUM_AGG then va.DATUM_AGG
                                                                            else nvl(to_date(decode(vt.endeam, to_date('01.01.0111','dd.mm.yyyy'), NULL, vt.endeam), 'dd.mm.yy'), trunc(vt.ende)) end), -13)) DATUM_VON_12M
                                                , last_day(ADD_MONTHS((case when nvl(to_date(decode(vt.endeam, to_date('01.01.0111','dd.mm.yyyy'), NULL, vt.endeam), 'dd.mm.yy'), trunc(vt.ende)) > va.DATUM_AGG then va.DATUM_AGG
                                                                            else nvl(to_date(decode(vt.endeam, to_date('01.01.0111','dd.mm.yyyy'), NULL, vt.endeam), 'dd.mm.yy'), trunc(vt.ende)) end), -7)) DATUM_VON_6M
                                                , last_day(ADD_MONTHS((case when nvl(to_date(decode(vt.endeam, to_date('01.01.0111','dd.mm.yyyy'), NULL, vt.endeam), 'dd.mm.yy'), trunc(vt.ende)) > va.DATUM_AGG then va.DATUM_AGG
                                                                            else nvl(to_date(decode(vt.endeam, to_date('01.01.0111','dd.mm.yyyy'), NULL, vt.endeam), 'dd.mm.yy'), trunc(vt.ende)) end), -1)) DATUM_BIS
                                              from vorantraege va
                                                join vt on vt.sysantrag = va.VA_SYSID
                                              where round(MONTHS_BETWEEN(DATUM_AGG, nvl(to_date(decode(vt.endeam, to_date('01.01.0111','dd.mm.yyyy'), NULL, vt.endeam), 'dd.mm.yy'), trunc(vt.ende))), 0) <= 60  -- Exkl. VV älter 5 Jahre
                                              order by va.sysid, vt.sysid
                                              )
                                            , Buchsaldo AS (
                                              select /*+materialize*/ vv.VV_SYSID
                                                , nvl(sum (case when fivkznm.sysperiod = EXTRACT (YEAR FROM vv.vv_beginn) - 2005 and fivkznm.periode = EXTRACT (MONTH FROM vv.vv_beginn) then (fivkznm.sbsaldos - fivkznm.sbsaldoh) end),0) Buchsaldo_Ausz
                                                , nvl(sum (case when fivkznm.sysperiod = EXTRACT (YEAR FROM vv.DATUM_VON_6M) - 2005 and fivkznm.periode = EXTRACT (MONTH FROM vv.DATUM_VON_6M) then (fivkznm.sbsaldos - fivkznm.sbsaldoh) end),0) Buchsaldo_6M
                                                , nvl(sum (case when fivkznm.sysperiod = EXTRACT (YEAR FROM vv.DATUM_VON_12M) - 2005 and fivkznm.periode = EXTRACT (MONTH FROM vv.DATUM_VON_12M) then (fivkznm.sbsaldos - fivkznm.sbsaldoh) end),0) Buchsaldo_12M
                                                , nvl(sum (case when fivkznm.sysperiod = EXTRACT (YEAR FROM vv.DATUM_BIS) - 2005 and fivkznm.periode = EXTRACT (MONTH FROM vv.DATUM_BIS) then (fivkznm.sbsaldos - fivkznm.sbsaldoh) end),0) Buchsaldo_Datum
                                              from Vorvertraege vv
                                                JOIN penkonto ON vv.vv_sysid = penkonto.sysvt
                                                JOIN fivkznm ON fivkznm.sysskonto = penkonto.sysnkonto
                                              where vv.VV_SYSVART <> 6
                                                AND penkonto.rang = 10000
                                              group by vv.VV_SYSID
                                              order by 1
                                              )
                                            , Mahnungen AS (
                                              SELECT /*+materialize*/ vv.VV_SYSID
                                                , SUM(case when rnmahn.mahnstufe = 1 then 1 end) M1_6M
                                                , SUM(case when rnmahn.mahnstufe = 2 then 1 end) M2_6M
                                                , SUM(case when rnmahn.mahnstufe = 3 then 1 end) M3_6M
                                              FROM  fi, rnmahn, Vorvertraege vv
                                              WHERE rnmahn.sysfi = fi.sysfi
                                                and fi.sysvt = vv.VV_SYSID
                                                and rnmahn.mahnstufe IN (1,2,3)
                                                AND fi.sysft in (702,471)
                                                and rnmahn.mahndatum >= vv.DATUM_VON_6M AND rnmahn.mahndatum < vv.DATUM_BIS
                                                and rnmahn.stornoflag = 0
                                                and fi.sysvt >= 400000000
                                              group by vv.VV_SYSID
                                              )
                                            , Zahlungen AS (
                                              select  /*+materialize*/ vv.VV_SYSID
                                                , count(*) Zahlung_Anz_12M
                                              from Vorvertraege vv, fi
                                              WHERE vv.VV_SYSID = fi.sysvt
                                                AND fi.gebuchtflag = 1
                                                and fi.gegenkonto IN ('100211', '100200', '100699', '200116', '200117', '100000')
                                                and fi.stornoflag = 0
                                                and fi.art = 1
                                                and (fi.gbetrag + fi.gsteuer) * decode(fi.sollhaben,0,-1,1) > 0
                                                and fi.gebuchtdatum >= vv.DATUM_VON_12M and fi.gebuchtdatum < vv.DATUM_BIS
                                              group by vv.VV_SYSID
                                              )
                                            , OP AS (
                                              select  /*+materialize*/ vv.VV_SYSID
                                                , SUM(((NVL(rn.GBetrag, 0)
                                                      + NVL(rn.GSteuer, 0)
                                                      + NVL(rn.Mahnbetrag, 0)
                                                      + NVL(rn.Zinsen, 0)
                                                      + NVL(rn.Gebuehr, 0)
                                                      + NVL(rn.FremdGebuehr, 0))
                                                      - (NVL(rn.Teilzahlung, 0)
                                                      + NVL(rn.Anzahlung, 0)
                                                      + NVL(rn.Storno, 0)
                                                      + NVL(rn.WAusgleich, 0)))
                                                      * DECODE(rn.art,  0, 1,  1, -1)) Betrag
                                              from Vorvertraege vv, rn
                                              where rn.sysvt = vv.VV_SYSID
                                                and rn.valutadatum >= vv.DATUM_VON_12M and rn.valutadatum < vv.DATUM_BIS
                                                and rn.bezahlt = 0
                                                and rn.kreis in (201,10200,20200,30200,3000,6000,7000)
                                                and not(rn.mahnsperre = 1 and rn.zahlsperre = 1 and rn.msperrebis >= trunc(vv.DATUM_BIS))
                                              group by vv.VV_SYSID
                                              )

                                            , aggreg_va as ( 
                                              select sysid
                                                , min(VA_ERFASSUNG) DATUMERSTERANTRAG
                                                , max(VA_ERFASSUNG) DATUMLETZTERANTRAG
                                              from Vorantraege
                                              group by SYSID
                                              )

                                            , aggreg_vv as (
                                              select vv.SYSID
                                                , count(*) VV_ANZ
                                                , avg(nvl(mg.M1_6M, 0)) ANZ1MAHNUNGEN6MAVG
                                                , avg(nvl(mg.M2_6M, 0)) ANZ2MAHNUNGEN6MAVG
                                                , avg(nvl(mg.M3_6M, 0)) ANZ3MAHNUNGEN6MAVG
                                                , avg(nvl(zlg.Zahlung_Anz_12M, 0)) ANZZAHLUNGEN12MAVG
                                                , avg(nvl(OP.Betrag, 0)) RUECKSTANDAVG
                                                , sum(nvl(bs.Buchsaldo_12M, nvl(bs.Buchsaldo_Ausz,0))) BS_12M
                                                , sum(nvl(bs.Buchsaldo_Datum, 0)) BS_Datum
                                              from Vorvertraege vv    
                                                LEFT JOIN Mahnungen mg ON mg.VV_SYSID = vv.VV_SYSID
                                                LEFT JOIN Buchsaldo bs ON bs.VV_SYSID = vv.VV_SYSID
                                                LEFT JOIN OP ON OP.VV_SYSID = vv.VV_SYSID
                                                LEFT JOIN Zahlungen zlg ON zlg.VV_SYSID = vv.VV_SYSID
                                              group by SYSID
                                              )
  
                                            select an.SYSID
                                              , aggreg_va.DATUMERSTERANTRAG
                                              , aggreg_va.DATUMLETZTERANTRAG
                                              -- [JSCH]: Unterdrücken NULL zur Umgehung der Default 0
                                              , case when nvl(VV_ANZ,0) > 0 then round(aggreg_vv.ANZ1MAHNUNGEN6MAVG, 3) else -999 end ANZMAHN1AVG6M
                                              , case when nvl(VV_ANZ,0) > 0 then round(aggreg_vv.ANZ2MAHNUNGEN6MAVG, 3) else -999 end ANZMAHN2AVG6M
                                              , case when nvl(VV_ANZ,0) > 0 then round(aggreg_vv.ANZ3MAHNUNGEN6MAVG, 3) else -999 end ANZMAHN3AVG6M
                                              , case when nvl(VV_ANZ,0) > 0 then round(aggreg_vv.ANZZAHLUNGEN12MAVG, 3) else -999 end ANZZAHLAVG12M
                                              , case when nvl(VV_ANZ,0) > 0 then round(aggreg_vv.RUECKSTANDAVG, 3) else -999 end RUECKSTANDAVG
                                              , case when nvl(VV_ANZ,0) > 0 then round(case when BS_12M > 0 then 1 - (BS_Datum/BS_12M) else 0 end, 3) else -999 end BUCHSALDOAVG
                                            from ANTRAG_AKTUELL an
                                              left join aggreg_va on aggreg_va.sysid = an.sysid
                                              left join aggreg_vv on aggreg_vv.sysid = aggreg_va.sysid
                                            ";
                    queryString = MyBuildQueryWithDoubletten(queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL30");
                    Mapper.Map(MyCalcFeldBySQL<AggregationOLOutDto>(context, queryString, sysAntragId), outDto);

                 
                    // HAUSHALTSENGAGEMENT  (2.AS nicht dabei, muss allerdings für 1. AS und 2.AS denselben Wert anzeigen) -----------
                    // HAUSHALTSENGAGEMENT = GesamtEngagement für den 1. AS + Engagement für den 2. AS

                    // Doubletten für den 2. AS holen
                    kdDoubletten = MyGetAggregationOLDoublettenForAS(context, sysAntragId, ASInfo.sysPersonAS2);

                    // Engagement für den 2. AS:
                    queryString = " SELECT nvl(sum(fivkznm.sbsaldos - fivkznm.sbsaldoh),0) AS ENGAGEMENT2 " +
                                    " FROM penkonto, nkonto, fivkznm, vt " +
                                    " WHERE fivkznm.sysskonto = nkonto.sysnkonto " +
                                    " AND nkonto.sysnkonto = penkonto.sysnkonto " +
                                    " AND penkonto.sysvt = vt.sysid " +
                                    " AND penkonto.rang = 10000 " +
                                    " AND fivkznm.sysperiod = EXTRACT (YEAR FROM sysdate) - 2005 " +
                                    " AND fivkznm.periode = EXTRACT (MONTH FROM sysdate) " +
                                    " AND (fivkznm.sbsaldos - fivkznm.sbsaldoh <> 0) " +
                                    " AND vt.syskd in ( :pDoubletten ) ";

                    decimal engagement2 = MyExecuteQuery<decimal>(context, queryString, ":pDoubletten", kdDoubletten, "AGGSQLOL20");
                    outDto.HAUSHALTENGAGEMENT = outDto.GESAMTENGAGEMENT + engagement2;

                    // Haushaltsengagement Ende -----------------------------------------------
                }
                return outDto;
            }
            catch (System.ArgumentException ex)
            {
                _log.Error("ArgumentException in GetOLDatenBySysAntrag ", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler bei Get OL-Daten By SysAntrag ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Aggregiert DeltaVista Daten über die SysID des Antrags
        /// </summary>
        /// <param name="auskunftId"></param>
        /// <param name="sysAntragId"></param>
        /// <returns>AggregationDVOutDto</returns>
        public AggregationDVOutDto GetDVDatenBySysAntrag(long auskunftId, long sysAntragId)
        {
            try
            {
                AggregationDVOutDto outDto = new AggregationDVOutDto();
                using (DdOlExtended context = new DdOlExtended())
                {
                    String queryString = String.Empty;
                    EaiparDao eaiParDao = new EaiparDao();

                    // Den AS ermitteln
                    long sysPerson = ASInfo.sysPersonAS1;
                    if (ASInfo.sysPerson != ASInfo.sysPersonAS1)
                        sysPerson = ASInfo.sysPersonAS2;

             

                    // Die Auskunft-Ids holen
                    queryString = @"select max(auskunft.sysauskunft) from rating,ratingauskunft,ratingergebnis,auskunfttyp,auskunft
                                                where rating.sysrating = ratingergebnis.sysrating
                                                and ratingergebnis.sysratingergebnis = ratingauskunft.sysratingergebnis
                                                and auskunfttyp.sysauskunfttyp = auskunft.sysauskunfttyp
                                                and ratingauskunft.sysauskunft = auskunft.sysauskunft
                                                and rating.area = 'ANTRAG'
                                                and rating.flag1 = 0
                                                and  instr(:Param2,','||upper(auskunfttyp.methode)||',')>0
                                                and auskunft.STATUSNUM = 0
                                                and ratingauskunft.AREA = 'PERSON'
                                                and ratingauskunft.SYSID = :Param1
                                                and rating.sysid = :Param0";
                    
                    queryString = eaiParDao.getEaiParFileByCode("AGGSQLDV1", queryString);

                    // Die Auskunft-Id für AddressIdentification
                    long sysAuskunftDVAddrIdent = MyCalcFeldBySQL<long>(context, queryString, sysAntragId, sysPerson, ",ADRESSIDENTIFIKATION,CRIFIDENTIFYADDRESS,");

                    // Die Auskunft-Id für GetDebtDetails
                    long sysAuskunftDVGetDebtDetails = MyCalcFeldBySQL<long>(context, queryString, sysAntragId, sysPerson, ",GETDEBTDETAILSBYADRESSID,CRIFGETREPORT,");

                    // Die Auskunft-Id für GetCompanyDetails
                    long sysAuskunftDVGetCompanyDetails = MyCalcFeldBySQL<long>(context, queryString, sysAntragId, sysPerson, ",GETCOMPANYDETAILSBYADRESSID,");

                    // Aggregation
                    queryString = " SELECT TREFFER, ANZDECISIONMAKER, " +
                                           " ANZBONMEL, " +
                                           " ANZBONMEL12, " +
                                           " ANZBONMEL24, " +
                                           " ANZBONMEL36, " +
                                           " ANZBONMEL48, " +
                                           " ANZBONMEL60, " +

                                           " ANZBONMEL01STA, " +
                                           " ANZBONMEL01STA12, " +
                                           " ANZBONMEL01STA24, " +
                                           " ANZBONMEL01STA36, " +
                                           " ANZBONMEL01STA48, " +
                                           " ANZBONMEL01STA60, " +

                                           " ANZBONMEL02STA, " +
                                           " ANZBONMEL02STA12, " +
                                           " ANZBONMEL02STA24, " +
                                           " ANZBONMEL02STA36, " +
                                           " ANZBONMEL02STA48, " +
                                           " ANZBONMEL02STA60, " +

                                           " ANZBONMEL03STA, " +
                                           " ANZBONMEL03STA12, " +
                                           " ANZBONMEL03STA24, " +
                                           " ANZBONMEL03STA36, " +
                                           " ANZBONMEL03STA48, " +
                                           " ANZBONMEL03STA60, " +

                                           " ANZBONMEL04STA, " +
                                           " ANZBONMEL04STA12, " +
                                           " ANZBONMEL04STA24, " +
                                           " ANZBONMEL04STA36, " +
                                           " ANZBONMEL04STA48, " +
                                           " ANZBONMEL04STA60, " +
                                           " ANZBPMMFSTAT00, " +

                                           " BADSTAT, " +
                                           " BADSTAT12, " +
                                           " BADSTAT24, " +
                                           " BADSTAT36, " +
                                           " BADSTAT48, " +
                                           " BADSTAT60 " +

                                           " FROM " +
                                           " (select " +
                                               " count (sysdvdebtentry) ANZBONMEL12, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,1,1,0)) ANZBONMEL01STA12, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,2,1,0)) ANZBONMEL02STA12, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,3,1,0)) ANZBONMEL03STA12, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,4,1,0)) ANZBONMEL04STA12, " +
                                               " MAX(dvdebtentry.riskclass) BADSTAT12 " +
                                               " from auskunft,dvoutboni,dvdebtentry " +
                                               " where dvoutboni.sysdvoutboni = dvdebtentry.sysdvoutboni " +
                                               " and dvoutboni.sysauskunft = auskunft.sysauskunft " +
                                               " and auskunft.sysauskunft = :Param1 " +                       // Param1 ist sysAuskunftDVGetDebtDetails
                                               " and months_between(sysdate,to_date(dvdebtentry.dateopen,'YYYY,MM,DD')) < 12), " +
                                           " (select " +
                                               " count (sysdvdebtentry) ANZBONMEL24, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,1,1,0)) ANZBONMEL01STA24, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,2,1,0)) ANZBONMEL02STA24, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,3,1,0)) ANZBONMEL03STA24, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,4,1,0)) ANZBONMEL04STA24, " +
                                               " MAX(dvdebtentry.riskclass) BADSTAT24 " +
                                               " from auskunft, dvoutboni, dvdebtentry " +
                                               " where dvoutboni.sysdvoutboni = dvdebtentry.sysdvoutboni " +
                                               " and dvoutboni.sysauskunft = auskunft.sysauskunft " +
                                               " and auskunft.sysauskunft = :Param1 " +                       // Param1 ist sysAuskunftDVGetDebtDetails
                                               " and months_between(sysdate,to_date(dvdebtentry.dateopen,'YYYY,MM,DD')) < 24), " +
                                           " (select " +
                                               " count (sysdvdebtentry) ANZBONMEL36, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,1,1,0)) ANZBONMEL01STA36, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,2,1,0)) ANZBONMEL02STA36, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,3,1,0)) ANZBONMEL03STA36, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,4,1,0)) ANZBONMEL04STA36, " +
                                               " MAX(dvdebtentry.riskclass) BADSTAT36 " +
                                               " from auskunft, dvoutboni, dvdebtentry " +
                                               " where dvoutboni.sysdvoutboni = dvdebtentry.sysdvoutboni " +
                                               " and dvoutboni.sysauskunft = auskunft.sysauskunft " +
                                               " and auskunft.sysauskunft = :Param1 " +                       // Param1 ist sysAuskunftDVGetDebtDetails
                                               " and months_between(sysdate,to_date(dvdebtentry.dateopen,'YYYY,MM,DD')) < 36), " +

                                           " (select " +
                                               " count (sysdvdebtentry) ANZBONMEL48, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,1,1,0)) ANZBONMEL01STA48, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,2,1,0)) ANZBONMEL02STA48, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,3,1,0)) ANZBONMEL03STA48, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,4,1,0)) ANZBONMEL04STA48, " +
                                               " MAX(dvdebtentry.riskclass) BADSTAT48 " +
                                               " from auskunft, dvoutboni, dvdebtentry " +
                                               " where dvoutboni.sysdvoutboni = dvdebtentry.sysdvoutboni " +
                                               " and dvoutboni.sysauskunft = auskunft.sysauskunft " +
                                               " and auskunft.sysauskunft = :Param1 " +                       // Param1 ist sysAuskunftDVGetDebtDetails
                                               " and months_between(sysdate,to_date(dvdebtentry.dateopen,'YYYY,MM,DD')) < 48), " +

                                           " (select " +
                                               " count (sysdvdebtentry) ANZBONMEL60, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,1,1,0)) ANZBONMEL01STA60, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,2,1,0)) ANZBONMEL02STA60, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,3,1,0)) ANZBONMEL03STA60, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,4,1,0)) ANZBONMEL04STA60, " +
                                               " MAX(dvdebtentry.riskclass) BADSTAT60 " +
                                               " from auskunft, dvoutboni, dvdebtentry " +
                                               " where dvoutboni.sysdvoutboni = dvdebtentry.sysdvoutboni " +
                                               " and dvoutboni.sysauskunft = auskunft.sysauskunft " +
                                               " and auskunft.sysauskunft = :Param1 " +                       // Param1 ist sysAuskunftDVGetDebtDetails
                                               " and months_between(sysdate,to_date(dvdebtentry.dateopen,'YYYY,MM,DD')) < 60), " +

                                           " (SELECT  " +
                                               " COUNT (dvdebtentry .sysdvdebtentry) ANZBONMEL, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,1,1,0)) ANZBONMEL01STA, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,2,1,0)) ANZBONMEL02STA, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,3,1,0)) ANZBONMEL03STA, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,4,1,0)) ANZBONMEL04STA, " +
                                               " SUM(DECODE (dvdebtentry.riskclass,0,1,0)) ANZBPMMFSTAT00, " +
                                               " MAX(dvdebtentry.riskclass) BADSTAT " +
                                               " FROM auskunft, dvoutboni, dvdebtentry " +
                                               " WHERE dvoutboni.sysdvoutboni = dvdebtentry.sysdvoutboni " +
                                                   " AND dvoutboni.sysauskunft = auskunft.sysauskunft " +
                                                   " AND auskunft.sysauskunft = :Param1),  " +

                                           " (select count(dvmgmntm.sysdvmgmntm) AS ANZDECISIONMAKER " + 
                                               " from dvoutcd, dvmgmntm " + 
                                               " where dvoutcd.sysauskunft = :Param2 and dvoutcd.sysdvoutcd = dvmgmntm.sysdvoutcd and " + 
                                               " (dvmgmntm.enddate > sysdate or dvmgmntm.enddate is null)), " +

                                           " (select count(sysdvadrmatch) TREFFER from dvoutadrid,dvadrmatch " +
                                               " where dvoutadrid.sysauskunft = :Param0 " +                 // Param0 ist sysAuskunftDVAddrIdent
                                               " and dvadrmatch.sysdvoutadrid = dvoutadrid.sysdvoutadrid) ";

                    String queryStringCrif = @"SELECT TREFFER,
                                                  ANZDECISIONMAKER,
                                                  ANZBONMEL,
                                                  ANZBONMEL12,
                                                  ANZBONMEL24,
                                                  ANZBONMEL36,
                                                  ANZBONMEL48,
                                                  ANZBONMEL60,
                                                  ANZBONMEL01STA,
                                                  ANZBONMEL01STA12,
                                                  ANZBONMEL01STA24,
                                                  ANZBONMEL01STA36,
                                                  ANZBONMEL01STA48,
                                                  ANZBONMEL01STA60,
                                                  ANZBONMEL02STA,
                                                  ANZBONMEL02STA12,
                                                  ANZBONMEL02STA24,
                                                  ANZBONMEL02STA36,
                                                  ANZBONMEL02STA48,
                                                  ANZBONMEL02STA60,
                                                  ANZBONMEL03STA,
                                                  ANZBONMEL03STA12,
                                                  ANZBONMEL03STA24,
                                                  ANZBONMEL03STA36,
                                                  ANZBONMEL03STA48,
                                                  ANZBONMEL03STA60,
                                                  ANZBONMEL04STA,
                                                  ANZBONMEL04STA12,
                                                  ANZBONMEL04STA24,
                                                  ANZBONMEL04STA36,
                                                  ANZBONMEL04STA48,
                                                  ANZBONMEL04STA60,
                                                  ANZBPMMFSTAT00,
                                                  BADSTAT,
                                                  BADSTAT12,
                                                  BADSTAT24,
                                                  BADSTAT36,
                                                  BADSTAT48,
                                                  BADSTAT60
                                                FROM
                                                  (SELECT COUNT(1) AS TREFFER
                                                  FROM CFOUTIDENTADDR,
                                                    CFADDRESSMATCH,
                                                    CFCANDIDATE
                                                  WHERE CFOUTIDENTADDR.SYSCFADDRESSMATCH = CFADDRESSMATCH.SYSCFADDRESSMATCH
                                                  AND CFCANDIDATE.SYSCFADDRESSMATCH (+)  = CFADDRESSMATCH.SYSCFADDRESSMATCH
                                                  AND CFOUTIDENTADDR.SYSAUSKUNFT         = :Param0
                                                  ),
                                                  (SELECT COUNT(1) AS ANZDECISIONMAKER
                                                  FROM CFOUTGETREPORT,
                                                    CFORGAPOSITION,
                                                    CFPOSITIONFUNC
                                                  WHERE CFOUTGETREPORT.SYSCFOUTGETREPORT = CFORGAPOSITION.SYSCFOUTGETREPORT
                                                  AND CFPOSITIONFUNC.SYSCFPOSITIONFUNC   = CFORGAPOSITION.SYSCFPOSITIONFUNC
                                                  AND CFOUTGETREPORT.SYSAUSKUNFT         = :Param1
                                                  AND (CFORGAPOSITION.PERIODEND          > TRUNC(sysdate)
                                                  OR CFORGAPOSITION.PERIODEND           IS NULL)
                                                  AND (CFORGAPOSITION.PERIODSTART        < TRUNC(sysdate) 
                                                  OR CFORGAPOSITION.PERIODSTART        IS NULL
                                                  )),
                                                  (SELECT COUNT(1)                                                                                                         AS ANZBONMEL ,
                                                    NVL(SUM(DECODE(riskclass,'NO_NEGATIVE',1,0)),0)                                                                        AS ANZBPMMFSTAT00 ,
                                                    NVL(MAX(DECODE(riskclass,'NO_NEGATIVE',0,'PRE_LEGAL',1,'LEGAL_INITIAL',2,'LEGAL_ESCALATION',3,'LEGAL_DEFAULTED',4)),0) AS BADSTAT ,
                                                    NVL(SUM(DECODE(riskclass,'PRE_LEGAL',1,0)),0)                                                                          AS ANZBONMEL01STA ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_INITIAL',1,0)),0)                                                                      AS ANZBONMEL02STA ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_ESCALATION',1,0)),0)                                                                   AS ANZBONMEL03STA ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_DEFAULTED',1,0)),0)                                                                    AS ANZBONMEL04STA
                                                  FROM CFDEBT,
                                                    CFOUTGETREPORT
                                                  WHERE CFDEBT.SYSCFOUTGETREPORT = CFOUTGETREPORT.SYSCFOUTGETREPORT
                                                  AND CFOUTGETREPORT.SYSAUSKUNFT = :Param1
                                                  ),
                                                  (SELECT COUNT(1)                                                                                                         AS ANZBONMEL12 ,
                                                    NVL(SUM(DECODE(riskclass,'PRE_LEGAL',1,0)),0)                                                                          AS ANZBONMEL01STA12 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_INITIAL',1,0)),0)                                                                      AS ANZBONMEL02STA12 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_ESCALATION',1,0)),0)                                                                   AS ANZBONMEL03STA12 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_DEFAULTED',1,0)),0)                                                                    AS ANZBONMEL04STA12 ,
                                                    NVL(MAX(DECODE(riskclass,'NO_NEGATIVE',0,'PRE_LEGAL',1,'LEGAL_INITIAL',2,'LEGAL_ESCALATION',3,'LEGAL_DEFAULTED',4)),0) AS BADSTAT12
                                                  FROM CFDEBT,
                                                    CFOUTGETREPORT
                                                  WHERE CFDEBT.SYSCFOUTGETREPORT                            = CFOUTGETREPORT.SYSCFOUTGETREPORT
                                                  AND months_between(TRUNC(sysdate),TRUNC(CFDEBT.DATEOPEN)) < 12
                                                  AND CFOUTGETREPORT.SYSAUSKUNFT                            = :Param1
                                                  ) ,
                                                  (SELECT COUNT(1)                                                                                                         AS ANZBONMEL24 ,
                                                    NVL(SUM(DECODE(riskclass,'PRE_LEGAL',1,0)),0)                                                                          AS ANZBONMEL01STA24 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_INITIAL',1,0)),0)                                                                      AS ANZBONMEL02STA24 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_ESCALATION',1,0)),0)                                                                   AS ANZBONMEL03STA24 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_DEFAULTED',1,0)),0)                                                                    AS ANZBONMEL04STA24 ,
                                                    NVL(MAX(DECODE(riskclass,'NO_NEGATIVE',0,'PRE_LEGAL',1,'LEGAL_INITIAL',2,'LEGAL_ESCALATION',3,'LEGAL_DEFAULTED',4)),0) AS BADSTAT24
                                                  FROM CFDEBT,
                                                    CFOUTGETREPORT
                                                  WHERE CFDEBT.SYSCFOUTGETREPORT                            = CFOUTGETREPORT.SYSCFOUTGETREPORT
                                                  AND months_between(TRUNC(sysdate),TRUNC(CFDEBT.DATEOPEN)) < 24
                                                  AND CFOUTGETREPORT.SYSAUSKUNFT                            = :Param1
                                                  ) ,
                                                  (SELECT COUNT(1)                                                                                                         AS ANZBONMEL36 ,
                                                    NVL(SUM(DECODE(riskclass,'PRE_LEGAL',1,0)),0)                                                                          AS ANZBONMEL01STA36 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_INITIAL',1,0)),0)                                                                      AS ANZBONMEL02STA36 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_ESCALATION',1,0)),0)                                                                   AS ANZBONMEL03STA36 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_DEFAULTED',1,0)),0)                                                                    AS ANZBONMEL04STA36 ,
                                                    NVL(MAX(DECODE(riskclass,'NO_NEGATIVE',0,'PRE_LEGAL',1,'LEGAL_INITIAL',2,'LEGAL_ESCALATION',3,'LEGAL_DEFAULTED',4)),0) AS BADSTAT36
                                                  FROM CFDEBT,
                                                    CFOUTGETREPORT
                                                  WHERE CFDEBT.SYSCFOUTGETREPORT                            = CFOUTGETREPORT.SYSCFOUTGETREPORT
                                                  AND months_between(TRUNC(sysdate),TRUNC(CFDEBT.DATEOPEN)) < 36
                                                  AND CFOUTGETREPORT.SYSAUSKUNFT                            = :Param1
                                                  ) ,
                                                  (SELECT COUNT(1)                                                                                                         AS ANZBONMEL48 ,
                                                    NVL(SUM(DECODE(riskclass,'PRE_LEGAL',1,0)),0)                                                                          AS ANZBONMEL01STA48 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_INITIAL',1,0)),0)                                                                      AS ANZBONMEL02STA48 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_ESCALATION',1,0)),0)                                                                   AS ANZBONMEL03STA48 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_DEFAULTED',1,0)),0)                                                                    AS ANZBONMEL04STA48 ,
                                                    NVL(MAX(DECODE(riskclass,'NO_NEGATIVE',0,'PRE_LEGAL',1,'LEGAL_INITIAL',2,'LEGAL_ESCALATION',3,'LEGAL_DEFAULTED',4)),0) AS BADSTAT48
                                                  FROM CFDEBT,
                                                    CFOUTGETREPORT
                                                  WHERE CFDEBT.SYSCFOUTGETREPORT                            = CFOUTGETREPORT.SYSCFOUTGETREPORT
                                                  AND months_between(TRUNC(sysdate),TRUNC(CFDEBT.DATEOPEN)) < 48
                                                  AND CFOUTGETREPORT.SYSAUSKUNFT                            = :Param1
                                                  ) ,
                                                  (SELECT COUNT(1)                                                                                                         AS ANZBONMEL60 ,
                                                    NVL(SUM(DECODE(riskclass,'PRE_LEGAL',1,0)),0)                                                                          AS ANZBONMEL01STA60 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_INITIAL',1,0)),0)                                                                      AS ANZBONMEL02STA60 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_ESCALATION',1,0)),0)                                                                   AS ANZBONMEL03STA60 ,
                                                    NVL(SUM(DECODE(riskclass,'LEGAL_DEFAULTED',1,0)),0)                                                                    AS ANZBONMEL04STA60 ,
                                                    NVL(MAX(DECODE(riskclass,'NO_NEGATIVE',0,'PRE_LEGAL',1,'LEGAL_INITIAL',2,'LEGAL_ESCALATION',3,'LEGAL_DEFAULTED',4)),0) AS BADSTAT60
                                                  FROM CFDEBT,
                                                    CFOUTGETREPORT
                                                  WHERE CFDEBT.SYSCFOUTGETREPORT                            = CFOUTGETREPORT.SYSCFOUTGETREPORT
                                                  AND months_between(TRUNC(sysdate),TRUNC(CFDEBT.DATEOPEN)) < 60
                                                  AND CFOUTGETREPORT.SYSAUSKUNFT                            = :Param1
                                                  )";


                        String CRIF = "select wert from cfgvar where syscfgsec in (select syscfgsec from cfgsec where syscfg in (select syscfg from cfg where code = 'ANTRAGSASSISTENT') and code = 'CRIF') and code = 'ACTIVEFLAG' ";
                        String iscrif = context.ExecuteStoreQuery<string>(CRIF, null).FirstOrDefault();
                        if (iscrif != null && iscrif == "TRUE")
                        {
                            queryString = eaiParDao.getEaiParFileByCode("AGGSQLDV2_CRIF", queryStringCrif);
                            DateTime startTime = DateTime.Now;
                            outDto = MyCalcFeldBySQL<AggregationDVOutDto>(context, queryString, sysAuskunftDVAddrIdent, sysAuskunftDVGetDebtDetails);

                            queryString = "";
                            _log.Info("AGGSQLDV2_CRIF Query duration : " + (TimeSpan)(DateTime.Now - startTime));

                        }
                        else
                        {
                            queryString = eaiParDao.getEaiParFileByCode("AGGSQLDV2", queryString);

                            DateTime startTime = DateTime.Now;
                            outDto = MyCalcFeldBySQL<AggregationDVOutDto>(context, queryString, sysAuskunftDVAddrIdent, sysAuskunftDVGetDebtDetails, sysAuskunftDVGetCompanyDetails);

                            queryString = "";
                            _log.Info("AGGSQLDV2 Query duration : " + (TimeSpan)(DateTime.Now - startTime));
                        }



   
                }
                return outDto;
            }
            catch (System.ArgumentException ex)
            {
                _log.Error("ArgumentException in Get DeltavistaDaten By SysAntrag ", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler bei Get DeltavistaDaten By SysAntrag ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Aggregiert ZEK Daten über die SysID des Antrags 
        /// </summary>
        /// <param name="auskunftId"></param>
        /// <param name="sysAntragId"></param>
        /// <returns>AggregationZekOutDto</returns>
        public AggregationZekOutDto GetZEKDatenBySysAntrag(long auskunftId, long sysAntragId)
        {
            try
            {
                AggregationZekOutDto outDto = new AggregationZekOutDto();
                outDto.AnzBC0506 = 0;
                outDto.AnzBC04 = 0;
                outDto.AnzBC04L12 = 0;
                outDto.AnzBC04L24 = 0;
                outDto.AnzBC04L36 = 0;
                outDto.AnzBC03 = 0;
                outDto.AnzBC03L12 = 0;
                outDto.AnzBC03L24 = 0;
                outDto.AnzBC03L36 = 0;
                outDto.ANZBC04LFD = 0;
                outDto.ANZBC04SAL = 0;
                outDto.ANZBC03LFD = 0;
                outDto.ANZBC03SAL = 0;
                outDto.ANZZEKENGBCODE61 = 0;
                outDto.ANZZEKENGBCODE61L12M = 0;
                outDto.ANZZEKENGBCODE61L24M = 0;
                outDto.ANZZEKENGBCODE61L36M = 0;
                outDto.ANZZEKENGBCODE61LFD = 0;
                outDto.ANZZEKENGBCODE61SAL = 0;
                outDto.ANZZEKENGBCODE71 = 0;
                outDto.ANZZEKENGBCODE71L12M = 0;
                outDto.ANZZEKENGBCODE71L24M = 0;
                outDto.ANZZEKENGBCODE71L36M = 0;
                outDto.ANZZEKENGBCODE71LFD = 0;
                outDto.ANZZEKENGBCODE71SAL = 0;



                using (DdOlExtended context = new DdOlExtended())
                {
                    DateTime startTime;
                    String queryString = String.Empty;

                    EaiparDao eaiParDao = new EaiparDao();

                    // ------------------------------------------------------------------------------------
                    long sysZekFC = 0;

                    // AntObSich.Rang : 120 = Solidarschuldner, 130 = ?, 800 = Partner
                    queryString = "select nvl(antobsich.sysperson,0) from antobsich where antobsich.sysantrag = :Param0 and antobsich.rang in (130, 800) ";
                    queryString = eaiParDao.getEaiParFileByCode("AGGSQLZEK1", queryString);
                    startTime = DateTime.Now;
                    long sysID = MyCalcFeldBySQL<int>(context, queryString, sysAntragId);
                    _log.Info("AGGSQLZEK1 Query duration : " + (TimeSpan)(DateTime.Now - startTime));

                    if (sysID == ASInfo.sysPerson)
                    {
                        // AS2 ist kein Solidarschuldner
                        queryString = "select max(zekfc.syszekfc) from ratingauskunft,auskunft,zekoutec2,zekinfr,zekfc " +
                                        " where ratingauskunft.sysid= :Param0 " +
                                        " and ratingauskunft.sysauskunft=auskunft.sysauskunft " +
                                        " and auskunft.sysauskunft=zekoutec2.sysauskunft " +
                                        " and zekoutec2.syszekinfr=zekinfr.syszekinfr " +
                                        " and zekinfr.syszekfc=zekfc.syszekfc " +
                                        " and auskunft.sysauskunfttyp = 11 " +
                                        " and auskunft.sysid= :Param1 " +
                                        " and auskunft.area= 'ANTRAG' " +
                                        " and auskunft.statusnum=0 " +
                                        " and auskunft.fehlercode=0 " +
                                        " order by auskunft.sysauskunft desc ";
                    }
                    else
                    {
                        // AS2 ist Solidarschuldner oder der AS ist der erste AS
                        queryString = "select max(zekfc.syszekfc) from auskunft,zekoutec1, zekcmr, zekresdesc, zekfc " +
                                        " where auskunft.sysauskunft=zekoutec1.sysauskunft " +
                                        " and zekoutec1.syszekcmr=zekcmr.syszekcmr " +
                                        " and zekcmr.syszekcmr=zekresdesc.syszekcmr " +
                                        " and zekresdesc.syszekfc=zekfc.syszekfc " +
                                        " and auskunft.sysauskunfttyp = 10  " +
                                        " and zekresdesc.refno= :Param0 " +
                                        " and auskunft.sysid= :Param1 " +
                                        " and auskunft.area= 'ANTRAG' " +
                                        " and auskunft.statusnum=0 " +
                                        " and auskunft.fehlercode=0 " +
                                        " order by auskunft.sysauskunft desc";
                    }
                    startTime = DateTime.Now;
                    sysZekFC = MyCalcFeldBySQL<int>(context, queryString, ASInfo.sysPerson, auskunftId);
                    _log.Info("AGGSQLZEK2 Query duration : " + (TimeSpan)(DateTime.Now - startTime));
                    // ------------------------------------------------------------------------------------

                    AggregationZekOutDto resultQueryDto = new AggregationZekOutDto();
                   

                    // ZKVERTRAGSART Werte :
                    // Code Comment                         Alt
                    // ------------------------------------------
                    // 1    Bardarlehen                     BDC
                    // 2    Festkredit                      FKC
                    // 3    Miet- / Leasingkredit           LMC
                    // 4    Teilzahlungskredit              TKC
                    // 5    Solidarschuldnervertrag         SSC
                    // 6    Kontokorrentkredit              KKC
                    // 7    Überziehungslimite              UKC
                    // 8    Kartenengagement                KEC

                    // Anzahl ZEK-Kartenmeldungen 
                    queryString = " select " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<12 then DECODE(ereigniscode,21,1,0) end) ANZKM21L12, " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<12 then DECODE(ereigniscode,22,1,0) end) ANZKM22L12, " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<24 then DECODE(ereigniscode,21,1,0) end) ANZKM21L24, " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<24 then DECODE(ereigniscode,22,1,0) end) ANZKM22L24, " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<36 then DECODE(ereigniscode,21,1,0) end) ANZKM21L36, " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<36 then DECODE(ereigniscode,22,1,0) end) ANZKM22L36, " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<48 then DECODE(ereigniscode,21,1,0) end) ANZKM21L48, " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<48 then DECODE(ereigniscode,22,1,0) end) ANZKM22L48, " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<60 then DECODE(ereigniscode,21,1,0) end) ANZKM21L60, " +
                                    " SUM(case when months_between(sysdate,to_date(datumnegativereignis,'YYYY-MM-DD'))<60 then DECODE(ereigniscode,22,1,0) end) ANZKM22L60, " +
                                    " sum(DECODE(ereigniscode,23,1,0)+DECODE(ereigniscode,24,1,0)+DECODE(ereigniscode,25,1,0)+DECODE(ereigniscode,26,1,0)) ANZKM2x, " +
                                    " SUM(case when datumpositivmeldung is not null then DECODE(ereigniscode,21,1,0) end) ANZZEKKMELDMCODE21POS, " +   //Zähle die Kartenmeldungen in der ZEK mit einem Ereigniscode 21 UND einem Positiveintrag
                                    " SUM(case when datumnegativereignis is not null and datumpositivmeldung is null then DECODE(ereigniscode,21,1,0) end) ANZZEKKMELDMCODE21NEG, " +  //Zähle die Kartenmeldungen in der ZEK mit einem Ereigniscode 21 OHNE Positiveintrag
                                    " SUM(case when datumpositivmeldung is not null then DECODE(ereigniscode,22,1,0) end) ANZZEKKMELDMCODE22POS, " +   //Zähle die Kartenmeldungen in der ZEK mit einem Ereigniscode 22 UND einem Positiveintrag
                                    " SUM(case when datumnegativereignis is not null and datumpositivmeldung is null then DECODE(ereigniscode,22,1,0) end) ANZZEKKMELDMCODE22NEG " +   //Zähle die Kartenmeldungen in der ZEK mit einem Ereigniscode 22 OHNE Positiveintrag
                                    " from zekkic where sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK3");
                    Mapper.Map(resultQueryDto, outDto);


                    // Anzahl ZEK Gesuche mit Ablehnungscode
                    queryString = " SELECT " +
                                    " SUM(DECODE(ABLEHNUNGSGRUND,5,1,0)+DECODE(ABLEHNUNGSGRUND,6,1,0)+DECODE(ABLEHNUNGSGRUND,8,1,0)+DECODE(ABLEHNUNGSGRUND,12,1,0)) ANZGAC05060812, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<12 then DECODE(ABLEHNUNGSGRUND,4,1,0) end) ANZGAC04L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<12 then DECODE(ABLEHNUNGSGRUND,7,1,0) end) ANZGAC07L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<12 then DECODE(ABLEHNUNGSGRUND,9,1,0) end) ANZGAC09L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<12 then DECODE(ABLEHNUNGSGRUND,10,1,0) end) ANZGAC10L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<12 then DECODE(ABLEHNUNGSGRUND,13,1,0) end) ANZGAC13L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<12 then DECODE(ABLEHNUNGSGRUND,14,1,0) end) ANZGAC14L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<12 then DECODE(ABLEHNUNGSGRUND,99,1,0) end) ANZGAC99L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<24 then DECODE(ABLEHNUNGSGRUND,4,1,0) end) ANZGAC04L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<24 then DECODE(ABLEHNUNGSGRUND,7,1,0) end) ANZGAC07L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<24 then DECODE(ABLEHNUNGSGRUND,9,1,0) end) ANZGAC09L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<24 then DECODE(ABLEHNUNGSGRUND,10,1,0) end) ANZGAC10L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<24 then DECODE(ABLEHNUNGSGRUND,13,1,0) end) ANZGAC13L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<24 then DECODE(ABLEHNUNGSGRUND,14,1,0) end) ANZGAC14L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMABLEHNUNG,'YYYY-MM-DD'))<24 then DECODE(ABLEHNUNGSGRUND,99,1,0) end) ANZGAC99L24 " +
                                    " FROM zekkgc where sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK4");
                    Mapper.Map(resultQueryDto, outDto);


                    // Anzahl ZEK Engagements mit Bonitätscode (Bardarlehen) //BNRSZ - 471 ANZZEKENGBCODE61
                    queryString = " SELECT SUM(DECODE(VERTRAGSSTATUS,4,0,1)) ANZBD, " +
                                    " count(syszekbdc) ANZENG, " +
                                    " SUM(DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0)) ANZBC0506, " +
                                    " SUM(DECODE(BONITAETSCODE,3,1,0)) ANZBC03, " +
                                    " SUM(DECODE(BONITAETSCODE,4,1,0)) ANZBC04, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L36, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04SAL, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03SAL, " +
                                    " SUM(DECODE(BONITAETSCODE,61,1,0)) ANZZEKENGBCODE61, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L12M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L24M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L36M, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61SLD " +
                                    " FROM zekbdc WHERE sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK5");
        
                    if (resultQueryDto.AnzBC0506 != null)
                        resultQueryDto.AnzBC0506 += outDto.AnzBC0506;
                    else
                        resultQueryDto.AnzBC0506 = outDto.AnzBC0506;

                    if (resultQueryDto.AnzBC03 != null)
                        resultQueryDto.AnzBC03 += outDto.AnzBC03;
                    else
                        resultQueryDto.AnzBC03 = outDto.AnzBC03;

                    if (resultQueryDto.AnzBC04 != null)
                        resultQueryDto.AnzBC04 += outDto.AnzBC04;
                    else
                        resultQueryDto.AnzBC04 = outDto.AnzBC04;

                    if (resultQueryDto.AnzBC04L12 != null)
                        resultQueryDto.AnzBC04L12 += outDto.AnzBC04L12;
                    else
                        resultQueryDto.AnzBC04L12 = outDto.AnzBC04L12;

                    if (resultQueryDto.AnzBC03L12 != null)
                        resultQueryDto.AnzBC03L12 += outDto.AnzBC03L12;
                    else
                        resultQueryDto.AnzBC03L12 = outDto.AnzBC03L12;


                    if (resultQueryDto.AnzBC04L24 != null)
                        resultQueryDto.AnzBC04L24 += outDto.AnzBC04L24;
                    else
                        resultQueryDto.AnzBC04L24 = outDto.AnzBC04L24;

                    if (resultQueryDto.AnzBC03L24 != null)
                        resultQueryDto.AnzBC03L24 += outDto.AnzBC03L24;
                    else
                        resultQueryDto.AnzBC03L24 = outDto.AnzBC03L24;


                    if (resultQueryDto.AnzBC04L36 != null)
                        resultQueryDto.AnzBC04L36 += outDto.AnzBC04L36;
                    else
                        resultQueryDto.AnzBC04L36 = outDto.AnzBC04L36;

                    if (resultQueryDto.AnzBC03L36 != null)
                        resultQueryDto.AnzBC03L36 += outDto.AnzBC03L36;
                    else
                        resultQueryDto.AnzBC03L36 = outDto.AnzBC03L36;
                    if (resultQueryDto.ANZZEKENGBCODE61 != null)
                        resultQueryDto.ANZZEKENGBCODE61 += outDto.ANZZEKENGBCODE61;
                    else
                        resultQueryDto.ANZZEKENGBCODE61 = outDto.ANZZEKENGBCODE61;

                    if (resultQueryDto.ANZZEKENGBCODE71 != null)
                        resultQueryDto.ANZZEKENGBCODE71 += outDto.ANZZEKENGBCODE71;
                    else
                        resultQueryDto.ANZZEKENGBCODE71 = outDto.ANZZEKENGBCODE71;

                    if (resultQueryDto.ANZZEKENGBCODE61L12M != null)
                        resultQueryDto.ANZZEKENGBCODE61L12M += outDto.ANZZEKENGBCODE61L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L12M = outDto.ANZZEKENGBCODE61L12M;

                    if (resultQueryDto.ANZZEKENGBCODE71L12M != null)
                        resultQueryDto.ANZZEKENGBCODE71L12M += outDto.ANZZEKENGBCODE71L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L12M = outDto.ANZZEKENGBCODE71L12M;



                    if (resultQueryDto.ANZZEKENGBCODE61L24M != null)
                        resultQueryDto.ANZZEKENGBCODE61L24M += outDto.ANZZEKENGBCODE61L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L24M = outDto.ANZZEKENGBCODE61L24M;

                    if (resultQueryDto.ANZZEKENGBCODE71L24M != null)
                        resultQueryDto.ANZZEKENGBCODE71L24M += outDto.ANZZEKENGBCODE71L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L24M = outDto.ANZZEKENGBCODE71L24M;


                    if (resultQueryDto.ANZZEKENGBCODE61L36M != null)
                        resultQueryDto.ANZZEKENGBCODE61L36M += outDto.ANZZEKENGBCODE61L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L36M = outDto.ANZZEKENGBCODE61L36M;

                    if (resultQueryDto.ANZZEKENGBCODE71L36M != null)
                        resultQueryDto.ANZZEKENGBCODE71L36M += outDto.ANZZEKENGBCODE71L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L36M = outDto.ANZZEKENGBCODE71L36M;

                    if (resultQueryDto.ANZBC04LFD != null)
                        resultQueryDto.ANZBC04LFD += outDto.ANZBC04LFD;
                    else
                        resultQueryDto.ANZBC04LFD = outDto.ANZBC04LFD;

                    if (resultQueryDto.ANZBC04SAL != null)
                        resultQueryDto.ANZBC04SAL += outDto.ANZBC04SAL;
                    else
                        resultQueryDto.ANZBC04SAL = outDto.ANZBC04SAL;

                    if (resultQueryDto.ANZBC03LFD != null)
                        resultQueryDto.ANZBC03LFD += outDto.ANZBC03LFD;
                    else
                        resultQueryDto.ANZBC03LFD = outDto.ANZBC03LFD;

                    if (resultQueryDto.ANZBC03SAL != null)
                        resultQueryDto.ANZBC03SAL += outDto.ANZBC03SAL;
                    else
                        resultQueryDto.ANZBC03SAL = outDto.ANZBC03SAL;

                    if (resultQueryDto.ANZZEKENGBCODE61LFD != null)
                        resultQueryDto.ANZZEKENGBCODE61LFD += outDto.ANZZEKENGBCODE61LFD;
                    else
                        resultQueryDto.ANZZEKENGBCODE61LFD = outDto.ANZZEKENGBCODE61LFD;

                    if (resultQueryDto.ANZZEKENGBCODE71SAL != null)
                        resultQueryDto.ANZZEKENGBCODE71SAL += outDto.ANZZEKENGBCODE71SAL;
                    else
                        resultQueryDto.ANZZEKENGBCODE71SAL = outDto.ANZZEKENGBCODE71SAL;

                    Mapper.Map(resultQueryDto, outDto);


                    // Anzahl ZEK Engagements mit Bonitätscode (Festkredit) //BNRSZ - 471 ANZZEKENGBCODE61
                    queryString = " SELECT SUM(DECODE(VERTRAGSSTATUS,4,0,1)) ANZFK, " +
                                    " count(syszekfkc) ANZENG, " +
                                    " SUM(DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0)) ANZBC0506, " +
                                    " SUM(DECODE(BONITAETSCODE,3,1,0)) ANZBC03, " +
                                    " SUM(DECODE(BONITAETSCODE,4,1,0)) ANZBC04, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L36, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04SAL, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03SAL, " +
                                    " SUM(DECODE(BONITAETSCODE,61,1,0)) ANZZEKENGBCODE61, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L12M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L24M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L36M, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61SLD " +
                                    " FROM zekfkc WHERE sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK6");
                    resultQueryDto.AnzENG += outDto.AnzENG;
                    if (resultQueryDto.AnzBC0506 != null)
                        resultQueryDto.AnzBC0506 += outDto.AnzBC0506;
                    else
                        resultQueryDto.AnzBC0506 = outDto.AnzBC0506;

                    if (resultQueryDto.AnzBC03 != null)
                        resultQueryDto.AnzBC03 += outDto.AnzBC03;
                    else
                        resultQueryDto.AnzBC03 = outDto.AnzBC03;

                    if (resultQueryDto.AnzBC04 != null)
                        resultQueryDto.AnzBC04 += outDto.AnzBC04;
                    else
                        resultQueryDto.AnzBC04 = outDto.AnzBC04;

                    if (resultQueryDto.AnzBC04L12 != null)
                        resultQueryDto.AnzBC04L12 += outDto.AnzBC04L12;
                    else
                        resultQueryDto.AnzBC04L12 = outDto.AnzBC04L12;

                    if (resultQueryDto.AnzBC03L12 != null)
                        resultQueryDto.AnzBC03L12 += outDto.AnzBC03L12;
                    else
                        resultQueryDto.AnzBC03L12 = outDto.AnzBC03L12;


                    if (resultQueryDto.AnzBC04L24 != null)
                        resultQueryDto.AnzBC04L24 += outDto.AnzBC04L24;
                    else
                        resultQueryDto.AnzBC04L24 = outDto.AnzBC04L24;

                    if (resultQueryDto.AnzBC03L24 != null)
                        resultQueryDto.AnzBC03L24 += outDto.AnzBC03L24;
                    else
                        resultQueryDto.AnzBC03L24 = outDto.AnzBC03L24;


                    if (resultQueryDto.AnzBC04L36 != null)
                        resultQueryDto.AnzBC04L36 += outDto.AnzBC04L36;
                    else
                        resultQueryDto.AnzBC04L36 = outDto.AnzBC04L36;

                    if (resultQueryDto.AnzBC03L36 != null)
                        resultQueryDto.AnzBC03L36 += outDto.AnzBC03L36;
                    else
                        resultQueryDto.AnzBC03L36 = outDto.AnzBC03L36;

                    if (resultQueryDto.ANZZEKENGBCODE61 != null)
                        resultQueryDto.ANZZEKENGBCODE61 += outDto.ANZZEKENGBCODE61;
                    else
                        resultQueryDto.ANZZEKENGBCODE61 = outDto.ANZZEKENGBCODE61;

                    if (resultQueryDto.ANZZEKENGBCODE71 != null)
                        resultQueryDto.ANZZEKENGBCODE71 += outDto.ANZZEKENGBCODE71;
                    else
                        resultQueryDto.ANZZEKENGBCODE71 = outDto.ANZZEKENGBCODE71;

                    if (resultQueryDto.ANZZEKENGBCODE61L12M != null)
                        resultQueryDto.ANZZEKENGBCODE61L12M += outDto.ANZZEKENGBCODE61L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L12M = outDto.ANZZEKENGBCODE61L12M;

                    if (resultQueryDto.ANZZEKENGBCODE71L12M != null)
                        resultQueryDto.ANZZEKENGBCODE71L12M += outDto.ANZZEKENGBCODE71L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L12M = outDto.ANZZEKENGBCODE71L12M;



                    if (resultQueryDto.ANZZEKENGBCODE61L24M != null)
                        resultQueryDto.ANZZEKENGBCODE61L24M += outDto.ANZZEKENGBCODE61L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L24M = outDto.ANZZEKENGBCODE61L24M;

                    if (resultQueryDto.ANZZEKENGBCODE71L24M != null)
                        resultQueryDto.ANZZEKENGBCODE71L24M += outDto.ANZZEKENGBCODE71L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L24M = outDto.ANZZEKENGBCODE71L24M;


                    if (resultQueryDto.ANZZEKENGBCODE61L36M != null)
                        resultQueryDto.ANZZEKENGBCODE61L36M += outDto.ANZZEKENGBCODE61L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L36M = outDto.ANZZEKENGBCODE61L36M;

                    if (resultQueryDto.ANZZEKENGBCODE71L36M != null)
                        resultQueryDto.ANZZEKENGBCODE71L36M += outDto.ANZZEKENGBCODE71L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L36M = outDto.ANZZEKENGBCODE71L36M;

                    if (resultQueryDto.ANZBC04LFD != null)
                        resultQueryDto.ANZBC04LFD += outDto.ANZBC04LFD;
                    else
                        resultQueryDto.ANZBC04LFD = outDto.ANZBC04LFD;

                    if (resultQueryDto.ANZBC04SAL != null)
                        resultQueryDto.ANZBC04SAL += outDto.ANZBC04SAL;
                    else
                        resultQueryDto.ANZBC04SAL = outDto.ANZBC04SAL;

                    if (resultQueryDto.ANZBC03LFD != null)
                        resultQueryDto.ANZBC03LFD += outDto.ANZBC03LFD;
                    else
                        resultQueryDto.ANZBC03LFD = outDto.ANZBC03LFD;

                    if (resultQueryDto.ANZBC03SAL != null)
                        resultQueryDto.ANZBC03SAL += outDto.ANZBC03SAL;
                    else
                        resultQueryDto.ANZBC03SAL = outDto.ANZBC03SAL;

                    if (resultQueryDto.ANZZEKENGBCODE61LFD != null)
                        resultQueryDto.ANZZEKENGBCODE61LFD += outDto.ANZZEKENGBCODE61LFD;
                    else
                        resultQueryDto.ANZZEKENGBCODE61LFD = outDto.ANZZEKENGBCODE61LFD;

                    if (resultQueryDto.ANZZEKENGBCODE71SAL != null)
                        resultQueryDto.ANZZEKENGBCODE71SAL += outDto.ANZZEKENGBCODE71SAL;
                    else
                        resultQueryDto.ANZZEKENGBCODE71SAL = outDto.ANZZEKENGBCODE71SAL;

                    Mapper.Map(resultQueryDto, outDto);


                    // Anzahl ZEK Engagements mit Bonitätscode (Miet- / Leasingkredit) //BNRSZ - 471 ANZZEKENGBCODE71
                    queryString = " SELECT SUM(DECODE(VERTRAGSSTATUS,4,0,1)) ANZLEA, " +
                                    " count(syszeklmc) ANZENG, " +
                                    " SUM(DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0)) ANZBC0506, " +
                                    " SUM(DECODE(BONITAETSCODE,3,1,0)) ANZBC03, " +
                                    " SUM(DECODE(BONITAETSCODE,4,1,0)) ANZBC04, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L36, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04SAL, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03SAL, " +
                                    " SUM(DECODE(BONITAETSCODE,71,1,0)) ANZZEKENGBCODE71, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,71,1,0) end) ANZZEKENGBCODE71L12M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,71,1,0) end) ANZZEKENGBCODE71L24M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,71,1,0) end) ANZZEKENGBCODE71L36M, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,71,1,0) end) ANZZEKENGBCODE71LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,71,1,0) end) ANZZEKENGBCODE71SLD " +
                                    " FROM zeklmc WHERE sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK7");
                    resultQueryDto.AnzENG += outDto.AnzENG;
                    if (resultQueryDto.AnzBC0506 != null)
                        resultQueryDto.AnzBC0506 += outDto.AnzBC0506;
                    else
                        resultQueryDto.AnzBC0506 = outDto.AnzBC0506;

                    if (resultQueryDto.AnzBC03 != null)
                        resultQueryDto.AnzBC03 += outDto.AnzBC03;
                    else
                        resultQueryDto.AnzBC03 = outDto.AnzBC03;

                    if (resultQueryDto.AnzBC04 != null)
                        resultQueryDto.AnzBC04 += outDto.AnzBC04;
                    else
                        resultQueryDto.AnzBC04 = outDto.AnzBC04;

                    if (resultQueryDto.AnzBC04L12 != null)
                        resultQueryDto.AnzBC04L12 += outDto.AnzBC04L12;
                    else
                        resultQueryDto.AnzBC04L12 = outDto.AnzBC04L12;

                    if (resultQueryDto.AnzBC03L12 != null)
                        resultQueryDto.AnzBC03L12 += outDto.AnzBC03L12;
                    else
                        resultQueryDto.AnzBC03L12 = outDto.AnzBC03L12;


                    if (resultQueryDto.AnzBC04L24 != null)
                        resultQueryDto.AnzBC04L24 += outDto.AnzBC04L24;
                    else
                        resultQueryDto.AnzBC04L24 = outDto.AnzBC04L24;

                    if (resultQueryDto.AnzBC03L24 != null)
                        resultQueryDto.AnzBC03L24 += outDto.AnzBC03L24;
                    else
                        resultQueryDto.AnzBC03L24 = outDto.AnzBC03L24;


                    if (resultQueryDto.AnzBC04L36 != null)
                        resultQueryDto.AnzBC04L36 += outDto.AnzBC04L36;
                    else
                        resultQueryDto.AnzBC04L36 = outDto.AnzBC04L36;

                    if (resultQueryDto.AnzBC03L36 != null)
                        resultQueryDto.AnzBC03L36 += outDto.AnzBC03L36;
                    else
                        resultQueryDto.AnzBC03L36 = outDto.AnzBC03L36;

                    if (resultQueryDto.ANZZEKENGBCODE61 != null)
                        resultQueryDto.ANZZEKENGBCODE61 += outDto.ANZZEKENGBCODE61;
                    else
                        resultQueryDto.ANZZEKENGBCODE61 = outDto.ANZZEKENGBCODE61;

                    if (resultQueryDto.ANZZEKENGBCODE71 != null)
                        resultQueryDto.ANZZEKENGBCODE71 += outDto.ANZZEKENGBCODE71;
                    else
                        resultQueryDto.ANZZEKENGBCODE71 = outDto.ANZZEKENGBCODE71;

                    if (resultQueryDto.ANZZEKENGBCODE61L12M!= null)
                        resultQueryDto.ANZZEKENGBCODE61L12M += outDto.ANZZEKENGBCODE61L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L12M = outDto.ANZZEKENGBCODE61L12M;

                    if (resultQueryDto.ANZZEKENGBCODE71L12M != null)
                        resultQueryDto.ANZZEKENGBCODE71L12M += outDto.ANZZEKENGBCODE71L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L12M = outDto.ANZZEKENGBCODE71L12M;



                    if (resultQueryDto.ANZZEKENGBCODE61L24M != null)
                        resultQueryDto.ANZZEKENGBCODE61L24M += outDto.ANZZEKENGBCODE61L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L24M = outDto.ANZZEKENGBCODE61L24M;

                    if (resultQueryDto.ANZZEKENGBCODE71L24M != null)
                        resultQueryDto.ANZZEKENGBCODE71L24M += outDto.ANZZEKENGBCODE71L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L24M = outDto.ANZZEKENGBCODE71L24M;


                    if (resultQueryDto.ANZZEKENGBCODE61L36M != null)
                        resultQueryDto.ANZZEKENGBCODE61L36M += outDto.ANZZEKENGBCODE61L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L36M = outDto.ANZZEKENGBCODE61L36M;

                    if (resultQueryDto.ANZZEKENGBCODE71L36M != null)
                        resultQueryDto.ANZZEKENGBCODE71L36M += outDto.ANZZEKENGBCODE71L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L36M = outDto.ANZZEKENGBCODE71L36M;

                    if (resultQueryDto.ANZBC04LFD != null)
                        resultQueryDto.ANZBC04LFD += outDto.ANZBC04LFD;
                    else
                        resultQueryDto.ANZBC04LFD = outDto.ANZBC04LFD;

                    if (resultQueryDto.ANZBC04SAL != null)
                        resultQueryDto.ANZBC04SAL += outDto.ANZBC04SAL;
                    else
                        resultQueryDto.ANZBC04SAL = outDto.ANZBC04SAL;

                    if (resultQueryDto.ANZBC03LFD != null)
                        resultQueryDto.ANZBC03LFD += outDto.ANZBC03LFD;
                    else
                        resultQueryDto.ANZBC03LFD = outDto.ANZBC03LFD;

                    if (resultQueryDto.ANZBC03SAL != null)
                        resultQueryDto.ANZBC03SAL += outDto.ANZBC03SAL;
                    else
                        resultQueryDto.ANZBC03SAL = outDto.ANZBC03SAL;

                    if (resultQueryDto.ANZZEKENGBCODE61LFD != null)
                        resultQueryDto.ANZZEKENGBCODE61LFD += outDto.ANZZEKENGBCODE61LFD;
                    else
                        resultQueryDto.ANZZEKENGBCODE61LFD = outDto.ANZZEKENGBCODE61LFD;

                    if (resultQueryDto.ANZZEKENGBCODE71SAL != null)
                        resultQueryDto.ANZZEKENGBCODE71SAL += outDto.ANZZEKENGBCODE71SAL;
                    else
                        resultQueryDto.ANZZEKENGBCODE71SAL = outDto.ANZZEKENGBCODE71SAL;

                    Mapper.Map(resultQueryDto, outDto);


                    // Anzahl ZEK Engagements mit Bonitätscode (Teilzahlungskredit) //BNRSZ - 471 ANZZEKENGBCODE61
                    queryString = " SELECT SUM(DECODE(VERTRAGSSTATUS,4,0,1)) ANZTZ, " +
                                    " count(syszektkc) ANZENG, " +
                                    " SUM(DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0)) ANZBC0506, " +
                                    " SUM(DECODE(BONITAETSCODE,3,1,0)) ANZBC03, " +
                                    " SUM(DECODE(BONITAETSCODE,4,1,0)) ANZBC04, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L36, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04SAL, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03SAL, " +
                                    " SUM(DECODE(BONITAETSCODE,61,1,0)) ANZZEKENGBCODE61, " + 
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L12M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L24M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L36M, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61SLD " +

                                    " FROM zektkc WHERE sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK8");
                    resultQueryDto.AnzENG += outDto.AnzENG;
                    if (resultQueryDto.AnzBC0506 != null)
                        resultQueryDto.AnzBC0506 += outDto.AnzBC0506;
                    else
                        resultQueryDto.AnzBC0506 = outDto.AnzBC0506;

                    if (resultQueryDto.AnzBC03 != null)
                        resultQueryDto.AnzBC03 += outDto.AnzBC03;
                    else
                        resultQueryDto.AnzBC03 = outDto.AnzBC03;

                    if (resultQueryDto.AnzBC04 != null)
                        resultQueryDto.AnzBC04 += outDto.AnzBC04;
                    else
                        resultQueryDto.AnzBC04 = outDto.AnzBC04;

                    if (resultQueryDto.AnzBC04L12 != null)
                        resultQueryDto.AnzBC04L12 += outDto.AnzBC04L12;
                    else
                        resultQueryDto.AnzBC04L12 = outDto.AnzBC04L12;

                    if (resultQueryDto.AnzBC03L12 != null)
                        resultQueryDto.AnzBC03L12 += outDto.AnzBC03L12;
                    else
                        resultQueryDto.AnzBC03L12 = outDto.AnzBC03L12;


                    if (resultQueryDto.AnzBC04L24 != null)
                        resultQueryDto.AnzBC04L24 += outDto.AnzBC04L24;
                    else
                        resultQueryDto.AnzBC04L24 = outDto.AnzBC04L24;

                    if (resultQueryDto.AnzBC03L24 != null)
                        resultQueryDto.AnzBC03L24 += outDto.AnzBC03L24;
                    else
                        resultQueryDto.AnzBC03L24 = outDto.AnzBC03L24;


                    if (resultQueryDto.AnzBC04L36 != null)
                        resultQueryDto.AnzBC04L36 += outDto.AnzBC04L36;
                    else
                        resultQueryDto.AnzBC04L36 = outDto.AnzBC04L36;

                    if (resultQueryDto.AnzBC03L36 != null)
                        resultQueryDto.AnzBC03L36 += outDto.AnzBC03L36;
                    else
                        resultQueryDto.AnzBC03L36 = outDto.AnzBC03L36;

                    if (resultQueryDto.ANZZEKENGBCODE61 != null)
                        resultQueryDto.ANZZEKENGBCODE61 += outDto.ANZZEKENGBCODE61;
                    else
                        resultQueryDto.ANZZEKENGBCODE61 = outDto.ANZZEKENGBCODE61;

                    if (resultQueryDto.ANZZEKENGBCODE71 != null)
                        resultQueryDto.ANZZEKENGBCODE71 += outDto.ANZZEKENGBCODE71;
                    else
                        resultQueryDto.ANZZEKENGBCODE71 = outDto.ANZZEKENGBCODE71;

                    if (resultQueryDto.ANZZEKENGBCODE61L12M != null)
                        resultQueryDto.ANZZEKENGBCODE61L12M += outDto.ANZZEKENGBCODE61L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L12M = outDto.ANZZEKENGBCODE61L12M;

                    if (resultQueryDto.ANZZEKENGBCODE71L12M != null)
                        resultQueryDto.ANZZEKENGBCODE71L12M += outDto.ANZZEKENGBCODE71L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L12M = outDto.ANZZEKENGBCODE71L12M;



                    if (resultQueryDto.ANZZEKENGBCODE61L24M != null)
                        resultQueryDto.ANZZEKENGBCODE61L24M += outDto.ANZZEKENGBCODE61L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L24M = outDto.ANZZEKENGBCODE61L24M;

                    if (resultQueryDto.ANZZEKENGBCODE71L24M != null)
                        resultQueryDto.ANZZEKENGBCODE71L24M += outDto.ANZZEKENGBCODE71L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L24M = outDto.ANZZEKENGBCODE71L24M;


                    if (resultQueryDto.ANZZEKENGBCODE61L36M != null)
                        resultQueryDto.ANZZEKENGBCODE61L36M += outDto.ANZZEKENGBCODE61L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L36M = outDto.ANZZEKENGBCODE61L36M;

                    if (resultQueryDto.ANZZEKENGBCODE71L36M != null)
                        resultQueryDto.ANZZEKENGBCODE71L36M += outDto.ANZZEKENGBCODE71L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L36M = outDto.ANZZEKENGBCODE71L36M;

                    if (resultQueryDto.ANZBC04LFD != null)
                        resultQueryDto.ANZBC04LFD += outDto.ANZBC04LFD;
                    else
                        resultQueryDto.ANZBC04LFD = outDto.ANZBC04LFD;

                    if (resultQueryDto.ANZBC04SAL != null)
                        resultQueryDto.ANZBC04SAL += outDto.ANZBC04SAL;
                    else
                        resultQueryDto.ANZBC04SAL = outDto.ANZBC04SAL;

                    if (resultQueryDto.ANZBC03LFD != null)
                        resultQueryDto.ANZBC03LFD += outDto.ANZBC03LFD;
                    else
                        resultQueryDto.ANZBC03LFD = outDto.ANZBC03LFD;

                    if (resultQueryDto.ANZBC03SAL != null)
                        resultQueryDto.ANZBC03SAL += outDto.ANZBC03SAL;
                    else
                        resultQueryDto.ANZBC03SAL = outDto.ANZBC03SAL;

                    if (resultQueryDto.ANZZEKENGBCODE61LFD != null)
                        resultQueryDto.ANZZEKENGBCODE61LFD += outDto.ANZZEKENGBCODE61LFD;
                    else
                        resultQueryDto.ANZZEKENGBCODE61LFD = outDto.ANZZEKENGBCODE61LFD;

                    if (resultQueryDto.ANZZEKENGBCODE71SAL != null)
                        resultQueryDto.ANZZEKENGBCODE71SAL += outDto.ANZZEKENGBCODE71SAL;
                    else
                        resultQueryDto.ANZZEKENGBCODE71SAL = outDto.ANZZEKENGBCODE71SAL;
                    Mapper.Map(resultQueryDto, outDto);


                    // Anzahl ZEK Engagements mit Bonitätscode (Kontokorrentkredit) //BNRSZ - 471 ANZZEKENGBCODE61
                    queryString = " SELECT SUM(DECODE(VERTRAGSSTATUS,4,0,1)) ANZKK, " +
                                    " count(syszekkkc) ANZENG, " +
                                    " SUM(DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0)) ANZBC0506, " +
                                    " SUM(DECODE(BONITAETSCODE,3,1,0)) ANZBC03, " +
                                    " SUM(DECODE(BONITAETSCODE,4,1,0)) ANZBC04, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L12, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L24, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03L36, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,5,1,0)+DECODE(BONITAETSCODE,6,1,0) end) ANZBC0506L36, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,4,1,0) end) ANZBC04SAL, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,3,1,0) end) ANZBC03SAL, " +
                                    " SUM(DECODE(BONITAETSCODE,61,1,0)) ANZZEKENGBCODE61, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<12 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L12M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<24 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L24M, " +
                                    " SUM(case when months_between(sysdate,to_date(DATUMBONITAET,'YYYY-MM-DD'))<36 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61L36M, " +
                                    " SUM( case when vertragsstatus =3 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61LFD, " +
                                    " SUM( case when vertragsstatus =4 then DECODE(BONITAETSCODE,61,1,0) end) ANZZEKENGBCODE61SLD " +
                                    " FROM zekkkc WHERE sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK9");
                    resultQueryDto.AnzENG += outDto.AnzENG;
                    if (resultQueryDto.AnzBC0506 != null)
                        resultQueryDto.AnzBC0506 += outDto.AnzBC0506;
                    else
                        resultQueryDto.AnzBC0506 = outDto.AnzBC0506;

                    if (resultQueryDto.AnzBC03 != null)
                        resultQueryDto.AnzBC03 += outDto.AnzBC03;
                    else
                        resultQueryDto.AnzBC03 = outDto.AnzBC03;

                    if (resultQueryDto.AnzBC04 != null)
                        resultQueryDto.AnzBC04 += outDto.AnzBC04;
                    else
                        resultQueryDto.AnzBC04 = outDto.AnzBC04;

                    if (resultQueryDto.AnzBC04L12 != null)
                        resultQueryDto.AnzBC04L12 += outDto.AnzBC04L12;
                    else
                        resultQueryDto.AnzBC04L12 = outDto.AnzBC04L12;

                    if (resultQueryDto.AnzBC03L12 != null)
                        resultQueryDto.AnzBC03L12 += outDto.AnzBC03L12;
                    else
                        resultQueryDto.AnzBC03L12 = outDto.AnzBC03L12;


                    if (resultQueryDto.AnzBC04L24 != null)
                        resultQueryDto.AnzBC04L24 += outDto.AnzBC04L24;
                    else
                        resultQueryDto.AnzBC04L24 = outDto.AnzBC04L24;

                    if (resultQueryDto.AnzBC03L24 != null)
                        resultQueryDto.AnzBC03L24 += outDto.AnzBC03L24;
                    else
                        resultQueryDto.AnzBC03L24 = outDto.AnzBC03L24;


                    if (resultQueryDto.AnzBC04L36 != null)
                        resultQueryDto.AnzBC04L36 += outDto.AnzBC04L36;
                    else
                        resultQueryDto.AnzBC04L36 = outDto.AnzBC04L36;

                    if (resultQueryDto.AnzBC03L36 != null)
                        resultQueryDto.AnzBC03L36 += outDto.AnzBC03L36;
                    else
                        resultQueryDto.AnzBC03L36 = outDto.AnzBC03L36;

                    if (resultQueryDto.ANZZEKENGBCODE61 != null)
                        resultQueryDto.ANZZEKENGBCODE61 += outDto.ANZZEKENGBCODE61;
                    else
                        resultQueryDto.ANZZEKENGBCODE61 = outDto.ANZZEKENGBCODE61;

                    if (resultQueryDto.ANZZEKENGBCODE71 != null)
                        resultQueryDto.ANZZEKENGBCODE71 += outDto.ANZZEKENGBCODE71;
                    else
                        resultQueryDto.ANZZEKENGBCODE71 = outDto.ANZZEKENGBCODE71;

                    if (resultQueryDto.ANZZEKENGBCODE61L12M != null)
                        resultQueryDto.ANZZEKENGBCODE61L12M += outDto.ANZZEKENGBCODE61L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L12M = outDto.ANZZEKENGBCODE61L12M;

                    if (resultQueryDto.ANZZEKENGBCODE71L12M != null)
                        resultQueryDto.ANZZEKENGBCODE71L12M += outDto.ANZZEKENGBCODE71L12M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L12M = outDto.ANZZEKENGBCODE71L12M;



                    if (resultQueryDto.ANZZEKENGBCODE61L24M != null)
                        resultQueryDto.ANZZEKENGBCODE61L24M += outDto.ANZZEKENGBCODE61L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L24M = outDto.ANZZEKENGBCODE61L24M;

                    if (resultQueryDto.ANZZEKENGBCODE71L24M != null)
                        resultQueryDto.ANZZEKENGBCODE71L24M += outDto.ANZZEKENGBCODE71L24M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L24M = outDto.ANZZEKENGBCODE71L24M;


                    if (resultQueryDto.ANZZEKENGBCODE61L36M != null)
                        resultQueryDto.ANZZEKENGBCODE61L36M += outDto.ANZZEKENGBCODE61L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE61L36M = outDto.ANZZEKENGBCODE61L36M;

                    if (resultQueryDto.ANZZEKENGBCODE71L36M != null)
                        resultQueryDto.ANZZEKENGBCODE71L36M += outDto.ANZZEKENGBCODE71L36M;
                    else
                        resultQueryDto.ANZZEKENGBCODE71L36M = outDto.ANZZEKENGBCODE71L36M;

                    if (resultQueryDto.ANZBC04LFD != null)
                        resultQueryDto.ANZBC04LFD += outDto.ANZBC04LFD;
                    else
                        resultQueryDto.ANZBC04LFD = outDto.ANZBC04LFD;

                    if (resultQueryDto.ANZBC04SAL != null)
                        resultQueryDto.ANZBC04SAL += outDto.ANZBC04SAL;
                    else
                        resultQueryDto.ANZBC04SAL = outDto.ANZBC04SAL;

                    if (resultQueryDto.ANZBC03LFD != null)
                        resultQueryDto.ANZBC03LFD += outDto.ANZBC03LFD;
                    else
                        resultQueryDto.ANZBC03LFD = outDto.ANZBC03LFD;

                    if (resultQueryDto.ANZBC03SAL != null)
                        resultQueryDto.ANZBC03SAL += outDto.ANZBC03SAL;
                    else
                        resultQueryDto.ANZBC03SAL = outDto.ANZBC03SAL;

                    if (resultQueryDto.ANZZEKENGBCODE61LFD != null)
                        resultQueryDto.ANZZEKENGBCODE61LFD += outDto.ANZZEKENGBCODE61LFD;
                    else
                        resultQueryDto.ANZZEKENGBCODE61LFD = outDto.ANZZEKENGBCODE61LFD;

                    if (resultQueryDto.ANZZEKENGBCODE71SAL != null)
                        resultQueryDto.ANZZEKENGBCODE71SAL += outDto.ANZZEKENGBCODE71SAL;
                    else
                        resultQueryDto.ANZZEKENGBCODE71SAL = outDto.ANZZEKENGBCODE71SAL;
                    Mapper.Map(resultQueryDto, outDto);

                    // Anzahl ZEK Engagements (Kartenengagement)
                    // Es gibt bei Kartenengagements keine Bonitätscodes
                    queryString = " SELECT SUM(DECODE(VERTRAGSSTATUS,4,0,1)) ANZKA, " +
                                  " count(syszekkec) ANZENG " +
                                  " FROM zekkec WHERE sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK10");
                    resultQueryDto.AnzENG += outDto.AnzENG;
                    Mapper.Map(resultQueryDto, outDto);


                    // Anzahl ZEK Amtsmeldungen 01 - 05
                    // Anzahl ZEK Amtsmeldungen 01 - 05 in den letzten 12 Monaten	
                    queryString = " select " +
                                    " sum(DECODE(amtscode,1,1,0)+DECODE(amtscode,2,1,0)+DECODE(amtscode,3,1,0)+DECODE(amtscode,4,1,0)+DECODE(amtscode,5,1,0)) ANZZEKAM, " +
                                    " SUM(case when months_between(sysdate,to_date(datumeingabefrist,'YYYY-MM-DD'))<60 then " +
                                        " DECODE(amtscode,1,1,0)+DECODE(amtscode,2,1,0)+DECODE(amtscode,3,1,0)+DECODE(amtscode,4,1,0)+DECODE(amtscode,5,1,0) end) ANZZEKAML12 " +
                                    " from zekaic WHERE sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK11");
                    Mapper.Map(resultQueryDto, outDto);

                    // Anzahl ZEK Gesuche mit Ablehnungscodes 4, 7 und 9
                    queryString = "SELECT SUM(DECODE(ABLEHNUNGSGRUND, 4, 1, 0) + DECODE(ABLEHNUNGSGRUND, 7, 1, 0) + DECODE(ABLEHNUNGSGRUND, 9, 1, 0)) AS ANZGAC040709 FROM zekkgc where sysZekFC = :pSysId";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK18");
                    Mapper.Map(resultQueryDto, outDto);

                    // Anzahl laufende ZEK Fremdgesuche
                    // Kennzeichen-Werte (s. spe_XML-Interface_v2_5_5-en.pdf, 6.12 Kennzeichen) :
                    // 0=anderes Mitglied, 1=eigener Vertrag, eigene Filiale, 2=eigener Vertrag, andere Filiale
                    queryString = " select count(syszekkgc) ANZFREMDGESUCHE from zekkgc where KENNZEICHEN = 0 and VERTRAGSSTATUS = 1 and sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK12");
                    Mapper.Map(resultQueryDto, outDto);

                    // Anzahl laufende ZEK Eigengesuche
                    // Kennzeichen-Werte (s. spe_XML-Interface_v2_5_5-en.pdf, 6.12 Kennzeichen) :
                    // 0=anderes Mitglied, 1=eigener Vertrag, eigene Filiale, 2=eigener Vertrag, andere Filiale
                    queryString = " select count(syszekkgc) ANZEIGENGESUCHE from zekkgc where KENNZEICHEN <> 0 and VERTRAGSSTATUS = 1 and sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK17");
                    Mapper.Map(resultQueryDto, outDto);

                    // Anzahl ZEK Gesuche mit Ablehnungscode 
                    queryString = " select count(syszekkgc) AnzGACAll from zekkgc where ablehnungsgrund > 0 and sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK13");
                    Mapper.Map(resultQueryDto, outDto);

                    // Zähle die von BANK-now abgelehnten Anträge in der ZEK mit einem Ablehnungscode 04, 07, 09 oder 99
                    // Zähle die abgelehnten Anträge in der ZEK mit einem Ablehnungscode 04, 07, 09 oder 99, die NICHT von BANK-now abgelehnt wurden.
                    // Zähle die von BANK-now abgelehnten Anträge in der ZEK mit einem Ablehnungscode 13 oder 14
                    // Zähle die abgelehnten Anträge in der ZEK mit einem Ablehnungscode 13 oder 14, die NICHT von BANK-now abgelehnt wurden.
                    // Anzahl ZEK Gesuche mit Ablehnungscode 10
                    // Zähle die abgelehnten Anträge in der ZEK mit einem Ablehnungscode 10, die NICHT von BANK-now abgelehnt wurden.
                    queryString = " select " +
                                    " SUM( case when kennzeichen in (1,2) then DECODE(ABLEHNUNGSGRUND, 4, 1, 0) + DECODE(ABLEHNUNGSGRUND, 7, 1, 0) + DECODE(ABLEHNUNGSGRUND, 9, 1, 0) + DECODE(ABLEHNUNGSGRUND, 99, 1, 0)end) ANZZEKGESMABLCODE04070999E, " +
                                    " SUM( case when kennzeichen = 0 then DECODE(ABLEHNUNGSGRUND, 4, 1, 0) + DECODE(ABLEHNUNGSGRUND, 7, 1, 0) + DECODE(ABLEHNUNGSGRUND, 9, 1, 0) + DECODE(ABLEHNUNGSGRUND, 99, 1, 0)end) ANZZEKGESMABLCODE04070999F, " +
                                    " SUM( case when kennzeichen in (1,2) then DECODE(ABLEHNUNGSGRUND, 13, 1, 0) + DECODE(ABLEHNUNGSGRUND, 14, 1, 0) end) ANZZEKGESMABLCODE1314E,  " +
                                    " SUM( case when kennzeichen = 0 then DECODE(ABLEHNUNGSGRUND, 13, 1, 0) + DECODE(ABLEHNUNGSGRUND, 14, 1, 0) end) ANZZEKGESMABLCODE1314F, " +
                                    " SUM( case when kennzeichen in (1,2) then DECODE(ABLEHNUNGSGRUND, 10, 1, 0) end) ANZZEKGESMABLCODE10E,  " +
                                    " SUM( case when kennzeichen = 0 then DECODE(ABLEHNUNGSGRUND, 10, 1, 0)  end) ANZZEKGESMABLCODE10F " +
                                    " from zekkgc where sysZekFC = :pSysId ";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysId", sysZekFC.ToString(), "AGGSQLZEK20");
                    Mapper.Map(resultQueryDto, outDto);                
                                    

                    // Schlechtester ZEK Bonitätscode
                    queryString = " select BONITAETSCODE from ( " +
                                    " select BONITAETSCODE from zekbdc where sysZekFC = :pSysId " +
                                    " union " +
                                    " select BONITAETSCODE from zekkkc where sysZekFC = :pSysId " +
                                    " union " +
                                    " select BONITAETSCODE from zektkc where sysZekFC = :pSysId " +
                                    " union " +
                                    " select BONITAETSCODE from zeklmc where sysZekFC = :pSysId " +
                                    " union " +
                                    " select BONITAETSCODE from zekfkc  where sysZekFC = :pSysId " +
                                    " ) " +
                                    " where BONITAETSCODE is not null ";
                    queryString = eaiParDao.getEaiParFileByCode("AGGSQLZEK14", queryString);
                    Devart.Data.Oracle.OracleParameter[] QueryParams = new Devart.Data.Oracle.OracleParameter[] { new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysId", Value = sysZekFC } };
                    var allZekCodes = context.ExecuteStoreQuery<int>(queryString, QueryParams).ToArray();
                    outDto.WorstBC = MyGetWorstCode(allZekCodes, ZEK_Bonitaet);


                    // Schlechtester ZEK Ablehnungscode (darf nicht 0 sein)
                    queryString = " select distinct(ablehnungsgrund) from zekkgc where ablehnungsgrund is not null and ablehnungsgrund > 0 and sysZekFC = :pSysId ";
                    queryString = eaiParDao.getEaiParFileByCode("AGGSQLZEK15", queryString);
                    QueryParams = new Devart.Data.Oracle.OracleParameter[] { new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysId", Value = sysZekFC } };
                    allZekCodes = context.ExecuteStoreQuery<int>(queryString, QueryParams).ToArray();
                    outDto.WorstGAC = MyGetWorstCode(allZekCodes, ZEK_Ablehnungscode);


                    // Anzahl ZEK Synonyme
                    queryString = " SELECT count(*) " +
                                    " FROM auskunft, zekoutec1, zekcmr, zekresdesc, zekadrdesc " +
                                    " WHERE auskunft.sysauskunft = zekoutec1.sysauskunft " +
                                    " and auskunft.sysauskunfttyp = 10 " +
                                    " AND zekoutec1.syszekcmr    = zekcmr.syszekcmr " +
                                    " AND zekcmr.syszekcmr       = zekresdesc.syszekcmr " +
                                    " and zekresdesc.syszekresdesc = zekadrdesc.syszekresdesc " +
                                    " and auskunft.statusnum < 1 " +
                                    " AND auskunft.area          ='ANTRAG' " +
                                    " AND auskunft.sysid         = :Param0 " +
                                    " AND zekresdesc.retcode     = 8 " +
                                    " AND zekresdesc.refno = :Param1 " ;
                    queryString = eaiParDao.getEaiParFileByCode("AGGSQLZEK16", queryString);
                    outDto.AnzSynonyme = MyCalcFeldBySQL<int>(context, queryString, sysAntragId, ASInfo.sysPerson);


                    queryString = @"SELECT SUM(I_ZEK_NEGEINTRAG) I_ZEK_NEGEINTRAG, SUM(I_ZEK_POSEINTRAG) I_ZEK_POSEINTRAG FROM(
                                  SELECT SYSZEKFC, SUM(CASE WHEN BONITAETSCODE IN(3, 4, 61, 71, 5, 6) AND VERTRAGSSTATUS IN(3,4) THEN 1 ELSE 0 END) AS I_ZEK_NEGEINTRAG,
                                         SUM(CASE WHEN BONITAETSCODE IN(1, 2) AND VERTRAGSSTATUS IN(4) THEN 1 ELSE 0 END) AS I_ZEK_POSEINTRAG
                                    FROM ZEKBDC GROUP BY SYSZEKFC
                                  UNION
                                  SELECT SYSZEKFC, SUM(CASE WHEN BONITAETSCODE IN(3, 4, 61, 71, 5, 6) AND VERTRAGSSTATUS IN(3,4) THEN 1 ELSE 0 END) AS I_ZEK_NEGEINTRAG,
                                         SUM(CASE WHEN BONITAETSCODE IN(1, 2) AND VERTRAGSSTATUS IN(4) THEN 1 ELSE 0 END) AS I_ZEK_POSEINTRAG
                                    FROM ZEKKKC GROUP BY SYSZEKFC
                                  UNION
                                  SELECT SYSZEKFC, SUM(CASE WHEN BONITAETSCODE IN(3, 4, 61, 71, 5, 6) AND VERTRAGSSTATUS IN(3,4) THEN 1 ELSE 0 END) AS I_ZEK_NEGEINTRAG,
                                         SUM(CASE WHEN BONITAETSCODE IN(1, 2) AND VERTRAGSSTATUS IN(4) THEN 1 ELSE 0 END) AS I_ZEK_POSEINTRAG
                                    FROM ZEKLMC GROUP BY SYSZEKFC
                                  UNION
                                  SELECT SYSZEKFC, SUM(CASE WHEN BONITAETSCODE IN(3, 4, 61, 71, 5, 6) AND VERTRAGSSTATUS IN(3,4) THEN 1 ELSE 0 END) AS I_ZEK_NEGEINTRAG,
                                         SUM(CASE WHEN BONITAETSCODE IN(1, 2) AND VERTRAGSSTATUS IN(4) THEN 1 ELSE 0 END) AS I_ZEK_POSEINTRAG
                                    FROM ZEKSSC GROUP BY SYSZEKFC
                                  UNION
                                  SELECT SYSZEKFC, SUM(CASE WHEN BONITAETSCODE IN(3, 4, 61, 71, 5, 6) AND VERTRAGSSTATUS IN(3,4) THEN 1 ELSE 0 END) AS I_ZEK_NEGEINTRAG,
                                         SUM(CASE WHEN BONITAETSCODE IN(1, 2) AND VERTRAGSSTATUS IN(4) THEN 1 ELSE 0 END) AS I_ZEK_POSEINTRAG
                                    FROM ZEKTKC GROUP BY SYSZEKFC
                                  UNION
                                  SELECT SYSZEKFC, SUM(CASE WHEN BONITAETSCODE IN(3, 4, 61, 71, 5, 6) AND VERTRAGSSTATUS IN(3,4) THEN 1 ELSE 0 END) AS I_ZEK_NEGEINTRAG,
                                         SUM(CASE WHEN BONITAETSCODE IN(1, 2) AND VERTRAGSSTATUS IN(4) THEN 1 ELSE 0 END) AS I_ZEK_POSEINTRAG
                                    FROM ZEKFKC GROUP BY SYSZEKFC
                                  UNION
                                  SELECT SYSZEKFC, SUM(CASE WHEN nvl(EREIGNISCODE,0) > 0 THEN 1 ELSE 0 END) AS I_ZEK_NEGEINTRAG,
                                         SUM(CASE WHEN EREIGNISCODE in (1,2) THEN 1 ELSE 0 END) AS I_ZEK_POSEINTRAG
                                    FROM ZEKKIC GROUP BY SYSZEKFC
                                  UNION
                                  SELECT SYSZEKFC, SUM(CASE WHEN nvl(AMTSCODE,0) > 0 THEN 1 ELSE 0 END) AS I_ZEK_NEGEINTRAG,
                                         SUM(CASE WHEN AMTSCODE in (1,2) THEN 1 ELSE 0 END) AS I_ZEK_POSEINTRAG
                                    FROM ZEKAIC GROUP BY SYSZEKFC)
                                  WHERE SYSZEKFC = :pSysZEKFC";
                    resultQueryDto = MyExecuteQuery<AggregationZekOutDto>(context, queryString, ":pSysZEKFC", sysZekFC.ToString(), "AGGSQLZEK22");
                    Mapper.Map(resultQueryDto, outDto);

                }
                return outDto;
            }
            catch (System.ArgumentException ex)
            {
                _log.Error("ArgumentException in GetZEKDatenBySysAntrag ", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler bei Get ZEKDaten By SysAntrag ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Aggregiert VP Daten über die SysID des Antrags
        /// </summary>
        /// <param name="auskunftId"></param>
        /// <param name="sysAntragId"></param>
        /// <returns>AggregationVPOutDto</returns>
        public AggregationVPOutDto GetVPDatenBySysAntrag(long auskunftId, long sysAntragId)
        {
            try
            {
                // Die VP-Aggregation wird per Skripte gemacht
                return new AggregationVPOutDto();
            }
            catch (System.ArgumentException ex)
            {
                _log.Error("ArgumentException in GetVPDatenBySysAntrag ", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler bei Get VertriebsPartnerDaten By SysAntrag ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Creates new AggregationData, filled with data from AggregationInDto
        /// Creates new AggregationInp linked with AUSKUNFT and AggregationData
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveAggregationInDto(long sysAuskunft, AggregationInDto inDto)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    AGGINP AggregationInp = new AGGINP();
                    AggregationInp.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.AGGINP.Add(AggregationInp);
                    context.SaveChanges();

                    AGGINPDATA AggregationInpData = new AGGINPDATA();
                    
                    AggregationInpData = Mapper.Map<AggregationInDto, AGGINPDATA>(inDto);

                    AggregationInpData.AGGINP = AggregationInp;
                    context.AGGINPDATA.Add(AggregationInpData);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von AggregationInput. ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Updates existing Aggregation and creates or updates AggregationOut
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="sysAuskunft"></param>
        public void SaveAggregationOutDto(AggregationOutDto outDto, long sysAuskunft)
        {
            try
            {
                using (DdIcExtended context = new DdIcExtended())
                {
                    AGGOUT AggregationOut = new AGGOUT();
                    AggregationOut.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    context.AGGOUT.Add(AggregationOut);
                    context.SaveChanges();

                    // Wenn OutDto null ist, muss trotzdem eine leere Outputstruktur (AGGOUT, AGGOUTOL,...) angelegt werden, 
                    // denn sonst funktionieren Clarion-Scripte nicht richtig
                    MySaveAggregationDVOutDto(context, AggregationOut, outDto.aggDVOutDto);
                    MySaveAggregationOLOutDto(context, AggregationOut, outDto.aggOLOutDto);
                    MySaveAggregationVPOutDto(context, AggregationOut, outDto.aggVPOutDto);
                    MySaveAggregationZEKOutDto(context, AggregationOut, outDto.aggZEKOutDto);

                    DateTime startTime = DateTime.Now;
                    
                    _log.Info("SaveAggregationOutDto SaveChanges duration : " + (TimeSpan)(DateTime.Now - startTime));
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von AggregationOut. ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// GetAntragstellerInfo
        /// </summary>
        /// <param name="auskunftId"></param>
        /// <param name="sysAntragId"></param>
        /// <returns></returns>
        public void GetAntragstellerInfo(long auskunftId, long sysAntragId)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                using (DdOlExtended context = new DdOlExtended())
                {
                    // PersonID aus der RatingAuskunft für die aufgerufene AuskunftId holen
                    String queryString = "select sysid from ratingauskunft where sysauskunft = :Param0";
                    ASInfo.sysPerson = MyCalcFeldBySQL<long>(context, queryString, auskunftId);

                    // Die Id des 1. AS steht in antrag.syskd
                    queryString = "select syskd from antrag where antrag.sysid = :Param0";
                    ASInfo.sysPersonAS1 = MyCalcFeldBySQL<long>(context, queryString, sysAntragId);

                    // Die ID des 2. AS aus AntObSich in beiden Fällen holen:
                    queryString = "select nvl(antobsich.sysperson,0) from antobsich " +
                                  " where antobsich.sysantrag = :Param0 and antobsich.rang in (120, 130, 800) ";
                    
                    ASInfo.sysPersonAS2 = MyCalcFeldBySQL<int>(context, queryString, sysAntragId);
                    _log.Info("AGGSQL_AS2 Query duration : " + (TimeSpan)(DateTime.Now - startTime));

                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler in GetAntragstellerInfo. ", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Gets Aggregation by sysAuskunft and returns AggregationInDto filled with data from Aggregation
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>AggregationInDto</returns>
        public AggregationInDto FindBySysId(long sysAuskunft)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                /* var AggINPQuery = from aggInp in context.AGGINP
                                     join Auskunft in context.AUSKUNFT on aggInp.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                                     where Auskunft.SYSAUSKUNFT == sysAuskunft
                                     select aggInp;
                 // Hier reicht nur ein Datensatz, um in die AggInpData-Tabelle zu gelangen
                 AGGINP AggINP = AggINPQuery.FirstOrDefault();
                 if (AggINP == null)
                 {
                     throw new ArgumentException("Es existiert kein Datensatz mit sysAuskunft = " + sysAuskunft + " in der Tabelle AGGINP");
                 }

                 var AggregationQuery = from aggInpData in context.AGGINPDATA                                                            // Selektiere alle Aggregations
                                         join aggInp in context.AGGINP on aggInpData.AGGINP.SYSAGGINP equals aggInp.SYSAGGINP                // die in AggregationInp
                                         join Auskunft in context.AUSKUNFT on aggInp.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT     // mit Auskunft verbunden sind und
                                         where Auskunft.SYSAUSKUNFT == sysAuskunft                                                        // der gesuchten sysAuskunft entsprechen.
                                         orderby aggInpData.DELTAVISTAID descending, aggInpData.ZKSTATUS ascending
                                         select aggInpData;

                 // Hier reicht auch nur ein Datensatz, weil nur die AntragsID und DeltavistaID im InDto gebraucht wird.
                 // AntragsID ist in allen Datensätzen mit der sysAuskunft gleich.
                 AGGINPDATA AggInpData = AggregationQuery.FirstOrDefault();
                 if (AggInpData != null)
                 {
                     AggInpData.AGGINP = AggINP;
                 }
                 AggregationInDto AggregationInDto = new AggregationInDto();
                 AggregationInDto.SysAuskunft = sysAuskunft;                
                 AggregationInDto = Mapper.Map<AGGINPDATA, AggregationInDto>(AggInpData);
                 return AggregationInDto;
                 */
                long agginpcount = context.ExecuteStoreQuery<long>("select count(*) from agginp where sysauskunft="+sysAuskunft).FirstOrDefault();
                if (agginpcount==0)
                {
                    throw new ArgumentException("Es existiert kein Datensatz mit sysAuskunft = " + sysAuskunft + " in der Tabelle AGGINP");
                }
                AggregationInDto rval = context.ExecuteStoreQuery<AggregationInDto>("select aggInpData.* from agginpdata, agginp, auskunft where agginp.sysagginp = agginpdata.sysagginp and auskunft.sysauskunft = agginp.sysauskunft and auskunft.sysauskunft = " + sysAuskunft + " order by agginpdata.deltavistaid desc, agginpdata.zkstatus asc").FirstOrDefault();
                rval.SysAuskunft = sysAuskunft;
                return rval;
            }
        }



        #region My Methods

        private void MySaveAggregationDVOutDto(DdIcExtended context, AGGOUT AggregationOut, AggregationDVOutDto outDto)
        {
            try
            {
                // Deltavista
                AGGOUTDV AggOutDV = new AGGOUTDV();
                if (outDto != null)
                {
                  
                    AggOutDV = Mapper.Map<AggregationDVOutDto, AGGOUTDV>(outDto);
                }
                AggOutDV.AGGOUT = AggregationOut;

                context.AGGOUTDV.Add(AggOutDV);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von AggregationOut Deltavista.", ex);
                throw ex;
            }
        }

        private void MySaveAggregationOLOutDto(DdIcExtended context, AGGOUT AggregationOut, AggregationOLOutDto outDto)
        {
            try
            {
                // OpenLease
                AGGOUTOL AggOutOL = new AGGOUTOL();
                if (outDto != null)
                {
                    
                    AggOutOL = Mapper.Map<AggregationOLOutDto, AGGOUTOL>(outDto);
                }
                AggOutOL.AGGOUT = AggregationOut;
                AggOutOL.DATUMERSTERANTRAG = outDto.DATUMERSTERANTRAG;
                AggOutOL.DATUMLETZTERANTRAG = outDto.DATUMLETZTERANTRAG;
                AggOutOL.ANZMAHN1AVG6M = outDto.ANZMAHN1AVG6M;
                AggOutOL.ANZMAHN2AVG6M = outDto.ANZMAHN2AVG6M;
                AggOutOL.ANZMAHN3AVG6M = outDto.ANZMAHN3AVG6M;
                AggOutOL.ANZZAHLAVG12M = outDto.ANZZAHLAVG12M;
                AggOutOL.RUECKSTANDAVG = outDto.RUECKSTANDAVG;
                AggOutOL.BUCHSALDOAVG = outDto.BUCHSALDOAVG;
                context.AGGOUTOL.Add(AggOutOL);
                context.SaveChanges();

             

            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von AggregationOut OL. ", ex);
                throw ex;
            }
        }

        private void MySaveAggregationVPOutDto(DdIcExtended context, AGGOUT AggregationOut, AggregationVPOutDto outDto)
        {
            try
            {
                // Vertriebspartner
                AGGOUTVP AggOutVP = new AGGOUTVP();
                if (outDto != null)
                {
                    
                    AggOutVP = Mapper.Map<AggregationVPOutDto, AGGOUTVP>(outDto);
                }
                AggOutVP.AGGOUT = AggregationOut;

                context.AGGOUTVP.Add(AggOutVP);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von AggregationOut VertriebsPartner.", ex);
                throw ex;
            }
        }

        private void MySaveAggregationZEKOutDto(DdIcExtended context, AGGOUT AggregationOut, AggregationZekOutDto outDto)
        {
            try
            {
                // ZEK
                AGGOUTZEK AggOutZEK = new AGGOUTZEK();
                if (outDto != null)
                {
                    AggOutZEK = Mapper.Map<AggregationZekOutDto, AGGOUTZEK>(outDto);
                }
                AggOutZEK.AGGOUT = AggregationOut;
                AggOutZEK.ANZZEKMELDPOS = outDto.I_ZEK_POSEINTRAG;
                AggOutZEK.ANZZEKMELDNEG = outDto.I_ZEK_NEGEINTRAG;
                context.AGGOUTZEK.Add(AggOutZEK);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Speichern von AggregationOut ZEK.", ex);
                throw ex;
            }
        }

        //
        private int MyGetWorstCode(int[] allCodes, int[] codeTable)
        {
            int worstCode = -1;

            // alle Codes aus der Datenbank : AllCodes
            // Codes aus der Muster-Tabelle : CodeTable

            // array mit Indexes des Musterarray für die Codes aus der DB
            int[] indexes = new int[allCodes.Length];
            for (int i = 0; i < allCodes.Length; i++)
            {
                // wenn ein Element nicht gefunden wird, gibt IndexOf -1 zurück
                indexes[i] = Array.IndexOf(codeTable, allCodes[i]);
                // -1 darf nicht im Array stehen, sonst wird es als der kleinste Index zurückgegeben
                if (indexes[i] < 0) indexes[i] = codeTable.Length + 1;
            }
            // den kleinsten (schlechtesten) Code auf den Anfang des Array stellen
            Array.Sort(indexes);

            // wenn der Code in der CodeTabelle nicht gefunden wurde, dann return -1
            if (indexes.Length > 0 && indexes[0] >= 0 && indexes[0] < codeTable.Length)
            {
                worstCode = codeTable[indexes[0]];
            }
            return worstCode;
        }



        /// <summary>
        /// MyExecuteQuery 
        /// (Diese Methode ist nur wegen der Doubletten da, weil sie nicht über OracleParameter übergeben werden können.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="queryString"></param>
        /// <param name="paramName"></param>
        /// <param name="kdDoubletten"></param>
        /// <param name="queryCfgId"></param>
        /// <returns></returns>
        private T MyExecuteQuery<T>(DdOlExtended context, String queryString, String paramName, String kdDoubletten, String queryCfgId)
        {
            // Security check: kdDoubletten ist immer eine Liste von sysIds (long)
            String newQueryString = MyBuildQueryWithDoubletten(queryString, paramName, kdDoubletten, queryCfgId);

            int retries = 1 + Cic.OpenOne.GateBANKNOW.Common.Settings.Default.SQLRetryCount;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    DateTime startTime = DateTime.Now;
                    var result = context.ExecuteStoreQuery<T>(newQueryString, null).FirstOrDefault();
                    _log.Info(queryCfgId + " Query duration : " + (TimeSpan)(DateTime.Now - startTime));
                    return result;
                }
                catch (Exception ex)
                {
                    if (i == (retries - 1))
                    {
                        _log.Error("Retrying failed for " + newQueryString + ": ", ex);
                        throw ex;
                    }
                    else
                        _log.Warn("Retrying query " + newQueryString + " because of " + ex.Message, ex);
                }
            }
            throw new Exception("Error in Query: "+ queryCfgId);
        }

        /// <summary>
        /// MyBuildQueryWithDoubletten
        /// Holt einen QueryString mit queryCfgId aus der EaiPar-Tabelle. Wenn er nicht existiert, dann wird der queryString als Default zurückgegeben.
        /// Security check: kdDoubletten ist immer eine Liste von sysIds (long)
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="paramName"></param>
        /// <param name="kdDoubletten"></param>
        /// <param name="queryCfgId"></param>
        /// <returns></returns>
        private String MyBuildQueryWithDoubletten(String queryString, String paramName, String kdDoubletten, String queryCfgId)
        {
            // queryString = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(CFGSEC, queryCfgId, queryString, CFG);
            EaiparDao eaiParDao = new EaiparDao();
            String newQueryString = eaiParDao.getEaiParFileByCode(queryCfgId, queryString);
            newQueryString = newQueryString.Replace(paramName, kdDoubletten);
            return newQueryString;
        }


        /// <summary>
        /// MyCalcFeldBySQL
        /// Die Parameter im queryString müssen heissen: Param0, Param1, usw...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="queryString"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        private T MyCalcFeldBySQL<T>(DdOlExtended context, String queryString, params object[] sqlParams)
        {
            List<Devart.Data.Oracle.OracleParameter> paramsList = new List<Devart.Data.Oracle.OracleParameter>();
            for (int iParam = 0; iParam < sqlParams.Length; iParam++)
            {
                paramsList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "Param" + iParam, Value = sqlParams[iParam] });
            }
            return context.ExecuteStoreQuery<T>(queryString, paramsList.ToArray()).FirstOrDefault();
        }



        /// <summary>
        /// MyGetAggregationOLDoublettenForAS
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysAntragId"></param>
        /// <param name="sysPersonAS"></param>
        /// <returns></returns>
        private String MyGetAggregationOLDoublettenForAS(DdOlExtended context, long sysAntragId, long sysPersonAS)
        {
            try
            {
                String outDoubletten = String.Empty;

                // Doubletten holen (die Liste beinhaltet auch die sysPerson des AS)
                String queryDoublettenString = String.Empty;

                // Ist die gefundene sysPersonAS der 1. AS ?
                if (sysPersonAS == ASInfo.sysPersonAS1)
                    queryDoublettenString = "select sysperson from doublet where sysorigperson= :pSysPersonAS and syslease=:pAntragsID AND aktivkz = 1 " + 
                                            " union select syskd from antrag where sysid = :pAntragsID ";
                else
                    queryDoublettenString = "select sysperson from doublet where sysorigperson= :pSysPersonAS and syslease=:pAntragsID AND aktivkz = 1 " + 
                                            " union select sysperson from antobsich where sysantrag = :pAntragsID and rang in (120,130,800) ";

                Devart.Data.Oracle.OracleParameter[] queryParams = new Devart.Data.Oracle.OracleParameter[] { 
                                                                        new Devart.Data.Oracle.OracleParameter { ParameterName = "pAntragsID", Value = sysAntragId },
                                                                        new Devart.Data.Oracle.OracleParameter { ParameterName = "pSysPersonAS", Value = sysPersonAS } };
                DateTime startTime = DateTime.Now;
                List<long> kundenIds = context.ExecuteStoreQuery<long>(queryDoublettenString, queryParams).ToList();
                _log.Info("Doubletten Query duration : " + (TimeSpan)(DateTime.Now - startTime));

                outDoubletten = String.Join(",", kundenIds.ToArray());
                if (outDoubletten.Trim().Length == 0)
                {
                    // outDoubletten darf nicht leer sein
                    outDoubletten = sysPersonAS.ToString();
                }
                return outDoubletten;
            }
            catch (System.ArgumentException ex)
            {
                _log.Error("ArgumentException in MyGetAggregationOLDoublettenForAS ", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler bei MyGetAggregationOLDoublettenForAS ", ex);
                throw ex;
            }
        }

        #endregion
    }
}
