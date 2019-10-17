using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.Web.BO.Search
{
    public interface IXproSearchBo
    {
        /// <summary>
        /// Get List of Xproentites for xprocode and filter
        /// </summary>
        /// <param name="igetXproItems"></param>
        /// <returns>List of Xproentites</returns>
        XproEntityDto[] getXproItems(igetXproItemsDto input);

        /// <summary>
        /// Gets XproEntity for xprocode and entityid
        /// </summary>
        /// <param name="igetXproItem"></param>
        /// <returns>XproEntity</returns>
        XproEntityDto getXproItem(igetXproItemDto input);
    }
}