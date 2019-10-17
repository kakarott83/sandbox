using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Cic.One.DTO
{
    public enum CalculationCommand
    {
        [Description("CALC_RATE")]
        CALC_RATE,
        [Description("CALC_BARWERT")]
        CALC_BARWERT,
        [Description("CALC_RESTWERT")]
        CALC_RESTWERT,
        [Description("CALC_ZINS")]
        CALC_ZINS,
        [Description("CALC_SZ")]
        CALC_SZ,
        [Description("CALC_SZMK")]//Sonderzahlung Mietkauf
        CALC_SZMK,
        [Description("CALC_RABATTOFFEN")]
        CALC_RABATTOFFEN,
        [Description("CALC_HAENDLERBET")]
        CALC_HAENDLERBET,
        [Description("CALC_PROVISION")]
        CALC_PROVISION,
        [Description("CALC_BWMARGE")]
        CALC_BWMARGE
    }
}
