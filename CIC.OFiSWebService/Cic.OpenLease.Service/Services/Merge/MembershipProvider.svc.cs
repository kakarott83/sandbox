// OWNER MK, 18-09-2008
using Cic.OpenOne.Common.Util.Collection;
using System;
using Cic.OpenOne.Common.Util.Extension;
using Cic.OpenOne.Common.BO;
using System.Collections.Generic;
using Cic.OpenLease.ServiceAccess.Merge.MembershipProvider;
using CIC.Database.OL.EF6.Model;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Model.DdOw;

namespace Cic.OpenLease.Service.Merge.MembershipProvider
{

    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.MembershipProvider")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public sealed class MembershipProviderService : Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.IMembershipProviderService
    {
        #region Private constants
        private string CnstMasterPassword = "XAKLOP901ASDDDA";
        #endregion

        #region IMembershipService Members
        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus ValidateUser(string userName, string password)
        {
            Cic.OpenLease.Common.MembershipProvider Provider;

            // NOTE BK, Parameter checking removed

            // Get provider
            Provider = MembershipProviderHelper.DeliverProvider();
            
            // Check provider
            if (Provider == null) 
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralMembershipProvider);
            }

            try
            {
                if (Provider.ValidateUser(userName, password))
                {
                    // Valid
                    // NOTE JJ, More then one return
                    return Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid;
				}
                else
                {
                    try
                    {
                        if (!Provider.ValidateUserName(userName))
                        {
                            // UserNameNotValid
                            // NOTE JJ, More then one return
                            return Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.UserNameNotValid;
                        }
                        else if (!Provider.ValidateUserPassword(userName, password))
                        {
                            // PasswordNotValid
                            // NOTE JJ, More then one return
                            return Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.PasswordNotValid;
                        }
                        else if (!Provider.ValidateDisabled(userName))
                        {
                            // UserDisabled
                            // NOTE JJ, More then one return
                            return Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.UserDisabled;
                        }
                        else if (!Provider.ValidateUserGroup(userName, password))
                        {
                            // ValidRoleNotFound
                            // NOTE JJ, More then one return
                            return Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.ValidRoleNotFound;
                        }
                        else
                        {
                            // Otherwise: NotValid
                            // NOTE JJ, More then one return
                            return Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.NotValid;
                        }
                    } 
                    catch
                    {
                        // TODO MK 0 MK, Log exception
                        throw;
                    }
				}
            }
            catch
            {
                throw;
            }
        }
        private static CacheDictionary<String, Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo> credentialCache = CacheFactory<String, Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ExtendedValidateUser(string userName, string password)
		{
            String key = userName + "_" + password;
            if (!credentialCache.ContainsKey(key))
            {

                Cic.OpenLease.Common.MembershipProvider Provider;
                string RealPassword = null;

                // NOTE BK, Parameter checking removed

                // Get provider
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
                        

                        //Get Password from Configuration
                        MasterPassword = AppConfig.getValueFromDb("AIDA", "WEBSERVICES", "GENERALPASSWORD", string.Empty);
                    }
                    catch
                    {
                        MasterPassword = CnstMasterPassword;
                    }

                    //Check if password is correct
                    IsMasterPassword = password == MasterPassword;


                    if (IsMasterPassword)
                    {
                        // NOTE JJ, If the Master password is valid the RealPassword is replaced by PUSER password
                        RealPassword = Provider.GetPassword(userName);
                    }
                    else
                    {
                        // Set real password
                        RealPassword = password;
                    }
                }
                catch
                {
                    // Ignore exception
                }

                try
                {
                    // Extended validate user
                    Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo rval = Provider.ExtendedValidateUser(userName, RealPassword);
                    //dont cache on error
                    if (rval.MembershipUserValidationStatus == ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid)
                        credentialCache[key] = rval;
                    else return rval;
                }
                catch
                {
                    throw;
                }
            }
            return credentialCache[key];
		}

        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo ExtendedValidateDealer(string userName, string password, string dealerCode)
        {
            Cic.OpenLease.Common.MembershipProvider Provider=null;
            string RealPassword = null;

            // NOTE BK, Parameter checking removed

            // Get provider
            //Provider = MembershipProviderHelper.DeliverProvider();
            
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

                    MasterPassword = AppConfig.getValueFromDb("AIDA", "WEBSERVICES", "GENERALPASSWORD", string.Empty);
                }
                catch
                {
                    MasterPassword = CnstMasterPassword;
                }
                
                //Check if password is correct
                IsMasterPassword = password == MasterPassword;
                

                if (IsMasterPassword)
                {                    
                    // NOTE JJ, If the Master password is valid the RealPassword is replaced by PUSER password
                    RealPassword = Provider.GetPassword(userName);
                }
                else
                {
                    // Set real password
                    RealPassword = password;
                }
            }
            catch
            {
                // Ignore exception
            }

            try
            {
                // Extended validate dealer
                return Provider.ExtendedValidateDealer(userName, RealPassword);
            }
            catch
            {
                throw;
            }
        }

        public void SetDefaultPerole(long defaultPerole)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();
            

            try
            {               
                using ( DdOwExtended Context = new DdOwExtended())
                {
                    // Get PUSER
                    //Cic.OpenLease.Model.DdOw.PUSER PUSER = Model.DdOw.PortalUserHelper.SelectById(Context, ServiceValidator.SYSPUSER);

                    // Set default PEROLE
                    //PUSER.SYSDEFAULTPEROLE = defaultPerole;

                    // Update
                    //Context.Update<Cic.OpenLease.Model.DdOw.PUSER>(PUSER, null);

                    Context.ExecuteStoreCommand("update puser set sysdefaultperole=" + defaultPerole + " where syspuser=" + ServiceValidator.SYSPUSER);
                    Context.SaveChanges();
                    ServiceValidator.invalidateUser();




                    String key =  ServiceValidator.getLoginKey();
                    if (credentialCache.ContainsKey(key))
                        credentialCache.RemoveSafe(key);
                }
            }
            catch (System.Exception ex)
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralSelect, ex);
            }
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            System.Web.Security.MembershipProvider Provider=null;

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateEdit();

            // Get provider
          //  Provider = MembershipProviderHelper.DeliverProvider();
            
            // Check provider
            if (Provider == null)
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralMembershipProvider);
            }

            try
            {
                // Change password
                return Provider.ChangePassword(userName, oldPassword, newPassword);
            }
            catch
            {
                throw;
            }
        }

        public Cic.OpenLease.ServiceAccess.MessageHeader ReturnExampleMessageHeader()
        {  
            
            Cic.OpenLease.ServiceAccess.MessageHeader FakeMessageHeader = new Cic.OpenLease.ServiceAccess.MessageHeader("test", "test", 0, 0, "de-DE");
            return FakeMessageHeader;
        }

        public Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.RfgDto[] DeliverPermissions()
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            IRightsMapBo rightsMapBo = CommonBOFactory.getInstance().createRightsMapBo();
            List<Cic.OpenOne.Common.DTO.RightsMap> rights = rightsMapBo.getRightsForWFUser(ServiceValidator.SYSWFUSER);

            Dictionary<String, RfgDto> workMap = new Dictionary<string, RfgDto>();
            foreach (Cic.OpenOne.Common.DTO.RightsMap r in rights)
            {
                if (!workMap.ContainsKey(r.rightsMapId))
                {
                    if (!r.codeRFU.StartsWith("B2B")) continue;
                    RfgDto rm = new RfgDto
                    {
                        codeRFU = r.codeRFU,
                        codeRMO = r.codeRMO,
                        rechte = r.rechte
                    };
                    workMap[r.rightsMapId] = rm;
                    continue;
                }
                //doublette, or the rights
                RfgDto rmap = workMap[r.rightsMapId];
                int l1 = Convert.ToInt32(rmap.rechte, 2);
                int l2 = Convert.ToInt32(r.rechte, 2);
                int l3 = l1 | l2;
                rmap.rechte = Convert.ToString(l3, 2);
            }
            return new List<RfgDto>(workMap.Values).ToArray();


            /*

            System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.RfgDto> RfgDtoList = new System.Collections.Generic.List<ServiceAccess.Merge.MembershipProvider.RfgDto>();
            Cic.OpenLease.Model.DdOw.RFG RFG = new Model.DdOw.RFG();

            long SysRfn;
            RfgAssembler RfgAssembler;
            Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.RfgDto RfgDto;

            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            using (Cic.OpenLease.Model.DdOw.OwExtendedEntities Context = new Cic.OpenLease.Model.DdOw.OwExtendedEntities())
            {
                foreach (string RfnNameLoop in System.Enum.GetNames(typeof(Cic.OpenLease.ServiceAccess.Rfgs)))
                {
                    //Get SysRfn
                    SysRfn = Cic.OpenLease.Model.DdOw.RFNHelper.DeliverSysRfn(Context, RfnNameLoop);

                    //Get rfg values
                    RFG = Cic.OpenLease.Model.DdOw.RFGHelper.DeliverRFG(Context, ServiceValidator.SYSWFUSER, SysRfn);
                    
                    //Create net dto
                    RfgDto = new ServiceAccess.Merge.MembershipProvider.RfgDto();

                    //Create new assembler
                    RfgAssembler = new Service.RfgAssembler();

                    //Convert do Dto
                    RfgDto = RfgAssembler.ConvertToDto(RFG);
                    RfgDto.Rfg = (Cic.OpenLease.ServiceAccess.Rfgs)System.Enum.Parse(typeof(Cic.OpenLease.ServiceAccess.Rfgs), RfnNameLoop, true);

                    RfgDtoList.Add(RfgDto);
                }
        
            }
            return RfgDtoList.ToArray();*/
        }
        #endregion

        #region IMembershipService Members (not in contract)
        public bool DeleteUser(string userName)
        {
            System.Web.Security.MembershipProvider Provider=null;

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateDelete();

            // NOTE BK, Parameter checking removed

            // Get provider
            //Provider = MembershipProviderHelper.DeliverProvider();
            // Check provider
            if (Provider == null)
            {
                // Throw exception
                // TODO MK 0 BK, Add exception class localize text
                throw new System.ApplicationException("No valid provider available.");
            }

            try
            {
                // Return
                return Provider.DeleteUser(userName, false);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        /*
        public System.Web.Security.MembershipUser CreateUser(string userName, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            System.Web.Security.MembershipProvider Provider;

            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateInsert();

            // NOTE BK, Parameter checking removed

            // Get provider
            Provider = MembershipProviderHelper.DeliverProvider();
            // Check provider
            if (Provider == null)
            {
                // Throw exception
                // TODO MK 0 BK, Add exception class localize text
                throw new System.ApplicationException("No valid provider available.");
            }

            try
            {
                // Return
                return Provider.CreateUser(userName, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }*/

        // TODO MK 0 MK, Move this somwhere else (or not?)
        public CIC.Database.OW.EF6.Model.RFG DeliverRfg(string serviceName, string methodName)
        {
            ServiceValidator ServiceValidator = new ServiceValidator(serviceName, methodName);

            return ServiceValidator.RFG;
        }

        // TODO MK 0 MK, Move this somwhere else (or not?)
        public PEROLE[] DeliverPeRole(string userName, string password)
        {
            System.Collections.Generic.List< PEROLE> PeRoleList;

            PeRoleList = new System.Collections.Generic.List< PEROLE>();

            Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationInfo MembershipUserValidationInfo = null;

            try
            {
                MembershipUserValidationInfo = ExtendedValidateUser(userName, password);
            }
            catch
            {
                // Ignore exceotions
            }

            // Exception or user not found
            if (MembershipUserValidationInfo == null || MembershipUserValidationInfo.MembershipUserValidationStatus != Cic.OpenLease.ServiceAccess.Merge.MembershipProvider.MembershipUserValidationStatus.Valid)
            {
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.SecurityUserNotValid);
            }

            // TODO Erased
            //// Can impersonate Puser?
            //if (MembershipUserValidationInfo.Person != null)
            //{
            //    try
            //    {
            //        using (Cic.OpenLease.Model.DdOl.OlExtendedEntities Context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            //        {
            //            PeRoleList = Cic.OpenLease.Model.DdOl.PEROLEHelper.DeliverSightfieldPeRoles(Context, MembershipUserValidationInfo.Person.SYSPERSON);
            //        }
            //    }
            //    catch
            //    {
            //    }
            //}

            PeRoleList = new System.Collections.Generic.List< PEROLE>();

            return PeRoleList.ToArray();
        }
        #endregion
    }
}
