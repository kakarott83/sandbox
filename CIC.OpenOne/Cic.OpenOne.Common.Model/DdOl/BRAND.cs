using System;
using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.Common.Util.Extension;        //USE THIS EXTENSION!
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.Common.Model.DdOl
{
    public partial class BRANDHelper : global::System.Data.Objects.DataClasses.EntityObject
    {
        
        /// <summary>
        /// BRAND
        /// </summary>
        public BRANDHelper()
        {
        }

       

        #region Methods
        /// <summary>
        /// CheckPeroleInBrand
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="sysBRAND"></param>
        /// <returns></returns>
       /* public static bool CheckPeroleInBrand(OLContext context, long sysPEROLE, long sysBRAND)
        {
            List<long> IntersectList = null;
            bool IsValid = false;

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // Set current date
            System.DateTime CurrentTime = System.DateTime.Now.Date;

            // Get SYSPRHGROUP list by sysPEROLE
            var Query1 = from prhgroupm in context.PRHGROUPM
                         where prhgroupm.PEROLE.SYSPEROLE == sysPEROLE
                         select prhgroupm;

            var Query2 = from prhgroupm in Query1
                         where (prhgroupm.ACTIVEFLAG != null && prhgroupm.ACTIVEFLAG > 0 && (prhgroupm.VALIDFROM == null || prhgroupm.VALIDFROM <= CurrentTime) && (prhgroupm.VALIDUNTIL == null || prhgroupm.VALIDUNTIL >= CurrentTime))
                         select prhgroupm.PRHGROUP.SYSPRHGROUP;

            // Create list
            List<long> SYSPRHGROUPListFromPerole = Query2.ToList<long>();

            if (SYSPRHGROUPListFromPerole != null && SYSPRHGROUPListFromPerole.Count > 0)
            {
                // Get SYSPRHGROUP list by sysBRAND
                var Query3 = from prbrandm in context.PRBRANDM
                             where prbrandm.BRAND.SYSBRAND == sysBRAND
                             select prbrandm;

                var Query4 = from brand in Query3
                             where (brand.ACTIVEFLAG != null && brand.ACTIVEFLAG > 0 && (brand.VALIDFROM == null || brand.VALIDFROM <= CurrentTime) && (brand.VALIDUNTIL == null || brand.VALIDUNTIL >= CurrentTime))
                             select brand.PRHGROUP.SYSPRHGROUP;

                // Create list
                List<long> SYSPRHGROUPlistFromBrand = Query4.ToList<long>();

                if (SYSPRHGROUPlistFromBrand != null && SYSPRHGROUPlistFromBrand.Count > 0)
                {
                    // Make intersect
                    IntersectList = SYSPRHGROUPListFromPerole.Intersect(SYSPRHGROUPlistFromBrand).ToList();
                }
            }

            // Check intersect list
            if (IntersectList != null && IntersectList.Count > 0)
            {
                var Query = context.PRHGROUP.Where(context.BuildContainsExpression<PRHGROUP, long>(prhgroup => prhgroup.SYSPRHGROUP, IntersectList));
                Query = Query.Where(prhgroup => prhgroup.ACTIVEFLAG != null && prhgroup.ACTIVEFLAG > 0 && (prhgroup.VALIDFROM == null || prhgroup.VALIDFROM <= CurrentTime) && (prhgroup.VALIDUNTIL == null || prhgroup.VALIDUNTIL >= CurrentTime));

                // Get any
                IsValid = Query.Any<PRHGROUP>();
            }

            return IsValid;
        }*/
        #endregion

    }
}