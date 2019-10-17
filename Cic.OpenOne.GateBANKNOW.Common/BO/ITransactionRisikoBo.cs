using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public interface ITransactionRisikoBo 
    {
        /// <summary>
        /// processFinVorEinreichung
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ofinVorEinreichungDto processFinVorEinreichung(ifinVorEinreichungDto input);

        /// <summary>
        /// checkTrRisk
        /// </summary>
        /// <param name="input"></param>
        /// <param name="user"></param>
        /// <param name="isoCode"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        ocheckTrRiskDto checkTrRisk(icheckTrRiskDto input, long user, string isoCode, bool kalkVariante,long syswfuser);

        /// <summary>
        /// checkTrRiskBySysid
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        ocheckTrRiskDto checkTrRiskBySysid(long sysid, long sysPEROLE, string isoCode, long syswfuser);


        /// <summary>
        /// risikoSim
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        orisikoSimDto risikoSim(irisikoSimDto input);

        /// <summary>
        /// solveKalkVarianten
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        osolveKalkVariantenDto solveKalkVarianten(isolveKalkVariantenDto input);


        /// <summary>
        /// checkTrRiskBySysid
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        ocheckTrRiskByIdDto checkTrRiskBySysid(icheckTrRiskByIdDto inDto);

        /// <summary>
        /// checkTrRisk
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <param name="outputGUI"></param>
        /// <returns></returns>
        ocheckTrRiskDto checkTrRisk(icheckTrRiskDto input, long sysPEROLE, string isoCode,bool kalkVariante, ref ocheckTrRiskByIdDto outputGUI);

        /// <summary>
        /// getV_cluster
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        VClusterDto getV_cluster(long sysid, bool saveMarktwerteInDB, long sysprproduct, long sysperole);

        /// <summary>
        /// deleteVarianteUndDERulsByAntrag
        /// </summary>
        /// <param name="sysid"></param>
        void deleteVarianteUndDERulsByAntrag(long sysid);

        /// <summary>
        /// getVariante
        /// </summary>
        /// <param name="antrag"></param>
        /// <param name="rang"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        AngAntKalkDto getVariante(AntragDto antrag, long rang, long sysPEROLE, string isoCode, bool mitRisikoFilter);

        /// <summary>
        /// REMO-Staffel wird  angelegt
        /// </summary>
        /// <param name="sysid"></param>
        void remostaffelAnlegen(long sysid, long sysprproduct, long sysperole);

        /// <summary>
        /// EL-Kalk soll aber nur dann erfolgen, wenn ein EAIPAR-Statement (EAIPAR:CODE = 'EL_KALK') zum Antrag eine 1 liefert 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        bool getEL_KALKFlag(long sysid);
    }
}
