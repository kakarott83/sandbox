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
using System.Windows.Shapes;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Statements;
using Workflows.BO;
using System.Activities.Presentation.Toolbox;
using System.Reflection;
using System.Activities;
using System.Activities.Presentation.Hosting;
using System.ServiceModel.Activities;
using Cic.One.Workflow.Activities;
using System.ServiceModel.Activities.Presentation.Factories;
using Cic.One.Workflow.Design;
using Cic.One.Workflow.Icons;
using Cic.One.Workflow.Design.Utils;
using System.Activities.Presentation.View;
using Microsoft.Windows.Controls.Ribbon;
using System.Activities.Debugger;
using System.Windows.Threading;
using Cic.One.DTO;
using Microsoft.VisualBasic.Activities;
using Workflows.BO.ExpressionEditor;
using System.Activities.XamlIntegration;
using System.Xaml;

namespace Workflows
{
    /// <summary>
    /// Rehosted WF4 Designer, uses DLLs from an installed VStudio2010
    /// 
    /// 
    /// 
    /// </summary>
    public partial class ReshostingWfDesigner : Window
    {
        private WorkflowDesigner wd;
        //private VSExpressionEditor editor;
        private EditorService editor;
        private String filename="";
        private IntelliScan iScan;
        public ReshostingWfDesigner()
        {
            iScan = new IntelliScan();
            InitializeComponent();
            
        }
        protected override void OnInitialized(EventArgs e)
        {
            
            base.OnInitialized(e);
            this.Icon = ResourceLoading.getIconSource("CICOne.png");
            // register metadata
            (new DesignerMetadata()).Register();

            //For custom activity icons in the designer
            ActivityLibraryMetadata.RegisterAll();

            // create the workflow designer

            createNewWorkflow(new Flowchart());
            
            
        }
       

        private void newDesigner()
        {
            //if (editor != null) editor.Dispose();

            //editor = new VSExpressionEditor();
            editor = new EditorService(iScan);
            wd = new WorkflowDesigner();
            //For intellisense editor
            editor.CreateEditorService(wd);
            wd.PropertyInspectorView.IsEnabled = true;
           
            DesignerBorder.Child = wd.View;
            PropertyBorder.Child = wd.PropertyInspectorView;
            
        }
        private void CloseWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
           // editor.Dispose();
        }

        /// <summary>
        /// Show the location in the window
        /// </summary>
        /// <param name="srcLoc"></param>
        void ShowDebug(SourceLocation srcLoc)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Render
                , (Action)(() =>
                {
                    this.wd.DebugManagerView.CurrentLocation = srcLoc;

                }));

        }

        private void newWorkflowClick(object sender, RoutedEventArgs e)
        {
            Control c = (Control)sender;
            if(mniNewFlowchart.Name.Equals(c.Name))
            {
                createNewWorkflow(new Flowchart());
            }
            if (mniNewSequence.Name.Equals(c.Name))
            {
                createNewWorkflow(new Sequence());
                
            }
        }

        private void createNewWorkflow(Activity a)
        {

            //http://msdn.microsoft.com/en-us/library/ff458319.aspx
            newDesigner();
            ActivityBuilder ab = new ActivityBuilder
            {
                Implementation = a,
            };
            ab.Properties.Add(new DynamicActivityProperty
            {
                Name = "input",
                Type = typeof(InOutArgument<WorkflowContext>),
                Value = new WorkflowContext()
            });
            ab.Name = "CRMWorkflow";
            wd.Load(ab);
        }
       /* private void UIFontSizeChange(object sender, RoutedEventArgs e)
        {
            RibbonButton but = (RibbonButton)sender;
            if (but.Name.Equals(btnUpSize.Name))
                toolbox.FontSize++;
            else
            {
                if(toolbox.FontSize>8)
                    toolbox.FontSize--;
            }
        }*/

        private void showOpenDialog(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension

            dlg.DefaultExt = ".xaml";

            dlg.Filter = "Workflow documents (.xaml)|*.xaml";



            // Display OpenFileDialog by calling ShowDialog method

            Nullable<bool> result = dlg.ShowDialog();



            // Get the selected file name and display in a TextBox

            if (result == true)
            {

                // Open document

                filename = dlg.FileName;

                newDesigner();

                //Activity instance = ActivityXamlServices.Load(ActivityXamlServices.CreateReader(new XamlXmlReader(filename, new XamlXmlReaderSettings { LocalAssembly = System.Reflection.Assembly.GetExecutingAssembly() })));
                //wd.Load(instance);
                wd.Load(filename);
                

            }

        }

        private void showCloseDialog(object sender, RoutedEventArgs e)
        {
             // Create OpenFileDialog

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();



            // Set filter for file extension and default file extension

            dlg.DefaultExt = ".xaml";

            dlg.Filter = "Workflow documents (.xaml)|*.xaml";
            if (filename == null || filename.Length == 0 || filename.IndexOf("\\") < 0)
                dlg.FileName = "NewWorkflow.xaml";
            else
            {
                dlg.FileName = filename.Substring(filename.LastIndexOf("\\") + 1);
                dlg.InitialDirectory = filename.Substring(0, filename.LastIndexOf("\\"));
            }
            

            // Display OpenFileDialog by calling ShowDialog method

            Nullable<bool> result = dlg.ShowDialog();



            // Get the selected file name and display in a TextBox

            if (result == true)
            {

                filename = dlg.FileName;

                wd.Save(filename);
                
                

            }
            
        }

        private void closeApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       
    }
}
