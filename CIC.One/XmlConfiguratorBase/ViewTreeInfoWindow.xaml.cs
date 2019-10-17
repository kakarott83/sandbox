using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase
{
    /// <summary>
    /// Interaction logic for ViewInfoWindow.xaml
    /// </summary>
    public partial class ViewTreeInfoWindow : Window
    {
        /// <summary>
        /// Window for displaying the given Text
        /// </summary>
        /// <param name="info">Text that shall be displayed</param>
        public ViewTreeInfoWindow(StringTree info, string title = "")
        {
            InitializeComponent();

            if (title.Length > 0)
                Title = title;

            InfoTree.Items.Add(GetDependencyTree(info));
            Show();
        }

        /// <summary>
        /// Generate visual tree element for logical tree
        /// </summary>
        /// <param name="tree">logical tree</param>
        /// <returns>visual tree</returns>
        private TreeViewItem GetDependencyTree(StringTree tree)
        {
            TreeViewItem item = new TreeViewItem();
            item.IsExpanded = true;
            if (tree == null)
                return item;

            item.Header = tree.Element;
            if (tree.Children != null)
            {
                foreach (StringTree child in tree.Children)
                {
                    if (child == null)
                        continue;
                    item.Items.Add(GetDependencyTree(child));
                }
            }
            return item;
        }
    }
}
