using Cic.OpenOne.Common.DTO;
using System;

namespace Cic.One.DTO
{
    public class ZekPleaDto : EntityDto
    {

        /// <summary>
        /// Reason of Rejection
        /// </summary>
        public int Ablehnungsgrund { get; set; }
        /// <summary>
        /// Date of rejection
        /// </summary>
        public DateTime? DatumAblehnung { get; set; }
        /// <summary>
        /// Date Valid till
        /// </summary>
        public DateTime? DatumGueltigBis { get; set; }
        /// <summary>
        /// Date of Credit Application
        /// </summary>
        public DateTime? DatumKreditgesuch { get; set; }
        /// <summary>
        /// subsidiary
        /// </summary>
        public int Filiale { get; set; }
        /// <summary>
        /// Origin
        /// </summary>
        public int Herkunft { get; set; }
        /// <summary>
        /// Credit contract ID
        /// </summary>
        public string KreditVertragID { get; set; }
        /// <summary>
        /// Contract State
        /// </summary>
        public int VertragsStatus { get; set; }

        public override long getEntityId()
        {
            try
            {
                return Convert.ToInt64(KreditVertragID);
            }
            catch
            {
                return 0;
            }
        }
    }
}
