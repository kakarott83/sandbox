using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace XmlConfiguratorBase
{
    /// <summary>
    /// Interaction logic for ConfiguratorButtonsControl.xaml
    /// </summary>
    public partial class ConfiguratorButtonsControl : UserControl, IDependentControl
    {
        private ICoreControl corecontrol = null;
        public ICoreControl CoreControl
        {
            private get
            {
                if (corecontrol == null)
                    throw new NullReferenceException("The buttons control cannot exist without a core control which executes the commands. Assign it to CoreControl before menu usage.");
                return corecontrol;
            }
            set
            {
                corecontrol = value;
                corecontrol.Register(this);
            }
        }

        public ConfiguratorButtonsControl()
        {
            InitializeComponent();
        }

        private void CanExecuteAlways(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void OnClickOpenDatabase(object sender, RoutedEventArgs e)
        {
            CoreControl.DoOpenDatabase();
        }
        void OnClickOpenCustom(object sender, RoutedEventArgs e)
        {
            CoreControl.DoOpenCustom();
        }

        void OnExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            CoreControl.DoSave();
        }
        void OnExecuteOpenFile(object sender, ExecutedRoutedEventArgs e)
        {
            CoreControl.DoOpenFile();
        }
    }
}
