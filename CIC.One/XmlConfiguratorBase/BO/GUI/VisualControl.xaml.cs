using Cic.One.DTO;
using System.Windows.Controls;

namespace XmlConfiguratorBase.BO.GUI
{
    /// <summary>
    /// Interaction logic for VisualControl.xaml
    /// </summary>
    public partial class VisualControl : UserControl
    {
        public object SelectedObject
        {
            set
            {
                if (value == null)
                    SelectedWfv = null;
                else if (!(value is WfvEntry))
                    SelectedWfv = null;
                else
                    SelectedWfv = (WfvEntry)value;
            }
        }
        private WfvEntry SelectedWfv { get; set; }

        public VisualControl()
        {
            InitializeComponent();
        }

        public bool CanShowVisual()
        {
            return SelectedWfv != null && false;
        }
    }
}
