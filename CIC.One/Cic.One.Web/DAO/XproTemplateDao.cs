using Cic.OpenOne.Common.DAO;

namespace Cic.One.Web.DAO
{
    public class XproTemplateDao : IHtmlTemplateDao
    {
        private XproInfoBaseDao info;

        public XproTemplateDao(XproInfoBaseDao info)
        {
            this.info = info;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="templateId">Für Template Id 0 wird das simple Template (für die Bezeichnung) verwendet und sonst das komplexe</param>
        /// <returns></returns>
        public string getHtmlTemplate(int templateId)
        {
            if (templateId == 0)
                return LoadTemplateBezeichnung();
            else
                return LoadTemplateBeschreibung();
        }

        private string LoadTemplateBeschreibung()
        {
            return info.TemplateBeschreibung;
        }

        private string LoadTemplateBezeichnung()
        {
            //TODO aus der Datenbank laden
            return info.TemplateBezeichnung;
        }


        public string getHtmlTemplate()
        {
            throw new System.NotImplementedException();
        }
    }
}