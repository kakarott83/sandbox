using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public class FileattBo :  AbstractFileattBo
    {
        /// <summary>
        /// Construktor
        /// </summary>
        /// <param name="fileattDao"></param>
        public FileattBo(IFileattDao fileattDao)
            : base(fileattDao)
        {
        }

        /// <summary>
        /// createOrUpdatefileatt entscheidet zwischen einem Create oder einem Update der Anfrage
        /// </summary>
        /// <param name="fileattDto">FileattDto mit einer SYSfileatt</param>
        /// <returns>FileattDto</returns>
        public override FileattDto createOrUpdateFileatt(FileattDto fileattDto)
        {

            return fileattDao.createOrUpdateFileatt(fileattDto);
        }


        public override FileattDto getFileatt(long sysfileatt)
        {

            return fileattDao.getFileatt(sysfileatt);
        }

    }
}
