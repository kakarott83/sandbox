using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util;

namespace Cic.One.DTO
{
    /// <summary>
    /// Types of XPRO-Combobox-Lists for GUI-XPRO
    /// IMPORTANT: also update DDLKPPOSType (when no explicit bo-loading is implemented in XproInfoFactory) when using ddlkppos for the type
    /// </summary>
    public enum XproEntityType : int
    {
        /// <summary>
        /// Use xpro code as string
        /// </summary>
        [StringValue("STRING")]
        STRING = 0,

        /// <summary>
        /// AUSLAUSWEIS
        /// </summary>
        [StringValue("AUSLAUSWEIS")]
        AUSLAUSWEIS = 1,

        /// <summary>
        /// UNTERSTZART
        /// </summary>
        [StringValue("UNTERSTZART")]
        UNTERSTZART,

        /// <summary>
        /// NATIONALITAETEN
        /// </summary>
        [StringValue("NATIONALITAETEN")]
        NATIONALITAETEN,

        /// <summary>
        /// TREIBSTOFF
        /// </summary>
        [StringValue("TREIBSTOFF")]
        TREIBSTOFF,

        /// <summary>
        /// ANREDEN
        /// </summary>
        [StringValue("ANREDEN")]
        ANREDEN,

        /// <summary>
        /// RUECKZAHLART
        /// </summary>
        [StringValue("RUECKZAHLART")]
        RUECKZAHLART,

        /// <summary>
        /// ERREICHBAR
        /// </summary>
        [StringValue("ERREICHBAR")]
        ERREICHBAR,

        /// <summary>
        /// ABLEHNGRUND
        /// </summary>
        [StringValue("ABLEHNGRUND")]
        ABLEHNGRUND,

        /// <summary>
        /// ZUSEINKART
        /// </summary>
        [StringValue("ZUSEINKART")]
        ZUSEINKART,

        /// <summary>
        /// BERUFAUSLART
        /// </summary>
        [StringValue("BERUFAUSLART")]
        BERUFAUSLART,

        /// <summary>
        /// CSEINHEIT
        /// </summary>

        [StringValue("CSEINHEIT")]
        CSEINHEIT,

        /// <summary>
        /// AUSZAHLART
        /// </summary>
        [StringValue("AUSZAHLART")]
        AUSZAHLART,

        /// <summary>
        /// EINKART
        /// </summary>
        [StringValue("EINKART")]
        EINKART,

        /// <summary>
        /// REGELMAUSLART
        /// </summary>
        [StringValue("REGELMAUSLART")]
        REGELMAUSLART,

        /// <summary>
        /// ZIVILSTAENDE
        /// </summary>
        [StringValue("ZIVILSTAENDE")]
        ZIVILSTAENDE,

        /// <summary>
        /// LENKER
        /// </summary>
        [StringValue("LENKER")]
        LENKER,

        /// <summary>
        /// GETRIEBEART
        /// </summary>
        [StringValue("GETRIEBEART")]
        GETRIEBEART,

        /// <summary>
        /// FAMILIENSTAND
        /// </summary>
        [StringValue("FAMILIENSTAND")]
        FAMILIENSTAND,

        /// <summary>
        /// STATUSMITANT
        /// </summary>
        [StringValue("STATUSMITANT")]
        STATUSMITANT,

        /// <summary>
        /// WOHNSITUATIONEN
        /// </summary>
        [StringValue("WOHNSITUATIONEN")]
        WOHNSITUATIONEN,

        /// <summary>
        /// BERUFLICHESIT
        /// </summary>
        [StringValue("BERUFLICHESIT")]
        BERUFLICHESIT,

        /// <summary>
        /// VERWENDUNG
        /// </summary>
        [StringValue("VERWENDUNG")]
        VERWENDUNG,

        /// <summary>
        /// RECHTSFORMEN
        /// </summary>
        [StringValue("RECHTSFORMEN")]
        RECHTSFORMEN,

        /// <summary>
        /// AUFBAU
        /// </summary>
        [StringValue("AUFBAU")]
        AUFBAU,

        /// <summary>
        /// AUFBAUCODE
        /// </summary>
        [StringValue("AUFBAUCODE")]
        AUFBAUCODE,

        /// <summary>
        /// DV LEGALFORM
        /// </summary>   
        [StringValue("DV LEGALFORM")]
        DVLEGALFORM,

        /// <summary>
        /// GETRIEBEARTCODE
        /// </summary>
        [StringValue("GETRIEBEARTCODE")]
        GETRIEBEARTCODE,

		/// <summary>
		/// SLAPAUSECODE
		/// </summary>
		[StringValue ("SLAPAUSECODE")]
		SLAPAUSECODE,

		/// <summary>
        /// TREIBSTOFFCODE
        /// </summary>
        [StringValue("TREIBSTOFFCODE")]
        TREIBSTOFFCODE,

        /// <summary>
        /// WEITEREAUSLAGEN
        /// </summary>
        [StringValue("WEITEREAUSLAGEN")]
        WEITEREAUSLAGEN,

        /// <summary>
        /// ACCOUNTS
        /// </summary>
        [StringValue("ACCOUNTS")]
        ACCOUNTS,

        /// <summary>
        /// WKTACCOUNTS
        /// </summary>
        [StringValue("WKTACCOUNTS")]
        WKTACCOUNTS,

        /// <summary>
        /// CAMP
        /// </summary>
        [StringValue("CAMP")]
        CAMP,

        /// <summary>
        /// Attribut Antrag/Angebot
        /// </summary>
        [StringValue("ANGANTATTRIBUT")]
        ANGANTATTRIBUT,

        /// <summary>
        /// ANTRAGZUSTAND
        /// </summary>
        [StringValue("ANTRAGZUSTAND")]
        ANTRAGZUSTAND,

        /// <summary>
        /// Vertrag Zustand
        /// </summary>
        [StringValue("VTZUSTAND")]
        VTZUSTAND,

        /// <summary>
        /// Attribut
        /// </summary>
        [StringValue("ATTRIBUT")]
        ATTRIBUT,

        /// <summary>
        /// Geschäftsart
        /// </summary>
        [StringValue("GESCHAEFTSART")]
        GESCHAEFTSART,

        /// <summary>
        /// OPPORTUNITY
        /// </summary>
        [StringValue("OPPORTUNITY")]
        OPPORTUNITY,



        OPPO_STATUS,

        OPPO_TYP,

        OPPO_PHASE,


        OPPO_GRUND,

        ACCOUNT_PRIVAT,

        ACCOUNT_GESELLSCHAFT,

        CONTACT_WAY,

        CONTACT_DIRECTION,

        /// <summary>
        /// Rolle des Benutzers im Prozess-Kontext
        /// </summary>
        BPROLE,
        /// <summary>
        /// Benutzer mit Rollen im Prozess-Kontext
        /// </summary>
        BPWFUSER,

        /// <summary>
        /// WFUSER
        /// </summary>
        [StringValue("WFUSER")]
        WFUSER,

        /// <summary>
        /// PUSER
        /// </summary>
        [StringValue("PUSER")]
        PUSER,

        /// <summary>
        /// FILEATT_TYPCODE
        /// </summary>
        [StringValue("FILEATT_TYPCODE")]
        FILEATT_TYPCODE,

        CAMP_STATUS,

        CONTACT_PLACE,
        CONTACT_REASON,

        PERSON_ANREDECODE,
        PERSON_ERREICHBTEL,
        PERSON_GESFLAG,
        PERSON_PRIVATFLAG,
        PERSON_REFOCODE,
        PERSON_TITELCODE,
        PTRELATE_FUNCCODE,
        PTRELATE_ADDINFO,
        PTRELATE_TYPCODE,

        CTLANG,
        LAND,
        BRANCHE,
        ADRTP,

        /// <summary>
        /// Die bei der ZEK-Anfrage erwarteten Zahlenwerte für die Rechtsform einer Firma
        /// </summary>
        RECHTSFORMCODE,
        /// <summary>
        /// Die bei der ZEK-Anfrage erwarteten Zahlenwerte für IKO-pflichtig / ...
        /// </summary>
        ZEKHERKUNFT,
        /// <summary>
        /// Die bei der ZEK-Anfrage erwarteten Zahlenwerte für den Grund der Anfrage
        /// </summary>
        ANFRAGEGRUND,
        /// <summary>
        /// Die bei der ZEK-Anfrage erwarteten Zahlenwerte für das Geschlecht einer Person
        /// </summary>
        ZEK_GESCHLECHT,

        /// <summary>
        /// Liefert alle Kategorien von Exchange.
        /// </summary>
        EXCHANGE_CATEGORIES,

        /// <summary>
        /// Liefert alle Kontakte die den Suchkriterien entwprechen.
        /// </summary>
        EXCHANGE_CONTACTS,

        /// <summary>
        /// Liefert eine Liste aller Kontotypen
        /// </summary>
        KONTO_TYP,

        /// <summary>
        /// Liefert eine Liste aller Banken anhand der BLZ
        /// </summary>
        KONTO_BLZ,

        /// <summary>
        /// Liefert eine Liste aller Banken anhand der BLZ
        /// </summary>
        BANK_BLZ,


        /// <summary>
        /// Liefert eine Liste aller Banken anhand der BLZ
        /// </summary>
        BLZ_BIC,


        /// <summary>
        /// PARTNER
        /// </summary>
        [StringValue("PARTNER")]
        PARTNER,

        /// <summary>
        /// Follow
        /// </summary>
        [StringValue("FOLLOW_TYPE")]
        FOLLOW_TYPE,

        /// <summary>
        /// Typ eines Kontakts
        /// </summary>
        CONTACT_TYP,

        BESUCHSBERICHT_TYP,
        JANEIN,
        BESUCHSBERICHT_MARKTSTELLUNG,
        BESUCHSBERICHT_INVESTTYP,
        BESUCHSBERICHT_EIGENTUEMERTYP,
        BESUCHSBERICHT_GFQUALIFIKATION,
        BESUCHSBERICHT_AFATYP,
        BESUCHSBERICHT_GESCHAEFTSTYP,
        BESUCHSBERICHT_PRIVATGEWERBLICH,
        BESUCHSBERICHT_KONDITIONEN,
        BESUCHSBERICHT_MVORIGINALANTRAG,

        /// <summary>
        /// BETEILIGTER
        /// </summary>
        [StringValue("BETEILIGTER")]
        BETEILIGTER,

        /// <summary>
        /// CRMNM_ADDINFO
        /// </summary>
        [StringValue("CRMNM_ADDINFO")]
        CRMNM_ADDINFO,

        /// <summary>
        /// CRMNM_TYPCODE
        /// </summary>
        [StringValue("CRMNM_TYPCODE")]
        CRMNM_TYPCODE,

        /// <summary>
        /// CAMP_TYP
        /// </summary>
        [StringValue("CAMP_TYP")]
        CAMP_TYP,

        /// <summary>
        /// NKONTO
        /// </summary>
        [StringValue("NKONTO")]
        NKONTO,

        /// <summary>
        /// PRPRODUCT
        /// </summary>
        [StringValue("PRPRODUCT")]
        PRPRODUCT,

        /// <summary>
        /// PRINTSET
        /// </summary>
        [StringValue("PRINTSET")]
        PRINTSET,

        /// <summary>
        /// PRTLGSET
        /// </summary>
        [StringValue("PRTLGSET")]
        PRTLGSET,

        /// <summary>
        /// WAEHRUNG
        /// </summary>
        [StringValue("WAEHRUNG")]
        WAEHRUNG,

        /// <summary>
        /// OBKAT
        /// </summary>
        [StringValue("OBKAT")]
        OBKAT,

        /// <summary>
        /// Finanzierung/Kontoführung/Zinstermine
        /// </summary>
        [StringValue("NKKTYP_PPY")]
        NKKTYP_PPY,

        /// <summary>
        /// Finanzierung/Kontoführung/Zinsusance
        /// </summary>
        [StringValue("NKKTYP_DCC")]
        NKKTYP_DCC,

        /// <summary>
        /// Finanzierung/Kontoführung/Feiertagskorrektur
        /// </summary>
        [StringValue("NKKTYP_BDC")]
        NKKTYP_BDC,

        /// <summary>
        /// OBTYP
        /// </summary>
        [StringValue("OBTYP")]
        OBTYP,

        /// <summary>
        /// RAHMEN
        /// </summary>
        [StringValue("RAHMEN")]
        RAHMEN,

        /// <summary>
        /// ZINSTAB
        /// </summary>
        [StringValue("ZINSTAB")]
        ZINSTAB,

        /// <summary>
        /// KALKTYP
        /// </summary>
        [StringValue("KALKTYP")]
        KALKTYP,

        /// <summary>
        /// OBART
        /// </summary>
        [StringValue("OBART")]
        OBART,

        /// <summary>
        /// Aussendienstmitarbeiter
        /// </summary>
        [StringValue("ADMADD")]
        ADMADD,

        /// <summary>
        /// KONSTELLATION
        /// </summary>
        [StringValue("KONSTELLATION")]
        KONSTELLATION,

        /// <summary>
        /// OPTKUNDENINFO
        /// </summary>
        [StringValue("OPTKUNDENINFO")]
        OPTKUNDENINFO,

        /// <summary>
        /// ETGADDITION
        /// </summary>
        [StringValue("ETGADDITION")]
        ETGADDITION,

        /// <summary>
        /// VSTYP
        /// </summary>
        [StringValue("VSTYP")]
        VSTYP,

        /// <summary>
        /// BSIPAKET
        /// </summary>
        [StringValue("BSIPAKET")]
        BSIPAKET,

        /// <summary>
        /// FSTYP
        /// </summary>
        [StringValue("FSTYP")]
        FSTYP,

        /// <summary>
        /// VERSICHERUNG
        /// </summary>
        [StringValue("VERSICHERUNG")]
        VERSICHERUNG,

        /// <summary>
        /// VART
        /// </summary>
        [StringValue("VART")]
        VART,

        /// <summary>
        /// WECHSELRHYTHMUS Reifendropdown
        /// </summary>
        [StringValue("WECHSELRHYTHMUS")]
        WECHSELRHYTHMUS,

        /// <summary>
        /// Typ einer Aufgabe
        /// </summary>
        PTASK_TYP,

        /// <summary>
        /// INTERESSENT
        /// </summary>
        [StringValue("IT")]
        IT,

        /// <summary>
        /// ENDOFTERM Service Request Status
        /// </summary>
        [StringValue("OPPO_EOTSRSTATUS")]
        OPPO_EOTSRSTATUS,
        /// <summary>
        /// ENDOFTERM Ergebnis aktueller Vertrag
        /// </summary>
        [StringValue("OPPO_EOTRESULTA")]
        OPPO_EOTRESULTA,
        /// <summary>
        /// ENDOFTERM Ergebnis Folgevertrag
        /// </summary>
        [StringValue("EOTRESULTF")]
        OPPO_EOTRESULTF,
        /// <summary>
        /// Aktivitätsstatus
        /// </summary>
        [StringValue("OPPOTASK_STATE")]
        OPPOTASK_STATE,
        /// <summary>
        /// Aktivitätstyp
        /// </summary>
        [StringValue("OPPOTASK_TYPE")]
        OPPOTASK_TYPE,

        /// <summary>
        /// Aktivitätstyp für Kampagne
        /// </summary>
        [StringValue("OPPOTASK_TYPE_CAMP")]
        OPPOTASK_TYPE_CAMP,

        /// <summary>
        /// Oppo Ergebnis
        /// </summary>
        [StringValue("OPPO_RESULT")]
        OPPO_RESULT,
        /// <summary>
        /// Oppo Agenturergebnis
        /// </summary>
        [StringValue("OPPO_AGRESULT")]
        OPPO_AGRESULT,

        /// <summary>
        /// Händlergebiet
        /// </summary>
        [StringValue("HAENDLER_GEBIET")]
        HAENDLER_GEBIET,//select distinct trim(ort) from person, perole where perole.sysperson=person.sysperson and perole.SYSROLETYPE=6 order by trim(ort);

        /// <summary>
        /// Phasen grün orange rote
        /// </summary>
        [StringValue("OPPOTASK_PHASE")]
        OPPOTASK_PHASE,

        /// <summary>
        /// Händler, aufgelöste Händlergruppen
        /// </summary>
        [StringValue("HAENDLER")]
        HAENDLER,

        /// <summary>
        /// Vertriebsmitarbeiter (vendors in the same or deeper level as the current logged in role)
        /// </summary>
        [StringValue("VERTRIEBSMA")]
        VERTRIEBSMA,

        /// <summary>
        /// Zinstyp
        /// </summary>
        [StringValue("INTTYPE")]
        INTTYPE,

        /// <summary>
        /// MANDANT
        /// </summary>
        [StringValue("MANDANT")]
        MANDANT,

        /// <summary>
        /// VERTRIEBSWEG
        /// </summary>
        [StringValue("VERTRIEBSWEG")]
        VERTRIEBSWEG,

        /// <summary>
        /// TARIFKZ
        /// </summary>
        [StringValue("TARIFKZ")]
        TARIFKZ,

        /// <summary>
        /// MODELL
        /// </summary>
        [StringValue("MODELL")]
        MODELL,

        /// <summary>
        /// FZART
        /// </summary>
        [StringValue("FZART")]
        FZART,

        /// <summary>
        /// MODELLCODE
        /// </summary>
        [StringValue("MODELLCODE")]
        MODELLCODE,

        /// <summary>
        /// VTSTATUS
        /// </summary>
        [StringValue("VTSTATUS")]
        VTSTATUS,
        /// <summary>
        /// HAENDLER_NUMMER
        /// </summary>
        [StringValue("HAENDLER_NUMMER")]
        HAENDLER_NUMMER,

        /// <summary>
        /// VT.VART
        /// </summary>
        [StringValue("VTVART")]
        VTVART,

        /// <summary>
        /// OBINVENTAR
        /// </summary>
        [StringValue("OBINVENTAR")]
        OBINVENTAR,


        /// <summary>
        /// BAUREIHE
        /// </summary>
        [StringValue("BAUREIHE")]
        BAUREIHE,

        /// <summary>
        /// MARKE
        /// </summary>
        [StringValue("MARKE")]
        MARKE,

        /// <summary>
        /// VERTRIEBSEBENE
        /// </summary>
        [StringValue("VERTRIEBSEBENE")]
        VERTRIEBSEBENE,

        /// <summary>
        /// VERTRIEBSEBENEVK
        /// </summary>
        [StringValue("VERTRIEBSEBENEVK")]
        VERTRIEBSEBENEVK,

        /// <summary>
        /// ANGEBOTE
        /// </summary>
        [StringValue("ANGEBOTE")]
        ANGEBOTE,

        /// <summary>
        /// ANGEBOTE
        /// </summary>
        [StringValue("ANTRAEGE")]
        ANTRAEGE,

        /// <summary>
        /// FAHRER
        /// </summary>
        [StringValue("FAHRER")]
        FAHRER,

        /// <summary>
        /// LIEFERANT
        /// </summary>
        [StringValue("LIEFERANT")]
        LIEFERANT,

        /// <summary>
        /// STAAT
        /// </summary>
        [StringValue("STAAT")]
        STAAT,

        /// <summary>
        /// KDTYP
        /// </summary>
        [StringValue("KDTYP")]
        KDTYP,

        /// <summary>
        /// VT.SYSVTTYP
        /// </summary>
        [StringValue("VTTYP")]
        VTTYP,

        /// <summary>
        /// Erfassungskanal
        /// </summary>
        [StringValue("ERFASSUNGSKANAL")]
        ERFASSUNGSKANAL,

        [StringValue("PLZ")]
        PLZ,


        /// <summary>
        /// Legitimationssmethode
        /// </summary>
        [StringValue("LEGITIMATION_METHODE")]
        LEGITIMATION_METHODE,

        /// <summary>
        /// Memo Kategorie
        /// </summary>
        [StringValue("WFMMKAT")]
        WFMMKAT,

        /// <summary>
        /// Memo Kategorie
        /// </summary>
        [StringValue("WFMMKATANGEBOT")]
        WFMMKATANGEBOT,

        /// <summary>
        /// Memo Kategorie
        /// </summary>
        [StringValue("WFMMKATANTRAG")]
        WFMMKATANTRAG,

        /// <summary>
        /// Memo Kategorie Für Vertrag
        /// </summary>
        [StringValue("WFMMKATVT")]
        WFMMKATVT,

        /// <summary>
        /// Status
        /// </summary>
        [StringValue("ZEKANTRAG")]
        ZEKANTRAG,

        /// <summary>
        /// Branchen
        /// </summary>
        [StringValue("NOGA08")]
        NOGA08,

        /// <summary>
        /// Zielverein for ZEK requests
        /// </summary>
        [StringValue("ZEKZIELVEREIN")]
        ZEKZIELVEREIN,

        /// <summary>
        /// Debitor Role for ZEK requests
        /// </summary>
        ZEKDEBTROLE,

        /// <summary>
        /// Vertragsstatus for ZEK requests
        /// </summary>
        ZEKVERTRAG,

        /// <summary>
        /// ZEK Bonicode for ZEK requests
        /// </summary>
        ZEK_IKO_BONICODE,

        /// <summary>
        /// Ecode Status for ZEK requests
        /// </summary>
        ECODE178STATUS,

        /// <summary>
        /// Sicherstellungs Status for ZEK requests
        /// </summary>
        ZEK_SICHERSTELLUNG,
		
		 /// <summary>
        /// Vertragsstatus for ZEK requests
        /// </summary>
        ZEK_VERTRAGSSTATUS,

        /// <summary>
        /// Strasse
        /// </summary>
        STRASSE,

        /// <summary>
        /// Kartentyp für ZEK
        /// </summary>
        ZEKKARTENTYCODE,

        /// <summary>
        /// Ereigniscode für ZEK
        /// </summary>
        ZEKEREIGNISCODE,


        /// <summary>
        /// SEG
        /// </summary>
        [StringValue("SEG")]
        SEG,

        
        /// <summary>
        /// KATEGORIE
        /// </summary>
        [StringValue("SEGPOS")]
        SEGPOS


    }
}