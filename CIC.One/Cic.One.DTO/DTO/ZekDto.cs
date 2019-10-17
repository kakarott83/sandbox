using System;
using Cic.OpenOne.Common.DTO;
using System.Collections.Generic;

namespace Cic.One.DTO
{
    /// <summary>
    /// data structure managing both request and response data of a zek information request
    /// </summary>
    public class ZekDto : EntityDto
    {
        #region input / request data
        /// <summary>
        /// person whose zek information is requested
        /// </summary>
        public AccountDto person { get; set; }
        /// <summary>
        /// unique number identifying the customer at zek
        /// </summary>
        public string zekcustomerid { get; set; }
        /// <summary>
        /// request and context information that can be saved in AUSKUNFT
        /// </summary>
        public AbfragedatenDto abfragedaten { get; set; }
        /// <summary>
        /// zek and/or iko
        /// </summary>
        public int zielverein { get; set; }
        /// <summary>
        /// Natürliche Person = 0, Juristische Person = 1
        /// </summary>
        public int persontyp { get; set; }
        /// <summary>
        /// comment to the zek request
        /// </summary>
        public string bemerkung { get; set; }

        /// <summary>
        /// context: nice number from object in specified context
        /// </summary>
        public string nummer { get; set; }

        #endregion input / request data

        #region output / response data

        /// <summary>
        /// primary key
        /// </summary>
        public long syszek { get; set; }

        /// <summary>
        /// person's contracts found by zek
        /// </summary>
        public List<ZekContractDto> contracts { get; set; }
        /// <summary>
        /// person's credit pleas found by zek
        /// </summary>
        public List<ZekPleaDto> pleas { get; set; }
        /// <summary>
        /// person's Kartenmelfung found by zek
        /// </summary>
        public List<ZekKartenmeldungDto> kartenmeldungen { get; set; }
        /// <summary>
        /// person's credit pleas found by zek
        /// </summary>
        public List<ZekECodeDto> ecodes { get; set; }
        #endregion output / response data

        /// <summary>
        /// get identification
        /// </summary>
        /// <returns>primary key</returns>
        public override long getEntityId()
        {
            return syszek;
        }

    }
}
