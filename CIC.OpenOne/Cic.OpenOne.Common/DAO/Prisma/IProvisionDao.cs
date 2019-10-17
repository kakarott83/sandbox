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
    /// Provision Data Access Object Interface
    /// </summary>
    public interface IProvisionDao
    {
        /// <summary>
        /// returns all Product Parameter Sets linked to Products
        /// </summary>
        /// <returns>Parameter Condition Link List</returns>
        List<ProvisionConditionLink> getProvisionConditionLinks();

        /// <summary>
        /// determines if the field and type are a valid pair
        /// </summary>
        /// <param name="sysprfld"></param>
        /// <param name="sysprprovtype"></param>
        /// <returns></returns>
        bool validProvision(long sysprfld, long sysprprovtype);

        /// <summary>
        /// Returns all configured Provsteps for Incentives (flagnokalk=1)
        /// </summary>
        /// <param name="sysPrHgroup"></param>
        /// <param name="sysPerole"></param>
        /// <param name="perDate"></param>
        /// <param name="sysProvType"></param>
        /// <returns></returns>
        List<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP> getProvstepsInc(long sysPrHgroup, long sysPerole, DateTime perDate, long sysProvType);

        /// <summary>
        /// Returns all configured Provsteps as of Prisma concept 5.2.2.2.1
        /// </summary>
        /// <param name="sysPrHgroup"></param>
        /// <param name="sysPerole"></param>
        /// <param name="perDate"></param>
        /// <param name="sysProvType"></param>
        /// <param name="assignType"></param>
        /// <returns></returns>
        List<PRPROVSTEP> getProvsteps(long sysPrHgroup, long sysPerole, DateTime perDate, long sysProvType, ProvGroupAssignType assignType);

        /// <summary>
        /// get all provision structure data entries (either rate, tarif, plan)
        /// as of Prisma concept 5.2.2.2.2
        /// </summary>
        /// <param name="sysprovstrct"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        List<PROVSTRCTDATA> getStrctData(long sysprovstrct, DateTime perDate);

        /// <summary>
        /// returns all Provision Adjustment Links as of Prisma concept 5.2.2.2.3
        /// </summary>
        /// <returns>Parameter Condition Link List</returns>
        List<ProvisionAdjustConditionLink> getProvisionAdjustLinks();

        /// <summary>
        /// returns all Provision Adjustment Steps
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        List<PRPROVADJSTEP> getProvisionAdjustStep();

        /// <summary>
        /// returns all Provision Shares as of Prisma concept 5.2.2.2.4
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        List<PROVSHAREDATA> getProvisionShares();

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        List<PRPROVTYPE> getProvisionTypes();

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        List<PRPROVTYPE> getProvisionTypes(long sysprfld);

        /// <summary>
        /// Returns a list of all prisma fields that have a provision configured
        /// </summary>
        /// <returns></returns>
        List<long> getProvisionedPrFlds();

        /// <summary>
        /// Returns the ABLTYP
        /// </summary>
        /// <param name="sysabltyp"></param>
        /// <returns></returns>
        CIC.Database.OL.EF4.Model.ABLTYP getAblTyp(long sysabltyp);

        /// <summary>
        /// ID der externen Abloese ermitteln
        /// </summary>
        /// <returns>ABLTYPID</returns>
        long getExternalABlID();

        /// <summary>
        /// if this returns false prhgroup must be 0
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        bool checkPrhGroup(long sysperole, long sysprhgroup);


        /// <summary>
        /// get the Eigenabloeseinformation
        /// </summary>
        /// <param name="sysvorvt"></param>
        /// <returns></returns>
        EigenAblInfo getEigenabloeseInfo(long sysvorvt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        long getVkparent(long sysperole);

         /// <summary>
        /// gets the sysperole of given type starting from the leaf sysperole
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysroletype"></param>
        /// <returns></returns>
        long getRoleByType(long sysperole, long sysroletype);

         /// <summary>
        /// returns the provisionplan (PRPROVSET)
        /// </summary>
        /// <param name="sysprprovset"></param>
        /// <returns></returns>
        PRPROVSET getPrprovset(long sysprprovset);
    }
}
