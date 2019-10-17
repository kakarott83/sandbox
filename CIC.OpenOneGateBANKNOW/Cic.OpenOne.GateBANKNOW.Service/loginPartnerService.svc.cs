using System;
using System.Reflection;
using System.ServiceModel;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.BO;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    /// <summary>
    /// Der Service loginPartner stellt die Loginfunktionalität bereit.
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class loginPartnerService : IloginPartnerService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Liefert ein Valid User Objekt zurück oder eine Exception mit den passenden Rückgabewerten.
        /// Benutzt USERTYPE-PUSER - Anmeldung, d.h. username muss ein PUSER sein
        /// </summary>
        /// <param name="Input">iExtendedUserDto</param>
        /// <returns>oExtendedUserDto</returns>
        public Cic.OpenOne.GateBANKNOW.Common.DTO.oExtendedUserDto extendedValidateUser(Cic.OpenOne.GateBANKNOW.Service.DTO.iExtendedUserDto Input)
        {
            oExtendedUserDto rval = new oExtendedUserDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (Input == null)
                {
                    throw new ArgumentException("No input ExtendedUserDto was sent.");
                }
                AuthenticationBo authBo = new AuthenticationBo();
                authBo.authenticate(Input.username, Input.presharedKey, MembershipProvider.USER_TYPE_PUSER, ref rval);

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)      // expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)                 // unhandled exception
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Returns all Vertragsarten for all Bildwelten and languages
        /// </summary>
        /// <returns>oVertragsartenDto</returns>
        public oVertragsartenDto getVertragsarten()
        {
            oVertragsartenDto rval = new oVertragsartenDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                rval.vertragsarten = Cic.OpenOne.Common.DAO.Prisma.PrismaDaoFactory.getInstance().getPrismaDao().getBildweltVertragsarten();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)      // expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)                 // unhandled exception
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// returns all GUI-Field Translations
        /// by this query
        /// select ctfoid.frontid,typ,verbaldescription MASTER, replaceterm TRANSLATION,replaceblob,isocode 
        /// FROM ctfoid,cttfoid,ctlang where ctfoid.frontid=cttfoid.frontid and ctlang.sysctlang=cttfoid.sysctlang and flagtranslate=1 order by ctfoid.frontid;
        /// </summary>
        /// <returns>oTranslationDto</returns>
        public oTranslationDto getTranslations()
        {
            oTranslationDto rval = new oTranslationDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                ITranslateBo Translator = new TranslateBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao());
                rval.translations = Translator.GetStaticList();

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)      // expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)                 // unhandled exception
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }
    }
}