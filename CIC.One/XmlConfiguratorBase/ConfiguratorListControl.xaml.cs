using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using XmlConfiguratorBase.BO.ContentLogics;
using XmlConfiguratorBase.BO.GUI;
using XmlConfiguratorBase.DAO;
using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase
{
    /// <summary>
    /// Interaction logic for ConfiguratorListControl.xaml
    /// </summary>
    public partial class ConfiguratorListControl : UserControl, IListControl
    {
        private ICoreControl corecontrol = null;
        public ICoreControl CoreControl
        {
            private get
            {
                if (corecontrol == null)
                    throw new NullReferenceException("The menu control cannot exist without a core control which executes the commands. Assign it to CoreControl before menu usage.");
                return corecontrol;
            }
            set
            {
                corecontrol = value;
                corecontrol.Register(this);
            }
        }

        public ListBox ListOfWfvEntries
        {
            get { return WfvEntryList; }
        }

        public ListBox ListOfWfvConfigEntries
        {
            get { return WfvConfigEntryList; }
        }

        
        public ConfiguratorListControl()
        {
            InitializeComponent();

            WfvEntryList.Items.SortDescriptions.Add(new SortDescription("syscode", ListSortDirection.Ascending));
            WfvConfigEntryList.Items.SortDescriptions.Add(new SortDescription("syscode", ListSortDirection.Ascending));

            WfvEntryList.Items.Filter = SearchFilter;
            WfvConfigEntryList.Items.Filter = SearchFilter;
        }


        /// <summary>
        /// Only display texts that contain the search text
        /// </summary>
        /// <param name="obj">list element</param>
        /// <returns>passes filter</returns>
        private bool SearchFilter(object obj)
        {
            string s = ContentManager.GetSyscodeUpper(obj);
            if (s.Equals(""))
                return true;
            return s.Contains(SearchBox.Text.ToUpper());
        }

        /// <summary>
        /// View details of newly selected object
        /// </summary>
        public void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object entry in e.AddedItems)
            {
                CoreControl.SelectedObject = entry;
                break;
            }
        }

        /// <summary>
        /// input in search textbox changed, so the search filter needs test the entries
        /// </summary>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WfvEntryList.ItemsSource != null)
                CollectionViewSource.GetDefaultView(WfvEntryList.ItemsSource).Refresh();
            if (WfvConfigEntryList.ItemsSource != null)
                CollectionViewSource.GetDefaultView(WfvConfigEntryList.ItemsSource).Refresh();
        }

    }

}
