using System.Collections.Generic;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using Cic.OpenOne.Common.DTO;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Angebot/ANtrag Drucken
    /// </summary>
    public class VertragsListenBo : AbstractVertragsListenBo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eaihotDao">EAIHOT Daten</param>
        public VertragsListenBo(IEaihotDao eaihotDao)
            : base(eaihotDao)
        {
        }

        /// <summary>
        /// Exportiert eine Liste von laufenden Verträgen
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override ocreateListenExportDto createListenExport(icreateListenExportDto input)
        {
            ocreateListenExportDto retVal = new ocreateListenExportDto();
            using (DdOwExtended owContext = new DdOwExtended())
            {
                EaihotDto eaiOutput = new EaihotDto();
                eaiOutput = new EaihotDto()
                {
                    CODE = input.code,
                    OLTABLE = "PEROLE",
                    SYSOLTABLE = input.sysPeRole,
                    SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 1,
                    INPUTPARAMETER1 = (input.anzahlMonate == 0 ? "" : input.anzahlMonate.ToString()),
                    INPUTPARAMETER2 = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(input.datum)).ToString(),
                    INPUTPARAMETER3 = input.mitDetails.ToString(),

                    // Input Parameter4 mit Timestamp der Abfrage im Format 'dd.mm.yyyy h24:mi'
                    INPUTPARAMETER4 = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null).ToString("dd.MM.yyyy HH:mm"),
                    INPUTPARAMETER5 = input.datum.ToString("dd.MM.yyyy"),

                    PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                    HOSTCOMPUTER = "*",
                    SYSWFUSER = input.sysWFUser
                };
                eaiOutput = eaihotDao.createEaihot(eaiOutput);
                retVal.sysEaiHot = eaiOutput.SYSEAIHOT;
            }
            return retVal;
        }

        /// <summary>
        /// Erstellt eine Liste der bereitgestellten Dokumente
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sysPerson">Die sysId vom Vertriebspartner (sysOLTable)</param>
        /// <returns></returns>
        public override List<EaihotDto> listInboxServices(string code, long sysPerson)
        {
            return eaihotDao.listEaiHotForCodeAndPerson(code, sysPerson);
        }

        /// <summary>
        /// Holt eine Fertige Liste als Excel-Datei 
        /// </summary>
        /// <param name="sysEaiHot"></param>
        /// <returns>Excel-File as Byte-Array</returns>
        public override byte[] getListeExport(long sysEaiHot, long sysPEROLE)
        {
            EaihotDto eaiOutput = eaihotDao.getEaihot(sysEaiHot);
            if (eaiOutput.SYSOLTABLE != sysPEROLE || !eaiOutput.OLTABLE.Equals("PEROLE")) return null;

            byte[] rval = null;
            EaihfileDto fileOutput = eaihotDao.getEaiHotFile(sysEaiHot);
            if (fileOutput != null && fileOutput.EAIHFILE != null )
            {
                rval = fileOutput.EAIHFILE;
            }
            return rval;
        }

        /// <summary>
        /// Löscht eine einzelne Vertragsliste 
        /// </summary>
        /// <param name="sysEaiHot"></param>
        /// <returns></returns>
        public override bool deleteListeExport(long sysEaiHot, long sysPEROLE)
        {
            EaihotDto eaiOutput = eaihotDao.getEaihot(sysEaiHot);
            if (eaiOutput.SYSOLTABLE != sysPEROLE || !eaiOutput.OLTABLE.Equals("PEROLE")) return false;
            if (eaiOutput.FILEFLAGOUT == null)
            {
                eaiOutput.FILEFLAGOUT = 0;
            }
            // FILEFLAGOUT > 0 bedeutet "Dokument wurde gelesen"
            eaiOutput.FILEFLAGOUT += 1;
            eaiOutput = eaihotDao.updateEaihot(eaiOutput);
            return true;
        }
    }
}