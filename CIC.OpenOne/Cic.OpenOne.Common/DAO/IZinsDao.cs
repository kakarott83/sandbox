using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Interest Rate Data Access Object Interface
    /// </summary>
    public interface IZinsDao
    {
        /// <summary>
        /// Get Ibor Data
        /// </summary>
        /// <returns></returns>
        List<IborDto> getIbor();

        /// <summary>
        /// Get Interest Rates
        /// </summary>
        /// <returns>List of Interest Rates</returns>
        List<IntsDto> getIntsrate();

        /// <summary>
        /// Get Interest Rates Mature
        /// </summary>
        /// <returns>List of Interest Rates</returns>
        List<IntsDto> getIntsmatu();

        /// <summary>
        /// Get Interest Rates Band
        /// </summary>
        /// <returns>List of Interest Rates</returns>
        List<IntsDto> getIntsband();

        /// <summary>
        /// Get Interest Rate Structure
        /// </summary>
        /// <returns>Structure List of Interest Rates</returns>
        List<IntstrctDto> getIntstrct();

        /// <summary>
        /// get Product Links
        /// </summary>
        /// <returns>Product Links for Interest rates</returns>
        List<PRCLPRINTSETDto> getProductLinks();

        /// <summary>
        /// Get Interest Rate Groups
        /// </summary>
        /// <returns>List of Interest Rate Groups</returns>
        List<PRINTSETDto> getIntGroups();

        /// <summary>
        /// Get Interest Rate Steps
        /// </summary>
        /// <returns>List of Interest Rate steps</returns>
        List<InterestConditionLink> getIntSteps();

        /// <summary>
        /// returns the prrap of the product
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        PRRAP getPrRap(long sysprproduct);
        
        /// <summary>
        /// returns the rapvalues for the rap id
        /// </summary>
        /// <param name="sysprrap"></param>
        /// <returns></returns>
        List<PRRAPVAL> getRapValues(long sysprrap);

        /// <summary>
        /// getIntstrctById
        /// </summary>
        /// <returns></returns>
        List<IntstrctDto> getIntstrctById(long sysintstrct);


        /// <summary>
        /// getRapValByScore
        /// </summary>
        /// <param name="values"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        Cic.OpenOne.Common.Model.Prisma.PRRAPVAL getRapValByScore(List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> values, String score);

        /// <summary>
        /// getRapValues
        /// </summary>
        /// <param name="sysprrap"></param>
        /// <param name="sysprkgroup"></param>
        /// <returns></returns>
        List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> getRapValues(long sysprrap, long sysprkgroup);
    }
}
