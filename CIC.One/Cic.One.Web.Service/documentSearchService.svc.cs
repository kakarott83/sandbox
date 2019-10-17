using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.Web.Contract;
using Cic.One.Web.BO;
using Cic.One.Web.BO.Search;
using Cic.One.Web.DAO;
using Cic.One.DTO;

namespace Cic.One.Web.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "documentSearchService" in code, svc and config file together.

    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class documentSearchService : IdocumentSearchService
    {
        /// <summary>
        /// Sucht nach Dokumenten
        /// </summary>
        /// <param name="dynamicSearch">Parameter</param>
        /// <returns>Infos zu den gefundenen Dokumenten</returns>
        public oDynamicDocumentSearchDto DynamicDocumentSearch(iDynamicDocumentSearchDto dynamicSearch)
        {
            ServiceHandler<iDynamicDocumentSearchDto, oDynamicDocumentSearchDto> ew = new ServiceHandler<iDynamicDocumentSearchDto, oDynamicDocumentSearchDto>(dynamicSearch);
            return ew.process(delegate(iDynamicDocumentSearchDto input, oDynamicDocumentSearchDto rval)
            {
                if (input == null)
                    throw new ArgumentException("No valid input");

                IDocumentSearchBo bo = BOFactoryFactory.getInstance().getDocumentSearchBO();
                rval.Hitlist = bo.DynamicDocumentSearch(input);
            });
        }


        /// <summary>
        /// Lädt ein Dokument anhand dem Input
        /// </summary>
        /// <param name="docLoad"></param>
        /// <returns>Enthält das Dokument</returns>
        public oDocumentLoadDto DocumentLoad(iDocumentLoadDto docLoad)
        {
            ServiceHandler<iDocumentLoadDto, oDocumentLoadDto> ew = new ServiceHandler<iDocumentLoadDto, oDocumentLoadDto>(docLoad);
            return ew.process(delegate(iDocumentLoadDto input, oDocumentLoadDto rval)
            {
                if (input == null || input.Docid == null)
                    throw new ArgumentException("No valid input");

                IDocumentSearchBo bo = BOFactoryFactory.getInstance().getDocumentSearchBO();
                rval.Result = bo.DocumentLoad(input);
            });
        }


        /// <summary>
        /// Liefert die Version der ITA WebSearch zurück
        /// </summary>
        /// <param name="info"></param>
        /// <returns>Information</returns>
        public ogetVersionInfo getDocumentSearchVersionInfo(igetVersionInfo info)
        {
            ServiceHandler<igetVersionInfo, ogetVersionInfo> ew = new ServiceHandler<igetVersionInfo, ogetVersionInfo>(info);
            return ew.process(delegate(igetVersionInfo input, ogetVersionInfo rval)
            {
                if (input == null)
                    throw new ArgumentException("No valid input");

                IDocumentSearchBo bo = BOFactoryFactory.getInstance().getDocumentSearchBO();
                ogetVersionInfo result = bo.getVersionInfo(input);
                rval.Copyright = result.Copyright;
                rval.ITA_Frameware = result.ITA_Frameware;
                rval.Version = result.Version;
                rval.WFL_Frameware = result.WFL_Frameware;
            });
        }
    }
}
