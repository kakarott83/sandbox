using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenLease.Service.Services.DdOl.DAO
{
    public class AngebotTemplateDao : IHtmlTemplateDao
    {
        private String getTemplateName(int syskalktyp)
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

            if (syskalktyp == -1)//Vertragsübersicht
                return "vertragsuebersicht";

            if (syskalktyp == 39)
                return "kredit_ratenkredit";
            else if (syskalktyp == 40)
                return "kredit_selectkredit";
            else if (syskalktyp == 50)
                return "kredit_zielratenkredit";
            else if (syskalktyp == 44)
                return "leasing_restwertleasing";
            else if (syskalktyp == 42)
                return "leasing_nutzenleasing";
            else if (syskalktyp == 40)
                return "leasing_selectleasing";
            return "leasing_selectleasing"; 

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
            return getHtmlTemplate(int.Parse(templateId));
        }
    }
}