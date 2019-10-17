using System;
using System.ServiceModel;
using Cic.One.DTO;
using Cic.One.Web.BO;
using Cic.One.Web.Contract;
using Cic.OpenOne.Common.Util.Security;



namespace Cic.One.Web.Service
{
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class printService : IprintService
    {

        /// <summary>
        /// Prints the given html to pdf
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>        
        public ogetDocumentDto printFromHtml(String html)
        {
            ServiceHandler<String, ogetDocumentDto> ew = new ServiceHandler<String, ogetDocumentDto>(html);
            return ew.process(delegate(String input, ogetDocumentDto rval)
            {

                IPrintBo bo = BOFactoryFactory.getInstance().getPrintBo();
                rval.data = bo.htmlToPdf(input);
            });

        }

        /// <summary>
        /// Prints the given data into the given template
        /// </summary>
        /// <param name="iprint"></param>
        /// <returns></returns>
        public ogetDocumentDto printFromTemplate(iprintFromTemplateDto iprint)
        {
            ServiceHandler<iprintFromTemplateDto, ogetDocumentDto> ew = new ServiceHandler<iprintFromTemplateDto, ogetDocumentDto>(iprint);
            return ew.process(delegate(iprintFromTemplateDto input, ogetDocumentDto rval)
            {

                IPrintBo bo = BOFactoryFactory.getInstance().getPrintBo();
                rval.data = bo.templateToPdf(input.data, input.template);
            });
        }

        /// <summary>
        /// prints the given url to a pdf
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ogetDocumentDto printFromURL(String url)
        {
            ServiceHandler<String, ogetDocumentDto> ew = new ServiceHandler<String, ogetDocumentDto>(url);
            return ew.process(delegate(String input, ogetDocumentDto rval)
            {

                IPrintBo bo = BOFactoryFactory.getInstance().getPrintBo();
                rval.data = bo.urlToPdf(new Uri(input));
            });
        }

        /// <summary>
        /// prints a document via eai
        /// </summary>
        /// <param name="iprint"></param>
        /// <returns></returns>
        public ogetDocumentDto printDocument(iprintDocumentDto iprint)
        {
            ServiceHandler<iprintDocumentDto, ogetDocumentDto> ew = new ServiceHandler<iprintDocumentDto, ogetDocumentDto>(iprint);
            return ew.process(delegate(iprintDocumentDto input, ogetDocumentDto rval, CredentialContext ctx)
            {

                IPrintBo bo = BOFactoryFactory.getInstance().getPrintBo();
                rval.document = bo.printDocument(input, ctx.getMembershipInfo().sysWFUSER);
            });
        }

        /// <summary>
        /// Returns the list of configured documents for the document set code
        /// </summary>
        /// <param name="docCode"></param>
        /// <returns></returns>
        public ogetDocumentListDto getDocumentList(String docCode)
        {
            ServiceHandler<String, ogetDocumentListDto> ew = new ServiceHandler<String, ogetDocumentListDto>(docCode);
            return ew.process(delegate(String input, ogetDocumentListDto rval)
            {

                IPrintBo bo = BOFactoryFactory.getInstance().getPrintBo();
                rval.documents = bo.getDocumentList(input);
            });

        }
    }
}
