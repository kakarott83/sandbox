using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenLease.Service.Properties;


namespace Cic.OpenLease.Service.Versicherung
{
    /// <summary>
    /// Factory to create different Calculators dependend on the codeMethod of the VSTYP
    /// </summary>
    [System.CLSCompliant(true)]
    public class VSCalcFactory
    {
        #region Constants
        /// <summary>
        /// Types of different calculation methods used for creating the implementations
        /// </summary>
        public const string Cnst_CALC_DEFAULT = "DEFAULT";
        public const string Cnst_CALC_KASKO = "KASKO";
        public const string Cnst_CALC_HAFTPFLICHT = "HP";
        public const string Cnst_CALC_INSASSEN = "IUV";
        public const string Cnst_CALC_GAP = "GAP";
        public const string Cnst_CALC_RECHTSSCHUTZ = "RSV";
        public const string Cnst_CALC_RESTSCHULD = "RSDV";
        #endregion

        public IVSCalculator createKaskoCalculator()
        {
            return new KaskoCalculator();
        }

        public IVSCalculator createHaftpflichtCalculator()
        {
            return new HaftpflichtCalculator();
        }

        public IVSCalculator createInsassenUVCalculator()
        {
            return new InsassenUVCalculator();
        }

        public IVSCalculator createRechtsschutzCalculator()
        {
            return new RechtsschutzCalculator();
        }

        public IVSCalculator createRestschuldCalculator()
        {
            return new RestschuldCalculator();
        }

        public IVSCalculator createDefaultCalculator()
        {
            return new DefaultCalculator();
        }

        public IVSCalculator createGAPCalculator()
        {
            return new GAPCalculator();
        }

        public static IVSCalculator createCalculator(string codeMethod)
        {
            if(codeMethod==null || "".Equals(codeMethod))
                throw new NullReferenceException( String.Format(ExceptionMessages.GeneralError,404));//"CODEMETHOD for VSTYP not defined");

            switch(codeMethod)
            {
                case Cnst_CALC_KASKO:
                    return new KaskoCalculator();
                case Cnst_CALC_HAFTPFLICHT:
                    return new HaftpflichtCalculator();
                case Cnst_CALC_INSASSEN:
                    return new InsassenUVCalculator();
                case Cnst_CALC_RECHTSSCHUTZ:
                    return new RechtsschutzCalculator();
                case Cnst_CALC_RESTSCHULD:
                    return new RestschuldCalculator();
                case Cnst_CALC_GAP:
                    return new GAPCalculator();
                case Cnst_CALC_DEFAULT:
                    return new DefaultCalculator();
            }
            throw new NotSupportedException(codeMethod + " not yet supported");
        }
    }
}