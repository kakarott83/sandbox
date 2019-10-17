// OWNER MK, 05-08-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using directives
    using System.Linq;
    using System;
    using Cic.OpenOne.Common.Util;
    #endregion

    [System.CLSCompliant(true)]
    public class OBTYPHelper
    {
        #region Methods
        public static long DeliverSysRwVg(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, string obTypMapCode)
        {
            var Query = from obTypMap in context.OBTYPMAP
                        where obTypMap.CODE == obTypMapCode
                        orderby obTypMap.SYSOBTYPMAP descending
                        select obTypMap.OBTYP;

            System.Collections.Generic.List<OBTYP> ObTypList = Query.Distinct().ToList();

            OBTYP ObTyp = null;

            if (ObTypList != null && ObTypList.Count > 0)
            {
                ObTyp = ObTypList[0];
            }

            return ObTyp.SYSVGRW.GetValueOrDefault();
        }

        public static string GetSchwacke(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long sysObTyp)
        {
            string Schwacke = string.Empty;

            OBTYP OBTYP = MyGetObTyp(context, sysObTyp);

            if (OBTYP == null)
            {
                throw new Exception("OBTYP is null");
            }

            while(OBTYP.SYSOBTYPP != null)
            {
                Schwacke = MyGetSchwacke(context, OBTYP.SYSOBTYPP.GetValueOrDefault());
                if (Schwacke == "10" || Schwacke == "20" || Schwacke == "40")
                {
                    return Schwacke;
                }

                OBTYP = MyGetObTyp(context, OBTYP.SYSOBTYPP.GetValueOrDefault());
            }

            return Schwacke;
        }

        public static OBTYP[] SearchDescription(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, string description, int skip, int top)
        {
            try
            {
                //TODO WB, if Dictionary will change add IMPORTSOURCE field
                OBTYP[] OBTYP;

                // Check if description is empty
                if (StringUtil.IsTrimedNullOrEmpty(description))
                {
                    // Query OBTYP
                    var Query = from obTyp in context.OBTYP
                                where obTyp.IMPORTSOURCE == 2
                                select obTyp;

                    // Get the rows
                    OBTYP = Query.Distinct().OrderByDescending(Typ => Typ.SYSOBTYP).Skip(skip).Take(top).ToArray<OBTYP>();
                }
                else
                {
                    // Query OBTYP
                    var Query = from obTyp in context.OBTYP
                                where obTyp.BEZEICHNUNG == description && obTyp.IMPORTSOURCE == 2
                                select obTyp;

                    // Get the rows
                    OBTYP = Query.Distinct().OrderByDescending(Typ => Typ.SYSOBTYP).Skip(skip).Take(top).ToArray<OBTYP>();
                }

                // Return the OBTYP array
                return OBTYP;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("SearchDescription() failed", e);
            }
        }

        public static int SearchDescriptionCount(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, string description)
        {
            try
            {
                // Declare count
                int Count;

                // Check if description is empty
                if (StringUtil.IsTrimedNullOrEmpty(description))
                {
                    // Query OBTYP
                    var Query = from obTyp in context.OBTYP
                                where obTyp.IMPORTSOURCE == 2
                                orderby obTyp.SYSOBTYP descending
                                select obTyp;

                    // Count the rows
                    Count = Query.Distinct().Count();
                }
                else
                {
                    // Query OBTYP
                    var Query = from obTyp in context.OBTYP
                                where obTyp.BEZEICHNUNG == description && obTyp.IMPORTSOURCE == 2
                                orderby obTyp.SYSOBTYP descending
                                select obTyp;

                    // Count the rows
                    Count = Query.Distinct().Count();
                }

                // Return the number of the rows
                return Count;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("SearchDescriptionCount() failed", e);
            }
        }
        #endregion

        #region My methods
        private static string MyGetSchwacke(OlExtendedEntities context, long sysObTyp)
        {
            OBTYP OBTYP;
            var Query = from obtyp in context.OBTYP
                        where obtyp.SYSOBTYP == sysObTyp
                        select obtyp;

            OBTYP = Query.FirstOrDefault();

            return OBTYP.SCHWACKE;
        }

        private static OBTYP MyGetObTyp(OlExtendedEntities context, long sysObTyp)
        {
            OBTYP OBTYP;
            var Query = from obtyp in context.OBTYP
                        where obtyp.SYSOBTYP == sysObTyp
                        select obtyp;

            OBTYP = Query.FirstOrDefault();

            return OBTYP;
        }
        #endregion
    }
}
