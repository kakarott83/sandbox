using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Interface von KontoBo
    /// </summary>
    public interface IKontoBo
    {
        /// <summary>
        /// Methode createOrUpdateKonto
        /// </summary>
        /// <param name="konto"></param>
        /// <returns></returns>
        KontoDto createOrUpdateKonto(KontoDto konto);

        /// <summary>
        /// finds the Blz by the given key and type of key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<BlzDto> findBlz(String data, BlzType type);

        /// <summary>
        /// finds the Bank id and name for the given account number and bank code
        /// </summary>
        /// <param name="kontoNummer"></param>
        /// <param name="bcpcNummer"></param>
        /// <returns></returns>
        ofindBankByBlzDto findBankByBlz(String kontoNummer, String bcpcNummer);

        /// <summary>
        /// finds the IBAN for the given account number and bank code
        /// </summary>
        /// <param name="kontoNummer"></param>
        /// <param name="bcpcNummer"></param>
        /// <returns></returns>
        ofindIBANByBlzDto findIBANByBlz(String kontoNummer, String bcpcNummer);

    }
}
