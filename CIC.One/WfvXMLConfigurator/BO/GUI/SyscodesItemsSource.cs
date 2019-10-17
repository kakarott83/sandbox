using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using WfvXmlConfigurator.DTO;
using WfvXmlConfigurator.BO.ContentLogics;

namespace WfvXmlConfigurator.BO.GUI
{
    /// <summary>
    /// Base class for existing syscodes collection
    /// </summary>
    public abstract class SyscodesItemsSource : IItemsSource
    {
        private List<EntryType> AllowedEntryTypes = new List<EntryType>();
        
        /// <summary>
        /// Define list of allowed entry types. If none are given, all entry types are allowed.
        /// </summary>
        /// <param name="entrytypes">allowed types of entries</param>
        public SyscodesItemsSource(params EntryType[] entrytypes)
        {
            foreach (EntryType entrytype in entrytypes)
            {
                AllowedEntryTypes.Add(entrytype);
            }

            if (AllowedEntryTypes.Count == 0)
            {
                foreach (EntryType entrytype in Enum.GetValues(typeof(EntryType)))
                {
                    AllowedEntryTypes.Add(entrytype);
                }
            }
        }


        /// <summary>
        /// Get all existing syscodes with matching entry type
        /// </summary>
        /// <returns></returns>
        public ItemCollection GetValues()
        {
            ItemCollection possibleItems = new ItemCollection();

            IDictionary<string, EntryType> existingEntries = ContentManager.GetExistingSyscodes();
            foreach (string syscode in existingEntries.Keys)
            {
                if (!AllowedEntryTypes.Contains(existingEntries[syscode]))
                    continue;

                possibleItems.Add(syscode);
            }
            possibleItems.Sort((syscode1, syscode2) => syscode1.Value.ToString().CompareTo(syscode2.Value.ToString()));
            return possibleItems;
        }

    }

    /// <summary>
    /// item selection for syscodes of type details and list
    /// </summary>
    public class DetailListSyscodesItemsSource : SyscodesItemsSource
    {
        public DetailListSyscodesItemsSource()
            : base(EntryType.Details, EntryType.Liste)
        {
        }
    }

    /// <summary>
    /// item selection for syscodes of type details and dashboard
    /// </summary>
    public class DetailDashboardSyscodesItemsSource : SyscodesItemsSource
    {
        public DetailDashboardSyscodesItemsSource()
            : base(EntryType.Details, EntryType.Dashboard, EntryType.Wizard)
        {
        }
    }

    /// <summary>
    /// item selection for syscodes of all types
    /// </summary>
    public class AllSyscodesItemsSource : SyscodesItemsSource
    {
        public AllSyscodesItemsSource()
            : base()
        {
        }
    }

}
