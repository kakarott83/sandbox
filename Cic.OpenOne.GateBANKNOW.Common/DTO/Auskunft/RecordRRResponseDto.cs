using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// RecordRRResponseDto
    /// </summary>
    public class RecordRRResponseDto
    {
        /// <summary>
        /// Getter/Setter DET Antragsteller
        /// </summary>
        public decimal? DET_Antragsteller { get; set; }

        /// <summary>
        /// Getter/Setter DEC Liste getroffener Regeln BP
        /// </summary>
        public string DEC_Liste_getroffeneRegeln_BP { get; set; }

        /// <summary>
        /// Getter/Setter DEC Liste getroffener Regeln RP
        /// </summary>
        public string DEC_Liste_getroffeneRegeln_RP { get; set; }

        /// <summary>
        /// Getter/Setter DEC Liste getroffener Regeln Auflagen
        /// </summary>
        public string DEC_Liste_getroffeneRegeln_Auflagen { get; set; }

        /// <summary>
        /// Getter/Setter DEC Liste getroffener Regeln Formalitäten
        /// </summary>
        public string DEC_Liste_getroffeneRegeln_Formalit { get; set; }

        /// <summary>
        /// Getter/Setter DEC Liste getroffener Regeln VP
        /// </summary>
        public string DEC_Liste_getroffeneRegeln_VP { get; set; }

        /// <summary>
        ///  Getter/Setter DEC Liste getroffener Regeln FP
        /// </summary>
        public string DEC_Liste_getroffeneRegeln_FP { get; set; }

        /// <summary>
        /// Getter/Setter DEC Scorecard Code
        /// </summary>
        public string DEC_Scorekarte_Code { get; set; }

        /// <summary>
        /// Getter/Setter DEC Scorecard Description
        /// </summary>
        public string DEC_Scorekarte_Bezeichnung { get; set; }

        /// <summary>
        /// Getter/Setter DEC Scorevalue Total
        /// </summary>
        public decimal? DEC_Scorewert_Total { get; set; }

        /// <summary>
        /// Getter/Setter DEC Risikoklasse ID
        /// </summary>
        public decimal? DEC_Risikoklasse_ID { get; set; }

        /// <summary>
        /// Getter/Setter DEC Risikoklasse Description
        /// </summary>
        public string DEC_Risikoklasse_Bezeichnung { get; set; }

        /// <summary>
        /// Getter/Setter Score_PD
        /// </summary>
        public decimal? DEC_Score_PD { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 1
        /// </summary>
        public decimal? SC_ID_1 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 1
        /// </summary>
        public string SC_Bezeichnung_1 { get; set; }

        /// <summary>
        /// Getter/Setter SC Result value 1
        /// </summary>
        public decimal? SC_Resultatwert_1 { get; set; }

        /// <summary>
        /// Getter/Setter SC Inputvalue 1
        /// </summary>
        public string SC_Eingabewert_1 { get; set; }

        /// <summary>
        /// Getter/Setter SC Calculation variable 1
        /// </summary>
        public string SC_Berechnungsvariablen_1 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 2
        /// </summary>
        public decimal? SC_ID_2 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 2
        /// </summary>
        public string SC_Bezeichnung_2 { get; set; }

        /// <summary>
        /// Getter/Setter SC Result value 2
        /// </summary>
        public decimal? SC_Resultatwert_2 { get; set; }

        /// <summary>
        /// Getter/Setter Eingabewert 2
        /// </summary>
        public string SC_Eingabewert_2 { get; set; }

        /// <summary>
        /// Getter/Setter SC Calculation variable 2
        /// </summary>
        public string SC_Berechnungsvariablen_2 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 3
        /// </summary>
        public decimal? SC_ID_3 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 3
        /// </summary>
        public string SC_Bezeichnung_3 { get; set; }

        /// <summary>
        /// Getter/Setter Result value 3
        /// </summary>
        public decimal? SC_Resultatwert_3 { get; set; }

        /// <summary>
        /// Getter/Setter SC input value 3
        /// </summary>
        public string SC_Eingabewert_3 { get; set; }

        /// <summary>
        /// Getter/Setter SC Calculation variable 3
        /// </summary>
        public string SC_Berechnungsvariablen_3 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 4
        /// </summary>
        public decimal? SC_ID_4 { get; set; }

        /// <summary>
        /// Getter/Setter SC description
        /// </summary>
        public string SC_Bezeichnung_4 { get; set; }

        /// <summary>
        /// Getter/Setter SC Resultvalue 4
        /// </summary>
        public decimal? SC_Resultatwert_4 { get; set; }

        /// <summary>
        /// SC Inputvalue 4
        /// </summary>
        public string SC_Eingabewert_4 { get; set; }

        /// <summary>
        /// Getter/Setter SC Calculation variable 4
        /// </summary>
        public string SC_Berechnungsvariablen_4 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 5
        /// </summary>
        public decimal? SC_ID_5 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 5
        /// </summary>
        public string SC_Bezeichnung_5 { get; set; }

        /// <summary>
        /// SC Result value 5
        /// </summary>
        public decimal? SC_Resultatwert_5 { get; set; }

        /// <summary>
        /// Getter/Setter  SC Input value 5
        /// </summary>
        public string SC_Eingabewert_5 { get; set; }

        /// <summary>
        /// Getter/Setter SC Calcualtion varivable 5
        /// </summary>
        public string SC_Berechnungsvariablen_5 { get; set; }

        /// <summary>
        /// Getter/Setter  SC ID 6
        /// </summary>
        public decimal? SC_ID_6 { get; set; }

        /// <summary>
        /// Getter/Setter SC Descripton 6
        /// </summary>
        public string SC_Bezeichnung_6 { get; set; }

        /// <summary>
        /// Getter/Setter  SC Result value 6
        /// </summary>
        public decimal? SC_Resultatwert_6 { get; set; }

        /// <summary>
        /// Getter/Setter  SC Input value 6
        /// </summary>
        public string SC_Eingabewert_6 { get; set; }

        /// <summary>
        /// Getter/Setter  SC calculation variable 6
        /// </summary>
        public string SC_Berechnungsvariablen_6 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 7
        /// </summary>
        public decimal? SC_ID_7 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 7
        /// </summary>
        public string SC_Bezeichnung_7 { get; set; }

        /// <summary>
        /// Getter/Setter SC resultvalue 7
        /// </summary>
        public decimal? SC_Resultatwert_7 { get; set; }

        /// <summary>
        /// Getter/Setter SC Input value 7
        /// </summary>
        public string SC_Eingabewert_7 { get; set; }

        /// <summary>
        /// Getter/Setter SC calcualtion variable 7
        /// </summary>
        public string SC_Berechnungsvariablen_7 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 8
        /// </summary>
        public decimal? SC_ID_8 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 8
        /// </summary>
        public string SC_Bezeichnung_8 { get; set; }

        /// <summary>
        /// Getter/Setter SC result value 8
        /// </summary>
        public decimal? SC_Resultatwert_8 { get; set; }

        /// <summary>
        /// Getter/Setter SC Input value 8
        /// </summary>
        public string SC_Eingabewert_8 { get; set; }

        /// <summary>
        /// Getter/Setter SC Calculation variable 8
        /// </summary>
        public string SC_Berechnungsvariablen_8 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 9
        /// </summary>
        public decimal? SC_ID_9 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 9
        /// </summary>
        public string SC_Bezeichnung_9 { get; set; }

        /// <summary>
        /// Getter/Setter SC Result value 9
        /// </summary>
        public decimal? SC_Resultatwert_9 { get; set; }

        /// <summary>
        /// Getter/Setter SC Input Value 9
        /// </summary>
        public string SC_Eingabewert_9 { get; set; }

        /// <summary>
        /// Getter/Setter SC Calculation variable 9
        /// </summary>
        public string SC_Berechnungsvariablen_9 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 10
        /// </summary>
        public decimal? SC_ID_10 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 10
        /// </summary>
        public string SC_Bezeichnung_10 { get; set; }

        /// <summary>
        /// Getter/Setter SC result value 10
        /// </summary>
        public decimal? SC_Resultatwert_10 { get; set; }

        /// <summary>
        /// Getter/Setter  SC Input value 10
        /// </summary>
        public string SC_Eingabewert_10 { get; set; }

        /// <summary>
        /// Getter/Setter SC Calculation variable 10
        /// </summary>
        public string SC_Berechnungsvariablen_10 { get; set; }

        /// <summary>
        /// Getter/Setter  SC ID 11
        /// </summary>
        public decimal? SC_ID_11 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 11
        /// </summary>
        public string SC_Bezeichnung_11 { get; set; }

        /// <summary>
        /// Getter/Setter  SC result value 11
        /// </summary>
        public decimal? SC_Resultatwert_11 { get; set; }

        /// <summary>
        /// Getter/Setter sc iNPUT VALUE !!
        /// </summary>
        public string SC_Eingabewert_11 { get; set; }

        /// <summary>
        /// Getter/Setter SC calculation variable 11
        /// </summary>
        public string SC_Berechnungsvariablen_11 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 12
        /// </summary>
        public decimal? SC_ID_12 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 12
        /// </summary>
        public string SC_Bezeichnung_12 { get; set; }

        /// <summary>
        /// Getter/Setter SC result value 12
        /// </summary>
        public decimal? SC_Resultatwert_12 { get; set; }

        /// <summary>
        /// Getter/Setter  SC input value 12
        /// </summary>
        public string SC_Eingabewert_12 { get; set; }

        /// <summary>
        /// Getter/Setter SC calculation variable 12
        /// </summary>
        public string SC_Berechnungsvariablen_12 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 13
        /// </summary>
        public decimal? SC_ID_13 { get; set; }

        /// <summary>
        /// Getter/Setter  SC Description 13
        /// </summary>
        public string SC_Bezeichnung_13 { get; set; }

        /// <summary>
        /// Getter/Setter  SC result value 13
        /// </summary>
        public decimal? SC_Resultatwert_13 { get; set; }

        /// <summary>
        /// Getter/Setter SC input value 13
        /// </summary>
        public string SC_Eingabewert_13 { get; set; }

        /// <summary>
        /// Getter/Setter SC calculation variable 13
        /// </summary>
        public string SC_Berechnungsvariablen_13 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 14
        /// </summary>
        public decimal? SC_ID_14 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 14
        /// </summary>
        public string SC_Bezeichnung_14 { get; set; }

        /// <summary>
        /// Getter/Setter SC result value 14
        /// </summary>
        public decimal? SC_Resultatwert_14 { get; set; }

        /// <summary>
        /// Getter/Setter SC input value 14
        /// </summary>
        public string SC_Eingabewert_14 { get; set; }

        /// <summary>
        /// Getter/Setter calcualtion variable 14
        /// </summary>
        public string SC_Berechnungsvariablen_14 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 15
        /// </summary>
        public decimal? SC_ID_15 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 15
        /// </summary>
        public string SC_Bezeichnung_15 { get; set; }

        /// <summary>
        /// Getter/Setter SC result value 15
        /// </summary>
        public decimal? SC_Resultatwert_15 { get; set; }

        /// <summary>
        /// Getter/Setter SC input value 15
        /// </summary>
        public string SC_Eingabewert_15 { get; set; }

        /// <summary>
        /// Getter/Setter SC calculation variable 15
        /// </summary>
        public string SC_Berechnungsvariablen_15 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 16
        /// </summary>
        public decimal? SC_ID_16 { get; set; }

        /// <summary>
        /// Getter/Setter SC Description 16
        /// </summary>
        public string SC_Bezeichnung_16 { get; set; }

        /// <summary>
        /// Getter/Setter SC result value 16
        /// </summary>
        public decimal? SC_Resultatwert_16 { get; set; }

        /// <summary>
        /// Getter/Setter SC input value 16
        /// </summary>
        public string SC_Eingabewert_16 { get; set; }

        /// <summary>
        /// Getter/Setter SC calcualtion variable 16
        /// </summary>
        public string SC_Berechnungsvariablen_16 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 17
        /// </summary>
        public decimal? SC_ID_17 { get; set; }

        /// <summary>
        /// Getter/Setter SC description 17
        /// </summary>
        public string SC_Bezeichnung_17 { get; set; }

        /// <summary>
        /// Getter/Setter SC result value 17
        /// </summary>
        public decimal? SC_Resultatwert_17 { get; set; }

        /// <summary>
        /// Getter/Setter SC input value 17
        /// </summary>
        public string SC_Eingabewert_17 { get; set; }

        /// <summary>
        /// Getter/Setter SC calcualtion variable 17
        /// </summary>
        public string SC_Berechnungsvariablen_17 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 18
        /// </summary>
        public decimal? SC_ID_18 { get; set; }

        /// <summary>
        /// Getter/Setter SC description 18
        /// </summary>
        public string SC_Bezeichnung_18 { get; set; }

        /// <summary>
        /// Getter/Setter SC result value 18
        /// </summary>
        public decimal? SC_Resultatwert_18 { get; set; }

        /// <summary>
        /// Getter/Setter SC input value 18
        /// </summary>
        public string SC_Eingabewert_18 { get; set; }

        /// <summary>
        /// Getter/Setter SC calcualtion variable 18
        /// </summary>
        public string SC_Berechnungsvariablen_18 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 19
        /// </summary>
        public decimal? SC_ID_19 { get; set; }

        /// <summary>
        /// Getter/Setter SC description 19
        /// </summary>
        public string SC_Bezeichnung_19 { get; set; }

        /// <summary>
        /// Getter/Setter SC result value 19
        /// </summary>
        public decimal? SC_Resultatwert_19 { get; set; }

        /// <summary>
        /// Getter/Setter SC input vlaue 19
        /// </summary>
        public string SC_Eingabewert_19 { get; set; }

        /// <summary>
        /// Getter/Setter SC calcualtion value 19
        /// </summary>
        public string SC_Berechnungsvariablen_19 { get; set; }

        /// <summary>
        /// Getter/Setter SC ID 20
        /// </summary>
        public decimal? SC_ID_20 { get; set; }

        /// <summary>
        /// Getter/Setter SC description 20
        /// </summary>
        public string SC_Bezeichnung_20 { get; set; }

        /// <summary>
        /// Getter/Setter SC resut value 20
        /// </summary>
        public decimal? SC_Resultatwert_20 { get; set; }

        /// <summary>
        /// Getter/Setter SC input value 20
        /// </summary>
        public string SC_Eingabewert_20 { get; set; }

        /// <summary>
        /// Getter/Setter SC calcualtion variable 20
        /// </summary>
        public string SC_Berechnungsvariablen_20 { get; set; }
    }
}
