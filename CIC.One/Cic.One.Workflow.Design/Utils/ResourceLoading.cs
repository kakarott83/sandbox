using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using Cic.One.Workflow.Icons;
using System.Windows.Media;

namespace Cic.One.Workflow.Design.Utils
{
    public class ResourceLoading
    {
        /// <summary>
        /// Loads an icon from the Folder of a different Library by using the class residing in that libraries folder
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DrawingBrush loadIcon(String name)
        {
            Stream manifestResourceStream = typeof(IconResourceAnchor).Module.Assembly.GetManifestResourceStream(typeof(IconResourceAnchor), name);
            var bmpframe = BitmapFrame.Create(manifestResourceStream);
            return  new DrawingBrush
            {
                Drawing = new ImageDrawing
                {
                    Rect = new System.Windows.Rect(0, 0, 16, 16),
                    ImageSource = bmpframe
                }
            };
        }
        public static ImageSource getIconSource(String name)
        {
            Stream manifestResourceStream = typeof(IconResourceAnchor).Module.Assembly.GetManifestResourceStream(typeof(IconResourceAnchor), name);
            return BitmapFrame.Create(manifestResourceStream);
            
        }
        
    }
}
