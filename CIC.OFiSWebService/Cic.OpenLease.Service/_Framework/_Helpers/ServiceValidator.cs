// OWNER MK, 07-07-2009
using Cic.OpenOne.Common.Util.Collection;
using System;
using Cic.OpenLease.ServiceAccess.Merge.MembershipProvider;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;
using Cic.OpenOne.Common.Util.Config;

namespace Cic.OpenLease.Service
{
    // [System.CLSCompliant(true)]
    internal class ServiceValidator
    {
        #region Private constants
        private string CnstMasterPassword = "XAKLOP901ASDDDA";
        #endregion

        private static CacheDictionary<String, MembershipUserValidationInfo> credentialCache = CacheFactory<String, MembershipUserValidationInfo>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        #region Constructors
        internal ServiceValidator(string serviceName, string methodName) :
            this(serviceName, methodName, true)
        {
        }

        internal ServiceValidator(string serviceName, string methodName, bool validateInMembershipProvider)
        {
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedEmpty(serviceName))
            {
                throw new Exception("serviceName");
            }

            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedEmpty(methodName))
            {
                throw new Exception("methodName");
            }

            // Set properties
            this.ServiceName = serviceName;
            this.MethodName = methodName;

            // schould validate in Membership Provider?
            if (validateInMembershipProvider)
            {
                // Validate user
                this.MembershipUserValidationInfo = MyValidate();

                try
                {
                    //using (Cic.OpenLease.Model.DdOw.OwExtendedEntities context = new Cic.OpenLease.Model.DdOw.OwExtendedEntities())
                    {
                        // TODO: Get SYSRFN for a gien serviceName and methodName 
                        //this.RFG = Cic.OpenLease.Model.DdOw.RFGHelper.DeliverRFG(context, MembershipUserValidationInfo.WFUSERDto.SYSWFUSER.GetValueOrDefault(0), -1);

                        //Dummy
                        this.RFG = new CIC.Database.OW.EF6.Model.RFG();
                        this.RFG.AUSFUEHREN = 1;
                        this.RFG.BEARBEITEN = 1;
                        this.RFG.BEIAUFRUF = 1;
                        this.RFG.EINFUEGEN = 1;
                        this.RFG.LOESCHEN = 1;
                        this.RFG.SEHEN = 1;
                    }
                }
                catch (System.Exception Exception)
                {
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralGeneric, Exception.Message);
                }
            }
            else
            {
                MyReadHeader();
                this.RFG = new CIC.Database.OW.EF6.Model.RFG();
                this.RFG.AUSFUEHREN = 1;
                this.RFG.BEARBEITEN = 1;
                this.RFG.BEIAUFRUF = 1;
                this.RFG.EINFUEGEN = 1;
                this.RFG.LOESCHEN = 1;
                this.RFG.SEHEN = 1;
            }

            // Validate service
            // TODO MK 0 MK, Dummy


        }

        #endregion

        #region Methods
        public void ValidateExecute()
        {
            if (!(this.RFG.AUSFUEHREN.GetValueOrDefault()>0))
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityExecuteNotAllowed);
            }
        }

        public void ValidateView()
        {
            if (!(this.RFG.SEHEN.GetValueOrDefault() > 0))
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityViewNotAllowed);
            }
        }

        public void ValidateDelete()
        {
            if(!(this.RFG.LOESCHEN.GetValueOrDefault() > 0))
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityDeleteNotAllowed);
            }
        }

        public void ValidateEdit()
        {
            if (!(this.RFG.BEARBEITEN.GetValueOrDefault() > 0))
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityEditNotAllowed);
            }
        }

        public void ValidateInsert()
        {
            if (!(this.RFG.EINFUEGEN.GetValueOrDefault() > 0))
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityInsertNotAllowed);
            }
        }
        #endregion

        #region Properties
        public string ServiceName
        {
            get;
            private set;
        }

        public string MethodName
        {
            get;
            private set;
        }

        public long SYSPUSER
        {
            get;
            private set;
        }

        public long SYSWFUSER
        {
            get;
            private set;
        }

        public long SysPEROLE
        {
            get;
            private set;
        }

        public long SysPERSON
        {
            get;
            private set;
        }

        public long VpSysPEROLE
        {
            get;
            private set;
        }

        public long? VpSysPERSON
        {
            get;
            private set;
        }

        public long SysBRAND
        {
            get;
            private set;
        }

        public string ISOLanguageCode
        {
            get;
            private set;
        }

        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo MembershipUserValidationInfo
        {
            get;
            private set;
        }

        public CIC.Database.OW.EF6.Model.RFG RFG
        {
            get;
            private set;
        }
        #endregion

        #region My methods
        private void MyReadHeader()
        {
            Cic.OpenLease.ServiceAccess.MessageHeader MessageHeader;
            // Read header
            MessageHeader = Cic.OpenLease.ServiceAccess.MessageHeaderHelper.ReadHeader();

            if (MessageHeader == null)
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityNoMessageHeader, Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityNoMessageHeader.ToString());
            }

            // TODO: Validate ISO Language Code
            this.ISOLanguageCode = MessageHeader.ISOLanguageCode;
        }

        /// <summary>
        /// Removes user-data from cache
        /// </summary>
        public void invalidateUser()
        {
             // Read header
            Cic.OpenLease.ServiceAccess.MessageHeader MessageHeader = Cic.OpenLease.ServiceAccess.MessageHeaderHelper.ReadHeader();

            if (MessageHeader == null)
            {
                return;
            }

            // Validate
            string cacheKey = string.Join("_", new string[] { MessageHeader.UserName, MessageHeader.Password, MessageHeader.SysBRAND.ToString(), MessageHeader.SysPEROLE.ToString(), MessageHeader.ISOLanguageCode });

            credentialCache.RemoveSafe(cacheKey);

        }

        public String getLoginKey()
        {
            // Read header
            Cic.OpenLease.ServiceAccess.MessageHeader MessageHeader = Cic.OpenLease.ServiceAccess.MessageHeaderHelper.ReadHeader();

            if (MessageHeader == null)
            {
                return "";
            }

            // Validate
            string cacheKey = string.Join("_", new string[] { MessageHeader.UserName, MessageHeader.Password });

            return cacheKey;
        }

        // Throw MembershipExceptions
        private Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo MyValidate()
        {
            Cic.OpenLease.ServiceAccess.MessageHeader MessageHeader;
            Cic.OpenLease.Common.MembershipProvider Provider;
            string RealPassword = null;

            // Read header
            MessageHeader = Cic.OpenLease.ServiceAccess.MessageHeaderHelper.ReadHeader();

            if (MessageHeader == null)
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityNoMessageHeader, Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityNoMessageHeader.ToString());
            }

           


           

            // Validate
            string cacheKey = string.Join("_", new string[] { MessageHeader.UserName, MessageHeader.Password, MessageHeader.SysBRAND.ToString(), MessageHeader.SysPEROLE.ToString(), MessageHeader.ISOLanguageCode });

            if (credentialCache.ContainsKey(cacheKey))
            {
                MembershipUserValidationInfo = credentialCache[cacheKey];
            }
            else
            {
                // Get membership provider
                Provider = MembershipProviderHelper.DeliverProvider();

                // Check provider
                if (Provider == null)
                {
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralMembershipProvider);
                }

                try
                {
                    bool IsMasterPassword = false;

                    // Check is Master password
                    string MasterPassword;

                    try
                    {
                        
                        MasterPassword = AppConfig.getValueFromDb("AIDA", "WEBSERVICES", "GENERALPASSWORD", CnstMasterPassword);
                        //Get Password from Configuration
                        
                    }
                    catch
                    {
                        MasterPassword = CnstMasterPassword;
                    }

                    //Check if password is correct
                    IsMasterPassword = MessageHeader.Password == MasterPassword;

                    if (IsMasterPassword)
                    {
                        // NOTE JJ, If the Master password is valid the RealPassword is replaced by PUSER password
                        RealPassword = Provider.GetPassword(MessageHeader.UserName);
                    }
                    else
                    {
                        // Set real password
                        RealPassword = MessageHeader.Password;
                    }
                }
                catch
                {
                    // Ignore exception
                }

                MembershipUserValidationInfo = Provider.ExtendedValidateUser(MessageHeader.UserName, RealPassword, MessageHeader.SysBRAND, MessageHeader.SysPEROLE);
            }

            switch (MembershipUserValidationInfo.MembershipUserValidationStatus)
            {
                // NotValid
                case Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.NotValid:
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityUserNotValid, MembershipUserValidationInfo.MembershipUserValidationStatus.ToString());
                // UserNameNotValid
                case Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.UserNameNotValid:
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityUserNameNotValid, MembershipUserValidationInfo.MembershipUserValidationStatus.ToString());
                // PasswordNotValid
                case Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.PasswordNotValid:
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityPasswordNotValid, MembershipUserValidationInfo.MembershipUserValidationStatus.ToString());
                // ValidWorkflowUserNotFound
                case Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidWorkflowUserNotFound:
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityValidWorkflowUserNotFound, MembershipUserValidationInfo.MembershipUserValidationStatus.ToString());
                // ValidRoleNotFound
                case Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidRoleNotFound:
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityValidRoleNotFound, MembershipUserValidationInfo.MembershipUserValidationStatus.ToString());
                // ValidPersonNotFound
                case Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidPersonNotFound:
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityValidPersonNotFound, MembershipUserValidationInfo.MembershipUserValidationStatus.ToString());
                // ValidBrandNotFound
                case Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidBrandNotFound:
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityValidBrandNotFound, MembershipUserValidationInfo.MembershipUserValidationStatus.ToString());
                // UserDisabled
                case Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.UserDisabled:
                    throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityUserDisabled, MembershipUserValidationInfo.MembershipUserValidationStatusReason.ToString());
            }


            // User is valid, set properties
            this.SYSPUSER = MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value;
            this.SYSWFUSER = MembershipUserValidationInfo.WFUSERDto.SYSWFUSER.Value;
            this.SysPEROLE = MessageHeader.SysPEROLE;
            this.SysBRAND = MessageHeader.SysBRAND;
            this.ISOLanguageCode = MessageHeader.ISOLanguageCode;

            //this is role-dependent stuff
            RoleCacheDto rci = getRoleInfo(MessageHeader.SysPEROLE);
            this.SysPERSON = rci.SysPERSON;
            this.VpSysPEROLE = rci.VpSysPEROLE;
            this.VpSysPERSON = rci.VpSysPERSON;
            

            
            credentialCache[cacheKey] = MembershipUserValidationInfo;

            return MembershipUserValidationInfo;
        }

        public  RoleCacheDto getRoleInfo(long sysPEROLE)
        {
            if (!roleCache.ContainsKey(sysPEROLE))
            {
                RoleCacheDto rval = new RoleCacheDto();
                using (DdOlExtended Context = new DdOlExtended())
                {
                    PersonDto vkper = PeroleHelper.DeliverPerson(Context, SysPEROLE);
                    if (vkper != null)
                        rval.SysPERSON = vkper.sysperson;
                    else
                        rval.SysPERSON = MembershipUserValidationInfo.BRANDDto[0].VpPEROLEDtoArray[0].PEROLEDDtoArray[0].PERSONDto.SYSPERSON.Value;

                    PEROLE vpperole = Cic.OpenLease.Common.MembershipProvider.MyFindVpOrRootPEROLE(Context, SysPEROLE,  PeroleHelper.CnstVPRoleTypeNumber);
                    if (vpperole != null)
                    {
                        rval.VpSysPEROLE = vpperole.SYSPEROLE;
                        rval.VpSysPERSON = vpperole.SYSPERSON;
                    }
                    else
                    {
                        rval.VpSysPEROLE = (long)MembershipUserValidationInfo.BRANDDto[0].VpPEROLEDtoArray[0].SYSPEROLE;
                        rval.VpSysPERSON = MembershipUserValidationInfo.BRANDDto[0].VpPEROLEDtoArray[0].SYSPERSON;
                    }
                }
                roleCache[sysPEROLE] = rval;
            }
            return roleCache[sysPEROLE];
        }
        #endregion
        private static CacheDictionary<long, RoleCacheDto> roleCache = CacheFactory<long, RoleCacheDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
    }
    class RoleCacheDto
    {
        public long SysPERSON {get;set;}
        public long VpSysPEROLE {get;set;}
        public long? VpSysPERSON {get;set;}
    }
}