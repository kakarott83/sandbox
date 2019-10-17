using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.One.DTO;


namespace Cic.One.Web.Contract
{
    /// <summary>
    /// Methods for delivering a certain entity with all its data
    /// </summary>
    [ServiceContract(Name = "IprintService", Namespace = "http://cic-software.de/One")]
    public interface IprintService
    {

        /// <summary>
        /// Prints the given html to pdf
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>        
        [OperationContract]
        ogetDocumentDto printFromHtml(String html);

        /// <summary>
        /// Prints the given data into the given template
        /// </summary>
        /// <param name="iprint"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDocumentDto printFromTemplate(iprintFromTemplateDto iprint);

         /// <summary>
        /// prints the given url to a pdf
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDocumentDto printFromURL(String url);

        /// <summary>
        /// prints a document via eai
        /// </summary>
        /// <param name="iprint"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDocumentDto printDocument(iprintDocumentDto iprint);

        /// <summary>
        /// Returns the list of configured documents for the document set code
        /// </summary>
        /// <param name="docCode"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDocumentListDto getDocumentList(String docCode);
    }
}
