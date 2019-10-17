
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation.Toolbox;
using System.Windows;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Statements;
using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;

namespace Workflows.BO
{

    /// <summary>
    /// Source for the Toolbox, loads all types on same namespace as given in the tag from AllSiblingsOf
    /// </summary>

    [ContentProperty("Sources")]
    public class ToolboxItemSource : DependencyObject
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static ResourceDictionary resources = new ResourceDictionary { Source = new Uri("pack://application:,,,/System.Activities.Presentation;component/themes/icons.xaml") };
        
        public static ToolboxItemSource GetCategorySource(DependencyObject obj)
        {
            return (ToolboxItemSource)obj.GetValue(CategorySourceProperty);
        }
        public static void SetCategorySource(DependencyObject obj, ToolboxCategory value)
        {
            obj.SetValue(CategorySourceProperty, value);
        }
        public static readonly DependencyProperty CategorySourceProperty =
            DependencyProperty.RegisterAttached("CategorySource", typeof(ToolboxItemSource), typeof(ToolboxItemSource), new UIPropertyMetadata(null, new PropertyChangedCallback(OnCategorySourceChanged)));

        protected static void OnCategorySourceChanged(DependencyObject depobj, DependencyPropertyChangedEventArgs dpcea)
        {
            if (null != dpcea.NewValue && dpcea.NewValue is ToolboxItemSource)
            {
                var tbsrc = dpcea.NewValue as ToolboxItemSource;
                foreach (var source in tbsrc.Sources)
                {
                    AddTools(depobj as ToolboxControl, source);
                }
            }
        }

        private static void AddTools(ToolboxControl toolboxControl, ToolboxSource source)
        {

            if (null != source.AllSiblingsOf)
            {
                var cat = toolboxControl.Categories.Where(q => q.CategoryName.Equals(source.TargetCategory)).FirstOrDefault();
                if (null == cat)
                {
                    cat = new ToolboxCategory(source.TargetCategory);
                    toolboxControl.Categories.Add(cat);
                }


             
                var query = from type in source.AllSiblingsOf.Assembly.GetTypes()
                            where type.IsPublic &&
                            !type.IsNested &&
                            !type.IsAbstract &&
                            (typeof(Activity).IsAssignableFrom(type) ||
                            typeof(IActivityTemplateFactory).IsAssignableFrom(type))
                            orderby type.Name
                            select type;

                var builder = new AttributeTableBuilder();
                Dictionary<Type,bool> usage = new Dictionary<Type,bool>();

                foreach (var item in query)
                {
                    usage[item] = AddIconAttributes(item, builder);
                }
                MetadataStore.AddAttributeTable(builder.CreateTable());
                foreach (var item in query)
                {
                    var wrap = new ToolboxItemWrapper(item);
                    try
                    {
                        if(!usage[item])
                            cat.Add(wrap);
                    }catch(Exception e)
                    {
                        _log.Error("Error adding Toolbox items", e);
                    }
                }
            }
        }

        protected static bool AddIconAttributes(Type type, AttributeTableBuilder builder)
        {
            var secondary = false;
            var tbaType = typeof(ToolboxBitmapAttribute);
            var imageType = typeof(System.Drawing.Image);
            var constructor = tbaType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { imageType, imageType }, null);
            string resourceKey = type.IsGenericType ? type.GetGenericTypeDefinition().Name : type.Name;
            int index = resourceKey.IndexOf('`');
            if (index > 0)
            {
                resourceKey = resourceKey.Remove(index);
            }
            if (resourceKey == "Flowchart")
            {
                resourceKey = "FlowChart"; // it appears that themes/icons.xaml has a typo here
            }
            resourceKey += "Icon";
            Bitmap small, large;
            object resource = resources[resourceKey];
            if (!(resource is DrawingBrush))
            {
                object[] atts = type.GetCustomAttributes(false);
                //For our custom activity icons in the toolbox via the toolbox attribute
                if (atts != null && atts.Length>0)
                {
                    if(atts[0].GetType()==typeof(System.Drawing.ToolboxBitmapAttribute))
                    {
                        builder.AddCustomAttributes(type, (System.Drawing.ToolboxBitmapAttribute)atts[0]);
                        return false;
                    }
                }
                
                resource = resources["GenericLeafActivityIcon"];
                secondary = true;
            }
            var dv = new DrawingVisual();
            using (var context = dv.RenderOpen())
            {
                context.DrawRectangle(((DrawingBrush)resource), null, new Rect(0, 0, 32, 32));
                context.DrawRectangle(((DrawingBrush)resource), null, new Rect(32, 32, 16, 16));
            }
            var rtb = new RenderTargetBitmap(32, 32, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(rtb));
                enc.Save(outStream);
                outStream.Position = 0;
                large = new Bitmap(outStream);
            }
            rtb = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
            dv.Offset = new Vector(-32, -32);
            rtb.Render(dv);
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(rtb));
                enc.Save(outStream);
                outStream.Position = 0;
                small = new Bitmap(outStream);
            }

            var tba = constructor.Invoke(new object[] { small, large }) as ToolboxBitmapAttribute;
            builder.AddCustomAttributes(type, tba);
            return secondary;
        }

        public ToolboxSourceCollection Sources
        {
            get { return (ToolboxSourceCollection)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Sources", typeof(ToolboxSourceCollection),
                                        typeof(ToolboxItemSource)
                                        , new UIPropertyMetadata(new ToolboxSourceCollection()));


    }
}
