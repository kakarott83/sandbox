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

namespace WfvXmlConfigurator.BO.GUI
{
    /// <summary>
    /// Interaction logic for StringArrayEditorControl.xaml
    /// </summary>
    public partial class StringArrayEditor : UserControl, ITypeEditor
    {
        /// <summary>
        /// string array which is edited
        /// </summary>
        private PropertyItem ArrayPropertyItem = null;
        /// <summary>
        /// List of strings representating the string array in a way that allows adding and removing elements smoothly
        /// </summary>
        private List<string> Listenelemente = new List<string>();
        /// <summary>
        /// helper for indicating wheather a textbox change is self-induced and needs to be ignored
        /// </summary>
        private bool CurrentlyChanging = false;

        /// <summary>
        /// Initialize Editor with given value
        /// </summary>
        /// <param name="propertyItem">value to be edited</param>
        public StringArrayEditor()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Provide Editor
        /// </summary>
        /// <param name="propertyItem">element to be edited</param>
        /// <returns>editor for editing string arrays</returns>
        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            ArrayPropertyItem = propertyItem;
            if (ArrayPropertyItem.Value != null && ArrayPropertyItem.Value is ICollection<string>)
                Listenelemente.AddRange((ICollection<string>)ArrayPropertyItem.Value);
            Liste.ItemsSource = Listenelemente;
            if (Liste.Items.Count > 0)
                Liste.SelectedIndex = 0;

            return this;
        }

        /// <summary>
        /// Add a new string to the array and select it
        /// Move cursor to editing textbox for editing the new text
        /// </summary>
        /// <param name="text">new string for the array</param>
        private void AddNewText(string text)
        {
            Listenelemente.Add(text);
            Liste.Items.Refresh();
            Liste.SelectedIndex = Liste.Items.Count - 1;

            Textediting.Focus();
        }

        /// <summary>
        /// Remove the selected item
        /// After that, select the following item. If it was the last item, select the previous one
        /// </summary>
        private void RemoveSelectedItem()
        {
            if (Liste.SelectedItem == null)
                return;
            if (!(Liste.SelectedItem is string))
                return;

            int selectedIndex = Liste.SelectedIndex;

            Listenelemente.Remove((string)Liste.SelectedItem);
            Liste.Items.Refresh();

            selectedIndex = Math.Min(selectedIndex, Liste.Items.Count - 1);
            Liste.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// The user clicked the button for adding a new element to the array
        /// </summary>
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewText("");
        }

        /// <summary>
        /// The user clicked the button for removing the selected element of the array
        /// </summary>
        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelectedItem();
        }

        /// <summary>
        /// The user typed something to the list element editing textbox
        /// If there is no element selected, we add a new one with the textbox text as value (given it's not an empty string because we just removed the element)
        /// </summary>
        private void Textediting_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null || !(sender is TextBox))
                return;

            if (CurrentlyChanging)
                return;
            CurrentlyChanging = true;

            TextBox editbox = (TextBox)sender;
            int editboxSelectionStart = editbox.SelectionStart;
            int editboxSelectionLength = editbox.SelectionLength;

            if (Liste.SelectedIndex < 0)
            {
                if (editbox.Text.Length == 0)
                {
                    CurrentlyChanging = false;
                    return;
                }
                AddNewText(editbox.Text);
            }
            else
            {
                int selectedListIndex = Liste.SelectedIndex;
                Listenelemente[selectedListIndex] = editbox.Text;
                Liste.Items.Refresh();
                Liste.SelectedIndex = selectedListIndex;
            }

            editbox.SelectionStart = editboxSelectionStart;
            editbox.SelectionLength = editboxSelectionLength;

            ArrayPropertyItem.Value = Listenelemente.ToArray();
            CurrentlyChanging = false;
        }

        /// <summary>
        /// The user wants to see more (or less) infos of the string array
        /// </summary>
        private void ButtonMore_Click(object sender, RoutedEventArgs e)
        {
            Visibility visibility = Visibility.Collapsed;
            if (Liste.Visibility == Visibility.Collapsed)
                visibility = Visibility.Visible;
            
            Liste.Visibility = visibility;
            Buttons.Visibility = visibility;
        }

    }
}
