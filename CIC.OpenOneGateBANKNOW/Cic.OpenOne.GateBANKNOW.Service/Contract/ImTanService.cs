using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Contract for the mTan B2BOL AD Service for accessing bank now internal user/password-Management
    /// </summary>
    [ServiceContract(Name = "ImTanService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface ImTanService
    {
        /// <summary>
        /// Creates a BNOW User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [OperationContract]
        ocreatemTanUserDto CreateUser(imTanUserDto user);
        
        /// <summary>
        /// Reads a BNOW User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [OperationContract]
        ogetmTanUserDataDto GetData(imTanUserDto user);
        
        /// <summary>
        /// Updates a BNOW User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [OperationContract]
        osetmTanUserDataDto SetData(imTanUserDto user);

        /// <summary>
        /// Changes the BNOW User Password
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        osetPasswordDto SetPassword(isetmTanUserPasswordDto input);
        
    }
}
