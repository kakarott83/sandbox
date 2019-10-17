using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// KontoBo leitet die KontoDto anfragen zu create or update or save weiter an das KontoDao
    /// </summary>
    public class KontoBo : AbstractKontoBo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="kDao"></param>
        public KontoBo(IKontoDao kDao)
            : base(kDao)
        {
        }

        /// <summary>
        /// createOrUpdateKonto leitet die Erstellung oder das Update weiter an das KontoDAO
        /// </summary>
        /// <param name="konto">KontoDto mit einer SYSKONTO</param>
        /// <returns>KontoDto</returns>
        public override KontoDto createOrUpdateKonto(KontoDto konto)
        {
            if (konto.syskonto == 0)
            {
                return createKonto(konto);

            }
            else
            {
                return updateKonto(konto);
            }
        }

        /// <summary>
        /// createKonto leitet die Neuerstellung ans KontoDAO weiter
        /// </summary>
        /// <param name="konto">KontoDto mit einer SYSKONTO = 0</param>
        /// <returns>KontoDto mit einer SYSKONTO > 0</returns>
        public KontoDto createKonto(KontoDto konto)
        {
            return this.kontoDao.createKonto(konto);
        }

        /// <summary>
        /// updateKonto leitet das Update an das KontoDAO weiter
        /// </summary>
        /// <param name="konto">KontoDto  mit SYSKONTO > 0</param>
        /// <returns>KontoDto mit der gleichen SYSKONTO</returns>
        public KontoDto updateKonto(KontoDto konto)
        {
            return kontoDao.updateKonto(konto);

        }

        /// <summary>
        /// finds the Blz by the given key and type of key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override List<BlzDto> findBlz(String data, BlzType type)
        {
            return kontoDao.findBlz(data, type);
        }

        /// <summary>
        /// finds the Bank id and name for the given account number and bank code
        /// </summary>
        /// <param name="kontoNummer"></param>
        /// <param name="bcpcNummer"></param>
        /// <returns></returns>
        public override ofindBankByBlzDto findBankByBlz(String kontoNummer, String bcpcNummer)
        {
            ofindBankByBlzDto rval = new ofindBankByBlzDto();

            try
            {
                //try lokal ibanservice
                IBANKernel.IBANInfo info = IBANKernel.IBANKernelAccess.getIBANInfo(kontoNummer, bcpcNummer);
                rval.bankId = info.bankId;
                rval.bankName = info.bankName;
            }
            catch (Exception e)
            {

                IBANService.BANKernelClient c = new IBANService.BANKernelClient();
                IBANService.IBANInfo info = c.getIBANInfo(kontoNummer, bcpcNummer);


                rval.bankId = info.bankId;
                rval.bankName = info.bankName;
            }
           
            return rval;
        }

        /// <summary>
        /// finds the IBAN for the given account number and bank code
        /// </summary>
        /// <param name="kontoNummer"></param>
        /// <param name="bcpcNummer"></param>
        /// <returns></returns>
        public override ofindIBANByBlzDto findIBANByBlz(String kontoNummer, String bcpcNummer)
        {
            ofindIBANByBlzDto rval = new ofindIBANByBlzDto();

            try
            {
                //try lokal ibanservice
                IBANKernel.IBANInfo info = IBANKernel.IBANKernelAccess.getIBANInfo(kontoNummer, bcpcNummer);
                rval.iban = info.iban; 
            }
            catch (Exception e)
            {
                IBANService.BANKernelClient c = new IBANService.BANKernelClient();
                IBANService.IBANInfo info = c.getIBANInfo(kontoNummer, bcpcNummer);
                rval.iban = info.iban;
            }

            return rval;
        }

       
            

    }
}
