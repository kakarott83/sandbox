using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;

namespace Cic.One.Web.DAO
{
    /// <summary>
    /// Interface der Dokumentensuche
    /// </summary>
    public interface IDocumentSearchDao
    {
        /// <summary>
        /// Meldet sich am Server an
        /// </summary>
        /// <returns>true falls erfolgreich</returns>
        bool Login();

        /// <summary>
        /// Meldet sich am Server an
        /// </summary>
        /// <param name="ProfileName">Profilname, welcher verwendet werden soll</param>
        /// <returns>true falls erfolgreich</returns>
        bool Login(string ProfileName);

        /// <summary>
        /// Meldet sich am Server ab
        /// </summary>
        /// <returns>true falls erfolgreich</returns>
        bool Logout();

        /// <summary>
        /// Sucht nach Dokumenten
        /// </summary>
        /// <param name="input">Parameter</param>
        /// <returns>Liste von Infos der gefundenen Elementen</returns>
        HitlistDto DynamicDocumentSearch(iDynamicDocumentSearchDto input);

        /// <summary>
        /// Lädt ein Dokument anhand dem Input
        /// </summary>
        /// <param name="input"></param>
        /// <returns>das Dokument als byte[]</returns>
        byte[] DocumentLoad(iDocumentLoadDto input);

        /// <summary>
        /// Liefert die Version der ITA WebSearch zurück
        /// </summary>
        /// <param name="info"></param>
        /// <returns>Information</returns>
        ogetVersionInfo getVersionInfo(igetVersionInfo input);

    }
}