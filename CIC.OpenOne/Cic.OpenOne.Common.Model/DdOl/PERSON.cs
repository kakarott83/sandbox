using CIC.Database.OL.EF4.Model;
using System.Linq;

namespace Cic.OpenOne.Common.Model.DdOl
{
    public partial class PERSONHelper : global::System.Data.Objects.DataClasses.EntityObject
    {
      

        /// <summary>
        /// PERSON
        /// </summary>
        public PERSONHelper()
        {
        }

       

        #region Methods
        /// <summary>
        /// getPERSONByPeRole
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPEROLE"></param>
        /// <returns></returns>
        public static PERSON getPERSONByPeRole(OLContext context, long sysPEROLE)
        {
            string query = "select person.* from perole, person where perole.sysperson=person.sysperson and perole.sysperole=" + sysPEROLE;
            PERSON rval1 = context.ExecuteStoreQuery<PERSON>(query).FirstOrDefault<PERSON>();
            return rval1;
        }

        #endregion
    }
}