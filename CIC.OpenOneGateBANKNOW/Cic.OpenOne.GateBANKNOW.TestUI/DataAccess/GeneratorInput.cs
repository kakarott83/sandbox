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
using System.Windows.Navigation;


namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    class GeneratorInput
    {
        private int crows = -1; // count of rows
        public string filter = ""; //  list of Custommer Attributes 
        public GeneratorInput()
        {

        }

        private void ButtonTreeArray_Click(object sender, RoutedEventArgs e)
        {
     
        }


        private void ButtonTreeListe_Click(object sender, RoutedEventArgs e)
        {

        }
        // File load button action
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            Stream stream;
            Button b = e.Source as Button;
            FrameworkElement s = sender as FrameworkElement;
            DependencyObject parent = s.Parent;
            Grid g = parent as Grid;
            Grid pg = g.Parent as Grid;
            OpenFileDialog dlg = new OpenFileDialog();
            ListView lw = (ListView)LogicalTreeHelper.FindLogicalNode(pg, "notificationbodylw");
            if (dlg.ShowDialog() == true)
            {
                Byte[] buffer = null;
                if ((stream = dlg.OpenFile()) != null)
                {
                    if (stream != null && stream.Length > 0)
                    {
                        using (BinaryReader br = new BinaryReader(stream))
                        {
                            buffer = br.ReadBytes((Int32)stream.Length);
                        }
                    }

                    lw.ItemsSource = buffer;
                    stream.Close();
                }
            }

            //Explicit
            //   BindingExpression be = ((ListView)LogicalTreeHelper.FindLogicalNode(g, "notificationbodylw")).GetBindingExpression(ListView.ItemsSourceProperty);
            //   string ss = be.ParentBinding.Path;
            //   be.UpdateSource();





        }

        /// <summary>
        /// Calls a method that builds wpf controls and bindings for the attributes of the object e, adds these to Container ObjectInput and creates the instances of the specified type.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ObjectInput"></param>
        /// <param name="classorig"></param>
        public void GenerateObjectInputInit(Object e, Grid ObjectInput, string classorig)
        {
            GenerationInitial(e, classorig, null, null, "", filter, ObjectInput);
        }

        /// <summary>
        /// Calls a method that builds wpf controls for the attributes of object e. 
        /// the attributes have initial values in e.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ObjectInput"></param>
        public void GenerateObjectInput(Object e, Grid ObjectInput)
        {
            Generation(e, null, null, "", filter, ObjectInput);
        }

        /// <summary>
        /// Generation builds wpf controls and bindings for each attribute from objet e and adds these to Container ObjectInput
        /// filter is a string with information about which properties of e will not be includes. 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ancestor"></param>
        /// <param name="pancestor"></param>
        /// <param name="vorlabel"></param>
        /// <param name="filter"></param>
        /// <param name="ObjectInput"></param>
        private void Generation(Object e, Object ancestor, PropertyInfo pancestor, string vorlabel, string filter, Grid ObjectInput)
        {

            if (!(e == null || e.GetType().IsArray || e.GetType().IsPrimitive || e.GetType().IsEnum || e.GetType().IsValueType || e.GetType().ToString().Equals("System.String")))
            {

                Type t = e.GetType();
                PropertyInfo[] pi = t.GetProperties();

                foreach (PropertyInfo p in pi)
                {
                    if (p.GetValue(e, null) != null && p.GetValue(e, null).GetType().GetProperty("Count") != null)
                    {

                        string labeltemp = getLabel(p);

                        PropertyInfo pCount = p.GetValue(e, null).GetType().GetProperty("Count");

                        int count = (int)pCount.GetValue(p.GetValue(e, null), null);
                        PropertyInfo pItem = p.GetValue(e, null).GetType().GetProperty("Item");

                        if (vorlabel != "")
                            labeltemp = vorlabel + "." + labeltemp;

                        ObjectInput.RowDefinitions.Add(new RowDefinition());
                        Border bordertree = new Border();
                        bordertree.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;

                        TreeViewItem treeformular = new TreeViewItem();
                        treeformular.Header = labeltemp;

                        int tempcrows = crows;

                        for (int i = 0; i < count; i++)
                        {
                            TreeViewItem item = new TreeViewItem();
                            item.Header = pItem.Name;

                            //          Border b = new Border();
                            //          b.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;


                            Grid tempgrid = new Grid();
                            crows = -1;
                            object obj = pItem.GetValue(p.GetValue(e, null), new object[] { i });
                            item.Header = labeltemp + "[" + i + "]";
                            Generation(obj, e, p, "", filter, tempgrid);
                            //    b.Child = tempgrid;
                            item.Items.Add(tempgrid);
                            treeformular.Items.Add(item);

                        }

                        crows = tempcrows + 1;

                        bordertree.Child = treeformular;

                        Grid.SetRow(bordertree, crows);
                        Grid.SetColumn(bordertree, 0);

                        ObjectInput.Children.Add(bordertree);


                    }
                    else
                    {
                        if (pancestor != null)

                            Generation(p.GetValue(e, null), e, p, vorlabel + pancestor.Name, filter, ObjectInput);
                        else
                            Generation(p.GetValue(e, null), e, p, vorlabel, filter, ObjectInput);

                    }

                }

            }
            else
            {




                if (pancestor.GetValue(ancestor, null) != null && pancestor.PropertyType.IsArray)
                {

                    string labeltemp = getLabel(pancestor);
                    IList array = pancestor.GetValue(ancestor, null) as IList;
                    var iarray = 0;
                    ObjectInput.RowDefinitions.Add(new RowDefinition());
                    Border bordertree = new Border();
                    bordertree.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;
                    TreeViewItem treeformular = new TreeViewItem();
                    treeformular.Header = labeltemp;

                    int tempcrows = crows;
                    foreach (var arrElement in array)
                    {


                        if (vorlabel != "")
                            labeltemp = vorlabel + "." + labeltemp;
                        Border b = new Border();
                        b.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;
                        TreeViewItem item = new TreeViewItem();

                        Grid tempgrid = new Grid();
                        crows = -1;
                        item.Header = labeltemp + "[" + iarray + "]";

                        Generation(arrElement, ancestor, pancestor, "", filter, tempgrid);


                        iarray++;

                        b.Child = tempgrid;
                        item.Items.Add(b);
                        treeformular.Items.Add(item);

                    }

                    crows = tempcrows + 1;


                    bordertree.Child = treeformular;
                    Grid.SetRow(bordertree, crows);
                    Grid.SetColumn(bordertree, 0);

                    ObjectInput.Children.Add(bordertree);


                }



                else
                {
                    if (pancestor != null)
                    {
                        if (getFilterAtt(pancestor).Contains(filter) || getFilterAtt(pancestor)=="keine")
                        {
                            crows++;
                            string labeltemp = getLabel(pancestor);
                            ObjectInput.RowDefinitions.Add(new RowDefinition());
                            if (pancestor.PropertyType == typeof(Nullable<DateTime>) || pancestor.PropertyType == typeof(DateTime))
                            {
                                DatePicker datepicker = new DatePicker();
                                Binding binding = new Binding();
                                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                                binding.Source = ancestor;

                                binding.Path = new PropertyPath(pancestor);

                                datepicker.SetBinding(DatePicker.SelectedDateProperty, binding);
                                datepicker.MinWidth = 200;
                                Grid tempgrid2 = createGrid(labeltemp, datepicker);
                                ObjectInput.RowDefinitions.Add(new RowDefinition());
                                Grid.SetRow(tempgrid2, crows);
                                ObjectInput.Children.Add(tempgrid2);




                            }
                            else
                            {
                                if (pancestor.PropertyType.BaseType == typeof(Enum))
                                {
                                    ComboBox text = new ComboBox();

                                    Binding binding = new Binding();
                                    binding.Mode = BindingMode.TwoWay;
                                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                                    binding.Source = ancestor;
                                    binding.Path = new PropertyPath(pancestor);



                                    text.SetBinding(ComboBox.SelectedItemProperty, binding);
                                    text.ItemsSource = Enum.GetValues(pancestor.PropertyType);


                                    Grid tempgrid2 = createGrid(labeltemp, text);
                                    ObjectInput.RowDefinitions.Add(new RowDefinition());
                                    Grid.SetRow(tempgrid2, crows);
                                    ObjectInput.Children.Add(tempgrid2);


                                }

                                else
                                {

                                    TextBox text = new TextBox();
                                    text.FontSize = 12;
                                    text.FontWeight = FontWeights.Bold;
                                    Binding binding = new Binding();
                                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;


                                    binding.Source = ancestor;
                                    binding.Path = new PropertyPath(pancestor);
                                    text.SetBinding(TextBox.TextProperty, binding);
                                    Grid tempgrid2 = createGrid(labeltemp, text);
                                    Grid.SetRow(tempgrid2, crows);
                                    ObjectInput.Children.Add(tempgrid2);


                                }








                            }

                        }

                    }

                }

            }

        }

        /// <summary>
        /// Builds wpf controls for each attribute of the object e and adds these to Container ObjectInput
        /// filter is a string with information about which properties of e will not be includes. 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="classname"></param>
        /// <param name="ancestor"></param>
        /// <param name="pancestor"></param>
        /// <param name="vorlabel"></param>
        /// <param name="filter"></param>
        /// <param name="ObjectInput"></param>
        public void GenerationInitial(Object e, string classname, Object ancestor, PropertyInfo pancestor, string vorlabel, string filter, Grid ObjectInput)
        {
            
            if ((e == null) && (pancestor.PropertyType.IsArray))
            {   
                // This property is a Array and there isn't a instance.
                string labeltemp = getLabel(pancestor);
                ObjectInput.RowDefinitions.Add(new RowDefinition());
                if (pancestor.PropertyType.GetElementType().Name != "Byte")
                {
                    Type listType = typeof(List<>).MakeGenericType(new[] { pancestor.PropertyType.GetElementType() });
                    IList larray = (IList)Activator.CreateInstance(listType);
                    Border bordertree = new Border();
                    bordertree.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;
                    TreeViewItem treeformular = new TreeViewItem();
                    treeformular.Header = labeltemp;
                    treeformular.Name = "treeArray";                    
                    crows++;
                    Grid gridTemp = new Grid();
                 
                    gridTemp.RowDefinitions.Add(new RowDefinition());
                   
                    ContextMenu arrayMenu = new ContextMenu();
                    MenuItem plusMenuItem = new MenuItem();
                    plusMenuItem.Header = "Element hinzufügen";
                    plusMenuItem.Command = new CommandArray();

                    CommandParam param2 = new CommandParam();
                    plusMenuItem.CommandParameter = param2;
                    arrayMenu.Items.Add(plusMenuItem);
                    param2.ancestor = ancestor;
                    param2.larray = larray;
                    param2.tree = treeformular;
                    param2.pancestor = pancestor;
                    param2.filter = filter;
                    param2.vorlabel = vorlabel;
                    param2.grid = gridTemp;


                    MenuItem minusMenuItem = new MenuItem();
                    minusMenuItem.Header = "Leeren";
                    minusMenuItem.Command = new CommandRemove();
                   
                    minusMenuItem.CommandParameter = param2;
                    arrayMenu.Items.Add(minusMenuItem);
                  

                   
                    treeformular.ContextMenu = arrayMenu;






                 //   Grid.SetRow(b, 0);
                 //   Grid.SetColumn(b, 1);
                   // gridTemp.Children.Add(b);
                    bordertree.Child = treeformular;
                    Grid.SetRow(bordertree, 0);
                    Grid.SetColumn(bordertree, 0);
                    gridTemp.Children.Add(bordertree);
                    Grid.SetRow(gridTemp, crows);
                    Grid.SetColumn(gridTemp, 1);
                    ObjectInput.Children.Add(gridTemp);
                    
                   
                    
                    
                    
                   // pancestor.SetValue(ancestor, larray, null);
                }
                else
                {   // This property is a Array of Bytes and there isn't a instance.
                    crows++;
                    Array array = Array.CreateInstance(pancestor.PropertyType.GetElementType(), 100);
                    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                    array = enc.GetBytes("Das is ein Test");
                    pancestor.SetValue(ancestor, array, null);
                    Button button = new Button();
                    button.Click += new RoutedEventHandler(this.ButtonLoad_Click);
                    button.Content = "Datei laden";
                    Grid gridTextBox = createGrid(labeltemp, button);
                    Grid.SetRow(gridTextBox, crows);
                    Grid.SetColumn(gridTextBox, 0);
                    ObjectInput.Children.Add(gridTextBox);
                    ObjectInput.RowDefinitions.Add(new RowDefinition());
                    crows++;
                    ListView lw = new ListView();
                    //lw.ItemsSource = array;
                    lw.MaxHeight = 40;
                    lw.Name = "notificationbodylw";
                    lw.Visibility = Visibility.Hidden;
                    Binding binding = new Binding();
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    binding.Source = ancestor;
                    binding.Mode = BindingMode.TwoWay;
                    binding.Path = new PropertyPath(pancestor);
                    lw.SetBinding(ListView.ItemsSourceProperty, binding);
                    Grid.SetRow(lw, crows);
                    Grid.SetColumn(lw, 0);
                    ObjectInput.Children.Add(lw);
                }
            }
            else
                if (e != null && e.GetType().IsArray)
                {   // This property is a Array and there is a instance.
                    string labeltemp = getLabel(pancestor);
                    IList array = e as IList;
                    Type listType = typeof(List<>).MakeGenericType(new[] { pancestor.PropertyType.GetElementType() });
                    IList larray = (IList)Activator.CreateInstance(listType);
                    
                    var iarray = 0;
                    ObjectInput.RowDefinitions.Add(new RowDefinition());
                    Border bordertree = new Border();
                    bordertree.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;
                    TreeViewItem treeformular = new TreeViewItem();
                    treeformular.Header = labeltemp;
                    int tempcrows = crows;
                    foreach (var arrElement in array)
                    {
                        if (vorlabel != "")
                            labeltemp = vorlabel + "." + labeltemp;
                   
                        Grid tempgrid = new Grid();
                        crows = -1;
                    
                       GenerationInitial(arrElement, pancestor.PropertyType.ToString(), ancestor, pancestor, vorlabel,filter, tempgrid);
          
                    
                        iarray++;
                
                        treeformular.Items.Add(tempgrid);
                        larray.Add(arrElement);
                    }
                    crows = tempcrows + 1;
                    Grid gridTemp = new Grid();

                    gridTemp.RowDefinitions.Add(new RowDefinition());
                    ContextMenu arrayMenu = new ContextMenu();
                    MenuItem plusMenuItem = new MenuItem();
                    plusMenuItem.Header = "Add Element";
                    plusMenuItem.Command = new CommandArray();

                    CommandParam param2 = new CommandParam();
                    plusMenuItem.CommandParameter = param2;
                    arrayMenu.Items.Add(plusMenuItem);
                    param2.ancestor = ancestor;
                    param2.larray = larray;
                    param2.tree = treeformular;
                    param2.pancestor = pancestor;
                    param2.filter = filter;
                    param2.vorlabel = vorlabel;
                    param2.grid = gridTemp;


                    MenuItem minusMenuItem = new MenuItem();
                    minusMenuItem.Header = "Clear";
                    minusMenuItem.Command = new CommandRemove();

                    minusMenuItem.CommandParameter = param2;
                    arrayMenu.Items.Add(minusMenuItem);



                    treeformular.ContextMenu = arrayMenu;
                    bordertree.Child = treeformular;
                    gridTemp.Children.Add(bordertree);
                    Grid.SetRow(gridTemp, crows);
                    Grid.SetColumn(gridTemp, 1);
                    ObjectInput.Children.Add(gridTemp);
                }
                else
                    if (e == null && pancestor != null && pancestor.PropertyType.GetProperty("Count") != null)
                    {
                        // This property is a List and there isn't a instance.
                        string labeltemp = getLabel(pancestor);
                        PropertyInfo pItem = pancestor.PropertyType.GetProperty("Item");
                        if (vorlabel != "")
                            labeltemp = vorlabel + "." + labeltemp;
                        ObjectInput.RowDefinitions.Add(new RowDefinition());
                        Border bordertree = new Border();
                        bordertree.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;
                        TreeViewItem treeformular = new TreeViewItem();
                        treeformular.Header = labeltemp;
                        int tempcrows = crows;
                        Type listType = typeof(List<>).MakeGenericType(new[] { pItem.PropertyType });
                        IList liste = (IList)Activator.CreateInstance(listType);
                     
                        crows++;
                        Grid gridTemp = new Grid();
                     
                        ContextMenu listeMenu = new ContextMenu();
                        MenuItem plusMenuItem = new MenuItem();
                        plusMenuItem.Header = "Element hinzufügen";
                        plusMenuItem.Command = new CommandListe();
                          
                        CommandParam param2 = new CommandParam(); 
                        plusMenuItem.CommandParameter = param2;
                        listeMenu.Items.Add(plusMenuItem);
                        param2.ancestor = ancestor;
                        param2.liste = liste;
                        param2.tree = treeformular;
                        param2.pancestor = pancestor;
                        param2.filter = filter;
                        param2.vorlabel = vorlabel;
                        param2.grid = gridTemp;
                        treeformular.ContextMenu = listeMenu;


                      
                        MenuItem minusMenuItem = new MenuItem();
                        minusMenuItem.Header = "Leeren";
                        minusMenuItem.Command = new CommandRemove();
                        minusMenuItem.CommandParameter = param2;
                        listeMenu.Items.Add(minusMenuItem);
                      
                     

                        crows = tempcrows + 1;

                        DockPanel.SetDock(treeformular, Dock.Bottom);
                        
                        bordertree.Child = treeformular;
                        Grid.SetRow(bordertree, crows);
                        Grid.SetColumn(bordertree, 0);
                        ObjectInput.Children.Add(bordertree);
                     //   pancestor.SetValue(ancestor, liste, null);
                    }
                    else
                        if (e != null && pancestor != null && e.GetType().GetProperty("Count") != null)
                        {
                            // This property is a List and there is a instance.
                            string labeltemp = getLabel(pancestor);
                            PropertyInfo pCount = e.GetType().GetProperty("Count");
                            int count = (int)pCount.GetValue(e, null);
                            PropertyInfo pItem = e.GetType().GetProperty("Item");
                            if (vorlabel != "")
                                labeltemp = vorlabel + "." + labeltemp;
                            ObjectInput.RowDefinitions.Add(new RowDefinition());
                            Border bordertree = new Border();
                            bordertree.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;
                            TreeViewItem treeformular = new TreeViewItem();
                            treeformular.Header = labeltemp;
                            int tempcrows = crows;
                            for (int i = 0; i < count; i++)
                            {
                                TreeViewItem item = new TreeViewItem();
                                item.Header = pItem.Name;
                                //Border b = new Border();
                                //b.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;
                                Grid tempgrid = new Grid();
                                crows = -1;
                                object obj = pItem.GetValue(e, new object[] { i });
                                item.Header = labeltemp + "[" + i + "]";
                                GenerationInitial(obj, pancestor.PropertyType.ToString(), ancestor, pancestor, "", filter, tempgrid);
                                //b.Child = tempgrid;
                                item.Items.Add(tempgrid);
                                treeformular.Items.Add(item);
                            }
                            crows = tempcrows + 1;
                            bordertree.Child = treeformular;
                            Grid.SetRow(bordertree, crows);
                            Grid.SetColumn(bordertree, 0);
                            ObjectInput.Children.Add(bordertree);
                        }
                        else
                        {
                            if ((e == null) && (pancestor != null) && !(pancestor.PropertyType.IsPrimitive || pancestor.PropertyType.IsEnum || pancestor.GetType().IsValueType || pancestor.PropertyType.ToString().Equals("System.String")))
                            {   
                                //
                                pancestor.SetValue(ancestor, Activator.CreateInstance(pancestor.PropertyType), null);
                                e = pancestor.GetValue(ancestor, null);
                            }


                            if (!(e == null || e.GetType().GetProperty("Count") != null || e.GetType().IsArray || e.GetType().IsPrimitive || e.GetType().IsEnum || e.GetType().IsValueType || e.GetType().ToString().Equals("System.String")))
                            {

                                Type t = e.GetType();

                                PropertyInfo[] pi = t.GetProperties();
                                if (pancestor != null)
                                {
                                    ObjectInput.RowDefinitions.Add(new RowDefinition());
                                    Border bordertree = new Border();
                                    bordertree.Style = Application.Current.FindResource("DetailsBorderStyle") as Style;
                                    TreeViewItem treeformular = new TreeViewItem();
                                    treeformular.Header = pancestor.Name;
                                    treeformular.Name = "treeobj";
                                    CommandParam rp = new CommandParam();
                                    rp.ancestor = ancestor;
                                    rp.pancestor = pancestor;
                                    rp.tree = treeformular;
                                    rp.control = treeformular;
                                   
                                
                                    treeformular.Header = pancestor.Name;
                                   

                                    DockPanel dp1 = new DockPanel();


                                    if (!pancestor.PropertyType.IsArray)
                                    {
                                        ContextMenu textboxMenu = new ContextMenu();
                                        MenuItem excludeMenuItem = new MenuItem();
                                        excludeMenuItem.Header = "Exclude";
                                        excludeMenuItem.Command = new CommandRemove();
                                        excludeMenuItem.CommandParameter = rp;
                                        textboxMenu.Items.Add(excludeMenuItem);
                                        dp1.ContextMenu = textboxMenu;
                                        MenuItem includeMenuItem = new MenuItem();
                                        includeMenuItem.Header = "Include";
                                        includeMenuItem.Command = new CommandInclude();
                                        includeMenuItem.CommandParameter = rp;
                                        textboxMenu.Items.Add(includeMenuItem);
                                        dp1.ContextMenu = textboxMenu;
                                        dp1.ContextMenu = textboxMenu;
                                    }
                                    else
                                    {
                                        ContextMenu textboxMenu = new ContextMenu();
                                        MenuItem keineMenuItem = new MenuItem();
                                        keineMenuItem.Header = "Keine Aktion";
                                        textboxMenu.Items.Add(keineMenuItem);
                                        dp1.ContextMenu = textboxMenu;
                                    }
                                    
                                    
                                    
                                    Grid tempgrid = new Grid();
                                    rp.grid = tempgrid;
                                    rp.filter = filter;
                                    rp.tempcrow = crows;

                                    int tempcrows = crows;
                                    crows = -1;
                                    foreach (PropertyInfo p in pi)
                                    {
                                        GenerationInitial(p.GetValue(e, null), p.PropertyType.ToString(), e, p, vorlabel + pancestor.Name, filter, tempgrid);
                                    }


                                    treeformular.Items.Add(tempgrid);
                                 //   treeformular.IsExpanded = true;
                                    crows = tempcrows + 1;
                              
                                    DockPanel.SetDock(treeformular, Dock.Bottom);
                                    dp1.Children.Add(treeformular);
                                    bordertree.Child = dp1;
                                    Grid.SetRow(bordertree, crows);
                                    Grid.SetColumn(bordertree, 0);
                                    ObjectInput.Children.Add(bordertree);
                                }
                                else
                                    foreach (PropertyInfo p in pi)
                                    {

                                        if (getFilterAtt(p).Contains(filter)|| getFilterAtt(p)=="keine")
                                        {
                                            GenerationInitial(p.GetValue(e, null), p.PropertyType.ToString(), e, p, vorlabel, filter, ObjectInput);

                                        }
                                    }
                            }
                            else
                                if (pancestor != null)
                                {
                                    if (getFilterAtt(pancestor).Contains(filter) || getFilterAtt(pancestor)=="keine")
                                    {
                                        crows++;
                                        string labeltemp = getLabel(pancestor);                                       
                                        ObjectInput.RowDefinitions.Add(new RowDefinition());
                                        if (pancestor.PropertyType == typeof(Nullable<DateTime>) || pancestor.PropertyType == typeof(DateTime))
                                        {
                                            DatePicker datepicker = new DatePicker();
                                            Binding binding = new Binding();
                                            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                                            binding.Source = ancestor;
                                            binding.Path = new PropertyPath(pancestor);
                                            datepicker.SetBinding(DatePicker.SelectedDateProperty, binding);
                                            datepicker.MinWidth = 200;
                                            Grid tempgrid2 = createGrid(labeltemp, datepicker);
                                            Grid.SetRow(tempgrid2, crows);
                                            ObjectInput.Children.Add(tempgrid2);
                                        }
                                        else
                                        {
                                            if (pancestor.PropertyType.BaseType == typeof(Enum))
                                            {
                                                ComboBox text = new ComboBox();
                                                Binding binding = new Binding();
                                                binding.Mode = BindingMode.TwoWay;
                                                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                                                binding.Source = ancestor;
                                                binding.Path = new PropertyPath(pancestor);
                                                text.SetBinding(ComboBox.SelectedItemProperty, binding);
                                                text.ItemsSource = Enum.GetValues(pancestor.PropertyType);
                                                Grid tempgrid2 = createGrid(labeltemp, text);
                                                Grid.SetRow(tempgrid2, crows);
                                                ObjectInput.Children.Add(tempgrid2);
                                            }
                                            else
                                            {
                                                TextBox text = new TextBox();
                                                text.FontSize = 12;
                                                text.FontWeight = FontWeights.Bold;
                                                Binding binding = new Binding();
                                                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                                                binding.Source = ancestor;
                                                binding.Path = new PropertyPath(pancestor);
                                                text.SetBinding(TextBox.TextProperty, binding);
                                                Grid tempgrid2 = createGrid(labeltemp, text);
                                                Grid.SetRow(tempgrid2, crows);
                                                ObjectInput.Children.Add(tempgrid2);


                                                // COMMANDREMOVE
                                                string sname;
                                                if (ancestor != null && pancestor.PropertyType.Name != null)
                                                   sname = ancestor.GetType().Name.ToString();
                                                CommandParam rp = new CommandParam();
                                                rp.ancestor = ancestor;
                                                rp.pancestor = pancestor;
                                                rp.control = text;
                                                Grid tempgrid = new Grid();
                                                rp.grid = tempgrid;
                                               
                                                ContextMenu textboxMenu = new ContextMenu();
                                                MenuItem removeMenuItem = new MenuItem();
                                                removeMenuItem.Header = "Deaktivieren";
                                                removeMenuItem.Command = new CommandRemove();
                                                removeMenuItem.CommandParameter = rp;
                                                textboxMenu.Items.Add(removeMenuItem);
                                               
                                                MenuItem includeMenuItem = new MenuItem();
                                                includeMenuItem.Header = "Aktivieren";
                                                includeMenuItem.Command = new CommandInclude();
                                                includeMenuItem.CommandParameter = rp;
                                                textboxMenu.Items.Add(includeMenuItem);
                                                
                                                tempgrid2.ContextMenu = textboxMenu;
                                               
                                            
                                                


                                            }
                                        }

                                    }

                                }
                      }

        }


        /// <summary>
        /// Gets the custom attribute label or the name of this property
        /// </summary>
        /// <param name="pancestor"></param>
        /// <returns></returns>
        public string getLabel(PropertyInfo pancestor)
        {
            var atts = pancestor.GetCustomAttributes(false);
            string labeltemp = pancestor.Name;
            foreach (var att in atts)
                if (att is AttributeLabel)
                {
                    labeltemp = ((AttributeLabel)att).Label;
                    break;
                }
            return labeltemp;
        }

        /// <summary>
        /// Creates a Grid with one row and two columns
        /// Column 0 has a label, and column 1 has a control
        /// </summary>
        /// <param name="labeltemp"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        private Grid createGrid(string labeltemp, Control control)
        {
            Grid gridTextBox = new Grid();
            ColumnDefinition defcol0 = new ColumnDefinition();
            ColumnDefinition defcol1 = new ColumnDefinition();
            defcol0.Width = new GridLength(50, GridUnitType.Star);
            defcol1.Width = new GridLength(50, GridUnitType.Star);
            gridTextBox.ColumnDefinitions.Add(defcol0);
            gridTextBox.ColumnDefinitions.Add(defcol1);
            gridTextBox.RowDefinitions.Add(new RowDefinition());
            Label label = new Label();
            label.Content = labeltemp;
          //  label.AddHandler(Window.MouseRightButtonDownEvent, new MouseButtonEventHandler(label_MouseRightButtonDown), true);
          //  control.AddHandler(Window.MouseRightButtonDownEvent, new MouseButtonEventHandler(label_MouseRightButtonDown), true);
            Grid.SetRow(label, 0);
            Grid.SetColumn(label, 0);
            gridTextBox.Children.Add(label);
            Grid.SetRow(control, 0);
            Grid.SetColumn(control, 1);
            gridTextBox.Children.Add(control);
            return gridTextBox;
        }

        private void label_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Attribute wird ausgewählt!");
            Control s = sender as Control;
            if (s.GetType().GetProperty("Text") != null)
                 s.GetType().GetProperty("Text").SetValue(s, null, null);
            
          //  s.Items.Clear();
            if (s.IsEnabled) s.IsEnabled=false;
            
        }

        /// <summary>
        /// Gets the custom attribute Filter of this property or the string "keine" when the property hasn't the custom attribute Filter.
        /// </summary>
        /// <param name="pancestor"></param>
        /// <returns></returns>
        private string getFilterAtt(PropertyInfo pancestor)
        {
             var atts = pancestor.GetCustomAttributes(false);
             string filteratt = "keine";
             foreach (var att in atts)
             if (att is AttributeFilter)
                {
                    filteratt = ((AttributeFilter)att).Filter;
                    break;
                }
            return filteratt;
        }



}

}