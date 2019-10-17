using System;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Schnittstelle für Angebot/Anttrag drucken
    /// </summary>
    public interface IPrintAngAntBo
    {
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
        DokumenteDto[] listAvailableDokumente(DTO.Enums.EaiHotOltable type, long sysid, long sysperson, String userLanguage, String subArea, long sysperole);
        DokumenteDto[] listAvailableDokumente(DTO.Enums.EaiHotOltable type, long sysid, long sysperson, String userLanguage, String subArea, bool sort, long sysperole);

        /// <summary>
        /// Verfügbare Dokumente auflisten
        /// </summary>
        /// <param name="dokumente">Dokumentenliste</param>
        /// <param name="sysid">Primärschlüssel</param>
        /// <param name="type">Typ</param>
        /// <param name="sysperson">Personen ID</param>
        /// <param name="eCodeEintragund">eCodeEintragund</param>
        /// <returns></returns>
        byte[] printCheckedDokumente(DokumenteDto[] dokumente, long sysid,  long? variantenid,DTO.Enums.EaiHotOltable type, long sysperson, bool eCodeEintragund, ref long syseaihot);
        byte[] printCheckedDokumente(DokumenteDto[] dokumente, long sysid, long? variantenid, DTO.Enums.EaiHotOltable type, long sysperson, bool eCodeEintragund);

        /// <summary>
        /// Ausgewählte Dokumente drucken
        /// </summary>
        /// <param name="dokumente">Dokumentenliste</param>
        /// <param name="sysid">Primärschlüssel</param>
        /// <param name="type">Typ</param>
        /// <param name="sysperson">Personen ID</param>
        /// <returns></returns>
        byte[] checkPrintCheckedDokumente(DokumenteDto[] dokumente, long sysid,  long? variantenid,DTO.Enums.EaiHotOltable type, long sysperson);
    }
}