using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Abstract Class: Prisma Product Business Object
    /// </summary>
    public abstract class AbstractPrismaProductBO : IPrismaProductBo
    {
        /// <summary>
        /// Prisma Data Access Object
        /// </summary>
        protected IPrismaDao pDao;

        /// <summary>
        /// Object Type Data Access Object
        /// </summary>
        protected IObTypDao obDao;

        /// <summary>
        /// Translation Dao
        /// </summary>
        protected ITranslateDao transDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao">Prisma Data Access Object</param>
        /// <param name="obDao">Object Type Data Access Object</param>
        /// <param name="transDao">Translate Dao</param>
        public AbstractPrismaProductBO(IPrismaDao pDao, IObTypDao obDao, ITranslateDao transDao)
        {
            this.pDao = pDao;
            this.obDao = obDao;
            this.transDao = transDao;
        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <returns>Product</returns>
        public abstract PRPRODUCT getProduct(long sysprproduct);

        /// <summary>
        /// Get Vertragsart
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <returns>Vertragsart</returns>
        public abstract VART getVertragsart(long sysprproduct);

        /// <summary>
        /// returns a sorted flat List for the given Products
        /// </summary>
        /// <param name="products"></param>
        /// <param name="sysprbildwelt"></param>
        /// <returns></returns>
        public abstract List<AvailableProduktDto> listSortedAvailableProducts(List<PRPRODUCT> products, long sysprbildwelt);

        /// <summary>
        /// List available Products
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>Product List</returns>
        public abstract List<PRPRODUCT> listAvailableProducts(prKontextDto context);

       
        /// <summary>
        /// Returns Bildweltvertragsarten
        /// </summary>
        /// <returns></returns>
        public abstract List<PrBildweltVDto> getBildweltVertragsarten();
    }
}
