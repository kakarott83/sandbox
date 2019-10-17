using System;
using System.Collections.Generic;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstrakte VertragsListen-Klasse
    /// </summary>
    public abstract class AbstractVertragsListenBo : IVertragsListenBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IEaihotDao eaihotDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eaihotDao">EAIHOT DAO</param>
        public AbstractVertragsListenBo(IEaihotDao eaihotDao)
        {
            this.eaihotDao = eaihotDao;
        }


        /// <summary>
        /// Exportiert eine Liste von laufenden Verträgen
        /// </summary>
        /// <param name="input">icreateListenExportDto</param>
        /// <returns></returns>
        public abstract ocreateListenExportDto createListenExport(icreateListenExportDto input);

        /// <summary>
        /// Erstellt eine Liste der bereitgestellten Dokumente
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sysPerson">Die sysId vom Vertriebspartner (sysOLTable)</param>
        /// <returns></returns>
        public abstract List<EaihotDto> listInboxServices(string code, long sysPerson);

        /// <summary>
        /// Holt eine Fertige Liste als Excel-Datei 
        /// </summary>
        /// <param name="sysEaiHot"></param>
        /// <returns>Excel-File as Byte-Array</returns>
        public abstract byte[] getListeExport(long sysEaiHot, long sysPEROLE);

        /// <summary>
        /// Löscht eine einzelne Vertragsliste 
        /// </summary>
        /// <param name="sysEaiHot"></param>
        /// <returns></returns>
        public abstract bool deleteListeExport(long sysEaiHot, long sysPEROLE);
    }
}
