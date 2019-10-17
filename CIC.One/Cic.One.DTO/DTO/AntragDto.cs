using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO.BN;

namespace Cic.One.DTO
{
    public class AntragDto: EntityDto
    {
        public PersistenceType persistenceType { get; set; }

        /// <summary>
        ///  /*Primärschlüssel */
        /// </summary>
        public long sysID { get; set; }

        override public long getEntityId()
        {
            return sysID;
        }

        /// <summary>
        /// Bezeichnung for searchresults/fav/recent list
        /// </summary>
        /// <returns></returns>
        override public String getEntityBezeichnung()
        {
            return antrag;
        }

        //synonym for sysperson
        public long sysKd { get; set; }

        //synonym for sysit
        public long sysIt { get; set; }

        /// <summary>
        /// Angebot/ANTRAG
        /// </summary>
        public string  antrag { get; set; }
        
        public long sysls { get; set; }
        public long sysadm { get; set; }
        public long syswaehrung { get; set; }
        /// <summary>
        /// ANTRAG/LSADD
        /// </summary>
        public LsaddDto ls { get; set; }

        /// <summary>
        /// sysWaehrung/ANTRAG/WAEHRUNG
        /// </summary>
        public WaehrungDto waehrung { get; set; }

        /// <summary>
        /// ANTRAG/VART
        /// </summary>
        public long sysVart { get; set; }

        /// <summary>
        /// ANTRAG/VART
        /// </summary>
        public string vart { get; set; }

        /// <summary>
        /// Konstellation/ANTRAG
        /// </summary>
        public string  konstellation { get; set; }

        /// <summary>
        /// Vertriebsweg/ANTRAG
        /// </summary>
        public string vertriebsweg { get; set; }

        /// <summary>
        /// Fform/ANTRAG
        /// </summary>
        public string fform { get; set; }

        /// <summary>
        /// Beginn/ANTRAG
        /// </summary>
        public DateTime beginn { get; set; }

        /// <summary>
        /// Uebernahme/ANTRAG
        /// </summary>
        public DateTime uebernahme { get; set; }

        /// <summary>
        /// BGExtern/ANTRAG
        /// </summary>
        public double bgExtern { get; set; }

        /// <summary>
        /// LZ/ANTRAG
        /// </summary>
        public int lz { get; set; }

        /// <summary>
        /// DatAktiv/ANTRAG
        /// </summary>
        public DateTime datAktiv { get; set; }

        /// <summary>
        /// SZ/ANTRAG
        /// </summary>
        public double sz { get; set; }

        /// <summary>
        /// PPY/ANTRAG
        /// </summary>
        public int ppy { get; set; }

        /// <summary>
        /// Ende/ANTRAG
        /// </summary>
        public DateTime ende { get; set; }

        /// <summary>
        /// Rueckgabe/ANTRAG
        /// </summary>
        public DateTime rueckgabe { get; set; }

        /// <summary>
        /// RW/ANTRAG
        /// </summary>
        public double rw { get; set; }

        /// <summary>
        /// Zustand/ANTRAG
        /// </summary>
        public string zustand { get; set; }

        /// <summary>
        /// OK/ANTRAG
        /// </summary>
        public int ok { get; set; }

        /// <summary>
        /// AktivKZ/ANTRAG
        /// </summary>
        public int aktivKZ { get; set; }

        /// <summary>
        /// Locked/ANTRAG
        /// </summary>
        public int locked { get; set; }

        /// <summary>
        /// EndeKZ/ANTRAG
        /// </summary>
        public int endeKZ { get; set; }

        /// <summary>
        ///  EndeAm/ANTRAG
        /// </summary>
        public DateTime endeAm { get; set; }

        /// <summary>
        /// AntobDto
        /// </summary>
        public AntobDto antob { get; set; }

        /// <summary>
        /// AntoblsDto
        /// </summary>
        public List<AntobslDto> antoblsList { get; set; }

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


        /// <summary>
        /// GedrucktAm/ANTRAG
        /// </summary>
        public DateTime druck { get; set; }

        /// <summary>
        /// Attribut
        /// </summary>
        public string attribut { get; set; }

        /// <summary>
        /// Name des Kunden über syskd
        /// </summary>
        public String kundeName { get; set; }
        /// <summary>
        /// Kunde Ort
        /// </summary>
        public String kundeOrt { get; set; }

        public String kundeVorname { get; set; }
        public String kundeStrasse { get; set; }
        public String kundeHausnr { get; set; }
        /// <summary>
        /// Antragsowner über sysberater
        /// </summary>
        public String berater { get; set; }
        /// <summary>
        /// Händler über sysvm
        /// </summary>
        public String haendler { get; set; }
        /// <summary>
        /// Objektart
        /// </summary>
        public String hersteller { get; set; }
        /// <summary>
        /// Marke/Modell
        /// </summary>
        public String fabrikat { get; set; }
        /// <summary>
        /// Erfassungsdatum
        /// </summary>
        public DateTime? erfassung { get; set; }
        /// <summary>
        /// Änderungsdatum
        /// </summary>
        public DateTime? aenderung { get; set; }

        /// <summary>
        /// Mitantragsteller
        /// </summary>
        public long sysMa{ get; set; }

        /// <summary>
        /// Haendler
        /// </summary>
        public long sysVm { get; set; }

        /// <summary>
        /// Vermittler-Person
        /// </summary>
        public long sysBerater { get; set; }

        /// <summary>
        /// Produktinformationen aus der Kalkulation
        /// </summary>
        public ProduktInfoDto produkt { get; set; }

        /// <summary>
        /// Auflagenliste
        /// </summary>
        public List<RatingAuflageDto> auflagen{ get; set; }

        /// <summary>
        /// Regelliste
        /// </summary>
        public List<AuskunftRegelDto> regeln { get;set;}

        /// <summary>
        /// Gültigkeit Angebot
        /// </summary>
        public DateTime gueltigBis { get; set; }

        /// <summary>
        /// Nutzungsart
        /// </summary>
        public int nutzung { get; set; }

        //optionsflags
        public AngAntOptionDto options { get; set; }


        /// <summary>
        /// Marke+modell+typ
        /// </summary>
        public String objektVT { get; set; }

        /// <summary>
        /// Benutzer
        /// </summary>
        public String benutzer { get; set; }

        public long sysLs { get; set; }

        /// <summary>
        /// Verweis zum Kanal (FF, KF)
        /// </summary>
        public long sysprchannel { get; set; }
        public long sysangebot { get; set; }
        public String extratingcode { get; set; }
        public String beschrdeutsch { get; set; }
        /// <summary>
        /// Antragskonto, kontoref mit syskd + antrag verknüpft
        /// </summary>
        public KontoDto konto { get; set; }


        
        public long sysPrProduct { get; set; }
        public long sysVttyp { get; set; }
        public long sysVk { get; set; }

 
    }


    
}