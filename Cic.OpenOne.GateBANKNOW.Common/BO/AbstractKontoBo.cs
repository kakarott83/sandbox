using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstraktes Konten Business Objekt
    /// </summary>
    public abstract class AbstractKontoBo : IKontoBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IKontoDao kontoDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="kontoDao">Konto Datenübertragungsobjekt</param>
        public AbstractKontoBo(IKontoDao kontoDao)
        {
            this.kontoDao = kontoDao;
        }

        /// <summary>
        /// createOrUpdateKonto Methode
        /// </summary>
        /// <param name="konto">Konto Datenübertragungsobjekt</param>
        /// <returns>geändertes oder neues Datenobjekt</returns>
        public abstract KontoDto createOrUpdateKonto(KontoDto konto);

        /// <summary>
        /// finds the Blz by the given key and type of key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract List<BlzDto> findBlz(String data, BlzType type);

        /// <summary>
        /// finds the Bank id and name for the given account number and bank code
        /// </summary>
        /// <param name="kontoNummer"></param>
        /// <param name="bcpcNummer"></param>
        /// <returns></returns>
        public abstract ofindBankByBlzDto findBankByBlz(String kontoNummer, String bcpcNummer);

        /// <summary>
        /// finds the IBAN for the given account number and bank code
        /// </summary>
        /// <param name="kontoNummer"></param>
        /// <param name="bcpcNummer"></param>
        /// <returns></returns>
        public abstract ofindIBANByBlzDto findIBANByBlz(String kontoNummer, String bcpcNummer);

    }
}
