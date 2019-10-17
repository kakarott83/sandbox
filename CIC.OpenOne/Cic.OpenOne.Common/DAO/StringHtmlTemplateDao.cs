using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DAO
{

    /// <summary>
    /// Implementierung eines HtmlTemplates welches immer das selbe Template zurück gibt (welches im Konstruktor übergeben wird)
    /// </summary>
    public class StringHtmlTemplateDao : IHtmlTemplateDao
    {
        string template;
        /// <summary>
        /// Erzeugt ein Template, welches immer das Selbe Template zurück gibt.
        /// </summary>
        /// <param name="template">template welches zurück gegeben werden soll</param>
        public StringHtmlTemplateDao(string template)
        {
            this.template = template;
        }

        /// <summary>
        /// Gibt das Template zurück, welches bei dem Konstruktor übergeben wurde
        /// </summary>
        /// <returns></returns>
        public string getHtmlTemplate()
        {
            return template;
        }


        public string getHtmlTemplate(int templateId)
        {
            return getHtmlTemplate();
        }
    }
}
