using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Cic.One.DTO;

namespace Cic.One.Web.Contract
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract(Name = "IauskunftService", Namespace = "http://cic-software.de/One")]
    public interface IauskunftService
    {
        /// <summary>
        /// Validates the given iban
        /// when swiss iban also check with iban-service dll
        /// </summary>
        /// <param name="iban"></param>
        /// <returns></returns>
        [OperationContract]
        bool validateIBAN(String iban);

        /// <summary>
        /// Return BLZ and kontonummer
        /// </summary>
        /// <param name="iban"></param>
        /// <returns></returns>
        [OperationContract]
        IBANkonto getIBANkonto(String iban);

        /// <summary>
        ///  finds the IBAN for the given account number and bank code
        /// </summary>
        /// <param name="kontoNummer"></param>
        /// <param name="bcpcNummer"></param>
        /// <returns></returns>
        [OperationContract]
        IBANkonto  findIBANByBlz(String kontoNummer, String bcpcNummer) ;

        /// <summary>
        /// Ruft die Bonität von Crefo ab.
        /// </summary>
        /// <param name="input">igetBonitaetDto</param>
        /// <returns>ogetBonitaetDto</returns>
        [OperationContract]
        ogetBonitaetDto getBonitaet(igetBonitaetDto input);

        /// <summary>
        /// Ruft die Bonität von Schufa ab.
        /// </summary>
        /// <param name="input">igetBonitaetSchufaDto</param>
        /// <returns>ogetBonitaetSchufaDto</returns>
        [OperationContract]
        ogetBonitaetSchufaDto getBonitaetSchufa(igetBonitaetSchufaDto input);
    }
}


 