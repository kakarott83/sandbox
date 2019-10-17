using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.Web.BO.Search
{
    public abstract class AbstractXproSearchBo : IXproSearchBo
    {
        protected IXproLoaderDao loader;
        protected XproInfoFactory infoFactory;

        public AbstractXproSearchBo(IXproLoaderDao loader, XproInfoFactory infoFactory)
        {
            this.loader = loader;
            this.infoFactory = infoFactory;
        }
        /// <summary>
        /// Get List of Xproentites for xprocode and filter
        /// </summary>
        /// <param name="igetXproItems"></param>
        /// <returns>List of Xproentites</returns>
        public abstract XproEntityDto[] getXproItems(igetXproItemsDto input);
        

        /// <summary>
        /// Gets XproEntity for xprocode and entityid
        /// </summary>
        /// <param name="igetXproItem"></param>
        /// <returns>XproEntity</returns>
        public abstract XproEntityDto getXproItem(igetXproItemDto input);
    }
}