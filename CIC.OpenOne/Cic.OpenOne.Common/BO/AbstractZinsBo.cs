using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// abstract business object for getting product parameters
    /// </summary>
    public abstract class AbstractZinsBo : IZinsBo
    {
        /// <summary>
        /// the data access object to use
        /// </summary>
        protected IZinsDao zinsDao;
        /// <summary>
        /// Prisma Data Access Object
        /// </summary>
        protected IPrismaDao prismaDao;
        /// <summary>
        /// Object type Data Access Object
        /// </summary>
        protected IObTypDao obDao;
        /// <summary>
        /// VG Data Access Object
        /// </summary>
        protected IVGDao vgDao;
        /// <summary>
        /// constructs a business object
        /// </summary>
        /// <param name="zinsDao">the data access object to use</param>
        /// <param name="prismaDao">Prisma Data Access Object</param>
        /// <param name="obDao">Object Data Access Object</param>
        /// <param name="vgDao">Wertegruppen DAO</param>
        public AbstractZinsBo(IZinsDao zinsDao, IPrismaDao prismaDao,IObTypDao obDao,IVGDao vgDao)
        {
            this.zinsDao = zinsDao;
            this.prismaDao = prismaDao;
            this.obDao = obDao;
            this.vgDao = vgDao;
        }

        /// <summary>
        /// Get Interest Rate Base
        /// </summary>
        /// <param name="sysprproduct">Product ID</param>
        /// <param name="perDate">Date</param>
        /// <param name="lz">Duation</param>
        /// <param name="amount">Ammount</param>
        /// <returns>Interest Rate</returns>
        public abstract double getZinsBasis(long sysprproduct, DateTime perDate, long lz, double amount);

        /// <summary>
        /// Deliver Interest Rate Steps
        /// </summary>
        /// <param name="ctx">Context</param>
        /// <param name="lz">Duration</param>
        /// <param name="amount">Ammount</param>
        /// <param name="zinsBase">Base Interest Rate</param>
        /// <returns></returns>
        public abstract double getZinsSchritte(prKontextDto ctx, long lz, double amount, double zinsBase);

        /// <summary>
        /// Deliver Interest Rate
        /// </summary>
        /// <param name="ctx">Context</param>
        /// <param name="lz">Duration</param>
        /// <param name="amount">Ammount</param>
        /// <returns>Interest Rate</returns>
        public abstract double getZins(prKontextDto ctx, long lz, double amount);


        /// <summary>
        /// returns the Rap for the product
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        public abstract PRRAP getPrRap(long sysprproduct);

        /// <summary>
        /// returns the basel-ii (simple) score adjusted rap zins
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public abstract double getRapZinsByScore(long sysprproduct, String score);

        /// <summary>
        /// returns the basel-ii (simple) score adjusted rap zins
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public abstract PRRAPVAL getRapValByScore(long sysprproduct, String score);

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
        public abstract double getZinsRap(long sysprproduct, DateTime perDate, long lz, double amount, long sysintstrct, double rapValByScor);


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
        public abstract double[] getRAPZins(long sysprproduct, String kundenScore, long lz, double amount, prKontextDto prodCtx);


        /// <summary>
        /// getRapValByScore
        /// </summary>
        /// <param name="values"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public abstract Cic.OpenOne.Common.Model.Prisma.PRRAPVAL getRapValByScore(List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> values, String score);

        /// <summary>
        /// getRapValues
        /// </summary>
        /// <param name="sysprrap"></param>
        /// <param name="sysprkgroup"></param>
        /// <returns></returns>
        public abstract List<Cic.OpenOne.Common.Model.Prisma.PRRAPVAL> getRapValues(long sysprrap, long sysprkgroup);
    }
}