// OWNER BK, 06-10-2008
namespace Cic.OpenLease.Model.DdOw
{
    using System.Linq;
    using Cic.OpenOne.Common.Util.Extension;
    using Cic.OpenOne.Common.Util.Security;
    [System.CLSCompliant(true)]
    public static class PortalUserHelper
    {
        #region Private constants
        // TODO BK 0 BK, Check key
        private const string CnstBlowfishKey = "PiastGliwiceW1Lidze0ooooooooole";
        #endregion

        #region Methods
        // TEST BK 0 BK, Not tested
        public static string EncryptPassword(string password)
        {
            Blowfish blowFish;

            try
            {
                blowFish = new Blowfish(CnstBlowfishKey);
                // Encode
                return blowFish.Encode(password);
            }
            catch
            {
                // Ignore exception
                return password;
            }
        }

        // TEST BK 0 BK, Not tested
        public static string DecryptPassword(string password)
        {
            Blowfish blowFish;

            try
            {
                blowFish = new Blowfish(CnstBlowfishKey);
                // Encode
                return blowFish.Decode(password);
            }
            catch
            {
                // Ignore exception
                return password;
            }
        }

        // TEST BK 0 BK, Not tested
      /*  public static Cic.OpenLease.Model.DdOw.PUSER Update(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.PUSER modifiedPortalUser, Cic.OpenLease.Model.DdOw.PUSER originalPortalUser)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check modiefied portal user
            if (modifiedPortalUser == null)
            {
                // Throw exception
                throw new System.ArgumentException("modifiedPortalUser");
            }

            try
            {
                // Update
                return owExtendedEntities.Update<PUSER>(modifiedPortalUser, originalPortalUser);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // TEST BK 0 BK, Not tested
        public static Cic.OpenLease.Model.DdOw.PUSER SelectById(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, long id)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            try
            {
                // Select portal user
                return owExtendedEntities.SelectById<PUSER>(id);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // TEST BK 0 BK, Not tested
        public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.PUSER> SelectByExternalId(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string externalId)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check external id
            if (string.IsNullOrEmpty(externalId))
            {
                // Throw exception
                throw new System.ArgumentException("externalId");
            }

            try
            {
                // Select portal users
                return MySelectByExternalId(owExtendedEntities, externalId, 0, 0);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // TEST BK 0 BK, Not tested
        public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.PUSER> SelectByExternalId(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string externalId, int pageIndex, int pageSize)
        {
            int Skip;
            int Top;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check external id
            if (string.IsNullOrEmpty(externalId))
            {
                // Throw exception
                throw new System.ArgumentException("externalId");
            }

            // Check page index
            if (pageIndex < 0)
            {
                // Throw exception
                throw new System.ArgumentException("pageIndex");
            }

            // Check page index
            if (pageSize < 0)
            {
                // Throw exception
                throw new System.ArgumentException("pageSize");
            }

            // Set default values
            Skip = 0;
            Top = 0;

            // Check page size
            if (pageSize > 0)
            {
                // Set values
                Skip = (pageIndex * pageSize);
                Top = pageSize;
            }

            try
            {
                // Select portal users
                return MySelectByExternalId(owExtendedEntities, externalId, Skip, Top);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        */
        /*
        // TEST BK 0 BK, Not tested
        public static Cic.OpenLease.Model.DdOw.PUSER SelectSingleByExternalId(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string externalId)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check external id
            if (string.IsNullOrEmpty(externalId))
            {
                // Throw exception
                throw new System.ArgumentException("externalId");
            }

            try
            {
                // Select portal users
                return MySelectSingleByExternalId(owExtendedEntities, externalId);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }*/
        /*
        // TEST BK 0 BK, Not tested
        public static int CountByExternalId(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string externalId)
        {
           

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check email
            if (string.IsNullOrEmpty(externalId))
            {
                // Throw exception
                throw new System.ArgumentException("externalId");
            }

          
            try
            {
                // Select portal users
                return owExtendedEntities.ExecuteStoreQuery<int>("select count(*) from puser where externeid='" + externalId + "'").FirstOrDefault();
               
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // TEST BK 0 BK, Not tested
        public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.PUSER> SelectByEmail(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string email, int pageIndex, int pageSize)
        {
            int Skip;
            int Top;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check email
            if (string.IsNullOrEmpty(email))
            {
                // Throw exception
                throw new System.ArgumentException("email");
            }

            // Check page index
            if (pageIndex < 0)
            {
                // Throw exception
                throw new System.ArgumentException("pageIndex");
            }

            // Check page index
            if (pageSize < 0)
            {
                // Throw exception
                throw new System.ArgumentException("pageSize");
            }

            // Set default values
            Skip = 0;
            Top = 0;

            // Check page size
            if (pageSize > 0)
            {
                // Set values
                Skip = (pageIndex * pageSize);
                Top = pageSize;
            }

            try
            {
                // Select portal users
                return MySelectByEmail(owExtendedEntities, email, Skip, Top);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // TEST BK 0 BK, Not tested
        public static int CountByEmail(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string email)
        {
          
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check email
            if (string.IsNullOrEmpty(email))
            {
                // Throw exception
                throw new System.ArgumentException("email");
            }

            // TODO MK 0 BK, Add functionality
            //// Where string
            //Where = (Cic.OpenLease.Model.DdOw.PUSER.Properties.Email.ToString() + " = @0");
            //// Parameters
            //WhereParams = new System.Collections.Generic.List<object>();
            //WhereParams.Add(email);

            try
            {
                // Select portal users
                return owExtendedEntities.ExecuteStoreQuery<int>("select count(*) from puser where email='"+email+"'").FirstOrDefault();
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // TEST BK 0 BK, Not tested
        public static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.PUSER> SelectAll(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, int pageIndex, int pageSize)
        {
            int Skip;
            int Top;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check page index
            if (pageIndex < 0)
            {
                // Throw exception
                throw new System.ArgumentException("pageIndex");
            }

            // Check page index
            if (pageSize < 0)
            {
                // Throw exception
                throw new System.ArgumentException("pageSize");
            }

            // Set default values
            Skip = 0;
            Top = 0;

            // Check page size
            if (pageSize > 0)
            {
                // Set values
                Skip = (pageIndex * pageSize);
                Top = pageSize;
            }

            try
            {
                // Select portal users
                return owExtendedEntities.Select<PUSER>(null, null, null, Skip, Top);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // TEST BK 0 BK, Not tested
        public static int Count(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            try
            {
                // Select portal users
                return owExtendedEntities.Count<PUSER>();
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // TEST BK 0 BK, Not tested
        public static void DeleteByExternalId(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string externalId)
        {
            Cic.OpenLease.Model.DdOw.PUSER PortalUser = null;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check external id
            if (string.IsNullOrEmpty(externalId))
            {
                // Throw exception
                throw new System.ArgumentException("externalId");
            }

            try
            {
                // Find portal user
                PortalUser = Cic.OpenLease.Model.DdOw.PortalUserHelper.MySelectSingleByExternalId(owExtendedEntities, externalId);
            }
            catch
            {
                // Ignore exception
            }

            if (PortalUser != null)
            {
                try
                {
                    // Delete
                    owExtendedEntities.Delete<PUSER>(PortalUser);
                }
                catch
                {
                    // Throw caught exception
                    throw;
                }
            }
        }

        public static Cic.OpenLease.Model.DdOw.PUSER Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string externalId, string name, string firstName, string password, bool disabled, string disabledReason, string email, string passwordQuestion, string passwordAnswer)
        {
            Cic.OpenLease.Model.DdOw.PUSER PortalUser;

            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check external id
            if (string.IsNullOrEmpty(externalId))
            {
                // Throw exception
                throw new System.ArgumentException("externalId");
            }

            // TODO MK 0 BK, Add functionality
            // New portal user
            PortalUser = new Cic.OpenLease.Model.DdOw.PUSER();
            // Set properties
            PortalUser.SYSPUSER = 0;
            PortalUser.EXTERNEID = externalId;
            PortalUser.NAME = name;
            PortalUser.VORNAME = firstName;
            PortalUser.KENNWORT = password;
            PortalUser.DISABLED = disabled ? 1 : 0;
            PortalUser.DISABLEDREASON = disabledReason;
            //PortalUser.EMail = email;
            //PortalUser.PasswordQuestion = passwordQuestion;
            //PortalUser.PasswordAnswer = passwordAnswer;

            try
            {
                // Insert
                owExtendedEntities.Insert<PUSER>(PortalUser);
            }
            catch
            {
                // Throw caught exception
                throw;
            }

            // Return
            return PortalUser;
        }

        public static void Insert(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, Cic.OpenLease.Model.DdOw.PUSER portalUser)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                // Throw exception
                throw new System.ArgumentException("owEntities");
            }

            // Check portal user
            if (portalUser == null)
            {
                // Throw exception
                throw new System.ArgumentException("portalUser");
            }

            try
            {
                // Insert
                owExtendedEntities.Insert<PUSER>(portalUser);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        #endregion

        #region My methods
        private static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.PUSER> MySelectByEmail(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string email, int skip, int top)
        {
            System.Collections.Generic.List<object> WhereParams = null;
            string Where = null;

            // TODO MK 0 BK, Add functionality
            //// Where string
            //Where = (Cic.OpenLease.Model.DdOw.PUSER.Properties.Email.ToString() + " = @0");
            //// Parameters
            //WhereParams = new System.Collections.Generic.List<object>();
            //WhereParams.Add(email);

            try
            {
                // Select
                return owExtendedEntities.Select<PUSER>(Where, WhereParams.ToArray(), null, skip, top);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        private static System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.PUSER> MySelectByExternalId(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string externalId, int skip, int top)
        {
            System.Collections.Generic.List<object> WhereParams;
            string Where;

            // Where string
            Where = (Cic.OpenLease.Model.DdOw.PUSER.FieldNames.EXTERNEID + " = @0");
            // Parameters
            WhereParams = new System.Collections.Generic.List<object>();
            WhereParams.Add(externalId);

            try
            {
                // Select
                return owExtendedEntities.Select<PUSER>(Where, WhereParams.ToArray(), null, skip, top);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        */
        /*
        private static Cic.OpenLease.Model.DdOw.PUSER MySelectSingleByExternalId(Cic.OpenLease.Model.DdOw.OwExtendedEntities owExtendedEntities, string externalId)
        {
            System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.PUSER> PortalUsers;
            Cic.OpenLease.Model.DdOw.PUSER PortalUser = null;

            try
            {
                // Select portal user
                PortalUsers = MySelectByExternalId(owExtendedEntities, externalId, 0, 0);
            }
            catch
            {
                // Throw caught exception
                throw;
            }

            // Check result
            if ((PortalUsers != null) && (PortalUsers.Count == 1))
            {
                // Get first
                PortalUser = PortalUsers[0];
            }

            // Return
            return PortalUser;
        }*/
        #endregion
    }
}
