using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace Workflows.BO.ExpressionEditor
{
    /// <summary>
    /// Interaction logic for IntelliSensePopup.xaml
    /// </summary>
   /* public partial class IntelliSensePopup : Page
    {
        public IntelliSensePopup()
        {
            InitializeComponent();
        }
    }*/
    public partial class IntelliSensePopup : Popup
    {
        public IntelliSensePopup()
        {
            InitializeComponent();
        }
        internal TreeNodes SelectedItem
        {
            get { return (TreeNodes)lbxIntelliSense.SelectedItem; }
            set { lbxIntelliSense.SelectedItem = value; }
        }

        internal int SelectedIndex
        {
            get { return lbxIntelliSense.SelectedIndex; }
            set
            {
                if ((value >= lbxIntelliSense.Items.Count) || (value < -1))
                    return;
                lbxIntelliSense.SelectedIndex = value;
                lbxIntelliSense.ScrollIntoView(lbxIntelliSense.SelectedItem);
            }
        }

        internal int ItemsCount
        {
            get { return lbxIntelliSense.Items.Count; }
        }

        internal event ListBoxKeyDownEventHandler ListBoxKeyDown;
        internal delegate void ListBoxKeyDownEventHandler(object sender, KeyEventArgs e);
        internal event ListBoxItemDoubleClickEventHandler ListBoxItemDoubleClick;
        internal delegate void ListBoxItemDoubleClickEventHandler(object sender, MouseButtonEventArgs e);

        protected virtual void OnListBoxItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxItemDoubleClick != null)
            {
                ListBoxItemDoubleClick(sender, e);
            }
        }

        protected virtual void OnListBoxPrevieKeyDown(object sender, KeyEventArgs e)
        {
            if (ListBoxKeyDown != null)
            {
                ListBoxKeyDown(sender, e);
            }
        }

    }
}
