using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Activities;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.One.Web.DAO;
using Cic.One.Workflow.BO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;


namespace Cic.One.Web.BO
{
    /// <summary>
    /// Business-Object for Workflow-Handling
    /// </summary>
    public class WorkflowBo : IWorkflowBo
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IWFVDao dao;

        public WorkflowBo(IWFVDao dao)
        {
            this.dao = dao;
        }

       

        /// <summary>
        /// returns a VlmTable-Definition for the given OL-Area
        /// </summary>
        /// <returns></returns>
        public VlmTableDto getVlmTable(long sysvlm, OlArea area)
        {
            List<VlmConfDto> vlms =  dao.getVlmList();
            VlmConfDto vlm = (from a in vlms
                              where a.sysvlmconf == sysvlm
                              select a).FirstOrDefault();
            if (vlm == null||vlm.tables==null||vlm.tables.Count==0) return null;
            return (from v in vlm.tables
                    where v.syscode == area.ToString()
                    select v).FirstOrDefault();
        }

        /// <summary>
        /// returns a list of all webvlms
        /// </summary>
        /// <returns></returns>
        public List<VlmConfDto> getVlmList()
        {
            return dao.getVlmList();
        }

        /// <summary>
        /// Synchronizes the table of available webvlm commandlines(templates)
        /// </summary>
        /// <param name="commandLines"></param>
        public void synchronizeViewConfig(List<ViewConfigDto> commandLines)
        {
            dao.synchronizeViewConfig(commandLines);
        }

        /// <summary>
        /// delivers vlm config
        /// </summary>
        /// <param name="vlmid"></param>
        /// <returns></returns>
        public List<WfvEntry> getVlmConfig(String vlmid)
        {
            return dao.getVlmConfig(vlmid);
            
        }

        /// <summary>
        /// Returns the menu configuration
        /// </summary>
        /// <param name="vlmid"></param>
        /// <returns></returns>
        public List<VlmMenuDto> getMenus(String vlmid)
        {
               return dao.getMenus(vlmid);
            
        }

        /// <summary>
        /// Processes a workflow until
        ///  - finished
        ///  - or until bookmark reached (by UserIntarction-Activity)
        ///  
        /// Uses the given input context and returns the modified context
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public oprocessWorkflowDto processWorkflow(iprocessWorkflowDto input, oprocessWorkflowDto rval, Cic.One.DTO.IWFVDao dao, Cic.OpenOne.Common.BO.ICASBo casBo)
        {

            IWorkflowService ws = WorkflowServiceFactory.getInstance().getWorkflowService(input.type);
            
            return ws.processWorkflow(input, rval, dao,Cic.One.Web.BO.BOFactoryFactory.getInstance().getCASBo());
        }
    }
}