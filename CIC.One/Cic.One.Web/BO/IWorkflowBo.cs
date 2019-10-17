using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;
using Cic.One.Web.DAO;


namespace Cic.One.Web.BO
{
    public interface IWorkflowBo
    {
        /// <summary>
        /// returns a VlmTable-Definition for the given OL-Area
        /// </summary>
        /// <returns></returns>
        VlmTableDto getVlmTable(long sysvlm, OlArea area);

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
        /// Processes a workflow until
        ///  - finished
        ///  - or until bookmark reached (by UserIntarction-Activity)
        ///  
        /// Uses the given input context and returns the modified context
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        oprocessWorkflowDto processWorkflow(iprocessWorkflowDto input, oprocessWorkflowDto rval, Cic.One.DTO.IWFVDao wfdao, Cic.OpenOne.Common.BO.ICASBo casBo);

         /// <summary>
        /// delivers vlm config
        /// </summary>
        /// <param name="vlmid"></param>
        /// <returns></returns>
        List<WfvEntry> getVlmConfig(String vlmid);

         /// <summary>
        /// Returns the menu configuration
        /// </summary>
        /// <param name="vlmid"></param>
        /// <returns></returns>
        List<VlmMenuDto> getMenus(String vlmid);
    }
}