using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WfvXmlConfigurator;
using WfvXmlConfigurator.BO.ContentLogics;
using WfvXmlConfigurator.DTO;

namespace WfvXMLConfigurator
{

    /// <summary>
    /// Interaction logic for ConfiguratorWindow.xaml
    /// </summary>
    public partial class ConfiguratorControl : UserControl
    {
        private static readonly ILog log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Configurator ContentConfigurator = new Configurator();
        private static readonly DependencyProperty OverwritingSettingDependencyProperty = DependencyProperty.Register("OverwritingSettings", typeof(DataReadMode), typeof(ConfiguratorWindow));
        private DataReadMode OverwritingSettings
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

        public ConfiguratorControl()
        {
            InitializeComponent();

            WfvEntryList.Items.SortDescriptions.Add(new SortDescription("syscode", ListSortDirection.Ascending));
            WfvConfigEntryList.Items.SortDescriptions.Add(new SortDescription("syscode", ListSortDirection.Ascending));

            WfvEntryList.Items.Filter = SearchFilter;
            WfvConfigEntryList.Items.Filter = SearchFilter;

            OverwritingSettings = DataReadMode.GIVEN_SOURCE_ONLY;
        }

        /// <summary>
        /// Only display texts that contain the search text
        /// </summary>
        /// <param name="obj">list element</param>
        /// <returns>passes filter</returns>
        private bool SearchFilter (object obj)
        {
            string s = ContentManager.GetSyscodeUpper(obj);
            if (s.Equals(""))
                return true;
            return s.Contains(SearchBox.Text.ToUpper());
        }

        /// <summary>
        /// Save the content to the xml-file
        /// </summary>
        private void OnClickSave(object sender, RoutedEventArgs e)
        {
            StatusBarExecuting.Text = "Konfigurationselemente in derzeit geöffneter Datenquelle speichern...";
            Save();
        }

        /// <summary>
        /// Discard changes and close the editor
        /// </summary>
        private void OnClickCancel (object sender, RoutedEventArgs e)
        {
            Window parent = GetWindow();
            if (parent != null)
                parent.Close();
        }

        /// <summary>
        /// Get the window containing the control
        /// </summary>
        /// <returns>parent window</returns>
        private Window GetWindow()
        {
            Window parent = null;
            if (Parent is Window)
                parent = (Window)Parent;
            return parent;
        }

        /// <summary>
        /// Discard changes and load data from database
        /// </summary>
        private void OnClickOpenDatabase(object sender, RoutedEventArgs e)
        {
            StatusBarCurrentDataSource.Text = "Modus: Datenbank";
            Load(DataSource.DATABASE);
        }

        /// <summary>
        /// Read content from data source
        /// </summary>
        private void Load (DataSource datasource)
        {
            if (StatusBarCurrentDataSource.Text.Length == 0)
                StatusBarCurrentDataSource.Text = datasource.ToString();
            StatusBarExecuting.Text = "Lese Konfigurationselemente aus der Datenbank...";
            DataReadMode datareadmode = OverwritingSettings;
            RunTask((cc) => cc.Load(datasource, datareadmode), UpdateListElements);
        }

        /// <summary>
        /// perform a possibly long calculation outside of the GUI Thread
        /// </summary>
        /// <param name="actionForTask">possibly long calculation</param>
        /// <param name="actionAfterTask">what to do in GUI thread after the task finished</param>
        protected void RunTask (Action<Configurator> actionForTask, Action actionAfterTask = null)
        {
            LockUserInput();
            if (StatusBarExecuting.Text.Length == 0)
                StatusBarExecuting.Text = actionForTask.Method.ToString();
            Task task = Task.Run(new Action(() => actionForTask.Invoke(ContentConfigurator)));
            task.ContinueWith(t =>
            {
                ShowError(t.Exception);
                if (actionAfterTask != null)
                    actionAfterTask.Invoke();
                StatusBarExecuting.Text = "";
                UnlockUserInput();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// When the underlying data has changed, the list elements need to be updated.
        /// Discards changes and displays current data.
        /// </summary>
        private void UpdateListElements()
        {
            try
            {
                WfvEntryList.ItemsSource = null;
                WfvConfigEntryList.ItemsSource = null;
                SelectedPropertyGrid.SelectedObject = null;

                WfvEntryList.ItemsSource = ContentConfigurator.GetWfvEntries();
                WfvConfigEntryList.ItemsSource = ContentConfigurator.GetWfvConfigEntries();
            }
            catch (Exception e)
            {
                ShowError(e);
            }
        }
        
        /// <summary>
        /// View details of newly selected object
        /// </summary>
        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object entry in e.AddedItems)
            {
                SelectedPropertyGrid.SelectedObject = entry;
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

        /// <summary>
        /// Prevent further user input
        /// </summary>
        private void LockUserInput()
        {
            this.IsEnabled = false;
            Progress.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Reactivate gui
        /// </summary>
        private void UnlockUserInput()
        {
            Progress.Visibility = System.Windows.Visibility.Hidden;
            this.IsEnabled = true;
        }

        /// <summary>
        /// tell the user that an exception occurred
        /// </summary>
        /// <param name="e">occurred exception</param>
        private void ShowError(Exception e)
        {
            if (e == null)
                return;

            log.Warn("User encountered an exception", e);

            string message = "Error encountered:";
            string stacktrace = "Stack trace:";
            while (e != null)
            {
                message += "\r\n";
                message += e.Message;
                stacktrace += "\r\n\r\n";
                stacktrace += e.StackTrace;
                e = e.InnerException;
            }

            if (ConstantsDto.DEBUGGING)
            {
                message += "\r\n\r\n";
                message += stacktrace;
            }

            MessageBox.Show(GetWindow(), message, "Fehler", MessageBoxButton.OK);
        }

        /// <summary>
        /// user decided to save the current data in the xml file
        /// </summary>
        private void OnClickSaveFile(object sender, RoutedEventArgs e)
        {
            StatusBarExecuting.Text = "Speichere Konfigurationselemente in der XML-Datei...";
            Save(DataSource.XML_FILE);
        }

        /// <summary>
        /// user decided to save the current data in the database
        /// </summary>
        private void OnClickSaveDatabase(object sender, RoutedEventArgs e)
        {
            StatusBarExecuting.Text = "Speichere Konfigurationselemente in der Datenbank...";
            Save(DataSource.DATABASE);
        }

        /// <summary>
        /// Save data at given data destination
        /// </summary>
        /// <param name="datadestination">where the data shall be saved</param>
        private void Save(DataSource datadestination = DataSource.NO_SOURCE)
        {
            RunTask((cc) => cc.Save(datadestination));
        }

        /// <summary>
        /// user wants to add a new wfv entry to the list
        /// </summary>
        private void OnClickCreateNewWfvEntry(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// user wants to add a new wfv config entry to the list
        /// </summary>
        private void OnExecuteNewElement(object sender, RoutedEventArgs e)
        {
            WfvConfigEntryDto newElement = new WfvConfigEntryDto();
            StatusBarExecuting.Text = "Neues Konfigurationselement der Liste hinzufügen...";
            RunTask((cc) => cc.Load(newElement), () => { UpdateListElements(); SelectedPropertyGrid.SelectedObject = newElement; });
        }

        /// <summary>
        /// user wants to delete the currently selected item
        /// </summary>
        private void OnExecuteDeleteElement(object sender, ExecutedRoutedEventArgs e)
        {
            object objectToDelete = SelectedPropertyGrid.SelectedObject;
            if (objectToDelete == null)
                return;

            StatusBarExecuting.Text = "Konfigurationselemente aus der Liste löschen...";
            RunTask((cc) => cc.Remove(objectToDelete), () => UpdateListElements());
        }

        private void OnExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            StatusBarExecuting.Text = "Konfigurationselemente in derzeit geöffneter Datenquelle speichern...";
            Save();
        }

        /// <summary>
        /// Discard changes and open new xml-file
        /// </summary>
        private void OnExecuteOpenFile(object sender, ExecutedRoutedEventArgs e)
        {
            StatusBarCurrentDataSource.Text = "Modus: XML Datei";
            Load(DataSource.XML_FILE);
        }

        private void OnExecuteShowDependencies(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedPropertyGrid.SelectedObject == null)
                return;

            if (!(SelectedPropertyGrid.SelectedObject is WfvEntry))
                return;

            WfvEntry entry = (WfvEntry)SelectedPropertyGrid.SelectedObject;
            StringTree text = null;
            StatusBarExecuting.Text = "Maskenabhängigkeiten ermitteln";
            RunTask((cc) => text = cc.GetDependencyTree(entry), () => new ViewTreeInfoWindow(text, "Abhängigkeiten"));
        }

        private void CanExecuteAlways(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CanExecuteIfItemSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedPropertyGrid.SelectedObject != null;
        }
    }

}
