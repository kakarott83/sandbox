using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public enum BesuchsberichtTyp
    {
        Besuchsbericht = 0,
        Informationsbericht
    }

    public enum BesuchsberichtInvestitionstyp
    {
        Neuinvestition = 0,
        Ersatzinvestition
    }

    public enum JaNein
    {
        Nein=0,
        Ja
    }

    public enum Marktstellung
    {
        [Description("Normale Wettbewerbssituation")]
        normal = 0,
        [Description("Wettbewerbsvorteil")]
        vorteil,
        [Description("Ausgeprägtes Alleinstellungsmerkmal")]
        ausgepraegt
    }

    public enum Afatyp
    {
        [Description("Branchenspezifische Afa")]
        Branchenspezifische = 0,
        [Description("Allgemeine Afa")]
        Allgemeine
    }
    public enum GeschaeftsTyp
    {
        Kredit = 0,
        Leasing,
        Mietkauf
    }
    public enum PrivatGewerblich
    {
        privat = 0,
        gewerblich
    }

    public enum EigentuemerTyp
    {
        Kunde = 0,
        Gesellschafter,
        Familienmitglied
    }

    public enum Konditionen
    {
        Standardkonditionen = 0,
        [Description("Genehmigte Sonderkonditionen (beizulegen)")]
        GenehmigteSonderkonditionen
    }

    public enum MVOriginalantrag
    {
        [Description("liegt vor")]
        liegtVor = 0,
        fehlt
    }

    public enum GeschaeftsfuehrungQualifikation
    {
        [Description("sehr gut")]
        sehrGut = 0,
        gut,
        zufriedenstellend,
        [Description("nicht bewertbar")]
        nichtBewertbar,
        schlecht
    }

    public class BesuchsberichtDto : EntityDto
    {

        public long sysFileatt { get; set; }
        public long? sysId { get; set; }
        public string area { get; set; }
        public byte[] content { get; set; }

        public override long getEntityId()
        {
            return sysFileatt;
        }

        public int typInt { get; set; }

        public BesuchsberichtTyp typ
        {
            get
            {
                return (BesuchsberichtTyp)typInt;
            }
            set
            {
                typInt = (int)value;
            }
        }

        public string beraternummer { get; set; }

        public DateTime datum { get; set; }

        public string kundennummer { get; set; }

        public string kunde { get; set; }

        public string zusatz { get; set; }

        public string strasse { get; set; }

        public string plz { get; set; }

        public string ort { get; set; }

        public string antragsnummern { get; set; }

        public string gespraechsteilnehmer { get; set; }

        public string funktion { get; set; }

        public string beschreibung { get; set; }

        public string Objekt { get; set; }

        public double NettolistenpreisEUR { get; set; }
        public double NettokaufbreisEUR { get; set; }

        public int lieferantImAuslandInt { get; set; }
        public JaNein lieferantImAusland
        {
            get
            {
                return (JaNein)lieferantImAuslandInt;
            }
            set
            {
                lieferantImAuslandInt = (int)value;
            }
        }
        public int anzahlungsbuergschaftInt { get; set; }
        public JaNein anzahlungsbuergschaft
        {
            get
            {
                return (JaNein)anzahlungsbuergschaftInt;
            }
            set
            {
                anzahlungsbuergschaftInt = (int)value;
            }
        }
        public string besondereZahlungsvereinbarungen { get; set; }
        public int foerdermittelInt { get; set; }
        public JaNein foerdermittel
        {
            get
            {
                return (JaNein)foerdermittelInt;
            }
            set
            {
                foerdermittelInt = (int)value;
            }
        }
        public int investitionsTypInt { get; set; }
        public BesuchsberichtInvestitionstyp investitionsTyp
        {
            get
            {
                return (BesuchsberichtInvestitionstyp)investitionsTypInt;
            }
            set
            {
                investitionsTypInt = (int)value;
            }
        }

        public double bisherigeFinanzierungskosten { get; set; }
        public double bisherigeLeasingaufwendungen { get; set; }


        public int existierenRentabilitaetsberechnungenInt { get; set; }
        public JaNein existierenRentabilitaetsberechnungen
        {
            get
            {
                return (JaNein)existierenRentabilitaetsberechnungenInt;
            }
            set
            {
                existierenRentabilitaetsberechnungenInt = (int)value;
            }
        }

        public string investitionAusSachverhalt { get; set; }
        
        public int marktstellungInt { get; set; }
        public Marktstellung marktstellung
        {
            get
            {
                return (Marktstellung)marktstellungInt;
            }
            set
            {
                marktstellungInt = (int)value;
            }
        }

        public string wettbewerbsvorteildurch { get; set; }
        public string alleinstellungsmerkmaldurch { get; set; }

        //Abnehmer
        public string abnehmer1 { get; set; }
        public string abnehmer1AnteilUmsatz { get; set; }
        public string abnehmer2 { get; set; }
        public string abnehmer2AnteilUmsatz { get; set; }
        public string abnehmer3 { get; set; }
        public string abnehmer3AnteilUmsatz { get; set; }

        //wirtfin = Wirtschaftliche und finanzielle Verhältnisse

        public string wirtfinStilleReserven { get; set; }
        public string wirtfinImmaterielleWerte { get; set; }
        public string wirtfinFinanzbeteiligungen { get; set; }
        public string wirtfinSonstigeForderungen { get; set; }
        public string wirtfinSonstigeVerbindlichkeiten { get; set; }
        public string wirtfinKontokorrentlinien { get; set; }
        public double wirtfinAktuelleAuslastungEUR { get; set; }
        public double wirtfinLeasingaufwandEUR { get; set; }
        public string wirtfinSonstigeAufwendungen { get; set; }
        public string wirtfinSonstigeErtraege { get; set; }
        public string wirtfinSonstigeBemerkungen { get; set; }

        //Erläuterungen zur BWA
        public string bestandsveraenderungen { get; set; }
        public double geschaeftsfuehrerGehaelterEUR { get; set; }
        public string afa { get; set; }
        public string ergebnisprognose { get; set; }
        public double umsatzerwartungEUR { get; set; }
        public double umsatzVJEUR { get; set; }

        public string wirtschaftlicheVerflechtungen { get; set; }

        public int organigrammInt { get; set; }
        public JaNein organigramm
        {
            get
            {
                return (JaNein)organigrammInt;
            }
            set
            {
                organigrammInt = (int)value;
            }
        }

        public int anzahlMitarbeiter { get; set; }

        public int planzahlenInt { get; set; }
        public JaNein planzahlen
        {
            get
            {
                return (JaNein)planzahlenInt;
            }
            set
            {
                planzahlenInt = (int)value;
            }
        }

        public int bmBuergschaftsuebernahmeInt { get; set; }
        public JaNein bmBuergschaftsuebernahme
        {
            get
            {
                return (JaNein)bmBuergschaftsuebernahmeInt;
            }
            set
            {
                bmBuergschaftsuebernahmeInt = (int)value;
            }
        }
        
        //bm = besicherungsmöglichkeiten
        public string bmAndere { get; set; }

        public string SonstigeBemerkungen { get; set; }

        //immo = Immobilien
        public string immoArt { get; set; }
        public string immoAnschrift { get; set; }
        public double immoVerkehrswertEUR { get; set; }
        public double immoGrundschuldEUR { get; set; }
        public double immoRestschulfEUR { get; set; }

        public int immoEigentuemerTypInt { get; set; }
        public EigentuemerTyp immoEigentuemerTyp
        {
            get
            {
                return (EigentuemerTyp)immoEigentuemerTypInt;
            }
            set
            {
                immoEigentuemerTypInt = (int)value;
            }
        }

        public int immoIstGemietet { get; set; }
        public double immoJahresmieteEUR { get; set; }

        //gf = Angaben zur Geschäftsführung
        public int gfQualifikationInt { get; set; }
        public GeschaeftsfuehrungQualifikation gfQualifikation
        {
            get
            {
                return (GeschaeftsfuehrungQualifikation)gfQualifikationInt;
            }
            set
            {
                gfQualifikationInt = (int)value;
            }
        }
        public int gfBerufserfahrungJahre { get; set; }
        public double gfGehaltEUR { get; set; }
        public int gfNachfolgeregelungVorhandenInt { get; set; }
        public JaNein gfNachfolgeregelungVorhanden
        {
            get
            {
                return (JaNein)gfNachfolgeregelungVorhandenInt;
            }
            set
            {
                gfNachfolgeregelungVorhandenInt = (int)value;
            }
        }
        public int gfZweiteFuehrungsebeneVorhandenInt { get; set; }
        public JaNein gfZweiteFuehrungsebeneVorhanden
        {
            get
            {
                return (JaNein)gfZweiteFuehrungsebeneVorhandenInt;
            }
            set
            {
                gfZweiteFuehrungsebeneVorhandenInt = (int)value;
            }
        }
        public string gfBemerkungen { get; set; }

        //sb = Steuerberater / Wirtschaftsprüfer
        public string sbname { get; set; }
        public string sbtelefonnummer { get; set; }
        public int sbDarfAngesprochenWerdenInt { get; set; }
        public JaNein sbDarfAngesprochenWerden
        {
            get
            {
                return (JaNein)sbDarfAngesprochenWerdenInt;
            }
            set
            {
                sbDarfAngesprochenWerdenInt = (int)value;
            }
        }

        //mv = marktvotum

        public DateTime mvDatum { get; set; }
        public string mvSachbearbeiter { get; set; }
        public string mvAussendienstmitarbeiter { get; set; }
        public string mvKunde { get; set; }
        public string mvKundennummer { get; set; }
        public int mvAfaDauerInMonaten { get; set; }
        public string mvObjekt { get; set; }

        public int mvAfatypInt { get; set; }
        public Afatyp mvAfatyp
        {
            get
            {
                return (Afatyp)mvAfatypInt;
            }
            set
            {
                mvAfatypInt = (int)value;
            }
        }

        public int mvGeschaeftstypInt { get; set; }
        public GeschaeftsTyp mvGeschaeftstyp
        {
            get
            {
                return (GeschaeftsTyp)mvGeschaeftstypInt;
            }
            set
            {
                mvGeschaeftstypInt = (int)value;
            }
        }

        public int mvPrivatGewerblichInt { get; set; }
        public PrivatGewerblich mvPrivatGewerblich
        {
            get
            {
                return (PrivatGewerblich)mvPrivatGewerblichInt;
            }
            set
            {
                mvPrivatGewerblichInt = (int)value;
            }
        }

        public int mvOriginalantragInt { get; set; }
        public MVOriginalantrag mvOriginalantrag
        {
            get
            {
                return (MVOriginalantrag)mvOriginalantragInt;
            }
            set
            {
                mvOriginalantragInt = (int)value;
            }
        }

        public int mvKonditionenInt { get; set; }
        public Konditionen mvKonditionen
        {
            get
            {
                return (Konditionen)mvKonditionenInt;
            }
            set
            {
                mvKonditionenInt = (int)value;
            }
        }

        public int mvKalkulationLiegtVorInt { get; set; }
        public JaNein mvKalkulationLiegtVor
        {
            get
            {
                return (JaNein)mvKalkulationLiegtVorInt;
            }
            set
            {
                mvKalkulationLiegtVorInt = (int)value;
            }
        }

        public int mvAngebot { get; set; }
        public int mvBestellung { get; set; }
        public int mvRechnung { get; set; }
        public int mvAuftragsbestaetigung { get; set; }

        //rk = Risikoklasse
        //K gibt die Klasse an
        public int rkErforderlichePruefungsunterlagenK1 { get; set; }
        public int rkErforderlichePruefungsunterlagenK2 { get; set; }
        public int rkErforderlichePruefungsunterlagenK3 { get; set; }
        public int rkVCKundeK1 { get; set; }
        public int rkVCKundeK2 { get; set; }
        public int rkVCKundeK3 { get; set; }
        public int rkSelbstauskunftVollstaendigK1 { get; set; }
        public int rkSelbstauskunftVollstaendigK2 { get; set; }
        public int rkSelbstauskunftVollstaendigK3 { get; set; }
        public int rkBeiPrivatenMitNrK1 { get; set; }
        public int rkBeiPrivatenMitNrK2 { get; set; }
        public int rkBeiPrivatenMitNrK3 { get; set; }

        public int rkBankvollmachtK2 { get; set; }
        public int rkBankvollmachtK3 { get; set; }
        public int rkBankauskunftK2 { get; set; }
        public int rkBankauskunftK3 { get; set; }

        public int rkBWAK3 { get; set; }
        public int rkAktivaPassivaGuVK3 { get; set; }

        public int rkVCLieferantK1 { get; set; }
        public int rkVCLieferantK2 { get; set; }
        public int rkVCLieferantK3 { get; set; }

    }
}