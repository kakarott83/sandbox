using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.TestUI.DTOS;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;


using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.TestUI.DataAccess
{
    /// <summary>
    /// Data Loader 
    /// Test UI
    /// </summary>
    public class DataLoader
    {

        

        /// <summary>
        /// Load DtoFilter
        /// Builds and returns a ObservableCollection of DTO from properties of object e
        /// Filter is a string with information about which properties of e will not be includes in this ObservableCollection
        /// DTO is a object with attributes : typeAttribute, nameAttribute, objectAttribute, valueAttribute
        /// </summary>
        /// <param name="e"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static ObservableCollection<DTO> LoadDTOFilter(Object e, string filter)
        {
            var list = new ObservableCollection<DTO>();
            string temptype = "";
            if (!(e == null || e.GetType().IsPrimitive || e.GetType().IsEnum || e.GetType().IsValueType || e.GetType().ToString().Equals("System.String")))
            {
                Type t = e.GetType();
                if (t.GetProperty("Count") == null && t.GetProperty("Item")==null && t.IsArray!=true)
                {
                    PropertyInfo[] pi = t.GetProperties();
                    foreach (PropertyInfo p in pi)
                    {
                        if (getFilterAtt(p).Contains(filter)|| getFilterAtt(p)=="keine")
                        {
                            if (p.GetValue(e, null) != null)
                            {
                                temptype = p.GetValue(e, null).GetType().ToString();
                                if (p.GetValue(e, null).GetType().IsPrimitive || p.GetValue(e, null).GetType().IsEnum || p.GetValue(e, null).GetType().IsValueType || p.GetValue(e, null).GetType().ToString().Equals("System.String"))
                                    list.Add(new DTO { nameAttribute = p.Name, typeAttribute = p.GetType().ToString(), valueAttribute = p.GetValue(e, null).ToString(), objectAttribute = p.GetValue(e, null) });

                            }
                        }
                    }
                }
                

            }
            else
            {
                if (e!=null) list.Add(new DTO { nameAttribute = "Value", valueAttribute = e.ToString() });
            }
            return list;
        }

        /// <summary>
        /// Builds and returns a ObservableCollection of DTO from each property of object e,
        /// DTO is a object with attributes : typeAttribute, nameAttribute, objectAttribute, valueAttribute
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static ObservableCollection<DTO> LoadDTO(Object e)
        {
            var list = new ObservableCollection<DTO>();
            string temptype = "";
            if (!(e == null || e.GetType().IsPrimitive || e.GetType().IsEnum || e.GetType().IsValueType || e.GetType().ToString().Equals("System.String")))
            {
                Type t = e.GetType();
                PropertyInfo[] pi = t.GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    if (p.GetValue(e, null) != null)
                    {
                        temptype = p.GetValue(e, null).GetType().ToString();
                        if (p.GetValue(e, null).GetType().IsPrimitive || p.GetValue(e, null).GetType().IsEnum || p.GetValue(e, null).GetType().IsValueType || p.GetValue(e, null).GetType().ToString().Equals("System.String"))
                            list.Add(new DTO { nameAttribute = p.Name, typeAttribute = p.GetType().ToString(), valueAttribute = p.GetValue(e, null).ToString(), objectAttribute = p.GetValue(e, null) });

                    }
                }

            }
            else
                list.Add(new DTO { nameAttribute = "Value", valueAttribute = e.ToString() });
            return list;
        }

      

        /// <summary>
        /// Load Tree
        /// Builds and returns a TreeViewItem from properties of object e.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static TreeViewItem LoadTree(Object e)
        {
            TreeViewItem item = new TreeViewItem();
            if (!(e == null || e.GetType().IsPrimitive || e.GetType().IsEnum || e.GetType().IsValueType || e.GetType().ToString().Equals("System.String")))
            {
                Type t = e.GetType();
                item.Header = t.Name.ToString();

                PropertyInfo[] pi = t.GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    TreeViewItem child = new TreeViewItem();
                    child.Items.Add(LoadTree(p.GetValue(e, null)));

                    child.Header = p.Name;
                    if (p.GetValue(e, null) != null)
                    {
                        if (p.GetValue(e, null).GetType().GetProperty("Count") != null)
                        {
                            PropertyInfo pCount = p.GetValue(e, null).GetType().GetProperty("Count");
                            int count = (int)pCount.GetValue(p.GetValue(e, null), null);
                            PropertyInfo pItem = p.GetValue(e, null).GetType().GetProperty("Item");


                            for (int i = 0; i < count; i++)
                            {

                                object obj = pItem.GetValue(p.GetValue(e, null), new object[] { i });
                                child.Tag = new DTO { nameAttribute = p.Name + "[" + i + "]", typeAttribute = obj.GetType().ToString(), valueAttribute = obj.ToString(), objectAttribute = obj };
                                child.Items.Add(LoadTree(obj));
                            }

                        }
                        else
                        {
                            child.Tag = new DTO { nameAttribute = p.Name, typeAttribute = p.GetType().ToString(), valueAttribute = p.GetValue(e, null).ToString(), objectAttribute = p.GetValue(e, null) };
                            child.Items.Add(LoadTree(p.GetValue(e, null)));
                        }
                    }

                    item.Items.Add(child);



                }

            }


            return item;


        }

       
            /// <summary>
            /// 
            /// LoadTree builds and returns ein TreeViewItem from properties of object e.
            /// Elements of Arrays and Lists are items of this tree.
            /// </summary>
            /// <param name="e"></param>
            /// <returns></returns>
            public static TreeViewItem LoadTree2(Object e)
            {
            TreeViewItem item = null;
            Type t = e.GetType();
            if (!(e == null || e.GetType().IsPrimitive || e.GetType().IsEnum || e.GetType().IsValueType || e.GetType().ToString().Equals("System.String")))
            {

                

                
                
                if (e.GetType().IsArray)
                {
                    
                    item = new TreeViewItem();
                    item.Header = t.Name.ToString();
                    



                    IList array = e as IList;
                    var iarray = 0;

                    foreach (var arrElement in array)
                    {
                        item.Items.Add(LoadTree2(arrElement));
                        iarray++;
                    }
                    
                }
                else
                    if (e.GetType().GetProperty("Count") != null)
                    {
                      
                        item= new TreeViewItem();
                        item.Header = t.Name.ToString();
                        
                        PropertyInfo pCount = e.GetType().GetProperty("Count");
                        int count = (int)pCount.GetValue(e, null);
                        PropertyInfo pItem = e.GetType().GetProperty("Item");


                        for (int i = 0; i < count; i++)
                        {

                            object obj = pItem.GetValue(e, new object[] { i });
                            item.Items.Add(LoadTree2(obj));
                        }
                       
                    }

                else
                {
                    item = new TreeViewItem();
                    item.Header = t.Name.ToString();
                    item.Tag = e;
                    PropertyInfo[] pi = t.GetProperties();
                    foreach (PropertyInfo p in pi)
                    {
                        if (!(p.GetValue(e, null) == null || p.GetValue(e, null).GetType().IsPrimitive || p.GetValue(e, null).GetType().IsEnum || p.GetValue(e, null).GetType().IsValueType || p.GetValue(e, null).GetType().ToString().Equals("System.String")))
                        {
                            if (p.GetValue(e, null).GetType().GetProperty("Count") != null)
                            {
                                TreeViewItem itemList = null;
                                itemList = new TreeViewItem();
                                itemList.Header = p.Name.ToString();
                                itemList.Tag = p.GetValue(e, null);
                                PropertyInfo pCount = p.GetValue(e, null).GetType().GetProperty("Count");
                                int count = (int)pCount.GetValue(p.GetValue(e, null), null);
                                PropertyInfo pItem = p.GetValue(e, null).GetType().GetProperty("Item");


                                for (int i = 0; i < count; i++)
                                {

                                    object obj = pItem.GetValue(p.GetValue(e, null), new object[] { i });
                                    itemList.Items.Add(LoadTree2(obj));
                                }
                                item.Items.Add(itemList);
                            }
                            else
                                if (p.GetValue(e, null).GetType().IsArray)
                                {
                                    TreeViewItem itemArray = null;
                                    itemArray = new TreeViewItem();
                                    itemArray.Header = p.Name.ToString();
                                    itemArray.Tag = p.GetValue(e, null);



                                    IList array = p.GetValue(e, null) as IList;
                                    var iarray = 0;

                                    foreach (var arrElement in array)
                                    {
                                        itemArray.Items.Add(LoadTree2(arrElement));
                                        iarray++;
                                    }
                                    item.Items.Add(itemArray);
                                }
                                else
                                    item.Items.Add(LoadTree2(p.GetValue(e, null)));
                        }
                    }

                }
                    
                }
        
                return item;
            
        }

            private static string getFilterAtt(PropertyInfo pancestor)
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

