
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System.ServiceModel;
namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface IAuskunftService stellt die Schnittstelle für alle extern angebundenen Auskunftsdienste zur Verfügung, z.B. 
    /// 
    /// Eurotax|Mail|ZEK|Deltavista|Kremo
    /// 
    /// </summary>
    [ServiceContract(Name = "IDecisionService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IDecisionService
    {
       
        /// <summary>
        /// Guardean Decision Engine Result Interface
        /// </summary>
        /// <param name="input"></param>
        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
        osetDecisionResultDto setCreditDecisionResult(isetDecisionResultDto input);

        /// <summary>
        /// Guardean aggregation Interface
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
        getAggregationDto getAggregation(igetAggregationDto input);
        
        /// <summary>
        /// returns a certain Document. This method is only implemented for SHS MUW.
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
        ogetDocumentDto getDocument(long sysdmsdoc);

        /// <summary>
        /// INT 6: Guardean liability chain Interface
        /// Returns all KNE that are contained in the liability chain of the corresponding person.
        /// First we search for all GvK A/B and
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
        ogetLiabilityChainDto getLiabilityChain(igetLiabilityChainDto input);

        /// <summary>
        /// INT 7: Sets the customer check result.
        /// This was removed from INT 2 and put into it's own API.
        /// It is used for setting the customer check result for mainApplicant, CoApplicant, Guarantor, authorizedRepresentative, UBO, CBUProspect, CEO
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
        osetCustomerCheckResult setCustomerCheckResult(isetCustomerCheckResult input);
    }
}
