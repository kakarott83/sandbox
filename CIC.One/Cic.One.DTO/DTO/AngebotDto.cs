using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class AngebotDto : EntityDto
    {
        public PersistenceType persistenceType { get; set; }

        override public long getEntityId()
        {
            return sysID;
        }

        public long SysPerson { get; set; }
        public long SysOb { get; set; }
        public long SysKalk { get; set; }

        /// <summary>
        /// Bezeichnung for searchresults/fav/recent list
        /// </summary>
        /// <returns></returns>
        override public String getEntityBezeichnung()
        {
            return angebot + " " + objektVT+" "+kundeName;
        }

        /// <summary>
        /// sysID/ANGEBOT /*Primärschlüssel */
        /// </summary>
        public long sysID { get; set; }

        /// <summary>
        /// Angebot/ANGEBOT
        /// </summary>
        public string  angebot { get; set; }

        public List<AngvarDto> varianten { get; set; }

        /// <summary>
        /// LSADD
        /// </summary>
        public LsaddDto ls { get; set; }

        /// <summary>
        /// sysWaehrung/ANGEBOT/WAEHRUNG
        /// </summary>
        public WaehrungDto waehrung { get; set; }

        /// <summary>
        /// sysVART/ANGEBOT/VART
        /// </summary>
        public long sysVart { get; set; }

        /// <summary>
        /// sysVART/ANGEBOT/VART
        /// </summary>
        public string vart { get; set; }

        /// <summary>
        /// Konstellation/ANGEBOT
        /// </summary>
        public string  konstellation { get; set; }

        /// <summary>
        /// Vertriebsweg/ANGEBOT
        /// </summary>
        public string vertriebsweg { get; set; }

        /// <summary>
        /// Fform/ANGEBOT
        /// </summary>
        public string vform { get; set; }

        /// <summary>
        /// Beginn/ANGEBOT
        /// </summary>
        public DateTime beginn { get; set; }

        /// <summary>
        /// Uebernahme/ANGEBOT
        /// </summary>
        public DateTime uebernahme { get; set; }

        /// <summary>
        /// Ersterate
        /// </summary>
        public DateTime ersterate { get; set; }
        /// <summary>
        /// BGExtern/ANGEBOT
        /// </summary>
        public double bgExtern { get; set; }

        /// <summary>
        /// LZ/ANGEBOT
        /// </summary>
        public int lz { get; set; }

        /// <summary>
        /// DatAktiv/ANGEBOT
        /// </summary>
        public DateTime datAktiv { get; set; }

        /// <summary>
        /// SZ/ANGEBOT
        /// </summary>
        public double sz { get; set; }

        /// <summary>
        /// PPY/ANGEBOT
        /// </summary>
        public int ppy { get; set; }

        /// <summary>
        /// Ende/ANGEBOT
        /// </summary>
        public DateTime ende { get; set; }

        /// <summary>
        /// Rueckgabe/ANGEBOT
        /// </summary>
        public DateTime rueckgabe { get; set; }

        /// <summary>
        /// RW/ANGEBOT
        /// </summary>
        public double rw { get; set; }

        /// <summary>
        /// Zustand/ANGEBOT
        /// </summary>
        public string zustand { get; set; }

        /// <summary>
        /// OK/ANGEBOT
        /// </summary>
        public int ok { get; set; }

        /// <summary>
        /// AktivKZ/ANGEBOT
        /// </summary>
        public int aktivKZ { get; set; }

        /// <summary>
        /// Locked/ANGEBOT
        /// </summary>
        public int locked { get; set; }

        /// <summary>
        /// EndeKZ/ANGEBOT
        /// </summary>
        public int endeKZ { get; set; }

        /// <summary>
        ///  EndeAm/ANGEBOT
        /// </summary>
        public DateTime endeAm { get; set; }


        /// <summary>
        /// activeFlag 
        /// </summary>
        public int activeFlag { get; set; }


        /// <summary>
        /// ADMADD/Außendienstmitarbeiter
        /// </summary>
        public AdmaddDto aussendienstmitarbeiter { get; set; }


        /// <summary>
        /// VPFILADD/vertriebspartner
        /// </summary>
        public VpfiladdDto vertriebspartner { get; set; }


        // Added 1.10.2013
        
        /// <summary>
        /// Testfall
        /// </summary>
        public bool testFlag { get; set; }
        /// <summary>
        /// Notstop
        /// </summary>
        public bool notstopFlag { get; set; }
      

      

        /// <summary>
        /// Verweis zum Kanal (FF, KF)
        /// </summary>
        public long sysprchannel { get; set; }

        /// <summary>
        /// Verweis zur Handelsgruppe
        /// </summary>
        public long sysprhgroup { get; set; }

        /// <summary>
        /// Verweis zur Brand
        /// </summary>
        public long sysbrand { get; set; }

        /// <summary>
        /// Verweis zur Marketingaktion (Kampagnencode)
        /// </summary>
        public long sysmarktab { get; set; }

        /// <summary>
        /// Erfasser
        /// </summary>
        public long syswfuser { get; set; }

        /// <summary>
        /// Erfasst am
        /// </summary>
        public DateTime erfassung { get; set; }

        /// <summary>
        /// Änderer
        /// </summary>
        public long syswfuserchange { get; set; }

        /// <summary>
        /// Geändert am
        /// </summary>
        public DateTime aenderung { get; set; }

        /// <summary>
        /// Betreuer (Antragsowner)
        /// </summary>
        public long sysberater { get; set; }

        /// <summary>
        /// Verkäufer
        /// </summary>
        public long sysberataddb{ get; set; }

        /// <summary>
        /// Gültigkeit Angebot
        /// </summary>
        public DateTime gueltigBis { get; set; }

        /// <summary>
        /// Zustandsübergang am
        /// </summary>
        public DateTime zustandAm { get; set; }

        /// <summary>
        /// Attribut
        /// </summary>
        public String attribut { get; set; }

        /// <summary>
        /// KKG Pflicht
        /// </summary>
        public bool kkgpflicht { get; set; }

        /// <summary>
        /// Kartennummer bei Auszahlung auf Kreditkarte
        /// </summary>
        public String kartennummer { get; set; }

        /// <summary>
        /// Verweis zum Verwendungszweck(Lookup und Übersetzung)
        /// </summary>
        public String verwendungszweckCode { get; set; }

        /// <summary>
        /// Attributsübergang am
        /// </summary>
        public String attributAm { get; set; }

        /// <summary>
        /// ErfassungsClient
        /// </summary>
        public long erfassungsclient { get; set; }

        /// <summary>
        /// Name des Kunden über syskd
        /// </summary>
        public String kundeName { get; set; }

        /// <summary>
        /// Kunde Ort
        /// </summary>
        public String kundeOrt { get; set; }
        
        /// <summary>
        /// Händler über haendler
        /// </summary>
        public String haendler { get; set; }

        /// <summary>
        /// Händler über haendlerOrt
        /// </summary>
        public String haendlerOrt { get; set; }

        /// <summary>
        /// Name des Interessenten über sysit
        /// </summary>
        public String interessentName { get; set; }

        /// <summary>
        /// Name des Mandanten über sysls
        /// </summary>
        public String mandantName { get; set; }

        /// <summary>
        /// Name des Rahmenvertrags über sysrvt
        /// </summary>
        public String rahmenName { get; set; }

        public long sysRvt { get; set; }

        public long sysLs { get; set; }
        //synonym for sysperson
        public long sysKd { get; set; }

        public long sysIt { get; set; }

        public long sysAntrag { get; set; }

        /// <summary>
        /// Marke+modell+typ
        /// </summary>
        public String objektVT { get; set; }

        /// <summary>
        /// Benutzer
        /// </summary>
        public String benutzer { get; set; }

        //optionsflags
        public AngAntOptionDto options {get;set;}

        public DateTime? druck { get; set; }
        /// <summary>
        /// Nutzungsart
        /// </summary>
        public int nutzung { get; set; }
        /// <summary>
        /// Bruttokredit
        /// </summary>
        public int bruttokredit { get; set; }
        /// <summary>
        /// EWB
        /// </summary>
        public double ewbbetrag { get; set; }

        public String extratingcode { get; set; }
        public String beschrdeutsch { get; set; }

        /// <summary>
        /// Objektart
        /// </summary>
        public String hersteller { get; set; }
        /// <summary>
        /// Marke/Modell
        /// </summary>
        public String fabrikat { get; set; }

        /// <summary>
        /// BGINTERNBRUTTO
        /// </summary>
        public double betrag { get; set; }

        /// <summary>
        /// Produktinformationen aus der Kalkulation
        /// </summary>
        public ProduktInfoDto produkt { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String plz { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String bezeichnung { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String prProductname { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public double ratebrutto { get; set; }

		/// <summary>
		/// jahresKM
		/// </summary>
		public long jahresKM { get; set; }

		/// <summary>
		/// Anschaffungswert
		/// </summary>
		public double ahkbrutto { get; set; }

		/// <summary>
		/// VERKAUFERNAME
		/// </summary>
		public String verkaufername { get; set; }

		/// <summary>
		/// VERKAUFERVORNAME
		/// </summary>
		public String verkaufervorname { get; set; }

		/// <summary>
		/// Auflagenanzahl
		/// </summary>
		public int auflagenanzahl { get; set; }

		/// <summary>
		/// Auflagen JA / NEIN
		/// </summary>
		public String auflagen
		{
			get { if (auflagenanzahl > 0) return "JA"; else return "NEIN"; }
			set { }
		}
	}
}