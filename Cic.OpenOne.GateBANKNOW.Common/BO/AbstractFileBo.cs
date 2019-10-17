using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public abstract class AbstractFileBo : IFileBo
    {
    /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IFileDao fileDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileDao"></param>
        public AbstractFileBo(IFileDao fileDao)
        {
            this.fileDao = fileDao;
        }

        /// <summary>
        /// Erstellen oder Ändern eines Fileatt Objekts
        /// </summary>
        /// <param name="File">Fileatt Datenübertragungsobjekt</param>
        /// <returns>Rückgabe des neuen oder geänderten File Objekts</returns>
        public abstract FileDto createOrUpdateFile(FileDto File);

         /// <summary>
        /// legt eine Datei an oder aktualisiert diese für B2C
        /// </summary>
        /// <param name="fileDto">FileattDto mit einer SYSfileatt</param>
        /// <returns>FileattDto</returns>
        public abstract FileDto createOrUpdateFileAngebot(FileDto fileDto);

        /// <summary>
        ///  getFileatt
        /// </summary>
        /// <param name="sysfile"></param>
        /// <returns></returns>
        public abstract  FileDto getFile(long sysfile);
    }

}
