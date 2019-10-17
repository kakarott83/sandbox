using AutoMapper;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Serialization;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WfvXmlConfigurator.BO.GUI;
using WfvXmlConfigurator.DTO;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace WfvXmlConfigurator.BO.ContentLogics
{
    public class ContentManager
    {
        private static Dictionary<string, EntryType> syscodes = new Dictionary<string, EntryType>();
        private static HashSet<string> ConfigEntrySyscodes = new HashSet<string>();

        private WfvConfig ContentData = null;
        private List<WfvConfigEntryDto> ConfigEntryList = null;

        /// <summary>
        /// Data conversion and attribute manager.
        /// Provides the lists for working with given data and adds information to class properties
        /// </summary>
        public ContentManager ()
        {
            //init automatic mapping

            Mapper.CreateMap<WfvConfigEntry, WfvConfigEntryDto>();
            Mapper.CreateMap<WfvConfigEntryDto, WfvConfigEntry>();
            Mapper.CreateMap<WfvConfigEntry, WfvEntry>()
                .ForMember(dest => dest.entrytype, opt => opt.MapFrom(entry => int.Parse(entry.entrytype)))
                .ForAllMembers(opt => opt.Condition(src => src.DestinationValue == null && !src.IsSourceValueNull))
                ;


            //Add Property Attributes for PropertyGrid usage

            PropertyAttributeAdder<WfvEntry> propAttributeAdderEntry = new PropertyAttributeAdder<WfvEntry>();
            propAttributeAdderEntry.AddAttributes("syscode", new DescriptionAttribute("Eindeutiger Bezeichner für das Objekt"));
            propAttributeAdderEntry.AddAttributes("entrytype", new ItemsSourceAttribute(typeof(EntrytypeItemsSource)), new DescriptionAttribute("Typ der View 0=Liste, 1=Detail, 2=Dashboard mit wfvrefs, 3=Wizard"));
            propAttributeAdderEntry.AddAttributes("befehlszeile", new DescriptionAttribute("WORKFLOW, CAS, OLSTART, DASHBOARD, ..."));
            propAttributeAdderEntry.AddAttributes("customentry", new ExpandableObjectAttribute(), new EditorAttribute(typeof(NewInstanceEditor), typeof(NewInstanceEditor)), new DefaultValueAttribute(new CustomEntry()), new DescriptionAttribute("Detailinformationen zur Datenanzeige"));
            propAttributeAdderEntry.AddAttributes("references", new DescriptionAttribute("Im Dashboard oder Wizard enthaltene Teilelemente"));
            propAttributeAdderEntry.AddAttributes("coderfu", new DescriptionAttribute("Funktionsberechtigungscode"));
            propAttributeAdderEntry.AddAttributes("codermo", new DescriptionAttribute("Modulberechtigungscode"));
            propAttributeAdderEntry.AddAttributes("wizardtype", new DescriptionAttribute("Typ des Wizards. 0: Beliebig navigierbar, 1: Benutzer muss den Wizard von vorne nach hinten durchgehen"));
            propAttributeAdderEntry.ProvideAttributes();

            PropertyAttributeAdder<CustomEntry> propAttributeAdderCustomentry = new PropertyAttributeAdder<CustomEntry>();
            propAttributeAdderCustomentry.AddAttributes("detailsyscode", new ItemsSourceAttribute(typeof(DetailDashboardSyscodesItemsSource)), new DescriptionAttribute("Eindeutiger Bezeichner der Detailansicht"));
            propAttributeAdderCustomentry.AddAttributes("createsyscode", new ItemsSourceAttribute(typeof(DetailDashboardSyscodesItemsSource)), new DescriptionAttribute("Eindeutiger Bezeichner der Maske zum Erstellen eines neuen Objekts"));
            propAttributeAdderCustomentry.AddAttributes("forwardsyscode", new ItemsSourceAttribute(typeof(AllSyscodesItemsSource)), new DescriptionAttribute("Eindeutiger Bezeichner der Maske, zu der man nach dem Speichern weitergeleitet wird"));
            propAttributeAdderCustomentry.AddAttributes("title", new DescriptionAttribute("Bezeichnung der Maske"));
            propAttributeAdderCustomentry.AddAttributes("icon", new DescriptionAttribute("Symbol"));
            propAttributeAdderCustomentry.AddAttributes("filter", new DescriptionAttribute("Filter-Codes (durch Komma getrennt), die standardmäßig aktiviert sind. Nur Filter-Codes, die im korrespondierenden java-BO implementiert sind, werden unterstützt. Standardwerte: OWNER, NONE"));
            propAttributeAdderCustomentry.AddAttributes("filterfields", new DescriptionAttribute("Durch Komma getrennte Felder aus der searchQueryInfofactory-Abfrage, um nach benutzerdefinierten Werten zu suchen"));
            propAttributeAdderCustomentry.AddAttributes("internalfilter", new DescriptionAttribute("Dieses SQL wird an alle Listenabfragen als interner Filter angehängt"));
            propAttributeAdderCustomentry.AddAttributes("sortfields", new DescriptionAttribute("Semicolon(!)getrennte Felder, nach denen initial sortiert sein soll"));
            propAttributeAdderCustomentry.AddAttributes("sortorder", new DescriptionAttribute("ASC für aufsteigende Sortierung, DESC für absteigende Sortierung. Durch Komma getrennt."));
            propAttributeAdderCustomentry.AddAttributes("filters", new DescriptionAttribute("Definition für die in der GUI zur Verfügung gestellten Filter, um die Anzahl der Listeneinträge zu reduzieren"));
            propAttributeAdderCustomentry.AddAttributes("viewmeta", new ExpandableObjectAttribute(), new EditorAttribute(typeof(NewInstanceEditor), typeof(NewInstanceEditor)), new DescriptionAttribute("GUI Layout Beschreibung. Z.B. Selbstgebaute SQL-Abfrage für Elementauswahl"));
            propAttributeAdderCustomentry.ProvideAttributes();

            PropertyAttributeAdder<Filter> propAttributeAdderFilter = new PropertyAttributeAdder<Filter>();
            propAttributeAdderFilter.AddAttributes("fieldname", new DescriptionAttribute("Name des Feldes, nach dem gefiltert wird"));
            propAttributeAdderFilter.AddAttributes("value", new DescriptionAttribute("Wert, nach dem das Feld gefiltert wird"));
            propAttributeAdderFilter.AddAttributes("filterType", new DescriptionAttribute("Art des Filters"));
            propAttributeAdderFilter.AddAttributes("groupName", new DescriptionAttribute("Gruppenname. Alle Filter in der gleichen Gruppe werden in einer Klammer zusammengefasst ( a=1 OR b=2 ). In der GUI werden alle Filter der gleichen Gruppe zu einem Input-Element kombiniert (bevorzugt MULTISELECT)"));
            propAttributeAdderFilter.AddAttributes("orFilter", new DescriptionAttribute("Flag, das kennzeichnet ob der Gruppenname als or-Gruppe behandelt werden muss"));
            propAttributeAdderFilter.AddAttributes("andFilter", new DescriptionAttribute("Flag, das kennzeichnet ob der Gruppenname als and-Gruppe behandelt werden muss"));
            propAttributeAdderFilter.AddAttributes("values", new EditorAttribute(typeof(StringArrayEditor), typeof(StringArrayEditor)), new DescriptionAttribute("Werte für OR-/AND-Filterung, mit denen verschiedene Werte gruppiert mit einem Filterelement verglichen werden können. Man kann auch einen anderen Filter der gleichen Gruppe mit anderen Werten verwenden"));
            propAttributeAdderFilter.AddAttributes("groupFields", new EditorAttribute(typeof(StringArrayEditor), typeof(StringArrayEditor)), new DescriptionAttribute("Werte der Feldnamen in OR-Filtern, mit denen mehrere Feldnamen gruppiert mit einem Filterobjekt verglichen werden können. Man kann auch einen anderen Filter der gleichen Gruppe mit anderen Werten verwenden"));
            propAttributeAdderFilter.AddAttributes("xprocode", new ItemsSourceAttribute(typeof(EnumStringItemsSource<XproEntityType>)), new DescriptionAttribute("Typ der XPRO Combobox Listen, der die Comboboxelemente definiert"));
            propAttributeAdderFilter.AddAttributes("optionvalues", new DescriptionAttribute("Durch Komma getrennte Liste von Name:Wert Paaren für eine Selektionsliste"));
            propAttributeAdderFilter.AddAttributes("description", new DescriptionAttribute("GUI Beschreibung"));
            propAttributeAdderFilter.AddAttributes("valueType", new DescriptionAttribute("Typ des Werts für den GUI Konvertierer"));
            propAttributeAdderFilter.AddAttributes("fieldType", new DescriptionAttribute("Typ des GUI Eingabefelds"));
            propAttributeAdderFilter.AddAttributes("enabled", new DescriptionAttribute("Aktiv ja/nein. Gibt an, ob der Filter standardmäßig aktiv ist"));
            propAttributeAdderFilter.AddAttributes("locked", new DescriptionAttribute("Gesperrt ja/nein. Gibt an, ob der Filter aktiviert oder deaktiviert werden kann"));
            propAttributeAdderFilter.AddAttributes("triggerxpro", new DescriptionAttribute("Sorgt dafür, dass ein anderer Filter mit dem gegebenen xpro mithilfe der Werte dieses Filters aktualisiert wird"));
            propAttributeAdderFilter.ProvideAttributes();

            PropertyAttributeAdder<ViewMeta> propAttributeAdderViewMeta = new PropertyAttributeAdder<ViewMeta>();
            propAttributeAdderViewMeta.AddAttributes("query", new ExpandableObjectAttribute(), new EditorAttribute(typeof(NewInstanceEditor), typeof(NewInstanceEditor)), new DescriptionAttribute("SQL-Anfrage"));
            propAttributeAdderViewMeta.AddAttributes("tables", new DescriptionAttribute("Relevante Datenbanktabellen"));
            propAttributeAdderViewMeta.AddAttributes("fields", new DescriptionAttribute("Relevante Datenbankfelder"));
            propAttributeAdderViewMeta.ProvideAttributes();


            PropertyAttributeAdder<Viewfield> propAttributeAdderViewfield = new PropertyAttributeAdder<Viewfield>();
            propAttributeAdderViewfield.AddAttributes("id", new DescriptionAttribute("Eindeutige Identifizierung"));
            propAttributeAdderViewfield.AddAttributes("attr", new ExpandableObjectAttribute(), new EditorAttribute(typeof(NewInstanceEditor), typeof(NewInstanceEditor)), new DescriptionAttribute("GUI Feld Definition"));
            propAttributeAdderViewfield.AddAttributes("value", new ExpandableObjectAttribute(), new EditorAttribute(typeof(NewInstanceEditor), typeof(NewInstanceEditor)), new DescriptionAttribute("Wert des generischen Felds"));
            propAttributeAdderViewfield.ProvideAttributes();

            PropertyAttributeAdder<ViewValue> propAttributeAdderViewValue = new PropertyAttributeAdder<ViewValue>();
            propAttributeAdderViewValue.AddAttributes("l", new DescriptionAttribute("Ganze Zahl (groß)"));
            propAttributeAdderViewValue.AddAttributes("d", new DescriptionAttribute("Gleitkommazahl"));
            propAttributeAdderViewValue.AddAttributes("t", new DescriptionAttribute("Datum und Uhrzeit"));
            propAttributeAdderViewValue.AddAttributes("i", new DescriptionAttribute("Ganze Zahl (klein)"));
            propAttributeAdderViewValue.AddAttributes("s", new DescriptionAttribute("Text"));
            propAttributeAdderViewValue.AddAttributes("data", new EditorAttribute(typeof(TextBoxEditor), typeof(TextBoxEditor)), new DescriptionAttribute("Datenbytes"));
            propAttributeAdderViewValue.ProvideAttributes();

            PropertyAttributeAdder<ViewFieldAttributes> propAttributeAdderViewfieldAttributes = new PropertyAttributeAdder<ViewFieldAttributes>();
            propAttributeAdderViewfieldAttributes.AddAttributes("field", new DescriptionAttribute("Query ALIAS Or DB Column"));
            propAttributeAdderViewfieldAttributes.AddAttributes("label", new DescriptionAttribute("GUI Label"));
            propAttributeAdderViewfieldAttributes.AddAttributes("type", new ItemsSourceAttribute(typeof(EnumStringItemsSource<InternalType>)), new DescriptionAttribute("Internal Type (String, Long, DateTime, Int, Double, Byte)"));
            propAttributeAdderViewfieldAttributes.AddAttributes("viewtype", new ItemsSourceAttribute(typeof(EnumStringItemsSource<ViewType>)), new DescriptionAttribute("GUI Typ (text,boolean,time,date,currency,separator,number,xpro,headline)"));
            propAttributeAdderViewfieldAttributes.AddAttributes("ro", new DescriptionAttribute("read-only, schreibgeschützt"));
            propAttributeAdderViewfieldAttributes.AddAttributes("minimum", new DescriptionAttribute("Zahlenminimum"));
            propAttributeAdderViewfieldAttributes.AddAttributes("maximum", new DescriptionAttribute("Zahlenmaximum"));
            propAttributeAdderViewfieldAttributes.AddAttributes("pattern", new DescriptionAttribute("Zahlendarstellungsformat"));
            propAttributeAdderViewfieldAttributes.AddAttributes("suffix", new DescriptionAttribute("Feldsuffix"));
            propAttributeAdderViewfieldAttributes.AddAttributes("req", new DescriptionAttribute("required field, Pflichtfeld"));
            propAttributeAdderViewfieldAttributes.AddAttributes("code", new ItemsSourceAttribute(typeof(EnumStringItemsSource<XproEntityType>)), new DescriptionAttribute("XPRO Code"));
            propAttributeAdderViewfieldAttributes.ProvideAttributes();

            PropertyAttributeAdder<WfvRef> propAttributeAdderReferences = new PropertyAttributeAdder<WfvRef>();
            propAttributeAdderReferences.AddAttributes("syscode", new ItemsSourceAttribute(typeof(DetailListSyscodesItemsSource)), new DescriptionAttribute("Eindeutiger Bezeichner der eingebetteten Maske"));
            propAttributeAdderReferences.AddAttributes("column", new DescriptionAttribute("Spalte"));
            propAttributeAdderReferences.AddAttributes("row", new DescriptionAttribute("Zeile"));
            propAttributeAdderReferences.AddAttributes("visibility", new DescriptionAttribute("Sichtbarkeit"));
            propAttributeAdderReferences.AddAttributes("collapsed", new DescriptionAttribute("Maske eingeklappt"));
            propAttributeAdderReferences.AddAttributes("popup", new DescriptionAttribute("Maske nur als Popup anzeigen"));
            propAttributeAdderReferences.AddAttributes("mainref", new DescriptionAttribute("Die Maske ist die Hauptmaske des Dashboards"));
            propAttributeAdderReferences.AddAttributes("filter", new ExpandableObjectAttribute(), new EditorAttribute(typeof(NewInstanceEditor), typeof(NewInstanceEditor)), new DescriptionAttribute("Filter für die Filterung einer Liste dieser Maske mit den gegebenen Kriterien eines anderen Dashboardelements"));
            propAttributeAdderReferences.AddAttributes("customentry", new ExpandableObjectAttribute(), new EditorAttribute(typeof(NewInstanceEditor), typeof(NewInstanceEditor)), new DescriptionAttribute("Erlaubt das Überschreiben der eigentlichen wfv-Definition"));
            propAttributeAdderReferences.ProvideAttributes();

            PropertyAttributeAdder<RefFilter> propAttributeAdderRefFilter = new PropertyAttributeAdder<RefFilter>();
            propAttributeAdderRefFilter.AddAttributes("field", new DescriptionAttribute("SQL Filterfeld"));
            propAttributeAdderRefFilter.AddAttributes("wfvref", new DescriptionAttribute("WFVREF ID aus dem gleichen Dashboard, die als Quelle für den Filter dient"));
            propAttributeAdderRefFilter.AddAttributes("reffield", new DescriptionAttribute("Dto-Feld des WFVREF Objekts"));
            propAttributeAdderRefFilter.ProvideAttributes();
        }

        /// <summary>
        /// Clear current data, discard changes
        /// </summary>
        public void ResetData()
        {
            syscodes.Clear();
            ConfigEntrySyscodes.Clear();
            ContentData = null;
            ConfigEntryList = null;
        }

        /// <summary>
        /// Overwrite current data, discard changes
        /// </summary>
        public void SetData(WfvConfig data)
        {
            CalculateNewItems(data, true);
        }

        /// <summary>
        /// Add given data to existing data
        /// </summary>
        public void AddData(WfvConfig data)
        {
            CalculateNewItems(data, false);
        }

        /// <summary>
        /// Merge existing data with given data
        /// </summary>
        /// <param name="data">new data</param>
        /// <param name="overwrite">overwrite existing items with new items</param>
        private void CalculateNewItems(WfvConfig data, bool overwrite)
        {
            if (ContentData == null)
                ContentData = data;
            else if (data != null)
            {
                if (ContentData.entries == null)
                    ContentData.entries = new List<WfvEntry>();
                if (data.entries != null)
                {
                    foreach (WfvEntry entry in data.entries)
                    {
                        if (syscodes.ContainsKey(entry.syscode) || ConfigEntrySyscodes.Contains(entry.syscode))
                        {
                            if (!overwrite)
                                continue; //entry exists already, no need to add it

                            //Remove old entry
                            if (syscodes.ContainsKey(entry.syscode))
                            {
                                foreach (WfvEntry oldentry in ContentData.entries)
                                {
                                    if (!oldentry.syscode.Equals(entry.syscode))
                                        continue;
                                    ContentData.entries.Remove(oldentry);
                                    break;
                                }
                            }
                            else
                            {
                                foreach (WfvConfigEntry oldentry in ContentData.configentries)
                                {
                                    if (!oldentry.syscode.Equals(entry.syscode))
                                        continue;
                                    ContentData.configentries.Remove(oldentry);
                                    break;
                                }
                            }
                        }
                        RegisterEntry(entry);
                        ContentData.entries.Add(entry);
                    }
                }

                if (ContentData.configentries == null)
                    ContentData.configentries = new List<WfvConfigEntry>();
                if (data.configentries != null)
                {
                    foreach (WfvConfigEntry entry in data.configentries)
                    {
                        if (syscodes.ContainsKey(entry.syscode) || ConfigEntrySyscodes.Contains(entry.syscode))
                        {
                            if (!overwrite)
                                continue; //entry exists already, no need to add it

                            //Remove old entry

                            if (syscodes.ContainsKey(entry.syscode))
                            {
                                foreach (WfvEntry oldentry in ContentData.entries)
                                {
                                    if (!oldentry.syscode.Equals(entry.syscode))
                                        continue;
                                    ContentData.entries.Remove(oldentry);
                                    break;
                                }
                            }
                            else
                            {
                                foreach (WfvConfigEntry oldentry in ContentData.configentries)
                                {
                                    if (!oldentry.syscode.Equals(entry.syscode))
                                        continue;
                                    ContentData.configentries.Remove(oldentry);
                                    break;
                                }
                            }
                        }
                        ContentData.configentries.Add(entry);
                    }
                }
            }
            ConfigEntryList = null;
            ConfigEntryList = GetEntryConfigList();
        }

        /// <summary>
        /// Calculate all WfvEntry objects from data
        /// </summary>
        /// <returns>list of WfvEntry objects from data</returns>
        public List<WfvEntry> GetEntryList()
        {
            if (ContentData != null)
            {
                foreach (WfvEntry entry in ContentData.entries)
                {
                    RegisterEntry(entry);
                }
                return ContentData.entries;
            }
            else
                return new List<WfvEntry>();
        }

        /// <summary>
        /// Calculate all WfvConfigEntry objects from data
        /// </summary>
        /// <returns>list of WfvConfigEntry objects from data</returns>
        public List<WfvConfigEntryDto> GetEntryConfigList()
        {
            if (ConfigEntryList != null)
                return ConfigEntryList;

            if (ContentData != null)
            {
                ConfigEntryList = new List<WfvConfigEntryDto>();
                foreach (WfvConfigEntry xmlEntry in ContentData.configentries)
                {
                    WfvConfigEntryDto entry = Mapper.Map <WfvConfigEntry, WfvConfigEntryDto> (xmlEntry);
                    string typ = "";
                    if (entry.befehlszeile != null)
                        typ = entry.befehlszeile.ToUpper();
                    if (typ.Equals("CAS"))
                    {
                        XmlObjectConverter<iCASEvaluateDto> converter = new XmlObjectConverter<iCASEvaluateDto>();
                        entry.einrichtung = converter.ConvertTo(xmlEntry.einrichtung, typeof(iCASEvaluateDto));
                    }
                    else if (typ.Equals("WORKFLOW"))
                    {
                        XmlObjectConverter<Activity> converter = new XmlObjectConverter<Activity>();
                        entry.einrichtung = converter.ConvertTo(xmlEntry.einrichtung, typeof(Activity));
                    }
                    else if (typ.Equals("OLSTART"))
                    {
                        entry.einrichtung = null;
                    }
                    else
                    {
                        //This is a normal entry, not a configentry
                        XmlObjectConverter<WfvEntry> converter = new XmlObjectConverter<WfvEntry>();
                        if (xmlEntry.einrichtung == null || xmlEntry.einrichtung.Equals(""))
                            entry.einrichtung = new WfvEntry();
                        else
                            entry.einrichtung = converter.ConvertTo(xmlEntry.einrichtung, typeof(WfvEntry));
                        WfvEntry wfvEntry = (WfvEntry)entry.einrichtung;
                        Mapper.Map<WfvConfigEntry, WfvEntry>(xmlEntry, wfvEntry);
                        RegisterEntry(wfvEntry);
                        ContentData.entries.Add(wfvEntry);
                        continue;
                    }

                    ConfigEntryList.Add(entry);
                    RegisterEntry(entry);
                }
                return ConfigEntryList;
            }
            else
                return new List<WfvConfigEntryDto>();

        }

        /// <summary>
        /// Adds and registers an element not coming from the data source to the content data
        /// </summary>
        /// <param name="newElement"></param>
        public void AddData(object newElement)
        {
            if (ContentData == null)
                return;

            if (newElement is WfvConfigEntryDto)
            {
                WfvConfigEntryDto entry = (WfvConfigEntryDto)newElement;
                if (ConfigEntryList == null)
                    GetEntryConfigList();

                ConfigEntryList.Add(entry);
                RegisterEntry(entry);
            }
            else if (newElement is WfvEntry)
            {
                WfvEntry entry = (WfvEntry)newElement;
                ContentData.entries.Add(entry);
                RegisterEntry(entry);
            }
        }

        /// <summary>
        /// Add the object to the available elements and adapt further settings
        /// </summary>
        /// <param name="entry">available element</param>
        private static void RegisterEntry(WfvEntry entry)
        {
            if (entry == null)
                return;

            syscodes[entry.syscode] = (EntryType)entry.entrytype;
            CustomizeAttributesForInstance(entry);
        }
        /// <summary>
        /// Remove the object from the available elements
        /// </summary>
        /// <param name="entry">available element</param>
        private static void UnRegisterEntry(WfvEntry entry)
        {
            if (entry == null)
                return;

            syscodes.Remove(entry.syscode);
        }

        /// <summary>
        /// Add the object to the available elements and adapt further settings
        /// </summary>
        /// <param name="entry">available element</param>
        private static void RegisterEntry(WfvConfigEntryDto entry)
        {
            if (entry == null)
                return;

            ConfigEntrySyscodes.Add(entry.syscode);
            CustomizeAttributesForInstance(entry);
        }
        /// <summary>
        /// Remove the object from the available elements
        /// </summary>
        /// <param name="entry">available element</param>
        private static void UnRegisterEntry(WfvConfigEntryDto entry)
        {
            if (entry == null)
                return;

            ConfigEntrySyscodes.Remove(entry.syscode);
        }

        /// <summary>
        /// After editing the config entries, return updated data
        /// </summary>
        /// <returns>edited config data</returns>
        public WfvConfig GetContent()
        {
            if (ContentData == null)
                return null;

            if (ConfigEntryList == null)
                return ContentData;

            ContentData.configentries.Clear();
            XmlObjectConverter<object> converter = new XmlObjectConverter<object>();
            foreach (WfvConfigEntryDto configentry in ConfigEntryList)
            {
                WfvConfigEntry entry = Mapper.Map<WfvConfigEntryDto, WfvConfigEntry>(configentry);
                entry.einrichtung = (string)converter.ConvertTo(configentry.einrichtung, typeof(string));

                ContentData.configentries.Add(entry);
            }

            return ContentData;
        }

        /// <summary>
        /// provide a way to "anonymously" get the entry syscode
        /// </summary>
        /// <param name="obj">entry</param>
        /// <returns>syscode in capital letters</returns>
        public static string GetSyscodeUpper(object obj)
        {
            if (obj is WfvEntry)
            {
                WfvEntry entry = (WfvEntry)obj;
                if (entry.syscode == null)
                    return "";
                return entry.syscode.ToUpper();
            }
            if (obj is WfvConfigEntryDto)
            {
                WfvConfigEntryDto entry = (WfvConfigEntryDto)obj;
                if (entry.syscode == null)
                    return "";
                return entry.syscode.ToUpper();
            }
            return "";
        }


        /// <summary>
        /// Get list of all syscodes in use
        /// </summary>
        /// <returns>all syscodes in use, combined with their entry types</returns>
        public static IDictionary<string, EntryType> GetExistingSyscodes()
        {
            return syscodes;
        }

        /// <summary>
        /// Some fields don't make sense for e.g. specific entry types. We help the user by making those fields readonly
        /// Example: "wizard type" only has an effect when the entry type is "Wizard"
        /// </summary>
        private static void CustomizeAttributesForInstance(WfvEntry instance)
        {
            if (instance == null)
                return;

            //Note: The Propertygrid does not support instance-based property attributes properly, yet.
            //Most Attributes don't have any effect. 
            //As for the ReadOnly attribute: It means "starting with this property, all properties have this value as "read only". That means we can only use it for the last object value, if any.
            PropertyAttributeAdder<WfvEntry> propAdder = new PropertyAttributeAdder<WfvEntry>(instance);
            propAdder.AddAttributes("wizardtype", new ReadOnlyAttribute(instance.entrytype != (int)EntryType.Wizard));
            //propAdder.AddAttributes("customentry", new ReadOnlyAttribute(instance.entrytype == (int)EntryType.Dashboard || instance.entrytype == (int)EntryType.Wizard));
            //propAdder.AddAttributes("references", new ReadOnlyAttribute(instance.entrytype != (int)EntryType.Dashboard && instance.entrytype != (int)EntryType.Wizard));
            propAdder.ProvideAttributes();
        }

        /// <summary>
        /// Some fields don't make sense for e.g. specific entry types. We help the user by making those fields readonly
        /// Example: Command "olstart" does not support any further information in field "einrichtung", so there is no need to edit it
        /// </summary>
        private static void CustomizeAttributesForInstance(WfvConfigEntryDto instance)
        {
            if (instance == null)
                return;

            //Note: The Propertygrid does not support instance-based property attributes properly, yet.
            //Most Attributes don't have any effect.
            //As for the ReadOnly attribute: It means "starting with this property, all properties have this value as "read only". That means we can only use it for the last object value, if any.
            PropertyAttributeAdder<WfvConfigEntryDto> propAdder = new PropertyAttributeAdder<WfvConfigEntryDto>(instance);
            //propAdder.AddAttributes("einrichtung", new ReadOnlyAttribute(instance.befehlszeile != null && instance.befehlszeile.ToUpper().Equals("OLSTART")));
            propAdder.ProvideAttributes();
        }


        /// <summary>
        /// Delete given element from the list
        /// </summary>
        /// <param name="element">object to remove</param>
        public void Remove(object element)
        {
            if (element == null)
                return;

            if (ContentData == null)
                return;

            if (element is WfvEntry)
            {
                if (ContentData.entries == null)
                    return;
                ContentData.entries.Remove((WfvEntry)element);
                UnRegisterEntry((WfvEntry)element);
            }
            else if (element is WfvConfigEntryDto)
            {
                if (ConfigEntryList == null)
                    GetEntryConfigList();
                ConfigEntryList.Remove((WfvConfigEntryDto)element);
                UnRegisterEntry((WfvConfigEntryDto)element);
            }
        }

        /// <summary>
        /// Get the element data to the element's primary key
        /// </summary>
        /// <param name="syscode">primary key</param>
        /// <returns>element data for the syscode</returns>
        public WfvEntry GetEntry(string syscode)
        {
            if (syscode == null || syscode.Length == 0)
                return null;

            if (ContentData == null || ContentData.entries == null || !syscodes.ContainsKey(syscode))
                return null;

            foreach (WfvEntry entry in ContentData.entries)
            {
                if (entry.syscode.Equals(syscode))
                    return entry;
            }

            syscodes.Remove(syscode);
            return null;
        }
    }
}
