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
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace XmlConfiguratorBase.BO.GUI
{
    /// <summary>
    /// Interaction logic for NewInstanceEditor.xaml
    /// </summary>
    public partial class NewInstanceEditor : UserControl, ITypeEditor
    {
        /// <summary>
        /// object which is edited
        /// </summary>
        private PropertyItem ItemForEditing = null;
        /// <summary>
        /// There is just one Button for deleting and creating an instance. It needs to say what it does.
        /// </summary>
        private string ButtonText
        {
            get
            {
                if (ItemForEditing == null)
                    return "";
                if (ItemForEditing.Value == null)
                    return "new";
                return "del";
            }
        }

        /// <summary>
        /// Editor for complex nullable objects
        /// </summary>
        public NewInstanceEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open the editor for the instantiable object
        /// </summary>
        /// <param name="propertyItem">instantiable object</param>
        /// <returns>editor display element</returns>
        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            ItemForEditing = propertyItem;
            DataContext = ItemForEditing.Value;
            ButtonNewDelete.Content = ButtonText;
            return this;
        }

        /// <summary>
        /// create a new object of the property type and make that the new value
        /// </summary>
        private void CreateNewInstance()
        {
            try
            {
                ItemForEditing.Value = Activator.CreateInstance(ItemForEditing.PropertyType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// delete current instance of the object
        /// </summary>
        private void DeleteInstance()
        {
            ItemForEditing.Value = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ItemForEditing.Value == null)
                CreateNewInstance();
            else
                DeleteInstance();

            ButtonNewDelete.Content = ButtonText;
        }
    }
}
