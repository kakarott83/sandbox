using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Interest Rate Business Object Interface
    /// </summary>
    public interface IZinsBo
    {
        /// <summary>
        /// Get Interest Rate Base
        /// </summary>
        /// <param name="sysprproduct">Product ID</param>
        /// <param name="perDate">Date</param>
        /// <param name="lz">duration</param>
        /// <param name="amount">amount</param>
        /// <returns>Interest Rate</returns>
        double getZinsBasis(long sysprproduct, DateTime perDate, long lz, double amount);

        /// <summary>
        /// Deliver Interest rate steps
        /// </summary>
        /// <param name="ctx">context</param>
        /// <param name="lz">duration</param>
        /// <param name="amount">amount</param>
        /// <param name="zinsBase">Interest rate base</param>
        /// <returns>Interest Rate</returns>
        double getZinsSchritte(prKontextDto ctx, long lz, double amount, double zinsBase);

        /// <summary>
        /// Deliver Interest Rate
        /// </summary>
        /// <param name="ctx">Context</param>
        /// <param name="lz">Duration</param>
        /// <param name="amount">Amount</param>
        /// <returns>Interest Rate</returns>
        double getZins(prKontextDto ctx, long lz, double amount);

        /// <summary>
        /// returns the Rap for the product
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        PRRAP getPrRap(long sysprproduct);

        /// <summary>
        /// returns the basel-ii (simple) score adjusted rap zins
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        double getRapZinsByScore(long sysprproduct, String score);

        /// <summary>
        /// returns the basel-ii (simple) score adjusted rap zins
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        PRRAPVAL getRapValByScore(long sysprproduct, String score);

        /// <summary>
        /// returns ZinsRap mit Zinsstruktur 
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="perDate"></param>
        /// <param name="lz"></param>
        /// <param name="amount"></param>
        /// <param name="sysintstrct"></param>
        /// <param name="rapValByScor"></param>
        /// <returns></returns>
        double getZinsRap(long sysprproduct, DateTime perDate, long lz, double amount, long sysintstrct, double rapValByScor);
        
                /// <summary>
        /// Returns the RAP Zins
        /// index zero=RAP
        /// index 1 = MIN
        /// index 2 = MAX
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="kundenScore"></param>
        /// <param name="lz"></param>
        /// <param name="amount"></param>
        /// <param name="prodCtx"></param>
        /// <returns></returns>
        double[] getRAPZins(long sysprproduct, String kundenScore, long lz, double amount, prKontextDto prodCtx);

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
