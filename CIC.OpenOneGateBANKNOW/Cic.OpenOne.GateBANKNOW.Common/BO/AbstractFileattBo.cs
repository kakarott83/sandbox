using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public abstract class AbstractFileattBo: IFileattBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IFileattDao fileattDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileattDao"></param>
        public AbstractFileattBo(IFileattDao fileattDao)
        {
            this.fileattDao = fileattDao;
        }

        /// <summary>
        /// Erstellen oder Ändern eines Fileatt Objekts
        /// </summary>
        /// <param name="Fileatt">Fileatt Datenübertragungsobjekt</param>
        /// <returns>Rückgabe des neuen oder geänderten Fileattn Objekts</returns>
        public abstract FileattDto createOrUpdateFileatt(FileattDto Fileatt);

        /// <summary>
        ///  getFileatt
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        public abstract  FileattDto getFileatt(long sysfileatt);
    }
}
