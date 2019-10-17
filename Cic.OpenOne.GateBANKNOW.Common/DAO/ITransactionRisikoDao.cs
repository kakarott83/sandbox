using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public interface ITransactionRisikoDao
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kundenid"></param>
        /// <returns></returns>
        DecisionEngineInDto getInputDatenDE(long sysantrag);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        int getRiskFlag(long sysid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        int getPPI_Flag_Paket1(long sysid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        double? getPPI_Flag_Paket2(long sysid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        Budget4DESimDto getBudget4DESim(long sysid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        bool getRwbruttoFlag(long sysobtyp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        bool getRwbruttoBankNowFlag(long sysobtyp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        string getMarktwert_Cluster(long sysid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysObTyp"></param>
        /// <returns></returns>
        VG getSysVGByObTyp(long sysObTyp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        string getScoreBezeichnungByAntragId(long sysid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        double getAusfallwvgByAntragId(long sysid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="angAntKalkDto"></param>
        /// <param name="rang"></param>
        void saveVariante(long sysid, AngAntKalkDto angAntKalkDto, int? rang, string[] simulationDERules);

        /// <summary>
        /// Anzahl getroffenen Regeln
        /// </summary>
        /// <param name="sysAuskunft"></param>
        int getGetroffenenRegelnAnzahl(long sysAuskunft);

        /// <summary>
        /// save Varianten Deruls
        /// </summary>
        /// <param name="syskalk"></param>
        /// <param name="sysAuskunft"></param>
        void saveVariantenDERuls(long sysantkalkvar, long sysAuskunft);

        /// <summary>
        /// save Varianten DERuls from string[]
        /// </summary>
        /// <param name="sysantkalkvar"></param>
        /// <param name="simulationDERules"></param>
        void saveVariantenDERuls(long sysantkalkvar, string[] simulationDERules);

        /// <summary>
        /// updateKalkMitVariantenDaten
        /// </summary>
        /// <param name="kalk"></param>
        void updateKalkMitVariantenDaten(AngAntKalkDto kalk);

        /// <summary>
        /// deleteVarianteUndDERulsByAntrag
        /// </summary>
        /// <param name="sysid"></param>
        void deleteVarianteUndDERulsByAntrag(long sysid);


        /// <summary>
        /// getZustandFromObject
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        int getZustandFromObject(long sysid);



        /// <summary>
        ///  getVariante
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="rang"></param>
        /// <returns></returns>
        AngAntKalkDto getVariante(long sysid, long rang, bool mitRisikoFilter);

        /// <summary>
        /// evalRestwert
        /// </summary>
        /// <param name="scoreTR_RWFormel"></param>
        /// <param name="sysID"></param>
        /// <param name="pANZ"></param>
        /// <returns></returns>
        double? evalRestwert(string scoreTR_RWFormel, long sysID, int ALZ1);
    }
}
