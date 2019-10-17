// OWNER JJ, 16-12-2009
// NOTE JJ, IN PROGRESS

using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public sealed class ANGEBOTDto
    {
        #region Enums
        public enum AngebotState
        {
            New,
            Printed,
            HandedIn,
            Rejected,
            Expired
        }
        #endregion

        #region Ids properties

        /// <summary>
        /// ANGEBOT:SYSID - SYSID: SystemID for Table Angebot<br />Systemid für Angebotstabelle - Number(12) - Not Null
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSID
        {
            get;
            set;
        }
        /// <summary>
        /// ANGKALKFS:SYSANGKALKFS - SYSANGKALKFS: <br /> - Number(12) - Not Null
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSANGKALKFS
        {
            get;
            set;
        }
        /// <summary>
        /// ANGEBOT:SYSIT - SYSIT: <br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSIT
        {
            get;
            set;
        }

        /// <summary>
        /// ANGEBOT:SYSBERATADDB - SYSBERATADDB: <br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSBERATADDB
        {
            get;
            set;
        }

        /// <summary>
        /// ANGEBOT:SYSITRSVVN - SYSITRSVVN: <br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSITRSVVN
        {
            get;
            set;
        }
        /// <summary>
        /// ANGEBOT:SYSKD - SYSKD:SystemID From Table KD <br />Systemid von Kundentabelle - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSKD
        {
            get;
            set;
        }
        /// <summary>
        /// ANGKALK:SYSKALK - SYSKALK: SystemID for Table ANGKALK <br />SystemId von ANGKALK Tabelle - Number(12) - Not Null
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGKALKSYSKALK
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSBRAND
        {
            get;
            set;
        }

        // TODO JJ 10 JJ, Check if is necessary:
        // SYSBRAND
        // SYSLS
        #endregion

        #region Properties
        /// <summary>
        /// Angebot:ANGEBOT1 - ANGEBOT1: Offer identification number (*)<br />Angebotsnummer - Varchar2(25)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGEBOT1
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:ERFASSUNG - ERFASSUNG: Offer creation date (*)<br /> Giebt Erstelldatum wieder - DATE
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? ERFASSUNG
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:ZUSTAND - ZUSTAND: Offer state<br />Status - Varchar2(25)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ZUSTAND
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long SYSPRCHANNEL
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public long SYSPRHGROUP
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:ZUSTANDAM - ZUSTANDAM: Offer state date<br />Status Datum - Date
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? ZUSTANDAM
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:BEGINN - BEGINN: Offer start date<br />Beginn Datum - Date
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? BEGINN
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:ENDE - ENDE: Offer end date<br />End Datum - Date
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? ENDE
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalk:ZINSTYP - ANGKALKZINSTYP: Interest<br />Verzinsungsart - Number(6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKZINSTYP
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:ZINSEFF - ANGKALKZINSEFF: Effective interest<br />Effektivzinssatz - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZINSEFF
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZINSEFF_DEFAULT
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:REPCARPRICE - ANGKALKFSREPCARPRICE: Repair service price<br />Ersatzfahrzeug Preis pro Tag Netto - Number
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public double? ANGKALKFSREPCARPRICE
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:REPCARRATEUST - ANGKALKFSREPCARRATEUST: Repair monthly rate tax<br />Ersatzfahrzeug Rate Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSREPCARRATEUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:REPCARRATEBRUTTO - ANGKALKFSREPCARRATEBRUTTO: Repair monthly rate brutto<br />Ersatzfahrzeug Rate Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSREPCARRATEBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:REPCARRATE - ANGKALKFSREPCARRATE: Repair monthly rate<br />Ersatzfahrzeug Rate Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSREPCARRATE
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:PPY - ANGKALKPPY: Periods per year<br />Zahlungsmodus - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKPPY
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:FZART - ANGOBFZART: Vehicle art<br />Fahrzeugart - Varchear2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBFZART
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SYSOBKAT - ANGOBSYSOBKAT: <br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long ANGOBSYSOBKAT
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SYSOB - ANGSYSOB: <br /> - Number(12) - Not Null
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBSYSOB
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:HERSTELLER - ANGOBHERSTELLER: Manufacturer (*)<br />Hersteller des Fahrzeugs - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBHERSTELLER
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:FABRIKAT - ANGOBFABRIKAT: Brand (*)<br />Fahrzeugmodell, z.B. 3-er Touring - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBFABRIKAT
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SCHWACKE - ANGOBSCHWACKE: Eurotax number (*)<br />Eurotax / Schwacke-Code - Varchar2(20)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBSCHWACKE
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:BGN - ANGOBBGN: Expected usual life<br /> - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBBGN
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SYSKGRUPPE - ANGOBSYSKGRUPPE: Customer group<br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBSYSKGRUPPE
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:USGAAP - ANGOBUSGAAP: US-GAAP<br /> - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBUSGAAP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:TYP - ANGOBTYP: <br /> - Varchear2(60)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBTYP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:FARBEA - ANGOBFARBEA: Body color<br />Außenfarbe - Varchar2(20)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBFARBEA
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:FZNR - ANGOBFZNR: Vehicle number<br /> - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBFZNR
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:LIEFERUNG - ANGOBLIEFERUNG: Delivery date<br />Auslieferungsdatum - Date
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? ANGOBLIEFERUNG
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SERIE - ANGOBSERIE: Series<br />Fahrgestellnummer des Fahrzeugs - Varchar(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBSERIE
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string ANGOBFGNR
        {
            get;
            set;
        }
        //Kommisionsnummer
        [System.Runtime.Serialization.DataMember]
        public string ANGOBSPECIFICATION
        {
            get;
            set;
        }
        
        /// <summary>
        /// Angob:KW - ANGOBKW: Power (*)<br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBKW
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:VORBESITZER - ANGOBINIVORBESITZER: Previous owner<br />Anzahl der Vorbesitzer - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBINIVORBESITZER
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:ERSTZUL - ANGOBINIERSTZUL: <br /> Erstzulassung - Date
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? ANGOBINIERSTZUL
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:BAUJAHR - ANGOBBAUJAHR: Production year<br /> - Date
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? ANGOBBAUJAHR
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:KMSTAND - ANGOBINIKMSTAND: Starting mileage<br />Kilometerstand - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBINIKMSTAND
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:GRUND - ANGOBGRUND: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBGRUND
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:GRUNDBRUTTO - ANGOBGRUNDBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBGRUNDBRUTTO
        {
            get;
            set;
        }

        //Relevante Sonderausstattung auf Listenpreis
        /// <summary>
        /// has to contain NOVA!
        /// Ticket #3651 - Nova muss aufgeschlagen werden
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBRV
        {
            get;
            set;
        }

        /// <summary>
        /// Angob:GRUNDBRUTTO - ANGOBGRUNDBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBGRUNDBRUTTOEXKLNOVA
        {
            get;
            set;
        }

        /// <summary>
        /// Angob:GRUNDRABATTOP - ANGOBGRUNDRABATTOP: <br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBGRUNDRABATTOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:GRUNDRABATTO - ANGOBGRUNDRABATTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBGRUNDRABATTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:GRUNDEXTERNBRUTTO - ANGOBGRUNDEXTERNBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBGRUNDEXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:GRUNDEXTERNUST - ANGOBGRUNDEXTERNUST: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBGRUNDEXTERNUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:GRUNDEXTERN - ANGOBGRUNDEXTERN: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBGRUNDEXTERN
        {
            get;
            set;
        }
        /// <summary>
        /// Listenpreis Netto Netto - ANGOBGRUNDEXKLN: <br /> - Number(15,2)
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBGRUNDEXKLN
        {
            get;
            set;
        }

        /// <summary>
        /// Angob:SONZUBBRUTTO - ANGOBSONZUBBRUTTO <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBBRUTTO
        {
            get;
            set;
        }

        /// <summary>
        /// Angob:SONZUBBRUTTO - ANGOBSONZUBBRUTTO <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBBRUTTOEXKLNOVA
        {
            get;
            set;
        }

      

        /// <summary>
        /// Angob:PAKETE - ANGOBPAKETE <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBPAKETE
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:PAKETEBRUTTO - ANGOBPAKETEBRUTTO <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBPAKETEBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:PAKETEBRUTTO - ANGOBPAKETEBRUTTO <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBPAKETEBRUTTOEXKLNOVA
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:PAKETERABATTOP - ANGOBPAKETERABATTOP <br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBPAKETERABATTOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:PAKETERABATTO - ANGOBPAKETERABATTO <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBPAKETERABATTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:EXTERNBRUTTO - ANGOBEXTERNBRUTTO: Not found in Angob table <br />Nicht in der Angob Tabelle gefunden - 
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBEXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:PAKETEPAKETEEXTERNUST - ANGOBPAKETEPAKETEEXTERNUST <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBPAKETEEXTERNUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:PAKETEPAKETEEXTERNBRUTTO - ANGOBPAKETEPAKETEEXTERNBRUTTO <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBPAKETEEXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:PAKETEPAKETEEXTERN - ANGOBPAKETEPAKETEEXTERN <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBPAKETEEXTERN
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:HERZUBBRUTTO - ANGOBHERZUBBRUTTO <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBHERZUBBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Ueberführungskosten brutto
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBUEBERFUEHRUNGBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Zulassungskosten brutto
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBZULASSUNGBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:HERZUB - ANGOBHERZUB <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBHERZUB
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:HERZUBRABATTOP - ANGOBHERZUBRABATTOP <br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBHERZUBRABATTOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:HERZUBRABATTO - ANGOBHERZUBRABATTO <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBHERZUBRABATTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:HERZUBEXTERNBRUTTO - ANGOBHERZUBEXTERNBRUTTO <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBHERZUBEXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:HERZUBEXTERNUST - ANGOBHERZUBEXTERNUST <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBHERZUBEXTERNUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:HERZUBEXTERN - ANGOBHERZUBEXTERN <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBHERZUBEXTERN
        {

            get;
            set;
        }

        /// <summary>
        /// Angkalk:ZUBEHOERBRUTTO - ANGKALKZUBEHOERBRUTTO: Regular additions price brutto (*)<br />Händlerzubehör vor offenen Nachlass Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZUBEHOERBRUTTO
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSANABGROSSUNITPRICE
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSANABNACHLASS
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSFUELNACHLASS
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMAINTNACHLASS
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSTIRESNACHLASS
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSREPCARNACHLASS
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSEXTRASNACHLASS
        {
            get;
            set;
        }


        /// <summary>
        /// Angob:ZUBEHOER - ANGOBZUBEHOER<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBZUBEHOER
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:ZUBEHOEREXTERN - ANGOBZUBEHOEREXTERN<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBZUBEHOEREXTERN
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:ZUBEHOEREXTERNUST - ANGOBZUBEHOEREXTERNUST<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBZUBEHOEREXTERNUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:ZUBEHOEREXTERNBRUTTO - ANGOBZUBEHOEREXTERNBRUTTO<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBZUBEHOEREXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:ZUBEHOERRABATTO - ANGOBZUBEHOERRABATTO<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBZUBEHOERRABATTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:ZUBEHOERRABATTOP - ANGOBZUBEHOERRABATTOP<br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBZUBEHOERRABATTOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:ZUBEHOERBRUTTO - ANGOBZUBEHOERBRUTTO<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBZUBEHOERBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:NOVAZUABBRUTTO - ANGOBNOVAZUABBRUTTO<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBNOVAZUABBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:BGEXTERNBRUTTO - ANGKALKBGEXTERNBRUTTO: Total price brutto (*)<br />Anschaffungswert Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKBGEXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:BGEXTERNUST - ANGKALKBGEXTERNUST: Income tax (*)<br />Anschaffungswert Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKBGEXTERNUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:LZ - ANGKALKLZ: Contract duration<br />Laufzeit (Monate) - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKLZ
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RABATTOP - ANGKALKRABATTOP: Total discount in percent - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRABATTOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SONZUBRABATTOP - ANGOBSONZUBRABATTOP: <br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBRABATTOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SONZUBEXTERNBRUTTO - ANGOBSONZUBEXTERNBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBEXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SONZUBEXTERNUST - ANGOBSONZUBEXTERNUST: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBEXTERNUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SONZUBEXTERN - ANGOBSONZUBEXTERN: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBEXTERN
        {
            get;
            set;
        }


        /// <summary>
        /// Angob:PAKRABOP - ANGOBPAKRABOP: <br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBPAKRABOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:HERZUBRABOP - ANGKALKHERZUBRABOP: Open (visible) discount in percent<br /> - It is in ANGOB. HERZUBRABATTOP - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKHERZUBRABOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:ZUBEHOERORP - ANGKALKZUBEHOERORP:<br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZUBEHOERORP
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RABATTO - ANGKALKRABATTO: Total discount netto<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRABATTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SONZUBRABATTO -  ANGOBSONZUBRABATTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBRABATTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:PAKRABO - ANGKALKPAKRABO: Open (visible) packets discount<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKPAKRABO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:HERZUBRABO - ANGKALKHERZUBRABO: Open (visible) discount<br /> - It is in ANGOB. HERZUBRABATTO - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKHERZUBRABO
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalk:ZUBEHOEROR - ANGKALKZUBEHOEROR: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZUBEHOEROR
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GRUNDNACHLBRUTTO - ANGKALKGRUNDNACHLBRUTTO: Discounts brutto (*)<br /> - It is in ANGOB.GRUNDBRUTTO - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGRUNDNACHLBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:SONZUBNACHLBRUTTO - ANGKALKSONZUBNACHLBRUTTO: Special additions discount price brutto - It is in ANGOB. HERZUBRABATTO - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKSONZUBNACHLBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:PAKETENACHLBRUTTO - ANGKALKPAKETENACHLBRUTTO: Packets discount brutto<br /> - It is in ANGOB. PAKETEBRUTTO - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKPAKETENACHLBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:HERZUBNACHLBRUTTO - ANGKALKHERZUBNACHLBRUTTO: Manufacturer additional price discount brutto<br /> - It is in ANGOB.HERZUBBRUTTO - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKHERZUBNACHLBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:HERZUBUST - ANGKALKHERZUBUST:<br /> - It is in ANGOB.HERZUBEXTERNUST - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKHERZUBUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:ZUBEHOERNACHLBRUTTO - ANGKALKZUBEHOERNACHLBRUTTO:<br /> - It is in ANGKALK.ZUBEHOERBRUTTO - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZUBEHOERNACHLBRUTTO
        {
            get;
            set;
        }


        /// <summary>
        /// Angkalk:BGEXTERNNACHLBRUTTO - ANGKALKBGEXTERNNACHLBRUTTO:<br /> - It is in ANGKALK.BGEXTERNBRUTTO - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKBGEXTERNNACHLBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GRUNDUST - ANGKALKGRUNDUST:<br /> - It is in ANGOB.GRUNDEXTERNUST - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGKALKGRUNDUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:SONZUBUST - ANGKALKSONZUBUST:<br /> - It is in ANGOB.SONZUBEXTERNUST - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGKALKSONZUBUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:PAKETEUST - ANGKALKPAKETEUST: Total packets tax (*)<br /> - It is in ANGOB.PAKETEEXTERNUST - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKPAKETEUST
        {
            get;
            set;
        }


        /// <summary>
        /// Angkalk:ZUBEHOERUST - ANGKALKZUBEHOERUST: <br /> - It is in ANGOB.ZUBEHOEREXTERNUST - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGKALKZUBEHOERUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:NOVAZUABUST - ANGOBNOVAZUABUST:<br /> - Number(15,2) 
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBNOVAZUABUST
        {
            get;
            set;
        }
        /// <summary>
        /// ???:Angkalk or. kalk? - ANGKALK:<br /> - ???
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALK
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GRUNDNETTO - ANGKALKGRUNDNETTO: Total price netto (*)<br /> - It is in ANGKALK.GRUND - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGRUNDNETTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:SONZUBNETTO - ANGKALKSONZUBNETTO: Special additions price netto<br /> - It is in ANGKALK.SONZUB - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKSONZUBNETTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:PAKETENETTO - ANGKALKPAKETENETTO: Total packets price netto (*)<br /> - It is in ANGKALK.PAKETE - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKPAKETENETTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:HERZUBNETTO - ANGKALKHERZUBNETTO: Manufacturer additional price netto<br /> - It is in ANGOB. HERZUB - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKHERZUBNETTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:ZUBEHOERNETTO - ANGKALKZUBEHOERNETTO: Regular additions price netto (*)<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZUBEHOERNETTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:NOVAZUAB - ANGOBNOVAZUAB: Nova addtional/reduction netto<br />NoVA Zu-/Abschlag Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBNOVAZUAB
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:AHK - ANGOBAHK: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBAHK
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:AHKEXTERN - ANGOBAHKEXTERN: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBAHKEXTERN
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:AHKEXTERNUST - ANGOBAHKEXTERNUST: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBAHKEXTERNUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:AHKEXTERNBRUTTO - ANGOBAHKEXTERNBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBAHKEXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:AHKBRUTTO - ANGOBAHKBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBAHKBRUTTO
        {
            get;
            set;
        }

        /// <summary>
        /// Eingangsrechnung inkl. MWST
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBERINKLMWST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:AHKRABATTO - ANGOBAHKRABATTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBAHKRABATTO
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBAHKRABATTOBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:AHKRABATTOP - ANGOBAHKRABATTOP: <br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBAHKRABATTOP
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalk:BGEXTERN - ANGKALKBGEXTERN: Total price netto (*)<br />Anschaffungswert Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKBGEXTERN
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:BGINTERN - ANGKALKBGINTERN:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKBGINTERN
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:BGINTERN - ANGKALKBGINTERNBRUTTO:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKBGINTERNBRUTTO
        {
            get;
            set;
        }

        /// <summary>
        /// Angob:INVENTAR - ANGOBINVENTAR:<br /> - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBINVENTAR
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:CO2 - ANGOBINICO2: Co2 emission (*)<br />Co2-Emissionen in g/km - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBINICO2
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:PARTICLES - ANGOBINIPARTICLES: Particles (*)<br />Partikelausstoß in g/km - Number
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public double? ANGOBINIPARTICLES
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:CCM - ANGOBCCM: Capacity (*)<br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBCCM
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:ACTUATION - ANGOBINIACTUATION: Actuation (hybrid or not) (*)<br />Antriebsart - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBINIACTUATION
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:NOVA_P - ANGOBININOVA_P:<br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBININOVA_P
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:NOX - ANGOBININOX: Nitrous oxides (*)<br />NoX Emission in mg/km - Number
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public double? ANGOBININOX
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:VERBRAUCH_D - ANGOBINIVERBRAUCH_D: Average consumption (*)<br />Durchschnittverbrauch in l/100 km - Number(5,1)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBINIVERBRAUCH_D
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:KW - ANGOBINIKW:<br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBINIKW
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:AUTOMATIK - ANGOBAUTOMATIK: Automatic transmission flag (*)<br />Getriebeart Automatik - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBAUTOMATIK
        {
            get;
            set;
        }
        /// <summary>
        /// Angobini:MOTORTYP - ANGOBINIMOTORTYP:<br /> - Varchar2(1)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBINIMOTORTYP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:NOVAP - ANGOBNOVAP: Nova rate in percent (*)<br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBNOVAP
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:NOVA - ANGOBNOVA: Nova rate (*)<br />NoVA-Befreiung - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBNOVA
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:NOVABRUTTO - ANGOBNOVABRUTTO: Nova tax brutto<br />NoVA Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBNOVABRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:NOVAUST - ANGOBNOVAUST:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBNOVAUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:NOVABETRAG - ANGOBNOVABETRAG: Total nova Netto<br />NoVA Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBNOVABETRAG
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:SZBRUTTOP - ANGKALKSZBRUTTOP: First rate in percent<br />Anzahlung in Prozent des Anschaffungswertes - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKSZBRUTTOP
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKSZP
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalk:Verrechnung - ANGKALKVERRECHNUNG:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKVERRECHNUNG
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalk:SZBRUTTO - ANGKALKSZBRUTTO: First rate brutto<br />Anzahlung / Sonderzahlung Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKSZBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:SZUST - ANGKALKSZUST: First rate tax<br />Anzahlung / Sonderzahlung Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKSZUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:SZ - ANGKALKSZ: First rate<br />Anzahlung / Sonderzahlung Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKSZ
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:MITFINBRUTTO - ANGKALKMITFINBRUTTO: ???<br />Mitfinanzierten Bestandteile Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKMITFINBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:MITFINUST - ANGKALKMITFINUST: ???<br />Mitfinanzierten Bestandteile Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKMITFINUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:MITFIN - ANGKALKMITFIN:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKMITFIN
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalk:DEPOTP - ANGKALKDEPOTP:<br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKDEPOTP
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:DEPOT - ANGKALKDEPOT:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKDEPOT
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWKALKBRUTTOP - ANGKALKRWKALKBRUTTOP: Calculatory residual value in percent<br />Restwert in % - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKBRUTTOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWKALKBRUTTO - ANGKALKRWKALKBRUTTO: Calculatory residual value brutto<br />Restwert Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWKALKUST - ANGKALKRWKALKUST: Calculatory residual value tax<br />Restwert Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWKALK - ANGKALKRWKALK: Calculatory residual value<br />Restwert Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALK
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWKALKBRUTTOP - ANGKALKRWKALKBRUTTOP: Calculatory residual value in percent<br />Restwert in % - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKBRUTTOP_DEFAULT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKBRUTTOP_SUBVENTION
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWKALKBRUTTO - ANGKALKRWKALKBRUTTO: Calculatory residual value brutto<br />Restwert Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKBRUTTO_DEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKBRUTTO_SUBVENTION
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWKALKUST - ANGKALKRWKALKUST: Calculatory residual value tax<br />Restwert Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKUST_DEFAULT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKUST_SUBVENTION
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWKALK - ANGKALKRWKALK: Calculatory residual value<br />Restwert Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALK_DEFAULT
        {
            get;
            set;

        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALK_SUBVENTION
        {
            get;
            set;

        }
        /// <summary>
        /// Angkalk:RWKALK - ANGKALKRWKALK: Calculatory residual value<br />Restwert Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKKREDITBETRAG
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalk:RWBASEBRUTTOP - ANGKALKRWBASEBRUTTOP: Residual value base in percent<br />Restwertvorschlag in % - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWBASEBRUTTOP
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalk:RWBASEBRUTTO - ANGKALKRWBASEBRUTTO: Residual value base brutto<br />Restwertvorschlag Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWBASEBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWBASENETTO - ANGKALKRWBASE: Residual value base Netto<br />Restwertvorschlag Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWBASE
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWBASEUST - ANGKALKRWBASEUST: Residual value base tax<br />Restwertvorschlag Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWBASEUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:RWBASE - ANGOBRWBASE:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBRWBASE
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RATEBRUTTO - ANGKALKRATEBRUTTO: Brutto rate<br />berechnete Rate Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATEBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWCRVBRUTTOP - ANGKALKRWCRVBRUTTOP: Residual value CRV in percent<br />Restwert CRV in % - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWCRVBRUTTOP
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RWCRVBRUTTO - ANGKALKRWCRVBRUTTO: Residual value CRV brutto<br />Restwert CRV Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWCRVBRUTTO
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalk:RWCRVUST - ANGKALKRWCRVUST: Residual value CRV tax<br />Restwert CRV Umsatzsteuer - Number (15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWCRVUST
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalk:RWCRV - ANGKALKRWCRV: <br /> - Number (15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWCRV
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:RWCRV - ANGOBRWCRV: Residual value CRV<br /> - Number (15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBRWCRV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RATEUST - ANGKALKRATEUST: Rate tax<br />berechnete Rate Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATEUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RATE - ANGKALKRATE: Rate<br />berechnete Rate Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATE
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalk:RATEGESAMT - ANGKALKRATEGESAMT:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATEGESAMT
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalk:RATEGESAMTBRUTTO - ANGKALKRATEGESAMTBRUTTO:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATEGESAMTBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalk:RATEGESAMTUST - ANGKALKRATEGESAMTUST:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATEGESAMTUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:RATE - RATE:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? RATE
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RGGVERR - ANGKALKRGGVERR: RGG offset<br />RGG-Zahlungsart - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKRGGVERR
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RGGEBUEHR - ANGKALKRGGEBUEHR: RGG fee<br />Rechtsgeschäftsgebühr (Betrag) - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRGGEBUEHR
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:RGGFREI - ANGKALKRGGFREI: RGG free<br />Rechtsgeschäftsgebühr Befreiung JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKRGGFREI
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GEBUEHRBRUTTO - ANGKALKGEBUEHRBRUTTO: Additional fees brutto<br />Bearbeitungsgebühr Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGEBUEHRBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:REFIZINS1 - ANGKALKREFIZINS1: Real interest<br />Refi-Zinssatz - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKREFIZINS1
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:ZINS1 - ANGKALKZINS1: Interest<br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZINS1
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalk:ZINS - ANGKALKZINS: <br /> - Number(6,9)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZINS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZINS_DEFAULT
        {
            get;
            set;
        }

        /// <summary>
        /// Angebot:ZINS - ZINS: <br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ZINS
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GEBUEHRUST - ANGKALKGEBUEHRUST: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGEBUEHRUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GEBUEHR - ANGKALKGEBUEHR: Additional fees<br />Bearbeitungsgebühr Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGEBUEHR
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalk:GEBUEHRINTERNBRUTTO - ANGKALKGEBUEHRINTERNBRUTTO: <br/> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGEBUEHRINTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SATZMEHRKM - ANGOBSATZMEHRKM: Maximal expected annual mileage<br />Verrechnungssatz für Mehr-KM (Basis Finanzierung) NETTO - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSATZMEHRKM
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SATZMEHRKMBRUTTO - ANGOBSATZMEHRKMBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSATZMEHRKMBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SATZMINDERKM - ANGOBSATZMINDERKM: Minimal expected annual mileage<br />Verrechnungssatz für Minder-KM (Basis Finanzierung) NETTO - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSATZMINDERKM
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SATZMINDERKMBRUTTO - ANGOBSATZMINDERKMBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSATZMINDERKMBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:KMTOLERANZ - ANGOBKMTOLERANZ: Mileage margin<br />Toleranzgrenze für Mehr/Minder-km - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBKMTOLERANZ
        {
            get;
            set;
        }
        /// <summary>
        /// IT:NAME - ITNAME: Customer name<br /> - Varchar2(128)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ITNAME
        {
            get;
            set;
        }
        /// <summary>
        /// IT:VORNAME - ITVORNAME: Customer first name<br /> - Varchar2(60)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ITVORNAME
        {
            get;
            set;
        }
        /// <summary>
        /// IT:STRASSE - ITSTRASSE: Customer street<br /> - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ITSTRASSE
        {
            get;
            set;
        }
        /// <summary>
        /// IT:HSNR - ITHSNR: Customer house number<br /> - Varchar2(20)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ITHSNR
        {
            get;
            set;
        }
        /// <summary>
        /// IT:PLZ - ITPLZ: Customer postal code<br /> - Varchar2(15)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ITPLZ
        {
            get;
            set;
        }
        /// <summary>
        /// IT:ORT - ITORT: Customer city<br /> - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ITORT
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:OBJEKTVT - OBJEKTVT: Object description (* Brand/Model/Type)<br />Enthält die zusammengesetzten Felder: Hersteller, Typ, Fabrikat - Varchar2(180)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string OBJEKTVT
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:DATANGEBOT - DATANGEBOT: Date of Angebot<br /> Datum vom Angebot - Date
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? DATANGEBOT
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:OBJEKT - ANGOBOBJEKT: Object short description<br /> - Varchar(25)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBOBJEKT
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:ORANG - ANGOBORANG: <br /> - It is in ANGOB.RANG - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBORANG
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:TIRESINCLFLAG - ANGKALKFSTIRESINCLFLAG: <br /> - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSTIRESINCLFLAG
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:MAINTENANCEFLAG - ANGKALKFSMAINTENANCEFLAG: Maintenance flag<br />Wartung und Reparatur JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSMAINTENANCEFLAG
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:TIRESFLAG - ANGKALKFSTIRESFLAG: Tires flag<br />Reifen Limitierung JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSTIRESFLAG
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:FUELFLAG - ANGKALKFSFUELFLAG: Fuel flag<br />Petrolservice JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSFUELFLAG
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:REPCARFLAG - ANGKALKFSREPCARFLAG: Repair flag<br />Ersatzfahrzeugservice JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSREPCARFLAG
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:REPCARCOUNT - ANGKALKFSREPCARCOUNT: Expected repair count<br />Ersatzfahrzeug Anzahl der Tage - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSREPCARCOUNT
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalkfs:ANABKENNZINKLFLAG - ANGKALKFSANABKENNZINKLFLAG: <br /> - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSANABKENNZINKLFLAG
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:ANABMLDFLAG - ANGKALKFSANABMLDFLAG: Registration flag<br />An- und Abmeldegebühren: Service JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSANABMLDFLAG
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:EXTRASFLAG - ANGKALKFSEXTRASFLAG: Extras flag<br />Sonstige Dienstleistungen JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSEXTRASFLAG
        {
            get;
            set;
        }

        /// <summary>
        /// Angob:JAHRESKM - ANGOBJAHRESKM: Expected mileage per year<br />jährliche Laufleistung des Fahrzeugs in km - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBJAHRESKM
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:ABNAHMEKM - ANGOBABNAHMEKM: <br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBABNAHMEKM
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:FUELBRUTTO - ANGKALKFSFUELBRUTTO: Fuel price brutto<br />Petrol Rate Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSFUELBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:FUELUST - ANGKALKFSFUELUST: Fuel tax<br />Petrol Rate Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSFUELUST
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:FUELSYSFSTYP - ANGKALKFUELSYSFSTYP: Lieferant<br />
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGKALKFUELSYSFSTYP
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:FUELPRICE - ANGKALKFSFUELPRICE: Fuel price netto<br />Petrol Rate Netto - Number
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public double? ANGKALKFSFUELPRICE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? FUELPRICEAVGBRUTTO
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:TIRESFIXFLAG - ANGKALKFSTIRESFIXFLAG: Tires fix flag<br />Reifen Nachkalkulation ausgeschlossen JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSTIRESFIXFLAG
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalkfs:TIRESSETS - ANGKALKFSTIRESSETS: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSTIRESSETS
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESCOUNTH - ANGKALKFSSTIRESCOUNTH: Rear summer tires count<br />Sommerreifen Anzahl hinten - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSSTIRESCOUNTH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESCODEH - ANGKALKFSSTIRESCODEH: Rear summer tires code<br />Sommerreifen Code hinten - Varchar2(25)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSSTIRESCODEH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESMODH - ANGKALKFSSTIRESMODH: Rear summer tires extras<br />Sommerreifen Marke hinten - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSSTIRESMODH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESPRICEH - ANGKALKFSSTIRESPRICEH: Rear summer tires price<br />Sommerreifen Preis hinten Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICEH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESPRICEHUST - ANGKALKFSSTIRESPRICEHUST: Rear summer tires tax<br />Sommerreifen Preis hinten Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICEHUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESPRICEHBRUTTO - ANGKALKFSSTIRESPRICEHBRUTTO: Rear summer tires price brutto<br />Sommerreifen Preis hinten Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICEHBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESCOUNTV - ANGKALKFSSTIRESCOUNTV: Front summer tires count<br />Sommerreifen Anzahl vorne - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSSTIRESCOUNTV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESCODEV - ANGKALKFSSTIRESCODEV: Front summer tires code<br />Sommerreifen Code vorne - Varchar2(25)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSSTIRESCODEV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESMODV - ANGKALKFSSTIRESMODV: Front summer tires extras<br />Sommerreifen Marke vorne - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSSTIRESMODV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESPRICEV - ANGKALKFSSTIRESPRICEV: Front summer tires price<br />Sommerreifen Preis vorne Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICEV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESPRICEVUST - ANGKALKFSSTIRESPRICEVUST: Front summer tires tax<br />Sommerreifen Preis vorne Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICEVUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESPRICEVBRUTTO - ANGKALKFSSTIRESPRICEVBRUTTO: Front summer tires price brutto<br />Sommerreifen Preis vorne Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICEVBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESCOUNTH - ANGKALKFSWTIRESCOUNTH: Rear winter tires count<br />Winterreifen Anzahl hinten - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSWTIRESCOUNTH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESCODEH - ANGKALKFSWTIRESCODEH: Reat winter tires code<br />Winterreifen Code hinten - Varchar2(25)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSWTIRESCODEH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESMODH - ANGKALKFSWTIRESMODH: Rear winter tires extras<br />Winterreifen Marke hinten - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSWTIRESMODH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESPRICEH - ANGKALKFSWTIRESPRICEH: Rear winter tires price<br />Winterreifen Preis hinten Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSWTIRESPRICEH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESPRICEHUST - ANGKALKFSWTIRESPRICEHUST: Rear winter tires tax<br />Winterreifen Preis hinten Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSWTIRESPRICEHUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESPRICEHBRUTTO - ANGKALKFSWTIRESPRICEHBRUTTO: Rear winter tires price brutto<br />Winterreifen Preis hinten Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSWTIRESPRICEHBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESCOUNTV - ANGKALKFSWTIRESCOUNTV: Front winter tires count<br />Winterreifen Anzahl vorne - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSWTIRESCOUNTV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESCODEV - ANGKALKFSWTIRESCODEV: Front winter tires code<br />Winterreifen Code vorne - Varchar2(25)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSWTIRESCODEV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESMODV - ANGKALKFSWTIRESMODV: Front winter tires extras<br />Winterreifen Marke vorne - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSWTIRESMODV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESPRICEV - ANGKALKFSWTIRESPRICEV: Front winter tires price<br />Winterreifen Preis vorne Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSWTIRESPRICEV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESPRICEVUST - ANGKALKFSWTIRESPRICEVUST: Front winter tires tax<br />Winterreifen Preis vorne Umsatzsteuer\ - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSWTIRESPRICEVUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESPRICEVBRUTTO - ANGKALKFSWTIRESPRICEVBRUTTO: Front winter tires price brutto<br />Winterreifen Preis vorne Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSWTIRESPRICEVBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:RIMSCOUNTH - ANGKALKFSRIMSCOUNTH: Rear rims count<br />Felgen Anzahl hinten - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSRIMSCOUNTH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:RIMSCODEH - ANGKALKFSRIMSCODEH: Rear rims code<br />RimsCodeH - Varchar2(25)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSRIMSCODEH
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalkfs:RIMSMODH - ANKALKFSRIMSMODH: <br /> - Varchar2(80)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSRIMSMODH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:RIMSPRICEH - ANGKALKFSRIMSPRICEH: Rear rims price<br />Felgen Preis hinten Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSRIMSPRICEH
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalkfs:RIMSPRICEHUST - ANGKALKFSRIMSPRICEHUST:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSRIMSPRICEHUST
        {
            get;
            set;
        }
        /// <summary>
        /// Ankalkfs:RIMSPRICEHBRUTTOT - ANGKALKFSRIMSPRICEHBRUTTO:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSRIMSPRICEHBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:RIMSCOUNTV - ANGKALKFSRIMSCOUNTV: Front rims count<br />Felgen Anzahl vorne - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSRIMSCOUNTV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:RIMSCODEV - ANGKALKFSRIMSCODEV: Front rims code<br />Felgen Code vorne - Varchar2(25)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSRIMSCODEV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:RIMSMODV - ANGKALKFSRIMSMODV:<br /> - Varchar2(80)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSRIMSMODV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:RIMSPRICEV - ANGKALKFSRIMSPRICEV: Front rims price<br />Felgen Preis vorne Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSRIMSPRICEV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:RIMSPRICEVUST - ANGKALKFSRIMSPRICEVUST: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSRIMSPRICEVUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:RIMSPRICEVBRUTTO - ANGKALKFSRIMSPRICEVBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSRIMSPRICEVBRUTTO
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:TIRESADDITION - ANGKALKFSTIRESADDITION: Additional tires rate<br />Zusatzkosten pro Reifen Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSTIRESADDITION
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:TIRESADDITIONUST - ANGKALKFSTIRESADDITIONUST: Tires extras tax<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSTIRESADDITIONUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:TIRESADDITIONBRUTTO - ANGKALKFSTIRESADDITIONBRUTTO: Tires extras price brutton<br />Zusatzkosten pro Reifen Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSTIRESADDITIONBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESPRICE - ANGKALKFSSTIRESPRICE: Summer tires price<br />Reifen Gesamtrate Netto - Number
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public double? ANGKALKFSSTIRESPRICE
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESPRICEUST - ANGKALKFSSTIRESPRICEUST: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICEUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESPRICEBRUTTO - ANGKALKFSSTIRESPRICEBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICEBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:EXTRASBRUTTO - ANGKALKFSEXTRASBRUTTO: Extras total price<br />Sonstige Dienstleistungen Rate Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSEXTRASBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:EXTRASUST - ANGKALKFSEXTRASUST: Extras tax<br />Sonstige Dienstleistungen Rate Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSEXTRASUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:EXTRASPRICE - ANGKALKFSEXTRASPRICE: Extras price netto<br />Sonstige Dienstleistungen Rate Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSEXTRASPRICE
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:EXTRASPRICEUNIT - ANGKALKFSEXTRASPRICEUNIT: Extras price einmalig<br />Sonstige Dienstleistungen Rate Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSEXTRASPRICEUNIT
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:MEHRKM - ANGKALKFSMEHRKM: Expected mileage fee<br />Verrechnungssatz Wartung und Rep. pro Mehrkm Netto - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMEHRKM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSKOSTENPERKILOMETER
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:MINDERKM - ANGKALKFSMINDERKM: Return price for kilometer below expected mileage<br />Minderkm - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMINDERKM
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:MAINTFIXFLAG - ANGKALKFSMAINTFIXFLAG: Maintenance fix flag<br />Wartung und Reparatur: Fixe Ratenkalkulation JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSMAINTFIXFLAG
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:MAINTENANCEUST - ANGKALKFSMAINTENANCEUST: Maintenance tax<br />Wartung und Reparatur Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMAINTENANCEUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:MAINTENANCEBRUTTO - ANGKALKFSMAINTENANCEBRUTTO: Maintenance price brutto<br />Wartung und Reparatur Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMAINTENANCEBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:MAINTENANCE - ANGKALKFSMAINTENANCE: Maintenance price netto<br />Wartung und Reparatur Netto - Number
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public double? ANGKALKFSMAINTENANCE
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:ANABBRUTTO - ANGKALKFSANABBRUTTO: Registration fee brutto<br />An- und Abmeldegebühren Rate Brutto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSANABBRUTTO
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:ANABUST - ANGKALKFSANABUST: Registration fee tax<br />An- und Abmeldegebühren Rate Umsatzsteuer - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSANABUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:ANABMELDUNG - ANGKALKFSANABMELDUNG: Registration fee netto<br />An- und Abmeldegebühren Rate Netto - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSANABMELDUNG
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:SYSVART - SYSVART: Contract art - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSVART
        {
            get;
            set;
        }

        /// <summary>
        /// Angebot:SYSVARTTAB - SYSVARTTAB
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSVARTTAB
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:VART - VART: Vertragsart - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string VART
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:SYSKALKTYP - ANGKALKSYSKALKTYP: Calculation type<br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGKALKSYSKALKTYP
        {
            get;
            set;
        }

        /// <summary>
        /// Angebot:SYSVTTYP - SYSVTTYP: Contract type<br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSVTTYP
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:SYSVT - SYSVT: <br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSVT
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:SYSPRPRODUCT - SYSPRPRODUCT: Product<br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPRPRODUCT
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SYSVT - ANGOBSYSVT: <br /> - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBSYSVT
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:SYSOBTYP - SYSOBTYP:<br />  -  Is in ANGOB.SYSOBTYP - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSOBTYP
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:SYSOBART - SYSOBART:<br />  -  Is in ANGOB. SYSOBART - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSOBART
        {
            get;
            set;
        }
        /// <summary>
        /// It is ANGOBAUSDto[]
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public ANGOBAUSDto[] ANGOBAUST
        {
            get;
            set;
        }
        /// <summary>
        /// It is needed to calculate ZINS while saving Angebot
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPRKGROUP
        {
            get;
            set;
        }
        /// <summary>
        /// It is InsuranceDto[]
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public InsuranceDto[] ANGVSPARAM
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:STIRESTEXTV - ANGKALKFSSTIRESTEXTV:<br /> - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSSTIRESTEXTV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:STIRESTEXTVH - ANGKALKFSSTIRESTEXTH:<br /> - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSSTIRESTEXTH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESTEXTH - ANGKALKFSWTIRESTEXTH:<br /> - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSWTIRESTEXTH
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:WTIRESTEXTV - ANGKALKFSWTIRESTEXTV:<br /> - Varchar2(40)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGKALKFSWTIRESTEXTV
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:MEHRKMBRUTTO - ANGKALKFSMEHRKMBRUTTO:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMEHRKMBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:MEHRKMUST - ANGKALKFSMEHRKMUST:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMEHRKMUST
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:MINDERKMBRUTTO - ANGKALKFSMINDERKMBRUTTO:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMINDERKMBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalkfs:MINDERKMUST - ANGKALKFSMINDERKMUST:<br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMINDERKMUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angebot:RWCRV - RWCRV: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? RWCRV
        {
            get;
            set;
        }
        /// <summary>
        /// Angob:SONZUB - ANGOBSONZUB: <br /> - Number(15,2) - Gesamtsumme aus SA3- und User
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUB
        {
            get;
            set;
        }
        /// <summary>
        /// Von user eingegebenes Sonderzubehör zusätzlich zu SA3
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBUSER
        {
            get;
            set;
        }
        /// <summary>
        /// Aus SA3 kommendes Sonderzubehör
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBSONZUBDEFAULT
        {
            get;
            set;
        }
        /// <summary>
        /// Text für Sonderzubehör
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBSONZUBTEXT
        {
            get;
            set;
        }

        /// <summary>
        /// Angob:VORKENNZEICHEN - ANGOBVORKENNZEICHEN: <br /> - Varchar2(20)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBVORKENNZEICHEN
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GESAMTBRUTTO - ANGKALKGESAMTBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGESAMTBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GESAMTNETTO - ANGKALKGESAMTNETTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGESAMTNETTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GESAMTUST - ANGKALKGESAMTUST: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGESAMTUST
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GESAMTKOSTEN - ANGKALKGESAMTKOSTEN: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGESAMTKOSTEN
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GESAMTKOSTENBRUTTO - ANGKALKGESAMTKOSTENBRUTTO: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGESAMTKOSTENBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Angkalk:GESAMTKOSTENUST - ANGKALKGESAMTKOSTENUST: <br /> - Number(15,2)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGESAMTKOSTENUST
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public OfferTypeConstants ANGOBCONFIGSOURCE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public ServiceAccess.DdOl.CalculationDto.CalculationSources KALKULATIONSOURCE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBCONFIGID
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? TRADEONOWNACCOUNT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBPICTUREURL
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBWAGENTYP
        {
            get;
            set;
        }

        // Provisionen
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPROVTARIF
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? FINANZIERUNGSSUMME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? RESTKAUFPREIS
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:VKFLAG - ANGKALKFSVKFLAG: Full insurance flag<br />Vollkaskoversicherung JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSVKFLAG
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:HPFLAG - ANGKALKFSHPFLAG: Liability insurance flag<br />Haftpflichtversicherung JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSHPFLAG
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalkfs:RECHTSCHUTZFLAG - ANGKALKFSRECHTSCHUTZFLAG: Legal protection insurance<br />Rechtschutzversicherung JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSRECHTSCHUTZFLAG
        {
            get;
            set;
        }

        // TODO WB 0 MK, Delete
        /// <summary>
        /// Angkalkfs:INSASSENFLAG - ANGKALKFSINSASSENFLAG: Passengers insurance flag<br />Insassenunfallversicherung JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSINSASSENFLAG
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSGAPFLAG
        {
            get;
            set;
        }

        // TODO WB 0 MK, Delete
        /// <summary>
        /// Angkalkfs:INSASSENFLAG - ANGKALKFSINSASSENFLAG: Passengers insurance flag<br />Insassenunfallversicherung JA / NEIN - Number(3)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGKALKFSRSVFLAG
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMOTORVS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? GAPPROVISION
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal? ABSCHLUSSPROVISION
        {
            get;
            set;
        }
        
        
        [System.Runtime.Serialization.DataMember]
        public decimal? WUNSCHPROVISION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? KASKOPROVISION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ZUGANGSPROVISION
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal? ZUGANGSPROVISIONNETTO
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal? ZUGANGSPROVISIONPRO
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? PROVISIONSFAUFSCHLAG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? RESTSCHULDPROVISION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? WARTUNGSPROVISION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? GEBUEHRPROVISION
        {
            get;
            set;
        }
      

        
        [System.Runtime.Serialization.DataMember]
        public decimal? HAFTPFLICHTPROVISION
        {
            get;
            set;
        }

        // Subventionen
        // o	Rate aufgrund Zinsänderung (via PRINTSET)
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATE_DEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATE_SUBVENTION
        {
            get;
            set;
        }

        // o	Rechtsgeschäftsgebühr
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRGGEBUEHR_DEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRGGEBUEHR_SUBVENTION
        {
            get;
            set;
        }

        // o	Bearbeitungsgebühr
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGEBUEHR_DEFAULT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGEBUEHR_NACHLASS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKGEBUEHR_SUBVENTION
        {
            get;
            set;
        }

        // o    Mitfinanzierte Bestandteile
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKMITFIN_DEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKMITFIN_SUBVENTION
        {
            get;
            set;
        }

        // o	Alle Serviceraten
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSFUELPRICE_DEFAULT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSFUELPRICE_SUBVENTION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSANABMELDUNG_DEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSANABMELDUNG_SUBVENTION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSREPCARRATE_DEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSREPCARRATE_SUBVENTION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICE_DEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSSTIRESPRICE_SUBVENTION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMANAGEMENT_DEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMANAGEMENT_SUBVENTION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMAINTENANCE_DEFAULT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKFSMAINTENANCE_SUBVENTION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? WFMMEMOSTATTPREIS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? WFMMEMOANGEBOTSPREIS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string WFMMEMOTEXT
        {
            get;
            set;
        }

        //Sonderkalkulation Notiz Verkäufer
        
        [System.Runtime.Serialization.DataMember]
        public string WFMMEMOSCALCVKTEXT
        {
            get;
            set;
        }

        //Sonderkalkulation Notiz Innendienst
        
        [System.Runtime.Serialization.DataMember]
        public string WFMMEMOSCALCIDTEXT
        {
            get;
            set;
        }

        /// <summary>
        /// Sonderkalkulationsstatus
        /// 1 = Angefordert
        /// 2 = inBearbeitung
        /// 3 = Durchgeführt
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? SPECIALCALCSTATUS
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string SPECIALCALCSTATUSTEXT
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long SPECIALCALCSYSWFUSER
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string SPECIALCALCUSER
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? SPECIALCALCDATE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSFSTYPPETROL
        {
            get;
            set;
        }

        /// <summary>
        /// Stores id to sa3 picture
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSWFDADOC
        {
            get;
            set;
        }


        /// <summary>
        ///
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? SPECIALCALCCOUNT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string WFMSubmitKommentar
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ERRORMESSAGE
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string ERRORDETAIL
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBPOLSTERCODE
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string ANGOBPOLSTERTEXT
        {
            get;
            set;
        }




        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? GUELTIGBIS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool Gueltig
        {
            get;
            set;
        }

        #endregion

        #region ReifenCO2
        /// <summary>
        /// Angobini:CO2 - ANGOBINICO2: Co2 emission (*)<br />Co2-Emissionen in g/km - Number(12)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long? ANGOBINICO2DEF
        {
            get;
            set;
        }

        /// <summary>
        /// Angobini:NOX - ANGOBININOX: Nitrous oxides (*)<br />NoX Emission in mg/km - Number
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public double? ANGOBININOXDEF
        {
            get;
            set;
        }

        /// <summary>
        /// Angobini:VERBRAUCH_D - ANGOBINIVERBRAUCH_D: Average consumption (*)<br />Durchschnittverbrauch in l/100 km - Number(5,1)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBINIVERBRAUCH_DDEF
        {
            get;
            set;
        }

        /// <summary>
        /// Angobini:PARTICLES - ANGOBINIPARTICLES: Particles (*)<br />Partikelausstoß in g/km - Number
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public double? ANGOBINIPARTICLESDEF
        {
            get;
            set;
        }

        /// <summary>
        /// Angobini:ACTUATION - ANGOBINIACTUATION: Actuation (hybrid or not) (*)<br />Antriebsart - Number(5)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBINIACTUATIONDEF
        {
            get;
            set;
        }

        /// <summary>
        /// Angob:NOVAP - ANGOBNOVAP: Nova rate in percent (*)<br /> - Number(9,6)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGOBNOVAPDEF
        {
            get;
            set;
        }
        #endregion

        
        [System.Runtime.Serialization.DataMember]
        public decimal? TOTALPROVISION
        {
            get;
            set;
        }

        #region Extended properties
        
        [System.Runtime.Serialization.DataMember]
        public string ExtTitle
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string VERTRIEBSWEG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSANGEBOT
        {
            get;
            set;
        }



        
        [System.Runtime.Serialization.DataMember]
        public string HIST_SYSPRPRODUCT
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string HIST_GAP_VSPERSON
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string HIST_KASKO_VSPERSON
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_HP_VSPERSON
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string HIST_GAP_VSTYP
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string HIST_GAP_LZ
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string HIST_RSDV_VSTYP
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string HIST_KASKO_VSTYP
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_KASKO_DESC
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_HP_VSTYP
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_IUV_VSPERSON
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_ANGKALKFUELSYSFSTYP
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_ANGOBINIMOTORTYP1
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_RSDV_VSPERSON
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_SYSITRSVVN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string HIST_RSV_VSPERSON
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string HIST_ANGKALKZINSTYP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string HIST_SYSPROVTARIF
        {
            get;
            set;
        }

        
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_SYSOBART
        {
            get;
            set;
        }
         
        
        [System.Runtime.Serialization.DataMember]
        public string HIST_ANGOBSYSOBKAT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string KFZBRIEF
        {
            get;
            set;
        }
        /// <summary>
        /// Holds all price-Information
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public PriceInformationDto PRICES
        {
            get;
            set;
        }

        /// <summary>
        /// ABZUGNOVAAUFSCHLAG
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ABZUGNOVAAUFSCHLAG
        {
            get;
            set;
        }

        /// <summary>
        /// Angkalk:ZINS1 - ANGKALKZINS2: Ticket 4056
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZINS2
        {
            get;
            set;
        }
        #endregion

        #region Vertragsverlaengerung
        [System.Runtime.Serialization.DataMember]
        public int? CONTRACTEXT
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public int? CONTRACTTYPE
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string VORVERTRAG
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public long? SYSVORVT
        {
            get;
            set;
        }
        
        /// <summary>
        /// Flag to allow overriding the residual value pouvoirs for the internal user
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool RESTWERTGARANTIE
        {
            get;
            set;
        }

      
        /// <summary>
        /// Flag to disable usage of current user for provision-calculation
        /// instead, the original creator of the offer is used 
        /// needed (for special calculation)
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool noProvisionChange
        {
            get;
            set;
        }

        /// <summary>
        /// Flag to display a warning for depot when opening a new offer from a contract for extension
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool VVDEPOT
        {
            get;
            set;
        }

        /// <summary>
        /// Holder of the new MART-Id for filtering products (saved in ANGOBINI)
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long SYSMART
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public bool isMotorrad
        {
            get;
            set;
        }


        /// <summary>
        /// Eingegebener Restwert
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKBRUTTOORG
        {
            get;
            set;
        }
        /// <summary>
        /// Eingegebener Restwert Prozentsatz
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRWKALKBRUTTOPORG
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string VORVERTRAGSNUMMER
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long? SYSKALKTYPVORVT
        {
            get;
            set;
        }


        #endregion
        #region SEPA
        [System.Runtime.Serialization.DataMember]
        public long? SYSKI
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public int? EINZUG
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string UNTERSCHRIFTORT
        {
            get;
            set;
        }
        /// <summary>
        /// Perole für die Abschlussprovision
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long APROVPEROLE
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public DateTime? ANGKALKERSTERATE
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string ANTRAGSSTATUS
        {
            get;
            set;
        }
        #endregion

        #region Ablöse
        [System.Runtime.Serialization.DataMember]
        public String ANGABLIBAN { get; set; }
        [System.Runtime.Serialization.DataMember]
        public String ANGABLBANK { get; set; }
        [System.Runtime.Serialization.DataMember]
        public String ANGABLFREMDVERTRAG { get; set; }
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGABLAKTUELLERATE { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int ANGABLFLAGINTEXT { get; set; }
        #endregion 
    
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKRATE_SUBVENTION2
        {
            get;
            set;
        }
        public decimal? ANGKALKRATE_SUBVENTION3
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZINSBASIS
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKZINSAKTION
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public int PRODUCTVALID
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public String ANGKALKOBJECTMETACALCTARGET
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal? ANGKALKBWFEHLER
        {
            get;
            set;
        }

		/// <summary>
		/// VORVERTRAG ANGABL::DATKALKPER - GUARDEAN.replacementDate - Ablösung Vorvertrag vom (rh 20180412)
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public System.DateTime? VVTDATKALKPER
		{
			get;
			set;
		}

		/// <summary>
		/// VORVERTRAG ANGABL::AKTUELLERATE - GUARDEAN.rate - monatliche Rate (rh 20180412)
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public decimal? VVTAKTUELLERATE
		{
			get;
			set;
		}
	}
}