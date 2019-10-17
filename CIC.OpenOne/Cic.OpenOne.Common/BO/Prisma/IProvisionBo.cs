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
    /// Provision Business Object Interface
    /// </summary>
    public interface IProvisionBo
    {
        /// <summary>
        /// calculate Provision
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        List<AngAntProvDto> calculateProvision(provKontextDto ctx, iProvisionDto param);

        /// <summary>
        /// Calculates the Provisions for incentives
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="param"></param>
        /// <param name="tracing">traces all steps</param>
        /// <returns></returns>
        List<AngAntProvDto> calculateIncentiveProvision(provKontextDto ctx, iProvisionDto param, List<ProvKalkDto> tracing);

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
        List<long> getPrFlds();

        /// <summary>
        /// Finds the corresponding Prisma Field /Area Id for the given Provision and Context
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        ParamDto getPrfld(ProvisionSourceField sourceField, prKontextDto context);

        /// <summary>
        /// Finds the corresponding Prisma Field Id for the given Provision and Context
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        long getPrfldId(ProvisionSourceField sourceField, prKontextDto context);

        /// <summary>
        /// checks if the abloese is of the given type
        /// </summary>
        /// <param name="sysabltyp"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        bool isAbloesetyp(long sysabltyp, Abloesetyp typ);

        /// <summary>
        /// checks if prhgroup must be 0;
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        bool isPrhGroup(long sysperole, long sysprhgroup);

        /// <summary>
        /// get the Eigenabloeseinformation
        /// </summary>
        /// <param name="sysvorvt"></param>
        /// <returns></returns>
        EigenAblInfo getEigenabloeseInfo(long sysvorvt);

        /// <summary>
        /// Returns the current provision plan for Kickback for the given role and prhgroup
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        PRPROVSET getProvisionsPlan(long sysperole, DateTime perDate);

        /// <summary>
        /// Returns the current provision plan (prprovset) id for the given provision context
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="sysprprovset"></param>
        /// <returns></returns>
        long getProvisionPlan(provKontextDto ctx, long sysprprovset);
    }
}
