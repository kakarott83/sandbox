using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;

namespace Cic.One.Web.BO
{
    public interface IAuthenticationBo
    {
        /// <summary>
        /// /// Delivers all users in sight of field of the current user
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        WfuserDto[] getPermittedUsers(long sysperole);

        /// <summary>
        /// Authentifizieren
        /// </summary>
        /// <param name="input">Benutzerlogindaten</param>
        /// <param name="loginType">Anmeldetyp</param>
        /// <param name="authInfo">Authentifizierungsdaten</param>
        /// <returns>Authentifizierungsdaten Ausgang</returns>
        ogetValidateUserDto authenticate(igetValidateUserDto input, int loginType, ref ogetValidateUserDto authInfo);

        /// <summary>
        /// finds a certain wfuser
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        WfuserDto getWfuserDto(igetWfuserDto input);


        /// <summary>
        /// login via eaihot
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        /// <returns></returns>
        ologinEaiHotDto loginEaihot(iloginEaiHotDto input, ref ologinEaiHotDto rval);

         /// <summary>
        /// Changes the Users' password
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="newpassword"></param>
        void changeUserPassword(long syswfuser, String newpassword);

        /// <summary>
        /// Creates a deeplink for changing the password for the user, sent by email
        /// </summary>
        /// <param name="wfusername"></param>
        void createPasswordDeepLink(String wfusername);
		
    }
}