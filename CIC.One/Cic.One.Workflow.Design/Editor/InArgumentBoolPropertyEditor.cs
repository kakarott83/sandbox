using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;
using System.Windows.Markup;
using System.IO;


namespace Cic.One.Workflow.Design.Editor
{
    /// <summary>
    /// http://blogs.msdn.com/b/tilovell/archive/2011/04/06/wf4-showing-an-inargument-lt-bool-gt-as-a-checkbox-in-the-workflow-designer-property-grid.aspx
    /// </summary>
    public class InArgumentBoolPropertyEditor : PropertyValueEditor
    {
     

        public InArgumentBoolPropertyEditor()
        {

            ResourceDictionary resources = Application.Current.Resources;
            this.InlineEditorTemplate = (DataTemplate)resources["ArgumentBoolLiteralPropertyEditor"];

        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
