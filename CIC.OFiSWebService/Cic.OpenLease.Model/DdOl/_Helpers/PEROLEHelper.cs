// OWNER MK, 09-06-2009
namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System;
    using System.Linq;
    using System.Collections.Generic;
    #endregion


    /// <summary>
    /// @Deprecated old crap
    /// </summary>
    [System.CLSCompliant(true)]
    public class PEROLEHelper
    {
        #region Private constants
        public const long CnstVPRoleTypeNumber = 6;
        public const long CnstIMRoleTypeNumber = 3;
        #endregion

        #region Methods
       /* public static Cic.OpenLease.Model.DdOl.PEROLE FindVpOrRootPEROLE(Cic.OpenLease.Model.DdOl.OlExtendedEntities context, long sysPEROLE)
        {
            Cic.OpenLease.Model.DdOl.PEROLE PEROLE;

            if (context == null)
            {
                throw new System.ArgumentException("context");
            }

            try
            {
                // Deliver PEROLE
                PEROLE = Cic.OpenLease.Model.DdOl.PEROLEHelper.DeliverPeRole(context, sysPEROLE);

                // Fill instance (ROLETYPE)
                if (PEROLE != null)
                {
                    if (!PEROLE.ROLETYPEReference.IsLoaded)
                    {
                        // Load Roletype
                        PEROLE.ROLETYPEReference.Load();
                    }
                }
            }
            catch
            {
                throw;
            }

            if (PEROLE != null)
            {
                // Optymistic
                bool IsValid = true;
                bool IsVp = true;
 
                // Check is valid
                IsValid = MyCheckIsValid(PEROLE.VALIDFROM, PEROLE.VALIDUNTIL);

                // Check is vp
                IsVp = PEROLE.ROLETYPE != null && PEROLE.ROLETYPE.TYP != null && PEROLE.ROLETYPE.TYP.Value == CnstVPRoleTypeNumber;
                if (!(IsValid && IsVp))
                {
                    // Check the parrent
                    if (PEROLE.SYSPARENT.HasValue)
                    {
                        // Requrency
                        PEROLE = FindVpOrRootPEROLE(context, PEROLE.SYSPARENT.Value);
                    }
                }
            }

            return PEROLE;
        }
        */

       
       public static PEROLE DeliverPeRole(OlExtendedEntities context, long sysPeRole)
        {
            var Query = from perole in context.PEROLE
                        where perole.SYSPEROLE == sysPeRole
                        select perole;

            return Query.FirstOrDefault<PEROLE>();
        }

        /// <summary>
       /// Valid PEROLE has Handelsguppe
       /// TODO: Refactor and cache its usage
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPerson"></param>
        /// <returns></returns>
        public static List<PEROLE> DeliverValidPeRoles(OlExtendedEntities context, long sysPerson)
        {
            List<PEROLE> PeroleList;

            // PeRole list for a given person
            var Query = from perole in context.PEROLE
                        where perole.SYSPERSON == sysPerson
                        select perole;

            PeroleList = Query.ToList();

            return PeroleList;
        }

     /*   public static List<long> DeliverSightfieldPeRolesIds(OlExtendedEntities context, long sysPerson)
        {
            List<long> SysPeroleList;
            
            // PeRole list for a given person
            var Query = from perole in context.PEROLE
                        where perole.SYSPERSON == sysPerson
                        select perole.SYSPEROLE;

            SysPeroleList = Query.ToList<long>();

            foreach (long SysPeRoleLoop in Query.ToList<long>())
            {
                // Get peroles in sight field fo every perole
                SysPeroleList.AddRange(MyDeliverPeroles(context, SysPeRoleLoop));
            }

            // return unique peroles
            return SysPeroleList.Distinct<long>().ToList<long>();
        }*/

     /*   public static List<PEROLE> DeliverSightfieldPeRoles(OlExtendedEntities context, long sysPerson)
        {
            List<long> SysPeroleList;
            List<PEROLE> PeroleList;

            SysPeroleList = DeliverSightfieldPeRolesIds(context, sysPerson);
            PeroleList = new List<PEROLE>();

            foreach (long SysPeroleLoop in SysPeroleList)
            {
                var Query = from perole in context.PEROLE
                            where perole.SYSPEROLE == SysPeroleLoop
                            select perole;

                PEROLE PeRole = null;

                try
                {
                    PeRole = Query.FirstOrDefault<PEROLE>();
                }
                catch
                {
                    // Ignore
                }

                if (PeRole != null)
                {
                    // Load role types
                    if (!PeRole.ROLETYPEReference.IsLoaded)
                    {
                        PeRole.ROLETYPEReference.Load();
                    }
                    PeroleList.Add(PeRole);
                }
            }

            return PeroleList;
        }
        */
       
        #endregion

        #region My methods
        private static bool MyCheckIsValid(System.DateTime? validFrom, System.DateTime? validUntil)
        {
            // Optimistic
            bool IsValid = true;

            // Check Valid from
            if (validFrom.HasValue)
            {
                IsValid = IsValid && validFrom.Value.Date <= System.DateTime.Now.Date;
            }

            // Check Valid until
            if (validUntil.HasValue)
            {
                IsValid = IsValid && validUntil.Value.Date >= System.DateTime.Now.Date;
            }

            return IsValid;
        }

        // Go recursive up given number of elvels and get the new root perole
        private static long MyDeliverRootPerole(OlExtendedEntities context, long sysPerole, int levelUp)
        {
            levelUp--;

            if (levelUp < 0)
            {
                return sysPerole;
            }

            var Query = from perole in context.PEROLE
                        where perole.SYSPEROLE == sysPerole
                        select perole.SYSPARENT;

            long? SysParent = Query.FirstOrDefault<long?>();

            if (SysParent.HasValue)
            {
                sysPerole = MyDeliverRootPerole(context, SysParent.Value, levelUp);
            }

            return sysPerole;
        }

        // Get sight field peroles for a given perole
        private static List<long> MyDeliverPeroles(OlExtendedEntities context, long sysPerole)
        {
            List<long> SysPeroleList;
            long RootSysPerole;

            SysPeroleList = new List<long>();

            var Query = from perole in context.PEROLE
                        where perole.SYSPEROLE == sysPerole
                        select perole;

            PEROLE PeRole = Query.FirstOrDefault<PEROLE>();

            if (PeRole != null)
            {
                // Gu up SIGHTFIELDLEVEL number of levels to get new root value
                RootSysPerole = MyDeliverRootPerole(context, sysPerole, PeRole.SIGHTFIELDLEVEL.GetValueOrDefault(0));

                // Go down recursive and get the list
                MyDeliverPeroles(context, RootSysPerole, SysPeroleList, PeRole.SIGHTAUTHORITYLEVEL.GetValueOrDefault(0));
            }

            return SysPeroleList;
        }

        // Go recursive down and get all peroles in sight field
        private static void MyDeliverPeroles(OlExtendedEntities context, long sysPerole, List<long> sysPeroleList, int minLevel)
        {
            List<long> SysPeroleList;

            sysPeroleList.Add(sysPerole);

            var Query = from perole in context.PEROLE
                        where perole.SYSPARENT == sysPerole &&
                        perole.SIGHTAUTHORITYLEVEL <= minLevel
                        select perole.SYSPEROLE;

            SysPeroleList = Query.ToList<long>();

            foreach (long SysPeroleLoop in SysPeroleList)
            {
                MyDeliverPeroles(context, SysPeroleLoop, sysPeroleList, minLevel);
            }
        }
        #endregion

    }
}
