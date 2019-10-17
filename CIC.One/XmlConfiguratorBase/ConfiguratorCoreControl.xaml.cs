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
    /// Interaction logic for ConfiguratorWindow.xaml
    /// </summary>
    public partial class ConfiguratorCoreControl : UserControl, ICoreControl
    {
        public object SelectedObject
        {
            get
            {
                if (SelectedPropertyGrid == null)
                    return null;
                return SelectedPropertyGrid.SelectedObject;
            }
            set
            {
                if (SelectedPropertyGrid != null)
                {
                    SelectedPropertyGrid.SelectedObject = value;
                    UpdateXml(value);
                }
                
                if (WfvEntryList.Items.Contains(value))
                {
                    if (WfvEntryList.SelectedItem != value)
                        WfvEntryList.SelectedItem = value;
                }
                else if (WfvConfigEntryList.Items.Contains(value))
                {
                    if (WfvConfigEntryList.SelectedItem != value)
                        WfvConfigEntryList.SelectedItem = value;
                }
            }
        }
        private ICollection<IDependentControl> Children = new List<IDependentControl>();
        public IStatusBar Status { private get; set; }
        public IListControl ListBoxes { private get; set; }
        public ISettings Settings { private get; set; }

        private static readonly ILog log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Configurator ContentConfigurator = new Configurator();
        private List<Quest> RunningTasks = new List<Quest>();
        private DataReadMode OverwritingSettings
        {
            get
            {
                if (Settings == null)
                    return DataReadMode.GIVEN_SOURCE_ONLY;
                return Settings.OverwritingSettings;
            }
        }

        private ListBox FallbackWfvList = new ListBox();
        private ListBox FallbackWfvConfigList = new ListBox();
        private ListBox WfvEntryList
        {
            get
            {
                if (ListBoxes == null)
                    return FallbackWfvList;
                return ListBoxes.ListOfWfvEntries;
            }
        }
        private ListBox WfvConfigEntryList
        {
            get
            {
                if (ListBoxes == null)
                    return FallbackWfvList;
                return ListBoxes.ListOfWfvConfigEntries;
            }
        }

        private bool InvalidXml { get { return !Source.IsValid(); } }
        private bool VisualEnabled { get { return Visual.CanShowVisual(); } }

        public ConfiguratorCoreControl()
        {
            InitializeComponent();
        }

        public void Register(IDependentControl child)
        {
            Children.Add(child);
        }

        public void SetDataManager(IDataManager access)
        {
            DataManagerFactory.Custom = access;
        }

        /// <summary>
        /// Save the content to the xml-file
        /// </summary>
        public void DoSave()
        {
            if (Status != null)
                Status.TextLeft = "Konfigurationselemente in derzeit geöffneter Datenquelle speichern...";
            Save();
        }
        
        /// <summary>
        /// Discard changes and load data from database
        /// </summary>
        public void DoOpenDatabase()
        {
            if (Status != null)
                Status.TextRight = "Modus: Datenbank";
            Load(DataSource.DATABASE);
        }
        
        /// <summary>
        /// Discard changes and load data from given custom source
        /// </summary>
        public void DoOpenCustom()
        {
            if (Status != null)
                Status.TextRight = "Modus: Automatisch";
            Load(DataSource.CUSTOM);
        }
        
        /// <summary>
        /// Read content from data source
        /// </summary>
        protected void Load (DataSource datasource)
        {
            if (Status != null)
            {
                if (Status.TextRight.Length == 0)
                    Status.TextRight = datasource.ToString();
            }
            DataReadMode datareadmode = OverwritingSettings;
            string syscodeSelected = "";
            Quest loadQuest = new Quest
            {
                TaskDescription = "Lese Konfigurationselemente...",
                ActionBeforeTask = () =>
                {
                    syscodeSelected = ContentManager.GetSyscodeUpper(SelectedObject);
                },
                ActionForTask = (cc) =>
                {
                    cc.Load(datasource, datareadmode);
                },
                ActionAfterTask = () =>
                {
                    UpdateListElements();
                    object newSelected = null;
                    RunTask("Stelle Selektion wieder her...", null, (cc) => newSelected = cc.GetElement(syscodeSelected), () => SelectedObject = newSelected);
                }
            };
            RunTask(loadQuest);
        }

        /// <summary>
        /// perform a possibly long calculation outside of the GUI Thread
        /// </summary>
        /// <param name="actionForTask">possibly long calculation</param>
        /// <param name="actionAfterTask">what to do in GUI thread after the task finished</param>
        protected void RunTask(string taskDescription, Action actionBeforeTask, Action<Configurator> actionForTask, Action actionAfterTask = null)
        {
            RunTask(new Quest
            {
                TaskDescription = taskDescription,
                ActionBeforeTask = actionBeforeTask,
                ActionForTask = actionForTask,
                ActionAfterTask = actionAfterTask 
            });
        }

        /// <summary>
        /// perform a possibly long calculation outside of the GUI Thread
        /// </summary>
        /// <param name="quest">task to run</param>
        protected void RunTask (Quest quest)
        {
            if (!RunningTasks.Contains(quest))
            {
                RunningTasks.Add(quest);
                if (RunningTasks.Count > 1)
                    return;
            }

            LockUserInput();
            if (Status != null && Status.TextLeft.Length == 0)
                Status.TextLeft = quest.TaskDescription.Length > 0 ? quest.TaskDescription : quest.ActionForTask.Method.ToString();
            if (quest.ActionBeforeTask != null)
                quest.ActionBeforeTask.Invoke();
            Task task = Task.Run(new Action(() => quest.ActionForTask.Invoke(ContentConfigurator)));
            task.ContinueWith(t =>
            {
                ShowError(t.Exception);
                if (quest.ActionAfterTask != null)
                    quest.ActionAfterTask.Invoke();
                if (Status != null)
                    Status.TextLeft = "";
                RunningTasks.RemoveAt(0);
                if (RunningTasks.Count > 0)
                    RunTask(RunningTasks[0]);
                else
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
                SelectedObject = null;

                WfvEntryList.ItemsSource = ContentConfigurator.GetWfvEntries();
                WfvConfigEntryList.ItemsSource = ContentConfigurator.GetWfvConfigEntries();
            }
            catch (Exception e)
            {
                ShowError(e);
            }
        }
        

        /// <summary>
        /// Prevent further user input
        /// </summary>
        private void LockUserInput()
        {
            foreach (IDependentControl child in Children)
                child.IsEnabled = false;
            this.IsEnabled = false;
            if (Status != null)
                Status.ProgressVisible = true;
        }

        /// <summary>
        /// Reactivate gui
        /// </summary>
        private void UnlockUserInput()
        {
            if (Status != null)
                Status.ProgressVisible = false;
            this.IsEnabled = true;
            foreach (IDependentControl child in Children)
                child.IsEnabled = true;
        }

        /// <summary>
        /// tell the user that an exception occurred
        /// </summary>
        /// <param name="e">occurred exception</param>
        private void ShowError(Exception e)
        {
            if (e == null)
            {
                ErrorText.Text = "";
                return;
            }

            log.Warn("User encountered an exception", e);

            ErrorText.Text = BO.LogDebug.GetError(e);
            ErrorText.InvalidateVisual();
        }

        /// <summary>
        /// user decided to save the current data in the xml file
        /// </summary>
        public void DoSaveFile()
        {
            Save(DataSource.XML_FILE);
        }

        /// <summary>
        /// user decided to save the current data in the database
        /// </summary>
        public void DoSaveDatabase()
        {
            Save(DataSource.DATABASE);
        }

        /// <summary>
        /// Save data at given data destination
        /// </summary>
        /// <param name="datadestination">where the data shall be saved</param>
        private void Save(DataSource datadestination = DataSource.NO_SOURCE)
        {
            RunTask("Speichere Konfigurationselemente...", null, (cc) => cc.Save(datadestination));
        }

        /// <summary>
        /// user wants to add a new wfv entry to the list
        /// </summary>
        public void DoCreateNewWfvEntry()
        {
            WfvEntry newElement = new WfvEntry();
            RunTask("Neues Maskenelement der Liste hinzufügen...", null, (cc) => cc.Load(newElement), () => { UpdateListElements(); SelectedObject = newElement; });
        }

        /// <summary>
        /// user wants to add a new wfv config entry to the list
        /// </summary>
        public void DoCreateNewElement()
        {
            WfvConfigEntryDto newElement = new WfvConfigEntryDto();
            RunTask("Neues Konfigurationselement der Liste hinzufügen...", null, (cc) => cc.Load(newElement), () => { UpdateListElements(); SelectedObject = newElement; });
        }

        /// <summary>
        /// user wants to delete the currently selected item
        /// </summary>
        public void DoDeleteElement()
        {
            object objectToDelete = SelectedObject;
            if (objectToDelete == null)
                return;

            RunTask("Konfigurationselemente aus der Liste löschen...", null, (cc) => cc.Remove(objectToDelete), () => UpdateListElements());
        }

        /// <summary>
        /// Discard changes and open new xml-file
        /// </summary>
        public void DoOpenFile()
        {
            if (Status != null)
                Status.TextRight = "Modus: XML Datei";
            Load(DataSource.XML_FILE);
        }

        public void DoShowDependencies()
        {
            if (SelectedObject == null)
                return;

            if (!(SelectedObject is WfvEntry))
                return;

            WfvEntry entry = (WfvEntry)SelectedObject;
            StringTree text = null;
            RunTask("Maskenabhängigkeiten ermitteln", null, (cc) => text = cc.GetDependencyTree(entry), () => new ViewTreeInfoWindow(text, "Abhängigkeiten"));
        }

        /// <summary>
        /// Set selected element to the object described by the xml string
        /// </summary>
        /// <param name="xml"></param>
        public void SetElementFromXml(string xml)
        {
            object xmlObject = null;
            RunTask("Lade XML Objekt...", null, (cc) => xmlObject = cc.LoadXml(xml), () => { UpdateListElements(); SelectedObject = xmlObject; });
        }

        /// <summary>
        /// User selected a different view mode
        /// </summary>
        private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e == null)
                return;
            if (e.RemovedItems.Count == 0 || e.AddedItems.Count == 0)
                return;
            if (!(e.RemovedItems[0] is TabItem && e.AddedItems[0] is TabItem))
                return;


            ViewMode FromTab = GetViewMode(((TabItem)(e.RemovedItems[0])).Header.ToString());
            ViewMode ToTab = GetViewMode(((TabItem)(e.AddedItems[0])).Header.ToString());

            if (FromTab == ViewMode.SOURCE)
            {
                if (InvalidXml)
                    EnterInvalidXmlState();
                else
                {
                    LeaveInvalidXmlState();
                    object ParsedObject = Source.ParseXml();

                    RunTask("Update element", null, (cc) => cc.Load(ParsedObject), () =>
                    {
                        UpdateListElements();
                        SelectedObject = ParsedObject;
                    });
                }
            }
            else if (ToTab == ViewMode.SOURCE)
            {
                UpdateXml(SelectedObject);
            }
        }

        /// <summary>
        /// set the xml text to given object
        /// </summary>
        /// <param name="obj">object that shall be displayed as xml</param>
        private void UpdateXml(object obj)
        {
            if (Source.Xml.Length == 0 || !InvalidXml)
                Source.SetXmlObject(SelectedObject);
        }

        /// <summary>
        /// As long as the source tab contains an invalid xml, the other views are locked
        /// </summary>
        private void EnterInvalidXmlState()
        {
            if (Status != null)
                Status.InvalidXmlWarning = "Xml ungültig. Bitte korrigieren.";
            SelectedPropertyGrid.IsEnabled = false;
            Visual.IsEnabled = false;
        }
        /// <summary>
        /// As long as the source tab contains an invalid xml, the other views are locked
        /// </summary>
        private void LeaveInvalidXmlState()
        {
            SelectedPropertyGrid.IsEnabled = true;
            Visual.IsEnabled = true;
            if (Status != null)
                Status.InvalidXmlWarning = "";
        }

        /// <summary>
        /// Get view mode from tab header name
        /// </summary>
        /// <param name="ViewName">tab header name</param>
        /// <returns>view mode</returns>
        private static ViewMode GetViewMode(string ViewName)
        {
            ViewName = ViewName.ToUpper();
            switch(ViewName)
            {
                case "XML":
                    return ViewMode.SOURCE;
                case "Eigenschaften":
                    return ViewMode.PROPERTYGRID;
                case "Vorschau":
                    return ViewMode.VISUAL;
                default:
                    return ViewMode.PROPERTYGRID;
            }
        }
    }

}
