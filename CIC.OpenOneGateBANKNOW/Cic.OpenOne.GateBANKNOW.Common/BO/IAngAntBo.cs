using System;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Util.Security;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Iinterface for Conversions from Angebot to Antrag
    /// </summary>
    public interface IAngAntBo
    {
        /// <summary>
        /// Delivers the Auflagen for the Antrag
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        String[] getAuflagen(long sysid, String isoCode);

        /// <summary>
        /// Delivers the "real"zustand composed of ZUSTAND and ATTRIBUT for a Antrag
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        String getAntragZustand(long sysid);

        /// <summary>
        /// Delivers the Antrag stati history
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        ZustandDto[] getZustaende(long sysid, String isoCode);

        /// <summary>
        /// Angebot to Antrag Conversion
        /// </summary>
        /// <param name="Angebot">Anbeit input</param>
        /// <returns>Antrag Output</returns>
        AntragDto processAngebotToAntrag(AngebotDto Angebot);

        /// <summary>
        /// copyNotizenAngebotToAntrag
        /// Ticket#2012083110000047 — Übernahme Memos in den Antrag 
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        /// <param name="antragErfClient"></param>
        void copyNotizenAngebotToAntrag(long angebotSysId, long antragSysId, long antragErfClient);

        /// <summary>
        /// Übernahme Dokumente vom Angebot in den Antrag
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        void copyDms(long angebotSysId, long antragSysId);

        /// <summary>
        /// Neues Angebot erzeugen
        /// </summary>
        /// <param name="angebotInput">Angebot Eingang</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Angebot Ausgang</returns>
        AngebotDto createAngebot(AngebotDto angebotInput, long sysperole);

        /// <summary>
        /// Neues Angebot Dto erzeugen
        /// </summary>
        /// <param name="angebotInput">Angebot Dto</param>
        /// <param name="aktivKz"></param>
        /// <param name="endeKz"></param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Angebot Dto</returns>
        AngebotDto createAngebot(AngebotDto angebotInput, int? aktivKz, int? endeKz, long sysperole);
        
        /// <summary>
        /// Angebot aktualisieren
        /// </summary>
        /// <param name="ang">Angebot Eingang</param>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>Angebot Ausgang</returns>
        AngebotDto updateAngebot(AngebotDto ang, long sysperole);

        /// <summary>
        /// Neues Angebot erstellen oder bestehendes Angebot laden
        /// </summary>
        /// <param name="ang">Angebot Eingang</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Angebot Ausgang</returns>
        AngebotDto createOrUpdateAngebot(AngebotDto ang, long sysperole);

        /// <summary>
        /// Bestehendes Angebot holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Angebot Ausgang</returns>
        AngebotDto getAngebot(long sysid);

        /// <summary>
        /// Angebot löschen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        void deleteAngebot(long sysid);

        /// <summary>
        /// Angebot kopieren
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <param name="sysperole">User Id</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        AngebotDto copyAngebot(long sysid, long sysperole, String isoCode);

        /// <summary>
        /// Bestehenden Antrag holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Antrag Ausgang</returns>
        AntragDto getAntrag(long sysid,long sysperole);

        /// <summary>
        /// Antrag kopieren
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <param name="sysperole">User Id</param>
        /// <param name="b2b">b2b</param>
        /// <returns></returns>
        AntragDto copyAntrag(long sysid, long sysperole, bool b2b);

        /// <summary>
        /// Antrag löschen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        void deleteAntrag(long sysid);

        /// <summary>
        /// Neuen Antrag erstellen oder bestehenden Antrag updaten
        /// </summary>
        /// <param name="antrag">Antrag Eingang</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Antrag Ausgang</returns>
        AntragDto createOrUpdateAntrag(AntragDto antrag, long sysperole);


        /// <summary>
        /// Leeren Antrag anhand von SysNkk erzeugen.
        /// </summary>
        /// <param name="sysNkk"></param>
        /// <param name="syswfuser"></param>
        /// <param name="sysperole"></param>
        /// <param name="ISOlanguageCode"></param>
        /// <returns>Antrag Id</returns>
        AntragDto createAntragFromNkk(long sysNkk, long syswfuser, long sysperole, string ISOlanguageCode);


        /// <summary>
        /// Angebot prüfen
        /// </summary>
        /// <param name="sysid">Angebot Eingang</param>
        /// <param name="sysvart">VertragsartID</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns>ocheckAngebotDto</returns>
        ocheckAntAngDto checkAngebotById(long sysid, long sysvart, String isoCode);


        /// <summary>
        /// Kalkulation prüfen
        /// </summary>
        /// <param name="kalkulation"></param>
        /// <param name="kontext"></param>
        /// <param name="isoCode"></param>
        /// <param name="angAntObDto"></param>
        /// <returns></returns>
        ocheckAntAngDto checkAngebot(KalkulationDto kalkulation, prKontextDto kontext, String isoCode, AngAntObSmallDto angAntObDto);

        /// <summary>
        /// Antrag einreichen und daraus einen Vertrag generieren.
        /// </summary>
        /// <param name="antrag">Eingehender Antrag</param>
        /// <param name="syswfuser">Benutzerkennung</param>
        /// <param name="isocode">ISO-Code Sprache</param>
        void processAntragEinreichung(AntragDto antrag, long syswfuser, string isocode);

        /// <summary>
        /// Angebot einreichen
        /// </summary>
        /// <param name="angebot">Eingehendes Angebot</param>
        /// <param name="user">Benutzerkennung</param>
        /// <param name="isocode">Sprach ID</param>
        void processAngebotEinreichung(AngebotDto angebot, long user, string isocode);

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="key">Schlüssel</param>
        /// <returns>Daten</returns>
        AngAntObDto getObjektdaten(String key);

        /// <summary>
        /// getObjektdatenByVIN
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="syswfuser">syswfuser</param>
        /// <param name="ISOlanguageCode">ISOlanguageCode</param>
        /// <returns></returns>
        AngAntObDto getObjektdatenByVIN(String key, long syswfuser, string ISOlanguageCode, long sysid = 0, string area = ""); //BNRZW-1724

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="sysobtyp">Schlüssel</param>
        /// <returns>Daten</returns>
        AngAntObDto getObjektdaten(long sysobtyp);

        /// <summary>
        /// getAntragBezeichnungen
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        AntragDto getAntragBezeichnungen(AntragDto antrag);

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
        ocheckAntAngDto checkAntragByIdErweiterung(long sysid, long sysvart, String isoCode, bool b2b, bool nurallgemeine, long sysprprod, long syswfuser, long sysperole);

        /// <summary>
        /// preisschildDruck
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        oPreisschildDruckDto preisschildDruck(iPreisschildDruckDto input);

        /// <summary>
        /// speichern sysvttyp in Angebot;
        /// </summary>
        /// <param name="sysangebot"></param>
        void updateVttypinAngebot(long sysangebot);
        
        /// <summary>
        /// Erstellt einen neuen Vertrag, der eine Restwertverlängerung des gegebenen Vertrags darstellt
        /// </summary>
        /// <param name="sysVorvertrag">original contract's syscode</param>
        /// <returns>extended contract</returns>
        AntragDto createExtendedContract(CredentialContext cctx, long sysVorvertrag, int wsclient);

        /// <summary>
        /// Vertrag via ID auslesen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Daten</returns>
       VertragDto getVertrag(long sysid, long sysperole);

        /// <summary>
        /// Finanzierungsvarianten Drucken
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        oFinVariantenDruckenDto finanzierungsvariantenDrucken(iFinVariantenDruckenDto input);

		/// <summary>
		/// Returns Vorname und Name from SYSWFUSER (WfUser)
		/// </summary>
		/// <param name="sysWfUser"></param>
		/// <returns></returns>
		String getWfUserBezeichnung (long? sysWfUser);

	}
}
