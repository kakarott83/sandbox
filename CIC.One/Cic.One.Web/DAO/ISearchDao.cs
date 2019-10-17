using Cic.One.DTO;
using Cic.One.Web.BO.Search;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;

namespace Cic.One.Web.DAO
{
    public interface ISearchDao<R>
    {
        /// <summary>
        /// returns a description of all result fields
        /// </summary>
        /// <returns></returns>
        List<Viewfield> getFields();
        List<R> search(string query, object[] param);
        IEnumerable<R> search(ObjectContext ctx, string query, object[] param);
		void PostPrepare (oSearchDto<R> found, long syswfuser, QueryInfoData infoData);
        oSearchDto<T> PostConvert<T>(oSearchDto<R> found, long permissionId);
    }
}
