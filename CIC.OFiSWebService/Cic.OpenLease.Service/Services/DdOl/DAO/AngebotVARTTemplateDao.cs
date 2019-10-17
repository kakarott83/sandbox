using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenLease.Service.Services.DdOl.DAO
{
    public class AngebotVARTTemplateDao : IHtmlTemplateDao
    {
        private String getTemplateName(String templateId)
        {
            /*
            * 10	HEK_FIN
11	KREDIT_3WEGEFIN
12	KREDIT_BALLON
13	LEASING_KILOMETER
14	LEASING_RESTWERT
15	KREDIT_RATEN*/
            if (templateId == null || templateId.Length == 0)
                return "kredit_ballon";
            return templateId.ToLower();
        }

        public string getHtmlTemplate(int templateId)
        {

            String prefix = FileUtils.getCurrentPath() + "\\..\\resources\\template_";
            try
            {
                byte[] data = FileUtils.loadData(prefix + getTemplateName(""+templateId) + ".html");
                return System.Text.Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading Template: " + ex.Message, ex);
            }
        }

        public string getHtmlTemplateString(string templateId)
        {

            String prefix = FileUtils.getCurrentPath() + "\\..\\resources\\template_";
            try
            {
                byte[] data = FileUtils.loadData(prefix + getTemplateName(templateId) + ".html");
                return System.Text.Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading Template: " + ex.Message, ex);
            }
        }
     
    }
}