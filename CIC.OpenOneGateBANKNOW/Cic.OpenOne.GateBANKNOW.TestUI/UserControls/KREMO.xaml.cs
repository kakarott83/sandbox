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
using AutoMapper.Configuration;

namespace Cic.OpenOne.GateBANKNOW.TestUI.UserControls
{
    /// <summary>
    /// Interaction logic for KREMO.xaml
    /// </summary>
    public partial class KREMO : UserControl
    {
        private ColumnDefinition _extraColLayerInput;
        KREMOInput _ki;
        StateControl KREMOStatus = new StateControl();

        /// <summary>
        /// KREMO
        /// </summary>
        public KREMO()
        {
            InitializeComponent();
            _extraColLayerInput = new ColumnDefinition();
            _extraColLayerInput.Width = new GridLength(500);
            _extraColLayerInput.SharedSizeGroup = "pinCol";
            LayerOutput.Visibility = Visibility.Visible;
            btnPinIt.IsChecked = true;
            LayerInput.DataContext = _ki;
            LayerOutput.Visibility = Visibility.Visible;
            btnPinIt.IsChecked = true;



            //KREMO Initialize
            _ki = new KREMOInput();
          //  _ki.GebDatum = 19700101;
          //_ki.GebDatum2 = 19700101;
            LayerInput.DataContext = _ki;

        }



        #region pinning
        private void HandlePinning(object sender, RoutedEventArgs e)
        {
            LayerInput.ColumnDefinitions.Add(_extraColLayerInput);
            btnShowOutputControl.Visibility = Visibility.Collapsed;
            pinImage.Source = new BitmapImage(new Uri(@"..\Images\pin.png", UriKind.Relative));
        }

        private void HandleUnpinning(object sender, RoutedEventArgs e)
        {
            LayerInput.ColumnDefinitions.Remove(_extraColLayerInput);
            btnShowOutputControl.Visibility = Visibility.Visible;
            pinImage.Source = new BitmapImage(new Uri(@"..\Images\pin2.png", UriKind.Relative));
        }


        void HandleButtonExpMouseEnter(object sender, RoutedEventArgs e)
        {
            if (LayerOutput.Visibility != Visibility.Visible)
            {
                LayerOutputTrans.X = LayerOutput.ColumnDefinitions[1].Width.Value;
                LayerOutput.Visibility = Visibility.Visible;
                DoubleAnimation ani = new DoubleAnimation(0,
                new Duration(TimeSpan.FromMilliseconds(500)));
                LayerOutputTrans.BeginAnimation(TranslateTransform.XProperty, ani);
            }
        }

        void HandleLayerInputMouseEnter(object sender, RoutedEventArgs e)
        {
            if (!btnPinIt.IsChecked.GetValueOrDefault()
              && LayerOutput.Visibility == Visibility.Visible)
            {
                double to = LayerOutput.ColumnDefinitions[1].Width.Value;
                DoubleAnimation ani = new DoubleAnimation(to,
                new Duration(TimeSpan.FromMilliseconds(500)));
                ani.Completed += new EventHandler(ani_Completed);
                LayerOutputTrans.BeginAnimation(TranslateTransform.XProperty, ani);
            }
        }

        void ani_Completed(object sender, EventArgs e)
        {
            LayerOutput.Visibility = Visibility.Collapsed;
        }



        #endregion

        /// <summary>
        /// Handle Button KREMO CLick
        /// Test UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleButtonKREMOClick(object sender, RoutedEventArgs e)
        {

            // KREMO
            Mouse.OverrideCursor = Cursors.Wait;
            KREMOStatus.setIni();
            this.statusblock.DataContext = null;
            this.statusblock.DataContext = KREMOStatus;
            if (OutputObject.Items.Count > 0)
                OutputObject.Items.Clear();
            ObjectExplorer.DataContext = null;
            try
            {

                Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.KREMOcallByValues KREMOcallByValues = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.KREMOcallByValues();
                if (textBoxKREMO.Text != "")
                {
                    Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AuskunftDto output;
                    long a = Int64.Parse(textBoxKREMO.Text);
                    output = KREMOcallByValues.doAuskunft(a);
                    this.OutputObject.Items.Add(DataLoader.LoadTree2(output));
                    this.statusblock.DataContext = null;
                    KREMOStatus.setEnd();
                    this.statusblock.DataContext = KREMOStatus;

                }
                else
                {
                    AuskunftDto output;
                    IMapper mapper = Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("CONVERSION", delegate (MapperConfigurationExpression Config)
                    {
                        Config.CreateMap<KREMOInput, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.KREMOInDto>();
                        Config.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AuskunftDto, AuskunftDto>();

                    }, true);

                   
                    Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.KREMOInDto input = Mapper.Map<KREMOInput, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.KREMOInDto>(_ki);
                    output = mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.AuskunftDto, AuskunftDto>(KREMOcallByValues.doAuskunft(input));
                    this.OutputObject.Items.Add(DataLoader.LoadTree2(output));
                    this.statusblock.DataContext = null;
                    KREMOStatus.setEnd();
                    this.statusblock.DataContext = KREMOStatus;

                }

            }
            catch (Exception ex)
            {
                KREMOStatus.setError();
                this.statusblock.DataContext = null;
                this.statusblock.DataContext = KREMOStatus;
                MessageBox.Show("Exception caught. " + ex);
                this.statusblock.DataContext = null;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }



        private void ObjectItemSelected_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedTVI = (TreeViewItem)OutputObject.SelectedItem;
            if (selectedTVI != null)
                ObjectExplorer.DataContext = DataLoader.LoadDTO(selectedTVI.Tag);

            /*   else
               {
                   MessageBox.Show("Keine Attribute ausgewählt!");
               }*/


        }

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




    }
}
