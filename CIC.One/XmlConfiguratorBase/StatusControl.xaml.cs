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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XmlConfiguratorBase
{
    /// <summary>
    /// Interaction logic for StatusControl.xaml
    /// </summary>
    public partial class StatusControl : UserControl, IStatusBar
    {
        public string TextLeft
        { 
            get
            {
                return StatusBarExecuting.Text;
            }
            set
            {
                StatusBarExecuting.Text = value;
            }
        }
        public string TextRight
        {
            get
            {
                return StatusBarCurrentDataSource.Text;
            }
            set
            {
                StatusBarCurrentDataSource.Text = value;
            }
        }

        public string InvalidXmlWarning
        {
            get
            {
                return InvalidXml.Text;
            }
            set
            {
                InvalidXml.Text = value;
            }
        }

        public bool ProgressVisible
        {
            set
            {
                Progress.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public StatusControl()
        {
            InitializeComponent();
        }
    }
}
