using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public interface IFileattDao
    {
        /// <summary>
        /// createOrUpdateFileatt
        /// </summary>
        /// <param name="fileatt"></param>
        /// <returns></returns>
        FileattDto createOrUpdateFileatt(FileattDto fileatt);

        /// <summary>
        /// getFileatt
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        FileattDto getFileatt(long sysfileatt);

        
    }
}
