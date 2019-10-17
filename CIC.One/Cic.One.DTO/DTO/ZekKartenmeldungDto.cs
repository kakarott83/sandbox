using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO {
    public class ZekKartenmeldungDto : EntityDto {

        /// <summary>
        /// Getter/Setter signature
        /// </summary>
        public int Kennzeichen { get; set; }

        /// <summary>
        /// Getter/Setter Subsidiary
        /// </summary>
        public int Filiale { get; set; }

        /// <summary>
        /// Getter/Setter Event code
        /// </summary>
        public int EreignisCode { get; set; }

        /// <summary>
        /// Getter/Setter Card code
        /// </summary>
        public int KartenTypCode { get; set; }

        /// <summary>
        /// Getter/Setter Date of negative event
        /// </summary>
        public string DatumNegativereignis { get; set; }

        /// <summary>
        /// Getter/Setter Date of positive event
        /// </summary>
        public string DatumPositivmeldung { get; set; }
        /// <summary>
        /// Gets identification
        /// </summary>
        /// <returns>primary key</returns>
        public override long getEntityId() {
            try
            {
                return Convert.ToInt64(Kennzeichen);
            }
            catch
            {
                return 0;
            }
        }
    }
}