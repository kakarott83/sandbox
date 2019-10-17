using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;


namespace Cic.One.Web.DAO
{
    public interface IWorkflowDao
    {
         /// <summary>
        /// returns a list of all webvlms
        /// </summary>
        /// <returns></returns>
        List<VlmConfDto> getVlmList();

        /// <summary>
        /// Synchronizes the table of available webvlm commandlines(templates)
        /// </summary>
        /// <param name="commandLines"></param>
        void synchronizeViewConfig(List<ViewConfigDto> commandLines);

        /// <summary>
        /// delivers vlm config
        /// </summary>
        /// <param name="vlmid"></param>
        /// <returns></returns>
        List<WfvEntry> getVlmConfig(String vlmid);

        /// <summary>
        /// Load all Menus and Toolbars for the vlm
        /// </summary>
        /// <param name="vlmid"></param>
        /// <returns></returns>
        List<VlmMenuDto> getMenus(String vlmid);

        /// <summary>
        /// fetches the wfv configuration for the wfvEntry syscode from wfvconfig.dll
        /// also allows overriding the wfventry-settings from inside the dashboard, when wfvid is of structure dashboardsyscode:wfvsyscode
        /// </summary>
        /// <param name="wfvid"></param>
        /// <returns></returns>
        WfvEntry getWfvEntry(String wfvid);
    }
}