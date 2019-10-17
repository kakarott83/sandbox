namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Dto für aggregierte Daten aus ZEK
    /// </summary>
    public class AggregationZekOutDto
    {
        // <summary>
        // 
        // </summary>
        // public int? SysAggOutZEK { get; set; }        //        NUMBER(12,0), 

        /// <summary>
        /// Anzahl ZEK Synonyme
        /// </summary>
        public int? AnzSynonyme { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl laufende ZEK Engagements 
        /// </summary>
        public int? AnzENG { get; set; }        //  NUMBER(5,0)

        /// <summary>
        /// Anzahl laufende ZEK Engagements (Bardarlehen)
        /// </summary>
        public int? AnzBD { get; set; }        // NUMBER(5,0)

        /// <summary>
        /// Anzahl laufende ZEK Engagements (Festkredit)
        /// </summary>
        public int? AnzFK { get; set; }        // NUMBER(5,0)

        /// <summary>
        /// Anzahl laufende ZEK Engagements (Leasing)
        /// </summary>
        public int? AnzLEA { get; set; }        //  NUMBER(5,0)

        /// <summary>
        /// Anzahl laufende ZEK Engagements (Teilzahlungsvertrag)
        /// </summary>
        public int? AnzTZ { get; set; }        // NUMBER(5,0)

        /// <summary>
        /// Anzahl laufende ZEK Engagements (Kontokorrent)
        /// </summary>
        public int? AnzKK { get; set; }        // NUMBER(5,0)

        /// <summary>
        /// Anzahl laufende ZEK Engagements (Kartenengagement)
        /// </summary>
        public int? AnzKA { get; set; }        // NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 05 oder 06
        /// </summary>
        public int? AnzBC0506 { get; set; }        //     NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 04
        /// </summary>
        public int? AnzBC04 { get; set; }        //   NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 04 in den letzen 12 Monaten
        /// </summary>
        public int? AnzBC04L12 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 04 in den letzen 24 Monaten
        /// </summary>
        public int? AnzBC04L24 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 04 in den letzen 36 Monaten
        /// </summary>
        public int? AnzBC04L36 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 03
        /// </summary>
        public int? AnzBC03 { get; set; }        //   NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 03 in den letzen 12 Monaten
        /// </summary>
        public int? AnzBC03L12 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 03 in den letzen 24 Monaten
        /// </summary>
        public int? AnzBC03L24 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 03 in den letzen 36 Monaten
        /// </summary>
        public int? AnzBC03L36 { get; set; }        //      NUMBER(5,0)


        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 61
        /// </summary>
        public int? ANZZEKENGBCODE61 { get; set; }        //   NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 61 in den letzen 12 Monaten
        /// </summary>
        public int? ANZZEKENGBCODE61L12M { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 61 in den letzen 24 Monaten
        /// </summary>
        public int? ANZZEKENGBCODE61L24M { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 61 in den letzen 36 Monaten
        /// </summary>
        public int? ANZZEKENGBCODE61L36M { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 71
        /// </summary>
        public int? ANZZEKENGBCODE71 { get; set; }        //   NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 71 in den letzen 12 Monaten
        /// </summary>
        public int? ANZZEKENGBCODE71L12M { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 71 in den letzen 24 Monaten
        /// </summary>
        public int? ANZZEKENGBCODE71L24M { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Engagements mit Bonitätscode 71 in den letzen 36 Monaten
        /// </summary>
        public int? ANZZEKENGBCODE71L36M { get; set; }        //      NUMBER(5,0)

       
        /// <summary>
        /// Schlechtester ZEK Bonitätscode
        /// </summary>
        public int? WorstBC { get; set; }        //   NUMBER(5,0)

        /// <summary>
        /// Anzahl laufende ZEK Fremdgesuche
        /// </summary>
        public int? AnzFremdgesuche { get; set; }        //           NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 05, 06, 08 oder 12
        /// </summary>
        public int? AnzGAC05060812 { get; set; }        //          NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 04 in den letzten 12 Monaten
        /// </summary>
        public int? AnzGAC04L12 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 07 in den letzten 12 Monaten
        /// </summary>
        public int? AnzGAC07L12 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 09 in den letzten 12 Monaten
        /// </summary>
        public int? AnzGAC09L12 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 10 in den letzten 12 Monaten
        /// </summary>
        public int? AnzGAC10L12 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 13 in den letzten 12 Monaten
        /// </summary>
        public int? AnzGAC13L12 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 14 in den letzten 12 Monaten
        /// </summary>
        public int? AnzGAC14L12 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 99 in den letzten 12 Monaten
        /// </summary>
        public int? AnzGAC99L12 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 04 in den letzten 24 Monaten
        /// </summary>
        public int? AnzGAC04L24 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 07 in den letzten 24 Monaten
        /// </summary>
        public int? AnzGAC07L24 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 09 in den letzten 24 Monaten
        /// </summary>
        public int? AnzGAC09L24 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 10 in den letzten 24 Monaten
        /// </summary>
        public int? AnzGAC10L24 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 13 in den letzten 24 Monaten
        /// </summary>
        public int? AnzGAC13L24 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 14 in den letzten 24 Monaten
        /// </summary>
        public int? AnzGAC14L24 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 99 in den letzten 24 Monaten
        /// </summary>
        public int? AnzGAC99L24 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscode 
        /// </summary>
        public int? AnzGACAll { get; set; }        //     NUMBER(5,0)

        /// <summary>
        /// Schlechtester ZEK Ablehnungscode
        /// </summary>
        public int? WorstGAC { get; set; }        //    NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 21 in den letzten 12 Monaten
        /// </summary>
        public int? AnzKM21L12 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 21 in den letzten 24 Monaten
        /// </summary>
        public int? AnzKM21L24 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 21 in den letzten 36 Monaten
        /// </summary>
        public int? AnzKM21L36 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 21 in den letzten 48 Monaten
        /// </summary>
        public int? AnzKM21L48 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 21 in den letzten 60 Monaten
        /// </summary>
        public int? AnzKM21L60 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 22 in den letzten 12 Monaten
        /// </summary>
        public int? AnzKM22L12 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 22 in den letzten 24 Monaten
        /// </summary>
        public int? AnzKM22L24 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 22 in den letzten 36 Monaten
        /// </summary>
        public int? AnzKM22L36 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 22 in den letzten 48 Monaten
        /// </summary>
        public int? AnzKM22L48 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 22 in den letzten 60 Monaten
        /// </summary>
        public int? AnzKM22L60 { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK-Kartenmeldungen mit Eregniscode 23, 24, 25 oder 26
        /// </summary>
        public int? AnzKM2X { get; set; }        //   NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Amtsmeldungen 01 - 05
        /// </summary>
        public int? AnzZEKAM { get; set; }        //    NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Amtsmeldungen 01 - 05 in den letzten 12 Monaten
        /// </summary>
        public int? AnzZEKAML12 { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Eigene Gesuche
        /// </summary>
        public int? AnzEIGENGESUCHE { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Anzahl ZEK Gesuche mit Ablehnungscodes 4, 7 und 9
        /// </summary>
        public int? AnzGAC040709 { get; set; }        //       NUMBER(5,0)
        
        /// <summary>
        /// Zähle die laufenden Engagements in der ZEK, die einen Bonitätscode 04 haben.
        /// </summary>                                                                                                                                                            
        public int? ANZBC04LFD  { get; set; }  //           NUMBER(5)  
        
        /// <summary>
        /// Zähle die saldierten Engagements in der ZEK, die einen Bonitätscode 04 haben.
        /// </summary>                                                                                                                                                               
        public int? ANZBC04SAL { get; set; }  //            NUMBER(5)  

        /// <summary>
        /// Zähle die laufenden Engagements in der ZEK, die einen Bonitätscode 03 haben.
        /// </summary>                                                                                                                                                                 
        public int? ANZBC03LFD { get; set; }  //            NUMBER(5)  

        /// <summary>
        /// Zähle die saldierten Engagements in der ZEK, die einen Bonitätscode 03 haben.
        /// </summary>                                                                                                                                                               
        public int? ANZBC03SAL { get; set; }  //            NUMBER(5)

        /// <summary>
        ///Zähle die laufenden Engagements in der ZEK, die einen Bonitätscode 61 haben.
        /// </summary>
        public int? ANZZEKENGBCODE61LFD { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Zähle die saldierten Engagements in der ZEK, die einen Bonitätscode 61 haben.
        /// </summary>
        public int? ANZZEKENGBCODE61SAL { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Zähle die laufenden Engagements in der ZEK, die einen Bonitätscode 71 haben.
        /// </summary>
        public int? ANZZEKENGBCODE71LFD { get; set; }        //      NUMBER(5,0)

        /// <summary>
        ///  Zähle die saldierten Engagements in der ZEK, die einen Bonitätscode 71 haben.
        /// </summary>
        public int? ANZZEKENGBCODE71SAL { get; set; }        //      NUMBER(5,0)

        /// <summary>
        /// Zähle die von BANK-now abgelehnten Anträge in der ZEK mit einem Ablehnungscode 04, 07, 09 oder 99
        /// </summary>
        public int? ANZZEKGESMABLCODE04070999E         { get; set; }  // NUMBER(5)       
        
        /// <summary>
        /// Zähle die abgelehnten Anträge in der ZEK mit einem Ablehnungscode 04, 07, 09 oder 99, die NICHT von BANK-now abgelehnt wurden.
        /// </summary>                                                                                                                                                     
        public int? ANZZEKGESMABLCODE04070999F     { get; set; }  // NUMBER(5)  
        
        /// <summary>
        /// Zähle die von BANK-now abgelehnten Anträge in der ZEK mit einem Ablehnungscode 13 oder 14
        /// </summary>                                                                                                                                                         
        public int? ANZZEKGESMABLCODE1314E      { get; set; }  // NUMBER(5)   

        /// <summary>
        /// Zähle die abgelehnten Anträge in der ZEK mit einem Ablehnungscode 13 oder 14, die NICHT von BANK-now abgelehnt wurden.
        /// </summary>                                                                                                                                                             
        public int? ANZZEKGESMABLCODE1314F { get; set; }  //  NUMBER(5)

        /// <summary>
        /// Zähle die von BANK-now abgelehnten Anträge in der ZEK mit einem Ablehnungscode 10
        /// </summary>
        public int? ANZZEKGESMABLCODE10E { get; set; }        //       NUMBER(5,0)

        /// <summary>
        /// Zähle die abgelehnten Anträge in der ZEK mit einem Ablehnungscode 10, die NICHT von BANK-now abgelehnt wurden.
        /// </summary>
        public int? ANZZEKGESMABLCODE10F { get; set; } //   NUMBER(5,0)

        /// <summary>
        /// Zähle die Karenmeldungen in der ZEK mit einem Ereigniscode 21 UND einem Positiveintrag
        /// </summary>
        public int? ANZZEKKMELDMCODE21POS { get; set; } //   NUMBER(5,0)   
        
        /// <summary>
        /// Zähle die Karenmeldungen in der ZEK mit einem Ereigniscode 21 OHNE Positiveintrag
        /// </summary>                                                                                                                                                        
        public int? ANZZEKKMELDMCODE21NEG { get; set; } //   NUMBER(5,0) 
        
        /// <summary>
        /// Zähle die Karenmeldungen in der ZEK mit einem Ereigniscode 22 UND einem Positiveintrag
        /// </summary>                                                                                                                                                         
        public int? ANZZEKKMELDMCODE22POS { get; set; } //   NUMBER(5,0)     
        
        /// <summary>
        /// Zähle die Karenmeldungen in der ZEK mit einem Ereigniscode 22 OHNE Positiveintrag
        /// </summary>                                                                                                                                               
        public int? ANZZEKKMELDMCODE22NEG { get; set; } //   NUMBER(5,0)

        /// <summary>
        /// Anzahl Negativmeldungen Vertragsstatus 3,4 und Bonitätscode 3,4,5,6
        /// </summary>
        public int? I_ZEK_NEGEINTRAG { get; set; } //   NUMBER(5,0)

        /// <summary>
        /// Anzahl Positivmeldungen Vertragsstatus 4 und Bonitätscode1,2
        /// </summary>
        public int? I_ZEK_POSEINTRAG { get; set; } //   NUMBER(5,0)


    }
}