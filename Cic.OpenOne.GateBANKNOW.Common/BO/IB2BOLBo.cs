using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Interface of B2BOL mTan Service
    /// </summary>
    public interface IB2BOLBo
    {
        /// <summary>
        /// Creates a new User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ocreatemTanUserDto CreateUser(imTanUserDto user);

        /// <summary>
        /// Reads a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ogetmTanUserDataDto GetData(imTanUserDto user);

        /// <summary>
        /// Writes/Updates User Data
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        osetmTanUserDataDto SetData(imTanUserDto user);

        /// <summary>
        /// Updates the user Password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        mTanStatusDto SetPassword(isetmTanUserPasswordDto user);
    }
}
