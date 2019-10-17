using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Utils.Util.Config
{
    public class ConfigUtil
    {
        /// <summary>
        /// Returns a certain config from the web.config like
        /// section = "applicationSettings/Cic.OpenOne.Common.Properties.Config"
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Setting"></param>
        /// <returns></returns>
        public static string GetCustomSetting(string Section, string Setting)
        {
            var config = System.Configuration.ConfigurationManager.GetSection(Section);

            if (config != null)
                return ((System.Configuration.ClientSettingsSection)config).Settings.Get(Setting).Value.ValueXml.InnerText;

            return string.Empty;
        }
    }
}
