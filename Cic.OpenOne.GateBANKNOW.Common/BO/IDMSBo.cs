using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public interface IDMSBo
    {
        /// <summary>
        /// Creates or Updates a dossier (the attributes) in the DMS via the DMS-HTTP-Interface
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DMSAKTE createOrUpdateDMSAkte(icreateOrUpdateDMSAkteDto input);
        /// <summary>
        /// creates or updates a document (the file) in the DMS via the DMS Documentimport Interface
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        DMSAKTE createOrUpdateDMSDokument(icreateOrUpdateDMSDokmentDto input);    

        /// <summary>
        /// interface from DMS to OL for new incoming Documents
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void execDMSUploadTrigger(long sysperole, long syswfuser, iDMSUploadDto input);


        /// <summary>
        /// Creates or Updates (the attributes) in the DMS via the DMS-HTTP-Interface for all dmsakte-Entries of the dmsbatch
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ocreateOrUpdateDMSAkteBatchDto createOrUpdateDMSAkteBatch(icreateOrUpdateDMSAkteBatchDto input);
    }
}
