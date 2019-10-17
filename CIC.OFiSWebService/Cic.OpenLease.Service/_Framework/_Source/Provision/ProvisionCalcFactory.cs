using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenLease.ServiceAccess.DdOl;


namespace Cic.OpenLease.Service.Provision
{
    /// <summary>
    /// Factory to create different Calculators dependend on the codeMethod of the VSTYP
    /// </summary>
    [System.CLSCompliant(true)]
    public class ProvisionCalcFactory
    {
        #region Constants
        /// <summary>
        /// Types of different calculation methods used for creating the implementations
        /// </summary>
        /*public const int Cnst_CALC_ABSCHLUSSPROVISION = 10;
        public const int Cnst_CALC_ZUGANGSPROVISION = 20;
        public const int Cnst_CALC_WARTUNGREPARATURPROVISION = 30;
        public const int Cnst_CALC_KASKOPROVISION = 40;
        public const int Cnst_CALC_RESTSCHULDPROVISION = 50;
        public const int Cnst_CALC_BEARBEITUNGSGEBUEHRPROVISION = 60;
        public const int Cnst_CALC_HAFTPFLICHTPROVISION = 70;*/
        

        #endregion


        public static IProvisionCalculator createCalculator(ProvisionTypeConstants provisionType)
        {

            switch (provisionType)
            {
                case ProvisionTypeConstants.Abschluss:
                    return new AbschlussProvisionCalculator();
                case ProvisionTypeConstants.WartungReparatur:
                    return new WartungProvisionCalculator();
                case ProvisionTypeConstants.Kasko:
                    return new KaskoProvisionCalculator();
                case ProvisionTypeConstants.Haftpflicht:
                    return new HaftpflichtProvisionCalculator();
                case ProvisionTypeConstants.Restschuld:
                    return new RestschuldProvisionCalculator();
                case ProvisionTypeConstants.Bearbeitungsgebuehr:
                    return new GebuehrProvisionCalculator();
                case ProvisionTypeConstants.GAP:
                    return new GAPProvisionCalculator();
            }
            throw new NotSupportedException(provisionType + " not yet supported");
        }
    }
}