using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public interface IIncentivierungDao
    {
        /// <summary>
        /// saves the incentive provisions and the traces for them
        /// </summary>
        /// <param name="provs"></param>
        /// <param name="tracing"></param>
        void createIncentiveProvisions(List<AngAntProvDto> provs, List<ProvKalkDto> tracing);

        /// <summary>
        /// gets all ident rules for the given date
        /// </summary>
        /// <param name="perDate"></param>
        /// <returns></returns>
        List<PrkGroupIdentDto> getIdentRules(DateTime perDate);

        /// <summary>
        /// returns the segmentation prkgroup code currently assigned to the sales person
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        String getSegmentation(long sysperole);
        
        /// <summary>
        /// returns the segmentation prkgroup name currently assigned to the segmentation code
        /// </summary>
        /// <param name="segmentCode">code of the segment</param>
        /// <param name="perDate">date when the segment is valid (e.g. DateTime.Today)</param>
        /// <returns>nice name of the segment</returns>
        String getSegmentationName(string segmentCode, DateTime perDate);
        
        /// <summary>
        /// get provision set sysvg by provision set id and field
        /// </summary>
        /// <param name="sysprprovset"></param>
        /// <param name="sysprfld"></param>
        /// <returns>sysvg</returns>
        long getSysvg(long sysprprovset, long sysprfld);
        
        /// <summary>
        /// get value groups by sysvg
        /// </summary>
        /// <param name="sysvg"></param>
        /// <returns>value groups</returns>
        List<VGValuesDto> getSegmentations(long sysvg);

        /// <summary>
        /// returns the segmentation code, identified by the ident rules for the given prkgroup, date by the given gesamtumsatz
        /// </summary>
        /// <param name="gesamtumsatz"></param>
        /// <param name="sysprkgroup"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        String getSegmentation(double gesamtumsatz, long sysprkgroup, DateTime perDate);

        /// <summary>
        /// Fetches needed data of contract for incentivation calculation
        /// </summary>
        /// <param name="sysvt"></param>
        /// <returns></returns>
        VTIncentivierungDataDto getContractData(long sysvt);

        /// <summary>
        /// returns the actual payed money for the perole and provplan
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprprovset"></param>
        /// <returns></returns>
        double getAktpayoutTotal(long sysperole, long sysprprovset);

        /// <summary>
        /// gets the current umsatz for the given provplan for the given role
        /// sysvt allows to include the not yet provisioned contract
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprprovset"></param>
        /// <param name="sysvt"></param>
        /// <returns></returns>
        double getProvPlanUmsatz(long sysperole, long sysprprovset, long sysvt);

        /// <summary>
        /// get the document id
        /// </summary>
        /// <returns>syswftx</returns>
        long getDocumentId();
    }
}
