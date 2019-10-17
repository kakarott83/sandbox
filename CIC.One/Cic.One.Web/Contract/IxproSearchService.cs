using System.ServiceModel;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Extension;
using System;

namespace Cic.One.Web.Contract
{
    /// <summary>
    /// Das Interface IxproSearchService stellt die Methoden für Xpros bereit
    /// </summary>
    [ServiceContract(Name = "IxproSearchService", Namespace = "http://cic-software.de/One")]
    public interface IxproSearchService
    {
        /// <summary>
        /// Get List of Xproentites for xprocode and filter
        /// </summary>
        /// <param name="igetXproItems"></param>
        /// <returns>List of Xproentites</returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetXproItemsDto getXproItems(igetXproItemsDto igetXproItems);

        /// <summary>
        /// Gets XproEntity for xprocode and entityid
        /// </summary>
        /// <param name="igetXproItem"></param>
        /// <returns>XproEntity</returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        ogetXproItemDto getXproItem(igetXproItemDto igetXproItem);

        [OperationContract]
        XproEntityDto[] getItems(String code, long sysperole, long sysvlm, String isocode, String filter, String filtername1, String filtervalue1, String domain);

   
    }
}