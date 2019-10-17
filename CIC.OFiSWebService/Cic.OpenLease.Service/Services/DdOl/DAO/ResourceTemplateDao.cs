using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenLease.Service.Services.DdOl.DAO
{
    public class ResourceTemplateDao : IHtmlTemplateDao
    {
        public string getHtmlTemplate(int templateId)
        {
            String prefix = FileUtils.getCurrentPath() + "\\..\\resources\\template_";
            try
            {
                byte[] data = FileUtils.loadData(prefix + templateId + ".html");
                return System.Text.Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading Template: " + ex.Message, ex);
            }
        }


        public string getHtmlTemplateString(string templateId)
        {
            return getHtmlTemplate(int.Parse(templateId));
        }
    }
}