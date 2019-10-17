using System.Collections.Generic;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using Cic.OpenOne.Common.DTO;
using System;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Schnitstelle des EAIHOT DAO
    /// </summary>
    public interface IEaihotDao
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eaihotDto"></param>
        /// <returns></returns>
        EaihotDto updateEaihot(EaihotDto eaihotDto);

        /// <summary>
        /// activates an eaihot
        /// </summary>
        /// <param name="eaihotInput"></param>
        /// <param name="eve"></param>
        void activateEaihot(EaihotDto eaihotInput, int eve);

        /// <summary>
        /// Neuen Eaihot erstellen oder bestehenden Eaihot updaten
        /// </summary>
        /// <param name="Eaihot">Eaihot Eingang</param>
        /// <returns>Eaihot Ausgang</returns>
        EaihotDto createEaihot(EaihotDto Eaihot);

        /// <summary>
        /// Neuen Eaihot erstellen oder bestehenden Eaihot updaten
        /// </summary>
        /// <param name="Eaihot">Eaihot Eingang</param>
        /// <returns>Eaihot Ausgang</returns>
        EaihotDto createEaihotWithoutEaiArt(EaihotDto eaihotInput);

        /// <summary>
        /// Bestehendes Eaihot holen
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        /// <returns>Eaihot Ausgang</returns>
        EaihotDto getEaihot(long syseaihot);

        /// <summary>
        /// returns the eaihot for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        EaihotDto getEaihotByQuery(String query);

        /// <summary>
        /// Eaihot löschen
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        bool deleteEaihot(long syseaihot);

        /// <summary>
        /// EaihotFile löschen
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        bool deleteEaihotFile(long syseaihot);

        /// <summary>
        /// Eaihot kopieren
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        /// <returns>EAIHOT Daten</returns>
        EaihotDto copyEaihot(long syseaihot);

        /// <summary>
        /// verifyAreaId
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysAreaId"></param>
        /// <returns></returns>
        bool verifyAreaId(AreaConstants area, long sysAreaId);

        /// <summary>
        /// EAI Art auslesen
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>EAI Art Daten</returns>
        EAIART getEaiArt(string code);

        /// <summary>
        /// EAIHOT aus altem Tabellencode und Sysart ermittlen
        /// </summary>
        /// <param name="sysid">sysID</param>
        /// <param name="oltable">Alte Tabellen ID</param>
        /// <param name="code">CODE</param>
        /// <param name="syseaiart">EAI Art ID</param>
        /// <returns>EAIHOT Daten</returns>
        EaihotDto getEaiHotByOltableAndCodeAndSysart(long sysid, string oltable, string code, long syseaiart);

        /// <summary>
        /// EAI Queue Out's für EAIHOT ermitteln
        /// </summary>
        /// <param name="eaihot">EAIHOT</param>
        /// <returns>Liste mit Ausgangsqueues</returns>
        List<EaiqoutDto> listEaiqouForEaihot(long syseaihot, bool sort);

        /// <summary>
        /// EAI Queue In für EAIHOT ermittlen
        /// </summary>
        /// <param name="eaihot">EAIHOT</param>
        /// <returns>Liste mit Eingangsqueues</returns>
        List<EaiqinDto> listEaiqinForEaihot(long syseaihot);

        /// <summary>
        /// EAI Eingangsqueue erzeugen
        /// </summary>
        /// <param name="eaiqinInput">EAIQIN Eingangsdaten</param>
        /// <returns>EAIQIN Daten</returns>
        EaiqinDto createEaiqin(EaiqinDto eaiqinInput);

        /// <summary>
        /// EAI Eingangsqueue erzeugen
        /// </summary>
        /// <param name="eaiqinInput">EAIQIN Eingangsdaten</param>
        /// <returns>EAIQIN Daten</returns>
        void createEaiqin(List<EaiqinDto> eaiqinInputs);

        /// <summary>
        /// EAI Ausgangsqueue erzeugen
        /// </summary>
        /// <param name="eaiqinInput">EAIQOU Ausgangsdaten</param>
        void createEaiqout(List<EaiqoutDto> eaiqoutInputs);

        /// <summary>
        /// EAIHFILE des EAIHOT auslesen
        /// </summary>
        /// <param name="eaihot">EAIHOT</param>
        /// <returns>EAIHFILE Daten</returns>
        EaihfileDto getEaiHotFile(long eaihot);

        /// <summary>
        /// Returns the eaihfile by direct id
        /// </summary>
        /// <param name="syseaihfile"></param>
        /// <returns></returns>
        EaihfileDto getEaiHFile(long syseaihfile);

        /// <summary>
        /// EAIHFile anlegen
        /// </summary>
        /// <param name="eaihfile">EAIHFILE Daten</param>
        void createEaihfile(EaihfileDto eaihfile);

        /// <summary>
        /// WFTX holen
        /// </summary>
        /// <param name="sysctlang">Sprach ID</param>
        /// <param name="syswftx">WFTX ID</param>
        /// <returns>String</returns>
        string getWFTX(int sysctlang, int syswftx);

        /// <summary>
        /// EAIHOT's nach alter TableID und SYSART auflisten
        /// </summary>
        /// <param name="sysid">SYSID</param>
        /// <param name="oltable">alte Table ID</param>
        /// <param name="code">CODE</param>
        /// <param name="syseaiart">SYS EAI Art</param>
        /// <returns>Liste mit EAIHOT's</returns>
        List<EaihotDto> listEaiHotByOltableAndCodeAndSysart(long sysid, string oltable, string code, long syseaiart);

        /// <summary>
        /// Liste von EAIHOT aus Code und sysPerson ermittlen
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sysVP"></param>
        /// <returns></returns>
        List<EaihotDto> listEaiHotForCodeAndPerson(string code, long sysVP);
    }
}