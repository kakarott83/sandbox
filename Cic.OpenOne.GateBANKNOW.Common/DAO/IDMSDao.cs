using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// DMS DB Data Access
    /// </summary>
    public interface IDMSDao
    {
        /// <summary>
        /// Returns the data for DMS for the given area/id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        DMSExportDataDto getDataForDMS(String area, long sysid);

                /// <summary>
        /// Returns the dmsakte-instance
        /// </summary>
        /// <param name="sysdmsakte"></param>
        /// <returns></returns>
        DMSAKTE getDMSAkte(long sysdmsakte);

        /// <summary>
        /// Saves the dmsakte back to database
        /// </summary>
        /// <param name="akte"></param>
        void updateDMSAkte(DMSAKTE akte);

        /// <summary>
        /// Saves the dmsakte back to database
        /// </summary>
        /// <param name="akte"></param>
        DMSUPL createOrUpdateDMSUPL(DMSUPL uploadData, long sysdmsuplinst);

        /// <summary>
        /// Determines the sysdmsupl for the uplinst and docid
        /// </summary>
        /// <param name="docid"></param>
        /// <param name="sysdmsuplinst"></param>
        /// <returns></returns>
        long getSysDmsupl(long docid, long sysdmsuplinst);

         /// <summary>
        /// Updates DMSUPL
        /// </summary>
        /// <param name="akte"></param>
        void updateDMSUPL(DMSUPL uploadData);

        /// <summary>
        /// Saves the dmsupldetails back to database
        /// </summary>
        /// <param name="akte"></param>
        void createOrUpdateDmsUpldetails(List<DMSUPLDETAIL> details, long sysdmsupl);


        /// <summary>
        /// Returns the file attribute data for DMS for the given sysdmsdoc
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        DMSExportDataDto getFileDataForDMS(long sysdmsdoc);

        /// <summary>
        /// Finds the vt, antrag, angebot for the given number or the person
        /// </summary>
        /// <param name="nummer"></param>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        GebietInfoDto getDMSTarget(String nummer, long sysperson);

        /// <summary>
        /// Creates the DMSUPLINST for the given gebiet or updates its timestamp only
        /// </summary>
        /// <param name="gebietInfo"></param>
        /// <returns></returns>
        long createOrUpdateDmsUplInst(GebietInfoDto gebietInfo);
    }
}
