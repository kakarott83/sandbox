using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Prisma Product Business Object Interface
    /// </summary>
    public interface IPrismaProductBo
    {
        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <returns>Product</returns>
        PRPRODUCT getProduct(long sysprproduct);

        /// <summary>
        /// Get Vertragsart
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <returns>Vertragsart</returns>
        VART getVertragsart(long sysprproduct);

        /// <summary>
        /// List Available Products
        /// </summary>
        /// <param name="context">Context: pSysBRAND IN NUMBER, pSysHGroup IN NUMBER, pSysPeRole IN NUMBER, pSysVPPeRole IN NUMBER, pSysOBType IN NUMBER, pSysOBArt IN NUMBER</param>
        /// <returns>Product List</returns>
        List<OpenOne.Common.Model.Prisma.PRPRODUCT> listAvailableProducts(prKontextDto context);

       

        /// <summary>
        /// returns a sorted flat List for the given Products
        /// </summary>
        /// <param name="products"></param>
        /// <param name="sysprbildwelt"></param>
        /// <returns></returns>
        List<AvailableProduktDto> listSortedAvailableProducts(List<PRPRODUCT> products, long sysprbildwelt);

        /// <summary>
        /// Returns Bildweltvertragsarten
        /// </summary>
        /// <returns></returns>
        List<PrBildweltVDto> getBildweltVertragsarten();
    }
}
