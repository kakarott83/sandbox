using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.OpenOne.Common.Util.SOAP;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.Util.Extension;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Die Schnittstelle loginPartnerService definiert eine Methode für den Anmeldevorgang wie Autorisierung(login) und
    /// Global verwendete Listen wie Übersetzungen und Vertragsarten
    /// </summary>
    [ServiceContract(Name = "IloginPartnerService", Namespace = "http://cic-software.de/GateBANKNOW")]

    public interface IloginPartnerService
    {
        /// <summary>
        /// Liefert ein Valid User Objekt zurück oder eine Exception mit den passenden Rückgabewerten.
        /// </summary>
        /// <param name="Input">iExtendedUserDto</param>
        /// <returns>oExtendedUserDto</returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        Cic.OpenOne.GateBANKNOW.Common.DTO.oExtendedUserDto extendedValidateUser(DTO.iExtendedUserDto Input);


        /// <summary>
        /// returns all GUI-Field Translations
        /// </summary>
        /// <returns>oTranslationDto</returns>
        [OperationContract]
        [SoapLoggingAttribute(disable = true)]
        oTranslationDto getTranslations();

        /// <summary>
        /// Returns all Vertragsarten for all Bildwelten and languages
        /// </summary>
        /// <returns>oVertragsartenDto</returns>
        [OperationContract]
        oVertragsartenDto getVertragsarten();

    }
}
