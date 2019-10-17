using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    /// <summary>
    /// Prisma Data Access Object Interface
    /// </summary>
    public interface IPrismaServiceDao
    {
        /// <summary>
        ///  returns all Link info between VSTYP and PRPRODUCT
        /// </summary>
        /// <returns>Parameter list</returns>
        List<PRVSDto> getVSTYPForProduct(DateTime perDate, long productID);

        /// <summary>
        /// Objekt Versicherungstypen holen
        /// </summary>       
        /// <returns>Liste mit versicherungstypen</returns>
        List<VSTYP> getVSTYP();

        /// <summary>
        /// returns all condition links for a service
        /// </summary>
        /// <param name="tableName">link typ table name</param>
        /// <returns></returns>
        List<ServiceConditionLink> getServiceConditionLinks(String tableName);

        /// <summary>
        /// Returns PRRSVCODE for an PRPRODUKT
        /// </summary>
        /// <param name="perDate">perDate</param>
        /// <param name="sysprprodukt">sysprprodukt</param>
        /// <returns>prrsvcode</returns>
        PRRSVCODE getPrrsvCodeByPrProdukt(DateTime perDate, long sysprprodukt);

        /// <summary>
        /// returns the VSART
        /// </summary>
        /// <param name="sysvsart"></param>
        /// <returns></returns>
        VSART getVSART(long sysvsart);
    }
}
