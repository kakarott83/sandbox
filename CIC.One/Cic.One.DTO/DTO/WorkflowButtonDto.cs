using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Control Structure to configure a button displayed in the GUI from a workflow
    /// </summary>
    public class WorkflowButtonDto
    {
        /// <summary>
        /// Type of button where 
        ///    0 = command-button (UNIQUE Command will be checked)
        ///    1 = info-button, command not used, area and areaid for displaying info (UNIQUE area+areaid will be checked)
        ///    2 = like 1, but with info-overlay instead of new dashboard
        ///    3 = help item
        ///    4 = menu-button, supports multiple commands/texts
        ///    5 = segmented actionbar button with commands, segment=desc3
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// Disables the button for the user
        /// </summary>
        public int disabled {get;set;}
        /// <summary>
        /// Defines if the item will be removed upon resuming workflow
        /// </summary>
        public int persist { get; set; }
        /// <summary>
        /// displayed Button-Text
        /// </summary>
        public String text { get; set; }
        

        /// <summary>
        /// Additional Button Text
        /// </summary>
        public String desc1 { get; set; }
        public String desc2 { get; set; }
        public String desc3 { get; set; }

        /// <summary>
        /// used Button-Icon
        /// </summary>
        public String icon { get; set; }
        /// <summary>
        /// Command the button sends back in WorkflowContext.inputCommand when pressed by user
        /// </summary>
        public String command { get;set;}
        

        /// <summary>
        /// area e.g. VT, ANGEBOT
        /// </summary>
        public String area { get; set; }
        /// <summary>
        /// id of area 
        /// </summary>
        public String areaid { get; set; }

        public List<WorkflowButtonDto> items { get; set; }
        public int rang { get; set; }

        /// <summary>
        /// update command
        /// </summary>
        public String update { get; set; }
    }
}