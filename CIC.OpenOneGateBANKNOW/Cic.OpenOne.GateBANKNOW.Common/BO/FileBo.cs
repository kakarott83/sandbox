using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public class FileBo : AbstractFileBo
    {
     /// <summary>
        /// Construktor
        /// </summary>
        /// <param name="fileDao"></param>
        public FileBo(IFileDao fileDao)
            : base(fileDao)
        {
        }

        /// <summary>
        /// createOrUpdatefile entscheidet zwischen einem Create oder einem Update der Anfrage
        /// started einen bpe prozess, wenn dieser im dao konfiguriert ist
        /// </summary>
        /// <param name="fileDto">FileDto mit einer sysdmsdoc</param>
        /// <returns>FileattDto</returns>
        public override FileDto createOrUpdateFile(FileDto fileDto)
        {

            return fileDao.createOrUpdateFile(fileDto);
        }

        /// <summary>
        /// legt eine Datei an oder aktualisiert diese für B2C
        /// </summary>
        /// <param name="fileDto">FileattDto mit einer SYSfileatt</param>
        /// <returns>FileattDto</returns>
        public override FileDto createOrUpdateFileAngebot(FileDto fileDto)
        {
            
            return fileDao.createOrUpdateFileAngebot(fileDto);
        }


        public override FileDto getFile(long sysfile)
        {

            return fileDao.getFile(sysfile);
        }

    }
}