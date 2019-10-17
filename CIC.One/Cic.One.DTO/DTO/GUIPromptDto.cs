using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Structure for displaying a message box in the gui
    /// </summary>
    public class GUIPromptDto
    {
        public String title { get; set; }//Title of Popup
        public String icon { get; set; }//icon of popup
        public String text { get; set; }//displayed message/question
        public String defaultValue { get; set; } 
        public String value { get; set; } //value entered by user
        public int isInput { get; set; }//when set -> leads to OK-Button else leads to YES/NO-Button

    }
}