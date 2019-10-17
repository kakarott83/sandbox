using System;
using System.ComponentModel;
using XmlConfiguratorBase.BO.GUI;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace XmlConfiguratorBase.DTO
{
    /// <summary>
    /// A view configuration object, based on class WfvConfigEntry
    /// The original class WfvConfigEntry cannot be used because "einrichtung" is a string there, but in reality it's an object (represented by an xml string).
    /// </summary>
    public class WfvConfigEntryDto
    {
        [Description("Eindeutiger Code des Elements")]
        public String syscode { get; set; }

        [Description("Elementtyp")]
        [ItemsSource(typeof(EntrytypeItemsSource))]
        public String entrytype { get; set; }

        [Description("Typ der Elementdetails")]
        [ItemsSource(typeof(EnumStringItemsSource<CommandLine>))]
        public String befehlszeile { get; set; }

        [Description("Elementdetails")]
        [ExpandableObject()]
        [Editor(typeof(NewInstanceEditor), typeof(NewInstanceEditor))]
        public object einrichtung { get; set; }

        [Description("Beschreibung des Elements")]
        public String beschreibung { get; set; }
    }
}
