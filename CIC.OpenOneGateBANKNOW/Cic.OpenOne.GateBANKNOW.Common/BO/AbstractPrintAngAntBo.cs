using System;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstrake Angebot/Anbtrag drucken BO Klasse
    /// </summary>
    public abstract class AbstractPrintAngAntBo : IPrintAngAntBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IEaihotDao eaihotDao;
        /// <summary>
        /// Antrag/Angebot DAO
        /// </summary>
        protected IAngAntDao angAntDao;
        /// <summary>
        /// Dictionary List DAO
        /// </summary>
        protected IDictionaryListsDao dictionaryListDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eaihotDao">EAIHOT DAO</param>
        /// <param name="angAntDao">Angebot/Antrag DAO</param>
        /// <param name="dictionaryListDao">Dictionary Liste DAO</param>
        public AbstractPrintAngAntBo(IEaihotDao eaihotDao, IAngAntDao angAntDao, IDictionaryListsDao dictionaryListDao)
        {
            this.eaihotDao = eaihotDao;
            this.angAntDao = angAntDao;
            this.dictionaryListDao = dictionaryListDao;
        }

        /// <summary>
        /// Verfügbare Dokumente auflisten
        /// </summary>
        /// <param name="type">Typ</param>
        /// <param name="sysid">Primärschlüssel</param>
        /// <param name="sysperson">Personen ID</param>
        /// <param name="userLanguage">userLanguage</param>
        /// <param name="subArea">subArea</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Dokumenten ID</returns>
        public abstract DTO.DokumenteDto[] listAvailableDokumente(DTO.Enums.EaiHotOltable type, long sysid, long sysperson, String userLanguage, String subArea, long sysperole);
        public abstract DTO.DokumenteDto[] listAvailableDokumente(DTO.Enums.EaiHotOltable type, long sysid, long sysperson, String userLanguage, String subArea, bool sort, long sysperole);

        /// <summary>
        /// Ausgewählte Dokumente drucken
        /// </summary>
        /// <param name="dokumente">Dokumentenliste</param>
        /// <param name="sysid">Primärschlüssel</param>
        /// <param name="type">Typ</param>
        /// <param name="sysperson">Personen ID</param>
        /// <param name="eCodeEintragund">eCodeEintragund</param>
        /// <returns></returns>
        public abstract byte[] printCheckedDokumente(DokumenteDto[] dokumente, long sysid, long? variantenid,DTO.Enums.EaiHotOltable type, long sysperson, bool eCodeEintragund);
        public abstract byte[] printCheckedDokumente(DokumenteDto[] dokumente, long sysid, long? variantenid, DTO.Enums.EaiHotOltable type, long sysperson, bool eCodeEintragund, ref long syseaihot);

        /// <summary>
        /// Zu druckende Dokumente prüfen
        /// </summary>
        /// <param name="dokumente">Dokumentenliste</param>
        /// <param name="sysid">Primärschlüssel</param>
        /// <param name="type">Typ</param>
        /// <param name="sysperson">Personen ID</param>
        /// <returns></returns>
        public abstract byte[] checkPrintCheckedDokumente(DokumenteDto[] dokumente, long sysid,  long? variantenid,DTO.Enums.EaiHotOltable type, long sysperson);
    }
}