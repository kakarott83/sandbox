using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase.BO.GUI
{
    /// <summary>
    /// Possible entry type values for combobox
    /// </summary>
    public class EntrytypeItemsSource : IItemsSource
    {
        public ItemCollection GetValues()
        {
            ItemCollection possibleItems = new ItemCollection();
            foreach (EntryType item in Enum.GetValues(typeof(EntryType)))
            {
                possibleItems.Add((int)item, item.ToString());
            }
            return possibleItems;
        }
    }
}
