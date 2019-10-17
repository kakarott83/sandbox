using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Html Template Dao for accessing files of the form prefix templateid .html
    /// String prefix = FileUtils.getCurrentPath() + "\\..\\resources\\template_";
    /// </summary>
    public class ResourceTemplateDao : IHtmlTemplateDao
    {
        private String prefix;

        public ResourceTemplateDao(String prefix)
        {
            this.prefix = prefix;
        }


        public string getHtmlTemplate(int templateId)
        {
            
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

        public string getHtmlTemplate()
        {
            throw new NotImplementedException();
        }
    }
}
