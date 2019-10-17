using AutoMapper;
using Cic.One.DTO;
using System;
using System.Collections.Generic;
using XmlConfiguratorBase.DAO;
using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase.BO.ContentLogics
{
    [Serializable]
    /// <summary>
    /// Configurator for wfv config data
    /// </summary>
    public class Configurator
    {
        private ContentManager ContentData = new ContentManager();
        private IDataManager DataManager = null;

        public Configurator()
        {
        }

        /// <summary>
        /// Save data to defined data source
        /// <param name="datadestination">Where the data shall be saved to</param>
        /// </summary>
        public void Save(DataSource datadestination = DataSource.NO_SOURCE)
        {
            if (datadestination == DataSource.NO_SOURCE)
            {
                if (DataManager == null)
                    return; //If no file is opened, there is no file for saving the content
            }
            else
            {
                if (DataManager == null || datadestination != DataManagerFactory.GetLastDataManagerSource())
                    Load(datadestination, DataReadMode.ADD_DATA_IF_NOT_EXISTING);
            }
            WfvConfig data = ContentData.GetContent();
            DataManager.SaveData(data);
        }

        /// <summary>
        /// Change data source, discard current data
        /// </summary>
        /// <param name="datasource">new data source</param>
        /// <param name="readmode">setting if and what data shall be overwritten</param>
        public void Load(DataSource datasource, DataReadMode readmode)
        {
            if (readmode == DataReadMode.GIVEN_SOURCE_ONLY)
                ContentData.ResetData();
            if (DataManager != null && DataManager != DataManagerFactory.Custom)
                DataManager.Dispose();
            DataManager = DataManagerFactory.CreateDataManager(datasource);
            WfvConfig newdata = DataManager.ReadData();
            if (readmode == DataReadMode.ADD_DATA_IF_NOT_EXISTING)
                ContentData.AddData(newdata);
            else
                ContentData.SetData(newdata);
        }

        /// <summary>
        /// Adds and registers an element not coming from the data source to the content data
        /// </summary>
        /// <param name="newElement"></param>
        public void Load(object newElement)
        {
            ContentData.SetData(newElement);
        }

        /// <summary>
        /// List of all wfv entries describing a panel
        /// </summary>
        public IEnumerable<object> GetWfvEntries()
        {
            return ContentData.GetEntryList();
        }

        /// <summary>
        /// List of all wfv config entries describing special stuff that is not a panel
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetWfvConfigEntries()
        {
            return ContentData.GetEntryConfigList();
        }

        /// <summary>
        /// Delete given element from the list
        /// </summary>
        /// <param name="element">object to remove</param>
        public void Remove(object element)
        {
            ContentData.Remove(element);
        }
    
        /// <summary>
        /// Calculate the syscodes on which the given element depends on
        /// </summary>
        /// <param name="entry">dependency root</param>
        /// <param name="alreadyShown">syscodes whose dependency info will be hidden</param>
        /// <returns>Description of elements the given entry depends on</returns>
        public StringTree GetDependencyTree(WfvEntry entry, HashSet<string> alreadyShown = null)
        {
            if (entry == null)
                return null;

            if (alreadyShown == null)
                alreadyShown = new HashSet<string>();

            StringTree dependencies = new StringTree(entry.syscode);
            if (alreadyShown.Contains(entry.syscode))
            {
                dependencies.Element += " [...]";
                return dependencies;
            }
            alreadyShown.Add(entry.syscode);

            dependencies += GetCustomEntryDependencyTree(entry.customentry, alreadyShown);

            StringTree temp = null;
            foreach (WfvRef reference in entry.references)
            {
                if (reference == null)
                    continue;

                temp = GetDependencyTree(ContentData.GetEntry(reference.syscode), alreadyShown);
                if (temp == null)
                    continue;

                temp.Element = "(Ref) " + temp.Element;
                dependencies += temp;
            }

            return dependencies;
        }

        /// <summary>
        /// Get description of elements the given customentry depends on
        /// </summary>
        /// <param name="customentry">element depending on other elements</param>
        /// <returns>elements needed for the given customentry</returns>
        private IEnumerable<StringTree> GetCustomEntryDependencyTree(CustomEntry customentry, HashSet<string> alreadyShown = null)
        {
            List<StringTree> customentryDependencies = new List<StringTree>();
            
            if (customentry == null)
                return customentryDependencies;

            if (alreadyShown == null)
                alreadyShown = new HashSet<string>();

            StringTree temp = GetDependencyTree(ContentData.GetEntry(customentry.createsyscode), alreadyShown);
            if (temp != null)
            {
                temp.Element = "(create) " + temp.Element;
                customentryDependencies.Add(temp);
            }
            temp = GetDependencyTree(ContentData.GetEntry(customentry.detailsyscode), alreadyShown);
            if (temp != null)
            {
                temp.Element = "(detail) " + temp.Element;
                customentryDependencies.Add(temp);
            }
            temp = GetDependencyTree(ContentData.GetEntry(customentry.forwardsyscode), alreadyShown);
            if (temp != null)
            {
                temp.Element = "(forward) " + temp.Element;
                customentryDependencies.Add(temp);
            }

            return customentryDependencies;
        }
        
        /// <summary>
        /// Load object described by the xml string
        /// </summary>
        /// <param name="xml">xml describing the object to load</param>
        /// <returns>loaded object</returns>
        public object LoadXml(string xml)
        {
            IDataManager xmlReader = new XmlDataManager(xml);
            WfvConfig config = xmlReader.ReadData();
            ContentData.AddData(config);

            if (config.entries.Count > 0)
                return config.entries[0];
            else if (config.configentries.Count > 0)
                return config.configentries[0];
            else
                return null;
        }

        /// <summary>
        /// Access to the entry with given syscode
        /// </summary>
        /// <param name="syscode">syscode identifying the object</param>
        /// <returns>object with given id</returns>
        public object GetElement(string syscode)
        {
            return ContentData.GetEntry(syscode);
        }
    }
}
