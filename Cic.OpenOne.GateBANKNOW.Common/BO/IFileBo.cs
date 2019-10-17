using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public interface IFileBo
    { /// <summary>
        /// Neue Fileatt erstellen oder vorhandene Ändern
        /// </summary>
        /// <param name="File">Adressdaten</param>
        /// <returns>Neue oder geänderte Adressdaten</returns>
        FileDto createOrUpdateFile(FileDto File);

         /// <summary>
        /// legt eine Datei an oder aktualisiert diese für B2C
        /// </summary>
        /// <param name="fileDto">FileattDto mit einer SYSfileatt</param>
        /// <returns>FileattDto</returns>
        FileDto createOrUpdateFileAngebot(FileDto fileDto);

        /// <summary>
        ///  getFileatt
        /// </summary>
        /// <param name="sysfile"></param>
        /// <returns></returns>
        FileDto getFile(long sysfile);

    }
}