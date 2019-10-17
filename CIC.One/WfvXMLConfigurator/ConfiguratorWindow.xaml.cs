using System.Windows;

namespace WfvXmlConfigurator
{
    /// <summary>
    /// Interaction logic for ConfiguratorControl.xaml
    /// </summary>
    public partial class ConfiguratorWindow : Window
    {
        public ConfiguratorWindow()
        {
            InitializeComponent();

            Core.Status = Status;
            Core.Settings = Menu;
            Core.ListBoxes = Lists;
            Lists.CoreControl = Core;
            Menu.CoreControl = Core;
            Buttons.CoreControl = Core;
        }
    }
}