using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// DDLKPPOSType
    /// </summary>
    public enum DDLKPPOSType
    {
        /// <summary>
        /// AUSLAUSWEIS
        /// </summary>
        [StringValue("AUSLAUSWEIS")]
        AUSLAUSWEIS,

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


        //CRM-Types-------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Campaign Status
        /// </summary>
        [StringValue("CAMP_STATUS")]
        CAMP_STATUS,
        /// <summary>
        /// Kontakt Richtung
        /// </summary>
        [StringValue("CONTACT_DIRECTION")]
        CONTACT_DIRECTION,
        /// <summary>
        /// Kontakt Ort
        /// </summary>
        [StringValue("CONTACT_PLACE")]
        CONTACT_PLACE,
        /// <summary>
        /// Kontakt Grund
        /// </summary>
        [StringValue("CONTACT_REASON")]
        CONTACT_REASON,
        /// <summary>
        /// Kontakt Weg
        /// </summary>
        [StringValue("CONTACT_WAY")]
        CONTACT_WAY,
        /// <summary>
        /// Opportunity Grund
        /// </summary>
        [StringValue("OPPO_GRUND")]
        OPPO_GRUND,
        /// <summary>
        /// Opportunity Phase
        /// </summary>
        [StringValue("OPPO_PHASE")]
        OPPO_PHASE ,
        /// <summary>
        /// Opportunity Status
        /// </summary>
        [StringValue("OPPO_STATUS")]
        OPPO_STATUS,
        /// <summary>
        /// Person Anrede
        /// </summary>
        [StringValue("PERSON_ANREDECODE")]
        PERSON_ANREDECODE,
        /// <summary>
        /// Person Telefonarten
        /// </summary>
        [StringValue("PERSON_ERREICHBTEL")]
        PERSON_ERREICHBTEL,
        /// <summary>
        /// Person Gesellschaften
        /// </summary>
        [StringValue("PERSON_GESFLAG")]
        PERSON_GESFLAG,
        /// <summary>
        /// Person Privat
        /// </summary>
        [StringValue("PERSON_PRIVATFLAG")]
        PERSON_PRIVATFLAG,
        /// <summary>
        /// Person Rechtsformen
        /// </summary>
        [StringValue("PERSON_REFOCODE")]
        PERSON_REFOCODE,
        /// <summary>
        /// Person Titel
        /// </summary>
        [StringValue("PERSON_TITELCODE")]
        PERSON_TITELCODE,
        /// <summary>
        /// Beziehung Funktionen
        /// </summary>
        [StringValue("PTRELATE_FUNCCODE")]
        PTRELATE_FUNCCODE,
        /// <summary>
        /// Beziehung Typen
        /// </summary>
        [StringValue("PTRELATE_TYPCODE")]
        PTRELATE_TYPCODE,
        /// <summary>
        /// Follow Typen
        /// </summary>
        [StringValue("FOLLOW_TYPE")]
        FOLLOW_TYPE,        
        /// <summary>
        /// Beziehung erweiterte Infos
        /// </summary>
        [StringValue("PTRELATE_ADDINFO")]
        PTRELATE_ADDINFO,
        /// <summary>
        /// Beteiligte erweiterte Infos
        /// </summary>
        [StringValue("CRMNM_ADDINFO")]
        CRMNM_ADDINFO,
        /// <summary>
        /// Datei Typen
        /// </summary>
        [StringValue("FILEATT_TYPCODE")]
        FILEATT_TYPCODE,
        /// <summary>
        /// Beziehung Typen
        /// </summary>
        [StringValue("CRMNM_TYPCODE")]
        CRMNM_TYPCODE,

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
        [StringValue("OPPO_EOTRESULTF")]
        OPPO_EOTRESULTF,
        /// <summary>
        /// Aktivitätsstatus
        /// </summary>
        [StringValue("OPPOTASK_STATE")]
        OPPOTASK_STATE,
        /// <summary>
        /// Aktivitätstyp EOT
        /// </summary>
        [StringValue("OPPOTASK_TYPE")]
        OPPOTASK_TYPE,

        /// <summary>
        /// Aktivitätstyp CAMP
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
        /// Phasen grün orange rote
        /// </summary>
        [StringValue("OPPOTASK_PHASE")]
        OPPOTASK_PHASE,

        /// <summary>
        /// company legal forms for zek request
        /// </summary>
        [StringValue("RECHTSFORMCODE")]
        RECHTSFORMCODE,
        /// <summary>
        /// targets for zek (ZEK, ZEK+IKO)
        /// </summary>
        [StringValue("ZEKHERKUNFT")]
        ZEKHERKUNFT,
        /// <summary>
        /// reasons of the zek request
        /// </summary>
        [StringValue("ANFRAGEGRUND")]
        ANFRAGEGRUND,
        /// <summary>
        /// sexes in zek format
        /// </summary>
        [StringValue("ZEK_GESCHLECHT")]
        ZEK_GESCHLECHT,

        /// <summary>
        /// Erfassungskanal
        /// </summary>
        [StringValue("ERFASSUNGSKANAL")]
        ERFASSUNGSKANAL,
        
        /// <summary>
        /// Legitimationsmethode
        /// </summary>
        [StringValue("LEGITIMATION_METHODE")]
        LEGITIMATION_METHODE,

        /// <summary>
        /// Geschäftsart
        /// </summary>
        [StringValue("GESCHAEFTSART")]
        GESCHAEFTSART,

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
        /// Zielverein für ZEK Anfragen
        /// </summary>
        [StringValue("ZEKZIELVEREIN")]
        ZEKZIELVEREIN,

        /// <summary>
        /// Debitor Role for ZEK requests
        /// </summary>
        [StringValue("ZEKDEBTROLE")]
        ZEKDEBTROLE,

        /// <summary>
        /// Vertragsstatus for ZEK requests
        /// </summary>
        [StringValue("ZEKVERTRAG")]
        ZEKVERTRAG,

        /// <summary>
        /// ZEK Bonicode for ZEK requests
        /// </summary>
        [StringValue("ZEK_IKO_BONICODE")]
        ZEK_IKO_BONICODE,
		
		 /// <summary>
        /// Vertragsstatus for ZEK requests
        /// </summary>
        [StringValue("ZEK VERTRAGSSTATUS")]
        ZEK_VERTRAGSSTATUS
    }
}
