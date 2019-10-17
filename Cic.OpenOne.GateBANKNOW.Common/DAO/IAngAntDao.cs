using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Angebot/Antrag Datenzugriffsklasse
    /// </summary>
    public interface IAngAntDao
    {
        /// <summary>
        /// Delivers the Auflagen for the Antrag
        /// </summary>
        /// <param name="sysid">Primaerschlüssel</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        String[] getAuflagen(long sysid, String isoCode);

        /// <summary>
        /// Delivers the Antrag stati history
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        ZustandDto[] getZustaende(long sysid, String isoCode);

         /// <summary>
        /// Zustand auslesen
        /// </summary>
        /// <param name="antraege">Antragsliste</param>
        void fetchStates(AntragDto[] antraege);

        /// <summary>
        /// Delivers the "real"zustand composed of ZUSTAND and ATTRIBUT for a Antrag
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        String getAntragZustand(long sysid);


        /// <summary>
        /// Update Zustand and Attribute fields in Angebot for usecase UC
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <param name="uc">Usecase</param>
        void setAngebotZustandAttribute(long sysid, String uc);

        /// <summary>
        /// Angebot löschen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        void deleteAngebot(long sysid);

        /// <summary>
        /// Angebotdaten holen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <returns>Daten</returns>
        AngebotDto getAngebot(long sysid);

        /// <summary>
        /// Neues Angebot erzeugen
        /// </summary>
        /// <param name="angebotInput">Eingabe Daten</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Daten</returns>
        AngebotDto createAngebot(AngebotDto angebotInput, long sysperole);

        /// <summary>
        /// Neues Angebot erstellen
        /// </summary>
        /// <param name="angebotInput">Angebot Eingabe</param>
        /// <param name="aktivKz"></param>
        /// <param name="endeKz"></param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Angebot Ausgabe</returns>
        AngebotDto createAngebot(AngebotDto angebotInput, int? aktivKz, int? endeKz, long sysperole);

        /// <summary>
        /// Angebot ändern
        /// </summary>
        /// <param name="ang">Eingabedaten</param>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>Daten</returns>
        AngebotDto updateAngebot(AngebotDto ang, long sysperole);

        /// <summary>
        /// Antrag via ID auslesen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Daten</returns>
        AntragDto getAntrag(long sysid, long sysperole);

        /// <summary>
        /// Neuen Antrag erzeugen
        /// </summary>
        /// <param name="antragInput">Eingabedaten</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Daten</returns>
        AntragDto createAntrag(AntragDto antragInput, long sysperole);

        /// <summary>
        /// Neuen Antrag erzeugen
        /// </summary>
        /// <param name="antragInput">Antrag Eingang</param>
        /// <param name="aktivKz"></param>
        /// <param name="endeKz"></param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Antrag Ausgang</returns>
        AntragDto createAntrag(AntragDto antragInput, int? aktivKz, int? endeKz, long sysperole);

        /// <summary>
        /// Antrag kopieren
        /// </summary>
        /// <param name="antragInput">Antrag Eingang</param>
        /// <param name="aktivKz"></param>
        /// <param name="endeKz"></param>
        /// <param name="sysperole">User Id</param>
        /// <param name="b2b">b2b</param>
        /// <returns>Antrag Ausgang</returns>
        AntragDto copyAntrag(AntragDto antragInput, int? aktivKz, int? endeKz, long sysperole, bool b2b);

        /// <summary>
        /// Antrag ändern
        /// </summary>
        /// <param name="antragInput">Eingabedaten</param>
        /// <param name="sysperole">Personenrollen ID</param>
        /// <returns>Daten</returns>
        AntragDto updateAntrag(AntragDto antragInput, long sysperole);

        /// <summary>
        /// Antrag löschen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        void deleteAntrag(long sysid);

        /// <summary>
        /// Antrag einreichen und daraus einen Vertrag generieren.
        /// </summary>
        /// <param name="antrag">Eingehender Antrag</param>
        /// <param name="syswfuser">Benutzerkennung</param>
        /// <param name="isocode">ISO-Code Sprache</param>
        void processAntragEinreichung(AntragDto antrag, long syswfuser, string isocode);

        /// <summary>
        /// B2C Angebot einreichen
        /// </summary>
        /// <param name="angebot">Eingehendes Angebot</param>
        /// <param name="userid">Einreichender Benutzer</param>
        /// <param name="isocode">Sprachcode</param>
        /// <returns>Ausgehender Vertrag</returns>
        void processAngebotEinreichung(AngebotDto angebot, long userid, string isocode);

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="key">Schlüssel</param>
        /// <returns>Daten</returns>
        AngAntObDto getObjektdaten(String key);

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="sysobtyp">ObjectType</param>
        /// <returns>Daten</returns>
        AngAntObDto getObjektdaten(long sysobtyp);

        /// <summary>
        /// Objektdaten auslesen über Nkk anhand des SYSOBTYPs
        /// </summary>
        /// <param name="sysNkk">Nkk</param>
        /// <returns>Daten</returns>
        AngAntObDto getObjektdatenFromNkk(long sysNkk);


        /// <summary>
        /// Objektdaten auslesen über Nkk aus Tabelle OB/OBBRIEF
        /// </summary>
        /// <param name="sysNkk">Nkk</param>
        /// <returns>Daten</returns>
        AngAntObDto getObjektdatenFromOB(long sysNkk);

        /// <summary>
        /// Gibt den VinCode/Fident anhand der sysnkk zurück
        /// </summary>
        /// <param name="sysNkk"></param>
        /// <returns></returns>
        string getVinCodeFromNkk(long sysNkk);

        /// <summary>
        /// VartCode
        /// </summary>
        /// <param name="sysvart"></param>
        /// <returns></returns>
        VartDto getVart(long sysvart);

        /// <summary>
        /// VsArtCode
        /// </summary>
        /// <param name="sysvstyp"></param>
        /// <returns></returns>
        string getVsArtCode(long sysvstyp);


        /// <summary>
        /// istRatenabsicherung
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="sysvstyp"></param>
        /// <returns></returns>
        bool istRatenabsicherung(long sysprproduct, long sysvstyp);


        /// <summary>
        /// getAntragBezeichnungen
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        AntragDto getAntragBezeichnungen(AntragDto antrag);

        /// <summary>
        /// deleteNotiz
        /// </summary>
        /// <param name="kategorieBezeichnung"></param>
        /// <param name="sysLease"></param>
        void deleteNotiz(String kategorieBezeichnung, long sysLease);

        /// <summary>
        /// createOrUpdateNotiz
        /// </summary>
        /// <param name="kategorieBezeichnung"></param>
        /// <param name="antragInput"></param>
        /// <param name="sysPerson"></param>
        void createOrUpdateNotiz(String kategorieBezeichnung, AntragDto antragInput, long? sysPerson);

        /// <summary>
        /// copyNotizenAngebotToAntrag
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        void copyNotizenAngebotToAntrag(long angebotSysId, long antragSysId);

        /// <summary>
        /// Produkt-Prüfung:
        /// Liefert diese Methode einen Wert größer 0, dann wurde eine fakultative Ratenabsicherung gewählt.
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <returns></returns>
        long getFakultativeRatenabsicherung(long sysid);

        /// <summary>
        /// Produkt-Prüfung:
        /// Liefert das Eigentümer-Seit-Datum für den Antrag
        /// </summary>
        /// <param name="sysid">Antrag-Id</param>
        /// <returns></returns>
        DateTime getEigentuemerSeit(long sysid);

        /// <summary>
        /// getSysVglgd
        /// </summary>
        /// <param name="sysobtyp">sysobtyp</param>
        /// <returns></returns>
        long? getSysVglgd(long sysobtyp);

        /// <summary>
        /// getV_cluster
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        VClusterDto getV_cluster(long sysid);

        /// <summary>
        /// save Clustervalues in Antrag
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="vClusterDto"></param>
        void saveClusterInAntrag(long sysid, VClusterDto vClusterDto);

        /// <summary>
        /// getFform
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        String getFform(long sysid);

        /// <summary>
        /// getScorebezeichnung
        /// </summary>
        /// <param name="sysid">sysid)</param>
        /// <returns></returns>
        String getScorebezeichnung(long sysid);

        /// <summary>
        /// getStraccount
        /// </summary>
        /// <param name="sysvm"></param>
        int getStraccount(long sysvm);

        /// <summary>
        /// getFlagBwgarantie
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        int getFlagBwgarantie(long sysid);

        /// <summary>
        /// getScoreInDedetailBySysantrag
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        int getScoreInDedetailBySysantrag(long sysid);

        /// <summary>
        /// Prkgroup aus RATINGSIMUL
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        long getPrkgroupByAntragID(long sysid);

        /// <summary>
        /// getObtypBySchwacke
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        long getObtypBySchwacke(string schwacke);

        /// <summary>
        /// Übernahme Dokumente vom Angebot in den Antrag
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        void copyDms(long angebotSysId, long antragSysId);

        /// <summary>
        /// speichern sysvttyp in Angebot
        /// </summary>
        /// <param name="sysangebot"></param>
        void updateVttypinAngebot(long sysangebot);
        
        /// <summary>
        /// check if the contract is allowed to be extended CR139
        /// </summary>
        /// <returns>contract is allowed to be extended</returns>
        bool checkRwVerlVerfuegbarWeb(long sysvt);

        /// <summary>
        /// Vertrag via ID auslesen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Daten</returns>
        VertragDto getVertrag(long sysid, long sysperole);

        /// <summary>
        /// createZusatzdaten4ExtendedContract
        /// </summary>
        /// <param name="sysAntragOld"></param>
        /// <param name="sysAntragNew"></param>
        void createZusatzdaten4ExtendedContract(long sysit, long syskd, long sysAntragNew);

        /// <summary>
        /// Creates a MA (Sicherheit in ANTOBSICH) for the given antragid, KundeDto (sysit has to be filled) 
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="mitantragsteller"></param>
        /// <param name="maTyp">required, default sichtyprang, can be overridden bei mitantragsteller.sichtyprang</param>
        void createMitantragsteller(long sysid, KundeDto mitantragsteller, int maTyp);

		/// <summary>
		/// Returns Vorname und Name from SYSWFUSER (WfUser)
		/// </summary>
		/// <param name="sysWfUser"></param>
		/// <returns></returns>
		String getWfUserBezeichnung (long? sysWfUser);


    }
}