using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Bo for accessing the BNOW User Management Service
    /// </summary>
    public class B2BOLBo : IB2BOLBo
    {
        private B2BOLClient client;
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public B2BOLBo()
        {
            client = new B2BOLClient();
            client.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SOAPLogging());
        }

        /// <summary>
        /// Creates a new User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ocreatemTanUserDto CreateUser(imTanUserDto user)
        {
            try
            {
                //default the language
                if (String.IsNullOrEmpty(user.user.language))
                {
                    user.user.language = "de";
                }
                if (user.user.language.Length > 2)
                {
                    //de-DE      -> de
                    //fr-CH      -> fr
                    user.user.language = user.user.language.Substring(0, 2).ToLower();
                }
                B2BResponseContract resp = client.CreateUser(AutoMapper.Mapper.Map<mTanUserDto, B2BContract>(user.user));
                return AutoMapper.Mapper.Map<B2BResponseContract, ocreatemTanUserDto>(resp);
            }
            catch (Exception e)
            {
                _log.Error("CreateUser failed", e);
                ocreatemTanUserDto rval = new ocreatemTanUserDto();
                rval.status = new mTanStatusDto();
                rval.status.success = "False";
                rval.status.message = "technical Error, see Logfile for Details";
                return rval;

            }

        }

        /// <summary>
        /// Reads a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ogetmTanUserDataDto GetData(imTanUserDto user)
        {
            try
            {
                //default the language
                if (String.IsNullOrEmpty(user.user.language))
                {
                    user.user.language = "de";
                }
                if (user.user.language.Length > 2)
                {
                    //de-DE      -> de
                    //fr-CH      -> fr
                    user.user.language = user.user.language.Substring(0, 2).ToLower();
                }

                B2BResponseContract resp = client.GetUser(user.user.applicationId);
                return AutoMapper.Mapper.Map<B2BResponseContract, ogetmTanUserDataDto>(resp);
            }
            catch (Exception e)
            {
                _log.Error("CreateUser failed", e);
                ogetmTanUserDataDto rval = new ogetmTanUserDataDto();
                rval.status = new mTanStatusDto();
                rval.status.success = "False";
                rval.status.message = "technical Error, see Logfile for Details";
                return rval;

            }

        }

        /// <summary>
        /// Writes/Updates User Data
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public osetmTanUserDataDto SetData(imTanUserDto user)
        {
            try
            {
                //default the language
                if (String.IsNullOrEmpty(user.user.language))
                {
                    user.user.language = "de";
                }
                if (user.user.language.Length > 2)
                {
                    //de-DE      -> de
                    //fr-CH      -> fr
                    user.user.language = user.user.language.Substring(0,2).ToLower();
                }

                //fix the status to lower case
                if (!String.IsNullOrEmpty(user.user.status))
                {
                    user.user.status = user.user.status.ToLower();
                }
                B2BResponseContract resp = client.SetUser(AutoMapper.Mapper.Map<mTanUserDto, B2BContract>(user.user));

                if (resp!=null && resp.Status!=null && resp.Status.Success!=null &&"true".Equals(resp.Status.Success.ToLower())
                    && resp.Users != null && resp.Users.Length > 0 && resp.Users[0].ApplicationId!=null)
                {
                    /*
                     * Mobil 2 (PEOPTION.OPTION7) => neu Mobile EPos
                        E-Mail 2 (PEOPTION.OPTION3) => neu E-Mail EPos
                     */
                    using (PrismaExtended p = new PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters1 = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters1.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = resp.Users[0].ApplicationId });
                        long sysid = p.ExecuteStoreQuery<long>("select sysid from peoption,puser where peoption.sysid=puser.sysperson and puser.externeid=:id", parameters1.ToArray()).FirstOrDefault();
                        if(sysid>0)
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "option7", Value = resp.Users[0].Mobile });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "option3", Value = resp.Users[0].Mail });
                            p.ExecuteStoreCommand("update PEOPTION set option7=:option7, option3=:option3 where sysid=:sysid", parameters.ToArray());
                        }
                    }
                }


                return AutoMapper.Mapper.Map<B2BResponseContract, osetmTanUserDataDto>(resp);
            }
            catch (Exception e)
            {
                _log.Error("CreateUser failed", e);
                osetmTanUserDataDto rval = new osetmTanUserDataDto();
                rval.status = new mTanStatusDto();
                rval.status.success = "False";
                rval.status.message = "technical Error, see Logfile for Details: "+e.Message;
                return rval;

            }

        }

        /// <summary>
        /// Updates the user Password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public mTanStatusDto SetPassword(isetmTanUserPasswordDto user)
        {
            try
            {
                user.oldPassword = Base64Encode(user.oldPassword);
                user.newPassword = Base64Encode(user.newPassword);
                B2BStatusContract resp = client.SetPassword(user.applicationId, user.oldPassword, user.newPassword);
                return AutoMapper.Mapper.Map<B2BStatusContract, mTanStatusDto>(resp);
            }
            catch (Exception e)
            {
                _log.Error("CreateUser failed", e);
                mTanStatusDto rval = new mTanStatusDto();

                rval.success = "False";
                rval.message = "technical Error, see Logfile for Details";
                return rval;

            }

        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }

}