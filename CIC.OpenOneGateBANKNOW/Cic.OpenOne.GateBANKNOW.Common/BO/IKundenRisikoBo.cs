using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public interface IKundenRisikoBo 
    {
        /// <summary>
        /// Calculates the credit limits for the context
        /// </summary>
        /// <param name="kontext"></param>
        /// <param name="sysantrag"></param>
        /// <param name="sysvart"></param>
        /// <returns></returns>
        List<ProductCreditInfoDto> getCreditLimits(prKontextDto kontext, long sysantrag, long sysvart, String isoCode, long syswfuser);
       
         /// <summary>
        /// Calculates Credit Expected Loss for the offer and product context
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="kontext"></param>
        /// <returns></returns>
        VClusterDto getELValues(long sysid, prKontextDto kontext);

        /// <summary>
        /// Calculates Credit Expected Loss for the offer and product context providing gui debug data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ocheckKrRiskByIdDto checkKrRiskById(icheckKrRiskByIdDto input);


        /// <summary>
        /// Performs the EL Product Validation
        /// </summary>
        /// <param name="rval"></param>
        /// <param name="sysid"></param>
        /// <param name="kontext"></param>
        void performProductValidation(ocheckAntAngDto rval, long sysid, prKontextDto kontext, String isoCode, bool hasMA);


        /// <summary>
        /// get CreditLimits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ogetCreditLimitsGUIDto getCreditLimits(igetCreditLimitsGUIDto input );


        /// <summary>
        /// EL_DEFlag  Es sollte kein TR berechnet werden, wenn das CLUSTERVALUE nicht von der DE übergeben wird
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        bool KRBerechnen(long sysid);
        /// <summary>
        /// Returns true if Antrag has a MA
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        bool hasMA(long sysid);
    }
}
