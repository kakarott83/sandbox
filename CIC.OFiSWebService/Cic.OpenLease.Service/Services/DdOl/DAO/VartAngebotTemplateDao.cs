using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenLease.Service.Services.DdOl.DAO
{
    public class VartAngebotTemplateDao : IHtmlTemplateDao
    {
        private String getTemplateName(int sysvart)
        {
            /*
            * 41 = Kontokorrentkredit
            * 39 = Ratenkredit
            * 44 = Restwertleasing
            * 42 = Nutzenleasing
            * 52 = Selectleasing
            * 49 = Service Standalone
            * 54 = Versicherung Standalone
            */

            if (sysvart == -1)
                return "vertragsuebersicht";

            
               
            else if (sysvart == 12)
                return "ballonkredit";
            else if (sysvart == 13)
                return "kilometerleasing";
            else if (sysvart == 14)
                return "restwertleasing";
            else if (sysvart == 15)
                return "ratenkredit";

            return "3wegekredit";//11 and others

        }

        public string getHtmlTemplate(int templateId)
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


        public string getHtmlTemplateString(string templateId)
        {
            throw new NotImplementedException();
        }
    }
}