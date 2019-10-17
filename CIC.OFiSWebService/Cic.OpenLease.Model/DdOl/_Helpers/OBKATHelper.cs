// OWNER TB, 19-02-2010
namespace Cic.OpenLease.Model.DdOl
{
    #region Using directives
    using System.Linq;
    #endregion

    [System.CLSCompliant(true)]
    public class OBKATHelper
    {
        #region Methods
        public static OBKAT[] SearchDescription(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, string description)
        {
            OBKAT[] OBKAT;

            // if description is null search all
            if (description == null || description == string.Empty)
            {
                var Query = from obKat in context.OBKAT
                            orderby obKat.SYSOBKAT descending
                            select obKat;
                OBKAT = Query.ToArray<OBKAT>();
            }
            else
            {
                var Query = from obKat in context.OBKAT
                            where obKat.DESCRIPTION == description
                            orderby obKat.SYSOBKAT descending
                            select obKat;
                OBKAT = Query.ToArray<OBKAT>();
            }

            return OBKAT;
        }
        #endregion
    }
}
