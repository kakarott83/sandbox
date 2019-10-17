using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Objects;
using System.Dynamic;
using System.Linq;
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.One.Web.DAO
{
    public class XproLoaderDao : IXproLoaderDao
    {
        public ExpandoObject LoadData(XproInfoBaseDao info, long entityId)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                string query = info.QueryItem;
                query = string.Format(query, entityId);
                var c = new ObjectQuery<DbDataRecord>(query, ctx);

                DbDataRecord result = c.FirstOrDefault();
                if (result == null)
                {
                    throw new Exception("Entity Id " + entityId + " not found for " + info.Area);
                }

                return ToExpando(result);
            }
        }

        public ExpandoObject ToExpando(DbDataRecord record)
        {
            ExpandoObject result = new ExpandoObject();
            IDictionary<String, object> dict = (result as IDictionary<String, object>);

            for (int f = 0; f < record.FieldCount; f++)
            {
                //PropertyInfo p = item.GetType().GetProperty(record.GetName(f));
                dict[record.GetName(f)] = record.GetValue(f);
            }
            return result;
        }

        private List<ObjectParameter> CreateParameters(params string[] parameters)
        {
            var result = new List<ObjectParameter>();
            for (int i = 0; i < parameters.Length; i++)
            {
                result.Add(new ObjectParameter("arg" + i.ToString(), parameters[i]));
            }
            return result;
        }

        public List<ExpandoObject> LoadDatas(XproInfoBaseDao info, string filter)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                //string[] filters = null;
                string query = info.QueryItems;
                if (filter != null)
                {
                    string[] filters = filter.Split(' ');
                    query = string.Format(query, filters);
                }

                ObjectQuery<DbDataRecord> result = new ObjectQuery<DbDataRecord>(query, ctx);

                List<ExpandoObject> items = new List<ExpandoObject>();
                foreach (DbDataRecord record in result)
                {
                    items.Add(ToExpando(record));
                }
                return items;
            }
        }
    }
}