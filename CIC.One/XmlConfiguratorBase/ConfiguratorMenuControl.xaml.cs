using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase
{
    /// <summary>
    /// Interaction logic for ConfiguratorMenuControl.xaml
    /// </summary>
    public partial class ConfiguratorMenuControl : UserControl, IDependentControl, ISettings
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

        private static readonly DependencyProperty OverwritingSettingDependencyProperty = DependencyProperty.Register("OverwritingSettings", typeof(DataReadMode), typeof(ConfiguratorMenuControl));
        public DataReadMode OverwritingSettings
        {
            get
            {
                return (DataReadMode)GetValue(OverwritingSettingDependencyProperty);
            }
            set
            {
                SetValue(OverwritingSettingDependencyProperty, value);
            }
        }

        public ConfiguratorMenuControl()
        {
            InitializeComponent();
            FocusManager.SetIsFocusScope(this, true);
        }

        private void CanExecuteAlways(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CanExecuteIfItemSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CoreControl.SelectedObject != null;
        }

        private void OnExecuteNewElement(object sender, RoutedEventArgs e)
        {
            CoreControl.DoCreateNewElement();
        }
        private void OnExecuteDeleteElement(object sender, ExecutedRoutedEventArgs e)
        {
            CoreControl.DoDeleteElement();
        }
        private void OnExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            CoreControl.DoSave();
        }
        private void OnExecuteOpenFile(object sender, ExecutedRoutedEventArgs e)
        {
            CoreControl.DoOpenFile();
        }
        private void OnExecuteShowDependencies(object sender, ExecutedRoutedEventArgs e)
        {
            CoreControl.DoShowDependencies();
        }
        void OnClickOpenDatabase(object sender, RoutedEventArgs e)
        {
            CoreControl.DoOpenDatabase();
        }
        void OnClickOpenCustom(object sender, RoutedEventArgs e)
        {
            CoreControl.DoOpenCustom();
        }
        void OnClickSaveFile(object sender, RoutedEventArgs e)
        {
            CoreControl.DoSaveFile();
        }
        void OnClickSaveDatabase(object sender, RoutedEventArgs e)
        {
            CoreControl.DoSaveDatabase();
        }
        void OnClickCreateNewWfvEntry(object sender, RoutedEventArgs e)
        {
            CoreControl.DoCreateNewWfvEntry();
        }
        void OnClickDebug(object sender, RoutedEventArgs e)
        {
            CoreControl.SetElementFromXml("<WfvEntry><syscode>cockpit</syscode></WfvEntry>");
            CoreControl.DoOpenCustom();
        }
    }
}
