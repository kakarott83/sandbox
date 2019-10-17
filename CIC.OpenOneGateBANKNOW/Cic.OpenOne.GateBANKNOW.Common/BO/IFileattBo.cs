using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public interface IFileattBo 
    {
         /// <summary>
        /// Neue Fileatt erstellen oder vorhandene Ändern
        /// </summary>
        /// <param name="Fileatt">Adressdaten</param>
        /// <returns>Neue oder geänderte Adressdaten</returns>
        FileattDto createOrUpdateFileatt(FileattDto Fileatt);

        /// <summary>
        ///  getFileatt
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        FileattDto getFileatt(long sysfileatt);
        
    }
}

    
