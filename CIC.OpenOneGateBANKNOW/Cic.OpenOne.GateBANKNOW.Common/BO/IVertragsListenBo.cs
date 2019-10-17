using System;
using System.Collections.Generic;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Schnittstelle für VertragsListen
    /// </summary>
    public interface IVertragsListenBo
    {
        /// <summary>
        /// Exportiert eine Liste von laufenden Verträgen
        /// </summary>
        /// <param name="input">icreateListenExportDto</param>
        /// <returns></returns>
        ocreateListenExportDto createListenExport(icreateListenExportDto input);

        /// <summary>
        /// Erstellt eine Liste der bereitgestellten Dokumente
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sysPerson">Die sysId vom Vertriebspartner (sysOLTable)</param>
        /// <returns></returns>
        List<EaihotDto> listInboxServices(string code, long sysPerson);

        /// <summary>
        /// Holt eine Fertige Liste als Excel-Datei 
        /// </summary>
        /// <param name="sysEaiHot"></param>
        /// <returns>Excel-File as Byte-Array</returns>
        byte[] getListeExport(long sysEaiHot, long sysPEROLE);

        /// <summary>
        /// Löscht eine einzelne Vertragsliste 
        /// </summary>
        /// <param name="sysEaiHot"></param>
        /// <returns></returns>
        bool deleteListeExport(long sysEaiHot, long sysPEROLE);
    }
}
