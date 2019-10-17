using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.DTO;

namespace Cic.One.Web.BO
{
    public abstract class AbstractPrintBo : IPrintBo
    {
        /// <summary>
        /// Converts the html to a PDF byte array
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public abstract byte[] htmlToPdf(String html);

        /// <summary>
        /// Converts the given url to a pdf
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public abstract byte[] urlToPdf(Uri url);

        /// <summary>
        /// Fills the html-template with the data and converts to PDF
        /// </summary>
        /// <param name="data"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public abstract byte[] templateToPdf(object data, string template);

        /// <summary>
        /// Prints a document via EAI
        /// </summary>
        /// <param name="iprint"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public abstract EaihotDto printDocument(iprintDocumentDto iprint, long syswfuser);

        /// <summary>
        /// Returns a list of documents for the document Area Code
        /// </summary>
        /// <param name="docCode"></param>
        /// <returns></returns>
        public abstract List<PrintDocumentDto> getDocumentList(string docCode);

        /// <summary>
        /// Returns a list of documentsareas
        /// </summary>
        /// <returns></returns>
        public abstract List<String> getDocumentAreas();
    }
}
