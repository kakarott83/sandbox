// OWNER MK, 20-05-2008
using Cic.OpenLease.Service;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Common
{
    [System.CLSCompliant (true)]
    public sealed class MembershipProvider : System.Web.Security.MembershipProvider
    {
        #region Private variables
        private string _ApplicationName;
        private bool _EnablePasswordRetrieval;
        private bool _EnablePasswordReset;
        private bool _RequiresQuestionAndAnswer;
        private bool _RequiresUniqueEmail;
        private int _MaxInvalidPasswordAttempts;
        private int _PasswordAttemptWindow;
        private int _MinRequiredPasswordLength;
        private int _MinRequiredNonalphanumericCharacters;
        private string _PasswordStrengthRegularExpression;
        private System.Web.Security.MembershipPasswordFormat _PasswordFormat;
        private static readonly ILog _Log = Log.GetLogger (MethodBase.GetCurrentMethod ().DeclaringType);
        #endregion

        #region Methods
        public new void Initialize (string name, System.Collections.Specialized.NameValueCollection config)
        {
            System.Collections.Specialized.NameValueCollection NameValueCollection;

            // Provider name
            _ApplicationName = name;

            _EnablePasswordRetrieval = true;
            _EnablePasswordReset = false;
            _RequiresQuestionAndAnswer = false;
            _RequiresUniqueEmail = false;
            _MaxInvalidPasswordAttempts = 5;
            _PasswordAttemptWindow = 10;
            _MinRequiredPasswordLength = 3;
            _MinRequiredNonalphanumericCharacters = 1;
            _PasswordStrengthRegularExpression = "";
            _PasswordFormat = System.Web.Security.MembershipPasswordFormat.Clear;

            // New collection
            NameValueCollection = new System.Collections.Specialized.NameValueCollection ();

            // Set config values
            NameValueCollection.Add ("enablePasswordRetrieval", System.Convert.ToInt16 (_EnablePasswordRetrieval).ToString ()); // 0 or 1
            NameValueCollection.Add ("enablePasswordReset", System.Convert.ToInt16 (_EnablePasswordReset).ToString ()); // 0 or 1
            NameValueCollection.Add ("requiresQuestionAndAnswer", System.Convert.ToInt16 (_RequiresQuestionAndAnswer).ToString ()); // 0 or 1
            NameValueCollection.Add ("requiresUniqueEmail", System.Convert.ToInt16 (_RequiresUniqueEmail).ToString ()); // 0 or 1
            NameValueCollection.Add ("maxInvalidPasswordAttempts", _MaxInvalidPasswordAttempts.ToString ()); // > 0
            NameValueCollection.Add ("passwordAttemptWindow", _PasswordAttemptWindow.ToString ()); // > 0
            NameValueCollection.Add ("minRequiredPasswordLength", _MinRequiredPasswordLength.ToString ()); // > 0 <= 128
            NameValueCollection.Add ("minRequiredNonalphanumericCharacters", _MinRequiredNonalphanumericCharacters.ToString ()); // >= 0 <= 128
            NameValueCollection.Add ("passwordStrengthRegularExpression", _PasswordStrengthRegularExpression); // must be a valid regular expression
            NameValueCollection.Add ("passwordFormat", _PasswordFormat.ToString ()); // MembershipPasswordFormat enumeration

            // Initialize base
            base.Initialize (_ApplicationName, NameValueCollection);
        }

        // NOTE BK, No exceptions ! Return type: true if the password was updated successfully; otherwise, false. 
        public override bool ChangePassword (string userName, string oldPassword, string newPassword)
        {
            System.Web.Security.ValidatePasswordEventArgs ValidatePasswordEventArgs;
            CIC.Database.OW.EF6.Model.PUSER PortalUser;
            bool Success = false;

            // Check user name
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            // Check old password
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (oldPassword))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            // Check new password
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (newPassword))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            // Trim values
            userName = userName.Trim ();
            oldPassword = oldPassword.Trim ();
            newPassword = newPassword.Trim ();

            // New event arguments
            ValidatePasswordEventArgs = new System.Web.Security.ValidatePasswordEventArgs (userName, newPassword, false);
            // Validate
            this.OnValidatingPassword (ValidatePasswordEventArgs);

            // Check arguments
            if (ValidatePasswordEventArgs.Cancel == false)
            {
                try
                {
                    // Entities
                    using (DdOwExtended OwEntities = new DdOwExtended ())
                    {
                        // Find valid portal user
                        PortalUser = MyFindValidPortalUser (OwEntities, userName, oldPassword);
                        // Check object
                        if (PortalUser != null)
                        {
                            // Change password
                            if (_PasswordFormat == System.Web.Security.MembershipPasswordFormat.Encrypted)
                            {
                                PortalUser.KENNWORT = PUserUtil.EncryptPassword (newPassword);
                            }
                            else
                            {
                                PortalUser.KENNWORT = newPassword;
                            }

                            // Update
                          //  PortalUserHelper.Update (OwEntities, PortalUser, null);
                            // Set state
                            Success = true;
                        }
                    }
                }
                catch
                {
                    // Return (see NOTE)
                    // NOTE BK, More then one return
                    return Success;
                }
            }

            // Return
            // NOTE BK, More then one return
            return Success;
        }

        // NOTE BK, No exceptions ! Return type: true if the password question and answer are updated successfully; otherwise, false. If the supplied user name and password are not valid, false is returned. 
        public override bool ChangePasswordQuestionAndAnswer (string userName, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            CIC.Database.OW.EF6.Model.PUSER PortalUser;
            bool Success = false;

            // Check state
            if (!_RequiresQuestionAndAnswer)
            {
                // Throw exception
                throw new Exception ();
            }

            // Check user name
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            // Check password
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (password))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            // Check new password question
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (newPasswordQuestion))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            // Check new password answer
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (newPasswordAnswer))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            // Trim values
            userName = userName.Trim ();
            password = password.Trim ();
            newPasswordQuestion = newPasswordQuestion.Trim ();
            newPasswordAnswer = newPasswordAnswer.Trim ();

            try
            {
                // Entities
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Find valid portal user
                    PortalUser = MyFindValidPortalUser (OwEntities, userName, password);
                    // Check object
                    if (PortalUser != null)
                    {
                        // TODO MK 0 BK, Add functionality
                        //// Set properties
                        //PortalUser.PasswordQuestion = newPasswordQuestion;
                        //PortalUser.PasswordAnswer = newPasswordAnswer;
                        // Update
                        //PortalUserHelper.Update (OwEntities, PortalUser, null);
                        // Set state
                        Success = true;
                    }
                }
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            // Return
            // NOTE BK, More then one return
            return Success;
        }

        // NOTE BK, No exceptions !
       public override System.Web.Security.MembershipUser CreateUser (string userName, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            System.Web.Security.ValidatePasswordEventArgs ValidatePasswordEventArgs;
            PUSER PortalUser;
            System.Web.Security.MembershipUser MemberhipUser = null;
            int CountOfEntries;

            // Set state
            status = System.Web.Security.MembershipCreateStatus.ProviderError;

            // Check user name
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                // Set state
                status = System.Web.Security.MembershipCreateStatus.InvalidUserName;
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MemberhipUser;
            }

            // Check password
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (password))
            {
                // Set state
                status = System.Web.Security.MembershipCreateStatus.InvalidPassword;
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MemberhipUser;
            }

            // Check email
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (email))
            {
                // Set state
                status = System.Web.Security.MembershipCreateStatus.InvalidEmail;
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MemberhipUser;
            }

            // Check state
            if (_RequiresQuestionAndAnswer)
            {
                // Check new password question
                if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (passwordQuestion))
                {
                    // Set state
                    status = System.Web.Security.MembershipCreateStatus.InvalidQuestion;
                    // Return (see NOTE)
                    // NOTE BK, More then one return
                    return MemberhipUser;
                }

                // Check new password answer
                if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (passwordAnswer))
                {
                    // Set state
                    status = System.Web.Security.MembershipCreateStatus.InvalidAnswer;
                    // Return (see NOTE)
                    // NOTE BK, More then one return
                    return MemberhipUser;
                }
            }

            // Trim values
            userName = userName.Trim ();
            password = password.Trim ();
            email = email.Trim ();
            if (passwordQuestion != null)
            {
                passwordQuestion = passwordQuestion.Trim ();
            }
            if (passwordAnswer != null)
            {
                passwordAnswer = passwordAnswer.Trim ();
            }

            // New event arguments
            ValidatePasswordEventArgs = new System.Web.Security.ValidatePasswordEventArgs (userName, password, false);
            // Validate
            this.OnValidatingPassword (ValidatePasswordEventArgs);

            // Check arguments
            if (ValidatePasswordEventArgs.Cancel == false)
            {
                try
                {
                    // Entities
                    using (DdOwExtended OwEntities = new DdOwExtended ())
                    {
                        // Check state
                        if (_RequiresUniqueEmail)
                        {
                            // Set output
                          //  CountOfEntries = PortalUserHelper.CountByEmail (OwEntities, email);
                            // Check value
                           /* if (CountOfEntries > 0)
                            {
                                // Set state
                                status = System.Web.Security.MembershipCreateStatus.InvalidEmail;
                            }*/
                        }

                        // Check state
                        if (status != System.Web.Security.MembershipCreateStatus.InvalidEmail)
                        {
                            // Mapping PortalUser <> MembershipUser
                            //-------------------------------------
                            // PortalUser.SysPUSER         <> MembershipUser.ProviderUserKey
                            // PortalUser.ExternalId       <> MembershipUser.Email
                            // PortalUser.Name             <> MembershipUser.Email
                            // PortalUser.FirstName           null
                            // PortalUser.Password            -
                            // PortalUser.Disabled         <> !isApproved
                            // PortalUser.DisabledReason   <> (MembershipUser.isApproved ? null : "Is not approved.")
                            // PortalUser.Email            <> MembershipUser.Email
                            // PortalUser.PasswordQuestion <> MembershipUser.PasswordQuestion
                            // PortalUser.PasswordAnswer      -


                            // Encode password if needed
                            if (_PasswordFormat == System.Web.Security.MembershipPasswordFormat.Encrypted)
                            {
                                password = PUserUtil.EncryptPassword (password);
                            }

                            // Create portal user
                            // TODO MK 0 BK, Localize text
                            //PortalUser = PortalUserHelper.Insert (OwEntities, userName, userName, null, password, !isApproved, isApproved ? null : "Is not approved.", email, passwordQuestion, passwordAnswer);
                            // Check user
                          /*  if (PortalUser != null)
                            {
                                // Create membership user
                                MemberhipUser = MyCreateMemberhipUserFromPortalUser (PortalUser);
                                // Set state
                                status = System.Web.Security.MembershipCreateStatus.Success;
                            }*/
                        }
                    }
                }
                catch
                {
                    // Return (see NOTE)
                    // NOTE BK, More then one return
                    return MemberhipUser;
                }
            }
            else
            {
                // Set state
                status = System.Web.Security.MembershipCreateStatus.InvalidPassword;
            }

            // Return
            // NOTE BK, More then one return
            return MemberhipUser;
        }

        // NOTE BK, Only a ProviderException will be thrown if the ValidationKey property or DecryptionKey property is set to AutoGenerate.
        public new byte[] DecryptPassword (byte[] encodedPassword)
        {
            string EncodedPassword;
            string Password = null;

            // Check encoded password array
            if (encodedPassword == null)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return null;
            }

            // Check encoded password array
            if (encodedPassword.GetLength (0) == 0)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return null;
            }

            try
            {
                // Bytes to string
                EncodedPassword = System.Convert.ToBase64String (encodedPassword);
                // Decrypt password
                Password = PUserUtil.DecryptPassword (EncodedPassword);
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return null;
            }

            // Return
            // NOTE BK, More then one return
            return System.Text.ASCIIEncoding.ASCII.GetBytes (Password);
        }

        // NOTE BK, No exceptions ! Return type: true if the user was successfully deleted; otherwise, false.
        public override bool DeleteUser (string userName, bool deleteAllRelatedData)
        {
            bool Success = false;

            // Check user name
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            try
            {
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Delete user
                   // PortalUserHelper.DeleteByExternalId (OwEntities, userName);
                    // Set state
                    Success = true;
                }
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Success;
            }

            // Return
            // NOTE BK, More then one return
            return Success;
        }

        // NOTE BK, Only a ProviderException will be thrown if the ValidationKey property or DecryptionKey property is set to AutoGenerate.
        public new byte[] EncryptPassword (byte[] password)
        {
            string Password;
            string EncodedPassword = null;

            // Check password array
            if (password == null)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return null;
            }

            // Check encoded password array
            if (password.GetLength (0) == 0)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return null;
            }

            try
            {
                // Bytes to string
                Password = System.Convert.ToBase64String (password);
                // Encrypt password
                EncodedPassword = PUserUtil.EncryptPassword (Password);
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return null;
            }

            // Return
            // NOTE BK, More then one return
            return System.Text.ASCIIEncoding.ASCII.GetBytes (EncodedPassword);
        }

        // NOTE BK, NullReferenceException: The obj parameter is a null reference (Nothing in Visual Basic). 
        public override bool Equals (object obj)
        {
            try
            {
                // Base
                return base.Equals (obj);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // NOTE BK, No exceptions !
        public override System.Web.Security.MembershipUserCollection FindUsersByEmail (string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            System.Collections.Generic.List<PUSER> PortalUsers;
            System.Web.Security.MembershipUserCollection MembershipUserCollection;
            System.Web.Security.MembershipUser MembershipUser;

            // Set default output
            totalRecords = -1;
            // Create new collection
            MembershipUserCollection = new System.Web.Security.MembershipUserCollection ();

            // Check email to match
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (emailToMatch))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Check page index
            if (pageIndex < 0)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Check page index
            if (pageSize < 0)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Reset output
            totalRecords = 0;
            // Trim values
            emailToMatch = emailToMatch.Trim ();

            try
            {
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Select portal users
                 //   PortalUsers = PortalUserHelper.SelectByEmail (OwEntities, emailToMatch, pageIndex, pageSize);
                    // Set output
                 //   totalRecords = PortalUserHelper.CountByEmail (OwEntities, emailToMatch);
                }
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Check portal users
         /*   if ((PortalUsers != null) && (PortalUsers.Count > 0))
            {
                // Loop through portal users
                foreach (PUSER LoopPortalUser in PortalUsers)
                {
                    // Create membership user
                    MembershipUser = MyCreateMemberhipUserFromPortalUser (LoopPortalUser);
                    // Add to collection
                    MembershipUserCollection.Add (MembershipUser);
                }
            }*/

            // Return
            // NOTE BK, More then one return
            return MembershipUserCollection;
        }
      
        public override System.Web.Security.MembershipUserCollection FindUsersByName (string userNameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            System.Collections.Generic.List<PUSER> PortalUsers;
            System.Web.Security.MembershipUserCollection MembershipUserCollection;
            System.Web.Security.MembershipUser MembershipUser;

            // Set default output
            totalRecords = -1;
            // Create new collection
            MembershipUserCollection = new System.Web.Security.MembershipUserCollection ();

            // Check user name to match
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userNameToMatch))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Check page index
            if (pageIndex < 0)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Check page index
            if (pageSize < 0)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Reset output
            totalRecords = 0;
            // Trim values
            userNameToMatch = userNameToMatch.Trim ();

            try
            {
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Select portal users
                   // PortalUsers = PortalUserHelper.SelectByExternalId (OwEntities, userNameToMatch, pageIndex, pageSize);
                    // Set output
                   // totalRecords = PortalUserHelper.CountByExternalId (OwEntities, userNameToMatch);
                }
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Check portal users
           /* if ((PortalUsers != null) && (PortalUsers.Count > 0))
            {
                // Loop through portal users
                foreach (PUSER LoopPortalUser in PortalUsers)
                {
                    // Create membership user
                    MembershipUser = MyCreateMemberhipUserFromPortalUser (LoopPortalUser);
                    // Add to collection
                    MembershipUserCollection.Add (MembershipUser);
                }
            }*/

            // Return
            // NOTE BK, More then one return
            return MembershipUserCollection;
        }
       
        public override System.Web.Security.MembershipUserCollection GetAllUsers (int pageIndex, int pageSize, out int totalRecords)
        {
            System.Collections.Generic.List<PUSER> PortalUsers;
            System.Web.Security.MembershipUserCollection MembershipUserCollection;
            System.Web.Security.MembershipUser MembershipUser;

            // Set default output
            totalRecords = -1;
            // Create new collection
            MembershipUserCollection = new System.Web.Security.MembershipUserCollection ();

            // Check page index
            if (pageIndex < 0)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Check page index
            if (pageSize < 0)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Reset output
            totalRecords = 0;

            try
            {
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Select portal users
                    //PortalUsers = PortalUserHelper.SelectAll (OwEntities, pageIndex, pageSize);
                    // Set output
                    //totalRecords = PortalUserHelper.Count (OwEntities);
                }
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUserCollection;
            }

            // Check portal users
        /*    if ((PortalUsers != null) && (PortalUsers.Count > 0))
            {
                // Loop through portal users
                foreach (PUSER LoopPortalUser in PortalUsers)
                {
                    // Create membership user
                    MembershipUser = MyCreateMemberhipUserFromPortalUser (LoopPortalUser);
                    // Add to collection
                    MembershipUserCollection.Add (MembershipUser);
                }
            }*/

            // Return
            return MembershipUserCollection;
        }

        public override int GetHashCode ()
        {
            // Base
            return base.GetHashCode ();
        }

        // NOTE BK, No exceptions ! Returns an integer value that is the count of all the users in the data source where the LastActivityDate is greater than the current date and time minus the UserIsOnlineTimeWindow. The UserIsOnlineTimeWindow is a positive integer value specifying the number of minutes to use when determining whether a user is online.
        public override int GetNumberOfUsersOnline ()
        {
            // TODO MK 0 BK, Add functionality
            throw new Exception ();
        }

        // NOTE BK, For exceptions read this: GetPassword also checks the value of the RequiresQuestionAndAnswer property. If RequiresQuestionAndAnswer is true, GetPassword checks the value of the supplied answer parameter against the stored password answer in the data source. If they do not match, a MembershipPasswordException exception is thrown. 
        public override string GetPassword (string userName, string answer)
        {
            PUSER PortalUser;
            string Password = null;

            // Check state
            if (!_EnablePasswordRetrieval)
            {
                // Throw exception
                throw new Exception ();
            }

            // Check user name
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Password;
            }

            // Check state
            if (_RequiresQuestionAndAnswer)
            {
                // Check answer
                if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (answer))
                {
                    // Return (see NOTE)
                    // NOTE BK, More then one return
                    return Password;
                }
            }

            // Trim values
            userName = userName.Trim ();
            if (answer != null)
            {
                answer = answer.Trim ();
            }

            try
            {
                // Entities
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Find valid portal user
                    PortalUser = MyFindValidPortalUser (OwEntities, userName);
                    // Check portal user
                    if (PortalUser != null)
                    {
                        // Check state
                        if (_RequiresQuestionAndAnswer)
                        {
                            // TODO MK 0 BK, Add functionality
                            //// Check old password
                            //if (!PortalUser.PasswordAnswer.Equals(answer))
                            //{
                            //    // Throw exception
                            //    // TODO BK 0 BK, Change exception class
                            //    throw new System.Web.Security.MembershipPasswordException();
                            //}
                        }
                        // Get password
                        Password = PortalUser.KENNWORT;
                    }
                }
            }
            catch (System.Web.Security.MembershipPasswordException e)
            {
                throw e;
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Password;
            }

            // Return
            // NOTE BK, More then one return
            return Password;
        }

        // NOTE JJ, This method is added to the special reason, when the MasterPassword has to be replaced by original PUSER password.
        public string GetPassword (string userName)
        {
            string Password = null;

            if (userName == null)
            {
                throw new ArgumentException ("userName");
            }

            try
            {
                using (DdOwExtended Context = new DdOwExtended ())
                {
                    // Find PUSER
                    PUSER PUSER = MyFindValidPortalUser (Context, userName);

                    if (PUSER != null)
                    {
                        // Set password
                        Password = PUSER.KENNWORT;

                        // Encrypt
                        if (Password != null && _PasswordFormat == System.Web.Security.MembershipPasswordFormat.Encrypted)
                        {
                            // Encode password
                            Password = PUserUtil.EncryptPassword (Password);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return Password;
        }

        public new System.Type GetType ()
        {
            // Base
            return base.GetType ();
        }
        
        // NOTE BK, The provider user key is the external id (user name)
        // NOTE BK, No exceptions ! Return type: The GetUser method returns a MembershipUser object populated with current values from the data source for the specified user. If the user name is not found in the data source, GetUser returns nullNothingnullptra null reference (Nothing in Visual Basic).
        public override System.Web.Security.MembershipUser GetUser (object providerUserKey, bool userIsOnline)
        {
            PUSER PortalUser;
            System.Web.Security.MembershipUser MembershipUser = null;
            long Id = -1;

            // Check provider user key
            if (providerUserKey == null)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUser;
            }

            try
            {
                // Cast
                Id = (long) providerUserKey;
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUser;
            }

            // Check id
            if (Id == -1)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUser;
            }

            try
            {
                // Entities
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Find portal user
                    // NOTE BK, If no or more then one users found no exception is thrown
                  //  PortalUser = PortalUserHelper.SelectById (OwEntities, Id);
                    // Check portal user
                  /*  if (PortalUser != null)
                    {
                        // TODO MK 0 BK, Check user is online
                        // Create membership user
                        MembershipUser = MyCreateMemberhipUserFromPortalUser (PortalUser);
                    }*/
                }
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUser;
            }

            // Return
            // NOTE BK, More then one return
            return MembershipUser;
        }
        

        // NOTE BK, No exceptions ! Return type: GetUser returns a MembershipUser object populated with current values from the data source for the specified user. If the user name is not found in the data source, GetUser returns nullNothingnullptra null reference (Nothing in Visual Basic).
        public override System.Web.Security.MembershipUser GetUser (string userName, bool userIsOnline)
        {
            PUSER PortalUser;
            System.Web.Security.MembershipUser MembershipUser = null;

            // Check user name
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUser;
            }

            // Trim values
            userName = userName.Trim ();

            try
            {
                // Entities
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Find portal user
                    PortalUser = MyFindValidPortalUser (OwEntities, userName);
                    // Check portal user
                    if (PortalUser != null)
                    {
                        // TODO MK 0 BK, Check user is online
                        // Create membership user
                        MembershipUser = MyCreateMemberhipUserFromPortalUser (PortalUser);
                    }
                }
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return MembershipUser;
            }

            // Return
            // NOTE BK, More then one return
            return MembershipUser;
        }
        
        // NOTE BK, No exceptions ! Return type: The user name associated with the specified e-mail address. If no match is found, return nullNothingnullptra null reference (Nothing in Visual Basic). If multiple user names are found that match a particular e-mail address, only the first user name found should be returned. You can implement a custom mechanism for ensuring a unique e-mail address for each user name such as the RequiresUniqueEmail property supported by the SqlMembershipProvider provider.
        public override string GetUserNameByEmail (string email)
        {
            System.Collections.Generic.List<PUSER> PortalUsers;
            string UserName = null;

            // Check email
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (email))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return UserName;
            }

            // Trim value
            email = email.Trim ();

            try
            {
                // Entities
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Find portal users
                   // PortalUsers = PortalUserHelper.SelectByEmail (OwEntities, email, 0, 0);
                    // Check count of portal users
                   /* if (PortalUsers.Count > 0)
                    {
                        // Get first user name
                        UserName = PortalUsers[0].EXTERNEID;
                    }*/
                }
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return UserName;
            }

            // Return
            // NOTE BK, More then one return
            return UserName;
        }
        
        
        // NOTE BK, For exceptions read this: ResetPassword ensures that the EnablePasswordReset flag is set to true before performing any action. If EnablePasswordReset is false, a NotSupportedException exception is thrown. ResetPassword also checks the value of the RequiresQuestionAndAnswer property. If RequiresQuestionAndAnswer is true, ResetPassword checks the value of the supplied answer parameter against the stored password answer in the data source. If they do not match, a MembershipPasswordException exception is thrown. 
        public override string ResetPassword (string userName, string answer)
        {
            CIC.Database.OW.EF6.Model.PUSER PortalUser;
            string Password = null;
            int MinRequiredNonalphanumericCharacters;

            // Check state
            if (!_EnablePasswordReset)
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Password;
            }

            // Check user name
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Password;
            }

            // Check state
            if (_RequiresQuestionAndAnswer)
            {
                // Check answer
                if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (answer))
                {
                    // Return (see NOTE)
                    // NOTE BK, More then one return
                    return Password;
                }
            }

            // Trim values
            userName = userName.Trim ();
            if (answer != null)
            {
                answer = answer.Trim ();
            }

            try
            {
                // Entities
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    // Find valid portal user
                    PortalUser = MyFindValidPortalUser (OwEntities, userName, null);
                    // Check portal user
                    if (PortalUser != null)
                    {
                        // Check state
                        if (_RequiresQuestionAndAnswer)
                        {
                            // TODO MK 0 BK, Add functionality
                            //// Check old password
                            //if (!PortalUser.PasswordAnswer.Equals(answer))
                            //{
                            //    // Throw exception
                            //    // TODO BK 0 BK, Change exception class
                            //    throw new System.Web.Security.MembershipPasswordException();
                            //}
                        }

                        // Check value
                        if (_MinRequiredNonalphanumericCharacters > _MinRequiredPasswordLength)
                        {
                            // Set value
                            MinRequiredNonalphanumericCharacters = _MinRequiredPasswordLength;
                        }
                        else
                        {
                            // Set value
                            MinRequiredNonalphanumericCharacters = _MinRequiredNonalphanumericCharacters;
                        }

                        // NOTE BK, The random password created by the ResetPassword method is not guaranteed to pass the regular expression in the PasswordStrengthRegularExpression property. However, the random password will meet the criteria established by the MinRequiredPasswordLength and MinRequiredNonAlphanumericCharacters properties .

                        // Create password
                        Password = System.Web.Security.Membership.GeneratePassword (_MinRequiredPasswordLength, MinRequiredNonalphanumericCharacters);

                        // Change password
                        PortalUser.KENNWORT = PUserUtil.EncryptPassword (Password);
                        // Update
                       // PortalUserHelper.Update (OwEntities, PortalUser, null);
                    }
                }
            }
            catch (System.Web.Security.MembershipPasswordException e)
            {
                throw e;
            }
            catch
            {
                // Return (see NOTE)
                // NOTE BK, More then one return
                return Password;
            }

            // Return
            // NOTE BK, More then one return
            return Password;
        }
        
        public new string ToString ()
        {
            // Base
            return base.ToString ();
        }

        // NOTE BK, No exceptions ! Return type: true if the membership user was successfully unlocked; otherwise, false.
        public override bool UnlockUser (string userName)
        {
            // TODO MK 0 BK, Add functionality
            throw new Exception ();
        }
        
        public override void UpdateUser (System.Web.Security.MembershipUser user)
        {
            PUSER PortalUser = null;
            long Id = -1;
            string UserName;

            // Check object
            if ((user != null) && (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (user.UserName)))
            {
                try
                {
                    // Get provider user key
                    Id = (long) user.ProviderUserKey;
                }
                catch
                {
                    // Ignore exception
                }

                // Check id
                if (Id != -1)
                {
                    // Get user name
                    UserName = user.UserName.Trim ();

                    // Entities
                    using (DdOwExtended OwEntities = new DdOwExtended ())
                    {
                        // Find valid portal user
                       // PortalUser = PortalUserHelper.SelectById (OwEntities, Id);
                        // Check portal user
                        if (PortalUser != null)
                        {
                            // Mapping PortalUser <> MembershipUser
                            //-------------------------------------
                            // PortalUser.SysPUSER         <> MembershipUser.ProviderUserKey
                            // PortalUser.ExternalId       <> MembershipUser.Email
                            // PortalUser.Name             <> MembershipUser.Email
                            // PortalUser.FirstName           null
                            // PortalUser.Password            -
                            // PortalUser.Disabled         <> (MembershipUser.IsApproved ? 0 : 1)
                            // PortalUser.DisabledReason   <> (MembershipUser.isApproved ? null : "Is not approved.")
                            // PortalUser.Email            <> MembershipUser.Email
                            // PortalUser.PasswordQuestion <> MembershipUser.PasswordQuestion
                            // PortalUser.PasswordAnswer      -

                            // Set properties
                            // NOTE BK, Do not change password, password question or password answer
                            PortalUser.EXTERNEID = UserName;
                            PortalUser.NAME = UserName;
                            PortalUser.DISABLED = (user.IsApproved ? 0 : 1);
                            PortalUser.DISABLEDREASON = (user.IsApproved ? null : "Is not approved.");
                            // TODO MK 0 BK, Add functionality
                            //PortalUser.Email = user.Email;
                        }
                    }
                }
            }
        }
        
        // NOTE BK, No exceptions ! Return true if the specified username and password are valid; otherwise, false.
        public override bool ValidateUser (string userName, string password)
        {
            return (MyValidateUser (userName, password) != null);
        }

        // NOTE JJ, Return MembershipUserValidationInfo with statuses, PUSERDto, WFUSERDto and BRANDDto array.
        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ExtendedValidateDealer (string userName, string password)
        {
            // TODO JJ 9 JJ, Implement
            throw new System.NotImplementedException ();
        }

        // NOTE JJ, Return MembershipUserValidationInfo with statuses, PUSERDto, WFUSERDto and BRANDDto array.
        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ExtendedValidateUser (string userName, string password)
        {
            Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo MembershipUserValidationInfo = new Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ();

            // Validate portal and workflow user
            MyExtendedValidatePortalAndWorkflowUser (userName, password, MembershipUserValidationInfo);
            return ExtendedValidateUser (MembershipUserValidationInfo);

        }

        private static CacheDictionary<long, Cic.OpenLease.ServiceAccess.DdOl.BRANDDto[]> brandListCache = CacheFactory<long, Cic.OpenLease.ServiceAccess.DdOl.BRANDDto[]>.getInstance ().createCache (CacheDao.getInstance ().getCacheDuration (CacheCategory.Data), CacheCategory.Data);
        public static Cic.OpenLease.ServiceAccess.DdOl.BRANDDto[] listBrands (long syspuser)
        {
            if (!brandListCache.ContainsKey (syspuser))
            {
                //optimization to get valid roletypes and peroles
                using (DdOlExtended Context = new DdOlExtended())
                {
                    List<PeroleInfoDto> peroles = Context.ExecuteStoreQuery<PeroleInfoDto> ("SELECT roletype.typ,SysPerole FROM perole, roletype "
                        + " WHERE roletype.sysroletype = perole.sysroletype and "
                        // + " (validfrom is null or validfrom <= trunc(sysdate)) AND (validuntil is null or validuntil >= trunc(sysdate)) "
                        + SQL.CheckCurrentSysDate ("perole")
                        + "          AND sysperson IN (SELECT sysperson FROM person WHERE syspuser =" + syspuser + ")", null).ToList ();
                    // Check PEROLE
                    if (peroles.Count == 0)
                    {
                        return null;
                    }


                    List<long> prhgroupIds = new List<long> ();
                    foreach (PeroleInfoDto pinfo in peroles)
                    {



                        long vpSysPerole = PeroleHelper.FindRootPEROLEByRoleType(Context, pinfo.sysperole, PeroleHelper.CnstVPRoleTypeNumber);
                        prhgroupIds.AddRange (Context.ExecuteStoreQuery<long> ("select sysprhgroup from prhgroupm where "
                            // + " (validfrom is null or validfrom <= trunc(sysdate)) AND (validuntil is null or validuntil >= trunc(sysdate)) "
                            + SQL.CheckCurrentSysDate ("prhgroupm")
                            + " and activeflag=1 and sysperole=" + vpSysPerole, null).ToList ());
                    }

                    IEnumerable<long> prhgroupIdList = prhgroupIds.Distinct ();
                    if (prhgroupIdList.Count () == 0)
                    {

                        return null;

                    }
                    brandListCache[syspuser] = Context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.DdOl.BRANDDto> ("select brand.* from brand, prbrandm where brand.sysbrand=prbrandm.sysbrand and "
                        // + " (validfrom is null or validfrom <= trunc(sysdate)) AND (validuntil is null or validuntil >= trunc(sysdate)) "
                        + SQL.CheckCurrentSysDate ("prbrandm")
                        + " and prbrandm.activeflag=1 and sysprhgroup in (" + String.Join (",", prhgroupIdList.ToArray ()) + ")", null).ToArray ();

                }
            }
            return brandListCache[syspuser];
        }

        private static CacheDictionary<long, bool> imCache = CacheFactory<long, bool>.getInstance ().createCache (CacheDao.getInstance ().getCacheDuration (CacheCategory.Data), CacheCategory.Data);
        public static bool isInternalMitarbeiter (long syspuser)
        {
            if (!imCache.ContainsKey (syspuser))
            {
                //optimization to get valid roletypes and peroles
                using (DdOlExtended Context = new DdOlExtended())
                {
                    List<PeroleInfoDto> peroles = Context.ExecuteStoreQuery<PeroleInfoDto> ("SELECT roletype.typ,SysPerole FROM perole, roletype   WHERE roletype.sysroletype = perole.sysroletype and "
                        // + " (validfrom is null or validfrom <= trunc(sysdate)) AND (validuntil is null or validuntil >= trunc(sysdate))   "
                        + SQL.CheckCurrentSysDate ("perole")
                        + "      AND sysperson IN (SELECT sysperson FROM person WHERE syspuser =" + syspuser + ")", null).ToList ();
                    // Check PEROLE
                    if (peroles.Count == 0)
                    {
                        return false;
                    }

                    bool isIM = false;
                    List<long> prhgroupIds = new List<long> ();
                    foreach (PeroleInfoDto pinfo in peroles)
                    {
                        if (pinfo.TYP == Cic.OpenLease.Service.PeroleHelper.CnstIMRoleTypeNumber || pinfo.TYP == Cic.OpenLease.Service.PeroleHelper.CnstGebietsleiterTypeNumber || pinfo.TYP == Cic.OpenLease.Service.PeroleHelper.CnstBusinessLineRoleTypeNumber || pinfo.TYP == Cic.OpenLease.Service.PeroleHelper.CnstAussendienstTypeNumber)
                        {
                            isIM = true;
                            break;
                        }
                    }

                    imCache[syspuser] = isIM;

                }
            }
            return imCache[syspuser];
        }

        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ExtendedValidateUser (Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo MembershipUserValidationInfo)
        {
            using (DdOwExtended Context = new DdOwExtended ())
            {
                MembershipUserValidationInfo.systemlocked = RestOfTheHelpers.GetDisabled (Context);
                if (MembershipUserValidationInfo.systemlocked)
                {
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Systemlocked;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = RestOfTheHelpers.GetDisabledReason (Context);
                }
            }


            if (MembershipUserValidationInfo.MembershipUserValidationStatus != Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid)
            {
                // PUSER or WFUSER is not valid
                // NOTE JJ, More then one return
                return MembershipUserValidationInfo;
            }



            /*
            //optimization to get valid roletypes and peroles
            using (OlExtendedEntities Context = new OlExtendedEntities())
            {
                List<PeroleInfoDto> peroles = Context.ExecuteStoreQuery<PeroleInfoDto>("SELECT roletype.typ,SysPerole FROM perole, roletype           WHERE roletype.sysroletype = perole.sysroletype and " + 
                // " (validfrom is null or validfrom <= trunc(sysdate)) AND (validuntil is null or validuntil >= trunc(sysdate))    " + 
                + SQL.CheckCurrentDateValid ("perole")
                + "       AND sysperson IN (SELECT sysperson FROM person WHERE syspuser ="+MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value+")", null).ToList();
                // Check PEROLE
                if (peroles.Count == 0)
                {
                    // ValidRoleNotFound
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidRoleNotFound;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidRoleNotFound";
                    // PEROLE not found or not valid
                    // NOTE JJ, More then one return
                    return MembershipUserValidationInfo;
                }

                bool isIM = false;
                List<long> prhgroupIds = new List<long>();
                foreach (PeroleInfoDto pinfo in peroles)
                {

                    if (pinfo.TYP == Cic.OpenLease.Service.PeroleHelper.CnstIMRoleTypeNumber || pinfo.TYP == Cic.OpenLease.Service.PeroleHelper.CnstGebietsleiterTypeNumber || pinfo.TYP == Cic.OpenLease.Service.PeroleHelper.CnstBusinessLineRoleTypeNumber || pinfo.TYP == Cic.OpenLease.Service.PeroleHelper.CnstAussendienstTypeNumber)
                        isIM = true;

                    long vpSysPerole = PeroleHelper.FindRootPEROLEByRoleType(Context,pinfo.sysperole, PeroleHelper.CnstVPRoleTypeNumber);
                    prhgroupIds.Union(Context.ExecuteStoreQuery<long>("select sysprhgroup from prhgroupm where " + 
                        // " (validfrom is null or validfrom <= trunc(sysdate)) AND (validuntil is null or validuntil >= trunc(sysdate)) " + 
                        + SQL.CheckCurrentDateValid ("prhgroupm")
                        + " and activeflag=1 and sysperole="+vpSysPerole,null).ToList());
                }
                MembershipUserValidationInfo.IsInternalMitarbeiter = isIM;
                IEnumerable<long> prhgroupIdList =prhgroupIds.Distinct();
                if (prhgroupIdList.Count() == 0)
                {
                   
                        // ValidBrandNotFound
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidBrandNotFound;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidBrandNotFound";
                        // BRAND not found or not valid
                        // NOTE JJ, More then one return
                        return MembershipUserValidationInfo;
                   
                }
                MembershipUserValidationInfo.BRANDDto = Context.ExecuteStoreQuery<Cic.OpenLease.ServiceAccess.DdOl.BRANDDto>("select brand.* from brand,prbrandm where brand.sysbrand=prbrandm.sysbrand and "  
                // + " (validfrom is null or validfrom <= trunc(sysdate)) AND (validuntil is null or validuntil >= trunc(sysdate)) "
               + SQL.CheckCurrentDateValid ("prbrandm")
                + " and activeflag=1 and sysprhgroup in (" + String.Join(",", prhgroupIdList.ToArray()) + ")", null).ToArray();
             * //XXX TODO FILL VPEROLEDtoArray with perole and peroledtoarray and person
                MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid;
                MembershipUserValidationInfo.MembershipUserValidationStatusReason = "Valid";
            }*/




            _Log.Debug ("ExtendedValidateUser for SYSPUSER=" + MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);
            System.Collections.Generic.List<PERSON> PersonList = null;
            // Deliver person list with:
            // - 1 entry when TypPUser == 0
            // - Many entries when TypPUser == 1
            PersonList = MyDeliverPersonList (MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value);

            // Check person list
            if (PersonList == null || PersonList.Count == 0)
            {
                _Log.Debug ("No Person attached to PUSER!");
                // ValidPersonNotFound
                MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidPersonNotFound;
                MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidPersonNotFound";
                // PERSON not found or not valid
                // NOTE JJ, More then one return

                return MembershipUserValidationInfo;
            }

            using (DdOlExtended Context = new DdOlExtended ())
            {
                // Find all PEROLEs for each PERSON for PUSER and convert to the PEROLEDto list
                PEROLEAssembler PeRoleAssembler = new PEROLEAssembler ();
                PERSONAssembler PersonAssembler = new PERSONAssembler ();

                // Create PeRoleDtoList
                List<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto> PeRoleDtoList = new List<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto> ();
                bool IsIM = false;
                List<int> roletypes = new List<int> ();
                foreach (PERSON PersonLoop in PersonList)
                {
                    // Get PEROLES for each PERSON
                    List<PEROLE> PeRoleList = new List<PEROLE> ();
                    PeRoleList = PeroleHelper.DeliverValidPeRoles (Context, PersonLoop.SYSPERSON);
                    if (PeRoleList == null || PeRoleList.Count == 0)
                        _Log.Warn ("PUSER has no PEROLE");

                    foreach (PEROLE PeRoleLoop in PeRoleList)
                    {
                        if (MyCheckIsValid (PeRoleLoop.VALIDFROM, PeRoleLoop.VALIDUNTIL))
                        {
                            Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto TempPEROLEDto;

                            // Convert perole
                            TempPEROLEDto = PeRoleAssembler.ConvertToDto (PeRoleLoop);

                            // Add converted person
                            TempPEROLEDto.PERSONDto = PersonAssembler.ConvertToDto (PersonLoop);

                            // Add to list
                            PeRoleDtoList.Add (TempPEROLEDto);
                        }
                        if (PeRoleLoop.ROLETYPE == null)
                            Context.Entry(PeRoleLoop).Reference(f => f.ROLETYPE).Load();
                        
                        bool hasIMRole = false;
                        if (PeRoleLoop.ROLETYPE != null && PeRoleLoop.ROLETYPE.TYP != null && PeRoleLoop.ROLETYPE.TYP.HasValue)
                        {
                            int v = PeRoleLoop.ROLETYPE.TYP.Value;
                            roletypes.Add (v);
                            hasIMRole = (v == Cic.OpenLease.Service.PeroleHelper.CnstIMRoleTypeNumber || v == Cic.OpenLease.Service.PeroleHelper.CnstGebietsleiterTypeNumber || v == Cic.OpenLease.Service.PeroleHelper.CnstBusinessLineRoleTypeNumber || v == Cic.OpenLease.Service.PeroleHelper.CnstAussendienstTypeNumber);
                        }
                        IsIM = (IsIM || hasIMRole);

                    }

                }
                MembershipUserValidationInfo.roletypes = roletypes.ToArray ();
                MembershipUserValidationInfo.IsInternalMitarbeiter = IsIM;



                // Check PEROLE
                if (PeRoleDtoList.Count == 0)
                {
                    // ValidRoleNotFound
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidRoleNotFound;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidRoleNotFound";
                    // PEROLE not found or not valid
                    // NOTE JJ, More then one return
                    return MembershipUserValidationInfo;
                }

                // Temporary dictionary with BRAND and the list of PEROLEs
                Dictionary<BRAND, List<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto>> BrandPeroleTree;
                BrandPeroleTree = new Dictionary<BRAND, List<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto>> ();

                //Temporary dictionary with PEROLE and the list of VpPEROLE
                Dictionary<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto, PEROLE> PeroleVpPeroleDictionary;
                PeroleVpPeroleDictionary = new Dictionary<ServiceAccess.DdOl.PEROLEDto,PEROLE> ();

                // Loop through PeRole
                foreach (Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto PeRoleDtoLoop in PeRoleDtoList)
                {
                    PEROLE VpPeRole = null;

                    // Find the PeRole with VP typ in the perole tree
                    long rpr = PeroleHelper.FindRootPEROLEByRoleType(Context, (long)PeRoleDtoLoop.SYSPEROLE, PeroleHelper.CnstVPRoleTypeNumber);
                    //VpPeRole = Context.SelectById<PEROLE>(PeroleHelper.FindRootPEROLEByRoleType(Context, (long)PeRoleDtoLoop.SYSPEROLE,  PeroleHelper.CnstVPRoleTypeNumber));
                    VpPeRole = (from s in Context.PEROLE
                                where s.SYSPEROLE == rpr
                                select s).FirstOrDefault();
                    if (VpPeRole == null || VpPeRole.SYSPEROLE == 0)
                        _Log.Warn ("No PEROLE of Roletype.Typ=" + PeroleHelper.CnstVPRoleTypeNumber + " found for PEROLE of PUSER");

                    if (VpPeRole.ROLETYPE == null)
                        Context.Entry(VpPeRole).Reference(f => f.ROLETYPE).Load();
                    

                    //PEROLEHelper.FindVpOrRootPEROLE(Context, (long)PeRoleDtoLoop.SYSPEROLE);

                    // Get PRHGROUPList (Handelsgruppe) list
                    List<PRHGROUP> PRHGROUPList = new List<PRHGROUP> ();
                    //TODO WB 0 WB,  && VpPeRole.ROLETYPE.ENABLEPRHGROUP == 1 Add to condition
                    if (VpPeRole != null && VpPeRole.ROLETYPE.TYP == PeroleHelper.CnstVPRoleTypeNumber)
                    {
                        
                        if (!Context.Entry(VpPeRole).Collection(f => f.PRHGROUPMList).IsLoaded)
                            Context.Entry(VpPeRole).Collection(f => f.PRHGROUPMList).Load();
                        if (VpPeRole.PRHGROUPMList == null || VpPeRole.PRHGROUPMList.Count == 0)
                            _Log.Warn ("PRHGROUPM for PEROLE " + VpPeRole.SYSPEROLE + " was empty!");

                        if (VpPeRole.PRHGROUPMList != null)
                        {
                            foreach ( PRHGROUPM PrhgroupmLoop in VpPeRole.PRHGROUPMList)
                            {
                                // Check is PRHGROUPM active and valid
                                if (PrhgroupmLoop.ACTIVEFLAG.GetValueOrDefault (0) > 0 && MyCheckIsValid (PrhgroupmLoop.VALIDFROM, PrhgroupmLoop.VALIDUNTIL))
                                {
                                    if (PrhgroupmLoop.PRHGROUP == null)
                                        Context.Entry(PrhgroupmLoop).Reference(f => f.PRHGROUP).Load();
                                    

                                    if (PrhgroupmLoop.PRHGROUP != null && PrhgroupmLoop.PRHGROUP.ACTIVEFLAG.GetValueOrDefault (0) > 0 && MyCheckIsValid (PrhgroupmLoop.PRHGROUP.VALIDFROM, PrhgroupmLoop.PRHGROUP.VALIDUNTIL))
                                    {
                                        // Add PRHGROUP to the result list if exists, active and valid
                                        PRHGROUPList.Add (PrhgroupmLoop.PRHGROUP);
                                    }
                                }
                            }
                        }

                        //Add to dictionary pair of PeroleDto and VpPeRole
                        PeroleVpPeroleDictionary.Add (PeRoleDtoLoop, VpPeRole);
                    }

                    // Create BRAND list
                    List< BRAND> BRANDList = new List< BRAND> ();
                    if (PRHGROUPList.Count == 0)
                        _Log.Warn ("No valid PRHGROUP bound to PEROLE (via PRHGROUPM) " + VpPeRole.SYSPEROLE);

                    if (PRHGROUPList.Count > 0)
                    {
                        foreach (PRHGROUP PrhgroupLoop in PRHGROUPList)
                        {

                            if (!Context.Entry(PrhgroupLoop).Collection(f => f.PRBRANDMList).IsLoaded)
                                Context.Entry(PrhgroupLoop).Collection(f => f.PRBRANDMList).Load();
                            if (PrhgroupLoop.PRBRANDMList == null || PrhgroupLoop.PRBRANDMList.Count == 0)
                                _Log.Warn ("No BRAND linked to PRHGROUP (via PRBRANDM) " + PrhgroupLoop.SYSPRHGROUP);
                            if (PrhgroupLoop.PRBRANDMList != null)
                            {
                                foreach ( PRBRANDM PrbrandmLoop in PrhgroupLoop.PRBRANDMList)
                                {
                                    // Check is PRBRANDM active and valid
                                    if (PrbrandmLoop.ACTIVEFLAG.GetValueOrDefault (0) > 0 && MyCheckIsValid (PrbrandmLoop.VALIDFROM, PrbrandmLoop.VALIDUNTIL))
                                    {

                                        if (PrbrandmLoop.BRAND == null)
                                            Context.Entry(PrbrandmLoop).Reference(f => f.BRAND).Load();
                                        
                                        if (!BRANDList.Contains (PrbrandmLoop.BRAND))
                                        {
                                            // Add to the list
                                            BRANDList.Add (PrbrandmLoop.BRAND);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (BRANDList.Count == 0)
                    {
                        _Log.Warn ("No valid BRAND bound to PEROLE (via PRHGROUPM) " + VpPeRole.SYSPEROLE);
                    }
                    // Set BrandPeroleTree values
                    if (BRANDList.Count > 0)
                    {
                        foreach (BRAND BrandLoop in BRANDList)
                        {
                            // Check key
                            if (BrandPeroleTree.ContainsKey (BrandLoop))
                            {
                                // Add to the list
                                BrandPeroleTree[BrandLoop].Add (PeRoleDtoLoop);
                            }
                            else
                            {
                                // Create list with first value
                                BrandPeroleTree.Add (BrandLoop, new List<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto> () { PeRoleDtoLoop });
                            }
                        }
                    }
                }

                // Convert BrandPeroleTree to BrandDtoList
                List<Cic.OpenLease.ServiceAccess.DdOl.BRANDDto> BrandDtoList = new List<Cic.OpenLease.ServiceAccess.DdOl.BRANDDto> ();
                BRANDAssembler BrandAssembler = new BRANDAssembler ();
                if (BrandPeroleTree.Count > 0)
                {
                    foreach (KeyValuePair<BRAND, List<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto>> BrandPeroleLoop in BrandPeroleTree)
                    {
                        Dictionary<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto, List<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto>> VpWithPerolesDictionary = new Dictionary<ServiceAccess.DdOl.PEROLEDto, List<ServiceAccess.DdOl.PEROLEDto>> ();
                        Cic.OpenLease.ServiceAccess.DdOl.BRANDDto TempBRANDDto;

                        // Convert
                        TempBRANDDto = BrandAssembler.ConvertToDto (BrandPeroleLoop.Key);

                        PEROLEAssembler PEROLEAssembler = new PEROLEAssembler ();

                        foreach (KeyValuePair<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto, PEROLE> PeroleVpPeroleLoop in PeroleVpPeroleDictionary)
                        {
                            // Check if perole is on the brand list
                            if (BrandPeroleLoop.Value.Contains (PeroleVpPeroleLoop.Key))
                            {
                                // Get vp
                                Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto PEROLEDtoTemp = PEROLEAssembler.ConvertToDto (PeroleVpPeroleLoop.Value);

                                //Check if vp is in list vp's to perole
                                if (VpWithPerolesDictionary.Keys.Contains (PEROLEDtoTemp))
                                {
                                    //Add perole to vp
                                    VpWithPerolesDictionary[PEROLEDtoTemp].Add (PeroleVpPeroleLoop.Key);
                                }
                                else
                                {
                                    //Create list with peroles
                                    VpWithPerolesDictionary.Add (PEROLEDtoTemp, new List<Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto> ());
                                    VpWithPerolesDictionary[PEROLEDtoTemp].Add (PeroleVpPeroleLoop.Key);
                                }
                            }
                        }

                        //Convert list of vp to array and put to temp brand, 
                        TempBRANDDto.VpPEROLEDtoArray = VpWithPerolesDictionary.Keys.ToArray ();

                        //Add peroles to vp, every vp can have more peroles
                        foreach (Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto PeroleLoop in TempBRANDDto.VpPEROLEDtoArray)
                        {
                            PeroleLoop.PEROLEDDtoArray = VpWithPerolesDictionary[PeroleLoop].ToArray ();
                        }

                        // Add to list
                        BrandDtoList.Add (TempBRANDDto);
                    }
                }
                // Check BRAND
                if (BrandDtoList.Count == 0)
                {
                    // ValidBrandNotFound
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidBrandNotFound;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidBrandNotFound";
                    // BRAND not found or not valid
                    // NOTE JJ, More then one return
                    return MembershipUserValidationInfo;
                }


                // Copies to array
                MembershipUserValidationInfo.BRANDDto = BrandDtoList.ToArray ();

                // Valid
                MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid;
                MembershipUserValidationInfo.MembershipUserValidationStatusReason = "Valid";
                // Set default role if not exists or invalid
                bool IsDefaultPeroleValid = false;

                if (MembershipUserValidationInfo.PUSERDto.SYSDEFAULTPEROLE != null)
                {
                    foreach (Cic.OpenLease.ServiceAccess.DdOl.BRANDDto BrandDtoLoop in MembershipUserValidationInfo.BRANDDto)
                    {
                        foreach (Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto VpPeroleDtoLoop in BrandDtoLoop.VpPEROLEDtoArray)
                        {
                            foreach (Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto PeroleDtoLoop in VpPeroleDtoLoop.PEROLEDDtoArray)
                            {
                                if (MembershipUserValidationInfo.PUSERDto.SYSDEFAULTPEROLE == PeroleDtoLoop.SYSPEROLE)
                                {
                                    // The default perole is on the list
                                    IsDefaultPeroleValid = true;
                                    // Break from PEROLEDDtoArray loop
                                    break;
                                }
                            }

                            if (IsDefaultPeroleValid)
                            {
                                // Break from VpPEROLEDtoArray loop
                                break;
                            }
                        }

                        if (IsDefaultPeroleValid)
                        {
                            // Break from BRANDDto loop
                            break;
                        }
                    }
                }

                if (IsDefaultPeroleValid == false)
                {
                    // Set new default Perole - first entry
                    MembershipUserValidationInfo.PUSERDto.SYSDEFAULTPEROLE = MembershipUserValidationInfo.BRANDDto[0].VpPEROLEDtoArray[0].PEROLEDDtoArray[0].SYSPEROLE;
                }

                SetUserConfiguration (MembershipUserValidationInfo);

                // NOTE JJ, More then one return
                return MembershipUserValidationInfo;
            }


        }

        /// <summary>
        /// Assigns variables that will be used during the entire lifetime of the session in the GUI
        /// </summary>
        /// <param name="MembershipUserValidationInfo"></param>
        private static void SetUserConfiguration (Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo MembershipUserValidationInfo)
        {
            using (DdOlExtended Context = new DdOlExtended())
            {
                

                //MembershipUserValidationInfo.BMWICalcButton = CfgSingleton.GetEntry ("GENERAL", "BMWICALCBUTTON", "Vergleichsrechner", "AIDA");
                //MembershipUserValidationInfo.BMWICalcURL = CfgSingleton.GetEntry ("GENERAL", "BMWICALCURL", "http://www.bmw-i.at/de_at/bmw-i3/#bmw-i3-concept-vergleichsrechner", "AIDA");

                //der aktuelle händler wird über sysdefaultperole gesetzt, daher aus dieser perole den ort der person holen
                //PEROLE vpperole = Cic.OpenLease.Common.MembershipProvider.MyFindVpOrRootPEROLE (Context, MembershipUserValidationInfo.PUSERDto.SYSDEFAULTPEROLE.Value, PEROLEHelper.CnstVPRoleTypeNumber);
                //MembershipUserValidationInfo.MANDATORT = "";
                /*long? hdperson = 0;
                if (vpperole == null)
                {
                    hdperson = MembershipUserValidationInfo.BRANDDto[0].VpPEROLEDtoArray[0].SYSPERSON;
                }
                else
                {
                    hdperson = vpperole.SYSPERSON;
                }*/
                /*if (hdperson.HasValue)
                    MembershipUserValidationInfo.MANDATORT =  Context.ExecuteStoreQuery<String>("select person.ort from person where sysperson=" + vpperole.SYSPERSON, null).FirstOrDefault();*/

            }
        }

        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ExtendedValidateUser (string userName, string password, long sysBRAND, long sysPEROLE)
        {
            Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo MembershipUserValidationInfo = new Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ();
            CIC.Database.OW.EF6.Model.PUSER PUSER = null;
            PEROLE PEROLE = null;
            PERSON PERSON = null;
            PEROLE VpPEROLE = null;
            PERSON VpPERSON = null;
            BRAND BRAND = null;

            // Validate portal and workflow user
            double s = DateTime.Now.TimeOfDay.TotalMilliseconds;
            PUSER = MyExtendedValidatePortalAndWorkflowUser (userName, password, MembershipUserValidationInfo);
            double r1 = DateTime.Now.TimeOfDay.TotalMilliseconds - s;


            using (DdOwExtended Context = new DdOwExtended ())
            {
                MembershipUserValidationInfo.systemlocked = RestOfTheHelpers.GetDisabled (Context);
                if (MembershipUserValidationInfo.systemlocked)
                {
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Systemlocked;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = RestOfTheHelpers.GetDisabledReason (Context);
                }
            }


            if (MembershipUserValidationInfo.MembershipUserValidationStatus != Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid)
            {
                // PUSER or WFUSER is not valid
                // NOTE JJ, More then one return
                return MembershipUserValidationInfo;
            }

            double r2 = DateTime.Now.TimeOfDay.TotalMilliseconds - s;


            using (DdOlExtended Context = new DdOlExtended())
            {
                //Context.ContextOptions.UseLegacyPreserveChangesBehavior = 

                // Deliver PEROLE
                double s1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                PEROLE = (from c in Context.PEROLE
                          where c.SYSPEROLE == sysPEROLE
                          select c).FirstOrDefault();// PeroleHelper.DeliverPeRole (Context, sysPEROLE);
                double t1 = DateTime.Now.TimeOfDay.TotalMilliseconds - s1;
                //100

                s1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Check PEROLE
                if (PEROLE == null || !MyCheckIsValid (PEROLE.VALIDFROM, PEROLE.VALIDUNTIL))
                {
                    // ValidRoleNotFound
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidRoleNotFound;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidRoleNotFound";

                    // PEROLE not valid
                    // NOTE JJ, More then one return
                    return MembershipUserValidationInfo;
                }
                double t2 = DateTime.Now.TimeOfDay.TotalMilliseconds - s1;
                s1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                bool IsIM = false;
                if (PEROLE.ROLETYPE == null)
                    Context.Entry(PEROLE).Reference(f => f.ROLETYPE).Load();
                 
                IsIM = (IsIM || (PEROLE.ROLETYPE != null && PEROLE.ROLETYPE.TYP != null && PEROLE.ROLETYPE.TYP.Value == PeroleHelper.CnstIMRoleTypeNumber));
                MembershipUserValidationInfo.IsInternalMitarbeiter = IsIM;
                // Deliver PERSON




                PERSON = MyDeliverPerson (Context, sysPEROLE);
                //195
                double t3 = DateTime.Now.TimeOfDay.TotalMilliseconds - s1;
                s1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Check PERSON
                if (PERSON != null)
                {
                    // Deliver TypPUser
                    int TypPUser = RestOfTheHelpers.DeliverpTypPUser (Context);

                    if (TypPUser == 0)
                    {
                        // Check sysPERSON
                        if (PERSON.SYSPERSON != PUSER.SYSPERSON)
                        {
                            // ValidPersonNotFound
                            MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidPersonNotFound;
                            MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidPersonNotFound";

                            // PERSON not valid
                            // NOTE JJ, More then one return
                            return MembershipUserValidationInfo;
                        }
                    }
                    else if (TypPUser == 1)
                    {
                        // Check sysPUSER
                        if (PERSON.SYSPUSER != PUSER.SYSPUSER)
                        {
                            // ValidPersonNotFound
                            MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidPersonNotFound;
                            MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidPersonNotFound";

                            // PERSON not valid
                            // NOTE JJ, More then one return
                            return MembershipUserValidationInfo;
                        }
                    }
                    else
                    {
                        throw new Exception ("TypPUser");
                    }
                }
                else
                {
                    // ValidPersonNotFound
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidPersonNotFound;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidPersonNotFound";
                    // PERSON not valid
                    // NOTE JJ, More then one return
                    return MembershipUserValidationInfo;
                }
                double t4 = DateTime.Now.TimeOfDay.TotalMilliseconds - s1;
                // Check sysBRAND
                s1 = DateTime.Now.TimeOfDay.TotalMilliseconds;

                // Find the PeRole with VP typ in the perole tree
                //alt: VpPEROLE = PEROLEHelper.FindVpOrRootPEROLE(Context, sysPEROLE);

                VpPEROLE = MyFindVpOrRootPEROLE (Context, sysPEROLE, PeroleHelper.CnstVPRoleTypeNumber);
                double t8 = DateTime.Now.TimeOfDay.TotalMilliseconds - s1;
                s1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                if (VpPEROLE != null)
                {
                    //Get Person to VpPerole


                    VpPERSON = MyDeliverPerson (Context, VpPEROLE.SYSPEROLE);
                    //PEROLEHelper.DeliverPerson(Context, VpPEROLE.SYSPEROLE);
                    double t9 = DateTime.Now.TimeOfDay.TotalMilliseconds - s1;
                    s1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    if (!BRANDHelper.CheckPeroleInBrand (Context, VpPEROLE.SYSPEROLE, sysBRAND))
                    {
                        // ValidBrandNotFound
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidBrandNotFound;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidBrandNotFound";

                        // BRAND not valid
                        // NOTE JJ, More then one return
                        return MembershipUserValidationInfo;
                    }
                    double t10 = DateTime.Now.TimeOfDay.TotalMilliseconds - s1;
                }
                else
                {
                    // ValidBrandNotFound
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidBrandNotFound;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidBrandNotFound";

                    // BRAND not valid
                    // NOTE JJ, More then one return
                    return MembershipUserValidationInfo;
                }
                //820
                double t5 = DateTime.Now.TimeOfDay.TotalMilliseconds - s1;
                s1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Deliver BRAND
                BRAND = BRANDHelper.DeliverBRAND (Context, sysBRAND);

                double t6 = DateTime.Now.TimeOfDay.TotalMilliseconds - s1;
            }
            double r4 = DateTime.Now.TimeOfDay.TotalMilliseconds - s;
            // Otherwise, Valid
            MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid;
            MembershipUserValidationInfo.MembershipUserValidationStatusReason = "Valid";

            // Convert PERSON, PEROLE, BRAND
            BRANDAssembler BRANDAssembler = new BRANDAssembler ();
            PEROLEAssembler PEROLEAssembler = new PEROLEAssembler ();
            PERSONAssembler PERSONAssembler = new PERSONAssembler ();

            // Add to the validation info
            MembershipUserValidationInfo.BRANDDto = new Cic.OpenLease.ServiceAccess.DdOl.BRANDDto[1] { BRANDAssembler.ConvertToDto (BRAND) };
            MembershipUserValidationInfo.BRANDDto[0].VpPEROLEDtoArray = new Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto[1] { PEROLEAssembler.ConvertToDto (VpPEROLE) };
            if (VpPERSON != null)
            {
                MembershipUserValidationInfo.BRANDDto[0].VpPEROLEDtoArray[0].PERSONDto = PERSONAssembler.ConvertToDto (VpPERSON);
            }
            MembershipUserValidationInfo.BRANDDto[0].VpPEROLEDtoArray[0].PEROLEDDtoArray = new Cic.OpenLease.ServiceAccess.DdOl.PEROLEDto[1] { PEROLEAssembler.ConvertToDto (PEROLE) };
            MembershipUserValidationInfo.BRANDDto[0].VpPEROLEDtoArray[0].PEROLEDDtoArray[0].PERSONDto = PERSONAssembler.ConvertToDto (PERSON);
            double r6 = DateTime.Now.TimeOfDay.TotalMilliseconds - s;
            ////////////new

            // Check PUSER
            if (MembershipUserValidationInfo.PUSERDto != null)
            {
                if (MembershipUserValidationInfo.PUSERDto.SYSWFUSER != null)
                {
                    try
                    {
                        // Validate WFUSER
                        CIC.Database.OW.EF6.Model.WFUSER WFUSER = MyValidateSysWFUSER ((long) MembershipUserValidationInfo.PUSERDto.SYSWFUSER);

                        if (WFUSER != null)
                        {
                            WFUSERAssembler WFUSERAssembler = new WFUSERAssembler ();
                            MembershipUserValidationInfo.WFUSERDto = WFUSERAssembler.ConvertToDto (WFUSER);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }

                // Check WFUSER
                if (MembershipUserValidationInfo.WFUSERDto == null)
                {
                    // ValidWorkflowUserNotFound
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidWorkflowUserNotFound;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidWorkflowUserNotFound";
                }
                else
                {
                    // Valid
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "Valid";

                }
            }
            else
            {
                try
                {
                    if (!MyValidateUserName (userName))
                    {
                        // UserNameNotValid
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.UserNameNotValid;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ungültiger Benutzername";
                    }
                    else if (!MyValidateUserPassword (userName, password))
                    {
                        // PasswordNotValid
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.PasswordNotValid;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ungültiges Passwort";
                    }
                    else if (!MyValidateDisabled (userName))
                    {
                        // UserDisabled
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.UserDisabled;
                        if (PUSER.DISABLEDREASON != null)
                            MembershipUserValidationInfo.MembershipUserValidationStatusReason = PUSER.DISABLEDREASON;
                        else
                            MembershipUserValidationInfo.MembershipUserValidationStatusReason = "Beutzer gesperrt";
                    }
                    else if (!MyValidateUserGroup (userName, password))
                    {
                        // ValidRoleNotFound
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidRoleNotFound;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ungültige Rolle";
                    }
                    else
                    {
                        // Otherwise: NotValid
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.NotValid;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ungültiger Benutzer";
                    }
                }
                catch
                {
                    throw;
                }
            }



            //////////








            return MembershipUserValidationInfo;
        }


        public static PERSON MyDeliverPerson (DdOlExtended context, long sysPeRole)
        {
            //context.PERSON.MergeOption = System.Data.Objects.MergeOption.NoTracking;
            //context.PEROLE.MergeOption = System.Data.Objects.MergeOption.NoTracking;
            //test precompiled query!
            double s = DateTime.Now.TimeOfDay.TotalMilliseconds;

            /*

            var Query = from perole in context.PEROLE
                        join p in context.PERSON on perole.SYSPERSON equals p.SYSPERSON
                        where perole.SYSPEROLE == sysPeRole
                        select new Cic.OpenLease.ServiceAccess.DdOl.PERSONDto
                        {
                            SYSPERSON = p.SYSPERSON,
                            NAME = p.NAME
                        };
            Cic.OpenLease.ServiceAccess.DdOl.PERSONDto test = Query.FirstOrDefault();
            double r3 = DateTime.Now.TimeOfDay.TotalMilliseconds - s;
            
            System.Data.Common.DbParameter[] Parameters = 
                            { 
                            
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPeRole", Value = sysPeRole}
                            };
            string query1 = "select * from perole, person where perole.sysperole=:sysPeRole and perole.sysperson=person.sysperson";


            
            PEROLE rval2 = context.ExecuteStoreQuery<PEROLE>(query1, Parameters).FirstOrDefault<PEROLE>();
            double r6 = DateTime.Now.TimeOfDay.TotalMilliseconds - s;
             * */
            s = DateTime.Now.TimeOfDay.TotalMilliseconds;
            PERSON rval;
            try
            {
                System.Data.Common.DbParameter[] Parameters2 = 
                            { 
                            
                                new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPeRole", Value = sysPeRole}
                            };
                string query = "select person.* from perole, person where perole.sysperole=:sysPeRole and perole.sysperson=person.sysperson";
                s = DateTime.Now.TimeOfDay.TotalMilliseconds;
                rval = context.ExecuteStoreQuery<PERSON> (query, Parameters2).FirstOrDefault<PERSON> ();

            }
            catch (Exception e)
            {
                throw e;

            }
            double r7 = DateTime.Now.TimeOfDay.TotalMilliseconds - s;
            return rval;
        }

        /// <summary>
        /// Return the given roletype typ in HD/MAGRP, when not found look for roletype.typ=3 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="pRoleType"></param>
        /// <returns></returns>
        public static PEROLE MyFindVpOrRootPEROLE (DdOlExtended context, long sysPEROLE, long pRoleType)
        {

            string query = "select perole.* from perole,roletype where roletype.typ=:pRoleType and "
                 + SQL.CheckCurrentSysDate ("perole")
                 + " and roletype.code in ('HD','MAGRP') and perole.sysroletype=roletype.sysroletype connect by PRIOR sysparent = sysperole start with sysperole=:sysPerole";

            System.Data.Common.DbParameter[] Parameters = 
                        { 
                            
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPerole", Value = sysPEROLE},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "pRoleType", Value = pRoleType}
                        };


            PEROLE rval = context.ExecuteStoreQuery<PEROLE> (query, Parameters).FirstOrDefault<PEROLE> ();

            if(rval==null)
            {
                query = "select perole.* from perole,roletype where roletype.typ=14 and "
                    + SQL.CheckCurrentSysDate("perole")
                    + " and perole.sysroletype=roletype.sysroletype connect by PRIOR sysparent = sysperole start with sysperole=:sysPerole";
                System.Data.Common.DbParameter[] Parameters2 = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPerole", Value = sysPEROLE}
                        };


                rval = context.ExecuteStoreQuery<PEROLE>(query, Parameters2).FirstOrDefault<PEROLE>();


            }

            return rval;
        }
        #endregion


        #region Additional methods
        // NOTE: It is OK to throw exception from here
        public bool ValidateUserName (string userName)
        {
            try
            {
                // Private
                return MyValidateUserName (userName);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // NOTE: It is OK to throw exception from here
        public bool ValidateUserPassword (string userName, string password)
        {
            try
            {
                // Private
                return MyValidateUserPassword (userName, password);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // NOTE: It is OK to throw exception from here
        public bool ValidateUserGroup (string userName, string password)
        {
            try
            {
                // Private
                return MyValidateUserGroup (userName, password);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        // NOTE: It is OK to throw exception from here
        public bool ValidateDisabled (string userName)
        {
            try
            {
                // Private
                return MyValidateDisabled (userName);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        #endregion

        #region Event methods
        // TEST MK 0 BK, Not tested
        protected override void OnValidatingPassword (System.Web.Security.ValidatePasswordEventArgs e)
        {
            System.Text.RegularExpressions.Regex Regex;
            int CountOfAlphanumericChars;
            int MinRequiredNonalphanumericCharacters;

            // Set cancel
            e.Cancel = false;

            // Base
            base.OnValidatingPassword (e);

            // Check state
            if ((e.Cancel == false) && (_MinRequiredPasswordLength > 0))
            {
                // Validate password length
                e.Cancel = (e.Password.Length < _MinRequiredPasswordLength);
            }

            // Check state
            if ((e.Cancel == false) && (_MinRequiredNonalphanumericCharacters > 0))
            {
                // Check value
                if (_MinRequiredNonalphanumericCharacters > _MinRequiredPasswordLength)
                {
                    // Set value
                    MinRequiredNonalphanumericCharacters = _MinRequiredPasswordLength;
                }
                else
                {
                    // Set value
                    MinRequiredNonalphanumericCharacters = _MinRequiredNonalphanumericCharacters;
                }

                // Get count value
                CountOfAlphanumericChars = Cic.OpenOne.Common.Util.StringUtil.CountAlphanumericChars (e.Password);
                // Validate non alphanumeric characters
                e.Cancel = (CountOfAlphanumericChars < MinRequiredNonalphanumericCharacters);
            }

            // Check state
            if ((e.Cancel == false) && (!string.IsNullOrEmpty (_PasswordStrengthRegularExpression)))
            {
                // New regex
                Regex = new System.Text.RegularExpressions.Regex (_PasswordStrengthRegularExpression);
                // Validate regular expression
                e.Cancel = (!Regex.IsMatch (e.Password));
            }
        }
        #endregion

        #region Properties
        public override bool EnablePasswordReset
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _EnablePasswordReset;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _EnablePasswordRetrieval;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _RequiresQuestionAndAnswer;
            }
        }

        public override string ApplicationName
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _ApplicationName;
            }
            set
            {
                // Do nothing
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _MaxInvalidPasswordAttempts;
            }
        }

        public override int PasswordAttemptWindow
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _PasswordAttemptWindow;
            }
        }

        public override bool RequiresUniqueEmail
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _RequiresUniqueEmail;
            }
        }

        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _PasswordFormat;
            }
        }

        public override int MinRequiredPasswordLength
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _MinRequiredPasswordLength;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _MinRequiredNonalphanumericCharacters;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            // TEST MK 0 BK, Not tested
            get
            {
                return _PasswordStrengthRegularExpression;
            }
        }
        #endregion

        #region My methods
        private CIC.Database.OW.EF6.Model.PUSER MyExtendedValidatePortalAndWorkflowUser (string userName, string password, Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo MembershipUserValidationInfo)
        {
            // Validate PUSER
            CIC.Database.OW.EF6.Model.PUSER PUSER = MyValidateUser (userName, password);

            if (PUSER != null)
            {
                PUSERAssembler PUSERAssembler = new PUSERAssembler ();
                MembershipUserValidationInfo.PUSERDto = PUSERAssembler.ConvertToDto (PUSER);
            }

            // Check PUSER
            if (MembershipUserValidationInfo.PUSERDto != null && PUSER.DISABLED.GetValueOrDefault()==0)
            {
                if (MembershipUserValidationInfo.PUSERDto.SYSWFUSER != null)
                {
                    try
                    {
                        // Validate WFUSER
                        CIC.Database.OW.EF6.Model.WFUSER WFUSER = MyValidateSysWFUSER ((long) MembershipUserValidationInfo.PUSERDto.SYSWFUSER);

                        if (WFUSER != null)
                        {
                            WFUSERAssembler WFUSERAssembler = new WFUSERAssembler ();
                            MembershipUserValidationInfo.WFUSERDto = WFUSERAssembler.ConvertToDto (WFUSER);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }

                // Check WFUSER
                if (MembershipUserValidationInfo.WFUSERDto == null)
                {
                    // ValidWorkflowUserNotFound
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidWorkflowUserNotFound;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ValidWorkflowUserNotFound";
                }
                else
                {
                    // Valid
                    MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid;
                    MembershipUserValidationInfo.MembershipUserValidationStatusReason = "Valid";

                }
            }
            else
            {
                try
                {
                    if (!MyValidateUserName (userName))
                    {
                        // UserNameNotValid
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.UserNameNotValid;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ungültiger Benutzername";
                    }
                    else if (!MyValidateUserPassword (userName, password))
                    {
                        // PasswordNotValid
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.PasswordNotValid;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ungültiges Passwort";
                    }
                    else if (!MyValidateDisabled (userName))
                    {
                        // UserDisabled
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.UserDisabled;
                        if (PUSER != null && PUSER.DISABLEDREASON != null)
                            MembershipUserValidationInfo.MembershipUserValidationStatusReason = PUSER.DISABLEDREASON;
                        else
                            MembershipUserValidationInfo.MembershipUserValidationStatusReason = "Beutzer gesperrt";
                    }
                    else if (!MyValidateUserGroup (userName, password))
                    {
                        // ValidRoleNotFound
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidRoleNotFound;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ungültige Rolle";
                    }
                    else
                    {
                        // Otherwise: NotValid
                        MembershipUserValidationInfo.MembershipUserValidationStatus = Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.NotValid;
                        MembershipUserValidationInfo.MembershipUserValidationStatusReason = "ungültiger Benutzer";
                    }
                }
                catch
                {
                    throw;
                }
            }

            return PUSER;
        }

        private System.Web.Security.MembershipUser MyCreateMemberhipUserFromPortalUser (PUSER portalUser)
        {
            // Mapping MembershipUser <> PortalUser
            //-------------------------------------
            // MembershipUser.ProviderName               this.Name
            // MembershipUser.ProviderUserKey         <> PortalUser.SysPUSER
            // MembershipUser.UserName                <> PortalUser.ExterneID
            // MembershipUser.Email                   <> PortalUser.Email
            // MembershipUser.PasswordQuestion        <> PortalUser.PasswordQuestion
            // MembershipUser.Comment                    null
            // MembershipUser.IsApproved              <> !(PortalUser.Disabled == 1)
            // MembershipUser.IsLockedOut                true
            // MembershipUser.CreationDate               System.DateTime.MinValue
            // MembershipUser.LastLoginDate              System.DateTime.MinValue
            // MembershipUser.LastActivityDate           System.DateTime.MinValue
            // MembershipUser.LastPasswordChangedDate    System.DateTime.MinValue
            // MembershipUser.LastLockoutDate            System.DateTime.MinValue

            // TODO MK 0 BK, Add email and password question
            // NOTE BK, The provider user key is external id

            return new System.Web.Security.MembershipUser (this.Name, portalUser.EXTERNEID, portalUser.SYSPUSER, null, null, null, !(portalUser.DISABLED == 1), true, System.DateTime.MinValue, System.DateTime.MinValue, System.DateTime.MinValue, System.DateTime.MinValue, System.DateTime.MinValue);
        }

        // NOTE BK, context must be not null, user names and passwords are not null and not empty, they must be trimed
        private PUSER MyFindValidPortalUser (DdOwExtended owEntities, string userName)
        {
            PUSER PortalUser;

            try
            {
                // Find portal user
                // NOTE BK, If no or more then one users found no exception is thrown
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "extid", Value = userName });
                return owEntities.ExecuteStoreQuery<PUSER>("select * from puser where externeid=:extid", parameters.ToArray()).FirstOrDefault();
                

                
            }

            catch (System.Exception e)
            {
                // Throw new exception
                throw e;
            }

            
        }

        // NOTE BK, context must be not null, user names and passwords are not null and not empty, they must be trimed
        private CIC.Database.OW.EF6.Model.PUSER MyFindValidPortalUser (DdOwExtended owEntities, string userName, string password)
        {
            CIC.Database.OW.EF6.Model.PUSER PortalUser;
            string RealPassword = string.Empty;

            try
            {
                // Find portal user
                // Presume password is not encoded
                RealPassword = password;

                // Check password
                if (password != null && _PasswordFormat == System.Web.Security.MembershipPasswordFormat.Encrypted)
                {
                    // Encode old password
                    RealPassword = PUserUtil.EncryptPassword (password);
                }
                // && (p.DISABLED == 0 || p.DISABLED == null)
                // NOTE BK, If no or more then one users found no exception is thrown
                var query = from p in owEntities.PUSER
                            where p.EXTERNEID.ToUpper ().Equals (userName.ToUpper ()) //&& p.KENNWORT.Equals(RealPassword)
                            select p;
                PortalUser = query.FirstOrDefault ();

                /*PortalUser = PortalUserHelper.SelectSingleByExternalId(owEntities, userName);

                // Check current user password
                if (PortalUser != null)
                {
                    // Presume password is not encoded
                    RealPassword = password;

                    // Check password
                    if (password != null && _PasswordFormat == System.Web.Security.MembershipPasswordFormat.Encrypted)
                    {
                        // Encode old password
                        RealPassword = PortalUserHelper.EncryptPassword(password);
                    }

                    // Check password
                    if (PortalUser.KENNWORT == null || !PortalUser.KENNWORT.Equals(RealPassword))
                    {
                        // User not autenticated
                        PortalUser = null;
                    }

                    // Check disabled
                    if (PortalUser != null && PortalUser.DISABLED.HasValue && PortalUser.DISABLED != 0)
                    {
                        PortalUser = null;
                    }
                }*/
            }

            catch (System.Exception e)
            {
                // Throw new exception
                throw e;
            }

            // Return
            return PortalUser;
        }

        // NOTE BK, No exceptions ! Return type: portal user if the specified username and password are valid; otherwise, null.
        private CIC.Database.OW.EF6.Model.PUSER MyValidateUser (string userName, string password)
        {
            CIC.Database.OW.EF6.Model.PUSER PortalUser = null;

            // Check user name
            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                // Check password
                // if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(password))
                {
                    // Trim values
                    userName = userName.Trim ();
                    // password = password.Trim();

                    try
                    {
                        // Entities
                        using (DdOwExtended OwEntities = new DdOwExtended ())
                        {
                            // Find valid portal user
                            PortalUser = MyFindValidPortalUser (OwEntities, userName, password);
                        }
                    }
                    catch
                    {
                        // Ignore exception
                    }
                }
            }

            // Return
            return PortalUser;
        }

        // NOTE: It is OK to throw exception from here
        private bool MyValidateUserName (string userName)
        {
            PUSER PortalUser;
            bool Success = false;

            if (!Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                try
                {
                    using (DdOwExtended OwEntities = new DdOwExtended ())
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "extid", Value = userName });
                        int cnt = OwEntities.ExecuteStoreQuery<int>("select count(*) from puser where externeid=:extid", parameters.ToArray()).FirstOrDefault();
                        return cnt > 0;

                    }
                }
                catch
                {
                    throw;
                }
            }

            return Success;
        }

        // NOTE: It is OK to throw exception from here
        private bool MyValidateUserPassword (string userName, string password)
        {
            
            

            bool Success = false;

            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                return Success;
            }
            //HCE always true, we have a master password
            return Success;
            /*

            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(password))
            {
                return Success;
            }

            try
            {
                using (DdOwExtended OwEntities = new DdOwExtended())
                {
                    PortalUser = PortalUserHelper.SelectSingleByExternalId(OwEntities, userName);

                    // Check current user password
                    if (PortalUser != null)
                    {
                        // Presume password is not encoded
                        RealPassword = password;

                        // Check password
                        if (password != null && _PasswordFormat == System.Web.Security.MembershipPasswordFormat.Encrypted)
                        {
                            // Encode old password
                            RealPassword = PortalUserHelper.EncryptPassword(password);
                        }

                        // Check password
                        if (PortalUser.KENNWORT != null && PortalUser.KENNWORT.Equals(RealPassword))
                        {
                            Success = true;
                        }
                    }
                    else
                    {
                        Success = false;
                        //throw new MembershipProviderException(1, "User not found");
                    }
                }
            }
            catch
            {
                throw;
            }

            return Success;*/
        }

        /// <summary>
        /// return true when user is not disabled
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool MyValidateDisabled (string userName)
        {
            PUSER PortalUser;

            bool Success = false;

            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty (userName))
            {
                return Success;
            }

            try
            {
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "extid", Value = userName });
                    int cnt = OwEntities.ExecuteStoreQuery<int>("select count(*) from puser where (disabled is null or disabled=0) and externeid=:extid",parameters.ToArray()).FirstOrDefault();
                    return cnt > 0;
                    
                }
            }
            catch
            {
                throw;
            }

         
        }
        
        // NOTE: It is OK to throw exception from here
        private bool MyValidateUserGroup (string userName, string password)
        {
            return true;
        }

        // NOTE: It is OK to throw exception from here
        private CIC.Database.OW.EF6.Model.WFUSER MyValidateSysWFUSER (long sysWFUSER)
        {
            CIC.Database.OW.EF6.Model.WFUSER WorkflowUser = null;

            try
            {
                using (DdOwExtended OwEntities = new DdOwExtended ())
                {
                    WorkflowUser = (from o in OwEntities.WFUSER
                                    where o.SYSWFUSER==sysWFUSER
                                    select o).FirstOrDefault();// WorkflowUserHelper.SelectBySysWFUSER (OwEntities, sysWFUSER);
                }
            }
            catch
            {
                throw;
            }

            return WorkflowUser;
        }

        // NOTE: It is OK to throw exception from here
        private PERSON MyValidateSysPERSON (long sysPERSON)
        {
            PERSON Person = null;

            try
            {
                using (DdOlExtended OlEntities = new DdOlExtended ())
                {
                    Person = PERSONHelper.SelectBySysPERSONWithoutException (OlEntities, sysPERSON);
                }
            }
            catch
            {
                throw;
            }

            return Person;
        }

        private List<PERSON> MyDeliverPersonList (long sysPuser)
        {
            CIC.Database.OW.EF6.Model.PUSER PUSER = null;
            int TypPUser;
            List<PERSON> PersonList;

            try
            {
                using (DdOwExtended Context = new DdOwExtended ())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "extid", Value = sysPuser });
                    PUSER= Context.ExecuteStoreQuery<CIC.Database.OW.EF6.Model.PUSER>("select * from puser where syspuser=:extid", parameters.ToArray()).FirstOrDefault();


                }
            }
            catch
            {
                // Ignore exception
            }

            // Check PUSER
            if (PUSER == null)
            {
                throw new ArgumentException ("PUSER");
            }

            try
            {
                using (DdOlExtended Context = new DdOlExtended())
                {
                    // Deliver TypPUser
                    TypPUser = RestOfTheHelpers.DeliverpTypPUser (Context);

                    if (TypPUser == 0)
                    {
                        // Create list with one entry
                        PersonList = new List<PERSON> () { PERSONHelper.SelectBySysPERSONWithoutException (Context, PUSER.SYSPERSON.Value) };
                    }
                    else if (TypPUser == 1)
                    {
                        // Deliver list
                        PersonList = Context.ExecuteStoreQuery<PERSON> ("select * from person where syspuser=" + sysPuser).ToList ();
                        //PersonList = PersonHelper.SelectBySysPUSER(Context, sysPuser);
                    }
                    else
                    {
                        throw new Exception ("TypPUser");
                    }
                }
            }
            catch
            {
                throw;
            }

            return PersonList;
        }

        private bool MyCheckIsValid(System.DateTime? validFrom, System.DateTime? validUntil)
        {
            // Optimistic
            bool IsValid = true;

            DateTime? validFromTemp = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(validFrom);
            DateTime? validUntilTemp = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionDateToDtoDate(validUntil);

            // Check Valid from
            if (validFrom.HasValue && validFromTemp != null)
            {
                IsValid = IsValid && validFrom.Value.Date <= System.DateTime.Now.Date;
            }

            // Check Valid until
            if (validUntil.HasValue && validUntilTemp != null)
            {
                IsValid = IsValid && validUntil.Value.Date >= System.DateTime.Now.Date;
            }
            return IsValid;
        }
        #endregion
    }
    class PeroleInfoDto
    {
        public int TYP { get; set; }
        public long sysperole { get; set; }
    }
}
