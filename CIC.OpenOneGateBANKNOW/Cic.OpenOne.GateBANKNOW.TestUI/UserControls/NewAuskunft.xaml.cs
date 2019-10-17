using System;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using System.Windows.Media.Animation;
using Cic.OpenOne.GateBANKNOW.TestUI.Converters;
using Cic.OpenOne.GateBANKNOW.TestUI.UserControls;
using Cic.OpenOne.GateBANKNOW.TestUI.DTOS;
using Cic.OpenOne.GateBANKNOW.TestUI.DataAccess;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.IO.Compression;
using System.Reflection;
using AutoMapper;

namespace Cic.OpenOne.GateBANKNOW.TestUI.UserControls
{
    /// <summary>
    /// Interaction logic for NewAuskunft.xaml
    /// </summary>
    public partial class NewAuskunft : UserControl
    {
        private ColumnDefinition _extraColLayerInputAuskunft;
        // dynamic _ei;
        object _eiorig;
        object _BO;
        string _methode = "execute";
        string filter = "";

        StateControl auskunftStatus = new StateControl();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auskunftTitle"></param>
        /// <param name="obj"></param>
        /// <param name="BO"></param>
        /// <param name="typeObj"></param>
        /// <param name="methode"></param>
        /// <param name="Filter"></param>
        public NewAuskunft(string auskunftTitle, object obj, object BO, Type typeObj, string methode, string Filter)
        {
            InitializeComponent();
            btnShowOutputAuskunft.Content = auskunftTitle;
            btnShowOutputControlAuskunft.Content = auskunftTitle;
            _extraColLayerInputAuskunft = new ColumnDefinition();
            _extraColLayerInputAuskunft.Width = new GridLength(700);
            _extraColLayerInputAuskunft.SharedSizeGroup = "pinColAuskunft";
            LayerOutputAuskunft.Visibility = Visibility.Visible;
            btnPinItAuskunft.IsChecked = true;
            _eiorig = obj;
            _BO = BO;
            filter = Filter;
            GeneratorInput ginput = new GeneratorInput();
            ginput.filter = Filter;
            if (methode != null && methode != "")
                _methode = methode;
            if (_eiorig == null)
                _eiorig = Activator.CreateInstance(typeObj);

            ginput.GenerateObjectInputInit(_eiorig, ObjectInputAuskunft, "");
            dynamic _ei = new NotifyPropertyChangedProxy(_eiorig);
        }

        // Pinning Methoden
        #region Pinning
        private void HandlePinningAuskunft(object sender, RoutedEventArgs e)
        {
            LayerInputAuskunft.ColumnDefinitions.Add(_extraColLayerInputAuskunft);
            btnShowOutputControlAuskunft.Visibility = Visibility.Collapsed;
            pinImageAuskunft.Source = new BitmapImage(new Uri(@"..\Images\pin.png", UriKind.Relative));
        }

        private void HandleUnpinningAuskunft(object sender, RoutedEventArgs e)
        {
            LayerInputAuskunft.ColumnDefinitions.Remove(_extraColLayerInputAuskunft);
            btnShowOutputControlAuskunft.Visibility = Visibility.Visible;
            pinImageAuskunft.Source = new BitmapImage(new Uri(@"..\Images\pin2.png", UriKind.Relative));
        }


        void HandleButtonExpMouseEnterAuskunft(object sender, RoutedEventArgs e)
        {

            if (LayerOutputAuskunft.Visibility != Visibility.Visible)
            {
                LayerOutputTransAuskunft.X = LayerOutputAuskunft.ColumnDefinitions[1].Width.Value;
                LayerOutputAuskunft.Visibility = Visibility.Visible;
                DoubleAnimation aniAuskunft = new DoubleAnimation(0,
                new Duration(TimeSpan.FromMilliseconds(500)));
                LayerOutputTransAuskunft.BeginAnimation(TranslateTransform.XProperty, aniAuskunft);
            }
        }

        void HandleLayerInputAuskunftMouseEnter(object sender, RoutedEventArgs e)
        {
            if (!btnPinItAuskunft.IsChecked.GetValueOrDefault()
              && LayerOutputAuskunft.Visibility == Visibility.Visible)
            {
                double to = LayerOutputAuskunft.ColumnDefinitions[1].Width.Value;


                DoubleAnimation aniAuskunft = new DoubleAnimation(to,
                  new Duration(TimeSpan.FromMilliseconds(500)));
                aniAuskunft.Completed += new EventHandler(aniAuskunft_Completed);
                LayerOutputTransAuskunft.BeginAnimation(TranslateTransform.XProperty, aniAuskunft);
            }
        }

        void aniAuskunft_Completed(object sender, EventArgs e)
        {

            LayerOutputAuskunft.Visibility = Visibility.Collapsed;
        }
        #endregion


        // Handle Methoden
        /// <summary>
        /// Handle Information Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleButtonAuskunftClick(object sender, RoutedEventArgs e)
        {
            Type thisType = _BO.GetType();
            Type thisTypeObj = _eiorig.GetType();
            Mouse.OverrideCursor = Cursors.Wait;
            auskunftStatus.setIni();
            statusblock.DataContext = null;
            this.statusblock.DataContext = auskunftStatus;
            if (OutputObjectAuskunft.Items.Count > 0)
                OutputObjectAuskunft.Items.Clear();
            ObjectExplorerAuskunft.DataContext = null;
            try
            {
                if (textBoxAuskunft.Text != "")
                {
                    long a = Int64.Parse(textBoxAuskunft.Text);
                    MethodInfo theMethod = thisType.GetMethod(_methode, new[] { typeof(long) });
                    object[] mParam = new object[] { a };
                    this.OutputObjectAuskunft.Items.Add(DataLoader.LoadTree2(theMethod.Invoke(_BO, mParam)));
                    auskunftStatus.setEnd();
                    this.statusblock.DataContext = null;
                    this.statusblock.DataContext = auskunftStatus;
                }
                else
                {
                    MethodInfo theMethod = thisType.GetMethod(_methode, new[] { thisTypeObj });
                    object[] mParam = new object[] { _eiorig };
                    this.OutputObjectAuskunft.Items.Add(DataLoader.LoadTree2(theMethod.Invoke(_BO, mParam)));
                    auskunftStatus.setEnd();
                    this.statusblock.DataContext = null;
                    this.statusblock.DataContext = auskunftStatus;
                }
            }
            catch (Exception ex)
            {
                auskunftStatus.setError();
                this.statusblock.DataContext = null;
                this.statusblock.DataContext = auskunftStatus;
                MessageBox.Show("Exception caught. " + ex);
                this.statusblock.DataContext = null;
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObjectExplorer_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "typeAttribute")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString() == "nameAttribute")
            {
                e.Column.Header = "Name";
            }
            if (e.Column.Header.ToString() == "valueAttribute")
            {
                e.Column.Header = "Value";
            }
            if (e.PropertyName == "objectAttribute")
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObjectAuskunftItemSelected_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedTVI = (TreeViewItem)OutputObjectAuskunft.SelectedItem;
            if (selectedTVI != null)
                ObjectExplorerAuskunft.DataContext = DataLoader.LoadDTOFilter(selectedTVI.Tag, filter);
            //else
            //{
            //    MessageBox.Show("Keine Attribute ausgewählt!");
            //}
        }

        /// <summary>
        /// Object_ItemChanged Event Handler
        /// </summary>
        /// <param name="sender">Sendendes Control</param>
        /// <param name="e">Parameter</param>
        public void Object_ItemChanged(object sender, EventArgs e)
        {
            FrameworkElement s = sender as FrameworkElement;
            DependencyObject parent = s.Parent;

            Type underlyingType = null;
            if (s != null)
            {
                if ((s.GetType().IsGenericType) && (s.GetType().GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    underlyingType = Nullable.GetUnderlyingType(s.GetType());
                }
                else
                {
                    underlyingType = s.GetType();
                }
            }
        }
    }
}