using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.OpenLease.Service.Services.DdOl.DTO;
using Cic.OpenLease.Service.Services.DdOl.DAO;
using Cic.OpenLease.Service.Services.DdOl.BO;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenLease.Service.Services.DdOl.Contract;
using Cic.OpenLease.Service.DdOl;

namespace Cic.OpenLease.Service.Services.DdOl
{
   
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class HtmlReportService : IHtmlReportService
    {
      
        /// <summary>
        /// Erzeugt einen HtmlReport
        /// </summary>
        /// <param name="input">icreateHtmlReportANGEBOTDto</param>
        /// <returns>ocreateHtmlReportDto</returns>
        public ocreateHtmlReportDto CreateHtmlReportANGEBOT(icreateHtmlReportANGEBOTDto input)
        {
            return CreateHtmlReportGeneric(input);
        }


        /// <summary>
        /// Erzeugt einen HtmlReport
        /// </summary>
        /// <param name="input">icreateHtmlReportDto</param>
        /// <returns>ocreateHtmlReportDto</returns>
        public DTO.ocreateHtmlReportDto CreateHtmlReportGeneric<T>(DTO.icreateHtmlReportDto<T> input)
        {
            DTO.ocreateHtmlReportDto rval = new DTO.ocreateHtmlReportDto();

            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                if (input.Data == null)
                    throw new ArgumentException("No Data received");

                HtmlReportBo htmlReportBo = new HtmlReportBo(new DAO.ResourceTemplateDao());
                htmlReportBo.CreateHtmlReport(input.Data, ""+input.HtmlTemplateid);
                
                rval.success();
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

       

    }
}
