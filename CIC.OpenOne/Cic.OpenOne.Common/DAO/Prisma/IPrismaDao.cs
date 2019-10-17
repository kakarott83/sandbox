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
    public interface IPrismaDao
    {
        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <param name="isoCode">ISO Sprachen Code</param>
        /// <returns>Product</returns>
        PRPRODUCT getProduct(long sysprproduct, String isoCode);

        /// <summary>
        /// Get News
        /// </summary>
        /// <param name="sysprnews">ID</param>
        /// <returns>News</returns>
        PRNEWS getNews(long sysprnews);

         /// <summary>
        /// Get News Data Attributes
        /// </summary>
        /// <param name="sysnews">NewsData ID</param>
        /// <returns>NewsData</returns>
        List<NEWSATT> getNewsAttributes(long sysnews);

            /// <summary>
        /// Get News Data
        /// </summary>
        /// <param name="sysprnews">News ID</param>
        /// <param name="isoCode">ISO Sprachen Code</param>
        /// <returns>NewsData</returns>
        List<NEWS> getNewsData(long sysprnews, String isoCode);

        /// <summary>
        /// Get Products
        /// </summary>
        /// <returns>List of Products</returns>
        List<PRPRODUCT> getProducts(String isoCode);

         /// <summary>
        /// Get Vertragsart
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <returns>Vertragsart</returns>
        VART getVertragsart(long sysprproduct);

        /// <summary>
        /// Get News
        /// </summary>
        /// <returns>List of News</returns>
        List<PRNEWS> getNews();

        /// <summary>
        /// Get Producttypes
        /// </summary>
        /// <returns>List of Producttypes</returns>
        List<PRPRODTYPE> getProductTypes();

        /// <summary>
        /// get Product Conditions Link
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>List of Condition Links</returns>
        List<ProductConditionLink> getProductConditionLinks(String tableName);

        /// <summary>
        /// Get Parameter Condition Links
        /// </summary>
        /// <returns>List of Parameter Condition Links</returns>
        List<ParameterConditionLink> getParamConditionLinks();

        /// <summary>
        /// Get Params
        /// </summary>
        /// <returns>List of Parameters</returns>
        List<ParamDto> getParams();

        /// <summary>
        /// get Parametersets
        /// </summary>
        /// <returns>List of Parametersets</returns>
        List<ParameterSetConditionLink> getParamSets();

        /// <summary>
        /// returns all Product Parameter Sets (Hierarchical) Children
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        List<ParameterSetConditionLink> getParamSetChildren(long sysprparset);

        /// <summary>
        /// Get Prisma Fields
        /// </summary>
        /// <returns></returns>
        List<PRFLD> getFields();

        /// <summary>
        /// Returns true if product is difference Leasing
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        bool isDiffLeasing(long sysPrProduct);

        /// <summary>
        /// Returns the 'virtual' Prisma Parameter for Kundenzins, generated from the zinsstructure
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        ParamDto getKundenzinsParam(long sysPrProduct);

        /// <summary>
        /// Returns the 'virtual' extended Prisma Parameter for Kundenzins, generated from the zinsstructure
        /// </summary>
        /// <param name="sysPrProduct">ProduktID</param>
        /// <param name="laufzeit">laufzeit</param>
        /// <param name="saldo">Saldo</param>
        /// <returns></returns>
        KundenzinsDto getKundenzins(long sysPrProduct, long laufzeit, double saldo);

        /// <summary>
        /// returns all News Condition Links
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Product Condition List</returns>
        List<NewsConditionLink> getNewsConditionLinks(String tableName);

        /// <summary>
        /// Delivers Vertragsarten for different picworlds
        /// </summary>
        /// <returns></returns>
        List<PrBildweltVDto> getBildweltVertragsarten();

        /// <summary>
        /// Delivers vttyp by sysproduct
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        VTTYP getVttyp(long p);

        /// <summary>
        /// Delivers vttyp by sysvttyp
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        VTTYP getVttypById(long t);

        /// <summary>
        /// Delivers ID1 von Objtyp1
        /// </summary>
        /// <param name="fzart"></param>
        /// <returns></returns>
        long getObjtyp1ID(string fzart);
    }
}
