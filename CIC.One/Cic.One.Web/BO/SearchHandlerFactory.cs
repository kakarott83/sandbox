using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.DTO;

namespace Cic.One.Web.BO
{
    /// <summary>
    /// Creates the necessary wrapper for a webservice call into the Search-BO for the given Types
    /// the service-endpoint method only needs to call e.g.
    ///      return new SearchHandlerFactory<RecalcDto, oSearchRecalcDto>().search(iSearch);
    /// </summary>
    /// <typeparam name="V">the Entity Tpe, e.g. RecalcDto</typeparam>
    /// <typeparam name="T">the return Type e.g. oSearchRecalcDto which must extend oSearchResultDto</typeparam>
    public class SearchHandlerFactory<V,T> where T : oSearchResultDto<V>
    {
        public T search(iSearchDto iSearch)
        {
            ServiceHandler<iSearchDto, T> ew = new ServiceHandler<iSearchDto, T>(iSearch);
            return ew.process(delegate(iSearchDto input, T rval, CredentialContext ctx)
            {

                if (input == null)
                    throw new ArgumentException("No search input");
                rval.result = new SearchBo<V>(SearchQueryFactoryFactory.getInstance(), ctx.getMembershipInfo().sysPEROLE, input.queryId,ctx.getMembershipInfo().ISOLanguageCode).search(input);

            });
        }
    }
}