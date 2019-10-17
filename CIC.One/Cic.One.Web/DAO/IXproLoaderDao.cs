using System.Collections.Generic;
using System.Dynamic;

namespace Cic.One.Web.DAO
{
    public interface IXproLoaderDao
    {
        ExpandoObject LoadData(XproInfoBaseDao info, long entityId);

        List<ExpandoObject> LoadDatas(XproInfoBaseDao info, string filter);
    }
}