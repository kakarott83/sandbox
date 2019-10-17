// OWNER BK, 26-10-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using directives
    using System.Linq;
    using System.Collections.Generic;
    #endregion

    [System.CLSCompliant(true)]
    public static class INTTYPEHelper
    {
        #region Methods
        public static Cic.OpenLease.Model.DdOl.INTTYPE[] SearchInttypes(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long sysPrProdukt)
        {

            var Query = from inttype in context.INTTYPE
                        orderby inttype.INTTYPE1
                        select inttype;

            List<INTTYPE> rval = Query.ToList<INTTYPE>();;
            List<INTTYPE> result = new List<INTTYPE>();
 
            //either some are configured or all are used
            var Query2 = from p in context.PRCLPRINTTYPE
                         where  p.PRPRODUCT.SYSPRPRODUCT == sysPrProdukt
                         select p;
            bool all = true;
            if (Query2.Count() > 0)
            {
                all = false;
            }
            foreach (INTTYPE it in rval)
            {
                if (all)
                {
                    result.Add(it);
                }
                else {
                    var Query3 = from p in context.PRCLPRINTTYPE
                                 where it.SYSINTTYPE == p.INTTYPE.SYSINTTYPE
                                        && p.PRPRODUCT.SYSPRPRODUCT == sysPrProdukt
                                 select p.PRPRODUCT.INTTYPE.SYSINTTYPE;
                    if (Query3.Count() != 0)
                    {
                        result.Add(it);
                    }
                }
               

            }
            return result.ToArray<INTTYPE>();
        }
        #endregion

    }
}