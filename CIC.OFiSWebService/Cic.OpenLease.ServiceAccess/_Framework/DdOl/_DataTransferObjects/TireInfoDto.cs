using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    public class TireInfoDto
    {
        /// <summary>
        /// DeliverTiresFsPrices() - Replacement
        /// Nebenkosten-Combo
        /// </summary>
        public decimal[] tiresFsPrices { get; set; }

        /// <summary>
        /// DeliverEurotaxTires() - Replacement, also replaces DeliverEurotaxRims, because the rims are already correctly filtered in the RimsFront/RimsRear-Data!
        /// Dimension information for front and rear axis Tires, including the xxx/xx Rxx dimension displayed for the car
        /// first column of all comboboxes in the gui
        /// Field TiresFront/TiresRear are used for Summer and Winter
        /// </summary>
        public TiresEurotaxDto eurotaxTires { get; set; }

      

        public RimDto[] frontRims { get; set; }
        public RimDto[] rearRims { get; set; }

        public TireDto[] frontPricesSummer { get; set; }
        public TireDto[] rearPricesSummer { get; set; }
        public TireDto[] frontPricesWinter { get; set; }
        public TireDto[] rearPricesWinter { get; set; }

        public String reifencodeVorne{ get; set; }
        public String reifencodeHinten{ get; set; }
        public String reifencodeVorneSommer{ get; set; }
        public String reifencodeHintenSommer { get; set; }
    }
}
