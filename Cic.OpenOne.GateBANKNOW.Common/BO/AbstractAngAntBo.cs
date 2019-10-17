using System;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Util.Security;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abbstarct class for Offer and Application Operations
    /// </summary>
    public abstract class AbstractAngAntBo : IAngAntBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IAngAntDao angAntDao;
        /// <summary>
        /// 
        /// </summary>
        protected IPrismaParameterBo prismaParameterBo;
        /// <summary>
        /// Kunde DAO
        /// </summary>
        protected IKundeDao kundeDao;
        /// <summary>
        /// Ueberstzungs DAO
        /// </summary>
        protected ITranslateBo translateBo;

        /// <summary>
        /// qoute Dao
        /// </summary>
        protected IQuoteDao quoteDao;

        /// <summary>
        /// VG DAO
        /// </summary>
        protected IVGDao vgDao;

        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IEaihotDao eaihotDao;

        /// <summary>
        /// Transaction Risiko Bo
        /// </summary>
        protected ITransactionRisikoBo trBo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="angAntDao">Angenbot/Antrag DAO</param>
        /// <param name="kundeDao">Kundendaten DAO</param>
        /// <param name="prismaParameterBo">Prisma Paramter BO</param>
        /// <param name="translateBo">Übersetzungs BO</param>
        /// <param name="quoteDao">Quote Dao</param>
        /// <param name="vgDao">VG Dao</param>
        /// <param name="eaihotDao">eai Dao</param>
        public AbstractAngAntBo(IAngAntDao angAntDao, IKundeDao kundeDao, IPrismaParameterBo prismaParameterBo, ITranslateBo translateBo, IQuoteDao quoteDao, IVGDao vgDao, IEaihotDao eaihotDao, ITransactionRisikoBo trBo)
        {
            this.angAntDao = angAntDao;
            this.prismaParameterBo = prismaParameterBo;
            this.translateBo = translateBo;
            this.kundeDao = kundeDao;
            this.quoteDao = quoteDao;
            this.vgDao = vgDao;
            this.eaihotDao = eaihotDao;
            this.trBo = trBo;
        }

        /// <summary>
        /// Delivers the Auflagen for the Antrag
        /// </summary>
        /// <param name="sysid">Primaeschluessel</param>
        /// <param name="isoCode">Iso Sprachencode</param>
        /// <returns></returns>
        public abstract String[] getAuflagen(long sysid, String isoCode);

        /// <summary>
        /// Delivers the Antrag stati history
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        public abstract ZustandDto[] getZustaende(long sysid, String isoCode);

        /// <summary>
        /// Delivers the "real"zustand composed of ZUSTAND and ATTRIBUT for a Antrag
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public abstract String getAntragZustand(long sysid);

        /// <summary>
        /// Create or Update Offer
        /// </summary>
        /// <param name="ang">Offer Data</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Offer Data</returns>
        public abstract AngebotDto createOrUpdateAngebot(AngebotDto ang, long sysperole);

        /// <summary>
        /// Transfer Offer to Application
        /// </summary>
        /// <param name="Angebot">Offer Input</param>
        /// <returns>Application Output</returns>
        public abstract AntragDto processAngebotToAntrag(AngebotDto Angebot);

        /// <summary>
        /// copyNotizenAngebotToAntrag
        /// Ticket#2012083110000047 — Übernahme Memos in den Antrag 
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        /// <param name="antragErfClient"></param>
        public abstract void copyNotizenAngebotToAntrag(long angebotSysId, long antragSysId, long antragErfClient);

        /// <summary>
        /// Angebebot holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Angebot Dto</returns>
        public abstract AngebotDto getAngebot(long sysid);

        /// <summary>
        /// Neues Angebot Dto erzeugen
        /// </summary>
        /// <param name="angebotInput">Angebot Dto</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Angebot Dto</returns>
        public abstract AngebotDto createAngebot(AngebotDto angebotInput, long sysperole);

        /// <summary>
        /// Neues Angebot Dto erzeugen
        /// </summary>
        /// <param name="angebotInput">Angebot Dto</param>
        /// <param name="aktivKz"></param>
        /// <param name="endeKz"></param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Angebot Dto</returns>
        public abstract AngebotDto createAngebot(AngebotDto angebotInput, int? aktivKz, int? endeKz, long sysperole);

        /// <summary>
        /// Angebot Aktualisieren
        /// </summary>
        /// <param name="ang">Angebot Dto</param>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>Angebot Dto</returns>
        public abstract AngebotDto updateAngebot(AngebotDto ang, long sysperole);

        /// <summary>
        /// Anbegot löschen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        public abstract void deleteAngebot(long sysid);

        /// <summary>
        /// Angebot Kopieren
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <param name="sysperole">User Id</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns>Angebot</returns>
        public abstract AngebotDto copyAngebot(long sysid, long sysperole, String isoCode);

        /// <summary>
        /// Angebebot holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Angebot Dto</returns>
        public abstract AntragDto getAntrag(long sysid,long sysperole);

        /// <summary>
        /// Antrag kopieren
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysperole">User Id</param>
        /// <param name="b2b">b2b</param>
        /// <returns></returns>
        public abstract AntragDto copyAntrag(long sysid, long sysperole, bool b2b);

        /// <summary>
        /// Antrag löschen
        /// </summary>
        /// <param name="sysid"></param>
        public abstract void deleteAntrag(long sysid);

        /// <summary>
        /// Antrag erstellen oder updaten
        /// </summary>
        /// <param name="antrag"></param>
        /// <param name="sysperole">User Id</param>
        /// <returns></returns>
        public abstract AntragDto createOrUpdateAntrag(AntragDto antrag, long sysperole);

        /// <summary>
        /// Leeren Antrag anhand von SysNkk erzeugen.
        /// </summary>
        /// <param name="sysNkk"></param>
        /// <param name="syswfuser"></param>
        /// <param name="sysperole"></param>
        /// <param name="ISOlanguageCode"></param>
        /// <returns>Antrag Id</returns>
        public abstract AntragDto createAntragFromNkk(long sysNkk, long syswfuser, long sysperole, string ISOlanguageCode);


        /// <summary>
        /// Angebot prüfen
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="sysvart">Vertragsart</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        public abstract ocheckAntAngDto checkAngebotById(long sysid, long sysvart, String isoCode);


        /// <summary>
        /// Kalkulation prüfen
        /// </summary>
        /// <param name="kalkulation"></param>
        /// <param name="kontext"></param>
        /// <param name="isoCode"></param>
        /// <param name="angAntObDto"></param>
        /// <returns></returns>
        public abstract ocheckAntAngDto checkAngebot(KalkulationDto kalkulation, prKontextDto kontext, String isoCode, AngAntObSmallDto angAntObDto);

        /// <summary>
        /// Antrag einreichen und daraus einen Vertrag generieren.
        /// </summary>
        /// <param name="antrag">Eingehender Antrag</param>
        /// <param name="user">Benutzerkennung</param>
        /// <param name="isocode">Sprache ID</param>
        public abstract void processAntragEinreichung(AntragDto antrag, long user, string isocode);

        /// <summary>
        /// Angebot einreichen
        /// </summary>
        /// <param name="angebot">Eingehendes Angebot</param>
        /// <param name="user">Benutzerkennung</param>
        /// <param name="isocode">Sprach ID</param>
        public abstract void processAngebotEinreichung(AngebotDto angebot, long user, string isocode);

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="key">Schlüssel</param>
        /// <returns>Daten</returns>
        public abstract AngAntObDto getObjektdaten(String key);

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="sysobtyp">Schlüssel</param>
        /// <returns>Daten</returns>
        public abstract AngAntObDto getObjektdaten(long sysobtyp);


        /// <summary>
        /// Objektdaten aus EurotaxVin
        /// </summary>
        /// <param name="vincode">vincode</param>
        /// <param name="syswfuser">syswfuser</param>
        /// <param name="ISOlanguageCode">ISOlanguageCode</param>
        /// <returns></returns>
        public abstract AngAntObDto getObjektdatenByVIN(string vincode, long syswfuser, string ISOlanguageCode, long sysid = 0, string area = ""); //BNRZW-1724

        /// <summary>
        /// getAntragBezeichnungen
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        public abstract AntragDto getAntragBezeichnungen(AntragDto antrag);

        /// <summary>
        /// checkAntragById / PRODUKTPRÜFUNG
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="sysvart">Vertragsart</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <param name="b2b">B2B Flag</param>
        /// <param name="nurallgemeine">nurallgemeine</param>
        /// <param name="sysprprod">id des Produktes</param>
        /// <param name="syswfuser">syswfuser</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns></returns>
        public abstract ocheckAntAngDto checkAntragByIdErweiterung(long sysid, long sysvart, String isoCode, bool b2b, bool nurallgemeine, long sysprprod, long syswfuser, long sysperole);

        /// <summary>
        /// PreisschildDruck
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract oPreisschildDruckDto preisschildDruck(iPreisschildDruckDto input);

        /// <summary>
        /// Übernahme Dokumente vom Angebot in den Antrag
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        public abstract void copyDms(long angebotSysId, long antragSysId);

        /// <summary>
        /// save vttyp in ANGEBOT / b2c
        /// </summary>
        /// <param name="sysangebot"></param>
        public abstract void updateVttypinAngebot(long sysangebot);
        
        /// <summary>
        /// Erstellt einen neuen Vertrag, der eine Restwertverlängerung des gegebenen Vertrags darstellt
        /// </summary>
        /// <param name="sysVorvertrag">original contract's syscode</param>
        /// <returns>extended contract</returns>
        public abstract AntragDto createExtendedContract(CredentialContext cctx, long sysVorvertrag, int wsclient);

        /// <summary>
        /// Vertrag via ID auslesen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Daten</returns>
        public abstract VertragDto getVertrag(long sysid, long sysperole);

        /// <summary>
        /// Finanzierungsvarianten Drucken
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        public abstract oFinVariantenDruckenDto finanzierungsvariantenDrucken(iFinVariantenDruckenDto input);

		/// <summary>
		/// Returns Vorname und Name from SYSWFUSER (WfUser)
		/// </summary>
		/// <param name="sysWfUser"></param>
		/// <returns></returns>
		public abstract String getWfUserBezeichnung (long? sysWfUser);

    }
}