using Cic.OpenOne.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.BN
{
    /// <summary>
    /// Class for holding info of EWK/BA/GA Auskunft
    /// </summary>
    public class AuskunftDetailDto
    {
        public String vorname { get; set; }
        public String name { get; set; }
        public String refnum { get; set; }
        public String type { get; set; }


        public DateTime? anfragedatum { get; set; }
        public long? anfrageuhrzeit { get; set; }

        public DateTime? rcvDate
        {
            get
            {
                return DateTimeHelper.CreateDate(anfragedatum, anfrageuhrzeit);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    anfrageuhrzeit = (long)val.Value;
                else
                    anfrageuhrzeit = 0;
                anfragedatum = value;
            }
        }
        
        
        
    }
}
