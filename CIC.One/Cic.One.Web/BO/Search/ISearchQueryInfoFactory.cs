using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.Web.BO.Search
{
    public interface ISearchQueryInfoFactory
    {
        QueryInfoData getQueryInfo<T>();

        QueryInfoData getQueryInfo(String entityTable, String entityField);

        QueryInfoData getQueryInfo(String gviewId);
    }
}