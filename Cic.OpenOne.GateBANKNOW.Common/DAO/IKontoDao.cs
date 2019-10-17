using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// KontoDao Interface
    /// </summary>
    public interface IKontoDao
    {
        /// <summary>
        /// createKonto Methode
        /// </summary>
        /// <param name="kontoInput">KontoDto</param>
        /// <returns>KontoDto</returns>
        KontoDto createKonto(KontoDto kontoInput);

        /// <summary>
        /// updateKonto Methode
        /// </summary>
        /// <param name="kontoInput">KontoDto</param>
        /// <returns>KontoDto</returns>
        KontoDto updateKonto(KontoDto kontoInput);

        /// <summary>
        /// getKonto Methode
        /// </summary>
        /// <param name="sysid">SYSKONTO</param>
        /// <returns>KontoDto</returns>
        KontoDto getKonto(long sysid);

       /// <summary>
        /// finds the Blz by the given key and type of key
       /// </summary>
       /// <param name="data"></param>
       /// <param name="type"></param>
       /// <returns></returns>
        List<BlzDto> findBlz(String data, BlzType type);
    }
}
