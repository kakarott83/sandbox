using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Base Class of a condition Link
    /// The information in this class is used to determine the assignment of some condition-entity via its primary key conditionid to
    /// the target entity via its primary key targetid
    /// </summary>
    public class ConditionLink
    {
        /// <summary>
        /// the foreign key to use when the condition is met
        /// </summary>
        public long TARGETID;

        /// <summary>
        /// the value of the condition to test, e.g. sysbrand
        /// </summary>
        public long CONDITIONID;
        /// <summary>
        /// Valid From
        /// </summary>
        public DateTime VALIDFROM {get;set;}
        /// <summary>
        /// Valid UNtil
        /// </summary>
        public DateTime VALIDUNTIL {get;set;}



        /// <summary>
        /// fetches the defined condition link type value from the context object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctype"></param>
        /// <param name="map"></param>
        /// <param name="obDao"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public static List<long> getParameter(object context, ConditionLinkType ctype, Dictionary<ConditionLinkType, String> map, IObTypDao obDao, DateTime perDate)
        {
            List<long> rval = new List<long>();
            if (ctype == ConditionLinkType.COMMON)
            {
                rval.Add(0);
                return rval;
            }
            String paramId = map[ctype];
            string Parameter = "get_" + paramId.ToLower();
            MethodInfo mi = context.GetType().GetMethod(Parameter);
            if (mi == null) throw new System.MethodAccessException("Method " + Parameter + " not found in " + context.GetType());
            
            switch (ctype)
            {
                case ConditionLinkType.PRHGROUPEXT:
                {
                    long key = Convert.ToInt64(mi.Invoke(context, null));
                    paramId = map[ConditionLinkType.OBTYP];
                    mi = context.GetType().GetMethod("get_" + paramId);
                    long sysobtyp = Convert.ToInt64(mi.Invoke(context, null));
                    return obDao.getPrhGroups(key, sysobtyp, perDate);
                }
                case ConditionLinkType.OBTYP:
                {
                    long key = Convert.ToInt64(mi.Invoke(context, null));
                    return obDao.getObTypAscendants(key);
                }
               
                default:
                    {
                        long key = Convert.ToInt64(mi.Invoke(context, null));
                        rval.Add(key);
                        break;
                    }
            }

            return rval;
        }
    }
}
