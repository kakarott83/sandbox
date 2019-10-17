using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util;

namespace Cic.One.DTO
{
    /// <summary>
    /// OL Areas as defined in WFTABLE
    /// </summary>
    public enum OlArea : int
    {
        /*	ACCOUNT	*/
        [StringValue("ACCOUNT")]
        ACCOUNT,
        /*	ADMADD	*/
        [StringValue("ADMADD")]
        ADMADD,
        /*	Übersicht der Adressenübersicht	*/
        [StringValue("ADRESSE")]
        ADRESSE,
        /*	Allgemeine Geschäftsbedingungen	*/
        [StringValue("AGB")]
        AGB,
        /*	Aktenverbleib	*/
        [StringValue("AKTE")]
        AKTE,
        /*	Angebot	*/
        [StringValue("ANGEBOT")]
        ANGEBOT,
        /*	Kalkulation zum Angebot	*/
        [StringValue("ANGKALK")]
        ANGKALK,
        /*	Objekt zum Angebot	*/
        [StringValue("ANGOB")]
        ANGOB,
        /*	Angebotsvarianten	*/
        [StringValue("ANGVAR")]
        ANGVAR,
        /*	Antragskalkulation	*/
        [StringValue("ANTKALK")]
        ANTKALK,
        /*	Objekt zum Antrag	*/
        [StringValue("ANTOB")]
        ANTOB,
        /*	Händler zum Antrag	*/
        [StringValue("ANTOBHD")]
        ANTOBHD,
        /*	Antragssicherheiten	*/
        [StringValue("ANTOBSICH")]
        ANTOBSICH,
        /*	Antragsstaffel	*/
        [StringValue("ANTOBSL")]
        ANTOBSL,
        /*	Optionale Daten im Antrag	*/
        [StringValue("ANTOPTION")]
        ANTOPTION,
        /*	Provision im Antrag	*/
        [StringValue("ANTPROV")]
        ANTPROV,
        /*	Anträge	*/
        [StringValue("ANTRAG")]
        ANTRAG,
        /*	Artikelbezeichnung	*/
        [StringValue("ARTIKEL")]
        ARTIKEL,
        /*	Auftragstabelle	*/
        [StringValue("AUFTRAG")]
        AUFTRAG,
        /*	Auftragsposition	*/
        [StringValue("AUFTRPOS")]
        AUFTRPOS,
        /*	Ausfalltabelle	*/
        [StringValue("AUSFALL")]
        AUSFALL,
        /*	Ausfalltypen	*/
        [StringValue("AUSFALLTYP")]
        AUSFALLTYP,
        /*	Buchungsarten	*/
        [StringValue("BA")]
        BA,
        /*	Betriebliches Anlagevermögen	*/
        [StringValue("BAV")]
        BAV,
        /*	Betr. Anlagevermögen Stückliste	*/
        [StringValue("BAVSTL")]
        BAVSTL,
        /*	Bankbelege	*/
        [StringValue("BB")]
        BB,
      
        /*	BBAB	*/
        [StringValue("BBAB")]
        BBAB,
        /*	Geschäftsvorfall	*/
        [StringValue("BBGV")]
        BBGV,
        /*	Bankbelegbuchungsposition	*/
        [StringValue("BBPOS")]
        BBPOS,
        /*	BBTYP	*/
        [StringValue("BBTYP")]
        BBTYP,
        /*	Verkäufer	*/
        [StringValue("BERATADD")]
        BERATADD,
        /*	Beschaffungstabelle	*/
        [StringValue("BESCH")]
        BESCH,
        /*	Beschaffungsposition	*/
        [StringValue("BESCHSL")]
        BESCHSL,
        /*	Beschaffungsstaffelposition	*/
        [StringValue("BESCHSLPOS")]
        BESCHSLPOS,
        /*	Bundesland	*/
        [StringValue("BL")]
        BL,
        /*	Bankleitzahlentabelle	*/
        [StringValue("BLZ")]
        BLZ,
        /*	BLZ Filialen	*/
        [StringValue("BLZFIL")]
        BLZFIL,
        /*	Bonitätspositionen	*/
        [StringValue("BONIPOS")]
        BONIPOS,
        /*	Bonitätstabelle	*/
        [StringValue("BONITAET")]
        BONITAET,
        /*	Bonustabelle	*/
        [StringValue("BONUS")]
        BONUS,
        /*	Branchen	*/
        [StringValue("BRANCHE")]
        BRANCHE,
        /*	Bestellwesen	*/
        [StringValue("BST")]
        BST,
        /*	Bestellpositionen	*/
        [StringValue("BSTPOS")]
        BSTPOS,
        /*	Budget	*/
        [StringValue("BUDGET")]
        BUDGET,
        /*	DEBITOR	*/
        [StringValue("DEBITOR")]
        DEBITOR,
        /*	Eingangsrechnungsbuch	*/
        [StringValue("ERBUCH")]
        ERBUCH,
        /*	Buchungen	*/
        [StringValue("FI")]
        FI,
        /*	Fibu-Workbox	*/
        [StringValue("FIBU")]
        FIBU,
        /*	FIBUAWT	*/
        [StringValue("FIBUAWT")]
        FIBUAWT,
       
        /*	Fibuawt-Level 1	*/
        [StringValue("FIBULVL1")]
        FIBULVL1,
        /*	FIBU-AWT Level 2	*/
        [StringValue("FIBULVL2")]
        FIBULVL2,
        /*	Fibu-Awt Level 3	*/
        [StringValue("FIBULVL3")]
        FIBULVL3,
        /*	Fibu-AWT Level 4	*/
        [StringValue("FIBULVL4")]
        FIBULVL4,
        /*	Fibu-Awt Level 5	*/
        [StringValue("FIBULVL5")]
        FIBULVL5,
        /*	FIBUNM	*/
        [StringValue("FIBUNM")]
        FIBUNM,
        /*	FIBUNM1	*/
        [StringValue("FIBUNM1")]
        FIBUNM1,
        /*	FIBUNM2	*/
        [StringValue("FIBUNM2")]
        FIBUNM2,
        /*	FIBUNM3	*/
        [StringValue("FIBUNM3")]
        FIBUNM3,
        /*	FIBUNM4	*/
        [StringValue("FIBUNM4")]
        FIBUNM4,
        /*	FIBUNM5	*/
        [StringValue("FIBUNM5")]
        FIBUNM5,
        /*	FIBUNM6	*/
        [StringValue("FIBUNM6")]
        FIBUNM6,
        /*	Journal	*/
        [StringValue("FIJOUR")]
        FIJOUR,
        /*	FIJOURH	*/
        [StringValue("FIJOURH")]
        FIJOURH,
        /*	FIJOURS	*/
        [StringValue("FIJOURS")]
        FIJOURS,
        /*	Finanzierung	*/
        [StringValue("FIN")]
        FIN,
        /*	Perioden	*/
        [StringValue("FIPERIOD")]
        FIPERIOD,
        /*	Verkehrszahlen Haben Journal jährlich	*/
        [StringValue("FIVKZHJ")]
        FIVKZHJ,
        /*	Verkehrszahlen Haben Journal monatlich	*/
        [StringValue("FIVKZHM")]
        FIVKZHM,
        /*	FIVKZNJ	*/
        [StringValue("FIVKZNJ")]
        FIVKZNJ,
        /*	Verkehrszahlen	*/
        [StringValue("FIVKZNM")]
        FIVKZNM,
        /*	Verkehrszahlen Soll Journal jährlich	*/
        [StringValue("FIVKZSJ")]
        FIVKZSJ,
        /*	Verkehrszahlen Soll Journal monatlich	*/
        [StringValue("FIVKZSM")]
        FIVKZSM,
        /*	Full-Service-Kalkulationstabelle	*/
        [StringValue("FSKALK")]
        FSKALK,
        /*	Fullservicekalkulationsposition	*/
        [StringValue("FSKALKPOS")]
        FSKALKPOS,
        /*	Full-Service Kalkulationstypen	*/
        [StringValue("FSKALKTYP")]
        FSKALKTYP,
        /*	FSTYP	*/
        [StringValue("FSTYP")]
        FSTYP,
        /*	Buchungstypen	*/
        [StringValue("FT")]
        FT,
        /*	Buchungstexte	*/
        [StringValue("FTTEXT")]
        FTTEXT,
        /*	Fahrzeugtyp	*/
        [StringValue("FZTYP")]
        FZTYP,
        /*	Genehmigung	*/
        [StringValue("GENEHM")]
        GENEHM,
        /*	persönliche Daten	*/
        [StringValue("HOBBY")]
        HOBBY,
        /*	Interessent	*/
        [StringValue("IT")]
        IT,
        /*	Kalkulation	*/
        [StringValue("KALK")]
        KALK,
        /*	KALKCHK	*/
        [StringValue("KALKCHK")]
        KALKCHK,
        /*	Kalkulationstypen	*/
        [StringValue("KALKTYP")]
        KALKTYP,
        /*	Kunde	*/
        [StringValue("KD")]
        KD,
        /*	Kontogruppe	*/
        [StringValue("KGRUPPE")]
        KGRUPPE,
        /*	Kontoinhaber	*/
        [StringValue("KI")]
        KI,
        /*	Kontokorrent	*/
        [StringValue("KK")]
        KK,
        /*	KK Abschluss	*/
        [StringValue("KKABSCHL")]
        KKABSCHL,
        /*	Kontonkorrentauszug	*/
        [StringValue("KKAUSZUG")]
        KKAUSZUG,
        /*	Individuelle Tilgungsstaffel	*/
        [StringValue("KKISL")]
        KKISL,
        /*	Positionen Individuelle Tilgungsstaffel	*/
        [StringValue("KKISLPOS")]
        KKISLPOS,
        /*	individuelle Zinsstaffel am KK	*/
        [StringValue("KKISLZINS")]
        KKISLZINS,
        /*	Positionen für individuelle Zinsstaffel am KK	*/
        [StringValue("KKISLZPOS")]
        KKISLZPOS,
        /*	KKK	*/
        [StringValue("KKK")]
        KKK,
        /*	KKKHZINS	*/
        [StringValue("KKKHZINS")]
        KKKHZINS,
        /*	Kontokorrentbewegungen	*/
        [StringValue("KKKONTO")]
        KKKONTO,
        /*	Kontokorrent Person	*/
        [StringValue("KKKP")]
        KKKP,
        /*	KKKSZINS	*/
        [StringValue("KKKSZINS")]
        KKKSZINS,
        /*	Tilgungsstaffeln	*/
        [StringValue("KKSL")]
        KKSL,
        /*	Tilgungsstaffelpositionen	*/
        [StringValue("KKSLPOS")]
        KKSLPOS,
        /*	Zinsstaffel HEK	*/
        [StringValue("KKSLZINS")]
        KKSLZINS,
        /*	Zinsstaffelpositionen HEK	*/
        [StringValue("KKSLZPOS")]
        KKSLZPOS,
        /*	Zins- Tilgungsstaffelkombination	*/
        [StringValue("KKTYPZT")]
        KKTYPZT,
        /*	weitere Kontotabellen	*/
        [StringValue("KONTO")]
        KONTO,
        /*	Kontoklassen	*/
        [StringValue("KONTOKL")]
        KONTOKL,
        /*	Konzerne	*/
        [StringValue("KONZERN")]
        KONZERN,
        /*	KOSTART	*/
        [StringValue("KOSTART")]
        KOSTART,
        /*	Kostenstellen	*/
        [StringValue("KOSTSTEL")]
        KOSTSTEL,
        /*	KOSTTRAE	*/
        [StringValue("KOSTTRAE")]
        KOSTTRAE,
        /*	KREDITOR	*/
        [StringValue("KREDITOR")]
        KREDITOR,
        /*	KSUMLAGE	*/
        [StringValue("KSUMLAGE")]
        KSUMLAGE,
        /*	Währungskurse	*/
        [StringValue("KURS")]
        KURS,
        /*	Währungskurstabelle	*/
        [StringValue("KURSTAB")]
        KURSTAB,
        /*	Länder	*/
        [StringValue("LAND")]
        LAND,
        /*	Mandantenübersichtstabelle	*/
        [StringValue("LSADD")]
        LSADD,
        /*	MANDAT	*/
        [StringValue("MANDAT")]
        MANDAT,
        /*	Marketingverwaltungstabelle	*/
        [StringValue("MARK")]
        MARK,
        /*	Marketingtabelle	*/
        [StringValue("MARKTAB")]
        MARKTAB,
        /*	MASS-Auflösungsläufe	*/
        [StringValue("MASS")]
        MASS,
        /*	Mass-Staffel	*/
        [StringValue("MASSL")]
        MASSL,
        /*	Mass Staffelposition	*/
        [StringValue("MASSLPOS")]
        MASSLPOS,
        /*	MASSOPTION	*/
        [StringValue("MASSOPTION")]
        MASSOPTION,
        /*	Masstypen	*/
        [StringValue("MASSTYP")]
        MASSTYP,
        /*	MASSTYPOS	*/
        [StringValue("MASSTYPOS")]
        MASSTYPOS,
        /*	Mehrwertsteuer	*/
        [StringValue("MWST")]
        MWST,
        /*	MWSTDATE	*/
        [StringValue("MWSTDATE")]
        MWSTDATE,
        /*	MYCALC	*/
        [StringValue("MYCALC")]
        MYCALC,
        /*	Nummerkreis	*/
        [StringValue("NK")]
        NK,
        /*	Nummernkreis	*/
        [StringValue("NKNUM")]
        NKNUM,
        /*	Nebenkonten	*/
        [StringValue("NKONTO")]
        NKONTO,
        /*	Buchungsartenstatistik Nebenkonten	*/
        [StringValue("NKONTOBA")]
        NKONTOBA,
        /*	Notiz	*/
        [StringValue("NOTIZ")]
        NOTIZ,
        /*	Objekte	*/
        [StringValue("OB")]
        OB,
        /*	Objektdepot Kfz-Brief	*/
        [StringValue("OBDEPOT")]
        OBDEPOT,
        /*	Objekthalter	*/
        [StringValue("OBHALTER")]
        OBHALTER,
        /*	OBHIST	*/
        [StringValue("OBHIST")]
        OBHIST,
        /*	Objekt-Zusatzinformationen	*/
        [StringValue("OBINI")]
        OBINI,
        /*	Objekt-Optionen	*/
        [StringValue("OBOPTION")]
        OBOPTION,
        /*	Objektpool	*/
        [StringValue("OBPOOL")]
        OBPOOL,
        /*	Rücknahmevereinbarungen	*/
        [StringValue("OBRNVER")]
        OBRNVER,
        /*	Objekttypen	*/
        [StringValue("OBTYP")]
        OBTYP,
        /*	Objektverwertung	*/
        [StringValue("OBVWRT")]
        OBVWRT,
        /*	Angebote der Obhjektverwertung	*/
        [StringValue("OBVWRTAN")]
        OBVWRTAN,
        /*	OPBUCH	*/
        [StringValue("OPBUCH")]
        OPBUCH,
        /*	Übersicht der Partnertabellen	*/
        [StringValue("PARTNER")]
        PARTNER,
        /*	Personenmahntabelle	*/
        [StringValue("PEMAHN")]
        PEMAHN,
        /*	Obligoübersicht	*/
        [StringValue("PEOBLIGO")]
        PEOBLIGO,
        /*	Optionale Felder	*/
        [StringValue("PEOPTION")]
        PEOPTION,
        /*	Buchhaltungsperioden	*/
        [StringValue("PERIOD")]
        PERIOD,
        /*	Vertriebsbaum	*/
        [StringValue("PEROLE")]
        PEROLE,
        /*	Personentabelle	*/
        [StringValue("PERSON")]
        PERSON,
        /*	Postleitzahlen	*/
        [StringValue("PLZ")]
        PLZ,
        /*	Zahlungsläufe	*/
        [StringValue("PMTB")]
        PMTB,
        /*	Zahlungslaufpositionen	*/
        [StringValue("PMTP")]
        PMTP,
        /*	Provisionstabelle	*/
        [StringValue("PROV")]
        PROV,
        /*	QUEUE	*/
        [StringValue("QUEUE")]
        QUEUE,
        /*	Rechnungsabgrenzungspositionen	*/
        [StringValue("RAP")]
        RAP,
        /*	Rekalkulationen	*/
        [StringValue("RECALC")]
        RECALC,
        /*	Rechnungen	*/
        [StringValue("RN")]
        RN,
        /*	RNMAHN	*/
        [StringValue("RNMAHN")]
        RNMAHN,
        /*	Rechnungspositionen	*/
        [StringValue("RNPOS")]
        RNPOS,
        /*	RNTILG	*/
        [StringValue("RNTILG")]
        RNTILG,
        /*	Rechnungstypen	*/
        [StringValue("RNTYP")]
        RNTYP,
        /*	Rechnungszahlungen	*/
        [StringValue("RNZAHL")]
        RNZAHL,
        /*	Zinsen aus Rechnung	*/
        [StringValue("RNZINS")]
        RNZINS,
        /*	Rahrmenvertragstabelle	*/
        [StringValue("RVT")]
        RVT,
        /*	Rahmenvertragspositionen	*/
        [StringValue("RVTPOS")]
        RVTPOS,
        /*	SAP	*/
        [StringValue("SAP")]
        SAP,
        /*	Schadenstabelle	*/
        [StringValue("SCHADEN")]
        SCHADEN,
        /*	Scheckumlaufverwaltung	*/
        [StringValue("SCHUMV")]
        SCHUMV,
        /*	SCHWACKE	*/
        [StringValue("SCHWACKE")]
        SCHWACKE,
        /*	Sicherstellung	*/
        [StringValue("SICHER")]
        SICHER,
        /*	SICHTYP	*/
        [StringValue("SICHTYP")]
        SICHTYP,
        /*	Sachkonten	*/
        [StringValue("SKONTO")]
        SKONTO,
        /*	Buchungsartenstatistik Sachkonten	*/
        [StringValue("SKONTOBA")]
        SKONTOBA,
        /*	SKONTOM	*/
        [StringValue("SKONTOM")]
        SKONTOM,
        /*	Staffeltyp	*/
        [StringValue("SLTYP")]
        SLTYP,
        /*	Sonstige Funktion einer Person	*/
        [StringValue("SONSTADD")]
        SONSTADD,
        /*	STAAT	*/
        [StringValue("STAAT")]
        STAAT,
        /*	Subventionstabelle	*/
        [StringValue("SUBV")]
        SUBV,
        /*	SUBVTYP	*/
        [StringValue("SUBVTYP")]
        SUBVTYP,
        /*	System	*/
        [StringValue("SYSTEM")]
        SYSTEM,
        /*	Vertragsarten	*/
        [StringValue("VART")]
        VART,
        /*	Vertragsarttabbel	*/
        [StringValue("VARTTAB")]
        VARTTAB,
        /*	gehört zu WFUSER	*/
        [StringValue("VAUSER")]
        VAUSER,
        /*	Verkäufer	*/
        [StringValue("VK")]
        VK,
        /*	Vertriebspartner	*/
        [StringValue("VP")]
        VP,
        /*	Vertriebspartnerausprägung	*/
        [StringValue("VPADD")]
        VPADD,
        /*	Vertriebspartnerfilialen	*/
        [StringValue("VPFIL")]
        VPFIL,
        /*	Vertriebspartnerfilialie	*/
        [StringValue("VPFILADD")]
        VPFILADD,
        /*	VSTYP	*/
        [StringValue("VSTYP")]
        VSTYP,
        /*	VERTRAG	*/
        [StringValue("VT")]
        VT,
        /*	Vertragskündigung	*/
        [StringValue("VTKUEND")]
        VTKUEND,
        /*	Mahndaten aus dem Vertrag	*/
        [StringValue("VTMAHN")]
        VTMAHN,
        /*	Lieferantenzuordnungen	*/
        [StringValue("VTOBHD")]
        VTOBHD,
        /*	VTOBLIGO	*/
        [StringValue("VTOBLIGO")]
        VTOBLIGO,
        /*	Objektsicherheiten	*/
        [StringValue("VTOBSICH")]
        VTOBSICH,
        /*	Objektstaffeln	*/
        [StringValue("VTOBSL")]
        VTOBSL,
        /*	Ratenstaffeln	*/
        [StringValue("VTOBSLPOS")]
        VTOBSLPOS,
        /*	Stückliste	*/
        [StringValue("VTOBSTL")]
        VTOBSTL,
        /*	Vermittlerzuordnung	*/
        [StringValue("VTOBVM")]
        VTOBVM,
        /*	Vertrags-Optionen	*/
        [StringValue("VTOPTION")]
        VTOPTION,
        /*	Vertragsabrechnung	*/
        [StringValue("VTR")]
        VTR,
        /*	Vertragsabrechungspositionen	*/
        [StringValue("VTRPOS")]
        VTRPOS,
        /*	Vertragsrückrechnung	*/
        [StringValue("VTRUEK")]
        VTRUEK,
        /*	Vertragstypen	*/
        [StringValue("VTTYP")]
        VTTYP,
        /*	Waehrungstabellle	*/
        [StringValue("WAEHRTAB")]
        WAEHRTAB,
        /*	Währungstabelle	*/
        [StringValue("WAEHRUNG")]
        WAEHRUNG,
        /*	WFEXEC Bestandssätze	*/
        [StringValue("WFEXEC")]
        WFEXEC,
        /*	Jobhistorie	*/
        [StringValue("WFJEXEC")]
        WFJEXEC,
        /*	WFJLVL1	*/
        [StringValue("WFJLVL1")]
        WFJLVL1,
        /*	WFJLVL2	*/
        [StringValue("WFJLVL2")]
        WFJLVL2,
        /*	Jobgrundlage	*/
        [StringValue("WFJOB")]
        WFJOB,
        /*	Joblogbuch	*/
        [StringValue("WFJOBLOG")]
        WFJOBLOG,
        /*	Logbuch	*/
        [StringValue("WFLOG")]
        WFLOG,
        /*	Memo	*/
        [StringValue("WFMMEMO")]
        WFMMEMO,
        /*	WFSYS	*/
        [StringValue("WFSYS")]
        WFSYS,
        /*	Zustandräume	*/
        [StringValue("WFTZUST")]
        WFTZUST,
        /*	Zustandsraumvariablen	*/
        [StringValue("WFTZVAR")]
        WFTZVAR,
        /*	Benutzer	*/
        [StringValue("WFUSER")]
        WFUSER,
        /*	Zustandsraum	*/
        [StringValue("WFZUST")]
        WFZUST,
        /*	ZAHLWEG	*/
        [StringValue("ZAHLWEG")]
        ZAHLWEG,
        /*	ZINSLZ	*/
        [StringValue("ZINSLZ")]
        ZINSLZ,
        /*	Zinstabelle	*/
        [StringValue("ZINSTAB")]
        ZINSTAB,
        /*	Zahlungsvereinbarungen	*/
        [StringValue("ZVB")]
        ZVB,

    }
}
