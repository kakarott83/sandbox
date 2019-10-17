using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter 
    /// </summary>
    public class ogetListeExportDto : oBaseDto
    {
        /// <summary>
        /// Eine Liste der Verträge als eine Excel-Datei aus dem Feld EAIHFILE.EAIHFILE1
        /// </summary>
        public byte[] eaiHFile { get; set; }
    }
}