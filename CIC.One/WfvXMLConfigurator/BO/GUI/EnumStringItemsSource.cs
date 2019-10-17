using Cic.One.DTO;
using ExtraConstraints;
using System;
using System.Collections.Generic;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WfvXmlConfigurator.BO.GUI
{
    /// <summary>
    /// possible items for strings that can only have values of an enum
    /// </summary>
    public class EnumStringItemsSource<[EnumConstraint] T> : IItemsSource
    {
        public ItemCollection GetValues()
        {
            ItemCollection possibleItems = new ItemCollection();
            possibleItems.Add("");
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                possibleItems.Add(item, item.ToString());
            }
            possibleItems.Sort((item1, item2) => item1.ToString().CompareTo(item2.ToString()));
            return possibleItems;
        }
    }
}
