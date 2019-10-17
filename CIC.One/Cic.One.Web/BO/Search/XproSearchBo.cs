using Cic.One.Web.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.Web.BO.Search
{
    public class XproSearchBo : AbstractXproSearchBo
    {
        

        public XproSearchBo(IXproLoaderDao loader, XproInfoFactory infoFactory)
            : base(loader,infoFactory)
        {
        }

        public override XproEntityDto[] getXproItems(igetXproItemsDto input)
        {
            return infoFactory.getXproItems(loader, input);
        }

        public override XproEntityDto getXproItem(igetXproItemDto input)
        {
            return infoFactory.getXproItem(loader, input);
        }
    }
}