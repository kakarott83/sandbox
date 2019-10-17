
namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class MyPocketDto
    {
        /// <summary>
        /// Restlaufzeit (Monate): number of month the bonus program is still running
        /// </summary>
        public int rlz_monate { get; set; }
        /// <summary>
        /// Restlaufzeit (Tage): number of days the bonus program is still running
        /// </summary>
        public int rlz_tage { get; set; }
        /// <summary>
        /// current bonus level
        /// </summary>
        public string hd_segment_aktuell { get; set; }
        /// <summary>
        /// next bonus level
        /// </summary>
        public string hd_segment_next { get; set; }
        /// <summary>
        /// amount of budget money missing for next bonus level
        /// </summary>
        public double gap_value_kickback { get; set; }
        /// <summary>
        /// bonus for reaching next bonus level
        /// </summary>
        public double next_payout_kickback { get; set; }
        /// <summary>
        /// bonus earned up to now (during the current bonus program)
        /// </summary>
        public double akt_payout_total { get; set; }
        /// <summary>
        /// total budget needed for next bonus level
        /// </summary>
        public double next_target_kickback { get; set; }
        /// <summary>
        /// total budget from concluded contracts up to now (during the current bonus program)
        /// </summary>
        public double akt_volume_kickback { get; set; }
        /// <summary>
        /// proportion of total budget from concluded contracts to total budget needed for next bonus level (in percent)
        /// </summary>
        public double gap_percent_kickback { get { if (next_target_kickback == 0) return 0; return akt_volume_kickback / next_target_kickback; } set { } }
        /// <summary>
        /// syscode of seller whose "pocket" this ist
        /// </summary>
        public long sys_seller { get; set; }

        /// <summary>
        /// Dokumenten-Id für REPORT_VK_INCENTIVES
        /// </summary>
        public long report_vk_incentives { get; set; }
    }
}