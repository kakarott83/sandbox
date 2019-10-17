// OWNER MK, 04-06-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
	using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class CICCONFHelper
    {
        #region Methods
        public static string DeliverOpSql(OlExtendedEntities context)
        {
            string sql = null;

            var Query = from cicconf in context.CICCONF
                        select cicconf;

            try
            {
                CICCONF Cicconf = Query.FirstOrDefault<CICCONF>();

                if (Cicconf != null)
                {
                    sql = Cicconf.EVALOPT3;
                }
            }
            catch
            {
                throw;
            }

            return sql;
        }

        public static int DeliverpTypPUser(OlExtendedEntities context)
        {
            int pTypPUser = 0;

            var Query = from cicconf in context.CICCONF
                        select cicconf;

            try
            {
                CICCONF Cicconf = Query.FirstOrDefault<CICCONF>();

                if (Cicconf != null)
                {
                    pTypPUser = Cicconf.PTYPPUSER.GetValueOrDefault();
                }
            }
            catch
            {
                throw;
            }

            return pTypPUser;
        }

       
        #endregion
    }
}
