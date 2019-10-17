using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenLease.Model.DdOl;


namespace Cic.OpenLease.Service
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
        /// <returns>Product</returns>
        PRPRODUCTDto getProduct(long sysprproduct);

    
        /// <summary>
        /// Get Products
        /// </summary>
        /// <returns>List of Products</returns>
        List<PRPRODUCTDto> getProducts();

       

        /// <summary>
        /// Get Producttypes
        /// </summary>
        /// <returns>List of Producttypes</returns>
        List<Cic.OpenLease.Model.DdOl.PRPRODTYPE> getProductTypes();

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
        List<PRPARAMDto> getParams();

        /// <summary>
        /// get Parametersets
        /// </summary>
        /// <returns>List of Parametersets</returns>
        List<ParameterSetConditionLink> getParamSets();

    /*    /// <summary>
        /// returns all Product Parameter Sets (Hierarchical) Children
        /// </summary>
        /// <returns>Parameter Condition List</returns>*/
       // List<ParameterSetConditionLink> getParamSetChildren(long sysprparset);

        /// <summary>
        /// Get Prisma Fields
        /// </summary>
        /// <returns></returns>
        List<PRFLD> getFields();

      

    }
}
