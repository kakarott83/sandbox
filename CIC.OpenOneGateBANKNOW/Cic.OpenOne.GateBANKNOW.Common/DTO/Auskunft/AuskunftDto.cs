using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    using Crif;
    using Schufa;

    /// <summary>
    /// Interne Auskunft-FehlerCodes
    /// Code > 0 -> Fehlercode aus externem Dienst
    /// </summary>
    public enum AuskunftErrorCode
    {
        /// <summary>
        /// Erfolgreiche Verarbeitung
        /// </summary>
        NoError = 0,

        /// <summary>
        /// Externer Dienst nicht erreichbar (technischer Fehler externer Dienstleister)
        /// </summary>
        ErrorService = -1,

        /// <summary>
        /// Interner technischer Verarbeitungsfehler (technischer Fehler CIC)
        /// </summary>
        ErrorCIC = -2,

        /// <summary>
        /// Batch-Request an ZEK gesendet (bereit für batchStatus-Aufruf)
        /// </summary>
        BatchRequestSent = -5
    }

    /// <summary>
    /// Information Data Access Object
    /// </summary>
    [System.CLSCompliant(false)]
    public class AuskunftDto : Cic.OpenOne.AuskunftManagement.Common.DTO.AuskunftBaseDto
    {
       


        /// <summary>
        /// Getter/Setter Aggregation Input Data Structure
        /// </summary>
        public AggregationInDto AggregationInDto { get; set; }

        /// <summary>
        /// Getter/Setter Aggregation OutputData Structure
        /// </summary>
        public AggregationOutDto AggregationOutDto { get; set; }

        /// <summary>
        /// Getter/Setter KREMO Input Data Structure
        /// </summary>
        public KREMOInDto KremoInDto { get; set; }

        /// <summary>
        /// Getter/Setter KREMO OutputData Structure
        /// </summary>
        public KREMOOutDto KremoOutDto { get; set; }

        /// <summary>
        /// Getter/Setter DeltaVista Input Data Structure
        /// </summary>
        public DeltavistaInDto DeltavistaInDto { get; set; }

        /// <summary>
        /// Getter/Setter DeltaVista OutputData Structure
        /// </summary>
        public DeltavistaOutDto DeltavistaOutDto { get; set; }

        /// <summary>
        /// Getter/Setter Eurotax Inout Data Structure
        /// </summary>
        public EurotaxInDto EurotaxInDto { get; set; }

        /// <summary>
        /// Getter/Setter Eurotax Output Data Structure
        /// </summary>
        public EurotaxOutDto EurotaxOutDto { get; set; }

        /// <summary>
        /// Getter/Setter Decision Engine Input Data Structure
        /// </summary>
        public DecisionEngineInDto DecisionEngineInDto { get; set; }

        /// <summary>
        /// Getter/Setter Decision Engine Output Data Structure
        /// </summary>
        public DecisionEngineOutDto DecisionEngineOutDto { get; set; }

        /// <summary>
        /// Getter/Setter ZEK Input Data Structure
        /// </summary>
        public ZekInDto ZekInDto { get; set; }

        /// <summary>
        /// Getter/Setter ZEK Output Data Structure
        /// </summary>
        public ZekOutDto ZekOutDto { get; set; }

        /// <summary>
        /// Getter/Setter ZEK-Batch Output Data Structure
        /// </summary>
        public ZekBatchOutDto ZekBatchOutDto { get; set; }

        /// <summary>
        /// Getter/Setter Record RR Data Structure
        /// </summary>
        public RecordRRDto RecordRRDto { get; set; }

        /// <summary>
        /// SchufaInDto
        /// </summary>
        public SchufaInDto SchufaInDto { get; set; }

        /// <summary>
        /// SchufaOutDto
        /// </summary>
        public SchufaOutDto SchufaOutDto { get; set; }

        public CrifInDto CrifInDto { get; set; }

        public CrifOutDto CrifOutDto { get; set; }
    }
}