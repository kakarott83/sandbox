using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.Common.Util.Security
{
    /// <summary>
    /// Used for username+password Authentication as described in 
    /// https://www.codeproject.com/Articles/96028/WCF-Service-with-custom-username-password-authenti
    /// 
    /// used for 
    /// <serviceCredentials> 
    ///                 <serviceCertificate findValue = "MyWebSite"  storeLocation="LocalMachine" storeName="My"  x509FindType="FindBySubjectName" />
    ///                 <userNameAuthentication userNamePasswordValidationMode = "Custom" customUserNamePasswordValidatorType="Cic.OpenOne.Common.Util.Security.UserNamePassValidator, Cic.OpenOne.Common" />
    /// </serviceCredentials>
    /// </summary>
    class UserPasswordValidator :
          System.IdentityModel.Selectors.UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (userName == null || password == null)
            {
                throw new ArgumentNullException();
            }
            
        }
    }
}
