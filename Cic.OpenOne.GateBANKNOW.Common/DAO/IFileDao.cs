using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public interface IFileDao
    { 
        /// <summary>
        /// createOrUpdateFile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        FileDto createOrUpdateFile(FileDto file);

        /// <summary>
        /// getFile
        /// </summary>
        /// <param name="sysfile"></param>
        /// <returns></returns>
        FileDto getFile(long sysfile);

        /// <summary>
        /// createOrUpdateFileAngebot for B2C
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        FileDto createOrUpdateFileAngebot(FileDto file);

    }
}
