using System;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// KREMO Bo Interface
    /// </summary>
    public interface IKREMOBo
    {
        /// <summary>
        /// interface method to fill input values for KREMO Webservice by InDto and call KREMOWSDao method CallKremoByValues() 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>KREMODto filled with KREMO return values</returns>
        AuskunftDto callByValues(KREMOInDto inDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto callByValues(long sysAuskunft);

        /// <summary>
        /// interface method to call KREMOWSDao method getVersion() 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>KREMODto filled with version nr</returns>
        KREMOOutDto getVersion(KREMOInDto inDto);

        /// <summary>
        /// interface method to call KREMOWSDao method getVersion()
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto getVersion(long sysAuskunft);

        /// <summary>
        /// gets the available budget for the given data
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        double getBudget(KREMOInDto inDto);


        /// <summary>
        /// gets the Kremo values for the input 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        KREMOOutDto getKremoValues(KREMOInDto inDto);

        /// <summary>
        /// Gets the budget-Value from the KREMO result
        /// </summary>
        /// <param name="outDto"></param>
        /// <returns></returns>
        double getBudgetValue(KREMOOutDto outDto);

        /// <summary>
        /// Calculates the Budgetüberschuss for Budgetcalculator 
        /// considers AS1 and AS2 
        /// uses Ruleenine "ANTRAGSASSISTENT", "RULEENGINE_BUDGET", "USE_RULESET_B2B"
        /// calls Kremo-Interface before calling ruleengine
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="budget1"></param>
        /// <param name="budget2"></param>
        /// <returns></returns>
        DTO.ogetKremoBudget getKremoBudget(long syswfuser, Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto budget1, Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto budget2, long sysprproduct);
    }
}
