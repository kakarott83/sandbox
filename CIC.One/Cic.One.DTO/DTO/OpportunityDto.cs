using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    /// <summary>
    /// Verkaufschance, Gelegenheit
    /// 
    /// </summary>
    public class OpportunityDto : EntityDto
    {
        /*		*/
        public long sysOppo { get; set; }
        /*		*/
        public long sysOppoTp { get; set; }
        /*		*/
        public String oppoTpBezeichnung { get; set; }
        /*	Initiator	*/
        public long sysPerson { get; set; }

        /* Ansprechpartner */
        public long sysPartner { get; set; }

        /// <summary>
        /// Name der initiierenden Person
        /// </summary>
        public String personName { get; set; }
        /*		*/
        public long sysCamp { get; set; }
        /*	Inhaber	*/
        public long sysOwner { get; set; }
        /*	Privat	*/
        public int privateFlag { get; set; }
        /*	Opportunity	*/
        public String name { get; set; }
        /*	Beschreibung	*/
        public String description { get; set; }
        /*	Aktiv	*/
        public int activeFlag { get; set; }
        /*	Beginn	*/
        public DateTime? validFrom { get; set; }
        /*	Ende	*/
        public DateTime? validUntil { get; set; }
        /// <summary>
        /// Phase
        /// grün, gelb, rot
        /// </summary>
        public int phase { get; set; }
        /// <summary>
        /// Status
        /// DDLKPPOS<->OPPOSTATUS
        /// DDLKPPOS<->EOTSRSTATUS
        /// </summary>
        public int status { get; set; }
        /*	Grund	*/
        public int grund { get; set; }
        /*	Erwarteter Umsatz	*/
        public double expRevenue { get; set; }
        /*	Wahrscheinlichkeit	*/
        public int probRevenue { get; set; }
        /*	Nächste Aktion	*/
        public String nextStep { get; set; }

        /* Endkunde */
        public long sysPersonKd { get; set; }
        /* Ansprechpartner Endkunde */
        public long sysPartnerKd { get; set; }

        /*Kommentar Agentur
        Kommentar Bankmitarbeiter */
        public String notiz { get; set; }
        /* Kommentar Handelsmitarbeiter */
        public String notiz2 { get; set; }

        /// <summary>
        /// Ergebnis
        /// DDLKPPOS<->OPPORESULT
        /// DDLKPPOS<->EOTRESULTA
        /// </summary>
        public int resultat { get; set; }
        /// <summary>
        /// Ergebnis Folge-Gebiet
        /// DDLKPPOS<->EOTRESULTF
        /// </summary>
        public int nextresult { get; set; }
        /// <summary>
        /// Agenturergebnis
        /// </summary>
        public int extresultat { get; set; }
        /// <summary>
        /// Agentur vorqualifiziert
        /// </summary>
        public int extqualflag { get; set; }
        /// <summary>
        /// Händler
        /// </summary>
        public long sysPersonHd { get; set; }
        /// <summary>
        /// Folge Gebiets-ID
        /// </summary>
        public long nextSysid { get; set; }
        /// <summary>
        /// Gebiets-ID
        /// </summary>
        public long sysid { get; set; }
        /// <summary>
        /// Interaktionsmuster
        /// </summary>
        public long sysiam { get; set; }
        /// <summary>
        /// Bearbeiter
        /// </summary>
        public long syschguser { get; set; }
        /// <summary>
        /// Verkäufer
        /// </summary>
        public long syswfuser { get; set; }

        /// <summary>
        /// Anschreiben ja/nein
        /// </summary>
        public int letterflag { get; set; }
        /// <summary>
        /// Anrufen ja/nein
        /// </summary>
        public int telefonflag { get; set; }
        /// <summary>
        /// Geändert Flag
        /// </summary>
        public int changedflag { get; set; }
        /// <summary>
        /// Folgegebiet
        /// </summary>
        public String nextarea { get; set; }
        /// <summary>
        /// Gebiet
        /// </summary>
        public String area { get; set; }

        /// <summary>
        /// Mail geschäftlich
        /// </summary>
        public String email { get; set; }
        /// <summary>
        /// Mail privat
        /// </summary>
        public String email2 { get; set; }
        /// <summary>
        /// Mobil geschäftlich
        /// </summary>
        public String handy { get; set; }
        /// <summary>
        /// Mobil privat
        /// </summary>
        public String handy2 { get; set; }
        /// <summary>
        /// Telefon geschäftlich
        /// </summary>
        public String telefon { get; set; }
        /// <summary>
        /// Telefon privat
        /// </summary>
        public String ptelefon { get; set; }

        /// <summary>
        /// Erstelldatum
        /// </summary>
        public DateTime crtdate { get; set; }

        /// <summary>
        /// BMW-AIDA2 relevante Zusatzinformationen aus einer Subquery
        /// </summary>
        public String wfuserName { get; set; }
        public String kdName { get; set; }
        public String gebietName { get; set; }
        public String vkName { get; set; }
        public String hdName { get; set; }
        public String hdOrt { get; set; }
        public DateTime? ende { get; set; }
        public DateTime? erstzul { get; set; }
        public String vart { get; set; }
        public String marke { get; set; }
        public String modell { get; set; }
        public String kennzeichen { get; set; }
        public String serie { get; set; }
        public String campName { get; set; }
        public String hdCode { get; set; }
        public String baureihe { get; set; }

        /// <summary>
        /// schlechtester Aktivity-Status
        /// </summary>
        public int worstActivityStatus { get; set; }
        /// <summary>
        /// niedrigster Aktivity-Status
        /// </summary>
        public int bestActivityStatus { get; set; }
        /// <summary>
        /// Anzahl von Aktivitäten
        /// </summary>
        public int activityCount { get; set; }
        /// <summary>
        /// 1 wenn syschguser ein interner Mitarbeiter ist
        /// </summary>
        public int ownershipsf { get; set; }

        /// <summary>
        /// Oppo-Id der verknüpften Area
        /// </summary>
        public long sysOppoArea { get; set; }

        //---------fields used for csv-export single sql query
        public int kundeprivat { get; set; }//kd.privatflag
        public String schwacke { get; set; }
        public DateTime? vtbeginn { get; set; }
        public DateTime? vtende { get; set; }
        public String kundetitel { get; set; }
        public String kundevorname { get; set; }
        public String vertriebsweg { get; set; }
        public String konstellation { get; set; }
        public String mandant { get; set; }//vt.sysls==lsadd.syslsadd -> lsadd.mandant
        public String fahrer { get; set; }//ob.fahrer==Neuwagen/Gebrauchtwagen
        public String fzart { get; set; }//ob.fzart==neufahrzeug, gebraucht, vorführfahrzeug
        public String kdart { get; set; }//kd.rechtsform
        //---------------
        //Link to AIDA IT from person
        public int syskdtypit { get; set; }//it.syskdtyp
        public int sysit { get; set; }//it.sysit 
        //--------------

        /// <summary>
        /// Art der zugehörigen Oppo
        /// </summary>
        public String iamcode { get; set; }

        public double eurotaxblau { get; set; }
        public double eurotaxgelb { get; set; }

        public AccountDto kunde { get; set; }
        public VertragDto vertrag { get; set; }
        public KalkDto kalk { get; set; }

        public String kdstrasse { get; set; }
        public String kdplz { get; set; }
        public String kdort { get; set; }
        public String kdvorname { get; set; }

        override public long getEntityId()
        {
            return sysOppo;
        }
        public override string getEntityBezeichnung()
        {
            return name;
        }

    }
}